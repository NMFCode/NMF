using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models.Evolution
{
    public class ModelCreation : IModelChange
    {
        public IModelElement Element
        {
            get { throw new NotImplementedException(); }
        }

        public void Do()
        {
            throw new NotImplementedException();
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }

        public bool CanUndo
        {
            get { throw new NotImplementedException(); }
        }
    }
}
