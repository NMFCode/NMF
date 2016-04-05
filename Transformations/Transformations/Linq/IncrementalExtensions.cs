using NMF.Expressions;
using NMF.Expressions.Linq;
using NMF.Transformations.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Transformations.Linq
{
    /// <summary>
    /// This class contains the extension methods necessary for the NMF Transformations relational extensions
    /// </summary>
    public static class IncrementalExtensions
    {
        /// <summary>
        /// Creates a new computation source from a transformation rule
        /// </summary>
        /// <typeparam name="TInput">The type of the input arguments for the transformation rule</typeparam>
        /// <typeparam name="TOutput">The output type of the transformation rule</typeparam>
        /// <param name="rule">The rule that should be taken as a source of computation objects</param>
        /// <param name="context">The context in which the rule is used as source of computations</param>
        /// <returns>A source of computations that can further be dealt with</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static INotifyEnumerable<TransformationComputationWrapper<TInput, TOutput>> ToComputationSource<TInput, TOutput>(this TransformationRuleBase<TInput, TOutput> rule, ITransformationContext context)
            where TInput : class
            where TOutput : class
        {
            if (rule == null) throw new ArgumentNullException("rule");

            return new TransformationRuleSource<TInput, TOutput>(rule, context);
        }

        /// <summary>
        /// Creates a new computation source from a transformation rule
        /// </summary>
        /// <typeparam name="TInput">The type of the input arguments for the transformation rule</typeparam>
        /// <typeparam name="TOutput">The output type of the transformation rule</typeparam>
        /// <param name="rule">The rule that should be taken as a source of computation objects</param>
        /// <param name="allowNull">A boolean value indicating whether null values should be allowed</param>
        /// <param name="context">The context in which the rule is used as source of computations</param>
        /// <returns>A source of computations that can further be dealt with</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static INotifyEnumerable<TransformationComputationWrapper<TInput, TOutput>> ToComputationSource<TInput, TOutput>(this TransformationRuleBase<TInput, TOutput> rule, ITransformationContext context, bool allowNull)
            where TInput : class
            where TOutput : class
        {
            if (rule == null) throw new ArgumentNullException("rule");

            var trs = new TransformationRuleSource<TInput, TOutput>(rule, context);
            if (allowNull) trs.AddNullItem();
            return trs;
        }

        /// <summary>
        /// Creates a new computation source from a transformation rule
        /// </summary>
        /// <typeparam name="TInput">The type of the input arguments for the transformation rule</typeparam>
        /// <typeparam name="TOutput">The output type of the transformation rule</typeparam>
        /// <param name="rule">The rule that should be taken as a source of computation objects</param>
        /// <param name="filter">A method or lambda expression to filter the computation objects</param>
        /// <param name="context">The context in which the rule is used as source of computations</param>
        /// <returns>A source of computations that can further be dealt with</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static INotifyEnumerable<TransformationComputationWrapper<TInput, TOutput>> ToComputationSource<TInput, TOutput>(this TransformationRuleBase<TInput, TOutput> rule, ITransformationContext context, Func<TransformationComputationWrapper<TInput, TOutput>, bool> filter)
            where TInput : class
            where TOutput : class
        {
            if (rule == null) throw new ArgumentNullException("rule");

            return new TransformationRuleSource<TInput, TOutput>(rule, context) { Filter = filter };
        }

        /// <summary>
        /// Creates a new computation source from a transformation rule
        /// </summary>
        /// <typeparam name="TInput">The type of the input arguments for the transformation rule</typeparam>
        /// <typeparam name="TOutput">The output type of the transformation rule</typeparam>
        /// <param name="rule">The rule that should be taken as a source of computation objects</param>
        /// <param name="allowNull">A boolean value indicating whether null values should be allowed</param>
        /// <param name="context">The context in which the rule is used as source of computations</param>
        /// <param name="filter">A method or lambda expression to filter the computation objects</param>
        /// <returns>A source of computations that can further be dealt with</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static INotifyEnumerable<TransformationComputationWrapper<TInput, TOutput>> ToComputationSource<TInput, TOutput>(this TransformationRuleBase<TInput, TOutput> rule, ITransformationContext context, bool allowNull, Func<TransformationComputationWrapper<TInput, TOutput>, bool> filter)
            where TInput : class
            where TOutput : class
        {
            if (rule == null) throw new ArgumentNullException("rule");

            var trs = new TransformationRuleSource<TInput, TOutput>(rule, context) { Filter = filter };
            if (allowNull) trs.AddNullItem();
            return trs;
        }

        /// <summary>
        /// Creates a new computation source from a transformation rule
        /// </summary>
        /// <typeparam name="TInput1">The type of the first input arguments for the transformation rule</typeparam>
        /// <typeparam name="TInput2">The type of the second input arguments for the transformation rule</typeparam>
        /// <typeparam name="TOutput">The output type of the transformation rule</typeparam>
        /// <param name="rule">The rule that should be taken as a source of computation objects</param>
        /// <param name="context">The context in which the rule is used as source of computations</param>
        /// <returns>A source of computations that can further be dealt with</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static INotifyEnumerable<TransformationComputationWrapper<TInput1, TInput2, TOutput>> ToComputationSource<TInput1, TInput2, TOutput>(this TransformationRuleBase<TInput1, TInput2, TOutput> rule, ITransformationContext context)
            where TInput1 : class
            where TInput2 : class
            where TOutput : class
        {
            if (rule == null) throw new ArgumentNullException("rule");

            return new TransformationRuleSource<TInput1, TInput2, TOutput>(rule, context);
        }

        /// <summary>
        /// Creates a new computation source from a transformation rule
        /// </summary>
        /// <typeparam name="TInput1">The type of the first input arguments for the transformation rule</typeparam>
        /// <typeparam name="TInput2">The type of the second input arguments for the transformation rule</typeparam>
        /// <typeparam name="TOutput">The output type of the transformation rule</typeparam>
        /// <param name="rule">The rule that should be taken as a source of computation objects</param>
        /// <param name="allowNull">A boolean value indicating whether null values should be allowed</param>
        /// <param name="context">The context in which the rule is used as source of computations</param>
        /// <returns>A source of computations that can further be dealt with</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static INotifyEnumerable<TransformationComputationWrapper<TInput1, TInput2, TOutput>> ToComputationSource<TInput1, TInput2, TOutput>(this TransformationRuleBase<TInput1, TInput2, TOutput> rule, ITransformationContext context, bool allowNull)
            where TInput1 : class
            where TInput2 : class
            where TOutput : class
        {
            if (rule == null) throw new ArgumentNullException("rule");

            var trs = new TransformationRuleSource<TInput1, TInput2, TOutput>(rule, context);
            if (allowNull) trs.AddNullItem();
            return trs;
        }

        /// <summary>
        /// Creates a new computation source from a transformation rule
        /// </summary>
        /// <typeparam name="TInput1">The type of the first input arguments for the transformation rule</typeparam>
        /// <typeparam name="TInput2">The type of the second input arguments for the transformation rule</typeparam>
        /// <typeparam name="TOutput">The output type of the transformation rule</typeparam>
        /// <param name="rule">The rule that should be taken as a source of computation objects</param>
        /// <param name="filter">A method or lambda expression to filter the computation objects</param>
        /// <param name="context">The context in which the rule is used as source of computations</param>
        /// <returns>A source of computations that can further be dealt with</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static INotifyEnumerable<TransformationComputationWrapper<TInput1, TInput2, TOutput>> ToComputationSource<TInput1, TInput2, TOutput>(this TransformationRuleBase<TInput1, TInput2, TOutput> rule, ITransformationContext context, Func<TransformationComputationWrapper<TInput1, TInput2, TOutput>, bool> filter)
            where TInput1 : class
            where TInput2 : class
            where TOutput : class
        {
            if (rule == null) throw new ArgumentNullException("rule");

            return new TransformationRuleSource<TInput1, TInput2, TOutput>(rule, context) { Filter = filter };
        }

        /// <summary>
        /// Creates a new computation source from a transformation rule
        /// </summary>
        /// <typeparam name="TInput1">The type of the first input arguments for the transformation rule</typeparam>
        /// <typeparam name="TInput2">The type of the second input arguments for the transformation rule</typeparam>
        /// <typeparam name="TOutput">The output type of the transformation rule</typeparam>
        /// <param name="rule">The rule that should be taken as a source of computation objects</param>
        /// <param name="allowNull">A boolean value indicating whether null values should be allowed</param>
        /// <param name="context">The context in which the rule is used as source of computations</param>
        /// <param name="filter">A method or lambda expression to filter the computation objects</param>
        /// <returns>A source of computations that can further be dealt with</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static INotifyEnumerable<TransformationComputationWrapper<TInput1, TInput2, TOutput>> ToComputationSource<TInput1, TInput2, TOutput>(this TransformationRuleBase<TInput1, TInput2, TOutput> rule, ITransformationContext context, bool allowNull, Func<TransformationComputationWrapper<TInput1, TInput2, TOutput>, bool> filter)
            where TInput1 : class
            where TInput2 : class
            where TOutput : class
        {
            if (rule == null) throw new ArgumentNullException("rule");

            var trs = new TransformationRuleSource<TInput1, TInput2, TOutput>(rule, context) { Filter = filter };
            if (allowNull) trs.AddNullItem();
            return trs;
        }

        /// <summary>
        /// Creates a new computation source from a transformation rule
        /// </summary>
        /// <typeparam name="TInput">The type of the input arguments for the transformation rule</typeparam>
        /// <typeparam name="TOutput">The output type of the transformation rule</typeparam>
        /// <param name="rule">The rule that should be taken as a source of computation objects</param>
        /// <returns>A source of computations that can further be dealt with</returns>
        public static Func<ITransformationContext, INotifyEnumerable<TransformationComputationWrapper<TInput, TOutput>>> ToComputationSource<TInput, TOutput>(this TransformationRuleBase<TInput, TOutput> rule)
            where TInput : class
            where TOutput : class
        {
            if (rule == null) throw new ArgumentNullException("rule");

            return context => rule.ToComputationSource(context);
        }

        /// <summary>
        /// Creates a new computation source from a transformation rule
        /// </summary>
        /// <typeparam name="TInput">The type of the input arguments for the transformation rule</typeparam>
        /// <typeparam name="TOutput">The output type of the transformation rule</typeparam>
        /// <param name="rule">The rule that should be taken as a source of computation objects</param>
        /// <param name="allowNull">A boolean value indicating whether null values should be allowed</param>
        /// <returns>A source of computations that can further be dealt with</returns>
        public static Func<ITransformationContext, INotifyEnumerable<TransformationComputationWrapper<TInput, TOutput>>> ToComputationSource<TInput, TOutput>(this TransformationRuleBase<TInput, TOutput> rule, bool allowNull)
            where TInput : class
            where TOutput : class
        {
            if (rule == null) throw new ArgumentNullException("rule");

            return context => rule.ToComputationSource(context, allowNull);
        }

        /// <summary>
        /// Creates a new computation source from a transformation rule
        /// </summary>
        /// <typeparam name="TInput">The type of the input arguments for the transformation rule</typeparam>
        /// <typeparam name="TOutput">The output type of the transformation rule</typeparam>
        /// <param name="rule">The rule that should be taken as a source of computation objects</param>
        /// <param name="filter">A method or lambda expression to filter the computation objects</param>
        /// <returns>A source of computations that can further be dealt with</returns>
        public static Func<ITransformationContext, INotifyEnumerable<TransformationComputationWrapper<TInput, TOutput>>> ToComputationSource<TInput, TOutput>(this TransformationRuleBase<TInput, TOutput> rule, Func<TransformationComputationWrapper<TInput, TOutput>, bool> filter)
            where TInput : class
            where TOutput : class
        {
            if (rule == null) throw new ArgumentNullException("rule");

            return context => rule.ToComputationSource(context, filter);
        }

        /// <summary>
        /// Creates a new computation source from a transformation rule
        /// </summary>
        /// <typeparam name="TInput">The type of the input arguments for the transformation rule</typeparam>
        /// <typeparam name="TOutput">The output type of the transformation rule</typeparam>
        /// <param name="rule">The rule that should be taken as a source of computation objects</param>
        /// <param name="allowNull">A boolean value indicating whether null values should be allowed</param>
        /// <param name="filter">A method or lambda expression to filter the computation objects</param>
        /// <returns>A source of computations that can further be dealt with</returns>
        public static Func<ITransformationContext, INotifyEnumerable<TransformationComputationWrapper<TInput, TOutput>>> ToComputationSource<TInput, TOutput>(this TransformationRuleBase<TInput, TOutput> rule, bool allowNull, Func<TransformationComputationWrapper<TInput, TOutput>, bool> filter)
            where TInput : class
            where TOutput : class
        {
            if (rule == null) throw new ArgumentNullException("rule");

            return context => rule.ToComputationSource(context, allowNull, filter);
        }

        /// <summary>
        /// Creates a new computation source from a transformation rule
        /// </summary>
        /// <typeparam name="TInput1">The type of the first input arguments for the transformation rule</typeparam>
        /// <typeparam name="TInput2">The type of the second input arguments for the transformation rule</typeparam>
        /// <typeparam name="TOutput">The output type of the transformation rule</typeparam>
        /// <param name="rule">The rule that should be taken as a source of computation objects</param>
        /// <returns>A source of computations that can further be dealt with</returns>
        public static Func<ITransformationContext, INotifyEnumerable<TransformationComputationWrapper<TInput1, TInput2, TOutput>>> ToComputationSource<TInput1, TInput2, TOutput>(this TransformationRuleBase<TInput1, TInput2, TOutput> rule)
            where TInput1 : class
            where TInput2 : class
            where TOutput : class
        {
            if (rule == null) throw new ArgumentNullException("rule");

            return context => rule.ToComputationSource(context);
        }

        /// <summary>
        /// Creates a new computation source from a transformation rule
        /// </summary>
        /// <typeparam name="TInput1">The type of the first input arguments for the transformation rule</typeparam>
        /// <typeparam name="TInput2">The type of the second input arguments for the transformation rule</typeparam>
        /// <typeparam name="TOutput">The output type of the transformation rule</typeparam>
        /// <param name="rule">The rule that should be taken as a source of computation objects</param>
        /// <param name="allowNull">A boolean value indicating whether null values should be allowed</param>
        /// <returns>A source of computations that can further be dealt with</returns>
        public static Func<ITransformationContext, INotifyEnumerable<TransformationComputationWrapper<TInput1, TInput2, TOutput>>> ToComputationSource<TInput1, TInput2, TOutput>(this TransformationRuleBase<TInput1, TInput2, TOutput> rule, bool allowNull)
            where TInput1 : class
            where TInput2 : class
            where TOutput : class
        {
            if (rule == null) throw new ArgumentNullException("rule");

            return context => rule.ToComputationSource(context, allowNull);
        }

        /// <summary>
        /// Creates a new computation source from a transformation rule
        /// </summary>
        /// <typeparam name="TInput1">The type of the first input arguments for the transformation rule</typeparam>
        /// <typeparam name="TInput2">The type of the second input arguments for the transformation rule</typeparam>
        /// <typeparam name="TOutput">The output type of the transformation rule</typeparam>
        /// <param name="rule">The rule that should be taken as a source of computation objects</param>
        /// <param name="filter">A method or lambda expression to filter the computation objects</param>
        /// <returns>A source of computations that can further be dealt with</returns>
        public static Func<ITransformationContext, INotifyEnumerable<TransformationComputationWrapper<TInput1, TInput2, TOutput>>> ToComputationSource<TInput1, TInput2, TOutput>(this TransformationRuleBase<TInput1, TInput2, TOutput> rule, Func<TransformationComputationWrapper<TInput1, TInput2, TOutput>, bool> filter)
            where TInput1 : class
            where TInput2 : class
            where TOutput : class
        {
            if (rule == null) throw new ArgumentNullException("rule");

            return context => rule.ToComputationSource(context, filter);
        }

        /// <summary>
        /// Creates a new computation source from a transformation rule
        /// </summary>
        /// <typeparam name="TInput1">The type of the first input arguments for the transformation rule</typeparam>
        /// <typeparam name="TInput2">The type of the second input arguments for the transformation rule</typeparam>
        /// <typeparam name="TOutput">The output type of the transformation rule</typeparam>
        /// <param name="rule">The rule that should be taken as a source of computation objects</param>
        /// <param name="allowNull">A boolean value indicating whether null values should be allowed</param>
        /// <param name="filter">A method or lambda expression to filter the computation objects</param>
        /// <returns>A source of computations that can further be dealt with</returns>
        public static Func<ITransformationContext, INotifyEnumerable<TransformationComputationWrapper<TInput1, TInput2, TOutput>>> ToComputationSource<TInput1, TInput2, TOutput>(this TransformationRuleBase<TInput1, TInput2, TOutput> rule, bool allowNull, Func<TransformationComputationWrapper<TInput1, TInput2, TOutput>, bool> filter)
            where TInput1 : class
            where TInput2 : class
            where TOutput : class
        {
            if (rule == null) throw new ArgumentNullException("rule");

            return context => rule.ToComputationSource(context, allowNull, filter);
        }

        /// <summary>
        /// Creates a new computation source from a transformation rule
        /// </summary>
        /// <typeparam name="TInput">The type of the input arguments for the transformation rule</typeparam>
        /// <param name="rule">The rule that should be taken as a source of computation objects</param>
        /// <param name="context">The context in which the rule is used as source of computations</param>
        /// <returns>A source of computations that can further be dealt with</returns>
        public static INotifyEnumerable<InPlaceComputationWrapper<TInput>> ToComputationSource<TInput>(this InPlaceTransformationRuleBase<TInput> rule, ITransformationContext context)
            where TInput : class
        {
            if (rule == null) throw new ArgumentNullException("rule");

            return new InPlaceTransformationRuleSource<TInput>(rule, context);
        }

        /// <summary>
        /// Creates a new computation source from a transformation rule
        /// </summary>
        /// <typeparam name="TInput">The type of the input arguments for the transformation rule</typeparam>
        /// <param name="rule">The rule that should be taken as a source of computation objects</param>
        /// <param name="allowNull">A boolean value indicating whether null values should be allowed</param>
        /// <param name="context">The context in which the rule is used as source of computations</param>
        /// <returns>A source of computations that can further be dealt with</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static INotifyEnumerable<InPlaceComputationWrapper<TInput>> ToComputationSource<TInput>(this InPlaceTransformationRuleBase<TInput> rule, ITransformationContext context, bool allowNull)
            where TInput : class
        {
            if (rule == null) throw new ArgumentNullException("rule");

            var trs = new InPlaceTransformationRuleSource<TInput>(rule, context);
            if (allowNull) trs.AddNullItem();
            return trs;
        }

        /// <summary>
        /// Creates a new computation source from a transformation rule
        /// </summary>
        /// <typeparam name="TInput">The type of the input arguments for the transformation rule</typeparam>
        /// <param name="rule">The rule that should be taken as a source of computation objects</param>
        /// <param name="filter">A method or lambda expression to filter the computation objects</param>
        /// <param name="context">The context in which the rule is used as source of computations</param>
        /// <returns>A source of computations that can further be dealt with</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static INotifyEnumerable<InPlaceComputationWrapper<TInput>> ToComputationSource<TInput>(this InPlaceTransformationRuleBase<TInput> rule, ITransformationContext context, Func<InPlaceComputationWrapper<TInput>, bool> filter)
            where TInput : class
        {
            if (rule == null) throw new ArgumentNullException("rule");

            return new InPlaceTransformationRuleSource<TInput>(rule, context) { Filter = filter };
        }

        /// <summary>
        /// Creates a new computation source from a transformation rule
        /// </summary>
        /// <typeparam name="TInput">The type of the input arguments for the transformation rule</typeparam>
        /// <param name="rule">The rule that should be taken as a source of computation objects</param>
        /// <param name="allowNull">A boolean value indicating whether null values should be allowed</param>
        /// <param name="context">The context in which the rule is used as source of computations</param>
        /// <param name="filter">A method or lambda expression to filter the computation objects</param>
        /// <returns>A source of computations that can further be dealt with</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static INotifyEnumerable<InPlaceComputationWrapper<TInput>> ToComputationSource<TInput>(this InPlaceTransformationRuleBase<TInput> rule, ITransformationContext context, bool allowNull, Func<InPlaceComputationWrapper<TInput>, bool> filter)
            where TInput : class
        {
            if (rule == null) throw new ArgumentNullException("rule");

            var trs = new InPlaceTransformationRuleSource<TInput>(rule, context) { Filter = filter };
            if (allowNull) trs.AddNullItem();
            return trs;
        }

        /// <summary>
        /// Creates a new computation source from a transformation rule
        /// </summary>
        /// <typeparam name="TInput1">The type of the first input arguments for the transformation rule</typeparam>
        /// <typeparam name="TInput2">The type of the second input arguments for the transformation rule</typeparam>
        /// <param name="rule">The rule that should be taken as a source of computation objects</param>
        /// <param name="context">The context in which the rule is used as source of computations</param>
        /// <returns>A source of computations that can further be dealt with</returns>
        public static INotifyEnumerable<InPlaceComputationWrapper<TInput1, TInput2>> ToComputationSource<TInput1, TInput2>(this InPlaceTransformationRuleBase<TInput1, TInput2> rule, ITransformationContext context)
            where TInput1 : class
            where TInput2 : class
        {
            if (rule == null) throw new ArgumentNullException("rule");

            return new InPlaceTransformationRuleSource<TInput1, TInput2>(rule, context);
        }

        /// <summary>
        /// Creates a new computation source from a transformation rule
        /// </summary>
        /// <typeparam name="TInput1">The type of the first input arguments for the transformation rule</typeparam>
        /// <typeparam name="TInput2">The type of the second input arguments for the transformation rule</typeparam>
        /// <param name="rule">The rule that should be taken as a source of computation objects</param>
        /// <param name="allowNull">A boolean value indicating whether null values should be allowed</param>
        /// <param name="context">The context in which the rule is used as source of computations</param>
        /// <returns>A source of computations that can further be dealt with</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static INotifyEnumerable<InPlaceComputationWrapper<TInput1, TInput2>> ToComputationSource<TInput1, TInput2>(this InPlaceTransformationRuleBase<TInput1, TInput2> rule, ITransformationContext context, bool allowNull)
            where TInput1 : class
            where TInput2 : class
        {
            if (rule == null) throw new ArgumentNullException("rule");

            var trs = new InPlaceTransformationRuleSource<TInput1, TInput2>(rule, context);
            if (allowNull) trs.AddNullItem();
            return trs;
        }

        /// <summary>
        /// Creates a new computation source from a transformation rule
        /// </summary>
        /// <typeparam name="TInput1">The type of the first input arguments for the transformation rule</typeparam>
        /// <typeparam name="TInput2">The type of the second input arguments for the transformation rule</typeparam>
        /// <param name="rule">The rule that should be taken as a source of computation objects</param>
        /// <param name="filter">A method or lambda expression to filter the computation objects</param>
        /// <param name="context">The context in which the rule is used as source of computations</param>
        /// <returns>A source of computations that can further be dealt with</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static INotifyEnumerable<InPlaceComputationWrapper<TInput1, TInput2>> ToComputationSource<TInput1, TInput2>(this InPlaceTransformationRuleBase<TInput1, TInput2> rule, ITransformationContext context, Func<InPlaceComputationWrapper<TInput1, TInput2>, bool> filter)
            where TInput1 : class
            where TInput2 : class
        {
            if (rule == null) throw new ArgumentNullException("rule");

            return new InPlaceTransformationRuleSource<TInput1, TInput2>(rule, context) { Filter = filter };
        }

        /// <summary>
        /// Creates a new computation source from a transformation rule
        /// </summary>
        /// <typeparam name="TInput1">The type of the first input arguments for the transformation rule</typeparam>
        /// <typeparam name="TInput2">The type of the second input arguments for the transformation rule</typeparam>
        /// <param name="rule">The rule that should be taken as a source of computation objects</param>
        /// <param name="allowNull">A boolean value indicating whether null values should be allowed</param>
        /// <param name="context">The context in which the rule is used as source of computations</param>
        /// <param name="filter">A method or lambda expression to filter the computation objects</param>
        /// <returns>A source of computations that can further be dealt with</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static INotifyEnumerable<InPlaceComputationWrapper<TInput1, TInput2>> ToComputationSource<TInput1, TInput2>(this InPlaceTransformationRuleBase<TInput1, TInput2> rule, ITransformationContext context, bool allowNull, Func<InPlaceComputationWrapper<TInput1, TInput2>, bool> filter)
            where TInput1 : class
            where TInput2 : class
        {
            if (rule == null) throw new ArgumentNullException("rule");

            var trs = new InPlaceTransformationRuleSource<TInput1, TInput2>(rule, context) { Filter = filter };
            if (allowNull) trs.AddNullItem();
            return trs;
        }

        /// <summary>
        /// Creates a new computation source from a transformation rule
        /// </summary>
        /// <typeparam name="TInput">The type of the input arguments for the transformation rule</typeparam>
        /// <param name="rule">The rule that should be taken as a source of computation objects</param>
        /// <returns>A source of computations that can further be dealt with</returns>
        public static Func<ITransformationContext, INotifyEnumerable<InPlaceComputationWrapper<TInput>>> ToComputationSource<TInput>(this InPlaceTransformationRuleBase<TInput> rule)
            where TInput : class
            
        {
            if (rule == null) throw new ArgumentNullException("rule");

            return context => rule.ToComputationSource(context);
        }

        /// <summary>
        /// Creates a new computation source from a transformation rule
        /// </summary>
        /// <typeparam name="TInput">The type of the input arguments for the transformation rule</typeparam>
        /// <param name="rule">The rule that should be taken as a source of computation objects</param>
        /// <param name="allowNull">A boolean value indicating whether null values should be allowed</param>
        /// <returns>A source of computations that can further be dealt with</returns>
        public static Func<ITransformationContext, INotifyEnumerable<InPlaceComputationWrapper<TInput>>> ToComputationSource<TInput>(this InPlaceTransformationRuleBase<TInput> rule, bool allowNull)
            where TInput : class
            
        {
            if (rule == null) throw new ArgumentNullException("rule");

            return context => rule.ToComputationSource(context, allowNull);
        }

        /// <summary>
        /// Creates a new computation source from a transformation rule
        /// </summary>
        /// <typeparam name="TInput">The type of the input arguments for the transformation rule</typeparam>
        /// <param name="rule">The rule that should be taken as a source of computation objects</param>
        /// <param name="filter">A method or lambda expression to filter the computation objects</param>
        /// <returns>A source of computations that can further be dealt with</returns>
        public static Func<ITransformationContext, INotifyEnumerable<InPlaceComputationWrapper<TInput>>> ToComputationSource<TInput>(this InPlaceTransformationRuleBase<TInput> rule, Func<InPlaceComputationWrapper<TInput>, bool> filter)
            where TInput : class
            
        {
            if (rule == null) throw new ArgumentNullException("rule");

            return context => rule.ToComputationSource(context, filter);
        }

        /// <summary>
        /// Creates a new computation source from a transformation rule
        /// </summary>
        /// <typeparam name="TInput">The type of the input arguments for the transformation rule</typeparam>
        /// <param name="rule">The rule that should be taken as a source of computation objects</param>
        /// <param name="allowNull">A boolean value indicating whether null values should be allowed</param>
        /// <param name="filter">A method or lambda expression to filter the computation objects</param>
        /// <returns>A source of computations that can further be dealt with</returns>
        public static Func<ITransformationContext, INotifyEnumerable<InPlaceComputationWrapper<TInput>>> ToComputationSource<TInput>(this InPlaceTransformationRuleBase<TInput> rule, bool allowNull, Func<InPlaceComputationWrapper<TInput>, bool> filter)
            where TInput : class
            
        {
            if (rule == null) throw new ArgumentNullException("rule");

            return context => rule.ToComputationSource(context, allowNull, filter);
        }

        /// <summary>
        /// Creates a new computation source from a transformation rule
        /// </summary>
        /// <typeparam name="TInput1">The type of the first input arguments for the transformation rule</typeparam>
        /// <typeparam name="TInput2">The type of the second input arguments for the transformation rule</typeparam>
        /// <param name="rule">The rule that should be taken as a source of computation objects</param>
        /// <returns>A source of computations that can further be dealt with</returns>
        public static Func<ITransformationContext, INotifyEnumerable<InPlaceComputationWrapper<TInput1, TInput2>>> ToComputationSource<TInput1, TInput2>(this InPlaceTransformationRuleBase<TInput1, TInput2> rule)
            where TInput1 : class
            where TInput2 : class
        {
            if (rule == null) throw new ArgumentNullException("rule");

            return context => rule.ToComputationSource(context);
        }

        /// <summary>
        /// Creates a new computation source from a transformation rule
        /// </summary>
        /// <typeparam name="TInput1">The type of the first input arguments for the transformation rule</typeparam>
        /// <typeparam name="TInput2">The type of the second input arguments for the transformation rule</typeparam>
        /// <param name="rule">The rule that should be taken as a source of computation objects</param>
        /// <param name="allowNull">A boolean value indicating whether null values should be allowed</param>
        /// <returns>A source of computations that can further be dealt with</returns>
        public static Func<ITransformationContext, INotifyEnumerable<InPlaceComputationWrapper<TInput1, TInput2>>> ToComputationSource<TInput1, TInput2>(this InPlaceTransformationRuleBase<TInput1, TInput2> rule, bool allowNull)
            where TInput1 : class
            where TInput2 : class
            
        {
            if (rule == null) throw new ArgumentNullException("rule");

            return context => rule.ToComputationSource(context, allowNull);
        }

        /// <summary>
        /// Creates a new computation source from a transformation rule
        /// </summary>
        /// <typeparam name="TInput1">The type of the first input arguments for the transformation rule</typeparam>
        /// <typeparam name="TInput2">The type of the second input arguments for the transformation rule</typeparam>
        /// <param name="rule">The rule that should be taken as a source of computation objects</param>
        /// <param name="filter">A method or lambda expression to filter the computation objects</param>
        /// <returns>A source of computations that can further be dealt with</returns>
        public static Func<ITransformationContext, INotifyEnumerable<InPlaceComputationWrapper<TInput1, TInput2>>> ToComputationSource<TInput1, TInput2>(this InPlaceTransformationRuleBase<TInput1, TInput2> rule, Func<InPlaceComputationWrapper<TInput1, TInput2>, bool> filter)
            where TInput1 : class
            where TInput2 : class
            
        {
            if (rule == null) throw new ArgumentNullException("rule");

            return context => rule.ToComputationSource(context, filter);
        }

        /// <summary>
        /// Creates a new computation source from a transformation rule
        /// </summary>
        /// <typeparam name="TInput1">The type of the first input arguments for the transformation rule</typeparam>
        /// <typeparam name="TInput2">The type of the second input arguments for the transformation rule</typeparam>
        /// <param name="rule">The rule that should be taken as a source of computation objects</param>
        /// <param name="allowNull">A boolean value indicating whether null values should be allowed</param>
        /// <param name="filter">A method or lambda expression to filter the computation objects</param>
        /// <returns>A source of computations that can further be dealt with</returns>
        public static Func<ITransformationContext, INotifyEnumerable<InPlaceComputationWrapper<TInput1, TInput2>>> ToComputationSource<TInput1, TInput2>(this InPlaceTransformationRuleBase<TInput1, TInput2> rule, bool allowNull, Func<InPlaceComputationWrapper<TInput1, TInput2>, bool> filter)
            where TInput1 : class
            where TInput2 : class
            
        {
            if (rule == null) throw new ArgumentNullException("rule");

            return context => rule.ToComputationSource(context, allowNull, filter);
        }

        /// <summary>
        /// Filters the given monad instance with the given filter operator
        /// </summary>
        /// <typeparam name="T">The inner type of the monad instance</typeparam>
        /// <param name="items">The monad instance that should be filtered</param>
        /// <param name="filter">The filter that should be applied to the monad</param>
        /// <returns>A filtered monad instance</returns>
        /// <remarks>Please see the documentation of the Where extension method and its use with the LINQ-syntax for more details</remarks>
        public static Func<ITransformationContext, INotifyEnumerable<T>> Where<T>(this Func<ITransformationContext, INotifyEnumerable<T>> items, Expression<Func<T, bool>> filter)
        {
            if (items == null) throw new ArgumentNullException("items");
            if (filter == null) throw new ArgumentNullException("filter");

            return p => items(p).Where(filter);
        }

        /// <summary>
        /// Binds the Monad IRelationalSource to child items
        /// </summary>
        /// <typeparam name="T1">The inner type of the input monad instance</typeparam>
        /// <typeparam name="T2">The inner type of the result monad instance</typeparam>
        /// <param name="items">The monad instance whose child items should be selected</param>
        /// <param name="selector">A method that selects the result items from a source item</param>
        /// <returns>A monad instance of the result type that is based on the source monad instance</returns>
        public static Func<ITransformationContext, INotifyEnumerable<T2>> Select<T1, T2>(this Func<ITransformationContext, INotifyEnumerable<T1>> items, Expression<Func<T1, T2>> selector)
        {
            if (items == null) throw new ArgumentNullException("items");
            if (selector == null) throw new ArgumentNullException("selector");

            return p => items(p).Select(selector);
        }

        /// <summary>
        /// Binds the Monad IRelationalSource to child items
        /// </summary>
        /// <typeparam name="T1">The inner type of the outer monad instance</typeparam>
        /// <typeparam name="T2">The inner type of the inner monad instance</typeparam>
        /// <param name="items">The monad instance which children are to be selected</param>
        /// <param name="selector">A method that selects the output</param>
        /// <returns>A monad instance with items selected by the given selector function</returns>
        /// <remarks>Please see the documentation of the SelectMany extension method and its use with the LINQ-syntax for more details</remarks>
        public static Func<ITransformationContext, INotifyEnumerable<T2>> SelectMany<T1, T2>(this Func<ITransformationContext, INotifyEnumerable<T1>> items, Expression<Func<T1, Func<ITransformationContext, IEnumerable<T2>>>> selector)
        {
            if (items == null) throw new ArgumentNullException("items");
            if (selector == null) throw new ArgumentNullException("selector");

            return p =>
            {
                var newFunc = Expression.Lambda<Func<T1, IEnumerable<T2>>>
                    (Expression.Invoke(selector.Body, Expression.Constant(p)), selector.Parameters.ToArray());

                return items(p).SelectMany(newFunc);
            };
        }

        /// <summary>
        /// Binds the Monad IRelationalSource to child items
        /// </summary>
        /// <typeparam name="T1">The inner type of the outer monad instance</typeparam>
        /// <typeparam name="T2">The inner type of the inner monad instance</typeparam>
        /// <typeparam name="T3">The inner type of the return monad</typeparam>
        /// <param name="items">The monad instance which children are to be selected</param>
        /// <param name="func">The operator that should be applied in the bind function</param>
        /// <param name="selector">A method that selects the output</param>
        /// <returns>A monad instance with items selected by the given selector function</returns>
        /// <remarks>Please see the documentation of the SelectMany extension method and its use with the LINQ-syntax for more details</remarks>
        public static Func<ITransformationContext, INotifyEnumerable<T3>> SelectMany<T1, T2, T3>(this Func<ITransformationContext, INotifyEnumerable<T1>> items, Expression<Func<T1, Func<ITransformationContext, IEnumerable<T2>>>> func, Expression<Func<T1, T2, T3>> selector)
        {
            if (items == null) throw new ArgumentNullException("items");
            if (func == null) throw new ArgumentNullException("func");
            if (selector == null) throw new ArgumentNullException("selector");

            return p =>
            {
                var newFunc = Expression.Lambda<Func<T1, IEnumerable<T2>>>
                    (Expression.Invoke(func.Body, Expression.Constant(p)), func.Parameters.ToArray());

                return items(p).SelectMany(newFunc, selector);
            };
        }
    }
}
