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
        object GetInput(int index);
        object Output { get; }
        GeneralTransformationRule TransformationRule { get; }
    }

    public static class TraceEntryExtensions
    {
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
