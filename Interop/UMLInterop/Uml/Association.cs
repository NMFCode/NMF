//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:6.0.26
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using NMF.Collections.Generic;
using NMF.Collections.ObjectModel;
using NMF.Expressions;
using NMF.Expressions.Linq;
using NMF.Interop.Ecore;
using NMF.Models;
using NMF.Models.Collections;
using NMF.Models.Expressions;
using NMF.Models.Meta;
using NMF.Models.Repository;
using NMF.Serialization;
using NMF.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;


namespace NMF.Interop.Uml
{
    
    
    /// <summary>
    /// A link is a tuple of values that refer to typed objects.  An Association classifies a set of links, each of which is an instance of the Association.  Each value in the link refers to an instance of the type of the corresponding end of the Association.
    ///&lt;p&gt;From package UML::StructuredClassifiers.&lt;/p&gt;
    /// </summary>
    [XmlNamespaceAttribute("http://www.eclipse.org/uml2/5.0.0/UML")]
    [XmlNamespacePrefixAttribute("uml")]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//Association")]
    [DebuggerDisplayAttribute("Association {Name}")]
    public partial class Association : Classifier, IAssociation, IModelElement
    {
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _specialized_end_numberOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveSpecialized_end_numberOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _specialized_end_typesOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveSpecialized_end_typesOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _binary_associationsOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveBinary_associationsOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _association_endsOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveAssociation_endsOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _ends_must_be_typedOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveEnds_must_be_typedOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _isBinaryOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveIsBinaryOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _getEndTypesOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveGetEndTypesOperation);
        
