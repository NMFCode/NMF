using NMF.CodeGen;
using NMF.Models.Meta;
using NMF.Models.Repository;
using NMF.Transformations;
using NMF.Transformations.Core;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NMF.Models.Meta
{
    /// <summary>
    /// The transformation to transform an NMeta metamodel to code
    /// </summary>
    public partial class Meta2ClassesTransformation : ReflectiveTransformation
    {
        private static CodeTypeReference CreateTypeReference(ITypedElement typedElement, System.Action<CodeAttributeDeclaration> attributePersistor, System.Func<ITypedElement, CodeTypeReference, ITransformationContext, CodeTypeReference> getCollectionType, ITransformationContext context)
        {
            if (typedElement == null) return new CodeTypeReference(typeof(void));
            CodeTypeReference elementType;
            var systemType = typedElement.Type as IPrimitiveType;
            if (systemType != null)
            {
                elementType = new CodeTypeReference(systemType.SystemType);
            }
            else
            {
                var type = typedElement.Type;
                bool isReference;
                if (type != null)
                {
                    isReference = type is IReferenceType;
                }
                else
                {
                    isReference = typedElement is IReference;
                }
                elementType = CreateReference(typedElement.Type, isReference, context);
            }
            if (typedElement.UpperBound == 1)
            {
                return elementType;
            }
            else
            {
                if (attributePersistor != null)
                {
                    if (typedElement.LowerBound > 0) attributePersistor(new CodeAttributeDeclaration(typeof(LowerBoundAttribute).ToTypeReference(), new CodeAttributeArgument(new CodePrimitiveExpression(typedElement.LowerBound))));
                    if (typedElement.UpperBound > 1) attributePersistor(new CodeAttributeDeclaration(typeof(UpperBoundAttribute).ToTypeReference(), new CodeAttributeArgument(new CodePrimitiveExpression(typedElement.UpperBound))));
                }
                if (getCollectionType == null)
                {
                    return new CodeTypeReference(typeof(IEnumerable<>).Name, elementType);
                }
                else
                {
                    return getCollectionType(typedElement, elementType, context);
                }
            }
        }


        /// <summary>
        /// Gets the default imported system namespaces
        /// </summary>
        public static IEnumerable<string> DefaultSystemImports
        {
            get
            {
                yield return "System";
                yield return "System.Collections";
                yield return "System.Collections.Generic";
                yield return "System.Collections.ObjectModel";
                yield return "System.ComponentModel";
                yield return "System.Diagnostics";
                yield return "System.Linq";
                yield return "NMF.Expressions";
                yield return "NMF.Expressions.Linq";
                yield return "NMF.Models";
                yield return "NMF.Models.Collections";
                yield return "NMF.Models.Expressions";
                yield return "NMF.Collections.Generic";
                yield return "NMF.Collections.ObjectModel";
                yield return "NMF.Serialization";
                yield return "NMF.Utilities";
            }
        }


        /// <summary>
        /// Gets the imported system namespaces
        /// </summary>
        public virtual IEnumerable<string> SystemImports
        {
            get
            {
                return DefaultSystemImports;
            }
        }


        /// <summary>
        /// Creates a reference to the given NMeta type
        /// </summary>
        /// <param name="type">The NMeta type</param>
        /// <param name="isReference">A value indicating whether to default to IModelElement or object</param>
        /// <param name="context">The transformation context</param>
        /// <returns>A code type reference</returns>
        protected static CodeTypeReference CreateReference(IType type, bool isReference, ITransformationContext context)
        {
            if (type != null)
            {
                var declaration = context.Trace.ResolveIn((Type2Type)context.Transformation.GetRuleForRuleType(typeof(Type2Type)), type);
                if (declaration != null)
                {
                    return CodeDomHelper.GetReferenceForType(declaration);
                }
            }
            var primitiveType = type as IPrimitiveType;
            if (primitiveType != null)
            {
                return new CodeTypeReference(primitiveType.SystemType);
            }
            if (isReference)
            {
                return typeof(IModelElement).ToTypeReference();
            }
            else
            {
                return new CodeTypeReference(typeof(object));
            }
        }

        private static bool IsString(IType type)
        {
            var primitiveType = type as IPrimitiveType;
            return primitiveType != null && primitiveType.SystemType == typeof(string).FullName;
        }

        /// <summary>
        /// Decides whether the given type is a value type
        /// </summary>
        /// <param name="type">The NMeta type</param>
        /// <returns>True, if the type is represented as a value type. Override for more specifics on primitives</returns>
        protected virtual bool IsValueType(IType type)
        {
            if (type is IDataType || type is IEnumeration) return true;
            var primitive = type as IPrimitiveType;
            if (primitive != null)
            {
                switch (primitive.SystemType)
                {
                    case "System.String":
                    case "System.Object":
                    case "String":
                    case "Object":
                    case "System.Uri":
                    case "Uri":
                    case "System.Type":
                        return false;
                    default:
                        return true;
                }
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Gets the resource key for the given model element
        /// </summary>
        /// <param name="model">The model for which to generate a resource key</param>
        /// <returns>The resource key of the model</returns>
        protected virtual string GetResourceKey(Model model)
        {
            throw new NotImplementedException();
        }

        private bool createOperations = true;
        private string defaultNamespace;
        private bool separateImplementations = true;
        private bool onlyNested = false;

        /// <summary>
        /// Gets or sets a value indicating whether to generate operations
        /// </summary>
        /// <remarks>This value can only be set before the transformation is initialized</remarks>
        public bool CreateOperations
        {
            get
            {
                return createOperations;
            }
            set
            {
                if (IsInitialized) throw new System.InvalidOperationException();
                createOperations = value;
            }
        }

        /// <summary>
        /// Gets or sets the default namespace for the generated code
        /// </summary>
        /// <remarks>This value can only be set before the transformation is initialized</remarks>
        public string DefaultNamespace
        {
            get
            {
                return defaultNamespace;
            }
            set
            {
                defaultNamespace = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to separate the class implementations, i.e. create a public interface or not
        /// </summary>
        /// <remarks>This value can only be set before the transformation is initialized</remarks>
        public bool SeparateImplementations
        {
            get
            {
                return separateImplementations;
            }
            set
            {
                if (IsInitialized) throw new System.InvalidOperationException();
                separateImplementations = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether only nested namespaces of an initial namespace should be in the code compile unit or all that are used
        /// </summary>
        /// <remarks>This value can only be set before the transformation is initialized</remarks>
        public bool OnlyNested
        {
            get
            {
                return onlyNested;
            }
            set
            {
                if (IsInitialized) throw new System.InvalidOperationException();
                onlyNested = value;
            }
        }
    }
}
