using NMF.Transformations.Core;
using NMF.Transformations.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace NMF.Transformations
{
    /// <summary>
    /// Defines a transformation rule with a single input argument that is abstract, i.e. it cannot create an output
    /// This means that there must exist transformation rules that are marked instantiating for this rule. 
    /// </summary>
    /// <typeparam name="TIn">The type of the input argument for this transformation rule</typeparam>
    /// <typeparam name="TOut">The type of output, that is generated in an instantiating rule</typeparam>
    public abstract class AbstractTransformationRule<TIn, TOut> : TransformationRule<TIn, TOut>
        where TIn : class
        where TOut : class
    {
        /// <summary>
        /// Creates the output of this transformation rule
        /// </summary>
        /// <param name="input">The input argument for this computation</param>
        /// <param name="context">The context in which the output is required</param>
        /// <returns>The output for this transformation</returns>
        /// <remarks>Creation of transformation rule outputs is not supported for abstract transformation rules and thus, calling this method will result in an InvalidOperationException</remarks>
        public override TOut CreateOutput(TIn input, ITransformationContext context)
        {
            throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.ErrAbstractRuleCreateOutput, input != null ? input.GetType().Name : "(null)", this.GetType().Name));
        }
    }

    /// <summary>
    /// Defines a transformation rule with two input arguments that is abstract, i.e. it cannot create an output
    /// This means that there must exist transformation rules that are marked instantiating for this rule. 
    /// </summary>
    /// <typeparam name="TIn1">The type of the first input argument for this transformation rule</typeparam>
    /// <typeparam name="TIn2">The type of the second input argument for this transformation rule</typeparam>
    /// <typeparam name="TOut">The type of output, that is generated in an instantiating rule</typeparam>
    public abstract class AbstractTransformationRule<TIn1, TIn2, TOut> : TransformationRule<TIn1, TIn2, TOut>
        where TIn1 : class
        where TIn2 : class
        where TOut : class
    {
        /// <summary>
        /// Creates the output of this transformation rule
        /// </summary>
        /// <param name="input1">The first input argument for this computation</param>
        /// <param name="input2">The second input argument for this computation</param>
        /// <param name="context">The context in which the output is required</param>
        /// <returns>The output for this transformation</returns>
        /// <remarks>Creation of transformation rule outputs is not supported for abstract transformation rules and thus, calling this method will result in an InvalidOperationException</remarks>
        public override TOut CreateOutput(TIn1 input1, TIn2 input2, ITransformationContext context)
        {
            throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.ErrAbstractRuleT2CreateOutput, input1 != null ? input1.GetType().Name : "(null)", input2 != null ? input2.GetType().Name : "(null)", this.GetType().Name));
        }
    }
}
