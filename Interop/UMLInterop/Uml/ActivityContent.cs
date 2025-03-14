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
    /// The default implementation of the ActivityContent class
    /// </summary>
    [XmlNamespaceAttribute("http://www.eclipse.org/uml2/5.0.0/UML")]
    [XmlNamespacePrefixAttribute("uml")]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//ActivityContent")]
    public abstract partial class ActivityContent : ModelElement, IActivityContent, IModelElement
    {
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _containingActivityOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveContainingActivityOperation);
        
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
                    _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//ActivityContent")));
                }
                return _classInstance;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public IActivity ContainingActivity()
        {
            System.Func<IActivityContent, IActivity> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IActivityContent, IActivity>>(_containingActivityOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method containingActivity registered. Use the meth" +
                        "od broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _containingActivityOperation.Value);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _containingActivityOperation.Value, e));
            IActivity result = handler.Invoke(this);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _containingActivityOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveContainingActivityOperation()
        {
            return ClassInstance.LookupOperation("containingActivity");
        }
        
        /// <summary>
        /// Gets the Class for this model element
        /// </summary>
        public override NMF.Models.Meta.IClass GetClass()
        {
            if ((_classInstance == null))
            {
                _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//ActivityContent")));
            }
            return _classInstance;
        }
    }
}
