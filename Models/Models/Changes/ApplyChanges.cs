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
        public abstract void Apply(IModelRepository repository);
    }
    public partial class AssociationChange
    {
        public override void Apply(IModelRepository repository)
        {
            AffectedElement.SetReferencedElement((IReference)Feature, NewValue);
        }
    }
    public partial class CompositionChange
    {
        public override void Apply(IModelRepository repository)
        {
            AffectedElement.SetReferencedElement((IReference)Feature, NewValue);
        }
    }
    public partial class AttributeChange
    {
        public override void Apply(IModelRepository repository)
        {
            AffectedElement.SetAttributeValue((IAttribute)Feature, Feature.Type.Parse(NewValue));
        }
    }
    public partial class ElementaryChangeTransaction
    {
        public override void Apply(IModelRepository repository)
        {
            SourceChange.Apply(repository);
        }
    }
    public partial class AssociationCollectionDeletion
    {
        public override void Apply(IModelRepository repository)
        {
            AffectedElement.GetReferencedElements((IReference)Feature).Remove(repository.Resolve(DeletedElementUri));
        }
    }
    public partial class AssociationCollectionInsertion
    {

        public override void Apply(IModelRepository repository)
        {
            AffectedElement.GetReferencedElements((IReference)Feature).Add(AddedElement);
        }
    }
    public partial class AssociationCollectionReset
    {

        public override void Apply(IModelRepository repository)
        {
            AffectedElement.GetReferencedElements((IReference)Feature).Clear();
        }
    }
    public partial class AssociationListDeletion
    {

        public override void Apply(IModelRepository repository)
        {
            AffectedElement.GetReferencedElements((IReference)Feature).RemoveAt(Index);
        }
    }
    public partial class AssociationListInsertion
    {

        public override void Apply(IModelRepository repository)
        {
            AffectedElement.GetReferencedElements((IReference)Feature).Insert(Index, AddedElement);
        }
    }
    public partial class CompositionCollectionDeletion
    {

        public override void Apply(IModelRepository repository)
        {
            AffectedElement.GetReferencedElements((IReference)Feature).Remove(repository.Resolve(DeletedElementUri));
        }
    }
    public partial class CompositionCollectionInsertion
    {

        public override void Apply(IModelRepository repository)
        {
            AffectedElement.GetReferencedElements((IReference)Feature).Add(AddedElement);
        }
    }
    public partial class CompositionCollectionReset
    {

        public override void Apply(IModelRepository repository)
        {
            AffectedElement.GetReferencedElements((IReference)Feature).Clear();
        }
    }
    public partial class CompositionListDeletion
    {

        public override void Apply(IModelRepository repository)
        {
            AffectedElement.GetReferencedElements((IReference)Feature).RemoveAt(Index);
        }
    }
    public partial class CompositionListInsertion
    {

        public override void Apply(IModelRepository repository)
        {
            AffectedElement.GetReferencedElements((IReference)Feature).Insert(Index, AddedElement);
        }
    }
    public partial class AttributeCollectionDeletion
    {

        public override void Apply(IModelRepository repository)
        {
            AffectedElement.GetAttributeValues((IAttribute)Feature).Remove(Feature.Type.Parse(DeletedValue));
        }
    }
    public partial class AttributeCollectionInsertion
    {

        public override void Apply(IModelRepository repository)
        {
            AffectedElement.GetAttributeValues((IAttribute)Feature).Add(Feature.Type.Parse(AddedValue));
        }
    }
    public partial class AttributeCollectionReset
    {

        public override void Apply(IModelRepository repository)
        {
            AffectedElement.GetAttributeValues((IAttribute)Feature).Clear();
        }
    }
    public partial class AttributeListDeletion
    {

        public override void Apply(IModelRepository repository)
        {
            AffectedElement.GetAttributeValues((IAttribute)Feature).RemoveAt(Index);
        }
    }
    public partial class AttributeListInsertion
    {

        public override void Apply(IModelRepository repository)
        {
            AffectedElement.GetAttributeValues((IAttribute)Feature).Insert(Index, AddedValue);
        }
    }
}
