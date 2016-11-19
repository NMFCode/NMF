using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMF.Models.Repository;
using NMF.Serialization;
using System.ComponentModel;

namespace NMF.Models.Evolution
{
    public class ChangeTransaction : IModelChange
    {
        public Uri AbsoluteUri { get { return SourceChange.AbsoluteUri; } }
        
        public IModelChange SourceChange { get; set; }
        
        public List<IModelChange> NestedChanges { get; set; }
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ChangeTransaction()
        {
            NestedChanges = new List<IModelChange>();
        }

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
            ///NMF automatically propagates the nested changes
            SourceChange.Apply(repository);
        }

        public void Invert(IModelRepository repository)
        {
            //assumes nestedchanges is ordered like a flattened tree of changes
            for (var i = NestedChanges.Count - 1; i >= 0; i--)
                NestedChanges[i].Invert(repository);
            SourceChange.Invert(repository);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            var other = obj as ChangeTransaction;
            if (other == null)
                return false;
            return this.SourceChange.Equals(other.SourceChange)
                && this.NestedChanges.SequenceEqual(other.NestedChanges);
        }

        public override int GetHashCode()
        {
            return SourceChange?.GetHashCode() ?? 0
                ^ NestedChanges.GetHashCode();
        }
    }
}
