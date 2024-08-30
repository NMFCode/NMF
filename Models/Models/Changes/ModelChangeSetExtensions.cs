using System;
using System.Linq;

namespace NMF.Models.Changes
{
    public partial class ModelChangeSet
    {
        /// <summary>
        /// Applies the change set
        /// </summary>
        public void Apply()
        {
            for (int i = 0; i < Changes.Count; i++)
            {
                Changes[i].Apply();
            }
        }

        /// <summary>
        /// Inverts the change set
        /// </summary>
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

        /// <summary>
        /// Creates a model change set that represents the inversion of this change set
        /// </summary>
        /// <returns>a model change set that represents the inversion of this change set</returns>
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
