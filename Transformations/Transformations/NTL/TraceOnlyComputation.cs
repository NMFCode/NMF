using NMF.Transformations.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Transformations
{
    /// <summary>
    /// Represents a computation that is only used for tracing purposes
    /// </summary>
    /// <typeparam name="TInput">The type of the trace key</typeparam>
    /// <typeparam name="TOut">The output type of the trace entry</typeparam>
    public sealed class TraceEntry<TInput, TOut> : ITraceEntry
        where TInput : class
        where TOut : class
    {
        private TInput input;
        private TOut output;
        private TransformationRuleBase<TInput, TOut> transformationRule;

        /// <summary>
        /// Creates a new trace-only computation
        /// </summary>
        /// <param name="rule">The transformation rule used as trace group</param>
        /// <param name="input">The trace key for this transformation rule</param>
        /// <param name="output">The output for this trace entry</param>
        public TraceEntry(TransformationRuleBase<TInput, TOut> rule, TInput input, TOut output)
        {
            this.input = input;
            this.output = output;
            this.transformationRule = rule;
        }

        object ITraceEntry.GetInput(int index)
        {
            if (index != 0) throw new ArgumentOutOfRangeException("index");

            return input;
        }

        /// <summary>
        /// Gets the output of the trace entry
        /// </summary>
        public TOut Output
        {
            get { return output; }
        }

        object ITraceEntry.Output { get { return output; } }

        /// <summary>
        /// Gets the transformation rule used as trace key for this trace entry
        /// </summary>
        public TransformationRuleBase<TInput, TOut> TransformationRule
        {
            get { return transformationRule; }
        }

        GeneralTransformationRule ITraceEntry.TransformationRule
        {
            get { return transformationRule; }
        }
    }
}
