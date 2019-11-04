using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NMF.Expressions;
using NMF.Expressions.Linq;
using NMF.Models.Meta;
using NMF.Utilities;

namespace NMF.Models.Dynamic
{
    [DebuggerDisplay("{Representation}")]
    public class DynamicModelElement : ModelElement
    {
        private IClass _class;
        private Dictionary<string, IAttributeProperty> _attributeProperties = new Dictionary<string, IAttributeProperty>();
        private Dictionary<string, IReferenceProperty> _referenceProperties = new Dictionary<string, IReferenceProperty>();
        private IAttributeProperty _identifierProperty;
        private IdentifierScope _identifierScope;

        public DynamicModelElement(IClass @class) : this(@class, null)
        {
        }

        public DynamicModelElement(IClass @class, IClass implemented)
        {
            _class = @class;
            _class.Freeze();
            LoadProperties(@class, new HashSet<IClass>((implemented ?? ClassInstance).Closure(c => c.BaseTypes)));
        }

        private void LoadProperties(IClass @class, HashSet<IClass> alreadyImplemented)
        {
            if (!alreadyImplemented.Add(@class)) return;

            foreach (var attConstraint in @class.AttributeConstraints)
            {
                LoadAttributeConstraint(attConstraint);
            }

            foreach (var refConstraint in @class.ReferenceConstraints)
            {
                LoadReferenceConstraint(refConstraint);
            }

            foreach (var att in @class.Attributes)
            {
                var property = LoadAttribute(att);
                if (att == @class.Identifier && _identifierProperty == null)
                {
                    _identifierProperty = property;
                    _identifierScope = @class.IdentifierScope;
                }
            }

            foreach (var reference in @class.References)
            {
                LoadReference(reference);
            }

            foreach (var baseType in @class.BaseTypes)
            {
                LoadProperties(baseType, alreadyImplemented);
            }
        }

