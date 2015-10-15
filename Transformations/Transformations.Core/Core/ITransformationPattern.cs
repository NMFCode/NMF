using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Transformations.Core
{
    /// <summary>
    /// Represents a transformation pattern
    /// </summary>
    public interface ITransformationPattern
    {
        /// <summary>
        /// Applies the current pattern to the given transformation context
        /// </summary>
        /// <param name="context">The transformation context in which the pattern should be applied</param>
        /// <returns>A transformation pattern context object that represents the pattern within the given transformation context</returns>
        ITransformationPatternContext CreatePattern(ITransformationContext context);
    }

    /// <summary>
    /// Represents the application of a transformation pattern to a transformation context
    /// </summary>
    public interface ITransformationPatternContext
    {
        /// <summary>
        /// Is called by the transformation context when the transformation pattern should start working
        /// </summary>
        void Begin();

        /// <summary>
        /// Is called by the transformation context when the transformation pass is finished for cleanup purposes
        /// </summary>
        void Finish();
    }

    /// <summary>
    /// Represents a transformation pattern that can be tied to a transformation rule
    /// </summary>
    public interface ITransformationRulePattern : ITransformationPattern
    {
        /// <summary>
        /// Gets or sets the transformation rule that is the target for the current transformation rule pattern
        /// </summary>
        GeneralTransformationRule TargetRule { get; set; }
    }
}
