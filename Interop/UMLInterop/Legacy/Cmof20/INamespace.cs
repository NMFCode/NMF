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
    /// The public interface for Namespace
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(Namespace))]
    [XmlDefaultImplementationTypeAttribute(typeof(Namespace))]
    [ModelRepresentationClassAttribute("http://schema.omg.org/spec/MOF/2.0/cmof.xml#//Namespace")]
    public interface INamespace : IModelElement, INamedElement
    {
        
        /// <summary>
        /// The ownedRule property
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("ownedRule")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [XmlOppositeAttribute("namespace")]
        [ConstantAttribute()]
        ICollectionExpression<IConstraint> OwnedRule
        {
            get;
        }
        
        /// <summary>
        /// References the ElementImports owned by the Namespace. Subsets Element::ownedElement.
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("elementImport")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [XmlOppositeAttribute("importingNamespace")]
        [ConstantAttribute()]
        IOrderedSetExpression<IElementImport> ElementImport
        {
            get;
        }
        
        /// <summary>
        /// References the PackageImports owned by the Namespace. Subsets Element::ownedElement.
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("packageImport")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [XmlOppositeAttribute("importingNamespace")]
        [ConstantAttribute()]
        IOrderedSetExpression<IPackageImport> PackageImport
        {
            get;
        }
        
        /// <summary>
        /// All the members of a Namespace are distinguishable within it.
        ///membersAreDistinguishable()
        /// </summary>
        /// <param name="diagnostics"></param>
        /// <param name="context"></param>
        bool Members_are_distinguishable(object diagnostics, object context);
        
        /// <summary>
        /// The importedMember property is derived from the ElementImports and the PackageImports. References the PackageableElements that are members of this Namespace as a result of either PackageImports or ElementImports.
        ///result = self.importMembers(self.elementImport.importedElement.asSet()->union(self.packageImport.importedPackage->collect(p | p.visibleMembers())))
        /// </summary>
        ISetExpression<IPackageableElement> GetImportedMembers();
        
        /// <summary>
        /// The query getNamesOfMember() is overridden to take account of importing. It gives back the set of names that an element would have in an importing namespace, either because it is owned, or if not owned then imported individually, or if not individually then from a package.
        ///result = if self.ownedMember->includes(element)
        ///then Set{}->include(element.name)
        ///else let elementImports: ElementImport = self.elementImport->select(ei | ei.importedElement = element) in
        ///  if elementImports->notEmpty()
        ///  then elementImports->collect(el | el.getName())
        ///  else self.packageImport->select(pi | pi.importedPackage.visibleMembers()->includes(element))->collect(pi | pi.importedPackage.getNamesOfMember(element))
        ///  endif
        ///endif
        /// </summary>
        /// <param name="element"></param>
        ISetExpression<string> GetNamesOfMember(INamedElement element);
        
        /// <summary>
        /// The query importMembers() defines which of a set of PackageableElements are actually imported into the namespace. This excludes hidden ones, i.e., those which have names that conflict with names of owned members, and also excludes elements which would have the same name when imported.
        ///result = self.excludeCollisions(imps)->select(imp | self.ownedMember->forAll(mem | mem.imp.isDistinguishableFrom(mem, self)))
        /// </summary>
        /// <param name="imps"></param>
        ISetExpression<IPackageableElement> ImportMembers(IEnumerable<IPackageableElement> imps);
        
        /// <summary>
        /// The query excludeCollisions() excludes from a set of PackageableElements any that would not be distinguishable from each other in this namespace.
        ///result = imps->reject(imp1 | imps.exists(imp2 | not imp1.isDistinguishableFrom(imp2, self)))
        /// </summary>
        /// <param name="imps"></param>
        ISetExpression<IPackageableElement> ExcludeCollisions(IEnumerable<IPackageableElement> imps);
        
        /// <summary>
        /// The Boolean query membersAreDistinguishable() determines whether all of the namespace's members are distinguishable within it.
        ///result = self.member->forAll( memb |
        ///	self.member->excluding(memb)->forAll(other |
        ///		memb.isDistinguishableFrom(other, self)))
        /// </summary>
        bool MembersAreDistinguishable();
    }
}