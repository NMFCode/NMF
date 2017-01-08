/* ****************************************************************************
 *
 * Copyright (c) Microsoft Corporation. 
 *
 * This source code is subject to terms and conditions of the Microsoft Public
 * License. A  copy of the license can be found in the License.html file at the
 * root of this distribution. If  you cannot locate the  Microsoft Public
 * License, please send an email to  dlr@microsoft.com. By using this source
 * code in any fashion, you are agreeing to be bound by the terms of the 
 * Microsoft Public License.
 *
 * You must not remove this notice, or any other, from this software.
 *
 * ***************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;


using System.CodeDom;
using System.CodeDom.Compiler;

namespace PythonCodeGenerator.CodeDom {
    /* CodeGen Notes: 
     * 
     * return types are set using @returns(type) decorator syntax
     * argument types are set using @accepts(type) decorator syntax         
     * 
     * Classes are defined as:
     * 
     * class foo(object):
     *      """type(x) == int, type(y) == System.EventArgs, type(z) == bar"""     
     * 
     * @returns(str)
     * def bar():
     *     return "abc" 
     *      
     * @returns(str)
     * def baz():
     *     x = baz()
     *     return x
     * 
     * 
     */

    partial class PythonGenerator : CodeGenerator, ICodeGenerator {
        CodeEntryPointMethod entryPoint = null;
        string entryPointNamespace = null;
        string lastNamespace;        
        Stack<TypeDeclInfo> typeStack = new Stack<TypeDeclInfo>();
        Stack<CodeNamespace> namespaceStack = new Stack<CodeNamespace>();
        int col, row;               
        int lastIndent;
        internal const string ctorFieldInit = "_ConstructorFieldInitFunction";        
        List<string> processedTypes = new List<string>();
        List<string> topLevelTypes = new List<string>();
        int currentTypeLevel = 0; //0 = top level types, 1 = children types of top level types

        class TypeDeclInfo {
            public TypeDeclInfo(CodeTypeDeclaration decl) {
                Declaration = decl;
            }

            public CodeTypeDeclaration Declaration;            
        }

        static string[] keywords = new string[] { "and", "assert", "break", "class", "continue", "def", "del", "elif", "else", "except", "exec", "finally", "for", "from", "global", "if", "import", "in", "is", "lambda", "not", "or", "pass", "print", "raise", "return", "try", "while", "yield" };

        protected override void GenerateCompileUnit(CodeCompileUnit e) {
#if DEBUG
            try {
#endif
                if (Options != null) {
                    Options.BlankLinesBetweenMembers = false;
                }                                              

                CodeUnitPreProcessor.removeNetSystemImports(ref e);                    
                e = CodeUnitPreProcessor.replaceNetVariableTypesWithNativeVaribaleTypes(e);              
                    
                //base.GenerateCompileUnit(e);
                //the following three lines are the official call of the above line
                //it was replaced for debugging purposes
                base.GenerateCompileUnitStart(e);
                WriteWarning();
                GenerateNamespaces(e);
                base.GenerateCompileUnitEnd(e);                                    
                
#if DEBUG
            } catch (Exception ex) {
                Console.WriteLine(ex.StackTrace);
                Debug.Assert(false, String.Format("Unexpected exception: {0}", ex.Message), ex.StackTrace);
            }
#endif
        }

        /// <summary>
        /// Check if there are any types in the CodeDom tree that we don't currently have
        /// imports for.  If we find any then add them into the imports list.
        /// </summary>
        private void AddNewImports(CodeCompileUnit ccu) {            
            foreach (CodeNamespace cn in ccu.Namespaces) {
                CodeNamespaceImportCollection curImports = cn.Imports;

                foreach (CodeTypeDeclaration ctd in cn.Types) {
                    AddImportsForTypeDeclaration(curImports, ctd);
                }
            }
        }

        private void AddImportsForTypeDeclaration(CodeNamespaceImportCollection curImports, CodeTypeDeclaration ctd) {
            AddImportsForCodeTypeReferenceCollection(curImports, ctd.BaseTypes);

            foreach (CodeTypeMember member in ctd.Members) {
                CodeMemberProperty prop;
                CodeMemberMethod meth;
                CodeMemberField field;
                CodeMemberEvent evnt;
                CodeTypeDeclaration innerType;

                if ((prop = member as CodeMemberProperty) != null) {
                    AddImportsForProperty(curImports, prop);
                } else if ((evnt = member as CodeMemberEvent) != null) {
                    AddImportsForEvent(curImports, evnt);
                } else if ((field = member as CodeMemberField) != null) {
                    AddImportsForField(curImports, field);
                } else if ((meth = member as CodeMemberMethod) != null) {
                    AddImportsForMethod(curImports, meth);
                } else if ((innerType = member as CodeTypeDeclaration) != null) {
                    AddImportsForTypeDeclaration(curImports, innerType);
                }
            }
        }

        private void AddImportsForProperty(CodeNamespaceImportCollection imports, CodeMemberProperty prop) {
            MaybeAddImport(imports, prop.Type);
        }

        private void AddImportsForEvent(CodeNamespaceImportCollection imports, CodeMemberEvent evnt) {
            MaybeAddImport(imports, evnt.Type);
        }
        
        private void AddImportsForField(CodeNamespaceImportCollection imports, CodeMemberField field) {
            MaybeAddImport(imports, field.Type);
        }
        
        private void AddImportsForMethod(CodeNamespaceImportCollection imports, CodeMemberMethod method) {
            MaybeAddImport(imports, method.ReturnType);
        }

        private void AddImportsForCodeTypeReferenceCollection(CodeNamespaceImportCollection curImports, CodeTypeReferenceCollection ctrc) {
            foreach (CodeTypeReference ctr in ctrc) {
                MaybeAddImport(curImports, ctr);
            }
        }

        private void MaybeAddImport(CodeNamespaceImportCollection curImports, CodeTypeReference reference) {
            string typeName = reference.BaseType;

            if (reference.BaseType == "System.Object" ||
                reference.BaseType == "System.String" ||
                reference.BaseType == "System.Int32" ||
                reference.BaseType == "System.Double" ||
                reference.BaseType == "System.Void")
                return;

            
            int firstDot;
            if((firstDot = typeName.IndexOf('.')) == -1){
                return;
            }

            string typeNs = typeName.Substring(0, firstDot);

            foreach (CodeNamespaceImport cni in curImports) {
                if (cni.Namespace.StartsWith(typeNs))
                    return;
            }

            curImports.Add(new CodeNamespaceImport(typeName.Substring(0, typeName.LastIndexOf('.'))));
        }

        internal void InternalGenerateCompileUnit(CodeCompileUnit ccu) {
            ((ICodeGenerator)this).GenerateCodeFromCompileUnit(ccu, new System.IO.StringWriter(), null);
        }

        protected override void GenerateNamespace(CodeNamespace e) {
            if (Options != null) {
                Options.BlankLinesBetweenMembers = false;
            }

            GenerateCommentStatements(e.Comments);
            GenerateNamespaceStart(e);

            GenerateNamespaceImports(e);
            WriteLine("");

            GenerateTypes(e);
            GenerateNamespaceEnd(e);
        }
        #region CodeGenerator abstract overrides
        protected override string CreateEscapedIdentifier(string value) {
            // Python has no identifier escaping...
            return CreateValidIdentifier(value);
        }

        protected override string CreateValidIdentifier(string value) {
            if (IsValidIdentifier(value)) return value;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < value.Length; i++) {
                // mangle invalid identifier characters to _hexVal
                if ((value[i] >= 'a' && value[i] <= 'z') ||
                    (value[i] >= 'A' && value[i] <= 'Z') ||
                    (i != 0 && value[i] >= '0' && value[i] <= '9') ||
                    value[i] == '_') {
                    sb.Append(value[i]);
                    continue;
                }
                sb.AppendFormat("_{0:X}", (int)value[i]);
            }

            value = sb.ToString();
            if (!IsValidIdentifier(value)) {
                // keyword
                sb.Append("_");
                return sb.ToString();
            }

            return value;
        }

        protected override void GenerateArgumentReferenceExpression(CodeArgumentReferenceExpression e) {
            Write(e.ParameterName);
        }

        protected override void GenerateArrayCreateExpression(CodeArrayCreateExpression e) {
            CodeTypeReference elementType = e.CreateType.ArrayElementType;
            if (elementType == null) {
                // This is necessary to support clients which incorrectly pass a non-array
                // type.  CSharpCodeProvider has similar logic.
                elementType = e.CreateType;
            }

            if (e.Initializers.Count > 0) {
                Write("System.Array[");
                OutputType(elementType);
                Write("]");

                Write("((");
                for (int i = 0; i < e.Initializers.Count; i++) {
                    GenerateExpression(e.Initializers[i]);
                    Write(", "); // we can always end Tuple w/ an extra , and need to if the count == 1
                }
                Write("))");
            } else {
                Write("System.Array.CreateInstance(");
                OutputType(elementType);
                Write(",");

                if (e.SizeExpression != null) {
                    GenerateExpression(e.SizeExpression);
                } else {
                    Write(e.Size);
                }
                Write(")");
            }
        }

        protected override void GenerateArrayIndexerExpression(CodeArrayIndexerExpression e) {
            GenerateExpression(e.TargetObject);
            Write("[");
            string comma = "";
            for (int i = 0; i < e.Indices.Count; i++) {
                Write(comma);
                GenerateExpression(e.Indices[i]);
                comma = ", ";
            }
            Write("]");
        }

        protected override void GenerateAssignStatement(CodeAssignStatement e) {            
            GenerateExpression(e.Left);
            Write(" = ");
            GenerateExpression(e.Right);
            WriteLine();
        }

        protected override void GenerateAttachEventStatement(CodeAttachEventStatement e) {
            GenerateEventReferenceExpression(e.Event);

            Write(" += ");

            CodeObjectCreateExpression coce = e.Listener as CodeObjectCreateExpression;
            if (coce != null && coce.Parameters.Count == 1) {
                // += new Foo(methodname)
                // we want to transform it to:
                // += methodname
                GenerateExpression(coce.Parameters[0]);
            } else {
                GenerateExpression(e.Listener);
            }
            WriteLine();            
        }

        protected override void GenerateBaseReferenceExpression(CodeBaseReferenceExpression e) {
            //the last appended class in processedTypes is the current one we're generating
            //use it instead of type(self)
            Write("super(" + processedTypes[processedTypes.Count - 1] + ", self)");
        }

        protected override void GenerateCastExpression(CodeCastExpression e) {
            GenerateExpression(e.Expression);
        }

        protected override void GenerateComment(CodeComment e) {

            //Console.WriteLine(e.Text.Replace("\r", "\\r").Replace("\n", "\\n"));
            e.Text = e.Text.Replace("<summary>\r\n", "");
            e.Text = e.Text.Replace("\r\n </summary>", "");
            e.Text = e.Text.Replace("<summary>", "");
            e.Text = e.Text.Replace("</summary>", "");            
            string[] lines = e.Text.Replace("\r", "").Split('\n');
            foreach (string line in lines) {
                Write("# ");
                WriteLine(line);
            }
        }

        protected override void GenerateConditionStatement(CodeConditionStatement e) {
            Write("if ");
            GenerateExpression(e.Condition);
            if (e.TrueStatements.Count != 0) {
                WriteLine(":"); //!!! Consult UserData["NoNewLine"]

                Indent++;
                GenerateStatements(e.TrueStatements);
                Indent--;
            } else {
                WriteLine(": pass"); //!!! Consult UserData["NoNewLine"]
            }
            if (e.FalseStatements != null && e.FalseStatements.Count > 0) {
                WriteLine("else:"); //!!! Consult UserData["NoNewLine"]
                Indent++;
                GenerateStatements(e.FalseStatements);
                Indent--;
            }            
        }

        private bool NeedFieldInit() {
            bool needsInit = false;
            //if there is at least one field or event we will need to initialize it                                      
            for (int i = 0; i < CurrentClass.Members.Count; i++) {                
                if (CurrentClass.Members[i] is CodeMemberField || CurrentClass.Members[i] is CodeMemberEvent) {
                    needsInit = true;
                    break;
                }
            }
            return needsInit;
        }

        protected override void GenerateConstructor(CodeConstructor e, CodeTypeDeclaration c) {

            Write("def __init__(self");
            if (e.Parameters.Count > 0) {
                Write(", ");
                OutputParameters(e.Parameters);
            }
            WriteLine("):"); //!!! Consult UserData["NoNewLine"]
            Indent++;            
            if (NeedFieldInit()) {
                // if there already is an call to the constructorFieldInit in the statements, we don't need to generate a call
                bool needsCall = true;
                //only generate a call if really need it and there isn't already one
                for (int i = 0; i < e.Statements.Count; i++) {
                    CodeExpressionStatement ces = e.Statements[i] as CodeExpressionStatement;
                    if (ces != null) {
                        CodeMethodInvokeExpression cmie = ces.Expression as CodeMethodInvokeExpression;                        
                        if (cmie != null) {                           
                            if (cmie.Method.TargetObject is CodeThisReferenceExpression && ("_" + cmie.Method.MethodName) == ctorFieldInit) {
                                needsCall = false;
                                break;
                            }
                        }
                    }
                }
                //generate only if there isn't already one
                if (needsCall) {
                    WriteLine("self." + ctorFieldInit + "()");
                } else if (e.Statements.Count == 0) {
                    Write("pass");
                }
            } else if (e.Statements.Count == 0) {
                Write("pass");
            }

            GenerateStatements(e.Statements);

            Indent--;
           
            WriteLine();
        }

        protected override void GenerateDelegateCreateExpression(CodeDelegateCreateExpression e) {
            if (e.TargetObject != null) {
                GenerateExpression(e.TargetObject);
                Write(".");
            }
            if (e.TargetObject is CodeThisReferenceExpression) WritePrivatePrefix(e.MethodName);
            Write(e.MethodName);
        }

        protected override void GenerateDelegateInvokeExpression(CodeDelegateInvokeExpression e) {
            GenerateExpression(e.TargetObject);

            string comma = "";
            foreach (CodeExpression ce in e.Parameters) {
                Write(comma);
                GenerateExpression(ce);
                comma = ", ";
            }
        }

        protected override void GenerateEntryPointMethod(CodeEntryPointMethod e, CodeTypeDeclaration c) {
            entryPointNamespace = lastNamespace;
            entryPoint = e;
        }

        protected override void GenerateEvent(CodeMemberEvent e, CodeTypeDeclaration c) {
            //throw new NotImplementedException("The method or operation is not implemented.");
        }

        protected override void GenerateEventReferenceExpression(CodeEventReferenceExpression e) {
            if (e.TargetObject != null) {
                GenerateExpression(e.TargetObject);
                if (!String.IsNullOrEmpty(e.EventName)) Write(".");
            }

            if (!String.IsNullOrEmpty(e.EventName)) {
                if (e.TargetObject is CodeThisReferenceExpression) WritePrivatePrefix(e.EventName);
                Write(e.EventName);
            }
        }

        private void WritePrivatePrefix(string name) {
            for (int i = 0; i < CurrentClass.Members.Count; i++) {
                if (CurrentClass.Members[i].Name == name) {
                    if ((CurrentClass.Members[i].Attributes & MemberAttributes.AccessMask) == MemberAttributes.Private &&
                        (CurrentClass.Members[i].Attributes & MemberAttributes.ScopeMask) != MemberAttributes.Static)
                        Write("_");
                    break;
                }
            }
        }

        protected override void GenerateExpressionStatement(CodeExpressionStatement e) {
            if (e.Expression is CodeMethodInvokeExpression) {
                GenerateMethodInvokeExpression(e.Expression as CodeMethodInvokeExpression);
                WriteLine();
            } else {
                GenerateExpression(e.Expression);
            }
        }

        protected override void GenerateField(CodeMemberField e) {
            if (e.Type.TypeArguments.Count <= 0)
            {
                return;
            }
            Write("typeArgsOf");
            Write(e.Name.Replace("_", "").ToUpper());
            Write(" = [");
            for(int i = 0; i < e.Type.TypeArguments.Count; i++)
            {                
                Write(e.Type.TypeArguments[i].BaseType);             
                if (i != e.Type.TypeArguments.Count - 1) //if is not last iteration add comma
                {
                    Write(", ");
                }
            }
            Write("]");
            WriteLine("");
            // init expressions are generated in ctorFieldInit (const string)
            // and calls in the constructors are generated to the function.
        }

        protected override void GenerateFieldReferenceExpression(CodeFieldReferenceExpression e) {
            if (e.TargetObject != null) {
                GenerateExpression(e.TargetObject);
                Write(".");
            }

            if (e.TargetObject is CodeThisReferenceExpression) WritePrivatePrefix(e.FieldName);
            else if (e.TargetObject is CodeTypeReferenceExpression) WritePrivatePrefix(e.FieldName);

            Write(e.FieldName);
        }

        protected override void GenerateIndexerExpression(CodeIndexerExpression e) {
            GenerateExpression(e.TargetObject);
            Write("[");
            string comma = "";
            for (int i = 0; i < e.Indices.Count; i++) {
                Write(comma);
                GenerateExpression(e.Indices[i]);
                comma = ", ";
            }
            Write("]");
        }

        protected override void GenerateIterationStatement(CodeIterationStatement e) {
            if (e.InitStatement != null)
                GenerateStatement(e.InitStatement);
            Write("while ");
            GenerateExpression(e.TestExpression);
            WriteLine(":"); //!!! Consult UserData["NoNewLine"]
            Indent++;
            if (e.Statements.Count == 0) {
                if (e.IncrementStatement == null)
                    WriteLine("pass");
            } else
                GenerateStatements(e.Statements);
            if (e.IncrementStatement != null)
                GenerateStatement(e.IncrementStatement);
            Indent--;            
        }


        protected override void GenerateMethod(CodeMemberMethod e, CodeTypeDeclaration c) {
            if ((e.Attributes & MemberAttributes.ScopeMask) == MemberAttributes.Static)
                WriteLine("@staticmethod");

            string thisName = null;
            if ((e.Attributes & MemberAttributes.ScopeMask) != MemberAttributes.Static)
                thisName = UserDataString(e.UserData, "ThisArg", "self");

            string name = e.Name;
            if ((e.Attributes & MemberAttributes.AccessMask) == MemberAttributes.Private) name = "_" + e.Name;

            GenerateMethodWorker(thisName,
                UserDataString(e.UserData, "ThisType", null),
                name,
                e.Parameters,
                e.Statements,
                e.ReturnType,
                e.UserData);            
        }

        protected override void GeneratePrimitiveExpression(CodePrimitiveExpression e) {
            if (e.Value is char) {
                char chVal = (char)e.Value;
                if (chVal > 0xFF || chVal < 32) {
                    Write("System.Convert.ToChar(");
                    Write(((int)chVal).ToString());
                    Write(")");
                } else if (chVal == '\'') {
                    Write("'\\''");
                } else if (chVal == '\\') {
                    Write("'\\\\'");
                } else {
                    Write("System.Convert.ToChar('");
                    Write(chVal.ToString());
                    Write("')");
                }
                return;
            }

            string strVal = e.Value as string;
            if (strVal != null) {
                for (int i = 0; i < strVal.Length; i++) {
                    if (strVal[i] > 0xFF) {
                        // possibly un-encodable unicode characters,
                        // write unicode characters specially...
                        Write("u'");
                        for (i = 0; i < strVal.Length; i++) {
                            if (strVal[i] > 0xFF) {
                                Write(String.Format("\\u{0:X}", (int)strVal[i]));
                            } else if (strVal[i] < 32) {
                                Write(String.Format("\\x{0:X}", (int)strVal[i]));
                            } else if (strVal[i] == '\'') {
                                Write("\\'");
                            } else if (strVal[i] == '\\') {
                                Write("\\");
                            } else {
                                Write(strVal[i]);
                            }
                        }
                        Write("'");
                        return;
                    }
                }
            }

            Write(StringRepr(e.Value));
        }

        protected override void GenerateMethodInvokeExpression(CodeMethodInvokeExpression e) {
            if (e.Method.TargetObject != null) {
                GenerateExpression(e.Method.TargetObject);
                if (!String.IsNullOrEmpty(e.Method.MethodName)) Write(".");
            }

            if (e.Method.MethodName != null) {
                if (e.Method.TargetObject is CodeThisReferenceExpression)
                    //If the code is invoking the special method ctorFieldInit, then WritePrivatePrefix
                    //won't be able to detect it as a private member of the current class, which it will
                    //eventually become.
                    if ("_" + e.Method.MethodName == ctorFieldInit)
                        Write("_");
                    else
                        WritePrivatePrefix(e.Method.MethodName);
                Write(e.Method.MethodName);
            }
            EmitGenericTypeArgs(e.Method.TypeArguments);

            Write("(");
            OutputExpressionList(e.Parameters);
            Write(")");
        }

        private void EmitGenericTypeArgs(CodeTypeReferenceCollection typeArgs) {
            //we're using duck typing so the data types are irrelevant
            //if (typeArgs != null && typeArgs.Count > 0) {
            //    Write("[");
            //    for (int i = 0; i < typeArgs.Count; i++) {
            //        if (i != 0) Write(", ");
            //        Write(typeArgs[i].BaseType);
            //    }
            //    Write("]");
            //}
        }

        protected override void GenerateMethodReferenceExpression(CodeMethodReferenceExpression e) {
            GenerateExpression(e.TargetObject);
            Write(".");

            if (e.TargetObject is CodeThisReferenceExpression) WritePrivatePrefix(e.MethodName);

            Write(e.MethodName);

            EmitGenericTypeArgs(e.TypeArguments);
        }

        protected override void GenerateMethodReturnStatement(CodeMethodReturnStatement e) {
            Write("return ");
            if (e.Expression != null) {
                GenerateExpression(e.Expression);
            }
            WriteLine();            
        }

        protected override void GenerateNamespaceEnd(CodeNamespace e) {
            if (!String.IsNullOrEmpty(e.Name)) {
                Indent--;
                WriteLine("def CreateFromDocument(xml_text):");
                Indent++;
                WriteLine("\"\"\"Parse the given XML and use the document element to create a");
                WriteLine("Python instance.");
                WriteLine();
                WriteLine("@param xml_text An XML document.  This should be data (Python 2");
                WriteLine("str), or a text (Python 2 unicode)\"\"\"");
                WriteLine("types = [" + String.Join(", ", topLevelTypes.ToArray()) + "]");
                WriteLine("return serialize(xml_text, types)");
                Indent--;

                if (typeStack.Count == 0 && entryPointNamespace != null) {
                    // end of the outer most scope, generate the real call
                    // to the entry point if we have one.
                    System.Console.WriteLine("Generate real call to the entry point");
                    WriteLine("");
                    WriteLine(String.Format("{0}.RealEntryPoint()", entryPointNamespace));
                    entryPointNamespace = null;
                }

                namespaceStack.Pop();

                
            }
        }

        protected override void GenerateNamespaceImport(CodeNamespaceImport e) {
            if (namespaceStack.Count == 0) {
                RealGenerateNamespaceImport(e);
            }
        }

        protected override void GenerateNamespaceStart(CodeNamespace e) {            
            if (!UserDataFalse(e.UserData, "PreImport")) {
                // loigcally part of the namespace declaration, so
                // we generate these before flushing output (as flushing will advance
                // our cursor past the start of these).
                GenerateNamespaceImportsWorker(e);
            }

            if (!String.IsNullOrEmpty(e.Name)) {
                namespaceStack.Push(e);

                lastNamespace = e.Name;

                // the best way to realize namespaces in python are modules not classes                
                Write("# Namespace ");
                Write(e.Name);
                Write("\n");           
                //Write("class ");
                //Write(e.Name);
                //WriteLine(": # namespace");
                //Indent++;

                if (UserDataFalse(e.UserData, "PreImport")) {
                    GenerateNamespaceImportsWorker(e);
                }
            }
        }

        public static string StringRepr(object o)
        {
            if (o == null) return "None";

            string s = o as string;
            if (s != null) return StringOps.Quote(s);
            if (o is int) return o.ToString();
            if (o is long) return ((long)o).ToString() + "L";
            if (o is double) return ((double)o).ToString();
            if (o is float) return ((float)o).ToString();
            if (o is bool) return (bool)o ? "True" : "False";

            throw new Exception("FATAL ERROR: Could not resolve string representation for object " + o.ToString());
        }

        private void GenerateNamespaceImportsWorker(CodeNamespace e) {            
            foreach (CodeNamespaceImport cni in e.Imports) {
                RealGenerateNamespaceImport(cni);                                
            }            
            
            WriteLine("from NMF.python.decorators import accepts, returns, Self");  // import for @returns and @accepts
            WriteLine("from NMF.python.EventHandler import EventHandler");
            WriteLine("from NMF.python.serializer import serialize");
            
            WriteLine("import NMF");
        }

        private void RealGenerateNamespaceImport(CodeNamespaceImport e) {
            string fromImport = e.UserData["FromImport"] as string;
            if (e.UserData["Dupped"] != null) {
                WriteLine(String.Format("from {0} import {1}", e.Namespace, fromImport));
                WriteLine(String.Format("import {0}", e.Namespace));
            } else if (fromImport != null) {
                WriteLine(String.Format("from {0} import {1}", e.Namespace, fromImport));
            } else {
                WriteLine(String.Format("from {0} import *", e.Namespace));
            }            
        }

        protected override void GenerateObjectCreateExpression(CodeObjectCreateExpression e) {
            OutputType(e.CreateType);
            EmitGenericTypeArgs(e.CreateType.TypeArguments);
            Write("(");
            OutputExpressionList(e.Parameters);
            Write(")");
        }

        protected override void GenerateProperty(CodeMemberProperty e, CodeTypeDeclaration c) {
            string priv = String.Empty;
            if ((e.Attributes & MemberAttributes.AccessMask) == MemberAttributes.Private) priv = "_";

            string thisName = null;
            if ((e.Attributes & MemberAttributes.ScopeMask) != MemberAttributes.Static)
                thisName = UserDataString(e.UserData, "ThisArg", "self");

            if (e.HasGet) {
                //WriteLine(String.Format("#this name {0} {1}", thisName,e.Attributes));
                string getterName = UserDataString(e.UserData, "GetName", priv + "get_" + e.Name);

                GenerateMethodWorker(
                    thisName,
                    UserDataString(e.UserData, "ThisType", null),
                    getterName,
                    new CodeParameterDeclarationExpressionCollection(),
                    e.GetStatements,
                    e.Type,
                    e.UserData);
            }

            if (e.HasSet) {
                string setterName = UserDataString(e.UserData, "SetName", priv + "set_" + e.Name);

                GenerateMethodWorker(thisName, UserDataString(e.UserData, "ThisType", null), setterName, new CodeParameterDeclarationExpressionCollection( new CodeParameterDeclarationExpression[] { 
                            new CodeParameterDeclarationExpression(e.Type, "value") }), 
                    e.SetStatements, null, e.UserData);
            }

            string name = priv + e.Name;

            Write(name);
            Write(" = property(");
            string comma = "";
            if (e.HasGet) {
                Write("fget=" + priv + "get_");
                Write(e.Name);
                comma = ",";
            }
            if (e.HasSet) {
                Write(comma);
                Write("fset=" + priv + "set_");
                Write(e.Name);
            }
            if (e.Comments != null && e.Comments.Count > 0) {
                Write(",doc=\"\"\"");
                foreach (CodeCommentStatement comment in e.Comments) {
                    if (!comment.Comment.DocComment) continue;

                    WriteLine(comment.Comment.Text);
                }
                Write("\"\"\"");
            }
            WriteLine(")");

            
        }

        protected override void GeneratePropertyReferenceExpression(CodePropertyReferenceExpression e) {
            if (e.TargetObject != null) {
                GenerateExpression(e.TargetObject);
                Write(".");
            }

            if (e.TargetObject is CodeThisReferenceExpression) WritePrivatePrefix(e.PropertyName);

            Write(e.PropertyName);
        }

        protected override void GeneratePropertySetValueReferenceExpression(CodePropertySetValueReferenceExpression e) {
            Write("value");
        }

        protected override void GenerateRemoveEventStatement(CodeRemoveEventStatement e) {
            GenerateEventReferenceExpression(e.Event);

            Write(" -= ");

            GenerateExpression(e.Listener);
            WriteLine();

            
        }

        protected override void GenerateSnippetExpression(CodeSnippetExpression e) {
            Write(e.Value);
        }

        protected override void GenerateSnippetMember(CodeSnippetTypeMember e) {
            // the codedom base trys to generate w/o indentation, but
            // we need to generate with indentation due to the signficigance
            // of white space.

            int oldIndent = Indent;
            Indent = lastIndent;
            
            WriteLine("# begin snippet member " + Indent.ToString() + CurrentTypeName);

            WriteSnippetWorker(e.Text);

            WriteLine("# end snippet member");

            Indent = oldIndent;

            
        }

        /// <summary>
        /// Checks to see if the given code matches our current indentation level.
        /// </summary>
        private bool IsProperlyIndented(string []lines) {            
            string tabbedIndent = IndentString.Replace("    ", "\t");

            for (int i = 0; i < lines.Length; i++) {
                for (int j = 0; j < Indent; j++) {
                    if (lines[i].Length == 0 || lines[i][0] == '#') continue;
                    
                    if (lines[i].Length < IndentString.Length*IndentString.Length) return false;

                    int offset = 0;
                    for (int k = 0; k < Indent; k++) {
                        if (String.Compare(lines[i], offset, IndentString, 0, IndentString.Length) == 0)
                            offset += IndentString.Length;
                        else if (String.Compare(lines[i], offset, tabbedIndent, 0, tabbedIndent.Length) == 0)
                            offset += tabbedIndent.Length;
                        else
                            return false;
                    }
                }
            }
            return true;
        }

        private void WriteSnippetWorker(string text) {
            string[] lines = text.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            bool isProperlyIndented = IsProperlyIndented(lines);

            foreach (string line in lines) {
                WriteLine(line, isProperlyIndented);
            }
        }

        protected override void GenerateSnippetStatement(CodeSnippetStatement e) {
            int oldIndent = Indent;
            Indent = lastIndent;
            

            WriteLine("# Snippet Statement");

            WriteSnippetWorker(e.Value);

            WriteLine("# End Snippet Statement");


            Indent = oldIndent;

            
        }



        protected override void GenerateThisReferenceExpression(CodeThisReferenceExpression e) {
            Write("self");
        }

        protected override void GenerateThrowExceptionStatement(CodeThrowExceptionStatement e) {
            Write("raise ");
            GenerateExpression(e.ToThrow);
            WriteLine();

            
        }

        protected override void GenerateTryCatchFinallyStatement(CodeTryCatchFinallyStatement e) {
            WriteLine("try:"); //!!! Consult UserData["NoNewLine"]
            if (e.CatchClauses.Count != 0 && e.FinallyStatements != null && e.FinallyStatements.Count > 0) {
                Indent++;
                WriteLine("try:"); //!!! Consult UserData["NoNewLine"]
            }

            Indent++;
            if (e.TryStatements != null && e.TryStatements.Count > 0)
                GenerateStatements(e.TryStatements);
            else
                WriteLine("pass");
            Indent--;


            if (e.CatchClauses.Count != 0) {
                for (int i = 0; i < e.CatchClauses.Count; i++) {
                    Write("except ");
                    OutputType(e.CatchClauses[i].CatchExceptionType);
                    if (!String.IsNullOrEmpty(e.CatchClauses[i].LocalName)) {
                        Write(", ");
                        Write(e.CatchClauses[i].LocalName);
                    }
                    if (e.CatchClauses[i].Statements != null && e.CatchClauses[i].Statements.Count > 0) {
                        WriteLine(":"); //!!! Consult UserData["NoNewLine"]
                        Indent++;
                        GenerateStatements(e.CatchClauses[i].Statements);
                        Indent--;
                    } else {
                        WriteLine(": pass"); //!!! Consult UserData["NoNewLine"]
                    }
                }
            }

            if (e.CatchClauses.Count != 0 && e.FinallyStatements != null && e.FinallyStatements.Count > 0) {
                Indent--;
            }

            if (e.FinallyStatements != null && e.FinallyStatements.Count > 0) {
                WriteLine("finally:"); //!!! Consult UserData["NoNewLine"]
                Indent++;
                GenerateStatements(e.FinallyStatements);
                Indent--;
            }

            
        }

        protected override void GenerateTypeConstructor(CodeTypeConstructor e) {
            GenerateStatements(e.Statements);            
        }

        private void GenerateFieldInit() {
            WriteLine("def " + ctorFieldInit + "(self):");
            Indent++;

            for (int i = 0; i < CurrentClass.Members.Count; i++) {
                if (CurrentClass.Members[i] is CodeMemberEvent)
                {                    
                    CodeMemberEvent e = CurrentClass.Members[i] as CodeMemberEvent;
                    Write("self.");
                    if ((e.Attributes & MemberAttributes.AccessMask) == MemberAttributes.Private) Write("_");
                    Write(e.Name);
                    Write(" = ");                    
                    if (e.Type.BaseType.Equals("System.EventHandler`1"))
                    {
                        Write("EventHandler()");
                    } else
                    {
                        //don't know what it is                        
                        Write("None");
                    }
                    WriteLine();
                } else {
                    CodeMemberField e = CurrentClass.Members[i] as CodeMemberField;
                    if (e == null) continue;
                    //member is field

                    //if (e is CodeMemberField)
                    //{
                    //    Write("self.");
                    //    Write(e.Name);
                    //    Write(" = None");
                    //    WriteLine();

                    //}

                    if (e.InitExpression != null)
                    {
                        //!!! non-static init expression should be moved to constructor

                        Write("self.");
                        if ((e.Attributes & MemberAttributes.AccessMask) == MemberAttributes.Private) Write("_");
                        Write(e.Name);
                        Write(" = ");
                        GenerateExpression(e.InitExpression);
                        WriteLine();

                    }
                    else /*if ((e.Attributes & MemberAttributes.ScopeMask) == MemberAttributes.Static)*/
                    {

                        Write("self.");
                        if ((e.Attributes & MemberAttributes.AccessMask) == MemberAttributes.Private) Write("_");
                        Write(e.Name);
                        Write(" = ");
                        switch (e.Type.BaseType)
                        {
                            case "bool":
                            case "System.Boolean":
                                Write("False"); break;
                            case "int":
                            case "System.Int32":
                                Write("0"); break;
                            default:
                                Write("None"); break;
                        }

                        WriteLine();
                    }
                }
                
            }

            Indent--;
        }

        protected override void GenerateTypeEnd(CodeTypeDeclaration e) {
            if (e.Name != "__top__" || !UserDataFalse(e.UserData, "IsTopType")) {

                if (NeedFieldInit()) {
                    GenerateFieldInit();
                }

                TypeDeclInfo popped = typeStack.Pop();
                System.Diagnostics.Debug.Assert(popped.Declaration == e);

                if (UserDataFalse(e.UserData, "NoEmit")) {
                    Indent--;
                    WriteLine();
                }

                if (entryPoint != null) {
                    WriteLine("@staticmethod");
                    WriteLine("def RealEntryPoint():");
                    Indent++;

                    if (entryPoint.Parameters.Count == 1) {
                        // should be args
                        WriteLine("import sys");

                        WriteLine(String.Format("{0} = sys.argv", entryPoint.Parameters[0].Name));
                    }

                    if (entryPoint.Statements != null && entryPoint.Statements.Count > 0)
                        GenerateStatements(entryPoint.Statements);
                    else
                        WriteLine("pass");

                    // return type: either the user has a return statement
                    // or they don't, it's not our problem here.
                    Indent--;

                    entryPoint = null;
                }
            }
            currentTypeLevel--;
            
        }

        protected override void GenerateTypeStart(CodeTypeDeclaration e) {
            processedTypes.Add(e.Name);
            if (currentTypeLevel == 0)
            {
                topLevelTypes.Add(e.Name);
            }
            currentTypeLevel++;
           
            if (e.Name != "__top__" || !UserDataFalse(e.UserData, "IsTopType")) {

                typeStack.Push(new TypeDeclInfo(e));

                if (!UserDataFalse(e.UserData, "NoEmit")) return;

                Write("class ");
                Write(e.Name);
                Write("(");
                if (e.BaseTypes.Count > 0) {
                    string comma = "";
                    for (int i = 0; i < e.BaseTypes.Count; i++) {
                        Write(comma);
                        OutputType(e.BaseTypes[i]);
                        comma = ",";
                    }
                } else {
                    Write("object");
                }
                if (e.Members.Count == 0) {
                    WriteLine("): pass"); //!!! Consult UserData["NoNewLine"]
                    Indent++;
                } else {
                    WriteLine("):"); //!!! Consult UserData["NoNewLine"]
                    Indent++;

                    bool fHasSlots = UserDataTrue(e.UserData, "HasSlots");

                    if (fHasSlots) {
                        // generate type & slot information, eg:
                        // """type(x) == int, type(y) == System.EventArgs, type(z) == bar"""
                        // __slots__ = ['x', 'y', 'z']

                        bool fOpenedQuote = false;

                        List<string> slots = new List<string>();
                        string comma = "";
                        for (int i = 0; i < e.Members.Count; i++) {
                            CodeMemberField cmf = e.Members[i] as CodeMemberField;
                            if (cmf != null && (cmf.Attributes & MemberAttributes.ScopeMask) != MemberAttributes.Static) {
                                if (!fOpenedQuote) { Write("\"\"\""); fOpenedQuote = true; }

                                string name;

                                if ((cmf.Attributes & MemberAttributes.AccessMask) == MemberAttributes.Private) {
                                    name = "_" + cmf.Name;
                                } else {
                                    name = cmf.Name;
                                }

                                Write(comma);
                                Write("type(");
                                Write(name);
                                Write(") == ");
                                GenerateTypeReferenceExpression(new CodeTypeReferenceExpression(cmf.Type));
                                comma = ", ";

                                slots.Add(name);
                            }

                            CodeMemberEvent cme = e.Members[i] as CodeMemberEvent;
                            if (cme != null) {
                                if (!fOpenedQuote) { Write("\"\"\""); fOpenedQuote = true; }

                                string name;
                                if ((cme.Attributes & MemberAttributes.AccessMask) == MemberAttributes.Private) {
                                    name = "_" + cme.Name;
                                } else {
                                    name = cme.Name;
                                }

                                Write(comma);
                                Write("type(");
                                Write(name);
                                Write(") == ");
                                GenerateTypeReferenceExpression(new CodeTypeReferenceExpression(cme.Type));
                                comma = ", ";

                                slots.Add(name);
                            }
                        }
                        if (fOpenedQuote) WriteLine("\"\"\"");                        

                        for (int i = 0; i < e.Members.Count; i++) {
                            CodeMemberField cmf = e.Members[i] as CodeMemberField;
                            if (cmf != null && (cmf.Attributes & MemberAttributes.ScopeMask) == MemberAttributes.Static) {
                                if (cmf.Type.BaseType == "System.Boolean" || cmf.Type.BaseType == "bool")
                                    WriteLine(String.Format("{0} = False", cmf.Name));
                                else
                                    WriteLine(String.Format("{0} = None", cmf.Name));
                            }
                        }
                    }
                }
            }
            lastIndent = Indent;
        }

        protected override void GenerateVariableDeclarationStatement(CodeVariableDeclarationStatement e) {
            // if we have no init expression then we don't
            // need to declare the variable yet.  Once we
            // have the value we will infer it's type via
            // the parser on the re-parse and generate a
            // VariableDeclarationStatement.
            if (e.InitExpression != null) {

                Write(e.Name);
                Write(" = ");
                GenerateExpression(e.InitExpression);
                WriteLine();

                
            }
        }

        protected override void GenerateVariableReferenceExpression(CodeVariableReferenceExpression e) {
            Write(e.VariableName);
        }

        protected override string GetTypeOutput(CodeTypeReference value) {
            if (value.ArrayRank > 0) {
                return "list";
            }

            if (value.TypeArguments != null && value.TypeArguments.Count > 0) {
                // generate generic type reference
                string nonGenericName = value.BaseType.Substring(0, value.BaseType.LastIndexOf('`'));
                StringBuilder baseName = new StringBuilder(PythonizeType(nonGenericName));

                //since we're using duck typing the generic types are irrelevant
                //baseName.Append('[');
                //string comma = "";
                //for (int i = 0; i < value.TypeArguments.Count; i++) {
                //    baseName.Append(comma);
                //    baseName.Append(GetTypeOutput(value.TypeArguments[i]));
                //    comma = ", ";
                //}
                //baseName.Append(']');
                return baseName.ToString();
            }

            return PythonizeType(value.BaseType);
        }

        private static string PythonizeType(string baseType) {
            if (baseType == "Boolean" || baseType == "System.Boolean") {
                return "bool";
                /*} else if (baseType == "System.Int32") {
                    return "int";
                } else if (baseType == "System.String") {
                    return "str";*/
            } else if (baseType == "Void" || baseType == "System.Void" || baseType == "void") {
                return "None";
            }
            return baseType;
        }

        protected override bool IsValidIdentifier(string value) {
            for (int i = 0; i < keywords.Length; i++) {
                if (keywords[i] == value) return false;
            }
            for (int i = 0; i < value.Length; i++) {
                if ((value[i] >= 'a' && value[i] <= 'z') ||
                    (value[i] >= 'A' && value[i] <= 'Z') ||
                    (i != 0 && value[i] >= '0' && value[i] <= '9') ||
                    value[i] == '_') {
                    continue;
                }
                return false;
            }
            return true;
        }

        protected override string NullToken {
            get { return "None"; }
        }

        protected override void OutputType(CodeTypeReference typeRef) {
            Write(GetTypeOutput(typeRef));
        }

        protected override string QuoteSnippetString(string value) {
            return (string)StringRepr(value);
        }

        protected override bool Supports(GeneratorSupport support) {
            switch (support) {
                case GeneratorSupport.ArraysOfArrays: return true;
                case GeneratorSupport.AssemblyAttributes: return false;
                case GeneratorSupport.ChainedConstructorArguments: return false;
                case GeneratorSupport.ComplexExpressions: return true;
                case GeneratorSupport.DeclareDelegates: return false;
                case GeneratorSupport.DeclareEnums: return false;
                case GeneratorSupport.DeclareEvents: return false;
                case GeneratorSupport.DeclareIndexerProperties: return true;
                case GeneratorSupport.DeclareInterfaces: return false;
                case GeneratorSupport.DeclareValueTypes: return false;
                case GeneratorSupport.EntryPointMethod: return true;
                case GeneratorSupport.GenericTypeDeclaration: return false;
                case GeneratorSupport.GenericTypeReference: return true;
                case GeneratorSupport.GotoStatements: return false;
                case GeneratorSupport.MultidimensionalArrays: return true;
                case GeneratorSupport.MultipleInterfaceMembers: return false;
                case GeneratorSupport.NestedTypes: return true;
                case GeneratorSupport.ParameterAttributes: return false;
                case GeneratorSupport.PartialTypes: return false;
                case GeneratorSupport.PublicStaticMembers: return true;
                case GeneratorSupport.ReferenceParameters: return false;
                case GeneratorSupport.Resources: return false;
                case GeneratorSupport.ReturnTypeAttributes: return false;
                case GeneratorSupport.StaticConstructors: return false;
                case GeneratorSupport.TryCatchStatements: return true;
                case GeneratorSupport.Win32Resources: return false;
            }
            return false;
        }

        #region Not supported overrides
        protected override void GenerateAttributeDeclarationsEnd(CodeAttributeDeclarationCollection attributes) {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        protected override void GenerateAttributeDeclarationsStart(CodeAttributeDeclarationCollection attributes) {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        protected override void GenerateGotoStatement(CodeGotoStatement e) {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        protected override void GenerateLabeledStatement(CodeLabeledStatement e) {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        protected override void GenerateLinePragmaEnd(CodeLinePragma e) {
            WriteLine("");
            WriteLine("#End ExternalSource");
        }

        protected override void GenerateLinePragmaStart(CodeLinePragma e) {
            WriteLine("");
            Write("#ExternalSource(\"");
            Write(e.FileName);
            Write("\",");
            // adjust by 1 for the comment we add in for each snippet member or statement
            Write(e.LineNumber+1);  
            WriteLine(")");
        }
        #endregion

        #endregion

        #region Non-required overrides
        protected override void OutputParameters(CodeParameterDeclarationExpressionCollection parameters) {
            string comma = "";
            for (int i = 0; i < parameters.Count; i++) {
                Write(comma);
                Write(parameters[i].Name);
                comma = ",";
            }
        }
        protected override void GenerateTypeOfExpression(CodeTypeOfExpression e) {
            if (e.Type.BaseType == this.typeStack.Peek().Declaration.Name) {
                CodeMemberMethod curMeth = CurrentMember as CodeMemberMethod;
                if (curMeth != null) {
                    if ((curMeth.Attributes & MemberAttributes.ScopeMask) == MemberAttributes.Static)
                        throw new InvalidOperationException("can't access current type in static scope");
                }
                Write("self.__class__");
            } else {
                OutputType(e.Type);
            }
        }

        protected override void ContinueOnNewLine(string st) {
            WriteLine("\\");
        }

        protected override void GenerateBinaryOperatorExpression(CodeBinaryOperatorExpression e) {
            GenerateExpression(e.Left);
            switch (e.Operator) {
                case CodeBinaryOperatorType.Add: Write(" + "); break;
                case CodeBinaryOperatorType.Assign: Write(" = "); break;
                case CodeBinaryOperatorType.BitwiseAnd: Write(" & "); break;
                case CodeBinaryOperatorType.BitwiseOr: Write(" | "); break;
                case CodeBinaryOperatorType.BooleanAnd: Write(" and "); break;
                case CodeBinaryOperatorType.BooleanOr: Write(" or "); break;
                case CodeBinaryOperatorType.Divide: Write(" / "); break;
                case CodeBinaryOperatorType.GreaterThan: Write(" > "); break;
                case CodeBinaryOperatorType.GreaterThanOrEqual: Write(" >= "); break;
                case CodeBinaryOperatorType.IdentityEquality: Write(" is "); break;
                case CodeBinaryOperatorType.IdentityInequality: Write(" != "); break;
                case CodeBinaryOperatorType.LessThan: Write(" < "); break;
                case CodeBinaryOperatorType.LessThanOrEqual: Write(" <= "); break;
                case CodeBinaryOperatorType.Modulus: Write(" % "); break;
                case CodeBinaryOperatorType.Multiply: Write(" * "); break;
                case CodeBinaryOperatorType.Subtract: Write(" - "); break;
                case CodeBinaryOperatorType.ValueEquality: Write(" == "); break;
            }
            if (e.Right is CodeBinaryOperatorExpression) Write("(");
            GenerateExpression(e.Right);
            if (e.Right is CodeBinaryOperatorExpression) Write(")");
        }

        #endregion

        #region Overrides to ensure only we call Write
        public override void GenerateCodeFromMember(CodeTypeMember member, System.IO.TextWriter writer, CodeGeneratorOptions options) {
            CodeGeneratorOptions opts = (options == null) ? new CodeGeneratorOptions() : options;
            opts.BlankLinesBetweenMembers = false;

            base.GenerateCodeFromMember(member, writer, opts);
        }
        protected override void OutputAttributeDeclarations(CodeAttributeDeclarationCollection attributes) {
            //!!! Implement me
        }

        protected override void OutputAttributeArgument(CodeAttributeArgument arg) {
            //!!! Implement me
        }

        protected override void OutputTypeAttributes(System.Reflection.TypeAttributes attributes, bool isStruct, bool isEnum) {
            //!!! implement me
        }

        protected override void OutputDirection(FieldDirection dir) {
            // not supported in Python
        }

        protected override void OutputFieldScopeModifier(MemberAttributes attributes) {
            // not supported in Python
        }

        protected override void OutputMemberAccessModifier(MemberAttributes attributes) {
            // not supported in Python
        }

        protected override void OutputMemberScopeModifier(MemberAttributes attributes) {
            // not supported in Python
        }

        protected override void OutputTypeNamePair(CodeTypeReference typeRef, string name) {
            OutputType(typeRef);
            Write(" ");
            OutputIdentifier(name);
        }

        protected override void OutputIdentifier(string ident) {
            Write(ident);
        }

        protected override void OutputExpressionList(CodeExpressionCollection expressions, bool newlineBetweenItems) {
            bool first = true;
            IEnumerator en = expressions.GetEnumerator();
            Indent++;
            while (en.MoveNext()) {
                if (first) {
                    first = false;
                } else {
                    if (newlineBetweenItems) ContinueOnNewLine(",");
                    else Write(", ");
                }
                GenerateExpression((CodeExpression)en.Current);
            }
            Indent--;
        }

        protected override void OutputOperator(CodeBinaryOperatorType op) {
            switch (op) {
                case CodeBinaryOperatorType.Add: Write("+"); break;
                case CodeBinaryOperatorType.Subtract: Write("-"); break;
                case CodeBinaryOperatorType.Multiply: Write("*"); break;
                case CodeBinaryOperatorType.Divide: Write("/"); break;
                case CodeBinaryOperatorType.Modulus: Write("%"); break;
                case CodeBinaryOperatorType.Assign: Write("="); break;
                case CodeBinaryOperatorType.IdentityInequality: Write("!="); break;
                case CodeBinaryOperatorType.IdentityEquality: Write("=="); break;
                case CodeBinaryOperatorType.ValueEquality: Write("=="); break;
                case CodeBinaryOperatorType.BitwiseOr: Write("|"); break;
                case CodeBinaryOperatorType.BitwiseAnd: Write("&"); break;
                case CodeBinaryOperatorType.BooleanOr: Write("||"); break;
                case CodeBinaryOperatorType.BooleanAnd: Write("&&"); break;
                case CodeBinaryOperatorType.LessThan: Write("<"); break;
                case CodeBinaryOperatorType.LessThanOrEqual: Write("<="); break;
                case CodeBinaryOperatorType.GreaterThan: Write(">"); break;
                case CodeBinaryOperatorType.GreaterThanOrEqual: Write(">="); break;
            }
        }

        protected override void GenerateParameterDeclarationExpression(CodeParameterDeclarationExpression e) {
            if (e.CustomAttributes.Count > 0) {
                OutputAttributeDeclarations(e.CustomAttributes);
                Write(" ");
            }

            OutputDirection(e.Direction);
            OutputTypeNamePair(e.Type, e.Name);
        }

        protected override void GenerateSingleFloatValue(float s) {
            Write(StringRepr(s));
        }

        protected override void GenerateDoubleValue(double d) {
            Write(StringRepr(d));
        }

        protected override void GenerateDecimalValue(decimal d) {
            Write(StringRepr(d));
        }

        #endregion

        protected void OutputParameters(string instanceName, CodeParameterDeclarationExpressionCollection parameters) {
            string comma = "";
            if (instanceName != null) {
                Write(instanceName);
                comma = ", ";
            }
            for (int i = 0; i < parameters.Count; i++) {
                Write(comma);
                Write(parameters[i].Name);
                comma = ", ";
            }
        }

        protected override void GenerateTypeReferenceExpression(CodeTypeReferenceExpression e) {
            if ((e.Type.BaseType == "void")) {
                Write("None");
            } else { 
                base.GenerateTypeReferenceExpression(e);
            }

        }
        private void GenerateMethodWorker(string instanceName, string instanceType, string name, CodeParameterDeclarationExpressionCollection parameters, CodeStatementCollection stmts, CodeTypeReference retType, IDictionary userData) {
            // generate decorators w/ type info            

            if (retType != null)
            {
                if (UserDataTrue(userData, "HasReturns"))
                {
                    Write("@returns(");
                    if (processedTypes.Contains(retType.BaseType))
                    {
                        Write("Self()");
                    } else
                    {
                        GenerateTypeReferenceExpression(new CodeTypeReferenceExpression(retType));
                    }                    
                    WriteLine(")");
                }
            }

            if (UserDataTrue(userData, "HasAccepts")) {
                Write("@accepts(");
                string comma = "";                
                if (instanceType != null) {
                    Write("Self()");
                    comma = ", ";
                } else if (instanceName != null) {
                    Write("Self()");
                    comma = ", ";
                }
                foreach (CodeParameterDeclarationExpression param in parameters) {
                    Write(comma);
                    //since we're writing the accepts decorator which gets parsed in python before the class type instance
                    //exists we can't have accpets with it's own class => change it Self()
                    if (processedTypes.Contains(param.Type.BaseType)) {
                        Write("Self()");
                    } else
                    {
                        GenerateTypeReferenceExpression(new CodeTypeReferenceExpression(param.Type));
                    }                    
                    comma = ", ";
                }
                WriteLine(")");
            }            


            // generate raw method body
            Write("def ");
            Write(name);
            Write("(");
            OutputParameters(instanceName, parameters);

            int cursorCol, cursorRow;
            if (stmts.Count != 0) {
                WriteLine("):"); //!!! Consult UserData["NoNewLine"]
                Indent++;
                lastIndent = Indent;
                GenerateStatements(stmts);
                Indent--;
                cursorCol = col;
                cursorRow = row;
            } else {
                WriteLine("):"); //!!! Consult UserData["NoNewLine"]
                Indent++;
                Write("pass");
                cursorCol = col - 3;
                cursorRow = row;
                WriteLine();
                Indent--;
            }

            WriteLine("");

            // store the location the cursor shold goto for this function.
            //userData[typeof(System.Drawing.Point)] = new System.Drawing.Point(cursorCol, cursorRow); //???
        }        

        private static string UserDataString(IDictionary userData, string name, string defaultValue) {
            if (userData == null) return defaultValue;
            string res = userData[name] as string;
            if (res == null) return defaultValue;
            return res;
        }

        private static bool UserDataFalse(IDictionary userData, string name) {
            return userData == null ||
                userData[name] == null ||
                ((bool)userData[name]) == false;
        }

        private static bool UserDataTrue(IDictionary userData, string name) {
            return userData == null ||
                userData[name] == null ||
                ((bool)userData[name]) == true;
        }                

        private void Write(object val) {
            Write(val.ToString());
        }

        private void Write(int val) {
            Write(val.ToString());
        }

        private void WriteLine() {
            WriteLine("");
        }

        private void Write(string txt) {
            Write(txt, false);
        }

        private void Write(string txt, bool preserveSpaces) {
            // enforce consistent indenting - we always use 4 spaces for indentation
            // regardless of what the user provided as this guarantees the code compiles.  The
            // user could provide String.Empty for Indent which breaks Python code.

            int prevIndent = Indent;
            Indent = 0;
            string[] lines = txt.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            for (int i = 0; i < lines.Length; i++) {
                if (i != 0) {
                    WriteLineTargetBuffer(String.Empty);
                    if (!preserveSpaces) {
                        for (int j = 0; j < prevIndent; j++) WriteTargetBuffer(IndentString);
                        col = prevIndent * 4;
                    }
                    row++;
                } else if (col == 0 && !preserveSpaces) {
                    for (int j = 0; j < prevIndent; j++) WriteTargetBuffer(IndentString);
                    col = prevIndent * 4;
                }

                WriteTargetBuffer(lines[i]);
                col += lines[i].Length;
            }
            Indent = prevIndent;
        }

        private void WriteLine(string txt) {
            WriteLine(txt, false);
        }

        private void WriteLine(string txt, bool preserveSpaces) {
            // enforce consistent indenting - we always use 4 spaces for indentation
            // regardless of what the user provided as this guarantees the code compiles.  The
            // user could provide String.Empty for Indent which breaks Python code.

            int prevIndent = Indent;
            Indent = 0;
            string[] lines = txt.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            for (int i = 0; i < lines.Length; i++) {
                if (col == 0 && !preserveSpaces) {
                    for (int j = 0; j < prevIndent; j++) WriteTargetBuffer(IndentString);
                }

                WriteLineTargetBuffer(lines[i]);
                col = 0;
                row++;
            }
            Indent = prevIndent;
        }

        private string IndentString {
            get {
                if (String.IsNullOrEmpty(Options.IndentString)) return "    ";

                return Options.IndentString;
            }
        }
        private void WriteTargetBuffer(string text) {
            Output.Write(text);
        }

        private void WriteLineTargetBuffer(string text) {
            Output.WriteLine(text);
        }

        private void WriteWarning()
        {
            WriteLine("#------------------------------------------------------------------------------");
            WriteLine("#     This code was generated by a tool.");
            WriteLine("#");
            WriteLine("#     Changes to this file can cause unexpected behaviour");
            WriteLine("#     and get lost when the code gets generated again.");
            WriteLine("#------------------------------------------------------------------------------");
        }

    }


}