using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Synchronizations.Inconsistencies
{
    /// <summary>
    /// Denotes an interface for a syntax allowing to add custom descriptors
    /// </summary>
    /// <typeparam name="TLeft">the type of the left element</typeparam>
    /// <typeparam name="TRight">the type of the right element</typeparam>
    /// <typeparam name="TDepLeft">the type of the left value</typeparam>
    /// <typeparam name="TDepRight">the type of the right value</typeparam>
    public interface IInconsistencyDescriptorSyntax<TLeft, TRight, TDepLeft, TDepRight>
    {
        /// <summary>
        /// Adds a description for changes at the left side
        /// </summary>
        /// <param name="descriptor">a function returning a human-readable description of the change</param>
        /// <returns>a human-readable description of the change</returns>
        IInconsistencyDescriptorSyntax<TLeft, TRight, TDepLeft, TDepRight> DescribeLeftChange(Func<TLeft, TRight, TDepLeft, TDepRight, string> descriptor);


        /// <summary>
        /// Adds a description for changes at the right side
        /// </summary>
        /// <param name="descriptor">a function returning a human-readable description of the change</param>
        /// <returns>a human-readable description of the change</returns>
        IInconsistencyDescriptorSyntax<TLeft, TRight, TDepLeft, TDepRight> DescribeRightChange(Func<TLeft, TRight, TDepLeft, TDepRight, string> descriptor);
    }

    /// <summary>
    /// Denotes an interface for a syntax allowing to add custom descriptors
    /// </summary>
    /// <typeparam name="TLeft">the type of the left element</typeparam>
    /// <typeparam name="TRight">the type of the right element</typeparam>
    /// <typeparam name="TDepLeft">the type of the left value</typeparam>
    /// <typeparam name="TDepRight">the type of the right value</typeparam>
    public interface IInconsistencyDescriptorSyntaxLeft<TLeft, TRight, TDepLeft, TDepRight>
    {
        /// <summary>
        /// Adds a description for changes at the left side
        /// </summary>
        /// <param name="descriptor">a function returning a human-readable description of the change</param>
        /// <returns>a human-readable description of the change</returns>
        IInconsistencyDescriptorSyntaxLeft<TLeft, TRight, TDepLeft, TDepRight> DescribeLeftChange(Func<TLeft, TRight, TDepLeft, TDepRight, string> descriptor);
    }

    /// <summary>
    /// Denotes an interface for a syntax allowing to add custom descriptors
    /// </summary>
    /// <typeparam name="TLeft">the type of the left element</typeparam>
    /// <typeparam name="TRight">the type of the right element</typeparam>
    /// <typeparam name="TDepLeft">the type of the left value</typeparam>
    /// <typeparam name="TDepRight">the type of the right value</typeparam>
    public interface IInconsistencyDescriptorSyntaxRight<TLeft, TRight, TDepLeft, TDepRight>
    {
        /// <summary>
        /// Adds a description for changes at the right side
        /// </summary>
        /// <param name="descriptor">a function returning a human-readable description of the change</param>
        /// <returns>a human-readable description of the change</returns>
        IInconsistencyDescriptorSyntaxRight<TLeft, TRight, TDepLeft, TDepRight> DescribeRightChange(Func<TLeft, TRight, TDepLeft, TDepRight, string> descriptor);
    }
}
