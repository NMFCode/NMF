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
    /// The public interface for ElementImport
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(ElementImport))]
    [XmlDefaultImplementationTypeAttribute(typeof(ElementImport))]
    [ModelRepresentationClassAttribute("http://www.omg.org/spec/MOF/20131001/cmof.xmi#//ElementImport")]
    public interface IElementImport : IModelElement, IDirectedRelationship
    {
        
        /// <summary>
        /// Specifies the name that should be added to the namespace of the importing package in lieu of the name of the imported packagable element. The aliased name must not clash with any other member name in the importing package. By default, no alias is used.
        /// </summary>
        [DisplayNameAttribute("alias")]
        [DescriptionAttribute("Specifies the name that should be added to the namespace of the importing package" +
            " in lieu of the name of the imported packagable element. The aliased name must n" +
            "ot clash with any other member name in the importing package. By default, no ali" +
            "as is used.")]
        [CategoryAttribute("ElementImport")]
        [XmlElementNameAttribute("alias")]
        [XmlAttributeAttribute(true)]
        string Alias
        {
            get;
            set;
        }
        
        /// <summary>
        /// Specifies the visibility of the imported PackageableElement within the importing Package. The default visibility is the same as that of the imported element. If the imported element does not have a visibility, it is possible to add visibility to the element import.
        /// </summary>
        [DefaultValueAttribute(VisibilityKind.Public)]
        [DisplayNameAttribute("visibility")]
        [DescriptionAttribute(@"Specifies the visibility of the imported PackageableElement within the importing Package. The default visibility is the same as that of the imported element. If the imported element does not have a visibility, it is possible to add visibility to the element import.")]
        [CategoryAttribute("ElementImport")]
        [XmlElementNameAttribute("visibility")]
        [XmlAttributeAttribute(true)]
        VisibilityKind Visibility
        {
            get;
            set;
        }
        
        /// <summary>
        /// Specifies the PackageableElement whose name is to be added to a Namespace.
        /// </summary>
        [DisplayNameAttribute("importedElement")]
        [DescriptionAttribute("Specifies the PackageableElement whose name is to be added to a Namespace.")]
        [CategoryAttribute("ElementImport")]
        [XmlElementNameAttribute("importedElement")]
        [XmlAttributeAttribute(true)]
        IPackageableElement ImportedElement
        {
            get;
            set;
        }
        
        /// <summary>
        /// Specifies the Namespace that imports a PackageableElement from another Package.
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("importingNamespace")]
        [XmlAttributeAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        [XmlOppositeAttribute("elementImport")]
        NMF.Interop.Cmof.INamespace ImportingNamespace
        {
            get;
            set;
        }
        
        /// <summary>
        /// The visibility of an ElementImport is either public or private.
        ///self.visibility = #public or self.visibility = #private
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Visibility_public_or_private(object diagnostics, object context);
        
        /// <summary>
        /// An importedElement has either public visibility or no visibility at all.
        ///self.importedElement.visibility.notEmpty() implies self.importedElement.visibility = #public
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Imported_element_is_public(object diagnostics, object context);
        
        /// <summary>
        /// The query getName() returns the name under which the imported PackageableElement will be known in the importing namespace.
        ///result = if self.alias-&gt;notEmpty() then
        ///  self.alias
        ///else
        ///  self.importedElement.name
        ///endif
        /// </summary>
        string GetName();
    }
}
