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
    /// An extension is used to indicate that the properties of a metaclass are extended through a stereotype, and gives the ability to flexibly add (and later remove) stereotypes to classes.
    ///<p>From package UML::Packages.</p>
    /// </summary>
    [XmlNamespaceAttribute("http://www.eclipse.org/uml2/5.0.0/UML")]
    [XmlNamespacePrefixAttribute("uml")]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//Extension")]
    [DebuggerDisplayAttribute("Extension {Name}")]
    public partial class Extension : Association, NMF.Interop.Uml.IExtension, IModelElement
    {
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _non_owned_endOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveNon_owned_endOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _is_binaryOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveIs_binaryOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _getStereotypeOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveGetStereotypeOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _getStereotypeEndOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveGetStereotypeEndOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _isRequiredOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveIsRequiredOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _getMetaclassOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveGetMetaclassOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _metaclassEndOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveMetaclassEndOperation);
        
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
                    _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//Extension")));
                }
                return _classInstance;
            }
        }
        
        /// <summary>
        /// The non-owned end of an Extension is typed by a Class.
        ///metaclassEnd()->notEmpty() and metaclassEnd().type.oclIsKindOf(Class)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Non_owned_end(object diagnostics, object context)
        {
            System.Func<NMF.Interop.Uml.IExtension, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<NMF.Interop.Uml.IExtension, object, object, bool>>(_non_owned_endOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method non_owned_end registered. Use the method br" +
                        "oker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _non_owned_endOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _non_owned_endOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _non_owned_endOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveNon_owned_endOperation()
        {
            return ClassInstance.LookupOperation("non_owned_end");
        }
        
        /// <summary>
        /// An Extension is binary, i.e., it has only two memberEnds.
        ///memberEnd->size() = 2
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Is_binary(object diagnostics, object context)
        {
            System.Func<NMF.Interop.Uml.IExtension, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<NMF.Interop.Uml.IExtension, object, object, bool>>(_is_binaryOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method is_binary registered. Use the method broker" +
                        " to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _is_binaryOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _is_binaryOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _is_binaryOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveIs_binaryOperation()
        {
            return ClassInstance.LookupOperation("is_binary");
        }
        
        /// <summary>
        /// Retrieves the stereotype that extends a metaclass through this extension.
        /// </summary>
        public IStereotype GetStereotype()
        {
            System.Func<NMF.Interop.Uml.IExtension, IStereotype> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<NMF.Interop.Uml.IExtension, IStereotype>>(_getStereotypeOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method getStereotype registered. Use the method br" +
                        "oker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _getStereotypeOperation.Value);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _getStereotypeOperation.Value, e));
            IStereotype result = handler.Invoke(this);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _getStereotypeOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveGetStereotypeOperation()
        {
            return ClassInstance.LookupOperation("getStereotype");
        }
        
        /// <summary>
        /// Retrieves the extension end that is typed by a stereotype (as opposed to a metaclass).
        /// </summary>
        public IProperty GetStereotypeEnd()
        {
            System.Func<NMF.Interop.Uml.IExtension, IProperty> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<NMF.Interop.Uml.IExtension, IProperty>>(_getStereotypeEndOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method getStereotypeEnd registered. Use the method" +
                        " broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _getStereotypeEndOperation.Value);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _getStereotypeEndOperation.Value, e));
            IProperty result = handler.Invoke(this);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _getStereotypeEndOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveGetStereotypeEndOperation()
        {
            return ClassInstance.LookupOperation("getStereotypeEnd");
        }
        
        /// <summary>
        /// The query isRequired() is true if the owned end has a multiplicity with the lower bound of 1.
        ///result = (ownedEnd.lowerBound() = 1)
        ///<p>From package UML::Packages.</p>
        /// </summary>
        public bool IsRequired()
        {
            System.Func<NMF.Interop.Uml.IExtension, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<NMF.Interop.Uml.IExtension, bool>>(_isRequiredOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method isRequired registered. Use the method broke" +
                        "r to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _isRequiredOperation.Value);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _isRequiredOperation.Value, e));
            bool result = handler.Invoke(this);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _isRequiredOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveIsRequiredOperation()
        {
            return ClassInstance.LookupOperation("isRequired");
        }
        
        /// <summary>
        /// The query metaclass() returns the metaclass that is being extended (as opposed to the extending stereotype).
        ///result = (metaclassEnd().type.oclAsType(Class))
        ///<p>From package UML::Packages.</p>
        /// </summary>
        public NMF.Interop.Uml.IClass GetMetaclass()
        {
            System.Func<NMF.Interop.Uml.IExtension, NMF.Interop.Uml.IClass> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<NMF.Interop.Uml.IExtension, NMF.Interop.Uml.IClass>>(_getMetaclassOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method getMetaclass registered. Use the method bro" +
                        "ker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _getMetaclassOperation.Value);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _getMetaclassOperation.Value, e));
            NMF.Interop.Uml.IClass result = handler.Invoke(this);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _getMetaclassOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveGetMetaclassOperation()
        {
            return ClassInstance.LookupOperation("getMetaclass");
        }
        
        /// <summary>
        /// The query metaclassEnd() returns the Property that is typed by a metaclass (as opposed to a stereotype).
        ///result = (memberEnd->reject(p | ownedEnd->includes(p.oclAsType(ExtensionEnd)))->any(true))
        ///<p>From package UML::Packages.</p>
        /// </summary>
        public IProperty MetaclassEnd()
        {
            System.Func<NMF.Interop.Uml.IExtension, IProperty> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<NMF.Interop.Uml.IExtension, IProperty>>(_metaclassEndOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method metaclassEnd registered. Use the method bro" +
                        "ker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _metaclassEndOperation.Value);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _metaclassEndOperation.Value, e));
            IProperty result = handler.Invoke(this);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _metaclassEndOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveMetaclassEndOperation()
        {
            return ClassInstance.LookupOperation("metaclassEnd");
        }
        
        /// <summary>
        /// Gets the Class for this model element
        /// </summary>
        public override NMF.Models.Meta.IClass GetClass()
        {
            if ((_classInstance == null))
            {
                _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//Extension")));
            }
            return _classInstance;
        }
    }
}
