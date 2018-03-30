using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    public interface INotificationResult
    {
        INotifiable Source { get; }

        bool Changed { get; }

        void IncreaseReferences(int references);

        void FreeReference();
    }
}
