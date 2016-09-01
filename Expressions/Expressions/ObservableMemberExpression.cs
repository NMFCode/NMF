using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace NMF.Expressions
{
    internal class ObservableMemberExpression<TTarget, TMember> : NotifyExpression<TMember>
    {
        private static readonly bool memberIsNotifiableCollection = typeof(TMember).Implements<INotifiable>() && typeof(TMember).Implements<INotifyCollectionChanged>();
        private readonly IExecutionContext context;

        public ObservableMemberExpression(MemberExpression expression, ObservableExpressionBinder binder, string name, Func<TTarget, TMember> getter)
            : this(binder.VisitObservable<TTarget>(expression.Expression, true), name, getter, binder.Context) { }

        public ObservableMemberExpression(INotifyExpression<TTarget> target, string memberName, Func<TTarget, TMember> memberGet, IExecutionContext context)
        {
            if (memberGet == null) throw new ArgumentNullException("memberGet");
            if (memberName == null) throw new ArgumentNullException("memberName");

            Target = target;
            MemberGet = memberGet;
            MemberName = memberName;
            this.context = context;
        }
        
        public override ExpressionType NodeType { get { return ExpressionType.MemberAccess; } }

        public override bool CanBeConstant
        {
            get
            {
                if (Target == null) return true;
                if (!Target.IsConstant) return false;
                return Target.Value == null ||
                    !(Target.Value is INotifyPropertyChanged);
            }
        }

        public INotifyExpression<TTarget> Target { get; private set; }

        public Func<TTarget, TMember> MemberGet { get; private set; }

        public string MemberName { get; private set; }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                if (Target != null)
                {
                    yield return Target;
                    if (memberIsNotifiableCollection)
                        yield return (INotifiable)GetValue();
                }
                    
            }
        }

        protected override TMember GetValue()
        {
            if (Target != null)
            {
                return MemberGet(Target.Value);
            }
            else
            {
                return MemberGet(default(TTarget));
            }
        }

        public override string ToString()
        {
            return string.Format("{0}.{1}", Target.ToString(), MemberName);
        }

        public override bool IsParameterFree
        {
            get { return Target == null || Target.IsParameterFree; }
        }

        public override INotifyExpression<TMember> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableMemberExpression<TTarget, TMember>(Target.ApplyParameters(parameters), MemberName, MemberGet, context);
        }

        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            if (sources.Count == 2)
            {
                //If both target and target.Vale change, only handle target.
                if (sources[0].Source != Target)
                    sources[0] = sources[1];
            }

            if (sources[0].Source == Target)
            {
                var oldValue = ((ValueChangedNotificationResult<TTarget>)sources[0]).OldValue;
                DetachPropertyChangeListener(oldValue);
                AttachPropertyChangeListener(Target.Value);
                return base.Notify(sources);
            }
            else
            {
                return sources[0];
            }
        }

        protected override void OnAttach()
        {
            if (Target != null)
                AttachPropertyChangeListener(Target.Value);
        }

        protected override void OnDetach()
        {
            if (Target != null)
                DetachPropertyChangeListener(Target.Value);
        }

        private void AttachPropertyChangeListener(object target)
        {
            var newTarget = target as INotifyPropertyChanged;
            if (newTarget != null)
                context.AddChangeListener(this, newTarget, MemberName);
        }

        private void DetachPropertyChangeListener(object target)
        {
            var oldTarget = target as INotifyPropertyChanged;
            if (oldTarget != null)
                context.RemoveChangeListener(this, oldTarget, MemberName);
        }
    }

    internal class ObservableReversableMemberExpression<TTarget, TMember> : ObservableReversableExpression<TMember>
    {
        private static readonly bool memberIsNotifiableCollection = typeof(TMember).Implements<INotifiable>() && typeof(TMember).Implements<INotifyCollectionChanged>();
        private readonly IExecutionContext context;

        public ObservableReversableMemberExpression(MemberExpression expression, ObservableExpressionBinder binder, string name, FieldInfo field)
            : this(binder.VisitObservable<TTarget>(expression.Expression, true), name, ReflectionHelper.CreateDynamicFieldGetter<TTarget, TMember>(field), ReflectionHelper.CreateDynamicFieldSetter<TTarget, TMember>(field), binder.Context) { }

        public ObservableReversableMemberExpression(MemberExpression expression, ObservableExpressionBinder binder, string name, Func<TTarget, TMember> getter, Action<TTarget, TMember> setter)
            : this(binder.VisitObservable<TTarget>(expression.Expression, true), name, getter, setter, binder.Context) { }

        public ObservableReversableMemberExpression(INotifyExpression<TTarget> target, string memberName, Func<TTarget, TMember> memberGet, Action<TTarget, TMember> memberSet, IExecutionContext context)
        {
            if (memberGet == null) throw new ArgumentNullException("memberGet");
            if (memberSet == null) throw new ArgumentNullException("memberSet");
            if (memberName == null) throw new ArgumentNullException("memberName");

            Target = target;
            MemberGet = memberGet;
            MemberSet = memberSet;
            MemberName = memberName;
            this.context = context;
        }
        
        public override ExpressionType NodeType { get { return ExpressionType.MemberAccess; } }

        public override bool CanBeConstant
        {
            get
            {
                if (Target == null) return true;
                if (!Target.CanBeConstant) return false;
                return Target.Value == null ||
                    !(Target.Value is INotifyPropertyChanged);
            }
        }

        public INotifyExpression<TTarget> Target { get; private set; }

        public Func<TTarget, TMember> MemberGet { get; private set; }

        public Action<TTarget, TMember> MemberSet { get; set; }

        public string MemberName { get; private set; }

        public override bool IsParameterFree
        {
            get { return Target == null || Target.IsParameterFree; }
        }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                if (Target != null)
                {
                    yield return Target;
                    if (memberIsNotifiableCollection)
                        yield return (INotifiable)GetValue();
                }
            }
        }

        protected override TMember GetValue()
        {
            if (Target != null)
            {
                if (Target.Value == null)
                    return default(TMember);
                return MemberGet(Target.Value);
            }
            else
            {
                return MemberGet(default(TTarget));
            }
        }

        public override void SetValue(TMember value)
        {
            if (Target != null)
            {
                if (Target.Value != null)
                {
                    MemberSet(Target.Value, value);
                }
                else
                {
                    if (value != null) throw new NullReferenceException();
                }
            }
            else
            {
                MemberSet(default(TTarget), value);
            }
        }

        public override INotifyExpression<TMember> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableReversableMemberExpression<TTarget, TMember>(Target.ApplyParameters(parameters), MemberName, MemberGet, MemberSet, context);
        }

        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            INotificationResult collectionResult = null;
            if (sources.Count > 0)
            {
                foreach (var change in sources)
                {
                    if (change.Source == Target)
                    {
                        var oldValue = ((IValueChangedNotificationResult)change).OldValue;
                        DetachPropertyChangeListener(oldValue);
                        AttachPropertyChangeListener(Target.Value);
                        return base.Notify(sources);
                    }
                    else //change is from the member value being an INotifyEnumerable
                    {
                        collectionResult = change;
                    }
                }
            }

            var result = base.Notify(sources);
            if (collectionResult == null || result.Changed)
                return result;
            else
                return collectionResult;
        }

        protected override void OnAttach()
        {
            if (Target != null)
                AttachPropertyChangeListener(Target.Value);
        }

        protected override void OnDetach()
        {
            if (Target != null)
                DetachPropertyChangeListener(Target.Value);
        }

        private void AttachPropertyChangeListener(object target)
        {
            var newTarget = target as INotifyPropertyChanged;
            if (newTarget != null)
                context.AddChangeListener(this, newTarget, MemberName);
        }

        private void DetachPropertyChangeListener(object target)
        {
            var oldTarget = target as INotifyPropertyChanged;
            if (oldTarget != null)
                context.RemoveChangeListener(this, oldTarget, MemberName);
        }
    }
}
