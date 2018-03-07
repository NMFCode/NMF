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
        public abstract void Apply();
    }
    public partial class AssociationPropertyChange
    {
        public override void Apply()
        {
            AffectedElement.SetReferencedElement((IReference)Feature, NewValue);
        }
    }
    public partial class CompositionPropertyChange : ICompositionInsertion, ICompositionDeletion
    {
        public IModelElement DeletedElement => OldValue;

        public IModelElement AddedElement => NewValue;

        public override void Apply()
        {
            AffectedElement.SetReferencedElement((IReference)Feature, NewValue);
        }

        public IElementaryChange ConvertIntoMove(ICompositionDeletion originChange)
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
        public override void Apply()
        {
            AffectedElement.SetAttributeValue((IAttribute)Feature, Feature.Type.Parse(NewValue));
        }
    }
    public partial class ChangeTransaction
    {
        public override void Apply()
        {
            SourceChange.Apply();
        }
    }
    public partial class AssociationCollectionDeletion
    {
        public override void Apply()
        {
            AffectedElement.GetReferencedElements((IReference)Feature).Remove(DeletedElement);
        }
    }
    public partial class AssociationCollectionInsertion
    {

        public override void Apply()
        {
            AffectedElement.GetReferencedElements((IReference)Feature).Add(AddedElement);
        }
    }
    public partial class AssociationCollectionReset
    {

        public override void Apply()
        {
            AffectedElement.GetReferencedElements((IReference)Feature).Clear();
        }
    }
    public partial class AssociationListDeletion
    {

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

        public override void Apply()
        {
            AffectedElement.GetReferencedElements((IReference)Feature).Insert(Index, AddedElement);
        }
    }
    public partial class CompositionCollectionDeletion : ICompositionDeletion
    {

        public override void Apply()
        {
            AffectedElement.GetReferencedElements((IReference)Feature).Remove(DeletedElement);
        }
    }
    public partial class CompositionCollectionInsertion : ICompositionInsertion
    {

        public override void Apply()
        {
            AffectedElement.GetReferencedElements((IReference)Feature).Add(AddedElement);
        }

        public IElementaryChange ConvertIntoMove(ICompositionDeletion originChange)
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

        public override void Apply()
        {
            AffectedElement.GetReferencedElements((IReference)Feature).Clear();
        }
    }
    public partial class CompositionListDeletion : ICompositionDeletion
    {

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

        public override void Apply()
        {
            AffectedElement.GetReferencedElements((IReference)Feature).Insert(Index, AddedElement);
        }

        public IElementaryChange ConvertIntoMove(ICompositionDeletion originChange)
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

    public partial interface ICompositionInsertion : IElementaryChange
    {
        IElementaryChange ConvertIntoMove(ICompositionDeletion originChange);

        IModelElement AddedElement { get; }
    }

    public partial interface ICompositionDeletion : IElementaryChange
    {
        IModelElement DeletedElement { get; }
    }

    public partial class CompositionMoveIntoProperty
    {
        public override void Apply()
        {
            AffectedElement.SetReferencedElement((IReference)Feature, NewValue);
        }
    }

    public partial class CompositionMoveToCollection
    {
        public override void Apply()
        {
            AffectedElement.GetReferencedElements((IReference)Feature).Add(MovedElement);
        }
    }

    public partial class CompositionMoveToList
    {
        public override void Apply()
        {
            AffectedElement.GetReferencedElements((IReference)Feature).Insert(Index, MovedElement);
        }
    }
    public partial class AttributeCollectionDeletion
    {

        public override void Apply()
        {
            AffectedElement.GetAttributeValues((IAttribute)Feature).Remove(Feature.Type.Parse(DeletedValue));
        }
    }
    public partial class AttributeCollectionInsertion
    {

        public override void Apply()
        {
            AffectedElement.GetAttributeValues((IAttribute)Feature).Add(Feature.Type.Parse(AddedValue));
        }
    }
    public partial class AttributeCollectionReset
    {

        public override void Apply()
        {
            AffectedElement.GetAttributeValues((IAttribute)Feature).Clear();
        }
    }
    public partial class AttributeListDeletion
    {

        public override void Apply()
        {
            AffectedElement.GetAttributeValues((IAttribute)Feature).RemoveAt(Index);
        }
    }
    public partial class AttributeListInsertion
    {

        public override void Apply()
        {
            AffectedElement.GetAttributeValues((IAttribute)Feature).Insert(Index, AddedValue);
        }
    }

    public partial class OperationCall
    {
        public override void Apply()
        {
            throw new NotImplementedException();
        }
    }
}
