using NMF.Expressions;
using NMF.Transformations.Core;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Transformations.Linq
{
    internal abstract class IncrementalPatternContext
    {
        public abstract bool PushComputation();
    }

    /// <summary>
    /// This class represents the pattern context for a relational pattern applied to a transformation rule with one input argument
    /// </summary>
    /// <typeparam name="TIn">The type of the input argument of the parented transformation rule</typeparam>
    internal class RelationalPatternContext<TIn> : IncrementalPatternContext, ITransformationPatternContext
        where TIn : class
    {
        public INotifyEnumerable<TIn> Source { get; set; }
        public ITransformationContext Context { get; set; }
        public GeneralTransformationRule<TIn> TargetRule { get; set; }

        public RelationalPatternContext(INotifyEnumerable<TIn> relationalSource, GeneralTransformationRule<TIn> targetRule, ITransformationContext context)
        {
            this.Source = relationalSource;
            this.Context = context;
            this.TargetRule = targetRule;
        }

        public void Finish()
        {
            IncrementalPatternEngine.GetForContext(Context).Patterns.Remove(this);
            Source.Detach();
        }

        public override bool PushComputation()
        {
            foreach (var item in Source)
            {
                Context.CallTransformation(TargetRule, item);
                return true;
            }
            return false;
        }

        public void Begin()
        {
            if (this.Source != null)
            {
                var engine = IncrementalPatternEngine.GetForContext(Context);
                engine.Patterns.Add(this);
                engine.Run();
            }
        }
    }

    /// <summary>
    /// This class represents the pattern context for a relational pattern applied to a transformation rule with two input arguments
    /// </summary>
    /// <typeparam name="TIn1">The type of the first input argument of the parented transformation rule</typeparam>
    /// <typeparam name="TIn2">The type of the second input argument of the parented transformation rule</typeparam>
    internal class RelationalPatternContext<TIn1, TIn2> : IncrementalPatternContext, ITransformationPatternContext
        where TIn1 : class
        where TIn2 : class
    {
        public INotifyEnumerable<Tuple<TIn1, TIn2>> Source { get; set; }
        public ITransformationContext Context { get; set; }
        public GeneralTransformationRule<TIn1, TIn2> TargetRule { get; set; }

        public RelationalPatternContext(INotifyEnumerable<Tuple<TIn1, TIn2>> relationalSource, GeneralTransformationRule<TIn1, TIn2> targetRule, ITransformationContext context)
        {
            this.Source = relationalSource;
            this.Context = context;
            this.TargetRule = targetRule;
        }

        public void Finish()
        {
            IncrementalPatternEngine.GetForContext(Context).Patterns.Remove(this);
            Source.Detach();
        }

        public override bool PushComputation()
        {
            foreach (var item in Source)
            {
                if (item != null)
                {
                    Context.CallTransformation(TargetRule, item.Item1, item.Item2);
                    return true;
                }
            }
            return false;
        }

        public void Begin()
        {
            if (this.Source != null)
            {
                var engine = IncrementalPatternEngine.GetForContext(Context);
                if (engine.Patterns.Count == 0) PushComputation();
                engine.Patterns.Add(this);
            }
        }
    }
}
