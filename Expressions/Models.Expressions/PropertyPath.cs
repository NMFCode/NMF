using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace NMF.Expressions
{
    internal abstract class PropertyPath
    {
        public abstract PropertyPath Parent
        {
            get;
        }

        public abstract Type Type
        {
            get;
        }

        public abstract MemberInfo Member
        {
            get;
        }

        public abstract ParameterExpression RootParameter
        {
            get;
        }
    }

    internal class PropertyAccess : PropertyPath
    {
        private PropertyPath parent;
        private MemberInfo member;

        public override PropertyPath Parent
        {
            get
            {
                return parent;
            }
        }

        public override Type Type
        {
            get
            {
                if (member.MemberType == MemberTypes.Property)
                {
                    return ((PropertyInfo)member).PropertyType;
                }
                else
                {
                    return ((FieldInfo)member).FieldType;
                }
            }
        }

        public override MemberInfo Member
        {
            get
            {
                return member;
            }
        }

        public override ParameterExpression RootParameter
        {
            get
            {
                return Parent.RootParameter;
            }
        }

        public PropertyAccess(PropertyPath parent, MemberInfo member)
        {
            if (parent == null) throw new ArgumentNullException("parent");
            if (member == null) throw new ArgumentNullException("member");
            if (member.MemberType != MemberTypes.Field && member.MemberType != MemberTypes.Property) throw new ArgumentOutOfRangeException("member", "member must be a property or a field.");

            this.parent = parent;
            this.member = member;
        }
    }

    internal class PathRoot : PropertyPath
    {
        private ParameterExpression parameter;

        public PathRoot(ParameterExpression parameter)
        {
            if (parameter == null) throw new ArgumentNullException("parameter");

            this.parameter = parameter;
        }

        public override MemberInfo Member
        {
            get
            {
                return null;
            }
        }

        public override PropertyPath Parent
        {
            get
            {
                return null;
            }
        }

        public override ParameterExpression RootParameter
        {
            get
            {
                return parameter;
            }
        }

        public override Type Type
        {
            get
            {
                return parameter.Type;
            }
        }
    }
}
