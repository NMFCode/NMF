using System;

namespace NMF.Expressions
{
    /// <summary>
    /// Declares that the method will access a given property of the given named parameter
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ParameterDependencyAttribute : Attribute
    {
        /// <summary>
        /// The name of the parameter with the dependency
        /// </summary>
        public string Parameter { get; private set; }

        /// <summary>
        /// The member that is accessed
        /// </summary>
        public string Member { get; set; }

        /// <summary>
        /// True, if all nested properties should also be watched for, otherwise False
        /// </summary>
        public bool IsNestedMember { get; private set; }

        /// <summary>
        /// Declares that the method will access a given property of the given named parameter
        /// </summary>
        /// <param name="parameter">The name of the parameter with the dependency</param>
        /// <param name="member">The member that is accessed</param>
        public ParameterDependencyAttribute(string parameter, string member) : this(parameter, member, false) { }

        /// <summary>
        /// Declares that the method will access a given property of the given named parameter
        /// </summary>
        /// <param name="parameter">The name of the parameter with the dependency</param>
        /// <param name="member">The member that is accessed</param>
        /// <param name="isNested">True, if all nested properties should also be watched for, otherwise False</param>
        public ParameterDependencyAttribute(string parameter, string member, bool isNested)
        {
            Parameter = parameter;
            Member = member;
            IsNestedMember = isNested;
        }
    }
}
