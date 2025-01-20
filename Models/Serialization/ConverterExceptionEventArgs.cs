using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Serialization
{
    /// <summary>
    /// Denotes event data if a converter caused an exception
    /// </summary>
    public class ConverterExceptionEventArgs
    {
        /// <summary>
        /// Gets or sets the text value
        /// </summary>
        public string TextValue { get; set; }

        /// <summary>
        /// Gets or sets the object value
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Gets the exception
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Gets the element for which the exception occured
        /// </summary>
        public XmlSerializationContext Context { get; }

        /// <summary>
        /// Gets the type that was attempted to (de-)serialize
        /// </summary>
        public ITypeSerializationInfo Type { get; }

        /// <summary>
        /// Gets or sets a flag indicating that the exception has been handled
        /// </summary>
        public bool Handled { get; set; }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="textValue">the text value</param>
        /// <param name="value">the object value</param>
        /// <param name="exception">the exception</param>
        /// <param name="context">the element for which the exception occured</param>
        /// <param name="type">the type that was attempted to (de-)serialize</param>
        public ConverterExceptionEventArgs(string textValue, object value, Exception exception, XmlSerializationContext context, ITypeSerializationInfo type)
        {
            TextValue = textValue;
            Value = value;
            Exception = exception;
            Context = context;
            Type = type;
        }
    }
}
