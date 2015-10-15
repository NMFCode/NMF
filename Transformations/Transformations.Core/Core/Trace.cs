using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMF.Utilities;
using System.Collections;

namespace NMF.Transformations.Core
{
    /// <summary>
    /// Represents a trace class based on a collection of computations
    /// </summary>
    public class Trace : AbstractTrace, ITransformationTrace
    {
        private ICollection<ITraceEntry> computations;

        /// <summary>
        /// Creates a trace object for an empty set of computations
        /// </summary>
        public Trace() : this(new HashSet<ITraceEntry>()) { }

        /// <summary>
        /// Creates the trace for the given collection of computations
        /// </summary>
        /// <param name="computations">The collection of computations</param>
        public Trace(ICollection<ITraceEntry> computations)
        {
            this.computations = computations;
        }

        /// <summary>
        /// The computations, the trace is based upon
        /// </summary>
        public override IEnumerable<ITraceEntry> Computations
        {
            get { return computations; }
        }


        /// <summary>
        /// Revokes the given computation and deletes it from the trace
        /// </summary>
        /// <param name="traceEntry">The computation that is to be revoked</param>
        public override void RevokeEntry(ITraceEntry traceEntry)
        {
            if (traceEntry == null) throw new ArgumentNullException("traceEntry");

            computations.Remove(traceEntry);
        }

        /// <summary>
        /// Publishes the given computation to the trace
        /// </summary>
        /// <param name="traceEntry">The computation that should be added to the trace</param>
        public override void PublishEntry(ITraceEntry traceEntry)
        {
            if (traceEntry == null) throw new ArgumentNullException("traceEntry");

            if (!computations.Contains(traceEntry))
            computations.Add(traceEntry);
        }
    }

    /// <summary>
    /// Represents the base class for traces.
    /// </summary>
    /// <remarks>All trace operations are implemented and fully functional. However, they might be accelerated using index structures.</remarks>
    public abstract class AbstractTrace : ITransformationTrace
    {
        /// <summary>
        /// The computations, the trace is based upon
        /// </summary>
        public abstract IEnumerable<ITraceEntry> Computations { get; }


        /// <summary>
        /// Traces the computation based upon the specified input with the specified transformation rule
        /// </summary>
        /// <param name="rule">The transformation rule the object was transformed with</param>
        /// <returns>The computation or null, if there was none</returns>
        /// <param name="input">The input arguments</param>
        public virtual IEnumerable<ITraceEntry> TraceIn(GeneralTransformationRule rule, params object[] input)
        {
            if (rule == null) throw new ArgumentNullException("rule");

            if (input == null || input.Length != rule.InputType.Length) return Enumerable.Empty<ITraceEntry>();

            return from ITraceEntry c in Computations
                   where c.TransformationRule == rule
                   && IsInputArray(c, input)
                   select c;
        }

        private static bool IsInputArray(ITraceEntry c, object[] input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (!object.Equals(input[i], c.GetInput(i))) return false;
            }
            return true;
        }

        /// <summary>
        /// Traces all computations with any inputs that math the given filters with the specified transformation rule
        /// </summary>
        /// <param name="rule">The transformation rule</param>
        /// <returns>A collection with all computations made under these circumstances</returns>
        public virtual IEnumerable<ITraceEntry> TraceAllIn(GeneralTransformationRule rule)
        {
            if (rule == null) throw new ArgumentNullException("rule");

            return from ITraceEntry c in Computations
                   where c.TransformationRule == rule
                   select c;
        }

        /// <summary>
        /// Traces the computations based upon the specified input
        /// </summary>
        /// <returns>The computations with the given inputs</returns>
        /// <param name="input">The input arguments</param>
        public virtual IEnumerable<ITraceEntry> Trace(object[] input)
        {
            if (input == null) return Enumerable.Empty<ITraceEntry>();

            return Computations.Where(c => c.TransformationRule.InputType.Length == input.Length && IsInputArray(c, input));
        }

        /// <summary>
        /// Revokes the given computation and deletes it from the trace
        /// </summary>
        /// <param name="traceEntry">The computation that is to be revoked</param>
        public abstract void RevokeEntry(ITraceEntry traceEntry);

        /// <summary>
        /// Publishes the given computation to the trace
        /// </summary>
        /// <param name="traceEntry">The computation that should be added to the trace</param>
        public abstract void PublishEntry(ITraceEntry traceEntry);

        /// <summary>
        /// Traces the computations of the specified inputs with the specified transformation rules
        /// </summary>
        /// <param name="rule">The transformation rules that transformed the specified inputs</param>
        /// <param name="inputs">A collection of input arguments</param>
        /// <returns>A collection of computations</returns>
        public virtual IEnumerable<ITraceEntry> TraceManyIn(GeneralTransformationRule rule, IEnumerable<object[]> inputs)
        {
            if (rule == null) throw new ArgumentNullException("rule");
            if (inputs.IsNullOrEmpty()) return Enumerable.Empty<ITraceEntry>();
            return Computations.Where(c => c.TransformationRule == rule && inputs.Any(input => input != null && input.Length == rule.InputType.Length && IsInputArray(c, input)));
        }

        /// <summary>
        /// Traces the computations of the specified inputs that match the given type signature
        /// </summary>
        /// <param name="inputs">A collection of input arguments</param>
        /// <param name="inputTypes">The input types</param>
        /// <param name="outputType">The output types</param>
        /// <returns>A collection of computations</returns>
        public virtual IEnumerable<ITraceEntry> TraceMany(Type[] inputTypes, Type outputType, IEnumerable<object[]> inputs)
        {
            if (inputs.IsNullOrEmpty()) return Enumerable.Empty<ITraceEntry>();
            return Computations.Where(c => c.TransformationRule.InputType.ArrayEquals(inputTypes) && c.TransformationRule.OutputType == outputType
                                        && inputs.Any(input => input != null && input.Length == c.TransformationRule.InputType.Length && IsInputArray(c, input)));
        }

        /// <summary>
        /// Traces all computations that match the given type signature
        /// </summary>
        /// <param name="inputTypes">The input types</param>
        /// <param name="outputType">The output types</param>
        /// <returns>A collection of computations</returns>
        public virtual IEnumerable<ITraceEntry> TraceAll(Type[] inputTypes, Type outputType)
        {
            return Computations.Where(c => c.TransformationRule.InputType.ArrayEquals(inputTypes) && c.TransformationRule.OutputType == outputType);
        }
    }
}
