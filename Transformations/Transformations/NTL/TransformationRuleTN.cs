using NMF.Transformations.Core;
using NMF.Transformations.Properties;
using NMF.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace NMF.Transformations
{
    /// <summary>
    /// Defines a simple transformation rule of a transformation that has one input argument and an output
    /// </summary>
    /// <typeparam name="T">The type of the output</typeparam>
    /// <remarks>Simple means that the transformation rule does not require a custom computation class</remarks>
    public abstract class TransformationRule<T> : TransformationRuleBase<T>
        where T : class
    {
        private bool needDependencies;

        private static Type implementationType = typeof(T).GetImplementationType();

        /// <summary>
        /// Creates a new transformation rule
        /// </summary>
        public TransformationRule()
        {
            var createOutput = this.GetType().GetMethod("CreateOutput");
            needDependencies = createOutput.ReflectedType != typeof(TransformationRule<T>);
        }

        /// <summary>
        /// Gets a value indicating whether the output for all dependencies must have been created before this rule creates the output
        /// </summary>
        public override bool NeedDependenciesForOutputCreation
        {
            get { return needDependencies; }
        }

        /// <summary>
        /// Represents a aimple computation
        /// </summary>
        private sealed class SimpleComputation : TransformationComputation<T>
        {
            public SimpleComputation(TransformationRule<T> transformationRule, object[] input, IComputationContext context)
                : base(transformationRule, context, input) { }

            public override void Transform()
            {
                if (Input != null)
                {
                    (TransformationRule as TransformationRule<T>).Transform(Input, Output as T, TransformationContext);
                    OnComputed(EventArgs.Empty);
                }
            }

            public override object CreateOutput(IEnumerable context)
            {
                if (Input != null)
                {
                    return (TransformationRule as TransformationRule<T>).CreateOutput(Input, TransformationContext);
                }
                else
                {
                    return null;
                }
            }
        }


        /// <summary>
        /// Creates a new Computation instance for this transformation rule or the given input 
        /// </summary>
        /// <param name="input">The input arguments for this computation</param>
        /// <param name="context">The context for this computation</param>
        /// <returns>A computation object</returns>
        public sealed override Computation CreateComputation(object[] input, IComputationContext context)
        {
            if (input == null) return null;
            if (input.Length != InputType.Length) throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.ErrTransformationRuleWrongNumberOfArguments, this.GetType().Name));
            return new SimpleComputation(this, input, context);
        }

        /// <summary>
        /// Creates the output of this transformation rule
        /// </summary>
        /// <param name="input">The input of the transformation rule</param>
        /// <param name="context">The context (and trace!) object</param>
        /// <param name="output">The output of the transformation created previously</param>
        /// <returns>The output for this transformation under this input</returns>
        /// <remarks>At this point, not all of the computations have created their output and thus, the trace is not complete. Use the OutputDelayLevel-feature to have the trace contain all elements created in earlier levels</remarks>
        public virtual void Transform(object[] input, T output, ITransformationContext context) { }


        /// <summary>
        /// Initializes the transformation output
        /// </summary>
        /// <param name="input">The input of the transformation rule</param>
        /// <param name="context">The context (and trace!) object</param>
        /// <remarks>At this point, all the transformation outputs are created (also the delayed ones), thus, the trace is fully reliable</remarks>
        public virtual T CreateOutput(object[] input, ITransformationContext context)
        {
            if (implementationType != null)
            {
                return Activator.CreateInstance(implementationType) as T;
            }
            else
            {
                throw new ApplicationException(string.Format("The transformation rule {0} cannot directly create an output as the target type cannot be instantiated.", this.GetType().Name));
            }
        }
    }
}
