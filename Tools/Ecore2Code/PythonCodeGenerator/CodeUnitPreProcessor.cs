using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PythonCodeGenerator.CodeDom
{
    class CodeUnitPreProcessor
    {
        private static Dictionary<string, string> net2PythonTypes = net2PythonTypesInit();
        private static List<string> unwantedMethods = unwantedMethodsInit();

        //methods with these names will be removed from any type found
        private static List<string> unwantedMethodsInit()
        {
            List<string> tmp = new List<string>();
            tmp.Add("GetExpressionForReference");
            tmp.Add("GetExpressionForAttribute");
            tmp.Add("GetClass");
            return tmp;
        }

        private static Dictionary<string, string> net2PythonTypesInit()
        {
            Dictionary<string, string> tmp = new Dictionary<string, string>();
            tmp.Add("System.Int32", "int");
            tmp.Add("System.Int16", "int");
            tmp.Add("System.Int64", "int"); //long and int were unified PEP-237
            tmp.Add("System.String", "str");
            tmp.Add("System.Boolean", "bool");
            tmp.Add("System.Char", "str");
            tmp.Add("System.Decimal", "float");
            tmp.Add("System.Double", "float");
            tmp.Add("System.Object", "object");
            tmp.Add("System.Collections.Specialized.NotifyCollectionChangedEventArgs", "NMF.Collections.ObjectModel.NotifyCollectionChangedEventArgs");
            tmp.Add("System.EventArgs", "NMF.python.EventArgs");
            tmp.Add("System.Collections.IList", "list");
            tmp.Add("System.EventHandler", "NMF.python.EventHandler");       
            return tmp;
        }

        /*uses a depth first search approach to find all used variable types in the given compile unit
         *and replaces them with their python counter part as prescribed in net2PythonTypes dictonary.
         *Also removes all interfaces, Types that end with "Proxy" and methods that are listed in 
         *unwanted methods         
         */
        public static CodeCompileUnit replaceNetVariableTypesWithNativeVaribaleTypes(CodeCompileUnit e)
        {                        
            for(int i = 0; i < e.Namespaces.Count; i++)
            {
                CodeNamespace currentNamespace = e.Namespaces[i];
                List<CodeTypeDeclaration> removeLater = new List<CodeTypeDeclaration>();             
                for(int j = 0; j < currentNamespace.Types.Count; j++)
                {                    
                    if (currentNamespace.Types[j].IsInterface)
                    {
                        Console.WriteLine("Removing interface " + currentNamespace.Types[j].Name);
                        removeLater.Add(currentNamespace.Types[j]);
                        continue;                        
                    } 
                    
                    currentNamespace.Types[j] = replaceVariableTypesInType(currentNamespace.Types[j]);
                }
                foreach (CodeTypeDeclaration ctd in removeLater) {
                    currentNamespace.Types.Remove(ctd);
                }
            }            
            return e;
        }
        
        /*called for each type there is by replaceNetVariableTypesWithNativeVariableTypes*/
        public static CodeTypeDeclaration replaceVariableTypesInType(CodeTypeDeclaration e)
        {
            e = replaceInterfaceParameters(e);
            replaceVariableTypesInMembers(e.Members);
            for(int i = 0; i < e.BaseTypes.Count; i++)
            {
                e.BaseTypes[i] = replaceDeepVariableTypesInTypeReference(e.BaseTypes[i]);
            }
            return e;
        }

        /*Interfaces do not exist in python, however multi inheritance does. Instead of
         using interfaces, use multi intheritance.*/
        public static CodeTypeDeclaration replaceInterfaceParameters(CodeTypeDeclaration e)
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
            return e;
        }

        public static CodeTypeMemberCollection replaceVariableTypesInMembers(CodeTypeMemberCollection members)
        {
            bool hasConstructor = false;
            List<CodeTypeMember> removeLater = new List<CodeTypeMember>(); //removeLater or indeces would shift
            for(int i = 0; i < members.Count; i++)
            {
                CodeTypeMember current = members[i];                
                if (unwantedMethods.Contains(current.Name))
                {
                    Console.WriteLine("Removing method " + current.Name);
                    removeLater.Add(current);
                } else if(current is CodeTypeDeclaration)
                {
                    //Console.WriteLine("Found nested type: " + current.Name);
                    if (current.Name.EndsWith("Proxy"))
                    {
                        Console.WriteLine("Removing nested type " + current.Name + " since it's a proxy!");
                        removeLater.Add(current);
                    } else
                    {
                        members[i] = replaceVariableTypesInType((CodeTypeDeclaration)current);
                    }                    
                } else if (current is CodeMemberField)
                {                    
                    ((CodeMemberField) members[i]).Type = replaceDeepVariableTypesInTypeReference(((CodeMemberField)current).Type);
                } else if(current is CodeMemberProperty)
                {
                    ((CodeMemberProperty)members[i]).Type = replaceDeepVariableTypesInTypeReference(((CodeMemberProperty)current).Type);
                } else if (current is CodeMemberMethod)
                {
                    members[i] = replaceDeepVaribleTypesInCodeMemberMethod((CodeMemberMethod)members[i]);
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
            return members;
        }

        public static CodeMemberMethod replaceDeepVaribleTypesInCodeMemberMethod(CodeMemberMethod method)
        {                                    
            //ImplementationTypes
            for (int i = 0; i < method.ImplementationTypes.Count; i++)
            {
                method.ImplementationTypes[i] = replaceDeepVariableTypesInTypeReference(method.ImplementationTypes[i]);
            }

            //Parameters
            for(int i = 0; i < method.Parameters.Count; i++)
            {
                method.Parameters[i].Type = replaceDeepVariableTypesInTypeReference(method.Parameters[i].Type);
            }

            //PrivateImplementationType
            if (method.PrivateImplementationType != null)
            {
                method.PrivateImplementationType = replaceDeepVariableTypesInTypeReference(method.PrivateImplementationType);
            }            

            //ReturnType
            if (method.ReturnType != null)
            {
                method.ReturnType = replaceDeepVariableTypesInTypeReference(method.ReturnType);
            }            

            //ReturnTypeCustomAttributes
            /*for(int i = 0; i < method.ReturnTypeCustomAttributes.Count; i++)
            {
                method.ReturnTypeCustomAttributes[i].AttributeType = replaceDeepVariableTypesInTypeReference(method.ReturnTypeCustomAttributes[i].AttributeType);
            }*/
            return method;
        }

        public static CodeTypeReference replaceDeepVariableTypesInTypeReference(CodeTypeReference tr)
        {
            //python variables are nullable by default => remove it 
            if (tr.BaseType.Contains("Nullable"))
            {
                if(tr.TypeArguments.Count != 1)
                {
                    throw new Exception("Fatal Error: Unkown Nullable Generic");
                }
                return replaceDeepVariableTypesInTypeReference(tr.TypeArguments[0]);
            }
            else
            {
                tr.BaseType = replaceTypeString(tr.BaseType);

                for (int i = 0; i < tr.TypeArguments.Count; i++)
                {
                    tr.TypeArguments[i] = replaceDeepVariableTypesInTypeReference(tr.TypeArguments[i]);
                }
                return tr;
            }            
        }

        static string replaceTypeString(string type)
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
        

        //remove Imports which contain System
        //these imports are part of the .Net platform so they can't be resolved in a normal
        //python environment anyway
        public static void removeNetSystemImports(ref CodeCompileUnit e)
        {            
            foreach (CodeNamespace ns in e.Namespaces)
            {
                CodeNamespaceImportCollection cleaned = new CodeNamespaceImportCollection();
                foreach (CodeNamespaceImport import in ns.Imports)
                {
                    if (!import.Namespace.StartsWith("System"))
                    {
                        cleaned.Add(import);
                    }
                }
                ns.Imports.Clear();
                foreach (CodeNamespaceImport import in cleaned)
                {
                    ns.Imports.Add(import);
                }
            }
        }
    }
}
