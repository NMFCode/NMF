using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models.Evolution
{
    public interface IModelChange
    {
        void Do();
        void Undo();

        bool CanUndo { get; }
    }
}
