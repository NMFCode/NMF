using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Models
{
    public class UnlockEventArgs : EventArgs
    {
        public IModelElement Element { get; }

        public bool MayUnlock { get; set; } = true;

        public UnlockEventArgs(IModelElement element)
        {
            Element = element;
        }
    }
}
