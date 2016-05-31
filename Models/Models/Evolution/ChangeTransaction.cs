using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMF.Models.Repository;

namespace NMF.Models.Evolution
{
    public class ChangeTransaction : IModelChange
    {
        public Uri AbsoluteUri { get { return SourceChange.AbsoluteUri; } }

        public IModelChange SourceChange { get; private set; }

        public List<IModelChange> NestedChanges { get; private set; }

        public ChangeTransaction(IModelChange sourceChange, IEnumerable<IModelChange> nestedChanges)
        {
            if (sourceChange == null)
                throw new ArgumentNullException(nameof(sourceChange));
            if (nestedChanges == null)
                throw new ArgumentNullException(nameof(nestedChanges));
            if (!nestedChanges.Any())
                throw new ArgumentException("A change transaction must consist of at least two changes.", nameof(nestedChanges));

            SourceChange = sourceChange;
            NestedChanges = nestedChanges.ToList();
        }

        public void Apply(IModelRepository repository)
        {
            SourceChange.Apply(repository);
            foreach (var change in NestedChanges)
                change.Apply(repository);
        }

        public void Undo(IModelRepository repository)
        {
            throw new NotImplementedException();
        }
    }
}
