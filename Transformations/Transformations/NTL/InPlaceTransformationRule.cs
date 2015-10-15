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
    /// Defines a simple transformation rule of a transformation that has one input argument and no output
    /// </summary>
    /// <typeparam name="TIn">The type of the input argument</typeparam>
    /// <remarks>Simple means that the transformation rule does not require a custom computation class</remarks>
    public abstract class InPlaceTransformationRule<TIn> : InPlaceTransformationRuleBase<TIn>
        where TIn : class
    {
        private bool needDependencies;

        /// <summary>
        /// Creates a new transformation rule
        /// </summary>
        public InPlaceTransformationRule()
        {
            var createOutput = this.GetType().GetMethod("Init");
            needDependencies = createOutput.ReflectedType != typeof(InPlaceTransformationRule<TIn>);
        }

        /// <summary>
        /// Gets a value indicating whether the output for all dependencies must have been created before this rule creates the output
        /// </summary>
        public override bool NeedDependenciesForOutputCreation
        {
            get { return needDependencies; }
        }

        private class SimpleComputation : InPlaceComputation<TIn>
        {
            public SimpleComputation(InPlaceTransformationRule<TIn> transformationRule, TIn input, IComputationContext context)
                : base(transformationRule, context, input) { }

            public override void Transform()
            {
                if (Input != null)
                {
                    (TransformationRule as InPlaceTransformationRule<TIn>).Transform(Input, TransformationContext);
                    OnComputed(EventArgs.Empty);
                }
            }

            public override object CreateOutput(IEnumerable context)
            {
                if (Input != null)
                {
                    (TransformationRule as InPlaceTransformationRule<TIn>).Init(Input, TransformationContext);
                }
                return null;
            }
        }


        /// <summary>
        /// Creates a new Computation instance for this transformation rule or the given input 
        /// </summary>
        /// <param name="input">The input arguments for this computation</param>
        /// <param name="context">The context for this computation</param>
        /// <returns>A computation object</returns>
        public override Computation CreateComputation(object[] input, IComputationContext context)
        {
            if (input == null) return null;
            if (input.Length != 1) throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.ErrTransformationRuleWrongNumberOfArguments, this.GetType().Name));
            return new SimpleComputation(this, input[0] as TIn, context);
        }


        /// <summary>
        /// Initializes the transformation output
        /// </summary>
        /// <param name="input">The input of the transformation rule</param>
        /// <param name="context">The context (and trace!) object</param>
        /// <remarks>At this point, all the transformation outputs are created (also the delayed ones), thus, the trace is fully reliable</remarks>
        public virtual void Transform(TIn input, ITransformationContext context) { }

        /// <summary>
        /// Initializes the transformation. This is done before any other transformation rule hits Transform
        /// </summary>
        /// <param name="input">The input for this transformation rule</param>
        /// <param name="context">The current transformation context</param>
        public virtual void Init(TIn input, ITransformationContext context) { }
    }
}
