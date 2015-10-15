using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models.Evolution
{
    public interface IModelChangeHandler
    {
        void ProcessModelCreation(ModelCreation creation);

        void ProcessModelDeletion(ModelDeletion deletion);

        void ProcessPropertyChange(ModelPropertyChange propertyChange);

        void ProcessCollectionChange(ModelCollectionChange collectionChange);
    }
}
