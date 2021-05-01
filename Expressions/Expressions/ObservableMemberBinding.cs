using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace NMF.Expressions
{
    internal abstract class ObservableMemberBinding<T> : INotifiable
    {
        public ObservableMemberBinding()
        {
            Successors.Attached += (obj, e) => Attach();
            Successors.Detached += (obj, e) => Detach();
        }


        public MultiSuccessorList Successors { get; } = new MultiSuccessorList();

        ISuccessorList INotifiable.Successors => Successors;

        public abstract bool IsParameterFree { get; }
        
        public abstract IEnumerable<INotifiable> Dependencies { get; }

        public ExecutionMetaData ExecutionMetaData { get; } = new ExecutionMetaData();

        public void Dispose()
        {
            Successors.UnsetAll();
        }

        private void Attach()
        {
            foreach (var dep in Dependencies)
                dep.Successors.Set(this);
            OnAttach();
        }

        private void Detach()
        {
            OnDetach();
            foreach (var dep in Dependencies)
                dep.Successors.Unset(this);
        }

        protected virtual void OnAttach() { }

        protected virtual void OnDetach() { }

        public abstract ObservableMemberBinding<T> ApplyParameters(INotifyExpression<T> newTarget, IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace);

        public abstract INotificationResult Notify(IList<INotificationResult> sources);
    }

    internal class ObservablePropertyMemberBinding<T, TMember> : ObservableMemberBinding<T>
    {
        public ObservablePropertyMemberBinding(MemberAssignment node, ObservableExpressionBinder binder, INotifyExpression<T> target, FieldInfo field)
            : this(target, ReflectionHelper.CreateDynamicFieldSetter<T, TMember>(field), binder.VisitObservable<TMember>(node.Expression)) { }

        public ObservablePropertyMemberBinding(MemberAssignment node, ObservableExpressionBinder binder, INotifyExpression<T> target, Action<T, TMember> member)
            : this(target, member, binder.VisitObservable<TMember>(node.Expression)) { }


        public ObservablePropertyMemberBinding(INotifyExpression<T> target, Action<T, TMember> member, INotifyExpression<TMember> value)
        {
            if (value == null) throw new ArgumentNullException("value");
            if (member == null) throw new ArgumentNullException("member");
            if (target == null) throw new ArgumentNullException("target");

            Value = value;
            Member = member;
            Target = target;
        }

        public INotifyExpression<TMember> Value { get; private set; }

        public Action<T, TMember> Member { get; private set; }

        public INotifyExpression<T> Target { get; private set; }

        public override bool IsParameterFree
        {
            get { return Value.IsParameterFree; }
        }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return Value;
                yield return Target;
            }
        }

        public void Apply()
        {
            Member(Target.Value, Value.Value);
        }

        public override ObservableMemberBinding<T> ApplyParameters(INotifyExpression<T> newTarget, IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservablePropertyMemberBinding<T, TMember>(newTarget, Member, Value.ApplyParameters(parameters, trace));
        }

        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            T oldValue = Target.Value;
            Apply();
            return new ValueChangedNotificationResult<T>(this, oldValue, Target.Value);
        }

        protected override void OnAttach()
        {
            Apply();
        }
    }

    internal class ObservableReversablePropertyMemberBinding<T, TMember> : ObservableMemberBinding<T>
    {
        private readonly PropertyChangeListener listener;

        public ObservableReversablePropertyMemberBinding(INotifyExpression<T> target, string memberName, Func<T, TMember> memberGet, Action<T, TMember> memberSet, INotifyReversableExpression<TMember> value)
        {
            if (value == null) throw new ArgumentNullException("value");
            if (memberName == null) throw new ArgumentNullException("memberName");
            if (memberGet == null) throw new ArgumentNullException("memberGet");
            if (memberSet == null) throw new ArgumentNullException("memberSet");
            if (target == null) throw new ArgumentNullException("target");

            Value = value;
            MemberName = memberName;
            MemberGet = memberGet;
            MemberSet = memberSet;
            Target = target;
            listener = new PropertyChangeListener(this);
        }

        public INotifyReversableExpression<TMember> Value { get; private set; }

        public Action<T, TMember> MemberSet { get; private set; }

        public Func<T, TMember> MemberGet { get; private set; }

        public string MemberName { get; private set; }

        public INotifyExpression<T> Target { get; private set; }

        public override bool IsParameterFree
        {
            get { return Value.IsParameterFree; }
        }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return Value;
                yield return Target;
            }
        }

        public void Apply()
        {
            MemberSet(Target.Value, Value.Value);
        }

        public override ObservableMemberBinding<T> ApplyParameters(INotifyExpression<T> newTarget, IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservablePropertyMemberBinding<T, TMember>(newTarget, MemberSet, Value.ApplyParameters(parameters, trace));
        }

        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            IValueChangedNotificationResult<T> targetChange = null;
            if (sources.Count >= 1 && sources[0].Source == Target)
                targetChange = sources[0] as IValueChangedNotificationResult<T>;
            else if (sources.Count == 2 && sources[1].Source == Target)
                targetChange = sources[1] as IValueChangedNotificationResult<T>;
            
            if (targetChange != null)
            {
                listener.Unsubscribe();
                AttachPropertyChangeListener(targetChange.NewValue);
            }

            Apply();
            Value.Value = MemberGet(Target.Value);
            return new ValueChangedNotificationResult<T>(this, targetChange.OldValue, targetChange.NewValue);
        }

        protected override void OnAttach()
        {
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
