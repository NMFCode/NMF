using NMF.Expressions;
using NMF.Models;
using NMF.Models.Meta;
using NMF.Models.Repository;
using NMF.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace NMF.Synchronizations.Models
{
    /// <summary>
    /// Denotes a transformation rule that produces exact copies of model elements
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ModelCopyRule<T> : SynchronizationRule<T, T> where T : class, IModelElement
    {
        private static readonly IClass modelClass = MetaRepository.Instance.ResolveClass(typeof(T)) as IClass;

#pragma warning disable S2743 // Static fields should not be used in generic types
        private static readonly MethodInfo singleAttribute;
        private static readonly MethodInfo multipleAttribute;
        private static readonly MethodInfo singleReference;
        private static readonly MethodInfo multipleReference;
#pragma warning restore S2743 // Static fields should not be used in generic types

#pragma warning disable S3963 // "static" fields should be initialized inline
        static ModelCopyRule()
#pragma warning restore S3963 // "static" fields should be initialized inline
        {
            var methods = typeof(ModelCopyRule<T>).GetMethods();
            singleAttribute = Array.Find(methods, m => m.Name == "Synchronize" && m.IsGenericMethodDefinition && m.GetGenericArguments().Length == 1 && m.GetParameters().Length == 2);
            singleReference = Array.Find(methods, m => m.Name == "Synchronize" && m.IsGenericMethodDefinition && m.GetGenericArguments().Length == 2 && m.GetParameters().Length == 4);
            multipleAttribute = Array.Find(methods, m => m.Name == "SynchronizeMany" && m.IsGenericMethodDefinition && m.GetGenericArguments().Length == 1 && m.GetParameters().Length == 2);
            multipleReference = Array.Find(methods, m => m.Name == "SynchronizeMany" && m.IsGenericMethodDefinition && m.GetGenericArguments().Length == 2 && m.GetParameters().Length == 3);
        }

        /// <inheritdoc />
        public override void DeclareSynchronization()
        {
            if (modelClass.BaseTypes.Count > 1)
            {
                throw new InvalidOperationException("Model synchronization rules cannot be auto-generated for classes with multiple inheritance.");
            }

            var isModelElementType = typeof(T) == typeof(IModelElement);
            if (!isModelElementType)
            {
                CreateSynchronizationBlocks();
            }

            if (!isModelElementType && modelClass.BaseTypes.Count == 0)
            {
                MarkInstantiatingFor(Synchronization.GetSynchronizationRuleForSignature(typeof(IModelElement), typeof(IModelElement)));
            }
        }

        private void CreateSynchronizationBlocks()
        {
            foreach (var att in modelClass.Attributes)
            {
                SynchronizeAttribute(att);
            }

            foreach (var r in modelClass.References)
            {
                SynchronizeReference(r);
            }

            foreach (var baseCl in modelClass.BaseTypes)
            {
                var mapping = baseCl.GetExtension<MappedType>();
                var iface = mapping.SystemType;
                MarkInstantiatingFor(Synchronization.GetSynchronizationRuleForSignature(iface, iface));
            }
        }

        private void SynchronizeReference(IReference r)
        {
            if (r.UpperBound == 1)
            {
                // Create Synchronize call
                var lambda = CreateLambdaFor(r, null);
                var rule = Synchronization.GetSynchronizationRuleForSignature(lambda.ReturnType, lambda.ReturnType);
                singleReference
                    .MakeGenericMethod(lambda.ReturnType)
                    .Invoke(this, new object[] { rule, lambda, lambda, null });
            }
            else
            {
                // Create SynchronizeMany call
                var targetType = r.ReferenceType.GetExtension<MappedType>().SystemType;
                var lambda = CreateLambdaFor(r, typeof(Func<,>).MakeGenericType(typeof(T), typeof(ICollectionExpression<>).MakeGenericType(targetType)));
                var rule = Synchronization.GetSynchronizationRuleForSignature(targetType, targetType);
                multipleReference
                    .MakeGenericMethod(targetType, targetType)
                    .Invoke(this, new object[] { rule, lambda, lambda });
            }
        }

        private void SynchronizeAttribute(IAttribute att)
        {
            if (att.UpperBound == 1)
            {
                // Create Synchronize call
                var lambda = CreateLambdaFor(att, null);
                singleAttribute
                    .MakeGenericMethod(lambda.ReturnType)
                    .Invoke(this, new object[] { lambda, lambda });
            }
            else
            {
                // Create SynchronizeMany call
                var targetType = att.Type.GetExtension<MappedType>().SystemType;
                var lambda = CreateLambdaFor(att, typeof(Func<,>).MakeGenericType(typeof(T), typeof(ICollectionExpression<>).MakeGenericType(targetType)));
                multipleAttribute
                    .MakeGenericMethod(targetType)
                    .Invoke(this, new object[] { lambda, lambda });
            }
        }

        private static LambdaExpression CreateLambdaFor(ITypedElement feature, System.Type delegateType)
        {
            var par = Expression.Parameter(typeof(T), "element");
            if (delegateType == null)
            {
                return Expression.Lambda(Expression.MakeMemberAccess(par, FindProperty(feature.Name)), par);
            }
            else
            {
                return Expression.Lambda(delegateType, Expression.MakeMemberAccess(par, FindProperty(feature.Name)), par);
            }
        }

        private static PropertyInfo FindProperty(string name)
        {
            return typeof(T).GetProperty(name.ToPascalCase());
        }
    }
}