        private void LoadReference(IReference reference)
        {
            var referenceKey = reference.Name.ToUpperInvariant();
            if (_referenceProperties.TryGetValue(referenceKey, out var existingProperty))
            {
                if (existingProperty is DecomposedReferenceProperty)
                {
                    return;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            IReferenceProperty property;
            if (reference.UpperBound == 1)
            {
                if (reference.Opposite != null)
                {
                    property = new OppositeProperty(this, reference);
                }
                else if (reference.IsContainment)
                {
                    property = new ContainmentProperty(this);
                }
                else
                {
                    property = new AssociationProperty();
                }
            }
            else
            {
                if (reference.Opposite != null)
                {
                    property = new OppositeCollectionProperty(this, reference);
                }
                else if (reference.IsContainment)
                {
                    property = new ContainmentCollectionProperty(this, reference);
                }
                else
                {
                    property = new AssociationCollectionProperty(reference);
                }
            }
            _referenceProperties.Add(referenceKey, property);

            if (reference.Refines != null)
            {
                GetOrCreateDecomposedReference(reference.Refines).AddComponentReferenceProperty(property);
            }
        }

        private IDecomposedReferenceProperty GetOrCreateDecomposedReference(IReference refined)
        {
            var refinedReferenceKey = refined.Name.ToUpperInvariant();
            if (!_referenceProperties.TryGetValue(refinedReferenceKey, out var refinedProperty))
            {
                refinedProperty = new DecomposedReferenceProperty();
                _referenceProperties.Add(refinedReferenceKey, refinedProperty);
            }
            return ((IDecomposedReferenceProperty)refinedProperty);
        }

        private IDecomposedAttributeProperty GetOrCreateDecomposedAttribute(IAttribute refined)
        {
            var refinedReferenceKey = refined.Name.ToUpperInvariant();
            if (!_attributeProperties.TryGetValue(refinedReferenceKey, out var refinedProperty))
            {
                refinedProperty = new DecomposedAttributeProperty();
                _attributeProperties.Add(refinedReferenceKey, refinedProperty);
            }
            return ((IDecomposedAttributeProperty)refinedProperty);
        }

        private IAttributeProperty LoadAttribute(IAttribute attribute)
        {
            var referenceKey = attribute.Name.ToUpperInvariant();
            if (_attributeProperties.TryGetValue(referenceKey, out var existingProperty))
            {
                if (existingProperty is DecomposedAttributeProperty)
                {
                    return existingProperty;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            IAttributeProperty property;
            if (attribute.UpperBound == 1)
            {
                property = new AttributeProperty(attribute);
            }
            else
            {
                property = new AttributeCollectionProperty(attribute);
            }
            _attributeProperties.Add(referenceKey, property);
            return property;
        }

        private void LoadReferenceConstraint(IReferenceConstraint refConstraint)
        {
            GetOrCreateDecomposedReference(refConstraint.Constrains).AddConstraint(refConstraint.References);
        }

        private void LoadAttributeConstraint(IAttributeConstraint attConstraint)
        {
            GetOrCreateDecomposedAttribute(attConstraint.Constrains).AddConstraint(attConstraint.Values);
        }

        public override IClass GetClass()
        {
            return _class;
        }

        public override IEnumerableExpression<IModelElement> Children
        {
            get
            {
                var children = base.Children;
                foreach (var reference in _referenceProperties.Values)
                {
                    if (reference.IsContainment)
                    {
                        if (reference.Collection != null)
                        {
                            children = children.Concat((IEnumerableExpression<IModelElement>)reference.Collection);
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                    }
                }
                return children;
            }
        }

        public override bool IsIdentified => _identifierProperty != null;

        public override string ToIdentifierString()
        {
            return _identifierProperty.Value.Value?.ToString();
        }

        internal string Representation => IsIdentified ? $"{_class.Name} {ToIdentifierString()}" : _class.Name;

        private IAttributeProperty GetAttributeProperty(string attribute)
        {
            _attributeProperties.TryGetValue(attribute, out var attributeProperty);
            return attributeProperty;
        }

        private IReferenceProperty GetReferenceProperty(string reference)
        {
            _referenceProperties.TryGetValue(reference, out var referenceProperty);
            return referenceProperty;
        }

        protected override object GetAttributeValue(string attribute, int index)
        {
            var property = GetAttributeProperty(attribute);
            if (property != null)
            {
                return property.GetValue(index);
            }
            return base.GetAttributeValue(attribute, index);
        }

        protected override IList GetCollectionForFeature(string feature)
        {
            return GetAttributeProperty(feature)?.Collection ?? GetReferenceProperty(feature)?.Collection ?? base.GetCollectionForFeature(feature);
        }

        protected override string GetCompositionName(object container)
        {
            foreach (var prop in _attributeProperties)
            {
                if (prop.Value.Collection == container)
                {
                    return prop.Key;
                }
            }
            foreach (var prop in _referenceProperties)
            {
                if (prop.Value.Collection == container)
                {
                    return prop.Key;
                }
            }
            return base.GetCompositionName(container);
        }

        protected override INotifyExpression<object> GetExpressionForAttribute(string attribute)
        {
            return GetAttributeProperty(attribute).Value ?? base.GetExpressionForAttribute(attribute);
        }

        protected override INotifyExpression<IModelElement> GetExpressionForReference(string reference)
        {
            return GetReferenceProperty(reference).ReferencedElement ?? base.GetExpressionForReference(reference);
        }

        protected override IModelElement GetModelElementForReference(string reference, int index)
        {
            var property = GetReferenceProperty(reference.ToUpperInvariant());
            if (property != null)
            {
                return property.GetValue(index) as IModelElement;
            }
            return base.GetModelElementForReference(reference, index);
        }

        protected override string GetRelativePathForNonIdentifiedChild(IModelElement child)
        {
            foreach (var reference in _referenceProperties)
            {
                if (!reference.Value.IsContainment) continue;
                if (reference.Value.Collection != null)
                {
                    var index = reference.Value.Collection.IndexOf(child);
                    if (index != -1)
                    {
                        return ModelHelper.CreatePath(reference.Key, index);
                    }
                }
                else if (reference.Value.ReferencedElement.Value == child)
                {
                    return ModelHelper.CreatePath(reference.Key);
                }
            }
            return base.GetRelativePathForNonIdentifiedChild(child);
        }

        protected override void SetFeature(string feature, object value)
        {
            if (_attributeProperties.TryGetValue(feature, out var attributeProperty))
            {
                if (attributeProperty.Collection != null)
                {
                    attributeProperty.Collection.Add(value);
                }
                else
                {
                    attributeProperty.Value.Value = value;
                }
            }
            else if (_referenceProperties.TryGetValue(feature, out var referenceProperty))
            {
                if (referenceProperty.Collection != null)
                {
                    referenceProperty.Collection.Add(value);
                }
                else
                {
                    referenceProperty.ReferencedElement.Value = (IModelElement)value;
                }
            }
            else
            {
                base.SetFeature(feature, value);
            }
        }
    }
}
