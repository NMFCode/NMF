//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:6.0.26
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NMF.Interop.Legacy.Cmof
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using NMF.Expressions;
    using NMF.Expressions.Linq;
    using NMF.Models;
    using NMF.Models.Meta;
    using NMF.Models.Collections;
    using NMF.Models.Expressions;
    using NMF.Collections.Generic;
    using NMF.Collections.ObjectModel;
    using NMF.Serialization;
    using NMF.Utilities;
    using System.Collections.Specialized;
    using NMF.Models.Repository;
    using System.Globalization;
    
    
    /// <summary>
    /// The public interface for ValueSpecification
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(ValueSpecification))]
    [XmlDefaultImplementationTypeAttribute(typeof(ValueSpecification))]
    [ModelRepresentationClassAttribute("http://schema.omg.org/spec/MOF/2.0/cmof.xml#//ValueSpecification")]
    public interface IValueSpecification : IModelElement, IPackageableElement, NMF.Interop.Legacy.Cmof.ITypedElement
    {
        
        /// <summary>
        /// The query isComputable() determines whether a value specification can be computed in a model. This operation cannot be fully defined in OCL. A conforming implementation is expected to deliver true for this operation for all value specifications that it can compute, and to compute all of those for which the operation is true. A conforming implementation is expected to be able to compute the value of all literals.
        ///result = false
        /// </summary>
        bool IsComputable();
        
        /// <summary>
        /// The query integerValue() gives a single Integer value when one can be computed.
        ///result = Set{}
        /// </summary>
        int IntegerValue();
        
        /// <summary>
        /// The query booleanValue() gives a single Boolean value when one can be computed.
        ///result = Set{}
        /// </summary>
        bool BooleanValue();
        
        /// <summary>
        /// The query stringValue() gives a single String value when one can be computed.
        ///result = Set{}
        /// </summary>
        string StringValue();
        
        /// <summary>
        /// The query unlimitedValue() gives a single UnlimitedNatural value when one can be computed.
        ///result = Set{}
        /// </summary>
        int UnlimitedValue();
        
        /// <summary>
        /// The query isNull() returns true when it can be computed that the value is null.
        ///result = false
        /// </summary>
        bool IsNull();
    }
}