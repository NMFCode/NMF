using NMF.Models.Meta;
using NMF.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Models.Changes
{
    public partial class ModelChange
    {
        /// <summary>
        /// Calculates changes that invert the current change
        /// </summary>
        /// <returns>A collection of model changes that invert the current change</returns>
        public abstract IEnumerable<IModelChange> Invert();
    }
    public partial class AssociationPropertyChange
    {
        /// <inheritdoc />
        public override IEnumerable<IModelChange> Invert()
        {
            yield return new AssociationPropertyChange
            {
                AffectedElement = AffectedElement,
                Feature = Feature,
                OldValue = NewValue,
                NewValue = OldValue
            };
        }
    }
    public partial class CompositionPropertyChange
    {
        /// <inheritdoc />
        public override IEnumerable<IModelChange> Invert()
        {
            yield return new CompositionPropertyChange
            {
                AffectedElement = AffectedElement,
                Feature = Feature,
                OldValue = NewValue,
                NewValue = OldValue
            };
        }
    }
    public partial class AttributePropertyChange
    {
        /// <inheritdoc />
        public override IEnumerable<IModelChange> Invert()
        {
            yield return new AttributePropertyChange
            {
                AffectedElement = AffectedElement,
                Feature = Feature,
                OldValue = NewValue,
                NewValue = OldValue
            };
        }
    }
    public partial class ChangeTransaction
    {
        /// <inheritdoc />
        public override IEnumerable<IModelChange> Invert()
        {
            for (int i = NestedChanges.Count - 1; i >= 0; i--)
            {
                foreach (var inverted in NestedChanges[i].Invert())
                {
                    yield return inverted;
                }
            }
            foreach (var invertedSource in SourceChange.Invert())
            {
                yield return invertedSource;
            }
        }
    }
    public partial class AssociationCollectionDeletion
    {
        /// <inheritdoc />
        public override IEnumerable<IModelChange> Invert()
        {
            yield return new AssociationCollectionInsertion
            {
                AffectedElement = AffectedElement,
                Feature = Feature,
                AddedElement = DeletedElement
            };
        }
    }
    public partial class AssociationCollectionInsertion
    {
        /// <inheritdoc />
        public override IEnumerable<IModelChange> Invert()
        {
            yield return new AssociationCollectionDeletion
            {
                AffectedElement = AffectedElement,
                Feature = Feature,
                DeletedElement = AddedElement
            };
        }
    }
    public partial class AssociationCollectionReset
    {
        /// <inheritdoc />
        public override IEnumerable<IModelChange> Invert()
        {
            throw new NotSupportedException();
        }
    }
    public partial class AssociationListDeletion
    {
        /// <inheritdoc />
        public override IEnumerable<IModelChange> Invert()
        {
            yield return new AssociationListInsertion
            {
                AffectedElement = AffectedElement,
                Feature = Feature,
                Index = Index,
                AddedElement = DeletedElement
            };
        }
    }
    public partial class AssociationListInsertion
    {

        /// <inheritdoc />
        public override IEnumerable<IModelChange> Invert()
        {
            yield return new AssociationListDeletion
            {
                AffectedElement = AffectedElement,
                Feature = Feature,
                Index = Index,
                DeletedElement = AddedElement
            };
        }
    }
    public partial class CompositionCollectionDeletion
    {
        /// <inheritdoc />
        public override IEnumerable<IModelChange> Invert()
        {
            yield return new CompositionCollectionInsertion
            {
                AffectedElement = AffectedElement,
                Feature = Feature,
                AddedElement = DeletedElement
            };
        }
    }
    public partial class CompositionCollectionInsertion
    {
        /// <inheritdoc />
        public override IEnumerable<IModelChange> Invert()
        {
            yield return new CompositionCollectionDeletion
            {
                AffectedElement = AffectedElement,
                Feature = Feature,
                DeletedElement = AddedElement
            };
        }
    }
    public partial class CompositionCollectionReset
    {
        /// <inheritdoc />
        public override IEnumerable<IModelChange> Invert()
        {
            throw new NotSupportedException("Collection resets are not recoverable");
        }
    }
    public partial class CompositionListDeletion
    {
        /// <inheritdoc />
        public override IEnumerable<IModelChange> Invert()
        {
            yield return new CompositionListInsertion
            {
                AffectedElement = AffectedElement,
                Feature = Feature,
                Index = Index,
                AddedElement = DeletedElement
            };
        }
    }
    public partial class CompositionListInsertion
    {
        /// <inheritdoc />
        public override IEnumerable<IModelChange> Invert()
        {
            yield return new CompositionListDeletion
            {
                AffectedElement = AffectedElement,
                Feature = Feature,
                Index = Index,
                DeletedElement = AddedElement
            };
        }
    }
    public partial class AttributeCollectionDeletion
    {
        /// <inheritdoc />
        public override IEnumerable<IModelChange> Invert()
        {
            yield return new AttributeCollectionInsertion
            {
                AffectedElement = AffectedElement,
                Feature = Feature,
                AddedValue = DeletedValue
            };
        }
    }
    public partial class AttributeCollectionInsertion
    {
        /// <inheritdoc />
        public override IEnumerable<IModelChange> Invert()
        {
            yield return new AttributeCollectionDeletion
            {
                AffectedElement = AffectedElement,
                Feature = Feature,
                DeletedValue = AddedValue
            };
        }
    }
    public partial class AttributeCollectionReset
    {
        /// <inheritdoc />
        public override IEnumerable<IModelChange> Invert()
        {
            throw new NotSupportedException("Collection resets are not recoverable");
        }
    }
    public partial class AttributeListDeletion
    {
        /// <inheritdoc />
        public override IEnumerable<IModelChange> Invert()
        {
            yield return new AttributeListInsertion
            {
                AffectedElement = AffectedElement,
                Feature = Feature,
                Index = Index,
                AddedValue = DeletedValue
            };
        }
    }
    public partial class AttributeListInsertion
    {
        /// <inheritdoc />
        public override IEnumerable<IModelChange> Invert()
        {
            yield return new AttributeListDeletion
            {
                AffectedElement = AffectedElement,
                Feature = Feature,
                Index = Index,
                DeletedValue = AddedValue
            };
        }
    }
    public partial class CompositionMoveIntoProperty
    {
        /// <inheritdoc />
        public override IEnumerable<IModelChange> Invert()
        {
            var child = new CompositionPropertyChange
            {
                AffectedElement = AffectedElement,
                OldValue = NewValue,
                NewValue = OldValue,
                Feature = Feature
            };
            var composition = (ICompositionInsertion)(Origin.Invert().First());
            yield return composition.ConvertIntoMove(child);
        }
    }

    public partial class CompositionMoveToCollection
    {
        /// <inheritdoc />
        public override IEnumerable<IModelChange> Invert()
        {
            var child = new CompositionCollectionDeletion
            {
                AffectedElement = AffectedElement,
                Feature = Feature,
                DeletedElement = MovedElement
            };
            var composition = (ICompositionInsertion)(Origin.Invert().First());
            yield return composition.ConvertIntoMove(child);
        }
    }

    public partial class CompositionMoveToList
    {
        /// <inheritdoc />
        public override IEnumerable<IModelChange> Invert()
        {
            var child = new CompositionListDeletion
            {
                AffectedElement = AffectedElement,
                Feature = Feature,
                DeletedElement = MovedElement,
                Index = Index
            };
            var composition = (ICompositionInsertion)(Origin.Invert().First());
            yield return composition.ConvertIntoMove(child);
        }
    }

    public partial class OperationCall
    {
        /// <inheritdoc />
        public override IEnumerable<IModelChange> Invert()
        {
            // Operation is undone by undoing atomic changes it produced so nothing to do here
            yield break;
        }
    }
}
