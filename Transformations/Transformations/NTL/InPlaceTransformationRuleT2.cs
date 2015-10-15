using NMF.Transformations.Core;
using NMF.Transformations.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace NMF.Transformations
{
    /// <summary>
    /// Defines a simple transformation rule of a transformation that has two inputs argument and no output
    /// </summary>
    /// <typeparam name="TIn1">The type of the first input argument</typeparam>
    /// <typeparam name="TIn2">The type of the second input argument</typeparam>
    /// <remarks>Simple means that the transformation rule does not require a custom computation class</remarks>
    public abstract class InPlaceTransformationRule<TIn1, TIn2> : InPlaceTransformationRuleBase<TIn1, TIn2>
        where TIn1 : class
        where TIn2 : class
    {
        private bool needDependencies;

        /// <summary>
        /// Creates a new transformation rule
        /// </summary>
        public InPlaceTransformationRule()
        {
            var createOutput = this.GetType().GetMethod("Init");
            needDependencies = createOutput.ReflectedType != typeof(InPlaceTransformationRule<TIn1, TIn2>);
        }

        /// <summary>
        /// Gets a value indicating whether the output for all dependencies must have been created before this rule creates the output
        /// </summary>
        public override bool NeedDependenciesForOutputCreation
        {
            get { return needDependencies; }
        }

        private class SimpleComputation : InPlaceComputation<TIn1, TIn2>
        {
            public SimpleComputation(InPlaceTransformationRule<TIn1, TIn2> transformationRule, TIn1 input1, TIn2 input2, IComputationContext context)
                : base(transformationRule, context, input1, input2) { }

            public override void Transform()
            {
                (TransformationRule as InPlaceTransformationRule<TIn1, TIn2>).Transform(Input1, Input2, TransformationContext);
                OnComputed(EventArgs.Empty);
            }

            public override object CreateOutput(IEnumerable context)
            {
                (TransformationRule as InPlaceTransformationRule<TIn1, TIn2>).Init(Input1, Input2, TransformationContext);
                return null;
            }
        }

        /// <summary>
        /// Initializes the transformation output
        /// </summary>
        /// <param name="input1">The first input of the transformation rule</param>
        /// <param name="input2">The second input of the transformation rule</param>
        /// <param name="context">The context (and trace!) object</param>
        /// <remarks>At this point, all the transformation outputs are created (also the delayed ones), thus, the trace is fully reliable</remarks>
        public virtual void Transform(TIn1 input1, TIn2 input2, ITransformationContext context) { }

        /// <summary>
        /// Initializes the transformation. This is done before any other transformation rule hits Transform
        /// </summary>
        /// <param name="input1">The first input for this transformation rule</param>
        /// <param name="input2">The second input for this transformation rule</param>
        /// <param name="context">The current transformation context</param>
        public virtual void Init(TIn1 input1, TIn2 input2, ITransformationContext context) { }

        /// <summary>
        /// Creates a new Computation instance for this transformation rule or the given input 
        /// </summary>
        /// <param name="input">The input arguments for this computation</param>
        /// <param name="context">The context for this computation</param>
        /// <returns>A computation object</returns>
        public override Computation CreateComputation(object[] input, IComputationContext context)
        {
            if (input == null) return null;
            if (input.Length != 2) throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.ErrTransformationRuleWrongNumberOfArguments, this.GetType().Name));
            return new SimpleComputation(this, input[0] as TIn1, input[1] as TIn2, context);
        }
    }
}
