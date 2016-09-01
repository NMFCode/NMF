using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMF.Models.Meta;

namespace NMF.Models
{
    public struct ScopedIdentifier : IEquatable<ScopedIdentifier>
    {
        public IAttribute Identifier { get; private set; }
        public IdentifierScope Scope { get; private set; }

        public IdentifierScope OriginalScope
        {
            get
            {
                return ((IClass)Identifier.DeclaringType).IdentifierScope.GetActual(IdentifierScope.Local);
            }
        }

        public ScopedIdentifier(IAttribute identifier, IdentifierScope scope) : this()
        {
            Identifier = identifier;
            Scope = scope;
        }

        public override bool Equals(object obj)
        {
            if (obj is ScopedIdentifier)
            {
                return Equals((ScopedIdentifier)obj);
            }
            else
            {
                return false;
            }
        }

        public static bool operator ==(ScopedIdentifier obj1, ScopedIdentifier obj2)
        {
            if (obj1 == null)
                return false;
            return obj1.Equals(obj2);
        }

        public static bool operator !=(ScopedIdentifier obj1, ScopedIdentifier obj2)
        {
            return !(obj1 == obj2);
        }

        public override int GetHashCode()
        {
            var hash = Scope.GetHashCode();
            if (Identifier != null) hash ^= Identifier.GetHashCode();
            return hash;
        }

        public bool Equals(ScopedIdentifier other)
        {
            return Identifier == other.Identifier && Scope == other.Scope;
        }
    }
}
