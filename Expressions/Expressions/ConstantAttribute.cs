using System;

namespace NMF.Expressions
{
    /// <summary>
    /// Defines that a method result is constant for a given parameter
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
    public sealed class ConstantAttribute : Attribute
    {
    }
}
