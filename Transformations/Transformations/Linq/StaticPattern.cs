using NMF.Transformations.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Transformations.Linq
{
    /// <summary>
    /// Denotes a static pattern used as input for a transformation rule
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StaticPattern<T> : ITransformationRulePattern<T> where T : class
    {
        /// <inheritdoc />
        public GeneralTransformationRule<T> TargetRule { get; set; }

        /// <summary>
        /// Gets the function to select inputs for the target rule
        /// </summary>
        public Func<ITransformationContext, IEnumerable<T>> Selector { get; set; }

        /// <summary>
        /// Creates a new static pattern to fire transformation rule executions
        /// </summary>
        /// <param name="selector">A function that selects the inputs for the transformation rule</param>
        public StaticPattern(Func<ITransformationContext, IEnumerable<T>> selector)
        {
            Selector = selector;
        }

        GeneralTransformationRule Core.ITransformationRulePattern.TargetRule
        {
            get
            {
                return TargetRule;
            }
            set
            {
                TargetRule = (GeneralTransformationRule<T>)value;
            }
        }

        /// <inheritdoc />
        public ITransformationPatternContext CreatePattern(ITransformationContext context)
        {
            return new StaticPatternContext<T>(TargetRule, Selector(context), context);
        }
    }

    /// <summary>
    /// Denotes a static pattern used to generate inputs for transformation rules
    /// </summary>
    /// <typeparam name="T1">The first argument type of the transformation rule</typeparam>
    /// <typeparam name="T2">The second argument tyoe of the transformation rule</typeparam>
    public class StaticPattern<T1, T2> : ITransformationRulePattern<T1, T2> where T1 : class where T2: class
    {
        /// <inheritdoc />
        public GeneralTransformationRule<T1, T2> TargetRule { get; set; }

        /// <summary>
        /// The function to select the input elements of the target transformation rule
        /// </summary>
        public Func<ITransformationContext, IEnumerable<Tuple<T1, T2>>> Selector { get; set; }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="selector">The function to select the input elements of the target transformation rule</param>
        public StaticPattern(Func<ITransformationContext, IEnumerable<Tuple<T1, T2>>> selector)
        {
            Selector = selector;
        }

        GeneralTransformationRule Core.ITransformationRulePattern.TargetRule
        {
            get
            {
                return TargetRule;
            }
            set
            {
                TargetRule = (GeneralTransformationRule<T1, T2>)value;
            }
        }

        /// <inheritdoc />
        public ITransformationPatternContext CreatePattern(ITransformationContext context)
        {
            return new StaticPatternContext<T1, T2>(TargetRule, Selector(context), context);
        }
    }
}
