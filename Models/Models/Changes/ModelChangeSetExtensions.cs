using NMF.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Models.Changes
{
    public partial class ModelChangeSet
    {
        public void Apply(IModelRepository repository)
        {
            for (int i = 0; i < Changes.Count; i++)
            {
                Changes[i].Apply(repository);
            }
        }

        public void Invert(IModelRepository repository)
        {
            for (int i = Changes.Count - 1; i >= 0; i--)
            {
                Changes[i].Invert(repository);
            }
        }
    }
}
