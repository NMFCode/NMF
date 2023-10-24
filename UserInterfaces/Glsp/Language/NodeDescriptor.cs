using NMF.Expressions;
using NMF.Synchronizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Language
{
    public abstract class NodeDescriptor<T> : DescriptorBase
    {
        private readonly Stack<GElementSkeleton<T>> _skeletons = new Stack<GElementSkeleton<T>>();
        private readonly GElementSkeleton<T> _baseSkeleton;

        public NodeDescriptor()
        {
            _baseSkeleton = CreateSkeleton();
            _skeletons.Push(_baseSkeleton);
        }

        internal virtual GElementSkeleton<T> CreateSkeleton() => new GElementSkeleton<T>();

        private GElementSkeleton<T> CurrentSkeleton => _skeletons.Peek();

        protected IDisposable Compartment(string type, string layout = "vbox", Expression<Func<T, bool>> guard = null)
        {
            var skeleton = new GElementSkeleton<T>
            {
                StaticType = type
            };
            CurrentSkeleton.ChildContributions.Add(new CompartmentContribution<T> { Compartment = skeleton, Guard = guard });
            _skeletons.Push(skeleton);
            if (layout != null)
            {
                Forward("layout", layout);
            }
            return new InnerCompartment(this);
        }

        protected void CssClass(string cssClass, Expression<Func<T, bool>> guard = null)
        {
            if (guard != null && cssClass != null)
            {
                var expression = Expression.Lambda<Func<T, string>>(
                    Expression.Condition(guard.Body, Expression.Constant(cssClass), Expression.Constant(null)),
                    guard.Parameters[0]);
                CssClass(expression);
            }
            else if (cssClass != null)
            {
                CurrentSkeleton.StaticCssClasses.Add(cssClass);
            }
        }

        protected void CssClass(Expression<Func<T, string>> selector)
        {
            if (selector != null)
            {
                CurrentSkeleton.DynamicCssClasses.Add(ObservingFunc<T, string>.FromExpression(selector));
            }
        }

        protected void Type(string type)
        {
            CurrentSkeleton.StaticType = type;
        }

        protected void Type(Expression<Func<T, string>> selector)
        {
            CurrentSkeleton.DynamicType = selector;
        }

        protected void Forward(string key, string value)
        {
            CurrentSkeleton.StaticForwards.Add((key, value));
        }

        protected void Forward(string key, Expression<Func<T, string>> selector)
        {
            CurrentSkeleton.DynamicForwards.Add((key, ObservingFunc<T, string>.FromExpression(selector)));
        }

        protected void Nodes<TOther>(NodeDescriptor<TOther> targetDescriptor, Func<T, ICollectionExpression<TOther>> selector)
        {
            CurrentSkeleton.ChildContributions.Add(new ChildCollectionContribution<T, TOther>
            {
                Selector = selector,
                Skeleton = targetDescriptor._baseSkeleton
            });
        }

        protected void Label(Expression<Func<T, string>> labelSelector, string type = "label", bool canEdit = true, Expression<Func<T, bool>> guard = null)
        {
            var skeleton = new GLabelSkeleton<T>
            {
                StaticType = type,
                LabelValue = labelSelector,
                CanEdit = canEdit
            };
            CurrentSkeleton.ChildContributions.Add(new CompartmentContribution<T> { Compartment = skeleton, Guard = guard });
        }

        protected void Edges<TTransition>(EdgeDescriptor<TTransition> edgeDescriptor, Func<T, ICollectionExpression<TTransition>> selector)
        {
            CurrentSkeleton.ChildContributions.Add(new ChildCollectionContribution<T, TTransition>
            {
                Selector = selector,
                Skeleton = edgeDescriptor._baseSkeleton
            });
        }

        protected void Edges<TSource, TTarget>(EdgeDescriptor<TSource, TTarget> edgeDescriptor, Func<T, ICollectionExpression<(TSource, TTarget)>> selector)
        {
            CurrentSkeleton.ChildContributions.Add(new ChildCollectionContribution<T, (TSource, TTarget)>
            {
                Selector = selector,
                Skeleton = edgeDescriptor._baseSkeleton
            });
        }

        protected void Edges<TSource, TTarget>(NodeDescriptor<TSource> sourceDescriptor, NodeDescriptor<TTarget> targetDescriptor, Func<T, ICollectionExpression<(TSource, TTarget)>> selector)
        {
        }

        private class InnerCompartment : IDisposable
        {
            private readonly NodeDescriptor<T> _nodeDescriptor;

            public InnerCompartment(NodeDescriptor<T> nodeDescriptor)
            {
                _nodeDescriptor = nodeDescriptor;
            }

            public void Dispose()
            {
                _nodeDescriptor._skeletons.Pop();
            }
        }
    }
}
