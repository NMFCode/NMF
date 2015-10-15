using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace NMF.Expressions
{
    internal abstract class ObservableMethodBase<T, TDelegate, TResult> : NotifyExpression<TResult>
        where TDelegate : class
    {
        public ObservableMethodBase(INotifyExpression<T> target, MethodInfo method)
        {
            if (method == null) throw new ArgumentNullException("method");
            if (target == null) throw new ArgumentNullException("target");

            Target = target;
            Method = method;

            target.ValueChanged += TargetChanged;
        }

        private void RenewFunction()
        {
            Function = ReflectionHelper.CreateDelegate(typeof(TDelegate), Target.Value, Method) as TDelegate;
        }

        public MethodInfo Method { get; private set; }

        public TDelegate Function { get; private set; }

        private void TargetChanged(object sender, ValueChangedEventArgs e)
        {
            if (!IsAttached) return;
            var oldNotifier = e.OldValue as INotifyCollectionChanged;
            var newNotifier = e.NewValue as INotifyCollectionChanged;
            if (oldNotifier != null)
            {
                oldNotifier.CollectionChanged -= TargetCollectionChanged;
            }
            if (newNotifier != null)
            {
                newNotifier.CollectionChanged += TargetCollectionChanged;
            }
            RenewFunction();
            Refresh();
        }

        private void TargetCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!IsAttached) return;
            Refresh();
        }

        protected override void OnValueChanged(ValueChangedEventArgs e)
        {
            base.OnValueChanged(e);
            var disposable = e.OldValue as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }

        protected void ArgumentChanged(object sender, ValueChangedEventArgs e)
        {
            if (!IsAttached) return;
            Refresh();
        }

        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.Invoke;
            }
        }

        public override bool CanBeConstant
        {
            get
            {
                return Target.CanBeConstant && !(Target.Value is INotifyCollectionChanged);
            }
        }

        public INotifyExpression<T> Target { get; private set; }

        protected override void AttachCore()
        {
            Target.Attach();
            var targetNotifier = Target.Value as INotifyCollectionChanged;
            if (targetNotifier != null)
            {
                targetNotifier.CollectionChanged += TargetCollectionChanged;
            }
            RenewFunction();
        }

        protected override void DetachCore()
        {
            Target.Detach();
            var targetNotifier = Target.Value as INotifyCollectionChanged;
            if (targetNotifier != null)
            {
                targetNotifier.CollectionChanged -= TargetCollectionChanged;
            }
        }
    }

}
