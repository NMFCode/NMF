using NMF.Transformations.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Transformations
{
    /// <summary>
    /// Represents a computation that transforms a single input
    /// </summary>
    /// <typeparam name="TIn">The type of the input</typeparam>
    public abstract class ComputationBase<TIn> : Computation
        where TIn : class
    {
        private TIn input;

        /// <summary>
        /// Creates a new computation within the given transformation context for the given input
        /// </summary>
        /// <param name="transformationRule">The transformation rule that was used to transform the input</param>
        /// <param name="context">The context of this transformation</param>
        /// <param name="input">The input for this transformation</param>
        protected ComputationBase(GeneralTransformationRule<TIn> transformationRule, IComputationContext context, TIn input)
            : base(transformationRule, context)
        {
            this.input = input;
        }

        /// <summary>
        /// Gets the input argument for this computation with the given index
        /// </summary>
        /// <param name="index">The index of the input parameter</param>
        /// <returns>The input parameter at the ith position</returns>
        public override object GetInput(int index)
        {
            if (index == 0) return input;
            throw new ArgumentOutOfRangeException("index");
        }

        /// <summary>
        /// Gets the transformation rule, which has been used to compute this computation
        /// </summary>
        public new GeneralTransformationRule<TIn> TransformationRule
        {
            get
            {
                return base.TransformationRule as GeneralTransformationRule<TIn>;
            }
        }

        /// <summary>
        /// Gets the input argument for this computation
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
        public TIn Input
        {
            get
            {
                return input;
            }
        }
    }

    /// <summary>
    /// Represents a computation that transforms two inputs
    /// </summary>
    /// <typeparam name="TIn1">The type of the first input</typeparam>
    /// <typeparam name="TIn2">The type of the second input</typeparam>
    public abstract class ComputationBase<TIn1, TIn2> : Computation
        where TIn1 : class
        where TIn2 : class
    {
        private TIn1 input1;
        private TIn2 input2;

        /// <summary>
        /// Creates a new computation within the given transformation context for the given input
        /// </summary>
        /// <param name="transformationRule">The transformation rule that was used to transform the input</param>
        /// <param name="context">The context of this transformation</param>
        /// <param name="input1">The first input for this transformation</param>
        /// <param name="input2">The second input for this transformation</param>
        protected ComputationBase(GeneralTransformationRule<TIn1, TIn2> transformationRule, IComputationContext context, TIn1 input1, TIn2 input2)
            : base(transformationRule, context)
        {
            this.input1 = input1;
            this.input2 = input2;
        }

        /// <summary>
        /// Gets the input argument for this computation with the given index
        /// </summary>
        /// <param name="index">The index of the input parameter</param>
        /// <returns>The input parameter at the ith position</returns>
        public override object GetInput(int index)
        {
            switch (index)
            {
                case 0:
                    return input1;
                case 1:
                    return input2;
                default:
                    throw new ArgumentOutOfRangeException("index");
            }
        }

        /// <summary>
        /// Gets the transformation rule, which has been used to compute this computation
        /// </summary>
        public new GeneralTransformationRule<TIn1, TIn2> TransformationRule
        {
            get
            {
                return base.TransformationRule as GeneralTransformationRule<TIn1, TIn2>;
            }
        }

        /// <summary>
        /// Gets the first input argument for this computation
        /// </summary>
        public TIn1 Input1
        {
            get
            {
                return input1;
            }
        }

        /// <summary>
        /// Gets the second input argument for this computation
        /// </summary>
        public TIn2 Input2
        {
            get
            {
                return input2;
            }
        }
    }

    /// <summary>
    /// Represents a computation that transforms arbitrary many inputs
    /// </summary>
    public abstract class ComputationBase : Computation
    {
        private object[] inputs;

        /// <summary>
        /// Creates a new computation within the given transformation context for the given input
        /// </summary>
        /// <param name="transformationRule">The transformation rule that was used to transform the input</param>
        /// <param name="context">The context of this transformation</param>
        /// <param name="inputs">The input for this transformation</param>
        protected ComputationBase(GeneralTransformationRule transformationRule, IComputationContext context, object[] inputs)
            : base(transformationRule, context)
        {
            this.inputs = inputs;
        }

        /// <summary>
        /// Gets the input argument for this computation with the given index
        /// </summary>
        /// <param name="index">The index of the input parameter</param>
        /// <returns>The input parameter at the ith position</returns>
        public override object GetInput(int index)
        {
            return inputs[index];
        }

        /// <summary>
        /// Gets the inputs of this computation
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
        protected object[] Input { get { return inputs; } }
    }


}
