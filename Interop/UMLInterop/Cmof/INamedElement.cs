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
    /// The public interface for NamedElement
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(NamedElement))]
    [XmlDefaultImplementationTypeAttribute(typeof(NamedElement))]
    [ModelRepresentationClassAttribute("http://www.omg.org/spec/MOF/20131001/cmof.xmi#//NamedElement")]
    public interface INamedElement : IModelElement, IElement
    {
        
        /// <summary>
        /// The name of the NamedElement.
        /// </summary>
        [DisplayNameAttribute("name")]
        [DescriptionAttribute("The name of the NamedElement.")]
        [CategoryAttribute("NamedElement")]
        [XmlElementNameAttribute("name")]
        [IdAttribute()]
        [XmlAttributeAttribute(true)]
        string Name
        {
            get;
            set;
        }
        
        /// <summary>
        /// Determines where the NamedElement appears within different Namespaces within the overall model, and its accessibility.
        /// </summary>
        [DisplayNameAttribute("visibility")]
        [DescriptionAttribute("Determines where the NamedElement appears within different Namespaces within the " +
            "overall model, and its accessibility.")]
        [CategoryAttribute("NamedElement")]
        [XmlElementNameAttribute("visibility")]
        [XmlAttributeAttribute(true)]
        Nullable<VisibilityKind> Visibility
        {
            get;
            set;
        }
        
        /// <summary>
        /// If a NamedElement is not owned by a Namespace, it does not have a visibility.
        ///namespace-&gt;isEmpty() implies visibility-&gt;isEmpty()
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Visibility_needs_ownership(object diagnostics, object context);
        
        /// <summary>
        /// If there is no name, or one of the containing namespaces has no name, there is no qualified name.
        ///(self.name-&gt;isEmpty() or self.allNamespaces()-&gt;select(ns | ns.name-&gt;isEmpty())-&gt;notEmpty())
        ///  implies self.qualifiedName-&gt;isEmpty()
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Has_no_qualified_name(object diagnostics, object context);
        
        /// <summary>
        /// When there is a name, and all of the containing namespaces have a name, the qualified name is constructed from the names of the containing namespaces.
        ///(self.name-&gt;notEmpty() and self.allNamespaces()-&gt;select(ns | ns.name-&gt;isEmpty())-&gt;isEmpty()) implies
        ///  self.qualifiedName = self.allNamespaces()-&gt;iterate( ns : Namespace; result: String = self.name | ns.name-&gt;union(self.separator())-&gt;union(result))
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Has_qualified_name(object diagnostics, object context);
        
        /// <summary>
        /// The query allNamespaces() gives the sequence of namespaces in which the NamedElement is nested, working outwards.
        ///result = if self.namespace-&gt;isEmpty()
        ///then Sequence{}
        ///else self.namespace.allNamespaces()-&gt;prepend(self.namespace)
        ///endif
        /// </summary>
        IOrderedSetExpression<NMF.Interop.Cmof.INamespace> AllNamespaces();
        
        /// <summary>
        /// The query isDistinguishableFrom() determines whether two NamedElements may logically co-exist within a Namespace. By default, two named elements are distinguishable if (a) they have unrelated types or (b) they have related types but different names.
        ///result = if self.oclIsKindOf(n.oclType) or n.oclIsKindOf(self.oclType)
        ///then ns.getNamesOfMember(self)-&gt;intersection(ns.getNamesOfMember(n))-&gt;isEmpty()
        ///else true
        ///endif
        /// </summary>
        /// <param name="n"></param>
        /// <param name="ns"></param>
        bool IsDistinguishableFrom(INamedElement n, NMF.Interop.Cmof.INamespace ns);
        
        /// <summary>
        /// The query separator() gives the string that is used to separate names when constructing a qualified name.
        ///result = &apos;::&apos;
        /// </summary>
        string Separator();
    }
}