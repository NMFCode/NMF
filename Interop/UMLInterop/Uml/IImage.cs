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
    /// The public interface for Image
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(Image))]
    [XmlDefaultImplementationTypeAttribute(typeof(Image))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//Image")]
    public interface IImage : IModelElement, IElement
    {
        
        /// <summary>
        /// This contains the serialization of the image according to the format. The value could represent a bitmap, image such as a GIF file, or drawing 'instructions' using a standard such as Scalable Vector Graphic (SVG) (which is XML based).
        ///<p>From package UML::Packages.</p>
        /// </summary>
        [DisplayNameAttribute("content")]
        [DescriptionAttribute(@"This contains the serialization of the image according to the format. The value could represent a bitmap, image such as a GIF file, or drawing 'instructions' using a standard such as Scalable Vector Graphic (SVG) (which is XML based).
<p>From package UML::Packages.</p>")]
        [CategoryAttribute("Image")]
        [XmlElementNameAttribute("content")]
        [XmlAttributeAttribute(true)]
        string Content
        {
            get;
            set;
        }
        
        /// <summary>
        /// This indicates the format of the content, which is how the string content should be interpreted. The following values are reserved: SVG, GIF, PNG, JPG, WMF, EMF, BMP. In addition the prefix 'MIME: ' is also reserved. This option can be used as an alternative to express the reserved values above, for example "SVG" could instead be expressed as "MIME: image/svg+xml".
        ///<p>From package UML::Packages.</p>
        /// </summary>
        [DisplayNameAttribute("format")]
        [DescriptionAttribute(@"This indicates the format of the content, which is how the string content should be interpreted. The following values are reserved: SVG, GIF, PNG, JPG, WMF, EMF, BMP. In addition the prefix 'MIME: ' is also reserved. This option can be used as an alternative to express the reserved values above, for example ""SVG"" could instead be expressed as ""MIME: image/svg+xml"".
<p>From package UML::Packages.</p>")]
        [CategoryAttribute("Image")]
        [XmlElementNameAttribute("format")]
        [XmlAttributeAttribute(true)]
        string Format
        {
            get;
            set;
        }
        
        /// <summary>
        /// This contains a location that can be used by a tool to locate the image as an alternative to embedding it in the stereotype.
        ///<p>From package UML::Packages.</p>
        /// </summary>
        [DisplayNameAttribute("location")]
        [DescriptionAttribute("This contains a location that can be used by a tool to locate the image as an alt" +
            "ernative to embedding it in the stereotype.\n<p>From package UML::Packages.</p>")]
        [CategoryAttribute("Image")]
        [XmlElementNameAttribute("location")]
        [XmlAttributeAttribute(true)]
        string Location
        {
            get;
            set;
        }
    }
}
