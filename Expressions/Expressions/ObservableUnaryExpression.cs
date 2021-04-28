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
        protected abstract string Format { get; }

        public override string ToString()
        {
            return string.Format(Format, Target.ToString()) + "{" + (Value != null ? Value.ToString() : "(null)") + "}";
        }

        public ObservableUnaryExpressionBase(INotifyExpression<TInner> target)
        {
            if (target == null) throw new ArgumentNullException("target");

            Target = target;
        }

        public override bool CanBeConstant { get { return Target.CanBeConstant; } }

        public INotifyExpression<TInner> Target { get; private set; }

        public override IEnumerable<INotifiable> Dependencies { get { yield return Target; } }
        
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
                    if(Target is INotifyReversableExpression<TInner> reversable && reversable.IsReversable)
                    {
                        SetValue( reversable, value );
                    }
                }
            }
        }

        protected abstract void SetValue(INotifyReversableExpression<TInner> inner, TOuter value);

        public bool IsReversable
        {
            get
            {
                return Target is INotifyReversableExpression<TInner> reversable && reversable.IsReversable;
            }
        }
    }


    internal sealed class ObservableUnaryExpression<TInner, TOuter> : ObservableUnaryExpressionBase<TInner, TOuter>
    {
        protected override string Format
        {
            get
            {
                return Implementation.ToString() + "{0}";
            }
        }

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

        protected override INotifyExpression<TOuter> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableUnaryExpression<TInner, TOuter>(Target.ApplyParameters(parameters, trace), Implementation);
        }
    }

    internal sealed class ObservableConvert<TInner, TOuter> : ObservableUnaryReversableExpressionBase<TInner, TOuter>
    {
        private static readonly bool conversionRequired = ReflectionHelper.IsValueType(typeof(TInner));
        private static readonly Type nullableType = Nullable.GetUnderlyingType(typeof(TOuter)) ?? typeof(TOuter);

        protected override string Format
        {
            get
            {
                return "(" + typeof(TOuter).Name + "){0}";
            }
        }

        public ObservableConvert(UnaryExpression node, ObservableExpressionBinder binder)
            : base(binder.VisitObservable<TInner>(node.Operand)) { }

        public ObservableConvert(INotifyExpression<TInner> target)
            : base(target) { }

        protected override TOuter GetValue()
        {
            if (conversionRequired)
            {
                var value = Target.Value;
                return (value == null) ? default(TOuter) : (TOuter)System.Convert.ChangeType(value, nullableType);
            }
            else
            {
                return (TOuter)((object)Target.Value);
            }
        }

        protected override INotifyExpression<TOuter> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableConvert<TInner, TOuter>(Target.ApplyParameters(parameters, trace));
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

        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.Convert;
            }
        }
    }

    internal sealed class ObservableTypeAs<TInner, TOuter> : ObservableUnaryExpressionBase<TInner, TOuter>
        where TOuter : class
    {
        protected override string Format
        {
            get
            {
                return "{0} as " + typeof(TOuter).Name;
            }
        }

        public ObservableTypeAs(UnaryExpression node, ObservableExpressionBinder binder)
            : base(binder.VisitObservable<TInner>(node.Operand)) { }

        public ObservableTypeAs(INotifyExpression<TInner> target)
            : base(target) { }

        protected override TOuter GetValue()
        {
            return Target.Value as TOuter;
        }

        protected override INotifyExpression<TOuter> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableTypeAs<TInner, TOuter>(Target.ApplyParameters(parameters, trace));
        }
    }


}
