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
    /// The public interface for Association
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(Association))]
    [XmlDefaultImplementationTypeAttribute(typeof(Association))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//Association")]
    public interface IAssociation : IModelElement, IRelationship, IClassifier
    {
        
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
        bool IsDerived
        {
            get;
            set;
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
        IOrderedSetExpression<IProperty> NavigableOwnedEnd
        {
            get;
        }
        
        /// <summary>
        /// Each end represents participation of instances of the Classifier connected to the end in links of the Association.
        ///&lt;p&gt;From package UML::StructuredClassifiers.&lt;/p&gt;
        /// </summary>
        [LowerBoundAttribute(2)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [DisplayNameAttribute("memberEnd")]
        [DescriptionAttribute("Each end represents participation of instances of the Classifier connected to the" +
            " end in links of the Association.\n<p>From package UML::StructuredClassifiers.</p" +
            ">")]
        [CategoryAttribute("Association")]
        [XmlElementNameAttribute("memberEnd")]
        [XmlAttributeAttribute(true)]
        [XmlOppositeAttribute("association")]
        [ConstantAttribute()]
        IListExpression<IProperty> MemberEnd
        {
            get;
        }
        
        /// <summary>
        /// The ends that are owned by the Association itself.
        ///&lt;p&gt;From package UML::StructuredClassifiers.&lt;/p&gt;
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [DisplayNameAttribute("ownedEnd")]
        [DescriptionAttribute("The ends that are owned by the Association itself.\n<p>From package UML::Structure" +
            "dClassifiers.</p>")]
        [CategoryAttribute("Association")]
        [XmlElementNameAttribute("ownedEnd")]
        [XmlAttributeAttribute(true)]
        [XmlOppositeAttribute("owningAssociation")]
        [ConstantAttribute()]
        IListExpression<IProperty> OwnedEnd
        {
            get;
        }
        
        /// <summary>
        /// An Association specializing another Association has the same number of ends as the other Association.
        ///parents()-&gt;select(oclIsKindOf(Association)).oclAsType(Association)-&gt;forAll(p | p.memberEnd-&gt;size() = self.memberEnd-&gt;size())
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Specialized_end_number(object diagnostics, object context);
        
        /// <summary>
        /// When an Association specializes another Association, every end of the specific Association corresponds to an end of the general Association, and the specific end reaches the same type or a subtype of the corresponding general end.
        ///Sequence{1..memberEnd-&gt;size()}-&gt;
        ///	forAll(i | general-&gt;select(oclIsKindOf(Association)).oclAsType(Association)-&gt;
        ///		forAll(ga | self.memberEnd-&gt;at(i).type.conformsTo(ga.memberEnd-&gt;at(i).type)))
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Specialized_end_types(object diagnostics, object context);
        
        /// <summary>
        /// Only binary Associations can be aggregations.
        ///memberEnd-&gt;exists(aggregation &lt;&gt; AggregationKind::none) implies (memberEnd-&gt;size() = 2 and memberEnd-&gt;exists(aggregation = AggregationKind::none))
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Binary_associations(object diagnostics, object context);
        
        /// <summary>
        /// Ends of Associations with more than two ends must be owned by the Association itself.
        ///memberEnd-&gt;size() &gt; 2 implies ownedEnd-&gt;includesAll(memberEnd)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Association_ends(object diagnostics, object context);
        
        /// <summary>
        /// memberEnd-&gt;forAll(type-&gt;notEmpty())
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Ends_must_be_typed(object diagnostics, object context);
        
        /// <summary>
        /// Determines whether this association is a binary association, i.e. whether it has exactly two member ends.
        /// </summary>
        bool IsBinary();
        
        /// <summary>
        /// endType is derived from the types of the member ends.
        ///result = (memberEnd-&gt;collect(type)-&gt;asSet())
        ///&lt;p&gt;From package UML::StructuredClassifiers.&lt;/p&gt;
        /// </summary>
        ISetExpression<NMF.Interop.Uml.IType> GetEndTypes();
    }
}
