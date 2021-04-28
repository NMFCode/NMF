using System;
using System.CodeDom;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace PythonCodeGenerator.CodeDom
{
    class CodeUnitPreProcessor
    {
        private static readonly Dictionary<string, string> net2PythonTypes = new Dictionary<string, string>
        {
            { "System.Int32", "int" },
            { "System.Int16", "int" },
            { "System.Int64", "int" }, //long and int were unified PEP-237
            { "System.String", "str" },
            { "System.Boolean", "bool" },
            { "System.Char", "str" },
            { "System.Decimal", "float" },
            { "System.Double", "float" },
            { "System.Object", "object" },
            { "System.Collections.Specialized.NotifyCollectionChangedEventArgs", "pyNMF.collections.object_model.NotifyCollectionChangedEventArgs" },
            { "System.EventArgs", "pyNMF.EventArgs" },
            { "System.Collections.IList", "list" },
            { "System.EventHandler", "pyNMF.EventHandler" }
        };

        private static readonly List<string> supressedMembers = new List<string>
        {
            "GetExpressionForReference",
            "GetExpressionForAttribute",
            "GetClass",
            "ClassInstance",
            "_classInstance"
        };
        
         /// <summary>
         /// uses a depth first search approach to find all used variable types in the given compile unit
         /// and replaces them with their python counter part as prescribed in net2PythonTypes dictonary.
         /// Also removes all interfaces, Types that end with "Proxy" and methods that are listed in 
         /// unwanted methods
         /// </summary>
        public static void PreProcessCompileUnit(CodeCompileUnit unit)
        {
            RemoveNetSystemImports(unit);
            for(int i = 0; i < unit.Namespaces.Count; i++)
            {
                CodeNamespace currentNamespace = unit.Namespaces[i];
                List<CodeTypeDeclaration> removeLater = new List<CodeTypeDeclaration>();             
                for(int j = 0; j < currentNamespace.Types.Count; j++)
                {                    
                    if (currentNamespace.Types[j].IsInterface)
                    {
                        Console.WriteLine("Removing interface " + currentNamespace.Types[j].Name);
                        removeLater.Add(currentNamespace.Types[j]);
                        continue;                        
                    } 
                    
                    PreProcessType(currentNamespace.Types[j]);
                }
                foreach (CodeTypeDeclaration ctd in removeLater) {
                    currentNamespace.Types.Remove(ctd);
                }
            }
        }

        /// <summary>
        /// Called for each type there is by replaceNetVariableTypesWithNativeVariableTypes
        /// </summary>
        private static void PreProcessType(CodeTypeDeclaration e)
        {
            ReplaceInterfaceParameters(e);
            PreProcessMembers(e.Members);
            for(int i = 0; i < e.BaseTypes.Count; i++)
            {
                e.BaseTypes[i] = ReplaceDeepVariableTypesInTypeReference(e.BaseTypes[i]);
            }
        }

        /// <summary>
        /// Interfaces do not exist in python, however multi inheritance does. Instead of
        /// using interfaces, use multi intheritance.
        /// </summary>
        private static void ReplaceInterfaceParameters(CodeTypeDeclaration e)
        {
            List<string> parameters = new List<string>();
            CodeTypeReferenceCollection coll = new CodeTypeReferenceCollection();
            for(int i = 0; i < e.BaseTypes.Count; i++)
            {
                string baseType = null;
                if (e.BaseTypes[i].BaseType.StartsWith("I") && Char.IsUpper(e.BaseTypes[i].BaseType[1]))
                {
                    baseType = e.BaseTypes[i].BaseType.Remove(0, 1); //remove the first character ("I")
                    e.BaseTypes[i].BaseType = baseType; //assigning directly causes a bug
                } 
                
                //if after removing the char I from an Interface, the type already exists remove it                                
                if(!(parameters.Contains(baseType) || e.BaseTypes[i].BaseType.Equals(e.Name)))                
                {                    
                    parameters.Add(e.BaseTypes[i].BaseType);
                    coll.Add(e.BaseTypes[i]);
                }
            }
            //copy coll in e.BaseTypes
            e.BaseTypes.Clear();
            foreach(CodeTypeReference tRef in coll) {
                e.BaseTypes.Add(tRef);
            }
        }

        private static void PreProcessMembers(CodeTypeMemberCollection members)
        {
            bool hasConstructor = false;
            List<CodeTypeMember> removeLater = new List<CodeTypeMember>(); //removeLater or indeces would shift
            for(int i = 0; i < members.Count; i++)
            {
                CodeTypeMember current = members[i];                
                if (supressedMembers.Contains(current.Name) || ((current.Attributes & MemberAttributes.Static) == MemberAttributes.Static))
                {
                    Console.WriteLine("Removing member " + current.Name);
                    removeLater.Add(current);
                }
                else if(current is CodeTypeDeclaration)
                {
                    //Console.WriteLine("Found nested type: " + current.Name);
                    if (current.Name.EndsWith("Proxy"))
                    {
                        Console.WriteLine("Removing nested type " + current.Name + " since it's a proxy!");
                        removeLater.Add(current);
                    } else
                    {
                        PreProcessType((CodeTypeDeclaration)current);
                    }                    
                }
                else if (current is CodeMemberField)
                {                    
                    ((CodeMemberField) members[i]).Type = ReplaceDeepVariableTypesInTypeReference(((CodeMemberField)current).Type);
                }
                else if(current is CodeMemberProperty)
                {
                    var property = (CodeMemberProperty)members[i];
                    property.Type = ReplaceDeepVariableTypesInTypeReference(property.Type);
                    RemoveFeatureReferences(property.SetStatements);
                }
                else if (current is CodeMemberMethod)
                {
                    var method = (CodeMemberMethod)members[i];
                    ReplaceDeepVaribleTypesInCodeMemberMethod(method);
                    RemoveFeatureReferences(method.Statements);
                }

                if (members[i] is CodeConstructor)
                {
                    hasConstructor = true;
                }
            }

            for(int i = 0; i < removeLater.Count; i++)
            {
                members.Remove(removeLater[i]);
            }

            if (!hasConstructor)
            {
                //create empty constructor to be sure GenerateConstructor is called and therefore all fields get initialized
                CodeConstructor ctor = new CodeConstructor();
                //since the super constructor would have been called, add a call to it
                //this call needs to be AFTER the _ConstructorFieldInitFunction call to ensure its initialization isnt overriden by it                      
                ctor.Statements.Add(new CodeMethodReferenceExpression(new CodeBaseReferenceExpression(), "__init__()"));

                members.Add(ctor);
            }
        }

        private static void RemoveFeatureReferences(CodeStatementCollection statements)
        {
            foreach (var stmt in statements)
            {
                if ((stmt as CodeExpressionStatement)?.Expression is CodeMethodInvokeExpression call && (call.Method.MethodName.EndsWith("PropertyChanging") ||
                                     call.Method.MethodName.EndsWith("PropertyChanged") ||
                                     call.Method.MethodName.EndsWith("CollectionChanging") ||
                                     call.Method.MethodName.EndsWith("CollectionChanged")))
                {
                    call.Parameters.RemoveAt(call.Parameters.Count - 1);
                }
                var condition = (stmt as CodeConditionStatement);
                if (condition != null)
                {
                    RemoveFeatureReferences(condition.TrueStatements);
                    RemoveFeatureReferences(condition.FalseStatements);
                }
            }
        }

        private static void ReplaceDeepVaribleTypesInCodeMemberMethod(CodeMemberMethod method)
        {                                    
            //ImplementationTypes
            for (int i = 0; i < method.ImplementationTypes.Count; i++)
            {
                method.ImplementationTypes[i] = ReplaceDeepVariableTypesInTypeReference(method.ImplementationTypes[i]);
            }

            //Parameters
            for(int i = 0; i < method.Parameters.Count; i++)
            {
                method.Parameters[i].Type = ReplaceDeepVariableTypesInTypeReference(method.Parameters[i].Type);
            }

            //PrivateImplementationType
            if (method.PrivateImplementationType != null)
            {
                method.PrivateImplementationType = ReplaceDeepVariableTypesInTypeReference(method.PrivateImplementationType);
            }            

            //ReturnType
            if (method.ReturnType != null)
            {
                method.ReturnType = ReplaceDeepVariableTypesInTypeReference(method.ReturnType);
            }            

            //ReturnTypeCustomAttributes
            /*for(int i = 0; i < method.ReturnTypeCustomAttributes.Count; i++)
            {
                method.ReturnTypeCustomAttributes[i].AttributeType = replaceDeepVariableTypesInTypeReference(method.ReturnTypeCustomAttributes[i].AttributeType);
            }*/
        }

        private static CodeTypeReference ReplaceDeepVariableTypesInTypeReference(CodeTypeReference tr)
        {
            //python variables are nullable by default => remove it 
            if (tr.BaseType.Contains("Nullable"))
            {
                if(tr.TypeArguments.Count != 1)
                {
                    throw new Exception("Fatal Error: Unkown Nullable Generic");
                }
                return ReplaceDeepVariableTypesInTypeReference(tr.TypeArguments[0]);
            }
            else
            {
                tr.BaseType = ReplaceTypeString(tr.BaseType);

                for (int i = 0; i < tr.TypeArguments.Count; i++)
                {
                    tr.TypeArguments[i] = ReplaceDeepVariableTypesInTypeReference(tr.TypeArguments[i]);
                }
                return tr;
            }            
        }

        private static string ReplaceTypeString(string type)
        {
            if (type.Contains("`"))
            {
                type = type.Substring(0, type.LastIndexOf('`'));
            }
            if (net2PythonTypes.ContainsKey(type))
            {
                return net2PythonTypes[type];
            } else if (type.StartsWith("I") && Char.IsUpper(type[1]) ){
                return type.Remove(0, 1);
            } else
            {
                return type;
            }
        }

        
        /// <summary>
        /// remove Imports which contain System
        /// these imports are part of the .Net platform so they can't be resolved in a normal
        /// python environment anyway
        /// </summary>
        private static void RemoveNetSystemImports(CodeCompileUnit e)
        {            
            foreach (CodeNamespace ns in e.Namespaces)
            {
                CodeNamespaceImportCollection cleaned = new CodeNamespaceImportCollection();
                foreach (CodeNamespaceImport import in ns.Imports)
                {
                    if (!import.Namespace.StartsWith("System") &&
                        !import.Namespace.StartsWith("NMF"))
                    {
                        cleaned.Add(import);
                    }
                }
                cleaned.Add(new CodeNamespaceImport("pyNMF"));
                cleaned.Add(new CodeNamespaceImport("pyNMF.collections.generic"));
                cleaned.Add(new CodeNamespaceImport("pyNMF.collections.object_model"));
                cleaned.Add(new CodeNamespaceImport("pyNMF.serialization"));
                ns.Imports.Clear();
                foreach (CodeNamespaceImport import in cleaned)
                {
                    ns.Imports.Add(import);
                }
            }
        }
    }
}
