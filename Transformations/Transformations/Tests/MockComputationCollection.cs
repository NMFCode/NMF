using NMF.Transformations.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace NMF.Transformations.Tests
{
    /// <summary>
    /// Represents a collection of computations used by the mocked context
    /// </summary>
    public class MockComputationCollection : ObservableCollection<ITraceEntry>
    {

        private MockContext context;

        /// <summary>
        /// Creates a new mock computation collection for the given mocked context
        /// </summary>
        /// <param name="context">The mock context in which to create the computation collection</param>
        public MockComputationCollection(MockContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Creates a computation mock for the given transformation rule with the given input and the given context and adds the computation to the context
        /// </summary>
        /// <param name="transformationRule">The transformation rule for this computation mock</param>
        /// <param name="input">The input for this computation</param>
        /// <returns>The computation mock</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public MockComputation Add<TIn>(GeneralTransformationRule<TIn> transformationRule, TIn input)
            where TIn : class
        {
            var c = new MockComputation(new object[] { input }, transformationRule, CreateComputationContext());
            Add(c);
            return c;
        }

        private IComputationContext CreateComputationContext()
        {
            return new ComputationContext(context);
        }

        /// <summary>
        /// Creates a computation mock for the given transformation rule with the given input and the given context and adds the computation to the context
        /// </summary>
        /// <param name="transformationRule">The transformation rule for this computation mock</param>
        /// <param name="input">The input for this computation</param>
        /// <param name="output">The output for the mock computation</param>
        /// <returns>The computation mock</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public MockComputation Add<TIn, TOut>(TransformationRuleBase<TIn, TOut> transformationRule, TIn input, TOut output)
            where TIn : class
            where TOut : class
        {
            var c = new MockComputation(new object[] { input }, transformationRule, CreateComputationContext(), output);
            Add(c);
            return c;
        }

        /// <summary>
        /// Creates a computation mock for the given transformation rule with the given input and the given context and adds the computation to the context
        /// </summary>
        /// <param name="transformationRule">The transformation rule for this computation mock</param>
        /// <param name="input1">The first input for this computation</param>
        /// <param name="input2">The second input for this computation</param>
        /// <returns>The computation mock</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public MockComputation Add<TIn1, TIn2>(GeneralTransformationRule<TIn1, TIn2> transformationRule, TIn1 input1, TIn2 input2)
            where TIn1 : class
            where TIn2 : class
        {
            var c = new MockComputation(new object[] { input1, input2 }, transformationRule, CreateComputationContext());
            Add(c);
            return c;
        }

        /// <summary>
        /// Creates a computation mock for the given transformation rule with the given input and the given context and adds the computation to the context
        /// </summary>
        /// <param name="transformationRule">The transformation rule for this computation mock</param>
        /// <param name="input1">The first input for this computation</param>
        /// <param name="input2">The second input for this computation</param>
        /// <param name="output">The output of the mock computation</param>
        /// <returns>The computation mock</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public MockComputation Add<TIn1, TIn2, TOut>(TransformationRuleBase<TIn1, TIn2, TOut> transformationRule, TIn1 input1, TIn2 input2, TOut output)
            where TIn1 : class
            where TIn2 : class
            where TOut : class
        {
            var c = new MockComputation(new object[] { input1, input2 }, transformationRule, CreateComputationContext(), output);
            Add(c);
            return c;
        }

        /// <summary>
        /// Creates a computation mock for the given transformation rule with the given input and the given context and adds the computation to the context
        /// </summary>
        /// <param name="transformationRule">The transformation rule for this computation mock</param>
        /// <param name="input">The input for this computation</param>
        /// <returns>The computation mock</returns>
        public MockComputation Add(GeneralTransformationRule transformationRule, object[] input)
        {
            var c = new MockComputation(input, transformationRule, CreateComputationContext());
            Add(c);
            return c;
        }

        /// <summary>
        /// Creates a computation mock for the given transformation rule with the given input and the given context and adds the computation to the context
        /// </summary>
        /// <param name="transformationRule">The transformation rule for this computation mock</param>
        /// <param name="input">The input for this computation</param>
        /// <param name="output">The output of the mock computation</param>
        /// <returns>The computation mock</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public MockComputation Add<TOut>(TransformationRuleBase<TOut> transformationRule, object[] input, TOut output)
            where TOut : class
        {
            var c = new MockComputation(input, transformationRule, CreateComputationContext(), output);
            Add(c);
            return c;
        }
    }
}
