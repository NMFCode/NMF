using NMF.Transformations.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Transformations.Linq
{
    public class StaticPattern<T> : ITransformationRulePattern<T> where T : class
    {
        public GeneralTransformationRule<T> TargetRule { get; set; }
        public Func<ITransformationContext, IEnumerable<T>> Selector { get; set; }

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

        public ITransformationPatternContext CreatePattern(ITransformationContext context)
        {
            return new StaticPatternContext<T>(TargetRule, Selector(context), context);
        }
    }

    public class StaticPattern<T1, T2> : ITransformationRulePattern<T1, T2> where T1 : class where T2: class
    {
        public GeneralTransformationRule<T1, T2> TargetRule { get; set; }
        public Func<ITransformationContext, IEnumerable<Tuple<T1, T2>>> Selector { get; set; }

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

        public ITransformationPatternContext CreatePattern(ITransformationContext context)
        {
            return new StaticPatternContext<T1, T2>(TargetRule, Selector(context), context);
        }
    }
}
