using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using NMF.Utilities;

namespace NMF.CodeGen
{
    /// <summary>
    /// A helper class to generate code using CodeDOM
    /// </summary>
    public static class CodeDomHelper
    {
        internal static object ShadowKey = new object();
        internal static object DependentMembersKey = new object();
        internal static object DependentTypesKey = new object();
        internal static object ConstructorStatementsKey = new object();
        internal static object ConstructorKey = new object();
        internal static object InterfaceKey = new object();
        internal static object TypeReferenceKey = new object();
        internal static object NamespaceKey = new object();
        internal static object ClassKey = new object();
        internal static object BackingFieldKey = new object();
        internal static object BackingFieldRefKey = new object();
        internal static object MergeKey = new object();

        /// <summary>
        /// Gets the namespace associated with the given type reference
        /// </summary>
        /// <param name="typeReference">The type reference</param>
        /// <returns>The namespace associated with the type reference if any</returns>
        public static string Namespace(this CodeTypeReference typeReference)
        {
            return GetOrCreateUserItem<string>(typeReference, NamespaceKey);
        }

        /// <summary>
        /// Associates the given type reference with the given namespace
        /// </summary>
        /// <param name="typeReference">The type reference</param>
        /// <param name="necessaryNamespace">The namespace to be associated with the given type reference</param>
        public static void SetNamespace(this CodeTypeReference typeReference, string necessaryNamespace)
        {
            SetUserItem(typeReference, NamespaceKey, necessaryNamespace);
        }

        /// <summary>
        /// Gets the dependent types of a code member
        /// </summary>
        /// <param name="item">The code member</param>
        /// <param name="createIfNecessary">True, if the collection of dependent types should be created if not yet existing</param>
        /// <returns>The collection of dependent types or null</returns>
        public static ICollection<CodeTypeDeclaration> DependentTypes(this CodeTypeMember item, bool createIfNecessary)
        {
            return CodeDomHelper.GetUserCollection<CodeTypeDeclaration>(item, DependentTypesKey, createIfNecessary);
        }

        /// <summary>
        /// Gets the dependent members of a code member
        /// </summary>
        /// <param name="item">The code member</param>
        /// <param name="createIfNecessary">True, if the collection of dependent members should be created if not yet existing</param>
        /// <returns>The collection of dependent members</returns>
        public static ICollection<CodeTypeMember> DependentMembers(this CodeTypeMember item, bool createIfNecessary)
        {
            return CodeDomHelper.GetUserCollection<CodeTypeMember>(item, DependentMembersKey, createIfNecessary);
        }

        /// <summary>
        /// Gets the implied constructor statements of the given code member
        /// </summary>
        /// <param name="item">The code member</param>
        /// <param name="createIfNecessary">True, if the collection of implied statements should be created if not yet existing</param>
        /// <returns>The collection of implied constructor statements</returns>
        public static ICollection<CodeStatement> ImpliedConstructorStatements(this CodeTypeMember item, bool createIfNecessary)
        {
            return GetUserCollection<CodeStatement>(item, ConstructorStatementsKey, createIfNecessary);
        }

        /// <summary>
        /// Gets the implied default constructor statements of the given code member
        /// </summary>
        /// <param name="item">The code member</param>
        /// <param name="createIfNecessary">True, if the collection of implied constructor statements should be created if not yet existing</param>
        /// <returns>The collection of implied constructor statements</returns>
        public static ICollection<CodeStatement> ImpliedConstructorStatementsInternal(CodeTypeMember item, bool createIfNecessary)
        {
            return CodeDomHelper.GetUserCollection<CodeStatement>(item, ConstructorStatementsKey, createIfNecessary);
        }

        /// <summary>
        /// Gets a collection of members that are being shadowed by the given code member
        /// </summary>
        /// <param name="item">The code member</param>
        /// <param name="createIfNecessary">True, if the collection should be created if not yet existing</param>
        /// <returns>The collection of shadowed members or null</returns>
        public static ICollection<CodeTypeMember> Shadows(this CodeTypeMember item, bool createIfNecessary)
        {
            return GetUserCollection<CodeTypeMember>(item, ShadowKey, createIfNecessary);
        }

        /// <summary>
        /// Gets the type reference associated with the given code type declaration
        /// </summary>
        /// <param name="typeDeclaration">The code type declaration</param>
        /// <returns>The type reference associated with the given type</returns>
        public static CodeTypeReference GetReferenceForType(this CodeTypeDeclaration typeDeclaration)
        {
            return GetOrCreateUserItem<CodeTypeReference>(typeDeclaration, TypeReferenceKey);
        }

        /// <summary>
        /// Gets the type declaration associated with the given code type reference
        /// </summary>
        /// <param name="typeReference">The code type reference</param>
        /// <returns>The code type declaration associated with the given code type reference</returns>
        public static CodeTypeDeclaration GetTypeForReference(this CodeTypeReference typeReference)
        {
            return GetOrCreateUserItem<CodeTypeDeclaration>(typeReference, ClassKey);
        }

