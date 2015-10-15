using NMF.Transformations.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Transformations.Tests
{
    /// <summary>
    /// Mocks a computation
    /// </summary>
    public class MockComputation : Computation
    {
        private object[] inputs;

        /// <summary>
        /// Gets or sets an action that should be called when the computation is asked to perform its Transform task
        /// </summary>
        public Action OnTransform { get; set; }

        /// <summary>
        /// Gets or sets a function that should be called when the computation is asked to create its output
        /// </summary>
        public Func<object> OnCreateOutput { get; set; }

        /// <summary>
        /// Creates a new mocked computation
        /// </summary>
        /// <param name="input">The input for the mocked computation</param>
        /// <param name="rule">The transformation rule for the mocked computation</param>
        /// <param name="context">The transformation context</param>
        /// <param name="output">The output for the transformation</param>
        public MockComputation(object[] input, GeneralTransformationRule rule, ITransformationContext context, object output)
            : this(input, rule, new ComputationContext(context), output) { }

        /// <summary>
        /// Creates a new mocked computation
        /// </summary>
        /// <param name="input">The input for the mocked computation</param>
        /// <param name="rule">The transformation rule for the mocked computation</param>
        /// <param name="context">The transformation context</param>
        /// <param name="output">The output for the transformation</param>
        public MockComputation(object[] input, GeneralTransformationRule rule, IComputationContext context, object output)
            : base(rule, context)
        {
            inputs = input;
            InitializeOutput(output);
        }

        /// <summary>
        /// Creates a new mocked computation
        /// </summary>
        /// <param name="input">The input for the mocked computation</param>
        /// <param name="rule">The transformation rule for the mocked computation</param>
        /// <param name="context">The transformation context</param>
        public MockComputation(object[] input, GeneralTransformationRule rule, ITransformationContext context) : this(input, rule, new ComputationContext(context)) { }

        /// <summary>
        /// Creates a new mocked computation
        /// </summary>
        /// <param name="input">The input for the mocked computation</param>
        /// <param name="rule">The transformation rule for the mocked computation</param>
        /// <param name="context">The transformation context</param>
        public MockComputation(object[] input, GeneralTransformationRule rule, IComputationContext context) : base(rule, context) { inputs = input; }

        /// <summary>
        /// Performs the Transform operation
        /// </summary>
        public override void Transform()
        {
            if (OnTransform != null) OnTransform();
        }

        /// <summary>
        /// Creates the output for the given transformation
        /// </summary>
        /// <returns>The output</returns>
        public override object CreateOutput(IEnumerable context)
        {
            if (OnCreateOutput == null)
            {
                return OutputCore;
            }
            else
            {
                return OnCreateOutput();
            }
        }

        /// <summary>
        /// Gets or sets the output of the MockComputation
        /// </summary>
        /// <remarks>Unlike the version of the Computation base class, this version ignores the output delay and thus does not throw an exception when accessed when the computation is delayed</remarks>
        public new object Output
        {
            get
            {
                return OutputCore;
            }
            set
            {
                OutputCore = value;
            }
        }

        /// <summary>
        /// Gets the input at the i-th position
        /// </summary>
        /// <param name="index">The index of the desired input parameter</param>
        /// <returns>The input parameter with the i-th position</returns>
        public override object GetInput(int index)
        {
            return inputs[index];
        }

        /// <summary>
        /// Gets or sets the output internal
        /// </summary>
        protected override object OutputCore
        {
            get;
            set;
        }
    }
}
