using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Transformations
{
    /// <summary>
    /// Defines a transformation rule that takes an input argument, but does not produce a result
    /// </summary>
    /// <typeparam name="T">The type of the input argument</typeparam>
    public abstract class InPlaceTransformationRuleBase<T> : GeneralTransformationRule<T>
        where T : class
    {

        /// <summary>
        /// Gets the output type of this transformation rule
        /// </summary>
        public sealed override Type OutputType
        {
            get { return typeof(void); }
        }
    }
}