        /// <summary>
        /// Creates a type declaration with a reference attached to it
        /// </summary>
        /// <param name="name">The initial name of the type declaration</param>
        /// <returns>The generated type declaration</returns>
        public static CodeTypeDeclaration CreateTypeDeclarationWithReference(string name)
        {
            var typeDeclaration = new CodeTypeDeclaration()
            {
                Name = name,
                Attributes = MemberAttributes.Public
            };
            var reference = new CodeTypeReference(name);
            SetUserItem(typeDeclaration, TypeReferenceKey, reference);
            SetUserItem(reference, ClassKey, typeDeclaration);
            return typeDeclaration;
        }

        /// <summary>
        /// Gets the user collection associated with the given code object
        /// </summary>
        /// <typeparam name="TValue">The type of the collection elements</typeparam>
        /// <param name="item">The code object</param>
        /// <param name="key">The user key for retrieving the collection</param>
        /// <param name="createIfNecessary">True, if the collection should be created if not yet existing</param>
        /// <returns>The user collection or null</returns>
        public static List<TValue> GetUserCollection<TValue>(this CodeObject item, object key, bool createIfNecessary)
        {
            Func<List<TValue>> listCreator = null;
            if (createIfNecessary) listCreator = () => new List<TValue>();
            return GetOrCreateUserItem<List<TValue>>(item, key, listCreator);
        }

        /// <summary>
        /// Gets or creates the user item with the specified key
        /// </summary>
        /// <typeparam name="TValue">The type of the user item</typeparam>
        /// <param name="item">The code object</param>
        /// <param name="key">The key for the user item</param>
        /// <param name="valueCreator">A method that creates the default value if the user item does not yet exist or null, if no user item should be created</param>
        /// <returns>The user item with the specified key</returns>
        public static TValue GetOrCreateUserItem<TValue>(this CodeObject item, object key, Func<TValue> valueCreator = null)
            where TValue : class
        {
            if (item == null) throw new ArgumentNullException("item");

            if (item.UserData.Contains(key))
            {
                return item.UserData[key] as TValue;
            }
            else
            {
                if (valueCreator == null)
                {
                    return default(TValue);
                }
                else
                {
                    var val = valueCreator();
                    item.UserData.Add(key, val);
                    return val;
                }
            }
        }

        /// <summary>
        /// Overrides the user item with the given key
        /// </summary>
        /// <param name="item">The code object</param>
        /// <param name="key">The user item key</param>
        /// <param name="value">The value to set</param>
        public static void SetUserItem(this CodeObject item, object key, object value)
        {
            if (item == null) throw new ArgumentNullException("item");

            if (item.UserData.Contains(key))
            {
                item.UserData[key] = value;
            }
            else
            {
                item.UserData.Add(key, value);
            }
        }

        /// <summary>
        /// Creates a method that raises the given event
        /// </summary>
        /// <param name="memberEvent">The event that is to be raised</param>
        /// <param name="eventDataType">The event arguments type</param>
        /// <returns>A code method that raises the event</returns>
        public static CodeMemberMethod CreateOnChangedMethod(this CodeMemberEvent memberEvent, CodeTypeReference eventDataType)
        {
            var onChangedMethod = new CodeMemberMethod()
            {
                Name = "On" + memberEvent.Name,
                ReturnType = null,
                Attributes = MemberAttributes.Family
            };
            onChangedMethod.Parameters.Add(new CodeParameterDeclarationExpression(eventDataType, "eventArgs"));

            onChangedMethod.Statements.Add(new CodeVariableDeclarationStatement(memberEvent.Type, "handler",
                new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), memberEvent.Name)));

            var handler = new CodeVariableReferenceExpression("handler");
            var ifHandlerNotNull = new CodeConditionStatement();
            ifHandlerNotNull.Condition = new CodeBinaryOperatorExpression(
                handler, CodeBinaryOperatorType.IdentityInequality, new CodePrimitiveExpression(null));
            ifHandlerNotNull.TrueStatements.Add(new CodeMethodInvokeExpression(
                handler, "Invoke", new CodeThisReferenceExpression(), new CodeArgumentReferenceExpression("eventArgs")));

            onChangedMethod.Statements.Add(ifHandlerNotNull);

