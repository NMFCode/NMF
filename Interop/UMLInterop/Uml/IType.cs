//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:6.0.25
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
    /// The public interface for Type
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(Type))]
    [XmlDefaultImplementationTypeAttribute(typeof(Type))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//Type")]
    public interface IType : IModelElement, IPackageableElement
    {
        
        /// <summary>
        /// Creates a(n) (binary) association between this type and the specified other type, with the specified navigabilities, aggregations, names, lower bounds, and upper bounds, and owned by this type's nearest package.
        /// </summary>
        /// <param name="end1IsNavigable">The navigability for the first end of the new association.</param>
        /// <param name="end1Aggregation">The aggregation for the first end of the new association.</param>
        /// <param name="end1Name">The name for the first end of the new association.</param>
        /// <param name="end1Lower">The lower bound for the first end of the new association.</param>
        /// <param name="end1Upper">The upper bound for the first end of the new association.</param>
        /// <param name="end1Type">The type for the first end of the new association.</param>
        /// <param name="end2IsNavigable">The navigability for the second end of the new association.</param>
        /// <param name="end2Aggregation">The aggregation for the second end of the new association.</param>
        /// <param name="end2Name">The name for the second end of the new association.</param>
        /// <param name="end2Lower">The lower bound for the second end of the new association.</param>
        /// <param name="end2Upper">The upper bound for the second end of the new association.</param>
        IAssociation CreateAssociation(bool end1IsNavigable, AggregationKind end1Aggregation, string end1Name, int end1Lower, object end1Upper, NMF.Interop.Uml.IType end1Type, bool end2IsNavigable, AggregationKind end2Aggregation, string end2Name, int end2Lower, object end2Upper);
        
        /// <summary>
        /// Retrieves the associations in which this type is involved.
        /// </summary>
        ISetExpression<IAssociation> GetAssociations();
        
        /// <summary>
        /// The query conformsTo() gives true for a Type that conforms to another. By default, two Types do not conform to each other. This query is intended to be redefined for specific conformance situations.
        ///result = (false)
        ///<p>From package UML::CommonStructure.</p>
        /// </summary>
        /// <param name="other"></param>
        bool ConformsTo(NMF.Interop.Uml.IType other);
    }
}
