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
    /// <typeparam name="TIn1">The type of the first input argument</typeparam>
    /// <typeparam name="TIn2">The type of the second input argument</typeparam>
    /// <typeparam name="TOut">The type of the output</typeparam>
    /// <remarks>Simple means that the transformation rule does not require a custom computation class</remarks>
    
    public class TransformationRule<TIn1, TIn2, TOut> : TransformationRuleBase<TIn1, TIn2, TOut>
        where TIn1 : class
        where TIn2 : class
        where TOut : class
    {
        private bool needDependencies;

        private static Type implementationType = typeof(TOut).GetImplementationType();

        /// <summary>
        /// Creates a new transformation rule
        /// </summary>
        public TransformationRule()
        {
            var createOutput = this.GetType().GetMethod("CreateOutput");
            needDependencies = createOutput.ReflectedType != typeof(TransformationRule<TIn1, TIn2, TOut>);
        }

        /// <summary>
        /// Gets a value indicating whether the output for all dependencies must have been created before this rule creates the output
        /// </summary>
        public override bool NeedDependenciesForOutputCreation
        {
            get { return needDependencies; }
        }

        private sealed class SimpleComputation : TransformationComputation<TIn1, TIn2, TOut>
        {
            public SimpleComputation(TransformationRule<TIn1, TIn2, TOut> transformationRule, TIn1 input1, TIn2 input2, IComputationContext context)
                : base(transformationRule, context, input1, input2) { }

            public override void Transform()
            {
                (TransformationRule as TransformationRule<TIn1, TIn2, TOut>).Transform(Input1, Input2, Output, TransformationContext);
                OnComputed(EventArgs.Empty);
            }

            public override object CreateOutput(IEnumerable context)
            {
                return (TransformationRule as TransformationRule<TIn1, TIn2, TOut>).CreateOutput(Input1, Input2, TransformationContext);
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
            if (input.Length != 2) throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.ErrTransformationRuleWrongNumberOfArguments, this.GetType().Name));
            return new SimpleComputation(this, input[0] as TIn1, input[1] as TIn2, context);
        }



        /// <summary>
        /// Creates the output of this transformation rule
        /// </summary>
        /// <param name="input1">The first input of the transformation rule</param>
        /// <param name="input2">The second input of the transformation rule</param>
        /// <param name="context">The context (and trace!) object</param>
        /// <returns>The output for this transformation under this input</returns>
        /// <remarks>At this point, not all of the computations have created their output and thus, the trace is not complete. Use the OutputDelayLevel-feature to have the trace contain all elements created in earlier levels</remarks>
        public virtual TOut CreateOutput(TIn1 input1, TIn2 input2, ITransformationContext context)
        {
            if (implementationType != null)
            {
                return Activator.CreateInstance(implementationType) as TOut;
            }
            else
            {
                throw new ApplicationException(string.Format("The transformation rule {0} cannot directly create an output as the target type cannot be instantiated.", this.GetType().Name));
            }
        }

        /// <summary>
        /// Initializes the transformation output
        /// </summary>
        /// <param name="input1">The first input of the transformation rule</param>
        /// <param name="input2">The second input of the transformation rule</param>
        /// <param name="output">The output of the transformation rule generated earlier</param>
        /// <param name="context">The context (and trace!) object</param>
        /// <remarks>At this point, all the transformation outputs are created (also the delayed ones), thus, the trace is fully reliable</remarks>
        public virtual void Transform(TIn1 input1, TIn2 input2, TOut output, ITransformationContext context) { }
        
    }
}
