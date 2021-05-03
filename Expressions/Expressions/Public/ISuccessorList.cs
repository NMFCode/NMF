using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    public interface ISuccessorList
    {
        int Count { get; }

        INotifiable GetSuccessor(int index);

        bool HasSuccessors { get; }

        bool IsAttached { get; }

        void Set(INotifiable node);

        void SetDummy();

        void Unset(INotifiable node, bool leaveDummy = false);

        void UnsetAll();
    }
}
