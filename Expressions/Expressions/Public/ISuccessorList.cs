using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    public interface ISuccessorList : IEnumerable<INotifiable>
    {
        INotifiable this[int index] { get; }

        bool HasSuccessors { get; }

        bool IsAttached { get; }

        void Set(INotifiable node);

        void SetDummy();

        void Unset(INotifiable node, bool leaveDummy = false);

        void UnsetAll();

        event EventHandler Attached;

        event EventHandler Detached;
    }
}
