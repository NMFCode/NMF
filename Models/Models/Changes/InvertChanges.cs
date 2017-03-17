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
        public abstract void Invert(IModelRepository repository);
    }
    public partial class AssociationChange
    {
        public override void Invert(IModelRepository repository)
        {
            AffectedElement.SetReferencedElement((IReference)Feature, OldValue);
        }
    }
    public partial class CompositionChange
    {
        public override void Invert(IModelRepository repository)
        {
            AffectedElement.SetReferencedElement((IReference)Feature, OldValue);
        }
    }
    public partial class AttributeChange
    {
        public override void Invert(IModelRepository repository)
        {
            AffectedElement.SetAttributeValue((IAttribute)Feature, Feature.Type.Parse(OldValue));
        }
    }
    public partial class ElementaryChangeTransaction
    {
        public override void Invert(IModelRepository repository)
        {
            for (int i = NestedChanges.Count - 1; i >= 0; i--)
            {
                NestedChanges[i].Invert(repository);
            }
            SourceChange.Invert(repository);
        }
    }
    public partial class AssociationCollectionDeletion
    {
        public override void Invert(IModelRepository repository)
        {
            AffectedElement.GetReferencedElements((IReference)Feature).Remove(repository.Resolve(DeletedElementUri));
        }
    }
    public partial class AssociationCollectionInsertion
    {

        public override void Invert(IModelRepository repository)
        {
            AffectedElement.GetReferencedElements((IReference)Feature).Remove(AddedElement);
        }
    }
    public partial class AssociationCollectionReset
    {
        public override void Invert(IModelRepository repository)
        {
            throw new NotImplementedException();
        }
    }
    public partial class AssociationListDeletion
    {
        public override void Invert(IModelRepository repository)
        {
            AffectedElement.GetReferencedElements((IReference)Feature).Insert(Index, DeletedElement);
        }
    }
    public partial class AssociationListInsertion
    {

        public override void Invert(IModelRepository repository)
        {
            AffectedElement.GetReferencedElements((IReference)Feature).RemoveAt(Index);
        }
    }
    public partial class CompositionCollectionDeletion
    {

        public override void Invert(IModelRepository repository)
        {
            AffectedElement.GetReferencedElements((IReference)Feature).Add(DeletedElement);
        }
    }
    public partial class CompositionCollectionInsertion
    {

        public override void Invert(IModelRepository repository)
        {
            AffectedElement.GetReferencedElements((IReference)Feature).Remove(AddedElement);
        }
    }
    public partial class CompositionCollectionReset
    {

        public override void Invert(IModelRepository repository)
        {
            throw new NotImplementedException();
        }
    }
    public partial class CompositionListDeletion
    {

        public override void Invert(IModelRepository repository)
        {
            AffectedElement.GetReferencedElements((IReference)Feature).Insert(Index, DeletedElement);
        }
    }
    public partial class CompositionListInsertion
    {

        public override void Invert(IModelRepository repository)
        {
            AffectedElement.GetReferencedElements((IReference)Feature).RemoveAt(Index);
        }
    }
    public partial class AttributeCollectionDeletion
    {

        public override void Invert(IModelRepository repository)
        {
            AffectedElement.GetAttributeValues((IAttribute)Feature).Add(Feature.Type.Parse(DeletedValue));
        }
    }
    public partial class AttributeCollectionInsertion
    {

        public override void Invert(IModelRepository repository)
        {
            AffectedElement.GetAttributeValues((IAttribute)Feature).Remove(Feature.Type.Parse(AddedValue));
        }
    }
    public partial class AttributeCollectionReset
    {

        public override void Invert(IModelRepository repository)
        {
            throw new NotImplementedException();
        }
    }
    public partial class AttributeListDeletion
    {

        public override void Invert(IModelRepository repository)
        {
            AffectedElement.GetAttributeValues((IAttribute)Feature).Insert(Index, Feature.Type.Parse(DeletedValue));
        }
    }
    public partial class AttributeListInsertion
    {
        public override void Invert(IModelRepository repository)
        {
            AffectedElement.GetAttributeValues((IAttribute)Feature).RemoveAt(Index);
        }
    }
}
