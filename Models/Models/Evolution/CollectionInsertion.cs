using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMF.Models.Repository;

namespace NMF.Models.Evolution
{
    //TODO serialize
    public abstract class CollectionInsertionBase<T> : IModelChange
    {
        public Uri AbsoluteUri { get; }
        public void Apply(IModelRepository repository)
        {
            throw new NotImplementedException();
        }
        //TODO constructor
        public void Invert(IModelRepository repository)
        {
            throw new NotImplementedException();
        }
    }
}
