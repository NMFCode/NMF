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
    /// A Continuation is a syntactic way to define continuations of different branches of an alternative CombinedFragment. Continuations are intuitively similar to labels representing intermediate points in a flow of control.
    ///<p>From package UML::Interactions.</p>
    /// </summary>
    [XmlNamespaceAttribute("http://www.eclipse.org/uml2/5.0.0/UML")]
    [XmlNamespacePrefixAttribute("uml")]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//Continuation")]
    [DebuggerDisplayAttribute("Continuation {Name}")]
    public partial class Continuation : InteractionFragment, IContinuation, IModelElement
    {
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _first_or_last_interaction_fragmentOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveFirst_or_last_interaction_fragmentOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _same_nameOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveSame_nameOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _globalOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveGlobalOperation);
        
        /// <summary>
        /// The backing field for the Setting property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private bool _setting = true;
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _settingAttribute = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveSettingAttribute);
        
        private static NMF.Models.Meta.IClass _classInstance;
        
        /// <summary>
        /// True: when the Continuation is at the end of the enclosing InteractionFragment and False when it is in the beginning.
        ///<p>From package UML::Interactions.</p>
        /// </summary>
        [DefaultValueAttribute(true)]
        [TypeConverterAttribute(typeof(LowercaseBooleanConverter))]
        [DisplayNameAttribute("setting")]
        [DescriptionAttribute("True: when the Continuation is at the end of the enclosing InteractionFragment an" +
            "d False when it is in the beginning.\n<p>From package UML::Interactions.</p>")]
        [CategoryAttribute("Continuation")]
        [XmlElementNameAttribute("setting")]
        [XmlAttributeAttribute(true)]
        public bool Setting
        {
            get
            {
                return this._setting;
            }
            set
            {
                if ((this._setting != value))
                {
                    bool old = this._setting;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnPropertyChanging("Setting", e, _settingAttribute);
                    this._setting = value;
                    this.OnPropertyChanged("Setting", e, _settingAttribute);
                }
            }
        }
        
        /// <summary>
        /// Gets the Class model for this type
        /// </summary>
        public new static NMF.Models.Meta.IClass ClassInstance
        {
            get
            {
                if ((_classInstance == null))
                {
                    _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//Continuation")));
                }
                return _classInstance;
            }
        }
        
        /// <summary>
        /// Continuations always occur as the very first InteractionFragment or the very last InteractionFragment of the enclosing InteractionOperand.
        /// enclosingOperand->notEmpty() and 
        /// let peerFragments : OrderedSet(InteractionFragment) =  enclosingOperand.fragment in 
        ///   ( peerFragments->notEmpty() and 
        ///   ((peerFragments->first() = self) or  (peerFragments->last() = self)))
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool First_or_last_interaction_fragment(object diagnostics, object context)
        {
            System.Func<IContinuation, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IContinuation, object, object, bool>>(_first_or_last_interaction_fragmentOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method first_or_last_interaction_fragment register" +
                        "ed. Use the method broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _first_or_last_interaction_fragmentOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _first_or_last_interaction_fragmentOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _first_or_last_interaction_fragmentOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveFirst_or_last_interaction_fragmentOperation()
        {
            return ClassInstance.LookupOperation("first_or_last_interaction_fragment");
        }
        
        /// <summary>
        /// Across all Interaction instances having the same context value, every Lifeline instance covered by a Continuation (self) must be common with one covered Lifeline instance of all other Continuation instances with the same name as self, and every Lifeline instance covered by a Continuation instance with the same name as self must be common with one covered Lifeline instance of self. Lifeline instances are common if they have the same selector and represents associationEnd values.
        ///enclosingOperand.combinedFragment->notEmpty() and
        ///let parentInteraction : Set(Interaction) = 
        ///enclosingOperand.combinedFragment->closure(enclosingOperand.combinedFragment)->
        ///collect(enclosingInteraction).oclAsType(Interaction)->asSet()
        ///in 
        ///(parentInteraction->size() = 1) 
        ///and let peerInteractions : Set(Interaction) =
        /// (parentInteraction->union(parentInteraction->collect(_'context')->collect(behavior)->
        /// select(oclIsKindOf(Interaction)).oclAsType(Interaction)->asSet())->asSet()) in
        /// (peerInteractions->notEmpty()) and 
        ///  let combinedFragments1 : Set(CombinedFragment) = peerInteractions.fragment->
        /// select(oclIsKindOf(CombinedFragment)).oclAsType(CombinedFragment)->asSet() in
        ///   combinedFragments1->notEmpty() and  combinedFragments1->closure(operand.fragment->
        ///   select(oclIsKindOf(CombinedFragment)).oclAsType(CombinedFragment))->asSet().operand.fragment->
        ///   select(oclIsKindOf(Continuation)).oclAsType(Continuation)->asSet()->
        ///   forAll(c : Continuation |  (c.name = self.name) implies 
        ///  (c.covered->asSet()->forAll(cl : Lifeline | --  cl must be common to one lifeline covered by self
        ///  self.covered->asSet()->
        ///  select(represents = cl.represents and selector = cl.selector)->asSet()->size()=1))
        ///   and
        /// (self.covered->asSet()->forAll(cl : Lifeline | --  cl must be common to one lifeline covered by c
        /// c.covered->asSet()->
        ///  select(represents = cl.represents and selector = cl.selector)->asSet()->size()=1))
        ///  )
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Same_name(object diagnostics, object context)
        {
            System.Func<IContinuation, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IContinuation, object, object, bool>>(_same_nameOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method same_name registered. Use the method broker" +
                        " to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _same_nameOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _same_nameOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _same_nameOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveSame_nameOperation()
        {
            return ClassInstance.LookupOperation("same_name");
        }
        
        /// <summary>
        /// Continuations are always global in the enclosing InteractionFragment e.g., it always covers all Lifelines covered by the enclosing InteractionOperator.
        ///enclosingOperand->notEmpty() and
        ///  let operandLifelines : Set(Lifeline) =  enclosingOperand.covered in 
        ///    (operandLifelines->notEmpty() and 
        ///    operandLifelines->forAll(ol :Lifeline |self.covered->includes(ol)))
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Global(object diagnostics, object context)
        {
            System.Func<IContinuation, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IContinuation, object, object, bool>>(_globalOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method global registered. Use the method broker to" +
                        " register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _globalOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _globalOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _globalOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveGlobalOperation()
        {
            return ClassInstance.LookupOperation("global");
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveSettingAttribute()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.Continuation.ClassInstance)).Resolve("setting")));
        }
        
        /// <summary>
        /// Resolves the given attribute name
        /// </summary>
        /// <returns>The attribute value or null if it could not be found</returns>
        /// <param name="attribute">The requested attribute name</param>
        /// <param name="index">The index of this attribute</param>
        protected override object GetAttributeValue(string attribute, int index)
        {
            if ((attribute == "SETTING"))
            {
                return this.Setting;
            }
            return base.GetAttributeValue(attribute, index);
        }
        
        /// <summary>
        /// Sets a value to the given feature
        /// </summary>
        /// <param name="feature">The requested feature</param>
        /// <param name="value">The value that should be set to that feature</param>
        protected override void SetFeature(string feature, object value)
        {
            if ((feature == "SETTING"))
            {
                this.Setting = ((bool)(value));
                return;
            }
            base.SetFeature(feature, value);
        }
        
        /// <summary>
        /// Gets the property expression for the given attribute
        /// </summary>
        /// <returns>An incremental property expression</returns>
        /// <param name="attribute">The requested attribute in upper case</param>
        protected override NMF.Expressions.INotifyExpression<object> GetExpressionForAttribute(string attribute)
        {
            if ((attribute == "SETTING"))
            {
                return Observable.Box(new SettingProxy(this));
            }
            return base.GetExpressionForAttribute(attribute);
        }
        
        /// <summary>
        /// Gets the Class for this model element
        /// </summary>
        public override NMF.Models.Meta.IClass GetClass()
        {
            if ((_classInstance == null))
            {
                _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//Continuation")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the setting property
        /// </summary>
        private sealed class SettingProxy : ModelPropertyChange<IContinuation, bool>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public SettingProxy(IContinuation modelElement) : 
                    base(modelElement, "setting")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override bool Value
            {
                get
                {
                    return this.ModelElement.Setting;
                }
                set
                {
                    this.ModelElement.Setting = value;
                }
            }
        }
    }
}
