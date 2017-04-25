//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

using NMFExamples.Units;
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
using global::System.Collections;
using global::System.Collections.Generic;
using global::System.Collections.ObjectModel;
using global::System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace NMFExamples.Probfunction
{
    
    
    /// <summary>
    /// The default implementation of the LognormalDistribution class
    /// </summary>
    [XmlNamespaceAttribute("http://sdq.ipd.uka.de/ProbFunction/1.0")]
    [XmlNamespacePrefixAttribute("probfunction")]
    [ModelRepresentationClassAttribute("http://sdq.ipd.uka.de/ProbFunction/1.0#//LognormalDistribution")]
    public partial class LognormalDistribution : ContinuousPDF, ILognormalDistribution, IModelElement
    {
        
        /// <summary>
        /// The backing field for the Mu property
        /// </summary>
        private double _mu;
        
        private static Lazy<ITypedElement> _muAttribute = new Lazy<ITypedElement>(RetrieveMuAttribute);
        
        /// <summary>
        /// The backing field for the Sigma property
        /// </summary>
        private double _sigma;
        
        private static Lazy<ITypedElement> _sigmaAttribute = new Lazy<ITypedElement>(RetrieveSigmaAttribute);
        
        private static IClass _classInstance;
        
        /// <summary>
        /// The mu property
        /// </summary>
        [XmlElementNameAttribute("mu")]
        [XmlAttributeAttribute(true)]
        public double Mu
        {
            get
            {
                return this._mu;
            }
            set
            {
                if ((this._mu != value))
                {
                    double old = this._mu;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnMuChanging(e);
                    this.OnPropertyChanging("Mu", e, _muAttribute);
                    this._mu = value;
                    this.OnMuChanged(e);
                    this.OnPropertyChanged("Mu", e, _muAttribute);
                }
            }
        }
        
        /// <summary>
        /// The sigma property
        /// </summary>
        [XmlElementNameAttribute("sigma")]
        [XmlAttributeAttribute(true)]
        public double Sigma
        {
            get
            {
                return this._sigma;
            }
            set
            {
                if ((this._sigma != value))
                {
                    double old = this._sigma;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnSigmaChanging(e);
                    this.OnPropertyChanging("Sigma", e, _sigmaAttribute);
                    this._sigma = value;
                    this.OnSigmaChanged(e);
                    this.OnPropertyChanged("Sigma", e, _sigmaAttribute);
                }
            }
        }
        
        /// <summary>
        /// Gets the Class model for this type
        /// </summary>
        public new static IClass ClassInstance
        {
            get
            {
                if ((_classInstance == null))
                {
                    _classInstance = ((IClass)(MetaRepository.Instance.Resolve("http://sdq.ipd.uka.de/ProbFunction/1.0#//LognormalDistribution")));
                }
                return _classInstance;
            }
        }
        
        /// <summary>
        /// Gets fired before the Mu property changes its value
        /// </summary>
        public event global::System.EventHandler<ValueChangedEventArgs> MuChanging;
        
        /// <summary>
        /// Gets fired when the Mu property changed its value
        /// </summary>
        public event global::System.EventHandler<ValueChangedEventArgs> MuChanged;
        
        /// <summary>
        /// Gets fired before the Sigma property changes its value
        /// </summary>
        public event global::System.EventHandler<ValueChangedEventArgs> SigmaChanging;
        
        /// <summary>
        /// Gets fired when the Sigma property changed its value
        /// </summary>
        public event global::System.EventHandler<ValueChangedEventArgs> SigmaChanged;
        
        private static ITypedElement RetrieveMuAttribute()
        {
            return ((ITypedElement)(((ModelElement)(NMFExamples.Probfunction.LognormalDistribution.ClassInstance)).Resolve("mu")));
        }
        
        /// <summary>
        /// Raises the MuChanging event
        /// </summary>
        /// <param name="eventArgs">The event data</param>
        protected virtual void OnMuChanging(ValueChangedEventArgs eventArgs)
        {
            global::System.EventHandler<ValueChangedEventArgs> handler = this.MuChanging;
            if ((handler != null))
            {
                handler.Invoke(this, eventArgs);
            }
        }
        
        /// <summary>
        /// Raises the MuChanged event
        /// </summary>
        /// <param name="eventArgs">The event data</param>
        protected virtual void OnMuChanged(ValueChangedEventArgs eventArgs)
        {
            global::System.EventHandler<ValueChangedEventArgs> handler = this.MuChanged;
            if ((handler != null))
            {
                handler.Invoke(this, eventArgs);
            }
        }
        
        private static ITypedElement RetrieveSigmaAttribute()
        {
            return ((ITypedElement)(((ModelElement)(NMFExamples.Probfunction.LognormalDistribution.ClassInstance)).Resolve("sigma")));
        }
        
        /// <summary>
        /// Raises the SigmaChanging event
        /// </summary>
        /// <param name="eventArgs">The event data</param>
        protected virtual void OnSigmaChanging(ValueChangedEventArgs eventArgs)
        {
            global::System.EventHandler<ValueChangedEventArgs> handler = this.SigmaChanging;
            if ((handler != null))
            {
                handler.Invoke(this, eventArgs);
            }
        }
        
        /// <summary>
        /// Raises the SigmaChanged event
        /// </summary>
        /// <param name="eventArgs">The event data</param>
        protected virtual void OnSigmaChanged(ValueChangedEventArgs eventArgs)
        {
            global::System.EventHandler<ValueChangedEventArgs> handler = this.SigmaChanged;
            if ((handler != null))
            {
                handler.Invoke(this, eventArgs);
            }
        }
        
        /// <summary>
        /// Resolves the given attribute name
        /// </summary>
        /// <returns>The attribute value or null if it could not be found</returns>
        /// <param name="attribute">The requested attribute name</param>
        /// <param name="index">The index of this attribute</param>
        protected override object GetAttributeValue(string attribute, int index)
        {
            if ((attribute == "MU"))
            {
                return this.Mu;
            }
            if ((attribute == "SIGMA"))
            {
                return this.Sigma;
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
            if ((feature == "MU"))
            {
                this.Mu = ((double)(value));
                return;
            }
            if ((feature == "SIGMA"))
            {
                this.Sigma = ((double)(value));
                return;
            }
            base.SetFeature(feature, value);
        }
        
        /// <summary>
        /// Gets the Class for this model element
        /// </summary>
        public override IClass GetClass()
        {
            if ((_classInstance == null))
            {
                _classInstance = ((IClass)(MetaRepository.Instance.Resolve("http://sdq.ipd.uka.de/ProbFunction/1.0#//LognormalDistribution")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the mu property
        /// </summary>
        private sealed class MuProxy : ModelPropertyChange<ILognormalDistribution, double>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public MuProxy(ILognormalDistribution modelElement) : 
                    base(modelElement, "mu")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override double Value
            {
                get
                {
                    return this.ModelElement.Mu;
                }
                set
                {
                    this.ModelElement.Mu = value;
                }
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the sigma property
        /// </summary>
        private sealed class SigmaProxy : ModelPropertyChange<ILognormalDistribution, double>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public SigmaProxy(ILognormalDistribution modelElement) : 
                    base(modelElement, "sigma")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override double Value
            {
                get
                {
                    return this.ModelElement.Sigma;
                }
                set
                {
                    this.ModelElement.Sigma = value;
                }
            }
        }
    }
}
