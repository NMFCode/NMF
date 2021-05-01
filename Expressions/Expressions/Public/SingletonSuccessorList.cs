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

        public bool HasSuccessors => true;

        public bool IsAttached => true;

        public int Count => 0;

        public IEnumerable<INotifiable> AllSuccessors => Enumerable.Empty<INotifiable>();

        public void Set(INotifiable node) { }

        public void SetDummy() { }

        public void Unset(INotifiable node, bool leaveDummy = false) { }

        public void UnsetAll() { }

        public INotifiable GetSuccessor(int index)
        {
            throw new NotSupportedException();
        }
    }
}
