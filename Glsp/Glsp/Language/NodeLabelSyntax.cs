using NMF.Expressions;
using NMF.Glsp.Processing;
using NMF.Glsp.Protocol.Types;
using NMF.Glsp.Protocol.Validation;
using System;
using System.Linq.Expressions;

namespace NMF.Glsp.Language
{
    internal class NodeLabelSyntax<T> : INodeLabelSyntax<T>
    {
        private readonly GLabelSkeleton<T> _skeleton;
        private CompartmentContribution<T> _contribution;

        public NodeLabelSyntax(GLabelSkeleton<T> skeleton, CompartmentContribution<T> contribution)
        {
            _skeleton = skeleton;
            _contribution = contribution;
        }

        public INodeLabelSyntax<T> At(double x, double y)
        {
            _skeleton.Position = new Point(x, y);
            return this;
        }

        public INodeLabelSyntax<T> If(Expression<Func<T, bool>> guard)
        {
            _contribution.Guard = guard;
            return this;
        }

        public INodeLabelSyntax<T> Validate(Func<T, string, ValidationStatus> validator)
        {
            _skeleton.Validator = validator;
            return this;
        }

        public INodeLabelSyntax<T> WithConditionalCss(Expression<Func<T, string>> css)
        {
            _skeleton.DynamicCssClasses.Add(new ObservingFunc<T, string>(css));
            return this;
        }

        public INodeLabelSyntax<T> WithConditionalCss(string css, Expression<Func<T, bool>> condition)
        {
            if (condition != null && css != null)
            {
                var expression = Expression.Lambda<Func<T, string>>(
                    Expression.Condition(condition.Body, Expression.Constant(css), Expression.Constant(null, typeof(string))),
                    condition.Parameters[0]);
                return WithConditionalCss(expression);
            }
            else
            {
                return WithCss(css);
            }
        }

        public INodeLabelSyntax<T> WithCss(string css)
        {
            if (css != null)
            {
                _skeleton.StaticCssClasses.Add(css);
            }
            return this;
        }

        public INodeLabelSyntax<T> WithSetter(Action<T, string> setter)
        {
            _skeleton.CustomSetter = setter;
            _skeleton.CanEdit = setter != null;
            return this;
        }

        public INodeLabelSyntax<T> WithType(string type)
        {
            _skeleton.Type = type ?? _skeleton.Type;
            return this;
        }
    }
}
