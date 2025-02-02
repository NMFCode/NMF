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


namespace NMF.Interop.Legacy.Cmof
{
    
    
    /// <summary>
    /// The public interface for RedefinableElement
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(RedefinableElement))]
    [XmlDefaultImplementationTypeAttribute(typeof(RedefinableElement))]
    [ModelRepresentationClassAttribute("http://schema.omg.org/spec/MOF/2.0/cmof.xml#//RedefinableElement")]
    public interface IRedefinableElement : IModelElement, INamedElement
    {
        
        /// <summary>
        /// At least one of the redefinition contexts of the redefining element must be a specialization of at least one of the redefinition contexts for each redefined element.
        ///self.redefinedElement-&gt;forAll(e | self.isRedefinitionContextValid(e))
        /// </summary>
        /// <param name="diagnostics"></param>
        /// <param name="context"></param>
        bool Redefinition_context_valid(object diagnostics, object context);
        
        /// <summary>
        /// A redefining element must be consistent with each redefined element.
        ///self.redefinedElement-&gt;forAll(re | re.isConsistentWith(self))
        /// </summary>
        /// <param name="diagnostics"></param>
        /// <param name="context"></param>
        bool Redefinition_consistent(object diagnostics, object context);
        
        /// <summary>
        /// The query isConsistentWith() specifies, for any two RedefinableElements in a context in which redefinition is possible, whether redefinition would be logically consistent. By default, this is false; this operation must be overridden for subclasses of RedefinableElement to define the consistency conditions.
        ///result = false
        /// </summary>
        /// <param name="redefinee"></param>
        bool IsConsistentWith(IRedefinableElement redefinee);
        
        /// <summary>
        /// The query isRedefinitionContextValid() specifies whether the redefinition contexts of this RedefinableElement are properly related to the redefinition contexts of the specified RedefinableElement to allow this element to redefine the other. By default at least one of the redefinition contexts of this element must be a specialization of at least one of the redefinition contexts of the specified element.
        ///result = self.redefinitionContext-&gt;exists(c | redefinable.redefinitionContext-&gt;exists(r | c.allParents()-&gt;includes(r)))
        /// </summary>
        /// <param name="redefinable"></param>
        bool IsRedefinitionContextValid(IRedefinableElement redefinable);
    }
}
