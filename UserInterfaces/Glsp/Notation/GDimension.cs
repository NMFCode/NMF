//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:6.0.21
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
using System.Linq;

namespace NMF.Glsp.Notation
{
    
    
    /// <summary>
    /// The default implementation of the GDimension class
    /// </summary>
    [XmlNamespaceAttribute("http://www.eclipse.org/glsp/notation")]
    [XmlNamespacePrefixAttribute("notation")]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/glsp/notation#//GDimension")]
    public partial class GDimension : ModelElement, IGDimension, IModelElement
    {
        
        /// <summary>
        /// The backing field for the Width property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private double _width = 0D;
        
        private static Lazy<ITypedElement> _widthAttribute = new Lazy<ITypedElement>(RetrieveWidthAttribute);
        
        /// <summary>
        /// The backing field for the Height property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private double _height = 0D;
        
        private static Lazy<ITypedElement> _heightAttribute = new Lazy<ITypedElement>(RetrieveHeightAttribute);
        
        private static IClass _classInstance;
        
        /// <summary>
        /// The width property
        /// </summary>
        [DefaultValueAttribute(0D)]
        [DisplayNameAttribute("width")]
        [CategoryAttribute("GDimension")]
        [XmlElementNameAttribute("width")]
        [XmlAttributeAttribute(true)]
        public double Width
        {
            get
            {
                return this._width;
            }
            set
            {
                if ((this._width != value))
                {
                    double old = this._width;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnWidthChanging(e);
                    this.OnPropertyChanging("Width", e, _widthAttribute);
                    this._width = value;
                    this.OnWidthChanged(e);
                    this.OnPropertyChanged("Width", e, _widthAttribute);
                }
            }
        }
        
        /// <summary>
        /// The height property
        /// </summary>
        [DefaultValueAttribute(0D)]
        [DisplayNameAttribute("height")]
        [CategoryAttribute("GDimension")]
        [XmlElementNameAttribute("height")]
        [XmlAttributeAttribute(true)]
        public double Height
        {
            get
            {
                return this._height;
            }
            set
            {
                if ((this._height != value))
                {
                    double old = this._height;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnHeightChanging(e);
                    this.OnPropertyChanging("Height", e, _heightAttribute);
                    this._height = value;
                    this.OnHeightChanged(e);
                    this.OnPropertyChanged("Height", e, _heightAttribute);
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
                    _classInstance = ((IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/glsp/notation#//GDimension")));
                }
                return _classInstance;
            }
        }
        
        /// <summary>
        /// Gets fired before the Width property changes its value
        /// </summary>
        public event System.EventHandler<ValueChangedEventArgs> WidthChanging;
        
        /// <summary>
        /// Gets fired when the Width property changed its value
        /// </summary>
        public event System.EventHandler<ValueChangedEventArgs> WidthChanged;
        
        /// <summary>
        /// Gets fired before the Height property changes its value
        /// </summary>
        public event System.EventHandler<ValueChangedEventArgs> HeightChanging;
        
        /// <summary>
        /// Gets fired when the Height property changed its value
        /// </summary>
        public event System.EventHandler<ValueChangedEventArgs> HeightChanged;
        
        private static ITypedElement RetrieveWidthAttribute()
        {
            return ((ITypedElement)(((ModelElement)(NMF.Glsp.Notation.GDimension.ClassInstance)).Resolve("width")));
        }
        
        /// <summary>
        /// Raises the WidthChanging event
        /// </summary>
        /// <param name="eventArgs">The event data</param>
        protected virtual void OnWidthChanging(ValueChangedEventArgs eventArgs)
        {
            System.EventHandler<ValueChangedEventArgs> handler = this.WidthChanging;
            if ((handler != null))
            {
                handler.Invoke(this, eventArgs);
            }
        }
        
        /// <summary>
        /// Raises the WidthChanged event
        /// </summary>
        /// <param name="eventArgs">The event data</param>
        protected virtual void OnWidthChanged(ValueChangedEventArgs eventArgs)
        {
            System.EventHandler<ValueChangedEventArgs> handler = this.WidthChanged;
            if ((handler != null))
            {
                handler.Invoke(this, eventArgs);
            }
        }
        
        private static ITypedElement RetrieveHeightAttribute()
        {
            return ((ITypedElement)(((ModelElement)(NMF.Glsp.Notation.GDimension.ClassInstance)).Resolve("height")));
        }
        
        /// <summary>
        /// Raises the HeightChanging event
        /// </summary>
        /// <param name="eventArgs">The event data</param>
        protected virtual void OnHeightChanging(ValueChangedEventArgs eventArgs)
        {
            System.EventHandler<ValueChangedEventArgs> handler = this.HeightChanging;
            if ((handler != null))
            {
                handler.Invoke(this, eventArgs);
            }
        }
        
        /// <summary>
        /// Raises the HeightChanged event
        /// </summary>
        /// <param name="eventArgs">The event data</param>
        protected virtual void OnHeightChanged(ValueChangedEventArgs eventArgs)
        {
            System.EventHandler<ValueChangedEventArgs> handler = this.HeightChanged;
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
            if ((attribute == "WIDTH"))
            {
                return this.Width;
            }
            if ((attribute == "HEIGHT"))
            {
                return this.Height;
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
            if ((feature == "WIDTH"))
            {
                this.Width = ((double)(value));
                return;
            }
            if ((feature == "HEIGHT"))
            {
                this.Height = ((double)(value));
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
            if ((attribute == "WIDTH"))
            {
                return Observable.Box(new WidthProxy(this));
            }
            if ((attribute == "HEIGHT"))
            {
                return Observable.Box(new HeightProxy(this));
            }
            return base.GetExpressionForAttribute(attribute);
        }
        
        /// <summary>
        /// Gets the Class for this model element
        /// </summary>
        public override IClass GetClass()
        {
            if ((_classInstance == null))
            {
                _classInstance = ((IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/glsp/notation#//GDimension")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the width property
        /// </summary>
        private sealed class WidthProxy : ModelPropertyChange<IGDimension, double>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public WidthProxy(IGDimension modelElement) : 
                    base(modelElement, "width")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override double Value
            {
                get
                {
                    return this.ModelElement.Width;
                }
                set
                {
                    this.ModelElement.Width = value;
                }
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the height property
        /// </summary>
        private sealed class HeightProxy : ModelPropertyChange<IGDimension, double>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public HeightProxy(IGDimension modelElement) : 
                    base(modelElement, "height")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override double Value
            {
                get
                {
                    return this.ModelElement.Height;
                }
                set
                {
                    this.ModelElement.Height = value;
                }
            }
        }
    }
}