        /// <summary>
        /// The backing field for the IsDerived property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private bool _isDerived = false;
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _isDerivedAttribute = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveIsDerivedAttribute);
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _navigableOwnedEndReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveNavigableOwnedEndReference);
        
        /// <summary>
        /// The backing field for the NavigableOwnedEnd property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private ObservableAssociationOrderedSet<IProperty> _navigableOwnedEnd;
        
        private static NMF.Models.Meta.IClass _classInstance;
        
        /// <summary>
        /// Creates a new instance
        /// </summary>
        public Association()
        {
            this._navigableOwnedEnd = new ObservableAssociationOrderedSet<IProperty>();
            this._navigableOwnedEnd.CollectionChanging += this.NavigableOwnedEndCollectionChanging;
            this._navigableOwnedEnd.CollectionChanged += this.NavigableOwnedEndCollectionChanged;
        }
        
        /// <summary>
        /// Specifies whether the Association is derived from other model elements such as other Associations.
        ///&lt;p&gt;From package UML::StructuredClassifiers.&lt;/p&gt;
        /// </summary>
        [DefaultValueAttribute(false)]
        [TypeConverterAttribute(typeof(LowercaseBooleanConverter))]
        [DisplayNameAttribute("isDerived")]
        [DescriptionAttribute("Specifies whether the Association is derived from other model elements such as ot" +
            "her Associations.\n<p>From package UML::StructuredClassifiers.</p>")]
        [CategoryAttribute("Association")]
        [XmlElementNameAttribute("isDerived")]
        [XmlAttributeAttribute(true)]
        public bool IsDerived
        {
            get
            {
                return this._isDerived;
            }
            set
            {
                if ((this._isDerived != value))
                {
                    bool old = this._isDerived;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnPropertyChanging("IsDerived", e, _isDerivedAttribute);
                    this._isDerived = value;
                    this.OnPropertyChanged("IsDerived", e, _isDerivedAttribute);
                }
            }
        }
        
        /// <summary>
        /// The navigable ends that are owned by the Association itself.
        ///&lt;p&gt;From package UML::StructuredClassifiers.&lt;/p&gt;
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [DisplayNameAttribute("navigableOwnedEnd")]
        [DescriptionAttribute("The navigable ends that are owned by the Association itself.\n<p>From package UML:" +
            ":StructuredClassifiers.</p>")]
        [CategoryAttribute("Association")]
        [XmlElementNameAttribute("navigableOwnedEnd")]
        [XmlAttributeAttribute(true)]
        [ConstantAttribute()]
        public IOrderedSetExpression<IProperty> NavigableOwnedEnd
        {
            get
            {
                return this._navigableOwnedEnd;
            }
        }
        
        IListExpression<IProperty> IAssociation.MemberEnd
        {
            get
            {
                return new AssociationMemberEndCollection(this);
            }
        }
        
        IListExpression<IProperty> IAssociation.OwnedEnd
        {
            get
            {
                return new AssociationOwnedEndCollection(this);
            }
        }
        
        /// <summary>
        /// Gets the child model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> Children
        {
            get
            {
                return base.Children.Concat(new AssociationChildrenCollection(this));
            }
        }
        
        /// <summary>
        /// Gets the referenced model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> ReferencedElements
        {
            get
            {
                return base.ReferencedElements.Concat(new AssociationReferencedElementsCollection(this));
            }
        }
        
        /// <summary>
        /// Gets the Class model for this type
        /// </summary>
        public new static NMF.Models.Meta.IClass ClassInstance
        {
            get
            {
                if ((_classInstance == null))
                {
                    _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//Association")));
                }
                return _classInstance;
            }
        }
        
        /// <summary>
        /// An Association specializing another Association has the same number of ends as the other Association.
        ///parents()-&gt;select(oclIsKindOf(Association)).oclAsType(Association)-&gt;forAll(p | p.memberEnd-&gt;size() = self.memberEnd-&gt;size())
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Specialized_end_number(object diagnostics, object context)
        {
            System.Func<IAssociation, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IAssociation, object, object, bool>>(_specialized_end_numberOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method specialized_end_number registered. Use the " +
                        "method broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _specialized_end_numberOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _specialized_end_numberOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _specialized_end_numberOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveSpecialized_end_numberOperation()
        {
            return ClassInstance.LookupOperation("specialized_end_number");
        }
        
        /// <summary>
        /// When an Association specializes another Association, every end of the specific Association corresponds to an end of the general Association, and the specific end reaches the same type or a subtype of the corresponding general end.
        ///Sequence{1..memberEnd-&gt;size()}-&gt;
        ///	forAll(i | general-&gt;select(oclIsKindOf(Association)).oclAsType(Association)-&gt;
        ///		forAll(ga | self.memberEnd-&gt;at(i).type.conformsTo(ga.memberEnd-&gt;at(i).type)))
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Specialized_end_types(object diagnostics, object context)
        {
            System.Func<IAssociation, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IAssociation, object, object, bool>>(_specialized_end_typesOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method specialized_end_types registered. Use the m" +
                        "ethod broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _specialized_end_typesOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _specialized_end_typesOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _specialized_end_typesOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveSpecialized_end_typesOperation()
        {
            return ClassInstance.LookupOperation("specialized_end_types");
        }
        
        /// <summary>
        /// Only binary Associations can be aggregations.
        ///memberEnd-&gt;exists(aggregation &lt;&gt; AggregationKind::none) implies (memberEnd-&gt;size() = 2 and memberEnd-&gt;exists(aggregation = AggregationKind::none))
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Binary_associations(object diagnostics, object context)
        {
            System.Func<IAssociation, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IAssociation, object, object, bool>>(_binary_associationsOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method binary_associations registered. Use the met" +
                        "hod broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _binary_associationsOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _binary_associationsOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _binary_associationsOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveBinary_associationsOperation()
        {
            return ClassInstance.LookupOperation("binary_associations");
        }
        
        /// <summary>
        /// Ends of Associations with more than two ends must be owned by the Association itself.
        ///memberEnd-&gt;size() &gt; 2 implies ownedEnd-&gt;includesAll(memberEnd)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Association_ends(object diagnostics, object context)
        {
            System.Func<IAssociation, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IAssociation, object, object, bool>>(_association_endsOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method association_ends registered. Use the method" +
                        " broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _association_endsOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _association_endsOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _association_endsOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveAssociation_endsOperation()
        {
            return ClassInstance.LookupOperation("association_ends");
        }
        
        /// <summary>
        /// memberEnd-&gt;forAll(type-&gt;notEmpty())
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Ends_must_be_typed(object diagnostics, object context)
        {
            System.Func<IAssociation, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IAssociation, object, object, bool>>(_ends_must_be_typedOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method ends_must_be_typed registered. Use the meth" +
                        "od broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _ends_must_be_typedOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _ends_must_be_typedOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _ends_must_be_typedOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveEnds_must_be_typedOperation()
        {
            return ClassInstance.LookupOperation("ends_must_be_typed");
        }
        
        /// <summary>
        /// Determines whether this association is a binary association, i.e. whether it has exactly two member ends.
        /// </summary>
        public bool IsBinary()
        {
            System.Func<IAssociation, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IAssociation, bool>>(_isBinaryOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method isBinary registered. Use the method broker " +
                        "to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _isBinaryOperation.Value);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _isBinaryOperation.Value, e));
            bool result = handler.Invoke(this);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _isBinaryOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveIsBinaryOperation()
        {
            return ClassInstance.LookupOperation("isBinary");
        }
        
        /// <summary>
        /// endType is derived from the types of the member ends.
        ///result = (memberEnd-&gt;collect(type)-&gt;asSet())
        ///&lt;p&gt;From package UML::StructuredClassifiers.&lt;/p&gt;
        /// </summary>
        public ISetExpression<NMF.Interop.Uml.IType> GetEndTypes()
        {
            System.Func<IAssociation, ISetExpression<NMF.Interop.Uml.IType>> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IAssociation, ISetExpression<NMF.Interop.Uml.IType>>>(_getEndTypesOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method getEndTypes registered. Use the method brok" +
                        "er to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _getEndTypesOperation.Value);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _getEndTypesOperation.Value, e));
            ISetExpression<NMF.Interop.Uml.IType> result = handler.Invoke(this);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _getEndTypesOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveGetEndTypesOperation()
        {
            return ClassInstance.LookupOperation("getEndTypes");
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveIsDerivedAttribute()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.Association.ClassInstance)).Resolve("isDerived")));
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveNavigableOwnedEndReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.Association.ClassInstance)).Resolve("navigableOwnedEnd")));
        }
        
        /// <summary>
        /// Forwards CollectionChanging notifications for the NavigableOwnedEnd property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void NavigableOwnedEndCollectionChanging(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanging("NavigableOwnedEnd", e, _navigableOwnedEndReference);
        }
        
        /// <summary>
        /// Forwards CollectionChanged notifications for the NavigableOwnedEnd property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void NavigableOwnedEndCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanged("NavigableOwnedEnd", e, _navigableOwnedEndReference);
        }
        
        /// <summary>
        /// Resolves the given URI to a child model element
        /// </summary>
        /// <returns>The model element or null if it could not be found</returns>
        /// <param name="reference">The requested reference name</param>
        /// <param name="index">The index of this reference</param>
        protected override IModelElement GetModelElementForReference(string reference, int index)
        {
            if ((reference == "NAVIGABLEOWNEDEND"))
            {
                if ((index < this.NavigableOwnedEnd.Count))
                {
                    return this.NavigableOwnedEnd[index];
                }
                else
                {
                    return null;
                }
            }
            return base.GetModelElementForReference(reference, index);
        }
        
        /// <summary>
        /// Resolves the given attribute name
        /// </summary>
        /// <returns>The attribute value or null if it could not be found</returns>
        /// <param name="attribute">The requested attribute name</param>
        /// <param name="index">The index of this attribute</param>
        protected override object GetAttributeValue(string attribute, int index)
        {
            if ((attribute == "ISDERIVED"))
            {
                return this.IsDerived;
            }
            return base.GetAttributeValue(attribute, index);
        }
        
        /// <summary>
        /// Gets the Model element collection for the given feature
        /// </summary>
        /// <returns>A non-generic list of elements</returns>
        /// <param name="feature">The requested feature</param>
        protected override System.Collections.IList GetCollectionForFeature(string feature)
        {
            if ((feature == "NAVIGABLEOWNEDEND"))
            {
                return this._navigableOwnedEnd;
            }
            return base.GetCollectionForFeature(feature);
        }
        
        /// <summary>
        /// Sets a value to the given feature
        /// </summary>
        /// <param name="feature">The requested feature</param>
        /// <param name="value">The value that should be set to that feature</param>
        protected override void SetFeature(string feature, object value)
        {
            if ((feature == "ISDERIVED"))
            {
                this.IsDerived = ((bool)(value));
                return;
            }
            base.SetFeature(feature, value);
        }
        
        /// <summary>
        /// Gets the property expression for the given attribute
        /// </summary>
        /// <returns>An incremental property expression</returns>
        /// <param name="attribute">The requested attribute in upper case</param>
        protected override NMF.Expressions.INotifyExpression<object> GetExpressionForAttribute(string attribute)
        {
            if ((attribute == "ISDERIVED"))
            {
                return Observable.Box(new IsDerivedProxy(this));
            }
            return base.GetExpressionForAttribute(attribute);
        }
        
        /// <summary>
        /// Gets the Class for this model element
        /// </summary>
        public override NMF.Models.Meta.IClass GetClass()
        {
            if ((_classInstance == null))
            {
                _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//Association")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// The collection class to to represent the children of the Association class
        /// </summary>
        public class AssociationChildrenCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private Association _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public AssociationChildrenCollection(Association parent)
            {
                this._parent = parent;
            }
            
            /// <summary>
            /// Gets the amount of elements contained in this collection
            /// </summary>
            public override int Count
            {
                get
                {
                    int count = 0;
                    return count;
                }
            }
            
            /// <summary>
            /// Registers event hooks to keep the collection up to date
            /// </summary>
            protected override void AttachCore()
            {
            }
            
            /// <summary>
            /// Unregisters all event hooks registered by AttachCore
            /// </summary>
            protected override void DetachCore()
            {
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(IModelElement item)
            {
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                return false;
            }
            
            /// <summary>
            /// Copies the contents of the collection to the given array starting from the given array index
            /// </summary>
            /// <param name="array">The array in which the elements should be copied</param>
            /// <param name="arrayIndex">The starting index</param>
            public override void CopyTo(IModelElement[] array, int arrayIndex)
            {
            }
            
            /// <summary>
            /// Removes the given item from the collection
            /// </summary>
            /// <returns>True, if the item was removed, otherwise False</returns>
            /// <param name="item">The item that should be removed</param>
            public override bool Remove(IModelElement item)
            {
                return false;
            }
            
            /// <summary>
            /// Gets an enumerator that enumerates the collection
            /// </summary>
            /// <returns>A generic enumerator</returns>
            public override IEnumerator<IModelElement> GetEnumerator()
            {
                return Enumerable.Empty<IModelElement>().GetEnumerator();
            }
        }
        
        /// <summary>
        /// The collection class to to represent the children of the Association class
        /// </summary>
        public class AssociationReferencedElementsCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private Association _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public AssociationReferencedElementsCollection(Association parent)
            {
                this._parent = parent;
            }
            
            /// <summary>
            /// Gets the amount of elements contained in this collection
            /// </summary>
            public override int Count
            {
                get
                {
                    int count = 0;
                    return count;
                }
            }
            
            /// <summary>
            /// Registers event hooks to keep the collection up to date
            /// </summary>
            protected override void AttachCore()
            {
            }
            
            /// <summary>
            /// Unregisters all event hooks registered by AttachCore
            /// </summary>
            protected override void DetachCore()
            {
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(IModelElement item)
            {
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                return false;
            }
            
            /// <summary>
            /// Copies the contents of the collection to the given array starting from the given array index
            /// </summary>
            /// <param name="array">The array in which the elements should be copied</param>
            /// <param name="arrayIndex">The starting index</param>
            public override void CopyTo(IModelElement[] array, int arrayIndex)
            {
            }
            
            /// <summary>
            /// Removes the given item from the collection
            /// </summary>
            /// <returns>True, if the item was removed, otherwise False</returns>
            /// <param name="item">The item that should be removed</param>
            public override bool Remove(IModelElement item)
            {
                return false;
            }
            
            /// <summary>
            /// Gets an enumerator that enumerates the collection
            /// </summary>
            /// <returns>A generic enumerator</returns>
            public override IEnumerator<IModelElement> GetEnumerator()
            {
                return Enumerable.Empty<IModelElement>().GetEnumerator();
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the isDerived property
        /// </summary>
        private sealed class IsDerivedProxy : ModelPropertyChange<IAssociation, bool>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public IsDerivedProxy(IAssociation modelElement) : 
                    base(modelElement, "isDerived")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override bool Value
            {
                get
                {
                    return this.ModelElement.IsDerived;
                }
                set
                {
                    this.ModelElement.IsDerived = value;
                }
            }
        }
    }
}
