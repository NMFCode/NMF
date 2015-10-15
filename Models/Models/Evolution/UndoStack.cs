using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models.Evolution
{
    public class UndoStack : IModelChangeHandler
    {
        private List<IModelChange> changes = new List<IModelChange>();

        public void ProcessPropertyChange(ModelPropertyChange propertyChange)
        {
            changes.Add(propertyChange);
        }

        public void ProcessCollectionChange(ModelCollectionChange collectionChange)
        {
            changes.Add(collectionChange);
        }

        public void ProcessModelCreation(ModelCreation creation)
        {
            throw new NotImplementedException();
        }

        public void ProcessModelDeletion(ModelDeletion deletion)
        {
            throw new NotImplementedException();
        }
    }
}
