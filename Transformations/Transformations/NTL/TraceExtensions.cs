using NMF.Transformations.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace NMF.Transformations
{
    /// <summary>
    /// This helper class provides additional methods for tracing purposes
    /// </summary>
    public static class TraceExtensions
    {
        /// <summary>
        /// Trace the output of the computation that transformed the given input with the given transformation type
        /// </summary>
        /// <param name="rule">The rule that transformed the argument</param>
        /// <param name="input">The input arguments</param>
        /// <param name="trace">The trace component that is used as basis</param>
        /// <returns>The output of the computation with the specified input argument or null, if there is none such</returns>
        public static object ResolveIn(this ITransformationTrace trace, GeneralTransformationRule rule, object[] input)
        {
            if (trace == null) throw new ArgumentNullException("trace");

            var comp = trace.TraceIn(rule, input).FirstOrDefault();
            return comp != null ? comp.Output : null;
        }

        /// <summary>
        /// Trace the output of the computation that transformed the given input with the given transformation type
        /// </summary>
        /// <typeparam name="TIn">The input type that is looked for</typeparam>
        /// <param name="rule">The transformation rule the object was transformed with</param>
        /// <typeparam name="TOut">The output that is returned by the transformation rule</typeparam>
        /// <param name="trace">The trace component that is used as basis</param>
        /// <param name="input">The input argument</param>
        /// <returns>The output of the computation with the specified input argument or null, if there is none such</returns>
        [DebuggerStepThrough]
        public static TOut ResolveIn<TIn, TOut>(this ITransformationTrace trace, TransformationRuleBase<TIn, TOut> rule, TIn input)
        {
            if (trace == null) throw new ArgumentNullException("trace");

            var comp = trace.TraceIn(rule, input).FirstOrDefault();
            return comp != null ? (TOut)comp.Output : default;
        }

        /// <summary>
        /// Trace the output of the computation that transformed the given input into a desired output
        /// </summary>
        /// <typeparam name="TIn">The input type that is looked for</typeparam>
        /// <typeparam name="TOut">The desired output type</typeparam>
        /// <param name="trace">The trace component that is used as basis</param>
        /// <param name="input">The input argument</param>
        /// <returns>The output of the computation with the specified input argument or null, if there is none such</returns>
        [DebuggerStepThrough]
        public static TOut Resolve<TIn, TOut>(this ITransformationTrace trace, TIn input)
        {

            if (trace == null) throw new ArgumentNullException("trace");

            var comp = trace.Trace(input).FirstOrDefault();
            return comp != null ? (TOut)comp.Output : default;
        }

        /// <summary>
        /// Trace the output of the computation that transformed the given input with the given transformation type
        /// </summary>
        /// <typeparam name="TIn1">The first input type that is looked for</typeparam>
        /// <typeparam name="TIn2">The second input type that is looked for</typeparam>
        /// <param name="rule">The transformation rule the object was transformed with</param>
        /// <typeparam name="TOut">The output that is returned by the transformation rule</typeparam>
        /// <param name="trace">The trace component that is used as basis</param>
        /// <param name="input1">The first input argument</param>
        /// <param name="input2">The second input argument</param>
        /// <returns>The output of the computation with the specified input argument or null, if there is none such</returns>
        [DebuggerStepThrough]
        public static TOut ResolveIn<TIn1, TIn2, TOut>(this ITransformationTrace trace, TransformationRuleBase<TIn1, TIn2, TOut> rule, TIn1 input1, TIn2 input2)
        {
            if (trace == null) throw new ArgumentNullException("trace");

            var comp = trace.TraceIn(rule, input1, input2).FirstOrDefault();
            return comp != null ? (TOut)comp.Output : default;
        }

        /// <summary>
        /// Trace the output of the computation that transformed the given input with the given transformation type
        /// </summary>
        /// <typeparam name="TIn1">The first input type that is looked for</typeparam>
        /// <typeparam name="TIn2">The second input type that is looked for</typeparam>
        /// <typeparam name="TOut">The output that is returned by the transformation rule</typeparam>
        /// <param name="input1">The first input argument</param>
        /// <param name="trace">The trace component that is used as basis</param>
        /// <param name="input2">The second input argument</param>
        /// <returns>The output of the computation with the specified input argument or null, if there is none such</returns>
        [DebuggerStepThrough]
        public static TOut Resolve<TIn1, TIn2, TOut>(this ITransformationTrace trace, TIn1 input1, TIn2 input2)
        {
            if (trace == null) throw new ArgumentNullException("trace");

            var outputType = typeof(TOut);
            var comp = trace.Trace(input1, input2).FirstOrDefault(c => c.TransformationRule.OutputType == outputType);
            return comp != null ? (TOut)comp.Output : default;
        }

        /// <summary>
        /// Trace the output of the computation that transformed the given input with the given transformation type
        /// </summary>
        /// <typeparam name="TOut">The output that is returned by the transformation rule</typeparam>
        /// <param name="rule">The transformation rule the object was transformed with</param>
        /// <param name="input">The input arguments</param>
        /// <param name="trace">The trace component that is used as basis</param>
        /// <returns>The output of the computation with the specified input argument or null, if there is none such</returns>
        [DebuggerStepThrough]
        public static TOut ResolveIn<TOut>(this ITransformationTrace trace, TransformationRuleBase<TOut> rule, object[] input)
        {
            if (trace == null) throw new ArgumentNullException("trace");

            var comp = trace.TraceIn(rule, input).FirstOrDefault();
            return comp != null ? (TOut)comp.Output : default;
        }

        /// <summary>
        /// Trace the output of the computation that transformed the given input with the given transformation type
        /// </summary>
        /// <typeparam name="TOut">The output that is returned by the transformation rule</typeparam>
        /// <param name="input">The input arguments</param>
        /// <param name="trace">The trace component that is used as basis</param>
        /// <returns>The output of the computation with the specified input argument or null, if there is none such</returns>
        [DebuggerStepThrough]
        public static TOut Resolve<TOut>(this ITransformationTrace trace, object[] input)
        {
            if (trace == null) throw new ArgumentNullException("trace");

            var outputType = typeof(TOut);
            var comp = trace.Trace(input).FirstOrDefault();
            return comp != null ? (TOut)comp.Output : default;
        }

        /// <summary>
        /// Trace the output of the computation that transformed any input that matches the filter with the given transformation type
        /// </summary>
        /// <typeparam name="TIn">The input type that is looked for</typeparam>
        /// <param name="rule">The transformation rule the object was transformed with</param>
        /// <typeparam name="TOut">The output that is returned by the transformation rule</typeparam>
        /// <param name="filter">The filter that should filter the inputs</param>
        /// <param name="trace">The trace component that is used as basis</param>
        /// <returns>All outputs of computations with suitable input arguments or null, if there are none</returns>
        [DebuggerStepThrough]
        public static IEnumerable<TOut> ResolveInWhere<TIn, TOut>(this ITransformationTrace trace, TransformationRuleBase<TIn, TOut> rule, Predicate<TIn> filter)
        {
            if (trace == null) throw new ArgumentNullException("trace");

            return from ITraceEntry c in trace.TraceAllIn(rule)
                   where filter == null || filter((TIn)c.GetInput(0))
                   select (TOut)c.Output;
        }

        /// <summary>
        /// Trace the output of the computation that transformed any input that matches the filter into the desired type
        /// </summary>
        /// <typeparam name="TIn">The input type that is looked for</typeparam>
        /// <typeparam name="TOut">The desired output type</typeparam>
        /// <param name="trace">The trace component that is used as basis</param>
        /// <param name="filter">The filter that should filter the inputs</param>
        /// <returns>All outputs of computations with suitable input arguments or null, if there are none</returns>
        [DebuggerStepThrough]
        public static IEnumerable<TOut> ResolveWhere<TIn, TOut>(this ITransformationTrace trace, Predicate<TIn> filter)
        {
            if (trace == null) throw new ArgumentNullException("trace");

            return from ITraceEntry c in trace.TraceAll(new Type[] { typeof(TIn) }, typeof(TOut))
                   where filter == null || filter((TIn)c.GetInput(0))
                   select (TOut)c.Output;
        }

        /// <summary>
        /// Trace the output of the computation that transformed any input that matches the filter with the given transformation type
        /// </summary>
        /// <typeparam name="TIn">The input type that is looked for</typeparam>
        /// <param name="rule">The transformation rule the object was transformed with</param>
        /// <typeparam name="TOut">The output that is returned by the transformation rule</typeparam>
        /// <param name="input">The input to resolve multiple outputs</param>
        /// <param name="trace">The trace component that is used as basis</param>
        /// <returns>All outputs of computations with suitable input arguments or null, if there are none</returns>
        [DebuggerStepThrough]
        public static IEnumerable<TOut> ResolveManyIn<TIn, TOut>(this ITransformationTrace trace, TransformationRuleBase<TIn, TOut> rule, TIn input)
        {
            if (trace == null) throw new ArgumentNullException("trace");

            return from ITraceEntry c in trace.TraceIn(rule, input)
                   select (TOut)c.Output;
        }

        /// <summary>
        /// Trace the output of the computation that transformed any input that matches the filter with the given transformation type
        /// </summary>
        /// <typeparam name="TIn">The input type that is looked for</typeparam>
        /// <param name="rule">The transformation rule the object was transformed with</param>
        /// <typeparam name="TOut">The output that is returned by the transformation rule</typeparam>
        /// <param name="list">A list of allowed input arguments</param>
        /// <param name="trace">The trace component that is used as basis</param>
        /// <returns>All outputs of computations with suitable input arguments or null, if there are none</returns>
        [DebuggerStepThrough]
        public static IEnumerable<TOut> ResolveManyIn<TIn, TOut>(this ITransformationTrace trace, TransformationRuleBase<TIn, TOut> rule, IEnumerable<TIn> list)
        {
            if (trace == null) throw new ArgumentNullException("trace");
            if (list == null) return Enumerable.Empty<TOut>();

            return from ITraceEntry c in trace.TraceManyIn(rule, list.Select(input => new object[] { input }))
                   select (TOut)c.Output;
        }

        /// <summary>
        /// Trace the output of the computation that transformed any input that matches the filter into the desired type
        /// </summary>
        /// <typeparam name="TIn">The input type that is looked for</typeparam>
        /// <typeparam name="TOut">The desired output type</typeparam>
        /// <param name="trace">The trace component that is used as basis</param>
        /// <param name="list">A list of allowed input arguments</param>
        /// <returns>All outputs of computations with suitable input arguments or null, if there are none</returns>
        [DebuggerStepThrough]
        public static IEnumerable<TOut> ResolveMany<TIn, TOut>(this ITransformationTrace trace, IEnumerable<TIn> list)
        {
            if (trace == null) throw new ArgumentNullException("trace");
            if (list == null) return Enumerable.Empty<TOut>();

            return from ITraceEntry c in trace.TraceMany(new Type[] { typeof(TIn) }, typeof(TOut), list.Select(input => new object[] { input }))
                   select (TOut)c.Output;
        }

        /// <summary>
        /// Trace the output of the computation that transformed any input that matches the filter into the desired type
        /// </summary>
        /// <typeparam name="TIn">The input type that is looked for</typeparam>
        /// <typeparam name="TOut">The desired output type</typeparam>
        /// <param name="trace">The trace component that is used as basis</param>
        /// <param name="input">An input argument to look up multiple results</param>
        /// <returns>All outputs of computations with suitable input arguments or null, if there are none</returns>
        [DebuggerStepThrough]
        public static IEnumerable<TOut> ResolveMany<TIn, TOut>(this ITransformationTrace trace, TIn input)
        {
            if (trace == null) throw new ArgumentNullException("trace");

            return from ITraceEntry c in trace.Trace(input)
                   select (TOut)c.Output;
        }

        /// <summary>
        /// Trace the output of the computation that transformed any input that matches the filter with the given transformation type
        /// </summary>
        /// <param name="rule">The transformation rule the object was transformed with</param>
        /// <typeparam name="TOut">The output that is returned by the transformation rule</typeparam>
        /// <param name="filter">The filter that should filter the inputs</param>
        /// <param name="trace">The trace component that is used as basis</param>
        /// <returns>All outputs of computations with suitable input arguments or null, if there are none</returns>
        [DebuggerStepThrough]
        public static IEnumerable<TOut> ResolveInWhere<TOut>(this ITransformationTrace trace, TransformationRuleBase<TOut> rule, Predicate<object[]> filter)
        {
            if (trace == null) throw new ArgumentNullException("trace");

            return from ITraceEntry c in trace.TraceAllIn(rule)
                   where filter == null || filter(c.CreateInputArray())
                   select (TOut)c.Output;
        }

        /// <summary>
        /// Trace the output of the computation that transformed any input that matches the filter with the given transformation type
        /// </summary>
        /// <param name="inputTypes">The types of the input arguments that are expected to be matched</param>
        /// <typeparam name="TOut">The output that is returned by the transformation rule</typeparam>
        /// <param name="filter">The filter that should filter the inputs</param>
        /// <param name="trace">The trace component that is used as basis</param>
        /// <returns>All outputs of computations with suitable input arguments or null, if there are none</returns>
        [DebuggerStepThrough]
        public static IEnumerable<TOut> ResolveWhere<TOut>(this ITransformationTrace trace, Type[] inputTypes, Predicate<object[]> filter)
        {
            if (trace == null) throw new ArgumentNullException("trace");

            return from ITraceEntry c in trace.TraceAll(inputTypes, typeof(TOut))
                   where filter == null || filter(c.CreateInputArray())
                   select (TOut)c.Output;
        }

        /// <summary>
        /// Trace the output of the computation that transformed any input that matches the filter with the given transformation type
        /// </summary>
        /// <param name="rule">The transformation rule the object was transformed with</param>
        /// <typeparam name="TIn">The input type parameter of the transformation rule</typeparam>
        /// <typeparam name="TOut">The output that is returned by the transformation rule</typeparam>
        /// <param name="filter">The filter that should filter the inputs</param>
        /// <param name="trace">The trace component that is used as basis</param>
        /// <returns>All outputs of computations with suitable input arguments or null, if there are none</returns>
        public static IEnumerable<TOut> FindInWhere<TIn, TOut>(this ITransformationTrace trace, TransformationRuleBase<TIn, TOut> rule, Predicate<TOut> filter)
        {
            if (trace == null) throw new ArgumentNullException("trace");

            return trace.TraceAllIn(rule).Select(c => (TOut)c.Output).Where(o => filter == null || filter(o));
        }

        /// <summary>
        /// Finds all outputs of computations of from the specified source typpe to the specified target type that match the given filter
        /// </summary>
        /// <param name="trace">The trace component that is used as basis</param>
        /// <typeparam name="TIn">The input type of the transformation rule</typeparam>
        /// <typeparam name="TOut">The output type of the transformation rule</typeparam>
        /// <param name="filter">The filter that should be applied to the transformation outputs</param>
        /// <returns>A collection with all suitable outputs</returns>
        public static IEnumerable<TOut> FindWhere<TIn, TOut>(this ITransformationTrace trace, Predicate<TOut> filter)
        {
            return FindWhere<TOut>(trace, new Type[] { typeof(TIn) }, filter);
        }

        /// <summary>
        /// Finds all outputs of computations of the given transformation that match the given filter
        /// </summary>
        /// <typeparam name="TIn1">The type of the first transformation argument</typeparam>
        /// <typeparam name="TIn2">The type of the second transformation argument</typeparam>
        /// <param name="rule">The transformation rule the object was transformed with</param>
        /// <typeparam name="TOut">The output type of the transformation rule</typeparam>
        /// <param name="filter">The filter that should be applied to the transformation outputs</param>
        /// <returns>A collection with all suitable outputs</returns>
        /// <param name="trace">The trace component that is used as basis</param>
        public static IEnumerable<TOut> FindInWhere<TIn1, TIn2, TOut>(this ITransformationTrace trace, TransformationRuleBase<TIn1, TIn2, TOut> rule, Predicate<TOut> filter)
        {
            if (trace == null) throw new ArgumentNullException("trace");

            return trace.TraceAllIn(rule).Select(c => (TOut)c.Output).Where(o => filter == null || filter(o));
        }

        /// <summary>
        /// Finds all outputs of computations of from the specified source type to the specified target type that match the given filter
        /// </summary>
        /// <typeparam name="TIn1">The type of the first transformation argument</typeparam>
        /// <typeparam name="TIn2">The type of the second transformation argument</typeparam>
        /// <typeparam name="TOut">The output type of the transformation rule</typeparam>
        /// <param name="filter">The filter that should be applied to the transformation outputs</param>
        /// <returns>A collection with all suitable outputs</returns>
        /// <param name="trace">The trace component that is used as basis</param>
        public static IEnumerable<TOut> FindWhere<TIn1, TIn2, TOut>(this ITransformationTrace trace, Predicate<TOut> filter)
        {
            return FindWhere<TOut>(trace, new Type[] { typeof(TIn1), typeof(TIn2) }, filter);
        }

        /// <summary>
        /// Finds all outputs of computations of from the specified source type to the specified target type that match the given filter
        /// </summary>
        /// <param name="trace">The trace component that is used as basis</param>
        /// <typeparam name="TOut">The output type of the transformation rule</typeparam>
        /// <param name="filter">The filter that should be applied to the transformation outputs</param>
        /// <param name="rule">The transformation rule the object was transformed with</param>
        /// <returns>A collection with all suitable outputs</returns>
        public static IEnumerable<TOut> FindInWhere<TOut>(this ITransformationTrace trace, TransformationRuleBase<TOut> rule, Predicate<TOut> filter)
        {
            if (trace == null) throw new ArgumentNullException("trace");

            return trace.TraceAllIn(rule).Select(c => (TOut)c.Output).Where(o => filter == null || filter(o));
        }

        /// <summary>
        /// Finds all outputs of computations of from the specified source type to the specified target type that match the given filter
        /// </summary>
        /// <param name="trace">The trace component that is used as basis</param>
        /// <typeparam name="TOut">The output type of the transformation rule</typeparam>
        /// <param name="filter">The filter that should be applied to the transformation outputs</param>
        /// <param name="inputTypes">The input types for the transformation rule</param>
        /// <returns>A collection with all suitable outputs</returns>
        public static IEnumerable<TOut> FindWhere<TOut>(this ITransformationTrace trace, Type[] inputTypes, Predicate<TOut> filter)
        {
            if (trace == null) throw new ArgumentNullException("trace");

            return trace.TraceAll(inputTypes, typeof(TOut)).Select(c => (TOut)c.Output).Where(o => filter == null || filter(o));
        }

        /// <summary>
        /// Traces all computations with any inputs that math the given filters with the specified transformation rule
        /// </summary>
        /// <param name="trace">The trace component that is used as basis</param>
        /// <typeparam name="TIn">The input argument type of the transformation</typeparam>
        /// <param name="rule">The transformation rule</param>
        /// <param name="filter">The predicate of the inputs that are looked for</param>
        /// <returns>A collection with all computations made under these circumstances</returns>
        public static IEnumerable<ITraceEntry> TraceInWhere<TIn>(this ITransformationTrace trace, GeneralTransformationRule<TIn> rule, Predicate<TIn> filter) where TIn : class
        {
            if (trace == null) throw new ArgumentNullException("trace");

            return from ITraceEntry c in trace.TraceAllIn(rule)
                   where filter == null || filter((TIn)c.GetInput(0))
                   select c;
        }

        /// <summary>
        /// Traces all computations with any inputs that math the given filters with the specified transformation rule
        /// </summary>
        /// <param name="trace">The trace component that is used as basis</param>
        /// <typeparam name="TIn1">The first input argument type of the transformation</typeparam>
        /// <typeparam name="TIn2">The first input argument type of the transformation</typeparam>
        /// <param name="filter">The predicate of the inputs that are looked for</param>
        /// <param name="rule">The transformation rule</param>
        /// <returns>A collection with all computations made under these circumstances</returns>
        public static IEnumerable<ITraceEntry> TraceInWhere<TIn1, TIn2>(this ITransformationTrace trace, GeneralTransformationRule<TIn1, TIn2> rule, Func<TIn1, TIn2, bool> filter)
        {
            if (trace == null) throw new ArgumentNullException("trace");

            return from ITraceEntry c in trace.TraceAllIn(rule)
                   where filter == null || filter((TIn1)c.GetInput(0), (TIn2)c.GetInput(1))
                   select c;
        }

        /// <summary>
        /// Traces all computations with any inputs that math the given filters with the specified transformation rule
        /// </summary>
        /// <param name="trace">The trace component that is used as basis</param>
        /// <param name="filter">The predicate of the inputs that are looked for</param>
        /// <param name="rule">The transformation rule</param>
        /// <returns>A collection with all computations made under these circumstances</returns>
        public static IEnumerable<ITraceEntry> TraceInWhere(this ITransformationTrace trace, GeneralTransformationRule rule, Predicate<object[]> filter)
        {
            if (trace == null) throw new ArgumentNullException("trace");

            return from ITraceEntry c in trace.TraceAllIn(rule)
                   where filter == null || filter(c.CreateInputArray())
                   select c;
        }

        /// <summary>
        /// Trace the output of the computation that transformed any input that matches the filter with the given transformation type
        /// </summary>
        /// <param name="trace">The trace component that is used as basis</param>
        /// <param name="rule">The transformation rule that was used to transform the outputs</param>
        /// <param name="filter">The filter that should filter the inputs</param>
        /// <returns>All outputs of computations with suitable input arguments or null, if there are none</returns>
        public static IEnumerable<object> ResolveInWhere(this ITransformationTrace trace, GeneralTransformationRule rule, Predicate<ITraceEntry> filter)
        {
            if (trace == null) throw new ArgumentNullException("trace");

            return from ITraceEntry c in trace.TraceAllIn(rule)
                   where filter == null || filter(c)
                   select c.Output;
        }


        /// <summary>
        /// Traces all computations with any inputs that math the given filters with the specified transformation rule
        /// </summary>
        /// <param name="trace">The trace component that is used as basis</param>
        /// <param name="rule">The transformation rule</param>
        /// <returns>A collection with all computations made under these circumstances</returns>
        public static IEnumerable<TOut> FindAllIn<TOut>(this ITransformationTrace trace, TransformationRuleBase<TOut> rule) where TOut : class
        {
            if (trace == null) throw new ArgumentNullException("trace");

            return trace.TraceAllIn(rule).Select(c => (TOut)c.Output);
        }


        /// <summary>
        /// Traces all computations with any inputs that math the given filters with the specified transformation rule
        /// </summary>
        /// <param name="trace">The trace component that is used as basis</param>
        /// <param name="rule">The transformation rule</param>
        /// <returns>A collection with all computations made under these circumstances</returns>
        public static IEnumerable<TOut> FindAllIn<TIn, TOut>(this ITransformationTrace trace, TransformationRuleBase<TIn, TOut> rule)
        {
            if (trace == null) throw new ArgumentNullException("trace");

            return trace.TraceAllIn(rule).Select(c => (TOut)c.Output);
        }


        /// <summary>
        /// Traces all computations with any inputs that math the given filters with the specified transformation rule
        /// </summary>
        /// <param name="trace">The trace component that is used as basis</param>
        /// <param name="rule">The transformation rule</param>
        /// <returns>A collection with all computations made under these circumstances</returns>
        public static IEnumerable<TOut> FindAllIn<TIn1, TIn2, TOut>(this ITransformationTrace trace, TransformationRuleBase<TIn1, TIn2, TOut> rule)
        {
            if (trace == null) throw new ArgumentNullException("trace");

            return trace.TraceAllIn(rule).Select(c => (TOut)c.Output);
        }

        /// <summary>
        /// Traces all computations with any inputs that math the given filters with the specified transformation rule
        /// </summary>
        /// <param name="trace">The trace component that is used as basis</param>
        /// <param name="rule">The transformation rule</param>
        /// <param name="filter">A filter that is to be applied on the inputs</param>
        /// <returns>A collection with all computations made under these circumstances</returns>
        public static IEnumerable<TOut> ResolveInWhere<TIn1, TIn2, TOut>(this ITransformationTrace trace, TransformationRuleBase<TIn1, TIn2, TOut> rule, Func<TIn1, TIn2, bool> filter)
        {
            if (trace == null) throw new ArgumentNullException("trace");

            return from ITraceEntry c in trace.TraceAllIn(rule)
                   where (filter == null || filter((TIn1)c.GetInput(0), (TIn2)c.GetInput(1)))
                   select (TOut)c.Output;
        }

        /// <summary>
        /// Trace the output of the computation that transformed any input that matches the filter with the given transformation type
        /// </summary>
        /// <param name="trace">The trace component that is used as basis</param>
        /// <param name="filter">The filter that should filter the inputs</param>
        /// <returns>All outputs of computations with suitable input arguments or null, if there are none</returns>
        public static IEnumerable<TOut> ResolveWhere<TIn1, TIn2, TOut>(this ITransformationTrace trace, Func<TIn1, TIn2, bool> filter)
        {
            if (trace == null) throw new ArgumentNullException("trace");

            Type[] types = { typeof(TIn1), typeof(TIn2) };
            return from Computation c in trace.TraceAll(types, typeof(TOut))
                   where (filter == null || filter((TIn1)c.GetInput(0), (TIn2)c.GetInput(1)))
                   select (TOut)c.Output;
        }


        /// <summary>
        /// Trace the output of the computation that transformed any input that matches the filter with the given transformation type
        /// </summary>
        /// <param name="trace">The trace component that is used as basis</param>
        /// <typeparam name="TOut">The output that is returned by the transformation rule</typeparam>
        /// <returns>All outputs of computations</returns>
        /// <param name="inputTypes">The input types of the trace request</param>
        public static IEnumerable<TOut> FindAll<TOut>(this ITransformationTrace trace, Type[] inputTypes)
        {
            if (trace == null) throw new ArgumentNullException("trace");

            return from Computation c in trace.TraceAll(inputTypes, typeof(TOut))
                   select (TOut)c.Output;
        }


        /// <summary>
        /// Trace the output of the computation that transformed any input that matches the filter with the given transformation type
        /// </summary>
        /// <param name="trace">The trace component that is used as basis</param>
        /// <typeparam name="TOut">The output that is returned by the transformation rule</typeparam>
        /// <typeparam name="TIn">The input parameter type of the transformation rule</typeparam>
        /// <returns>All outputs of computations</returns>
        public static IEnumerable<TOut> FindAll<TIn, TOut>(this ITransformationTrace trace)
        {
            if (trace == null) throw new ArgumentNullException("trace");

            return trace.TraceAll(new Type[] { typeof(TIn) }, typeof(TOut)).Select(c => (TOut)c.Output);
        }


        /// <summary>
        /// Trace the output of the computation that transformed any input that matches the filter with the given transformation type
        /// </summary>
        /// <param name="trace">The trace component that is used as basis</param>
        /// <typeparam name="TOut">The output that is returned by the transformation rule</typeparam>
        /// <typeparam name="TIn1">The first input parameter type of the transformation rule</typeparam>
        /// <typeparam name="TIn2">The second input parameter type of the transformation rule</typeparam>
        /// <returns>All outputs of computations</returns>
        public static IEnumerable<TOut> FindAll<TIn1, TIn2, TOut>(this ITransformationTrace trace)
        {
            if (trace == null) throw new ArgumentNullException("trace");

            return trace.TraceAll(new Type[] { typeof(TIn1), typeof(TIn2) }, typeof(TOut)).Select(c => (TOut)c.Output);
        }


        /// <summary>
        /// Trace the output of the computation that transformed any input that matches the filter with the given transformation type
        /// </summary>
        /// <param name="trace">The trace component that is used as basis</param>
        /// <typeparam name="TIn">The input type that is looked for</typeparam>
        /// <param name="rule">The transformation rule the object was transformed with</param>
        /// <typeparam name="TOut">The output that is returned by the transformation rule</typeparam>
        /// <param name="list">A list of allowed input arguments</param>
        /// <returns>All outputs of computations with suitable input arguments or null, if there are none</returns>
        public static IEnumerable<TOut> ResolveManyIn<TIn, TOut>(this ITransformationTrace trace, TransformationRuleBase<TIn, TOut> rule, params TIn[] list)
        {
            return ResolveManyIn<TIn, TOut>(trace, rule, list as IEnumerable<TIn>);
        }

        /// <summary>
        /// Trace the output of the computation that transformed any input that matches the filter into the desired type
        /// </summary>
        /// <param name="trace">The trace component that is used as basis</param>
        /// <typeparam name="TIn">The input type that is looked for</typeparam>
        /// <typeparam name="TOut">The desired output type</typeparam>
        /// <param name="list">A list of allowed input arguments</param>
        /// <returns>All outputs of computations with suitable input arguments or null, if there are none</returns>
        public static IEnumerable<TOut> ResolveMany<TIn, TOut>(this ITransformationTrace trace, params TIn[] list)
        {
            return ResolveMany<TIn, TOut>(trace, list as IEnumerable<TIn>);
        }



        /// <summary>
        /// Gets or creates the user item with the specified key
        /// </summary>
        /// <typeparam name="TValue">The type of the user item</typeparam>
        /// <param name="context">The transformation context</param>
        /// <param name="key">The key for the user item</param>
        /// <param name="valueCreator">A method that creates the default value if the user item does not yet exist or null, if no user item should be created</param>
        /// <returns>The user item with the specified key</returns>
        public static TValue GetOrCreateUserItem<TValue>(this ITransformationContext context, object key, Func<TValue> valueCreator = null)
        {
            if (context == null) throw new ArgumentNullException("item");

            if (context.Data.ContainsKey(key))
            {
                return (TValue)context.Data[key];
            }
            else
            {
                if (valueCreator == null)
                {
                    return default(TValue);
                }
                else
                {
                    var val = valueCreator();
                    context.Data.Add(key, val);
                    return val;
                }
            }
        }
    }
}
