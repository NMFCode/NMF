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


namespace NMF.Interop.Cmof
{
    
    
    /// <summary>
    /// AggregationKind is an enumeration type that specifies the literals for defining the kind of aggregation of a property.
    /// </summary>
    [TypeConverterAttribute(typeof(AggregationKindConverter))]
    [ModelRepresentationClassAttribute("http://www.omg.org/spec/MOF/20131001/cmof.xmi#//AggregationKind")]
    public enum AggregationKind
    {
        
        /// <summary>
        ///Indicates that the property has no aggregation.
        ///</summary>
        None = 0,
        
        /// <summary>
        ///Indicates that the property has a shared aggregation.
        ///</summary>
        Shared = 1,
        
        /// <summary>
        ///Indicates that the property is aggregated compositely, i.e., the composite object has responsibility for the existence and storage of the composed objects (parts).
        ///</summary>
        Composite = 2,
    }
}