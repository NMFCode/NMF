using System;

namespace NMF.Expressions
{
    /// <summary>
    /// Marks a method as immutable
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class ImmutableMethodAttribute : Attribute
    {
    }
}
