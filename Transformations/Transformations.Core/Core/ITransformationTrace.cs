using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Transformations.Core
{
    /// <summary>
    /// Represents a trace for a transformation
    /// </summary>
    public interface ITransformationTrace
    {

        /// <summary>
        /// Revokes the given computation and deletes it from the trace
        /// </summary>
        /// <param name="traceEntry">The computation that is to be revoked</param>
        void RevokeEntry(ITraceEntry traceEntry);

        /// <summary>
        /// Publishes the given computation to the trace
        /// </summary>
        /// <param name="traceEntry">The computation that should be added to the trace</param>
        void PublishEntry(ITraceEntry traceEntry);

        /// <summary>
        /// Traces the computation based upon the specified input with the specified transformation rule
        /// </summary>
        /// <param name="rule">The transformation rule the object was transformed with</param>
        /// <returns>The computation or null, if there was none</returns>
        /// <param name="input">The input arguments</param>
        IEnumerable<ITraceEntry> TraceIn(GeneralTransformationRule rule, params object[] input);

        /// <summary>
        /// Traces the computations based upon the specified input
        /// </summary>
        /// <returns>The computations with the given inputs</returns>
        /// <param name="input">The input arguments</param>
        IEnumerable<ITraceEntry> Trace(params object[] input);

        /// <summary>
        /// Traces the computations of the specified inputs with the specified transformation rules
        /// </summary>
        /// <param name="rule">The transformation rules that transformed the specified inputs</param>
        /// <param name="inputs">A collection of input arguments</param>
        /// <returns>A collection of computations</returns>
        IEnumerable<ITraceEntry> TraceManyIn(GeneralTransformationRule rule, IEnumerable<object[]> inputs);

        /// <summary>
        /// Traces the computations of the specified inputs that match the given type signature
        /// </summary>
        /// <param name="inputs">A collection of input arguments</param>
        /// <param name="inputTypes">The input types</param>
        /// <param name="outputType">The output types</param>
        /// <returns>A collection of computations</returns>
        IEnumerable<ITraceEntry> TraceMany(Type[] inputTypes, Type outputType, IEnumerable<object[]> inputs);

        /// <summary>
        /// Traces all computations with any inputs that math the given filters with the specified transformation rule
        /// </summary>
        /// <param name="rule">The transformation rule</param>
        /// <returns>A collection with all computations made under these circumstances</returns>
        IEnumerable<ITraceEntry> TraceAllIn(GeneralTransformationRule rule);

        /// <summary>
        /// Traces all computations that match the given type signature
        /// </summary>
        /// <param name="inputTypes">The input types</param>
        /// <param name="outputType">The output types</param>
        /// <returns>A collection of computations</returns>
        IEnumerable<ITraceEntry> TraceAll(Type[] inputTypes, Type outputType);
    }
}
