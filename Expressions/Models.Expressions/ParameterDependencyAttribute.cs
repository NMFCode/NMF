using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Expressions
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ParameterDependencyAttribute : Attribute
    {
        public string Parameter { get; private set; }

        public string Member { get; set; }
        public bool IsNestedMember { get; private set; }

        public ParameterDependencyAttribute(string parameter, string member) : this(parameter, member, false) { }

        public ParameterDependencyAttribute(string parameter, string member, bool isNested)
        {
            Parameter = parameter;
            Member = member;
            IsNestedMember = isNested;
        }
    }
}
