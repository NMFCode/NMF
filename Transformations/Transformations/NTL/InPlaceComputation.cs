using NMF.Transformations.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NMF.Transformations
{
    /// <summary>
    /// Represents an in-place computation 
    /// </summary>
    /// <typeparam name="TIn">The type of the input argument</typeparam>
    public abstract class InPlaceComputation<TIn> : ComputationBase<TIn>
        where TIn : class
    {
        /// <summary>
        /// Creates a new in-place computation for the given transformation rule and the given transformation context
        /// </summary>
        /// <param name="transformationRule">The transformation rule for this computation</param>
        /// <param name="context">The transformation context in which this computations resides</param>
        /// <param name="input">The input parameter for this computation</param>
        protected InPlaceComputation(InPlaceTransformationRuleBase<TIn> transformationRule, IComputationContext context, TIn input)
            : base(transformationRule, context, input) { }

        /// <summary>
        /// Gets a null instance as the output of the in-place computation
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override object OutputCore
        {
            get
            {
                return null;
            }
            set
            {
            }
        }
    }

    /// <summary>
    /// Represents an in-place computation 
    /// </summary>
    /// <typeparam name="TIn1">The type of the first input parameter</typeparam>
    /// <typeparam name="TIn2">The type of the second input parameter</typeparam>
    public abstract class InPlaceComputation<TIn1, TIn2> : ComputationBase<TIn1, TIn2>
        where TIn1 : class
        where TIn2 : class
    {

        /// <summary>
        /// Creates a new in-place computation for the given transformation rule and the given transformation context
        /// </summary>
        /// <param name="transformationRule">The transformation rule for this computation</param>
        /// <param name="context">The transformation context in which this computations resides</param>
        /// <param name="input1">The first input parameter for this computation</param>
        /// <param name="input2">The second input parameter for this computation</param>
        protected InPlaceComputation(InPlaceTransformationRuleBase<TIn1, TIn2> transformationRule, IComputationContext context, TIn1 input1, TIn2 input2)
            : base(transformationRule, context, input1, input2) { }

        /// <summary>
        /// Gets a null instance as the output of the in-place computation
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override object OutputCore
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

    }

    /// <summary>
    /// Represents an in-place computation 
    /// </summary>
    public abstract class InPlaceComputation : ComputationBase
    {

        /// <summary>
        /// Creates a new in-place computation for the given transformation rule and the given transformation context
        /// </summary>
        /// <param name="transformationRule">The transformation rule for this computation</param>
        /// <param name="context">The transformation context in which this computations resides</param>
        /// <param name="inputs">The input parameters for this computation</param>
        protected InPlaceComputation(InPlaceTransformationRuleBase transformationRule, IComputationContext context, object[] inputs)
            : base(transformationRule, context, inputs) { }

        /// <summary>
        /// Gets a null instance as the output of the in-place computation
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override object OutputCore
        {
            get
            {
                return null;
            }
            set
            {
            }
        }
    }
}
