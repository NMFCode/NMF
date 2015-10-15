using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Transformations
{
    /// <summary>
    /// Represents an in-place transformation rule that operates on two input arguments
    /// </summary>
    /// <typeparam name="TIn1">The type of the first input argument</typeparam>
    /// <typeparam name="TIn2">The type of the second input argument</typeparam>
    public abstract class InPlaceTransformationRuleBase<TIn1, TIn2> : GeneralTransformationRule<TIn1, TIn2>
        where TIn1 : class
        where TIn2 : class
    {
        /// <summary>
        /// Gets the output type of the transformation rule
        /// </summary>
        public sealed override Type OutputType
        {
            get { return typeof(void); }
        }
    }
}
