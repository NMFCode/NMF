﻿using NMF.Transformations.Core;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace NMF.Transformations
{
    /// <summary>
    /// Represents a transformation rule that is only used for tracing purposes and must not be used to create computations
    /// </summary>
    /// <typeparam name="TKey">The type for the keys of the trace entries</typeparam>
    /// <typeparam name="TOut">The type of the values of the trace entries</typeparam>
    public class TraceEntryGroup<TKey, TOut> : TransformationRuleBase<TKey, TOut>
    {
        /// <summary>
        /// Overridden to disallow creating computations
        /// </summary>
        /// <param name="input">Disallowed</param>
        /// <param name="context">Disallowed</param>
        /// <returns>Disallowed</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override Computation CreateComputation(object[] input, IComputationContext context)
        {
            throw new InvalidOperationException("This rule is only intended for tracing purposes and must not be called using regular dependencies.");
        }

        /// <inheritdoc />
        public override bool NeedDependenciesForOutputCreation
        {
            get { return false; }
        }
    }

    [DebuggerDisplay("{Representation}")]
    internal class InputTraceDependency<TIn, TKey> : ITransformationRuleDependency
    {
        public Func<TIn, TKey> KeySelector { get; set; }
        public TransformationRuleBase<TKey, TIn> TraceRule { get; set; }

        public void HandleDependency(Computation computation)
        {
            if (computation == null) return;
            var input = (TIn)computation.GetInput(0);
            var key = KeySelector(input);
            computation.TransformationContext.Trace.PublishEntry(new TraceEntry<TKey, TIn>(TraceRule, key, input));
        }

        public string Representation
        {
            get { return "Trace input as " + TraceRule.ToString(); }
        }

        public bool ExecuteBefore
        {
            get { return true; }
        }
    }

    [DebuggerDisplay("{Representation}")]
    internal class InputTraceDependency<TIn1, TIn2, TKey> : ITransformationRuleDependency
    {
        public Func<TIn1, TIn2, TKey> KeySelector { get; set; }
        public TransformationRuleBase<TKey, Tuple<TIn1, TIn2>> TraceRule { get; set; }

        public void HandleDependency(Computation computation)
        {
            if (computation == null) return;
            var input1 = (TIn1)computation.GetInput(0);
            var input2 = (TIn2)computation.GetInput(1);
            var key = KeySelector(input1, input2);
            computation.TransformationContext.Trace.PublishEntry(new TraceEntry<TKey, Tuple<TIn1, TIn2>>(TraceRule, key, Tuple.Create(input1, input2)));
        }

        public string Representation
        {
            get { return "Trace input as " + TraceRule.ToString(); }
        }

        public bool ExecuteBefore
        {
            get { return true; }
        }
    }

    [DebuggerDisplay("{Representation}")]
    internal class TraceDependency<TIn, TOut, TTraceIn, TTraceOut> : OutputDependency
    {
        public Func<TIn, TOut, TTraceIn> InputSelector { get; set; }
        public Func<TIn, TOut, TTraceOut> OutputSelector { get; set; }
        public TraceEntryGroup<TTraceIn, TTraceOut> TraceKey { get; set; }

        protected override void HandleReadyComputation(Computation computation)
        {
            if (computation == null) return;
            var input = (TIn)computation.GetInput(0);
            var output = (TOut)computation.Output;
            computation.TransformationContext.Trace.PublishEntry(new TraceEntry<TTraceIn, TTraceOut>(TraceKey, InputSelector(input, output), OutputSelector(input, output)));
        }

        public string Representation
        {
            get { return "Trace as " + TraceKey.ToString(); }
        }
    }

    [DebuggerDisplay("{Representation}")]
    internal class TraceDependency<TIn1, TIn2, TOut, TTraceIn, TTraceOut> : OutputDependency
    {
        public Func<TIn1, TIn2, TOut, TTraceIn> InputSelector { get; set; }
        public Func<TIn1, TIn2, TOut, TTraceOut> OutputSelector { get; set; }
        public TraceEntryGroup<TTraceIn, TTraceOut> TraceKey { get; set; }

        protected override void HandleReadyComputation(Computation computation)
        {
            if (computation == null) return;
            var input1 = (TIn1)computation.GetInput(0);
            var input2 = (TIn2)computation.GetInput(1);
            var output = (TOut)computation.Output;
            computation.TransformationContext.Trace.PublishEntry(new TraceEntry<TTraceIn, TTraceOut>(TraceKey, InputSelector(input1, input2, output), OutputSelector(input1, input2, output)));
        }

        public string Representation
        {
            get { return "Trace as " + TraceKey.ToString(); }
        }
    }

}
