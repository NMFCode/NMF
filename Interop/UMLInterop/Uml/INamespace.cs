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
    /// The public interface for Namespace
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(Namespace))]
    [XmlDefaultImplementationTypeAttribute(typeof(Namespace))]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//Namespace")]
    public interface INamespace : IModelElement, INamedElement
    {
        
        /// <summary>
        /// Specifies a set of Constraints owned by this Namespace.
        ///<p>From package UML::CommonStructure.</p>
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("ownedRule")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [XmlOppositeAttribute("context")]
        [ConstantAttribute()]
        IOrderedSetExpression<IConstraint> OwnedRule
        {
            get;
        }
        
        /// <summary>
        /// References the ElementImports owned by the Namespace.
        ///<p>From package UML::CommonStructure.</p>
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
        /// References the PackageImports owned by the Namespace.
        ///<p>From package UML::CommonStructure.</p>
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
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Members_distinguishable(object diagnostics, object context);
        
        /// <summary>
        /// A Namespace cannot have a PackageImport to itself.
        ///packageImport.importedPackage.oclAsType(Namespace)->excludes(self)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Cannot_import_self(object diagnostics, object context);
        
        /// <summary>
        /// A Namespace cannot have an ElementImport to one of its ownedMembers.
        ///elementImport.importedElement.oclAsType(Element)->excludesAll(ownedMember)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        bool Cannot_import_ownedMembers(object diagnostics, object context);
        
        /// <summary>
        /// Creates an import of the specified element into this namespace with the specified visibility.
        /// </summary>
        /// <param name="element">The element to import.</param>
        /// <param name="visibility">The visibility for the new element import.</param>
        IElementImport CreateElementImport(IPackageableElement element, VisibilityKind visibility);
        
        /// <summary>
        /// Creates an import of the specified package into this namespace with the specified visibility.
        /// </summary>
        /// <param name="package_">The package to import.</param>
        /// <param name="visibility">The visibility for the new package import.</param>
        IPackageImport CreatePackageImport(IPackage package_, VisibilityKind visibility);
        
        /// <summary>
        /// Retrieves the elements imported by this namespace.
        /// </summary>
        ISetExpression<IPackageableElement> GetImportedElements();
        
        /// <summary>
        /// Retrieves the packages imported by this namespace.
        /// </summary>
        ISetExpression<IPackage> GetImportedPackages();
        
        /// <summary>
        /// 
        /// </summary>
        ISetExpression<INamedElement> GetOwnedMembers();
        
        /// <summary>
        /// The query excludeCollisions() excludes from a set of PackageableElements any that would not be distinguishable from each other in this Namespace.
        ///result = (imps->reject(imp1  | imps->exists(imp2 | not imp1.isDistinguishableFrom(imp2, self))))
        ///<p>From package UML::CommonStructure.</p>
        /// </summary>
        /// <param name="imps"></param>
        ISetExpression<IPackageableElement> ExcludeCollisions(IEnumerable<IPackageableElement> imps);
        
        /// <summary>
        /// The query getNamesOfMember() gives a set of all of the names that a member would have in a Namespace, taking importing into account. In general a member can have multiple names in a Namespace if it is imported more than once with different aliases.
        ///result = (if self.ownedMember ->includes(element)
        ///then Set{element.name}
        ///else let elementImports : Set(ElementImport) = self.elementImport->select(ei | ei.importedElement = element) in
        ///  if elementImports->notEmpty()
        ///  then
        ///     elementImports->collect(el | el.getName())->asSet()
        ///  else 
        ///     self.packageImport->select(pi | pi.importedPackage.visibleMembers().oclAsType(NamedElement)->includes(element))-> collect(pi | pi.importedPackage.getNamesOfMember(element))->asSet()
        ///  endif
        ///endif)
        ///<p>From package UML::CommonStructure.</p>
        /// </summary>
        /// <param name="element"></param>
        ISetExpression<string> GetNamesOfMember(INamedElement element);
        
        /// <summary>
        /// The query importMembers() defines which of a set of PackageableElements are actually imported into the Namespace. This excludes hidden ones, i.e., those which have names that conflict with names of ownedMembers, and it also excludes PackageableElements that would have the indistinguishable names when imported.
        ///result = (self.excludeCollisions(imps)->select(imp | self.ownedMember->forAll(mem | imp.isDistinguishableFrom(mem, self))))
        ///<p>From package UML::CommonStructure.</p>
        /// </summary>
        /// <param name="imps"></param>
        ISetExpression<IPackageableElement> ImportMembers(IEnumerable<IPackageableElement> imps);
        
        /// <summary>
        /// The importedMember property is derived as the PackageableElements that are members of this Namespace as a result of either PackageImports or ElementImports.
        ///result = (self.importMembers(elementImport.importedElement->asSet()->union(packageImport.importedPackage->collect(p | p.visibleMembers()))->asSet()))
        ///<p>From package UML::CommonStructure.</p>
        /// </summary>
        ISetExpression<IPackageableElement> GetImportedMembers();
        
        /// <summary>
        /// The Boolean query membersAreDistinguishable() determines whether all of the Namespace's members are distinguishable within it.
        ///result = (member->forAll( memb |
        ///   member->excluding(memb)->forAll(other |
        ///       memb.isDistinguishableFrom(other, self))))
        ///<p>From package UML::CommonStructure.</p>
        /// </summary>
        bool MembersAreDistinguishable();
    }
}
