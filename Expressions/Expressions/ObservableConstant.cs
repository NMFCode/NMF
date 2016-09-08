using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions
{
    internal class ObservableConstant<T> : NotifyExpression<T>
    {
        public override string ToString()
        {
            return Value != null ? Value.ToString() : "(null)";
        }

        public ObservableConstant(T value) : base(value) { }

        public override ExpressionType NodeType { get { return ExpressionType.Constant; } }

        public override bool CanReduce { get { return false; } }

        public override bool CanBeConstant { get { return true; } }

        public override bool IsConstant { get { return true; } }

        public override bool IsParameterFree { get { return true; } }

        public override IEnumerable<INotifiable> Dependencies { get { return Enumerable.Empty<INotifiable>(); } }

        public override ISuccessorList Successors { get; } = new DummySuccessorList();

        public override INotifyExpression<T> ApplyParameters(IDictionary<string, object> parameters)
        {
            return this;
        }

        protected override T GetValue()
        {
            return Value;
        }

        public override INotifyExpression<T> Reduce()
        {
            return this;
        }

        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            throw new InvalidOperationException("A constant cannot have a dependency and therefore cannot be notified of a dependency change.");
        }

        private class DummySuccessorList : ISuccessorList
        {
            INotifiable ISuccessorList.this[int index] { get { return null; } }

            bool ISuccessorList.HasSuccessors { get { return false; } }

            bool ISuccessorList.IsAttached { get { return false; } }

            event EventHandler ISuccessorList.Attached { add { } remove { } }

            event EventHandler ISuccessorList.Detached { add { } remove { } }

            IEnumerator<INotifiable> IEnumerable<INotifiable>.GetEnumerator()
            {
                yield break;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                yield break;
            }

            void ISuccessorList.Set(INotifiable node) { }

            void ISuccessorList.SetDummy() { }

            void ISuccessorList.Unset(INotifiable node, bool leaveDummy) { }

            void ISuccessorList.UnsetAll() { }
        }
    }
}
