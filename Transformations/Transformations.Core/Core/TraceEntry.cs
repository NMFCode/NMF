using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Transformations.Core
{
    /// <summary>
    /// Represents a trace entry
    /// </summary>
    public interface ITraceEntry
    {
        /// <summary>
        /// Gets the input for this trace entry at the ith position
        /// </summary>
        /// <param name="index">The position index</param>
        /// <returns>The input at the ith position</returns>
        object GetInput(int index);

        /// <summary>
        /// Gets the transformation output for this trace entry
        /// </summary>
        object Output { get; }

        /// <summary>
        /// Gets the transformation rule for which the trace entry was generated
        /// </summary>
        GeneralTransformationRule TransformationRule { get; }
    }

    /// <summary>
    /// Provides some helper functionality for tracing
    /// </summary>
    public static class TraceEntryExtensions
    {
        /// <summary>
        /// Creates a new array of input elements for the given trace entry
        /// </summary>
        /// <param name="traceEntry">The current trace entry</param>
        /// <returns>A new object array containing all trace entries for this trace entry</returns>
        public static object[] CreateInputArray(this ITraceEntry traceEntry) 
        {
            if (traceEntry == null) throw new ArgumentNullException("traceEntry");

            var inputTypes = traceEntry.TransformationRule.InputType;
            var array = new object[inputTypes.Length];
            for (int i = 0; i < inputTypes.Length; i++)
            {
                array[i] = traceEntry.GetInput(i);
            }
            return array;
        }
    }
}