            onChangedMethod.WriteDocumentation(string.Format("Raises the {0} event", memberEvent.Name), null, new Dictionary<string, string>() {
                {
                    "eventArgs", "The event data"
                }
            });
            return onChangedMethod;
        }



        /// <summary>
        /// Adds a statement to throw an exception of the given type
        /// </summary>
        /// <typeparam name="TException">The type of the exception</typeparam>
        /// <param name="method"></param>
        public static void ThrowException<TException>(this CodeMemberMethod method, params object[] arguments)
        {
            CodeExpression[] codeArguments = null;
            if (arguments != null)
            {
                codeArguments = new CodeExpression[arguments.Length];
                for (int i = 0; i < arguments.Length; i++)
                {
                    var expr = arguments[i] as CodeExpression;
                    if (expr == null)
                    {
                        expr = CreateCodeExpression(arguments[i]);
                    }
                    codeArguments[i] = expr;
                }
            }
            method.Statements.Add(new CodeThrowExceptionStatement(new CodeObjectCreateExpression(new CodeTypeReference(typeof(TException).Name), codeArguments)));
        }

        private static CodeExpression CreateCodeExpression(object obj)
        {
            if (obj == null)
            {
                return new CodePrimitiveExpression();
            }
            var type = obj.GetType();
            if (type.IsEnum)
            {
                if (Enum.IsDefined(type, obj))
                {
                    return new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(type.Name), Enum.GetName(type, obj));
                }
            }
            else if (type == typeof(Type))
            {
                return new CodeTypeOfExpression(obj as Type);
            }
            return new CodePrimitiveExpression(obj);
        }

        /// <summary>
        /// Adds an attribute of the given attribute type to the given code member
        /// </summary>
        /// <param name="member">The code member</param>
        /// <param name="attributeType">The type of the attribute</param>
        /// <param name="arguments">The arguments of the attribute, can either be primitive values, code expressions or code attribute arguments</param>
        public static void AddAttribute(this CodeTypeMember member, Type attributeType, params object[] arguments)
        {
            member.CustomAttributes.Add(DeclareAttribute(attributeType, arguments));
        }

        internal static CodeAttributeDeclaration DeclareAttribute(Type attributeType, object[] arguments)
        {
            var decl = new CodeAttributeDeclaration(attributeType.Name);
            if (arguments != null)
            {
                for (int i = 0; i < arguments.Length; i++)
                {
                    var attrExpr = arguments[i] as CodeAttributeArgument;
                    if (attrExpr == null)
                    {
                        var expr = arguments[i] as CodeExpression;
                        if (expr == null)
                        {
                            expr = CreateCodeExpression(arguments[i]);
                        }
                        attrExpr = new CodeAttributeArgument(expr);
                    }
                    decl.Arguments.Add(attrExpr);
                }
            }
            return decl;
        }

        /// <summary>
        /// Creates a primitive expression from the serialized value
        /// </summary>
        /// <param name="value">The serialized value</param>
        /// <param name="type">The type for the primitive expression</param>
        /// <returns>A code expression that represents the value as a code expression or null if there is no such expression</returns>
        public static CodeExpression CreatePrimitiveExpression(string value, CodeTypeReference type)
        {
            if (value == null) return new CodePrimitiveExpression(null);
            if (type == null) throw new ArgumentNullException("type");

            if (type.BaseType.Contains("Nullable`1") && type.TypeArguments.Count == 1) type = type.TypeArguments[0];
            if (type == null) return null;
            if (type.BaseType == typeof(int).FullName)
            {
                int val;
                if (int.TryParse(value, out val))
                {
                    return new CodePrimitiveExpression(val);
                }
            }
            else if (type.BaseType == typeof(string).FullName)
            {
                return new CodePrimitiveExpression(value);
            }
            else if (type.BaseType == typeof(bool).FullName)
            {
                bool val;
                if (bool.TryParse(value, out val))
                {
                    return new CodePrimitiveExpression(val);
                }
            }
            else if (type.BaseType == typeof(double).FullName)
            {
                double val;
                if (double.TryParse(value, out val))
                {
                    return new CodePrimitiveExpression(val);
                }
            }
            else if (type.BaseType == typeof(float).FullName)
            {
                float val;
                if (float.TryParse(value, out val))
                {
                    return new CodePrimitiveExpression(val);
                }
            }
            else if (type.BaseType == typeof(long).FullName)
            {
                long val;
                if (long.TryParse(value, out val))
                {
                    return new CodePrimitiveExpression(val);
                }
            }
            return null;
        }

        /// <summary>
        /// Generates code to validate that the given argument is not null
        /// </summary>
        /// <param name="method">The method that should be checked</param>
        /// <param name="parameterName">The parameter name</param>
        public static void ValidateArgument(this CodeMemberMethod method, string parameterName)
        {
            var arg = new CodeArgumentReferenceExpression(parameterName);
            var notNull = new CodeBinaryOperatorExpression(arg, CodeBinaryOperatorType.IdentityInequality, new CodePrimitiveExpression(null));
            var exception = new CodeObjectCreateExpression(typeof(ArgumentNullException).Name, new CodePrimitiveExpression(parameterName));
            method.Statements.Add(new CodeConditionStatement(notNull, new CodeThrowExceptionStatement(exception)));
        }

        /// <summary>
        /// Generates an OnChanged-pattern for the given property
        /// </summary>
        /// <param name="property">The code property</param>
        /// <returns>The statement to call the OnChanged method for the given property</returns>
        public static CodeStatement CreateOnChangedEventPattern(this CodeMemberProperty property)
        {
            return CreateOnChangedEventPattern(property, null, null);
        }

        /// <summary>
        /// Generates an OnChanging-pattern for the given property
        /// </summary>
        /// <param name="property">The code property</param>
        /// <returns></returns>
        public static CodeStatement CreateOnChangingEventPattern(this CodeMemberProperty property)
        {
            var changingEvent = new CodeMemberEvent()
            {
                Name = property.Name + "Changing",
                Type = new CodeTypeReference(typeof(EventHandler).Name),
                Attributes = MemberAttributes.Public | MemberAttributes.Final
            };

            changingEvent.WriteDocumentation(string.Format("Gets fired before the {0} property changes its value", property.Name));

            var eventType = new CodeTypeReference(typeof(EventArgs).Name);
            var eventData = new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(eventType), "Empty");

            var onChanging = CreateOnChangedMethod(changingEvent, eventType);

            var dependent = property.DependentMembers(true);

            dependent.Add(onChanging);
            dependent.Add(changingEvent);

            var onChangedRef = new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), onChanging.Name);

            return new CodeExpressionStatement(new CodeMethodInvokeExpression(onChangedRef, eventData));
        }

        /// <summary>
        /// Generates an OnChanged-pattern for the given property
        /// </summary>
        /// <param name="property">The code property</param>
        /// <returns>The statement to call the OnChanged method for the given property</returns>
        public static CodeStatement CreateOnChangedEventPattern(this CodeMemberProperty property, CodeTypeReference eventType, CodeExpression eventData)
        {
            CodeTypeReference handlerType;
            if (eventType == null)
            {
                handlerType = new CodeTypeReference(typeof(EventHandler).Name);
            }
            else
            {
                handlerType = new CodeTypeReference(typeof(EventHandler<>).Name, eventType);
            }
            var changedEvent = new CodeMemberEvent()
            {
                Name = property.Name + "Changed",
                Type = handlerType,
                Attributes = MemberAttributes.Public | MemberAttributes.Final
            };

            changedEvent.WriteDocumentation(string.Format("Gets fired when the {0} property changed its value", property.Name));

            if (eventType == null)
            {
                eventType = new CodeTypeReference(typeof(EventArgs).Name);
                eventData = new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(eventType), "Empty");
            }
            var onChanged = CodeDomHelper.CreateOnChangedMethod(changedEvent, eventType);

            var dependent = property.DependentMembers(true);

            dependent.Add(onChanged);
            dependent.Add(changedEvent);

            var onChangedRef = new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), onChanged.Name);

            return new CodeExpressionStatement(new CodeMethodInvokeExpression(onChangedRef, eventData));
        }

        /// <summary>
        /// Creates a backing field for the given property
        /// </summary>
        /// <param name="property">The code property</param>
        /// <returns>A reference to the generated backing field</returns>
        public static CodeFieldReferenceExpression CreateBackingField(this CodeMemberProperty property)
        {
            return CreateBackingField(property, property.Type, null);
        }

        public static CodeFieldReferenceExpression GetBackingField(this CodeMemberProperty property)
        {
            return property.GetOrCreateUserItem<CodeFieldReferenceExpression>(BackingFieldRefKey);
        }

        /// <summary>
        /// Creates a backing field for the given property
        /// </summary>
        /// <param name="property">The code property</param>
        /// <param name="type">The type of the property</param>
        /// <param name="initialValue">The initial value expression for the field</param>
        /// <returns>A reference to the generated backing field</returns>
        public static CodeFieldReferenceExpression CreateBackingField(this CodeMemberProperty property, CodeTypeReference type, CodeExpression initialValue)
        {
            var reference = CodeDomHelper.GetOrCreateUserItem<CodeFieldReferenceExpression>(property, CodeDomHelper.BackingFieldKey);
            if (reference == null)
            {

                var field = new CodeMemberField()
                {
                    Attributes = MemberAttributes.Private,
                    Name = "_" + property.Name.ToCamelCase(),
                    Type = type ?? new CodeTypeReference(typeof(object))
                };

                field.WriteDocumentation(string.Format("The backing field for the {0} property", property.Name));

                property.DependentMembers(true).Add(field);

                if (initialValue != null)
                {
                    field.InitExpression = initialValue;
                }

                reference = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), field.Name);
                CodeDomHelper.SetUserItem(property, CodeDomHelper.BackingFieldRefKey, reference);
                CodeDomHelper.SetUserItem(property, CodeDomHelper.BackingFieldKey, field);
            }
            return reference;
        }

        /// <summary>
        /// Marks the given code property as a collection property
        /// </summary>
        /// <param name="property">The property</param>
        public static void MarkCollectionProperty(this CodeMemberProperty property)
        {
            property.AddAttribute(typeof(DesignerSerializationVisibilityAttribute), DesignerSerializationVisibility.Content);
        }

        /// <summary>
        /// Implements the getter of the given property
        /// </summary>
        /// <param name="property">The property</param>
        /// <param name="fieldRef">A reference to the underlying field</param>
        public static void ImplementGetter(this CodeMemberProperty property, CodeFieldReferenceExpression fieldRef)
        {
            property.GetStatements.Add(new CodeMethodReturnStatement(fieldRef));
            property.HasGet = true;
        }

        /// <summary>
        /// Implements the setter of the given property
        /// </summary>
        /// <param name="property">The code property</param>
        /// <param name="fieldRef">A reference to the underlying field</param>
        /// <param name="whenChanged">A collection of statements that should be performed when the value of the property changed</param>
        public static void ImplementSetter(this CodeMemberProperty property, CodeFieldReferenceExpression fieldRef, params CodeStatement[] whenChanged)
        {
            ImplementSetter(property, fieldRef, (IEnumerable<CodeStatement>)whenChanged);
        }

        /// <summary>
        /// Implements the setter of the given property
        /// </summary>
        /// <param name="property">The code property</param>
        /// <param name="fieldRef">A reference to the underlying field</param>
        /// <param name="whenChanged">A collection of statements that should be performed when the value of the property changed</param>
        public static void ImplementSetter(this CodeMemberProperty property, CodeFieldReferenceExpression fieldRef, IEnumerable<CodeStatement> whenChanged)
        {
            var assign = new CodeAssignStatement(fieldRef, new CodePropertySetValueReferenceExpression());

            if (whenChanged == null || !whenChanged.Any())
            {
                property.SetStatements.Add(assign);
            }
            else
            {
                var ifStmt = new CodeConditionStatement();
                ifStmt.Condition = new CodeBinaryOperatorExpression(new CodePropertySetValueReferenceExpression(), CodeBinaryOperatorType.IdentityInequality, fieldRef);
                ifStmt.TrueStatements.Add(assign);
                foreach (var stmt in whenChanged)
                {
                    ifStmt.TrueStatements.Add(stmt);
                }
                property.SetStatements.Add(ifStmt);
            }

            property.HasSet = true;
        }

        /// <summary>
        /// Validate all arguments to be not null
        /// </summary>
        /// <param name="method">The method whose arguments should be validated</param>
        public static void ValidateArguments(this CodeMemberMethod method)
        {
            foreach (CodeParameterDeclarationExpression par in method.Parameters)
            {
                ValidateArgument(method, par.Name);
            }
        }

        /// <summary>
        /// Splits the given code compile unit in multiple compile units to separate each type in its own compile unit
        /// </summary>
        /// <param name="unit">The code compile unit</param>
        /// <returns>A dictionary of full type names and code compile units that only contain this single type</returns>
        public static IDictionary<string, CodeCompileUnit> SplitCompileUnit(CodeCompileUnit unit)
        {
            var dict = new Dictionary<string, CodeCompileUnit>();
            string baseNamespace = unit.Namespaces.Cast<CodeNamespace>().Where(n => !string.IsNullOrEmpty(n.Name)).First().Name;
            CodeNamespace globalNamespace = null;
            foreach (CodeNamespace ns in unit.Namespaces)
            {
                if (string.IsNullOrEmpty(ns.Name))
                {
                    globalNamespace = ns;
                    continue;
                }
                if (ns.Name.StartsWith(baseNamespace)) continue;
                if (baseNamespace.StartsWith(ns.Name))
                {
                    baseNamespace = ns.Name;
                }
                else
                {
                    var commonLength = 0;
                    while (true)
                    {
                        var idx1 = baseNamespace.IndexOf('.', commonLength + 1);
                        var idx2 = ns.Name.IndexOf('.', commonLength + 1);
                        if (idx1 == -1 || idx2 == -1 || idx1 != idx2)
                        {
                            break;
                        }
                        idx1 -= commonLength;
                        if (baseNamespace.Substring(commonLength, idx1) != ns.Name.Substring(commonLength, idx1))
                        {
                            break;
                        }
                        commonLength = idx2;
                    }
                    baseNamespace = baseNamespace.Substring(0, commonLength);
                }
            }
            int offset = baseNamespace.Length;
            if (offset != 0) offset++;
            foreach (CodeNamespace ns in unit.Namespaces)
            {
                if (string.IsNullOrEmpty(ns.Name)) continue;
                foreach (CodeTypeDeclaration type in ns.Types)
                {
                    var newUnit = new CodeCompileUnit();
                    if (globalNamespace != null)
                    {
                        newUnit.Namespaces.Add(globalNamespace);
                    }
                    var newNamespace = new CodeNamespace();
                    newNamespace.Name = ns.Name;
                    for (int i = 0; i < ns.Imports.Count; i++)
                    {
                        newNamespace.Imports.Add(ns.Imports[i]);
                    }
                    newUnit.Namespaces.Add(newNamespace);
                    newNamespace.Types.Add(type);
                    string fileName = string.Empty;
                    if (ns.Name != baseNamespace)
                    {
                        fileName = ns.Name.Substring(offset).Replace('.', '\\') + "\\";
                    }
                    dict.Add(fileName + type.Name, newUnit);
                }
            }
            return dict;
        }

        /// <summary>
        /// Writes documentation for the given code member
        /// </summary>
        /// <param name="member">The code member</param>
        /// <param name="summary">The summary documentation</param>
        /// <param name="remarks">The documentation remarks</param>
        public static void WriteDocumentation(this CodeTypeMember member, string summary, string remarks = null)
        {
            var generateDoc = false;
            if (!string.IsNullOrEmpty(remarks))
            {
                remarks = "\r\n <remarks>" + remarks + "</remarks>";
                generateDoc = true;
            }
            else
            {
                remarks = string.Empty;
            }
            if (!string.IsNullOrEmpty(summary))
            {
                summary = string.Format("<summary>\r\n {0}\r\n </summary>", summary);
                generateDoc = true;
            }
            if (generateDoc)
            {
                member.Comments.Add(new CodeCommentStatement(summary + remarks, true));
            }
        }

        /// <summary>
        /// Writes documentation for the given code member
        /// </summary>
        /// <param name="member">The code member</param>
        /// <param name="summary">The summary documentation</param>
        /// <param name="returns">The documentation for the return value</param>
        /// <param name="parameters">The documentation for the method parameters</param>
        /// <param name="remarks">The documentation remarks</param>
        public static void WriteDocumentation(this CodeMemberMethod member, string summary, string returns, IDictionary<string, string> parameters = null, string remarks = null)
        {
            var doc = string.Empty;
            if (!string.IsNullOrEmpty(returns))
            {
                doc += string.Format("\r\n <returns>{0}</returns>", returns);
            }
            if (parameters != null)
            {
                foreach (var par in parameters)
                {
                    doc += string.Format("\r\n <param name=\"{0}\">{1}</param>", par.Key, par.Value);
                }
            }
            if (!string.IsNullOrEmpty(remarks))
            {
                doc += "\r\n <remarks>" + remarks + "</remarks>";
            }
            member.Comments.Add(new CodeCommentStatement(string.Format("<summary>\r\n {0}\r\n </summary>", summary) + doc, true));
        }

        /// <summary>
        /// Applies the given expression to the given code expressions as input
        /// </summary>
        /// <typeparam name="T1">The type of the first argument</typeparam>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="expression">The expression to be converted to CodeDOM</param>
        /// <param name="argument">The first argument</param>
        /// <returns>The CodeDOM expression that contains the equivalent code</returns>
        public static CodeExpression ApplyExpression<T1, TResult>(Expression<Func<T1, TResult>> expression, CodeExpression argument)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            var dict = new Dictionary<Expression, CodeExpression>();
            dict.Add(expression.Parameters[0], argument);

            return ApplyExpression(expression.Body, dict);
        }

        /// <summary>
        /// Applies the given expression to the given code expressions as input
        /// </summary>
        /// <typeparam name="T1">The type of the first argument</typeparam>
        /// <typeparam name="T2">The type of the second argument</typeparam>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="expression">The expression to be converted to CodeDOM</param>
        /// <param name="argument1">The first argument</param>
        /// <param name="argument2">The second argument</param>
        /// <returns>The CodeDOM expression that contains the equivalent code</returns>
        public static CodeExpression ApplyExpression<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> expression, CodeExpression argument1, CodeExpression argument2)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            var dict = new Dictionary<Expression, CodeExpression>();
            dict.Add(expression.Parameters[0], argument1);
            dict.Add(expression.Parameters[1], argument2);

            return ApplyExpression(expression.Body, dict);
        }

        /// <summary>
        /// Applies the given expression to the given code expressions as input
        /// </summary>
        /// <typeparam name="T1">The type of the first argument</typeparam>
        /// <typeparam name="T2">The type of the second argument</typeparam>
        /// <typeparam name="T3">The type of the third argument</typeparam>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="expression">The expression to be converted to CodeDOM</param>
        /// <param name="argument1">The first argument</param>
        /// <param name="argument2">The second argument</param>
        /// <param name="argument3">The third argument</param>
        /// <returns>The CodeDOM expression that contains the equivalent code</returns>
        public static CodeExpression ApplyExpression<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> expression, CodeExpression argument1, CodeExpression argument2, CodeExpression argument3)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            var dict = new Dictionary<Expression, CodeExpression>();
            dict.Add(expression.Parameters[0], argument1);
            dict.Add(expression.Parameters[1], argument2);
            dict.Add(expression.Parameters[2], argument3);

            return ApplyExpression(expression.Body, dict);
        }

        /// <summary>
        /// Applies the given expression to the given code expressions as input
        /// </summary>
        /// <typeparam name="T1">The type of the first argument</typeparam>
        /// <typeparam name="T2">The type of the second argument</typeparam>
        /// <typeparam name="T3">The type of the third argument</typeparam>
        /// <typeparam name="T4">The type of the fourth argument</typeparam>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="expression">The expression to be converted to CodeDOM</param>
        /// <param name="argument1">The first argument</param>
        /// <param name="argument2">The second argument</param>
        /// <param name="argument3">The third argument</param>
        /// <param name="argument4">The fourth argument</param>
        /// <returns>The CodeDOM expression that contains the equivalent code</returns>
        public static CodeExpression ApplyExpression<T1, T2, T3, T4, TResult>(Expression<Func<T1, T2, T3, T4, TResult>> expression, CodeExpression argument1, CodeExpression argument2, CodeExpression argument3, CodeExpression argument4)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            var dict = new Dictionary<Expression, CodeExpression>();
            dict.Add(expression.Parameters[0], argument1);
            dict.Add(expression.Parameters[1], argument2);
            dict.Add(expression.Parameters[2], argument3);
            dict.Add(expression.Parameters[3], argument4);

            return ApplyExpression(expression.Body, dict);
        }

        private static CodeExpression ApplyExpression(Expression expression, Dictionary<Expression, CodeExpression> arguments)
        {
            CodeExpression exp;
            if (arguments.TryGetValue(expression, out exp))
            {
                return exp;
            }
            switch (expression.NodeType)
            {
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                    return ApplyBinary(expression, CodeBinaryOperatorType.Add, arguments);
                case ExpressionType.AndAlso:
                case ExpressionType.And:
                    var and = expression as BinaryExpression;
                    return new CodeBinaryOperatorExpression(ApplyExpression(and.Left, arguments), and.Left.Type == typeof(bool) ? CodeBinaryOperatorType.BooleanAnd : CodeBinaryOperatorType.BitwiseAnd, ApplyExpression(and.Right, arguments));
                case ExpressionType.Assign:
                    return ApplyBinary(expression, CodeBinaryOperatorType.Assign, arguments);
                case ExpressionType.Call:
                    var call = expression as MethodCallExpression;
                    var callArguments = call.Arguments.Select(arg => ApplyExpression(arg, arguments)).ToArray();
                    if (call.Object == null)
                    {
                        return new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(call.Method.DeclaringType), call.Method.Name, callArguments);
                    }
                    else
                    {
                        return new CodeMethodInvokeExpression(ApplyExpression(call.Object, arguments), call.Method.Name, callArguments);
                    }
                case ExpressionType.Constant:
                    var constant = expression as ConstantExpression;
                    if (constant.Type == typeof(string) || constant.Type == typeof(int) || constant.Type == typeof(double) || constant.Value == null)
                        return new CodePrimitiveExpression(constant.Value);
                    break;
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    var cast = expression as UnaryExpression;
                    return new CodeCastExpression(cast.Type, ApplyExpression(cast.Operand, arguments));
                case ExpressionType.Divide:
                    return ApplyBinary(expression, CodeBinaryOperatorType.Divide, arguments);
                case ExpressionType.Equal:
                    return ApplyBinary(expression, CodeBinaryOperatorType.ValueEquality, arguments);
                case ExpressionType.GreaterThan:
                    return ApplyBinary(expression, CodeBinaryOperatorType.GreaterThan, arguments);
                case ExpressionType.GreaterThanOrEqual:
                    return ApplyBinary(expression, CodeBinaryOperatorType.GreaterThanOrEqual, arguments);
                case ExpressionType.Index:
                    var index = expression as IndexExpression;
                    return new CodeIndexerExpression(ApplyExpression(index.Object, arguments), index.Arguments.Select(arg => ApplyExpression(arg, arguments)).ToArray());
                case ExpressionType.LessThan:
                    return ApplyBinary(expression, CodeBinaryOperatorType.LessThan, arguments);
                case ExpressionType.LessThanOrEqual:
                    return ApplyBinary(expression, CodeBinaryOperatorType.LessThanOrEqual, arguments);
                case ExpressionType.MemberAccess:
                    var memberAccess = expression as MemberExpression;
                    if (memberAccess.Member.MemberType == System.Reflection.MemberTypes.Property)
                    {
                        return new CodePropertyReferenceExpression(ApplyExpression(memberAccess.Expression, arguments), memberAccess.Member.Name);
                    }
                    else if (memberAccess.Member.MemberType == System.Reflection.MemberTypes.Field)
                    {
                        return new CodeFieldReferenceExpression(ApplyExpression(memberAccess.Expression, arguments), memberAccess.Member.Name);
                    }
                    break;
                case ExpressionType.Modulo:
                    return ApplyBinary(expression, CodeBinaryOperatorType.Modulus, arguments);
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                    return ApplyBinary(expression, CodeBinaryOperatorType.Multiply, arguments);
                case ExpressionType.New:
                    break;
                case ExpressionType.NewArrayBounds:
                    break;
                case ExpressionType.NewArrayInit:
                    break;
                case ExpressionType.NotEqual:
                    return ApplyBinary(expression, CodeBinaryOperatorType.IdentityInequality, arguments);
                case ExpressionType.Or:
                    return ApplyBinary(expression, expression.Type == typeof(bool) ? CodeBinaryOperatorType.BooleanOr : CodeBinaryOperatorType.BitwiseOr, arguments);
                case ExpressionType.OrElse:
                    return ApplyBinary(expression, CodeBinaryOperatorType.BooleanOr, arguments);
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                    return ApplyBinary(expression, CodeBinaryOperatorType.Subtract, arguments);
                case ExpressionType.TypeAs:
                    break;
                case ExpressionType.TypeIs:
                    var typeIs = expression as UnaryExpression;
                    return new CodeMethodInvokeExpression(new CodeTypeOfExpression(typeIs.Type), "IsInstanceOf", ApplyExpression(typeIs.Operand, arguments));
                default:
                    break;
            }
            throw new NotSupportedException();
        }

        private static CodeExpression ApplyBinary(Expression expression, CodeBinaryOperatorType op, Dictionary<Expression, CodeExpression> arguments)
        {
            var divide = expression as BinaryExpression;
            return new CodeBinaryOperatorExpression(ApplyExpression(divide.Left, arguments), op, ApplyExpression(divide.Right, arguments));
        }

        /// <summary>
        /// Gets or creates the default constructor for the given code type declaration
        /// </summary>
        /// <param name="generatedType">The generated type declaration</param>
        /// <param name="constructorCreator">A function creating the default constructor if necessary</param>
        /// <returns>The types default constructor</returns>
        public static CodeConstructor GetOrCreateDefaultConstructor(this CodeTypeDeclaration generatedType, Func<CodeConstructor> constructorCreator = null)
        {
            return GetOrCreateUserItem<CodeConstructor>(generatedType, CodeDomHelper.ConstructorKey, constructorCreator);
        }

        /// <summary>
        /// Creates a code type reference for the given type
        /// </summary>
        /// <param name="type">The given system type for which to generate a type reference</param>
        /// <returns>A code reference with namespace set accordingly</returns>
        public static CodeTypeReference ToTypeReference(this Type type)
        {
            var reference = new CodeTypeReference(type.Name);
            reference.SetNamespace(type.Namespace);
            return reference;
        }


        /// <summary>
        /// Creates a code type reference for the given type
        /// </summary>
        /// <param name="type">The given system type for which to generate a type reference</param>
        /// <param name="parameters">The type arguments</param>
        /// <returns>A code reference with namespace set accordingly</returns>
        public static CodeTypeReference ToTypeReference(this Type type, params CodeTypeReference[] parameters)
        {
            var reference = new CodeTypeReference(type.Name, parameters);
            reference.SetNamespace(type.Namespace);
            return reference;
        }

        /// <summary>
        /// Defines the action that should be executed when the given member needs to be nerged with another member with the same name
        /// </summary>
        /// <param name="member">The code member</param>
        /// <param name="mergeAction">The action that should be performed in that case</param>
        public static void SetMerge(this CodeTypeMember member, Func<CodeTypeMember, CodeTypeMember> mergeAction)
        {
            if (mergeAction == null) throw new ArgumentNullException("mergeAction");
            member.UserData[MergeKey] = mergeAction;
        }

        /// <summary>
        /// Merges the given members
        /// </summary>
        /// <param name="member">The first code member</param>
        /// <param name="other">The second code member</param>
        /// <returns>A merged member</returns>
        public static CodeTypeMember Merge(this CodeTypeMember member, CodeTypeMember other)
        {
            if (other.UserData.Contains(MergeKey))
            {
                Func<CodeTypeMember, CodeTypeMember> mergeAction = other.UserData[MergeKey] as Func<CodeTypeMember, CodeTypeMember>;
                if (mergeAction != null)
                {
                    return mergeAction(member);
                }
            }
            if (member.UserData.Contains(MergeKey))
            {
                Func<CodeTypeMember, CodeTypeMember> mergeAction = member.UserData[MergeKey] as Func<CodeTypeMember, CodeTypeMember>;
                if (mergeAction != null)
                {
                    return mergeAction(other);
                }
            }
            throw new NotSupportedException("Neither of the given members supports merge.");
        }
    }
}
