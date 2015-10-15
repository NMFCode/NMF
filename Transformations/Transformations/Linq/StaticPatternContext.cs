using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NMF.Transformations.Core;
using NMF.Transformations;

namespace NMF.Transformations.Linq
{
    internal class StaticPatternContext<T> : ITransformationPatternContext where T : class
    {
        public GeneralTransformationRule<T> TargetRule { get; private set; }
        public IEnumerable<T> Values { get; private set; }
        public ITransformationContext Context { get; set; }

        public StaticPatternContext(GeneralTransformationRule<T> targetRule, IEnumerable<T> values, ITransformationContext context)
        {
            TargetRule = targetRule;
            Values = values;
            Context = context;
        }

        public void Begin()
        {
            if (Values != null)
            {
                foreach (var val in Values)
                {
                    Context.CallTransformation(TargetRule, val);
                }
            }
        }

        public void Finish() { }
    }

    internal class StaticPatternContext<T1, T2> : ITransformationPatternContext where T1 : class where T2 : class
    {
        public GeneralTransformationRule<T1, T2> TargetRule { get; private set; }
        public IEnumerable<Tuple<T1, T2>> Values { get; private set; }
        public ITransformationContext Context { get; set; }

        public StaticPatternContext(GeneralTransformationRule<T1, T2> targetRule, IEnumerable<Tuple<T1, T2>> values, ITransformationContext context)
        {
            TargetRule = targetRule;
            Values = values;
            Context = context;
        }

        public void Begin()
        {
            if (Values != null)
            {
                foreach (var val in Values)
                {
                    if (val != null)
                    {
                        Context.CallTransformation(TargetRule, val.Item1, val.Item2);
                    }
                }
            }
        }

        public void Finish() { }
    }
}
