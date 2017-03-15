using NMF.Models.Repository;
using NMF.Serialization;
using NMF.Serialization.Xmi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Models.Evolution
{
    /// <summary>
    /// Represents a root collection of model changes.
    /// </summary>
    [XmlNamespace("http://nmf.codeplex.com/changes")]
    [XmlNamespacePrefix("changes")]
    public class ModelChangeCollection
    {
        /// <summary>
        /// Gets the list of model changes.
        /// </summary>
        public List<IModelChange> Changes { get; set; }

        public ModelChangeCollection() : this(new List<IModelChange>()) { }

        internal ModelChangeCollection(List<IModelChange> changes)
        {
            Changes = changes;
        }

        /// <summary>
        /// Applies all changes in this collection to the given repository.
        /// </summary>
        /// <param name="repository"></param>
        public void Apply(IModelRepository repository)
        {
            foreach (var change in Changes)
                change.Apply(repository);
        }

        public IEnumerable<IModelChange> TraverseFlat()
        {
            return TraverseFlat(Changes);
        }

        private static IEnumerable<IModelChange> TraverseFlat(IEnumerable<IModelChange> baseList)
        {
            foreach (var change in baseList)
            {
                yield return change;
                var transaction = change as ChangeTransaction;
                if (transaction != null)
                {
                    yield return transaction.SourceChange;
                    foreach (var nested in TraverseFlat(transaction.NestedChanges))
                        yield return nested;
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            var other = obj as ModelChangeCollection;
            if (other == null)
                return false;
            return this.Changes.SequenceEqual(other.Changes);
        }

        public override int GetHashCode()
        {
            return Changes.GetHashCode();
        }
    }
}
