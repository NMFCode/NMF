using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Expressions
{
    public class SingletonSuccessorList : ISuccessorList
    {
        public static SingletonSuccessorList Instance { get; } = new SingletonSuccessorList();

        private SingletonSuccessorList() { }

        public INotifiable this[int index]
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public bool HasSuccessors => true;

        public bool IsAttached => true;

        event EventHandler ISuccessorList.Attached { add { } remove { } }

        event EventHandler ISuccessorList.Detached { add { } remove { } }

        public IEnumerator<INotifiable> GetEnumerator()
        {
            yield break;
        }

        public void Set(INotifiable node) { }

        public void SetDummy() { }

        public void Unset(INotifiable node, bool leaveDummy = false) { }

        public void UnsetAll() { }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
