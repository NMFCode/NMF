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
        public void Apply()
        {
            for (int i = 0; i < Changes.Count; i++)
            {
                Changes[i].Apply();
            }
        }

        public void Invert()
        {
            foreach (var change in Changes.Reverse())
            {
                foreach (var inverted in change.Invert())
                {
                    inverted.Apply();
                }
            }
        }

        public ModelChangeSet CreateInvertedChangeSet()
        {
            var inverse = new ModelChangeSet();
            foreach (var change in Changes.Reverse())
            {
                foreach (var inverted in change.Invert())
                {
                    inverse.Changes.Add(inverted);
                }
            }
            return inverse;
        }
    }
}
