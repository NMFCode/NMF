using NMF.Transformations.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Transformations
{

    /// <summary>
    /// Represents a transformation pattern that can be tied to a transformation rule
    /// </summary>
    /// <typeparam name="TIn">The type of the first input parameter of the target transformation rule</typeparam>
    public interface ITransformationRulePattern<TIn> : ITransformationRulePattern
        where TIn : class
    {
        /// <summary>
        /// Gets or sets the transformation rule that is the target for the current transformation rule pattern
        /// </summary>
        new GeneralTransformationRule<TIn> TargetRule { get; set; }
    }

    /// <summary>
    /// Represents a transformation pattern that can be tied to a transformation rule
    /// </summary>
    /// <typeparam name="TIn1">The type of the first input parameter of the target transformation rule</typeparam>
    /// <typeparam name="TIn2">The type of the second input parameter of the target transformation rule</typeparam>
    public interface ITransformationRulePattern<TIn1, TIn2> : ITransformationRulePattern
        where TIn1 : class
        where TIn2 : class
    {
        /// <summary>
        /// Gets or sets the transformation rule that is the target for the current transformation rule pattern
        /// </summary>
        new GeneralTransformationRule<TIn1, TIn2> TargetRule { get; set; }
    }
}
