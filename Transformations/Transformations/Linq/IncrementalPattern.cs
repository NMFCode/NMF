using NMF.Expressions;
using NMF.Transformations.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Transformations.Linq
{
    /// <summary>
    /// Represents a relational pattern for a transformation rule with one input argument
    /// </summary>
    /// <typeparam name="TIn">The input type of the targeted transformation rule</typeparam>
    public class IncrementalPattern<TIn> : ITransformationRulePattern<TIn>
        where TIn : class
    {
        /// <summary>
        /// Creates a new relational pattern with the given pattern constructor
        /// </summary>
        /// <param name="sourceCreator">A method that creates a relational source for a given transformation context</param>
        public IncrementalPattern(Func<ITransformationContext, INotifyEnumerable<TIn>> sourceCreator)
        {
            if (sourceCreator == null) throw new ArgumentNullException("sourceCreator");

            SourceCreator = sourceCreator;
        }

        /// <summary>
        /// Gets the pattern constructor function that is used to build up relational patterns
        /// </summary>
        public Func<ITransformationContext, INotifyEnumerable<TIn>> SourceCreator { get; private set; }

        /// <summary>
        /// Creates a pattern context for the given transformation context
        /// </summary>
        /// <param name="context">The transformation context the pattern should be created for</param>
        /// <returns></returns>
        public ITransformationPatternContext CreatePattern(ITransformationContext context)
        {
            return new RelationalPatternContext<TIn>(SourceCreator(context), TargetRule, context);
        }

        /// <summary>
        /// The transformation rule, the pattern should be applied to
        /// </summary>
        public GeneralTransformationRule<TIn> TargetRule
        {
            get;
            set;
        }

        GeneralTransformationRule ITransformationRulePattern.TargetRule
        {
            get
            {
                return TargetRule;
            }
            set
            {
                TargetRule = value as GeneralTransformationRule<TIn>;
            }
        }
    }

    /// <summary>
    /// Represents a relational pattern for a transformation rule with one input argument
    /// </summary>
    /// <typeparam name="TIn1">The first input type of the targeted transformation rule</typeparam>
    /// <typeparam name="TIn2">The second input type of the targeted transformation rule</typeparam>
    public class IncrementalPattern<TIn1, TIn2> : ITransformationRulePattern<TIn1, TIn2>
        where TIn1 : class
        where TIn2 : class
    {

        /// <summary>
        /// Creates a new relational pattern with the given pattern constructor
        /// </summary>
        /// <param name="sourceCreator">A method that creates a relational source for a given transformation context</param>
        public IncrementalPattern(Func<ITransformationContext, INotifyEnumerable<Tuple<TIn1, TIn2>>> sourceCreator)
        {
            if (sourceCreator == null) throw new ArgumentNullException("sourceCreator");

            SourceCreator = sourceCreator;
        }

        /// <summary>
        /// The transformation rule, the pattern should be applied to
        /// </summary>
        public GeneralTransformationRule<TIn1, TIn2> TargetRule
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the pattern constructor function that is used to build up relational patterns
        /// </summary>
        public Func<ITransformationContext, INotifyEnumerable<Tuple<TIn1, TIn2>>> SourceCreator { get; private set; }

        /// <summary>
        /// Creates a pattern context for the given transformation context
        /// </summary>
        /// <param name="context">The transformation context the pattern should be created for</param>
        /// <returns></returns>
        public ITransformationPatternContext CreatePattern(ITransformationContext context)
        {
            return new RelationalPatternContext<TIn1, TIn2>(SourceCreator(context), TargetRule, context);
        }

        GeneralTransformationRule ITransformationRulePattern.TargetRule
        {
            get
            {
                return TargetRule;
            }
            set
            {
                TargetRule = value as GeneralTransformationRule<TIn1, TIn2>;
            }
        }
    }

}
