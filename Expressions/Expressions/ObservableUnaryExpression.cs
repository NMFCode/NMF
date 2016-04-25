using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace NMF.Expressions
{
    internal abstract class ObservableUnaryExpressionBase<TInner, TOuter> : NotifyExpression<TOuter>
    {
        public ObservableUnaryExpressionBase(INotifyExpression<TInner> target)
        {
            if (target == null) throw new ArgumentNullException("target");

            Target = target;

            target.ValueChanged += TargetChanged;
        }

        private void TargetChanged(object sender, EventArgs e)
        {
            if (!IsAttached) return;
            Refresh();
        }

        public override bool CanBeConstant
        {
            get
            {
                return Target.CanBeConstant;
            }
        }

        public INotifyExpression<TInner> Target { get; private set; }

        protected override void DetachCore()
        {
            Target.Detach();
        }

        protected override void AttachCore()
        {
            Target.Attach();
        }

        public override bool IsParameterFree
        {
            get { return Target.IsParameterFree; }
        }
    }

    internal abstract class ObservableUnaryReversableExpressionBase<TInner, TOuter> : ObservableUnaryExpressionBase<TInner, TOuter>, INotifyReversableExpression<TOuter>
    {
        public ObservableUnaryReversableExpressionBase(INotifyExpression<TInner> target)
            : base(target) { }

        public new TOuter Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                if (!EqualityComparer<TOuter>.Default.Equals(Value, value))
                {
                    var reversable = Target as INotifyReversableExpression<TInner>;
                    if (reversable != null && reversable.IsReversable)
                    {
                        SetValue(reversable, value);
                    }
                }
            }
        }

        public object ValueObject
        {
            get
            {
                return Value;
            }
        }

        protected abstract void SetValue(INotifyReversableExpression<TInner> inner, TOuter value);

        public bool IsReversable
        {
            get
            {
                var reversable = Target as INotifyReversableExpression<TInner>;
                return reversable != null && reversable.IsReversable;
            }
        }
    }


    internal sealed class ObservableUnaryExpression<TInner, TOuter> : ObservableUnaryExpressionBase<TInner, TOuter>
    {
        public Func<TInner, TOuter> Implementation { get; private set; }

        public ObservableUnaryExpression(UnaryExpression node, ObservableExpressionBinder binder)
            : this(binder.VisitObservable<TInner>(node.Operand), ReflectionHelper.CreateDelegate<Func<TInner, TOuter>>(node.Method)) { }

        public ObservableUnaryExpression(INotifyExpression<TInner> inner, Func<TInner, TOuter> implementation)
            : base(inner)
        {
            Implementation = implementation;
        }

        protected override TOuter GetValue()
        {
            return Implementation(Target.Value);
        }

        public override INotifyExpression<TOuter> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableUnaryExpression<TInner, TOuter>(Target.ApplyParameters(parameters), Implementation);
        }
    }

    internal sealed class ObservableConvert<TInner, TOuter> : ObservableUnaryReversableExpressionBase<TInner, TOuter>
    {
        public ObservableConvert(UnaryExpression node, ObservableExpressionBinder binder)
            : base(binder.VisitObservable<TInner>(node.Operand)) { }

        public ObservableConvert(INotifyExpression<TInner> target)
            : base(target) { }

        protected override TOuter GetValue()
        {
            return (TOuter)System.Convert.ChangeType(Target.Value, typeof(TOuter));
        }

        public override INotifyExpression<TOuter> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableConvert<TInner, TOuter>(Target.ApplyParameters(parameters));
        }

        protected override void SetValue(INotifyReversableExpression<TInner> inner, TOuter value)
        {
            try
            {
                inner.Value = (TInner)System.Convert.ChangeType(value, typeof(TInner));
            }
            catch (InvalidCastException ex)
            {
                // Swallow
                Debug.WriteLine("Cast failed: " + ex.Message);
            }
        }
    }

    internal sealed class ObservableTypeAs<TInner, TOuter> : ObservableUnaryExpressionBase<TInner, TOuter>
        where TOuter : class
    {
        public ObservableTypeAs(UnaryExpression node, ObservableExpressionBinder binder)
            : base(binder.VisitObservable<TInner>(node.Operand)) { }

        public ObservableTypeAs(INotifyExpression<TInner> target)
            : base(target) { }

        protected override TOuter GetValue()
        {
            return Target.Value as TOuter;
        }

        public override INotifyExpression<TOuter> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableTypeAs<TInner, TOuter>(Target.ApplyParameters(parameters));
        }
    }


}
