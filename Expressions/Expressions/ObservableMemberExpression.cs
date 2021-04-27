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
        private readonly PropertyChangeListener listener;

        public ObservableMemberExpression(MemberExpression expression, ObservableExpressionBinder binder, string name, Func<TTarget, TMember> getter)
            : this(binder.VisitObservable<TTarget>(expression.Expression, true), name, getter) { }

        public ObservableMemberExpression(INotifyExpression<TTarget> target, string memberName, Func<TTarget, TMember> memberGet)
        {
            if (memberGet == null) throw new ArgumentNullException("memberGet");
            if (memberName == null) throw new ArgumentNullException("memberName");

            Target = target;
            MemberGet = memberGet;
            MemberName = memberName;
            listener = new PropertyChangeListener(this);
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
                    yield return Target;
                    
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
            return $"[MemberAccess {MemberName}]";
        }

        public override bool IsParameterFree
        {
            get { return Target == null || Target.IsParameterFree; }
        }

        protected override INotifyExpression<TMember> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableMemberExpression<TTarget, TMember>(Target.ApplyParameters(parameters, trace), MemberName, MemberGet);
        }

        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            if (sources.Count > 0)
            {
                var oldValue = ((IValueChangedNotificationResult<TTarget>)sources[0]).OldValue;
                listener.Unsubscribe();
                AttachPropertyChangeListener(Target.Value);
            }
            return base.Notify(sources);
        }

        protected override void OnAttach()
        {
            if (Target != null)
                AttachPropertyChangeListener(Target.Value);
        }

        protected override void OnDetach()
        {
            listener.Unsubscribe();
        }

        private void AttachPropertyChangeListener(object target)
        {
            if(target is INotifyPropertyChanged newTarget)
                listener.Subscribe( newTarget, MemberName );
        }
    }

    internal class ObservableReversableMemberExpression<TTarget, TMember> : ObservableReversableExpression<TMember>
    {
        private readonly PropertyChangeListener listener;

        public ObservableReversableMemberExpression(MemberExpression expression, ObservableExpressionBinder binder, string name, FieldInfo field)
            : this(binder.VisitObservable<TTarget>(expression.Expression, true), name, ReflectionHelper.CreateDynamicFieldGetter<TTarget, TMember>(field), ReflectionHelper.CreateDynamicFieldSetter<TTarget, TMember>(field)) { }

        public ObservableReversableMemberExpression(MemberExpression expression, ObservableExpressionBinder binder, string name, Func<TTarget, TMember> getter, Action<TTarget, TMember> setter)
            : this(binder.VisitObservable<TTarget>(expression.Expression, true), name, getter, setter) { }

        public ObservableReversableMemberExpression(INotifyExpression<TTarget> target, string memberName, Func<TTarget, TMember> memberGet, Action<TTarget, TMember> memberSet)
        {
            if (memberGet == null) throw new ArgumentNullException("memberGet");
            if (memberSet == null) throw new ArgumentNullException("memberSet");
            if (memberName == null) throw new ArgumentNullException("memberName");

            Target = target;
            MemberGet = memberGet;
            MemberSet = memberSet;
            MemberName = memberName;
            listener = new PropertyChangeListener(this);
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

        public override string ToString()
        {
            return $"[MemberAccess {MemberName}]";
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
                    yield return Target;
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

        protected override INotifyExpression<TMember> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableReversableMemberExpression<TTarget, TMember>(Target.ApplyParameters(parameters, trace), MemberName, MemberGet, MemberSet);
        }

        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            if (sources.Count > 0)
            {
                var oldValue = ((IValueChangedNotificationResult)sources[0]).OldValue;
                listener.Unsubscribe();
                AttachPropertyChangeListener(Target.Value);
            }
            return base.Notify(sources);
        }

        protected override void OnAttach()
        {
            if (Target != null)
                AttachPropertyChangeListener(Target.Value);
        }

        protected override void OnDetach()
        {
            listener.Unsubscribe();
        }

        private void AttachPropertyChangeListener(object target)
        {
            if(target is INotifyPropertyChanged newTarget)
                listener.Subscribe( newTarget, MemberName );
        }
    }
}
