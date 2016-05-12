using NMF.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models.Evolution
{
    public interface IModelChange
    {
        Uri AbsoluteUri { get; }

        void Apply();

        void Undo();
    }
}
