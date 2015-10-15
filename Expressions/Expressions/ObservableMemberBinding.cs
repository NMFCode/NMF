using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace NMF.Expressions
{
	internal abstract class ObservableMemberBinding<T>
	{
		public abstract void Attach();

		public abstract void Detach();

        public abstract bool IsParameterFree { get; }

        public abstract ObservableMemberBinding<T> ApplyParameters(INotifyExpression<T> newTarget, IDictionary<string, object> parameters);
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

        private void ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Apply();
        }

        public INotifyExpression<TMember> Value { get; private set; }

        public Action<T, TMember> Member { get; private set; }

        public INotifyExpression<T> Target { get; private set; }

        public void Apply()
        {
            Member(Target.Value, Value.Value);
        }

        public override void Attach()
        {
            Value.Attach();
            Apply();
            Target.ValueChanged += ValueChanged;
            Value.ValueChanged += ValueChanged;
            Apply();
        }

        public override void Detach()
        {
            Value.Detach();
            Target.ValueChanged -= ValueChanged;
            Value.ValueChanged -= ValueChanged;
        }

        public override bool IsParameterFree
        {
            get { return Value.IsParameterFree; }
        }

        public override ObservableMemberBinding<T> ApplyParameters(INotifyExpression<T> newTarget, IDictionary<string, object> parameters)
        {
            return new ObservablePropertyMemberBinding<T, TMember>(newTarget, Member, Value.ApplyParameters(parameters));
        }
    }

    internal class ObservableReversablePropertyMemberBinding<T, TMember> : ObservableMemberBinding<T>
    {
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
        }

        private void ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Apply();
        }

        public INotifyReversableExpression<TMember> Value { get; private set; }

        public Action<T, TMember> MemberSet { get; private set; }

        public Func<T, TMember> MemberGet { get; private set; }

        public string MemberName { get; private set; }

        public INotifyExpression<T> Target { get; private set; }

        public void Apply()
        {
            MemberSet(Target.Value, Value.Value);
        }

        public override void Attach()
        {
            Value.Attach();
            Apply();
            Target.ValueChanged += TargetValueChanged;
            Value.ValueChanged += ValueChanged;
            var target = Target.Value as INotifyPropertyChanged;
            if (target != null)
            {
                target.PropertyChanged += TargetPropertyChanged;
            }
            Apply();
        }

        private void TargetPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == MemberName)
            {
                Value.Value = MemberGet(Target.Value);
            }
        }

        private void TargetValueChanged(object sender, ValueChangedEventArgs e)
        {
            var old = e.OldValue as INotifyPropertyChanged;
            if (old != null) old.PropertyChanged -= TargetPropertyChanged;
            var newValue = e.NewValue as INotifyPropertyChanged;
            if (newValue != null) newValue.PropertyChanged += TargetPropertyChanged;
            Apply();
        }

        public override void Detach()
        {
            Value.Detach();
            Target.ValueChanged -= ValueChanged;
            Value.ValueChanged -= ValueChanged;
        }

        public override bool IsParameterFree
        {
            get { return Value.IsParameterFree; }
        }

        public override ObservableMemberBinding<T> ApplyParameters(INotifyExpression<T> newTarget, IDictionary<string, object> parameters)
        {
            return new ObservablePropertyMemberBinding<T, TMember>(newTarget, MemberSet, Value.ApplyParameters(parameters));
        }
    }
}
