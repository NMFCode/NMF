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
    /// The public interface for PackageMerge
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(PackageMerge))]
    [XmlDefaultImplementationTypeAttribute(typeof(PackageMerge))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//PackageMerge")]
    public interface IPackageMerge : IModelElement, IDirectedRelationship
    {
        
        /// <summary>
        /// References the Package that is to be merged with the receiving package of the PackageMerge.
        ///<p>From package UML::Packages.</p>
        /// </summary>
        [DisplayNameAttribute("mergedPackage")]
        [DescriptionAttribute("References the Package that is to be merged with the receiving package of the Pac" +
            "kageMerge.\n<p>From package UML::Packages.</p>")]
        [CategoryAttribute("PackageMerge")]
        [XmlElementNameAttribute("mergedPackage")]
        [XmlAttributeAttribute(true)]
        IPackage MergedPackage
        {
            get;
            set;
        }
        
        /// <summary>
        /// References the Package that is being extended with the contents of the merged package of the PackageMerge.
        ///<p>From package UML::Packages.</p>
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("receivingPackage")]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        [XmlAttributeAttribute(true)]
        [XmlOppositeAttribute("packageMerge")]
        IPackage ReceivingPackage
        {
            get;
            set;
        }
    }
}
