using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMF.Models.Meta;

namespace NMF.Models
{
    /// <summary>
    /// Denotes the tuple of an attribute and its scope
    /// </summary>
    public struct ScopedIdentifier : IEquatable<ScopedIdentifier>
    {
        /// <summary>
        /// Gets the identifier attribute
        /// </summary>
        public IAttribute Identifier { get; private set; }

        /// <summary>
        /// Gets the scope in which the identifier is valid
        /// </summary>
        public IdentifierScope Scope { get; private set; }

        /// <summary>
        /// Creates a new scoped identifier
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="scope"></param>
        public ScopedIdentifier(IAttribute identifier, IdentifierScope scope) : this()
        {
            Identifier = identifier;
            Scope = scope;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is ScopedIdentifier identifier)
            {
                return Equals(identifier);
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc />
        public bool Equals(ScopedIdentifier other)
        {
            return Identifier == other.Identifier && Scope == other.Scope;
        }

        /// <inheritdoc />
        public static bool operator ==(ScopedIdentifier obj1, ScopedIdentifier obj2)
        {
            return obj1.Equals(obj2);
        }

        /// <inheritdoc />
        public static bool operator !=(ScopedIdentifier obj1, ScopedIdentifier obj2)
        {
            return !(obj1 == obj2);
        }

        /// <inheritdoc />
        public override readonly int GetHashCode()
        {
            var hash = Scope.GetHashCode();
            if (Identifier != null) hash ^= Identifier.GetHashCode();
            return hash;
        }
    }
}
