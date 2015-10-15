using NMF.Transformations.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Transformations
{
    /// <summary>
    /// Represents a computation that represents that a set of input arguments are transformed into an output of type TOut
    /// </summary>
    /// <typeparam name="TOut">The type of the output</typeparam>
    /// <typeparam name="TIn">The type of the input parameter</typeparam>
    public abstract class TransformationComputation<TIn, TOut> : ComputationBase<TIn>
        where TOut : class
        where TIn : class
    {
        private TOut output;

        /// <summary>
        /// Creates a new transformation computation for the given input arguments
        /// </summary>
        /// <param name="transformationRule">The transformation rule for this computation</param>
        /// <param name="context">The context of this computation</param>
        /// <param name="input">The input for this computation</param>
        protected TransformationComputation(TransformationRuleBase<TIn, TOut> transformationRule, IComputationContext context, TIn input)
            : base(transformationRule, context, input) { }

        /// <summary>
        /// Gets or sets the output in a derived class
        /// </summary>
        protected override object OutputCore
        {
            get
            {
                return output;
            }
            set
            {
                var o = value as TOut;
                output = o;
                if (o == null && value != null)
                {
                    throw new InvalidOperationException("The output for the computation has been set to an invalid instance!");
                }
            }
        }

        /// <summary>
        /// Gets the output of this computation
        /// </summary>
        /// <exception cref="InvalidOperationException">This property may throw a DelayedOutputCreationException in case that the output has been tried to access, although the output creation was delayed</exception>
        public new TOut Output
        {
            get
            {
                if (IsDelayed) ThrowDelayedException();
                return output;
            }
        }
    }

    /// <summary>
    /// Represents a computation that represents that a set of input arguments are transformed into an output of type TOut
    /// </summary>
    /// <typeparam name="TOut">The type of the output</typeparam>
    /// <typeparam name="TIn1">The type of the first input parameter</typeparam>
    /// <typeparam name="TIn2">The type of the second input parameter</typeparam>
    public abstract class TransformationComputation<TIn1, TIn2, TOut> : ComputationBase<TIn1, TIn2>
        where TIn1 : class
        where TIn2 : class
        where TOut : class
    {
        private TOut output;

        /// <summary>
        /// Creates a new transformation computation for the given input arguments
        /// </summary>
        /// <param name="transformationRule">The transformation rule for this computation</param>
        /// <param name="context">The context of this computation</param>
        /// <param name="input1">The first input for this computation</param>
        /// <param name="input2">The second input for this computation</param>
        protected TransformationComputation(TransformationRuleBase<TIn1, TIn2, TOut> transformationRule, IComputationContext context, TIn1 input1, TIn2 input2)
            : base(transformationRule, context, input1, input2) { }

        /// <summary>
        /// Gets or sets the output in a derived class
        /// </summary>
        protected override object OutputCore
        {
            get
            {
                return output;
            }
            set
            {
                var o = value as TOut;
                output = o;
                if (o == null && value != null)
                {
                    throw new InvalidOperationException("The output for the computation has been set to an invalid instance!");
                }
            }
        }

        /// <summary>
        /// Gets the output of this computation
        /// </summary>
        /// <exception cref="InvalidOperationException">This property may throw a DelayedOutputCreationException in case that the output has been tried to access, although the output creation was delayed</exception>
        public new TOut Output
        {
            get
            {
                if (IsDelayed) ThrowDelayedException();
                return output;
            }
        }

    }

    /// <summary>
    /// Represents a computation that represents that a set of input arguments are transformed into an output of type TOut
    /// </summary>
    /// <typeparam name="TOut">The type of the output</typeparam>
    public abstract class TransformationComputation<TOut> : ComputationBase
        where TOut : class
    {
        private TOut output;

        /// <summary>
        /// Creates a new transformation computation for the given input arguments
        /// </summary>
        /// <param name="transformationRule">The transformation rule for this computation</param>
        /// <param name="context">The context of this computation</param>
        /// <param name="inputs">The input parameters for this computation</param>
        protected TransformationComputation(TransformationRuleBase<TOut> transformationRule, IComputationContext context, object[] inputs)
            : base(transformationRule, context, inputs) { }

        /// <summary>
        /// Gets or sets the output in a derived class
        /// </summary>
        protected override object OutputCore
        {
            get
            {
                return output;
            }
            set
            {
                var o = value as TOut;
                output = o;
                if (o == null && value != null)
                {
                    throw new InvalidOperationException("The output for the computation has been set to an invalid instance!");
                }
            }
        }

        /// <summary>
        /// Gets the output of this computation
        /// </summary>
        /// <exception cref="InvalidOperationException">This property may throw a DelayedOutputCreationException in case that the output has been tried to access, although the output creation was delayed</exception>
        public new TOut Output
        {
            get
            {
                if (IsDelayed) ThrowDelayedException();
                return output;
            }
        }
    }
}
