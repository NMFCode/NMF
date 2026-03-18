using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Synchronizations.Inconsistencies
{
    /// <summary>
    /// Denotes an interface to describe dependencies
    /// </summary>
    /// <typeparam name="TLeft">the type of the left element</typeparam>
    /// <typeparam name="TRight">the type of the right element</typeparam>
    /// <typeparam name="TDepLeft">the type of the left value</typeparam>
    /// <typeparam name="TDepRight">the type of the right value</typeparam>
    public interface IInconsistencyDescriptor<TLeft, TRight, TDepLeft, TDepRight>
    {
        /// <summary>
        /// Describe the inconsistency from the left point of view
        /// </summary>
        /// <param name="left">the left element</param>
        /// <param name="right">the right element</param>
        /// <param name="depLeft">the dependent left element</param>
        /// <param name="depRight">the dependent right element</param>
        /// <returns>A human-readable string</returns>
        string DescribeLeft(TLeft left, TRight right, TDepLeft depLeft, TDepRight depRight);

        /// <summary>
        /// Describe the inconsistency from the right point of view
        /// </summary>
        /// <param name="left">the left element</param>
        /// <param name="right">the right element</param>
        /// <param name="depLeft">the dependent left element</param>
        /// <param name="depRight">the dependent right element</param>
        /// <returns>A human-readable string</returns>
        string DescribeRight(TLeft left, TRight right, TDepLeft depLeft, TDepRight depRight);
    }
}
