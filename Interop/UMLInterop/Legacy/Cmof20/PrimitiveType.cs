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
    /// The PrimitiveTypes subpackage within the Core package defines the different types of primitive values that are used to define the Core metamodel. It is also intended that every metamodel based on Core will reuse the following primitive types.
    ///
    ///In Core and the UML metamodel, these primitive types are predefined and available to the Core and UML extensions at all time. These predefined value types are independent of any object model and part of the definition of the Core.
    ///
    ///
    ///A primitive type is a data type implemented by the underlying infrastructure and made available for modeling.
    /// </summary>
    [XmlNamespaceAttribute("http://schema.omg.org/spec/MOF/2.0/cmof.xml")]
    [XmlNamespacePrefixAttribute("cmof")]
    [ModelRepresentationClassAttribute("http://schema.omg.org/spec/MOF/2.0/cmof.xml#//PrimitiveType")]
    [DebuggerDisplayAttribute("PrimitiveType {Name}")]
    public partial class PrimitiveType : DataType, NMF.Interop.Legacy.Cmof.IPrimitiveType, IModelElement
    {
        
        private static NMF.Models.Meta.IClass _classInstance;
        
        /// <summary>
        /// Gets the Class model for this type
        /// </summary>
        public new static NMF.Models.Meta.IClass ClassInstance
        {
            get
            {
                if ((_classInstance == null))
                {
                    _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://schema.omg.org/spec/MOF/2.0/cmof.xml#//PrimitiveType")));
                }
                return _classInstance;
            }
        }
        
        /// <summary>
        /// Gets the Class for this model element
        /// </summary>
        public override NMF.Models.Meta.IClass GetClass()
        {
            if ((_classInstance == null))
            {
                _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://schema.omg.org/spec/MOF/2.0/cmof.xml#//PrimitiveType")));
            }
            return _classInstance;
        }
    }
}