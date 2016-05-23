using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions
{
    /// <summary>
    /// Represents the extraction of a formal parameter
    /// </summary>
    internal struct ParameterExtraction : IEquatable<ParameterExtraction>
    {
        /// <summary>
        /// Creates a new parameter extraction
        /// </summary>
        /// <param name="parameter">The parameter that should be created</param>
        /// <param name="value">The value that is promoted</param>
        public ParameterExtraction(ParameterExpression parameter, Expression value) : this()
        {
            Parameter = parameter;
            Value = value;
        }

        /// <summary>
        /// The new parameter
        /// </summary>
        public ParameterExpression Parameter { get; private set; }

        /// <summary>
        /// The parameter value
        /// </summary>
        public Expression Value { get; private set; }

        public override bool Equals(object obj)
        {
            if (obj is ParameterExtraction)
            {
                return Equals((ParameterExtraction)obj);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            var hash = 0;
            if (Parameter != null) hash ^= Parameter.GetHashCode();
            if (Value != null) hash ^= Value.GetHashCode();
            return hash;
        }

        public bool Equals(ParameterExtraction other)
        {
            return Parameter == other.Parameter && Value == other.Value;
        }
    }
}
