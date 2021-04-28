using NMF.Models.Meta;
using NMF.Models.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Models.Changes
{
    public partial class ModelChange
    {
        /// <summary>
        /// Applies the change
        /// </summary>
        public abstract void Apply();
    }
    public partial class AssociationPropertyChange
    {
        /// <inheritdoc />
        public override void Apply()
        {
            AffectedElement.SetReferencedElement((IReference)Feature, NewValue);
        }
    }
    public partial class CompositionPropertyChange : ICompositionInsertion, ICompositionDeletion
    {
        IModelElement ICompositionDeletion.DeletedElement => OldValue;

        IModelElement ICompositionInsertion.AddedElement => NewValue;

        /// <inheritdoc />
        public override void Apply()
        {
            AffectedElement.SetReferencedElement((IReference)Feature, NewValue);
        }

        IElementaryChange ICompositionInsertion.ConvertIntoMove(ICompositionDeletion originChange)
        {
            return new CompositionMoveIntoProperty
            {
                AffectedElement = AffectedElement,
                OldValue = OldValue,
                NewValue = NewValue,
                Feature = Feature,
                Origin = originChange
            };
        }
    }
    public partial class AttributePropertyChange
    {
        /// <inheritdoc />
        public override void Apply()
        {
            AffectedElement.SetAttributeValue((IAttribute)Feature, Feature.Type.Parse(NewValue));
        }
    }
    public partial class ChangeTransaction
    {
        /// <inheritdoc />
        public override void Apply()
        {
            SourceChange.Apply();
        }
    }
    public partial class AssociationCollectionDeletion
    {
        /// <inheritdoc />
        public override void Apply()
        {
            AffectedElement.GetReferencedElements((IReference)Feature).Remove(DeletedElement);
        }
    }
    public partial class AssociationCollectionInsertion
    {

        /// <inheritdoc />
        public override void Apply()
        {
            AffectedElement.GetReferencedElements((IReference)Feature).Add(AddedElement);
        }
    }
    public partial class AssociationCollectionReset
    {

        /// <inheritdoc />
        public override void Apply()
        {
            AffectedElement.GetReferencedElements((IReference)Feature).Clear();
        }
    }
    public partial class AssociationListDeletion
    {

        /// <inheritdoc />
        public override void Apply()
        {
#if DEBUG
            if (AffectedElement.GetReferencedElement((IReference)Feature, Index) != DeletedElement)
            {
                Debugger.Break();
            }
#endif
            AffectedElement.GetReferencedElements((IReference)Feature).RemoveAt(Index);
        }
    }
    public partial class AssociationListInsertion
    {

        /// <inheritdoc />
        public override void Apply()
        {
            AffectedElement.GetReferencedElements((IReference)Feature).Insert(Index, AddedElement);
        }
    }
    public partial class CompositionCollectionDeletion : ICompositionDeletion
    {

        /// <inheritdoc />
        public override void Apply()
        {
            AffectedElement.GetReferencedElements((IReference)Feature).Remove(DeletedElement);
        }
    }
    public partial class CompositionCollectionInsertion : ICompositionInsertion
    {

        /// <inheritdoc />
        public override void Apply()
        {
            AffectedElement.GetReferencedElements((IReference)Feature).Add(AddedElement);
        }

        IElementaryChange ICompositionInsertion.ConvertIntoMove(ICompositionDeletion originChange)
        {
            return new CompositionMoveToCollection
            {
                AffectedElement = AffectedElement,
                MovedElement = AddedElement,
                Feature = Feature,
                Origin = originChange
            };
        }
    }
    public partial class CompositionCollectionReset
    {

        /// <inheritdoc />
        public override void Apply()
        {
            AffectedElement.GetReferencedElements((IReference)Feature).Clear();
        }
    }
    public partial class CompositionListDeletion : ICompositionDeletion
    {

        /// <inheritdoc />
        public override void Apply()
        {
#if DEBUG
            if (AffectedElement.GetReferencedElement((IReference)Feature, Index) != DeletedElement)
            {
                Debugger.Break();
            }
#endif
            AffectedElement.GetReferencedElements((IReference)Feature).RemoveAt(Index);
        }
    }
    public partial class CompositionListInsertion : ICompositionInsertion
    {

        /// <inheritdoc />
        public override void Apply()
        {
            AffectedElement.GetReferencedElements((IReference)Feature).Insert(Index, AddedElement);
        }

        IElementaryChange ICompositionInsertion.ConvertIntoMove(ICompositionDeletion originChange)
        {
            return new CompositionMoveToList
            {
                AffectedElement = AffectedElement,
                Feature = Feature,
                Index = Index,
                MovedElement = AddedElement,
                Origin = originChange
            };
        }
    }

    internal partial interface ICompositionInsertion : IElementaryChange
    {
        IElementaryChange ConvertIntoMove(ICompositionDeletion originChange);

        IModelElement AddedElement { get; }
    }

    internal partial interface ICompositionDeletion : IElementaryChange
    {
        IModelElement DeletedElement { get; }
    }

    public partial class CompositionMoveIntoProperty
    {
        /// <inheritdoc />
        public override void Apply()
        {
            AffectedElement.SetReferencedElement((IReference)Feature, NewValue);
        }
    }

    public partial class CompositionMoveToCollection
    {
        /// <inheritdoc />
        public override void Apply()
        {
            AffectedElement.GetReferencedElements((IReference)Feature).Add(MovedElement);
        }
    }

    public partial class CompositionMoveToList
    {
        /// <inheritdoc />
        public override void Apply()
        {
            AffectedElement.GetReferencedElements((IReference)Feature).Insert(Index, MovedElement);
        }
    }
    public partial class AttributeCollectionDeletion
    {

        /// <inheritdoc />
        public override void Apply()
        {
            AffectedElement.GetAttributeValues((IAttribute)Feature).Remove(Feature.Type.Parse(DeletedValue));
        }
    }
    public partial class AttributeCollectionInsertion
    {

        /// <inheritdoc />
        public override void Apply()
        {
            AffectedElement.GetAttributeValues((IAttribute)Feature).Add(Feature.Type.Parse(AddedValue));
        }
    }
    public partial class AttributeCollectionReset
    {

        /// <inheritdoc />
        public override void Apply()
        {
            AffectedElement.GetAttributeValues((IAttribute)Feature).Clear();
        }
    }
    public partial class AttributeListDeletion
    {

        /// <inheritdoc />
        public override void Apply()
        {
            AffectedElement.GetAttributeValues((IAttribute)Feature).RemoveAt(Index);
        }
    }
    public partial class AttributeListInsertion
    {

        /// <inheritdoc />
        public override void Apply()
        {
            AffectedElement.GetAttributeValues((IAttribute)Feature).Insert(Index, AddedValue);
        }
    }

    public partial class OperationCall
    {
        /// <inheritdoc />
        public override void Apply()
        {
            throw new NotImplementedException();
        }
    }
}
