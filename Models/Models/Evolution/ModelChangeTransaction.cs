using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMF.Models.Repository;

namespace NMF.Models.Evolution
{
    public class ModelChangeTransaction : IModelChange
    {
        public Uri AbsoluteUri { get { return SourceChange.AbsoluteUri; } }

        public IModelChange SourceChange { get; private set; }

        public List<IModelChange> PropagatedChanges { get; private set; }

        public ModelChangeTransaction(IModelChange sourceChange, IEnumerable<IModelChange> propagatedChanges)
        {
            if (sourceChange == null)
                throw new ArgumentNullException(nameof(sourceChange));
            if (propagatedChanges == null)
                throw new ArgumentNullException(nameof(propagatedChanges));

            SourceChange = sourceChange;
            PropagatedChanges = propagatedChanges.ToList();
        }

        public void Apply(IModelRepository repository)
        {
            //TODO Rollback -> needs error handling
            //TODO decide whether to use public api (which does propagation by itself) or to bypass propagation
            foreach (var change in PropagatedChanges)
                change.Apply(repository);
            SourceChange.Apply(repository);
        }

        public void Undo(IModelRepository repository)
        {
            SourceChange.Undo(repository);
            foreach (var change in PropagatedChanges.Reverse<IModelChange>())
                change.Undo(repository);
        }
    }
}
