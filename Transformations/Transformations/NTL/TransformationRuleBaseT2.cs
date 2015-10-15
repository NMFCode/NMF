using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMF.Utilities;
using NMF.Transformations.Properties;
using NMF.Transformations.Core;
using System.ComponentModel;
using System.Collections;

namespace NMF.Transformations
{
    /// <summary>
    /// Defines a transformation rule of a transformation that has two input arguments and an output
    /// </summary>
    /// <typeparam name="TIn1">The type of the first input argument</typeparam>
    /// <typeparam name="TIn2">The type of the second input argument</typeparam>
    /// <typeparam name="TOut">The type of the output</typeparam>
    public abstract class TransformationRuleBase<TIn1, TIn2, TOut> : GeneralTransformationRule<TIn1, TIn2>
        where TIn1 : class
        where TIn2 : class
        where TOut : class
    {

        /// <summary>
        /// Marks the current transformation rule instantiating for every rule from S to T
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type</typeparam>
        /// <typeparam name="TRequiredOutput">The output argument type</typeparam>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void MarkInstantiatingFor<TRequiredInput1, TRequiredInput2, TRequiredOutput>()
            where TRequiredInput1 : class
            where TRequiredInput2 : class
            where TRequiredOutput : class
        {
            var rules = Transformation.GetRulesExact(new Type[] { typeof(TRequiredInput1), typeof(TRequiredInput2) }, typeof(TRequiredOutput))
                        .Where(r => r != this);

            if (rules.Count() > 1) throw new InvalidOperationException("A rule must not instantiate more than one other rule!");

            var rule = rules.FirstOrDefault();
            if (rule != null)
            MarkInstantiatingFor(rule);
        }

        /// <summary>
        /// Marks the current transformation rule instantiating for every rule from S to T
        /// </summary>
        /// <param name="filter">The filter that should be used to filter the inputs</param>
        /// <typeparam name="TRequiredInput1">The first input argument type</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type</typeparam>
        /// <typeparam name="TRequiredOutput">The output argument type</typeparam>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void MarkInstantiatingFor<TRequiredInput1, TRequiredInput2, TRequiredOutput>(Func<TIn1, TIn2, bool> filter)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
            where TRequiredOutput : class
        {
            var rules = Transformation.GetRulesExact(new Type[] { typeof(TRequiredInput1), typeof(TRequiredInput2) }, typeof(TRequiredOutput))
                        .Where(r => r != this);

            if (rules.Count() > 1) throw new InvalidOperationException("A rule must not instantiate more than one other rule!");

            var rule = rules.FirstOrDefault();
            if (rule != null)
            MarkInstantiatingFor(rule, c => HasCompliantInput(c) && filter(c.GetInput(0) as TIn1, c.GetInput(1) as TIn2));
        }

        /// <summary>
        /// Gets the output type of this transformation rule
        /// </summary>
        public sealed override Type OutputType
        {
            get { return typeof(TOut); }
        }


        /// <summary>
        /// Requires all transformation rules that transform items from S to T
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformations</typeparam>
        /// <param name="persistor">A method that persists the result of the dependent transformations</param>
        /// <remarks>This version Always takes the input parameter as input for the dependent transformations. Thus, this method will throw an exception, if the types do not match</remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void RequireByType<TRequiredInput1, TRequiredInput2, TRequiredOutput>(Action<TOut, TRequiredOutput> persistor)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
            where TRequiredOutput : class
        {
            if (typeof(TRequiredInput1).IsAssignableFrom(typeof(TIn1))
                && typeof(TRequiredInput2).IsAssignableFrom(typeof(TIn2))
                && typeof(TRequiredOutput).IsAssignableFrom(OutputType))
            {
                foreach (var rule in Transformation.GetRulesForTypeSignature(new Type[] { typeof(TRequiredInput1), typeof(TRequiredInput2)}, typeof(TRequiredOutput)))
                {
                    Depend(null, c => c.CreateInputArray(), rule, (o1, o2) => persistor((TOut)o1, (TRequiredOutput)o2), true, false);
                }
            }
            else
            {
                throw new InvalidOperationException(Resources.ErrRequires1ArgNoSelectorMustInherit);
            }
        }

        /// <summary>
        /// Requires the transformation rule with the given type
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformation</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformation</typeparam>
        /// <param name="persistor">A method that persists the result of the dependent transformation</param>
        /// <remarks>This version Always takes the input parameter as input for the dependent transformations. Thus, this method will throw an exception, if the types do not match</remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public ITransformationRuleDependency Require<TRequiredInput1, TRequiredInput2, TRequiredOutput>(TransformationRuleBase<TRequiredInput1, TRequiredInput2, TRequiredOutput> rule, Action<TOut, TRequiredOutput> persistor)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
            where TRequiredOutput : class
        {
            if (typeof(TRequiredInput1).IsAssignableFrom(typeof(TIn1)))
            {
                return Depend(null, c => c.CreateInputArray(), rule, (o1, o2) => persistor((TOut)o1, (TRequiredOutput)o2), true, false);
            }
            else
            {
                throw new InvalidOperationException(Resources.ErrRequiresTransNoSelectorMustInherit);
            }
        }

        /// <summary>
        /// Requires all transformation rules that transform items from S to T
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformations</typeparam>
        /// <param name="selector1">A method that selects the first input for the dependent transformations</param>
        /// <param name="selector2">A method that selects the second input for the dependent transformations</param>
        /// <param name="persistor">A method that persists the result of the dependent transformations</param>
        public void RequireByType<TRequiredInput1, TRequiredInput2, TRequiredOutput>(Func<TIn1, TIn2, TRequiredInput1> selector1, Func<TIn1, TIn2, TRequiredInput2> selector2, Action<TOut, TRequiredOutput> persistor)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
            where TRequiredOutput : class
        {
            if (persistor == null) persistor = (p1, p2) => { };
            if (selector1 == null) throw new ArgumentNullException("selector1");
            if (selector2 == null) throw new ArgumentNullException("selector2");
            foreach (var rule in Transformation.GetRulesForTypeSignature(new Type[] { typeof(TRequiredInput1), typeof(TRequiredInput2) }, typeof(TRequiredOutput)))
            {
                Depend(null, c => DependencySelect(selector1, selector2, c), rule, (s, t) => persistor((TOut)s, (TRequiredOutput)t), true, false);
            }
        }

        /// <summary>
        /// Requires all transformation rules that transform items from S to T
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformations</typeparam>
        /// <param name="selector">A method that selects the input for the dependent transformations</param>
        /// <param name="persistor">A method that persists the result of the dependent transformations</param>
        public void RequireByType<TRequiredInput, TRequiredOutput>(Func<TIn1, TIn2, TRequiredInput> selector, Action<TOut, TRequiredOutput> persistor)
            where TRequiredInput : class
            where TRequiredOutput : class
        {
            if (persistor == null) persistor = (p1, p2) => { };
            if (selector == null) throw new ArgumentNullException("selector");
            foreach (var rule in Transformation.GetRulesForTypeSignature(new Type[] { typeof(TRequiredInput) }, typeof(TRequiredOutput)))
            {
                Depend(null, c => DependencySelect(selector, c), rule, (s, t) => persistor((TOut)s, (TRequiredOutput)t), true, false);
            }
        }

        /// <summary>
        /// Requires the transformation rule with the given type
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformations</typeparam>
        /// <param name="selector1">A method that selects the first input for the dependent transformations</param>
        /// <param name="selector2">A method that selects the second input for the dependent transformations</param>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        /// <param name="persistor">A method that persists the result of the dependent transformation</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency Require<TRequiredInput1, TRequiredInput2, TRequiredOutput>(TransformationRuleBase<TRequiredInput1, TRequiredInput2, TRequiredOutput> rule, Func<TIn1, TIn2, TRequiredInput1> selector1, Func<TIn1, TIn2, TRequiredInput2> selector2, Action<TOut, TRequiredOutput> persistor)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
            where TRequiredOutput : class
        {
            if (persistor == null) persistor = (p1, p2) => { };
            if (selector1 == null) throw new ArgumentNullException("selector1");
            if (selector2 == null) throw new ArgumentNullException("selector2");
            return Depend(null, c => DependencySelect(selector1, selector2, c), rule, (s, t) => persistor((TOut)s, (TRequiredOutput)t), true, false);
        }

        /// <summary>
        /// Requires the transformation rule with the given type
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformations</typeparam>
        /// <param name="selector">A method that selects the input for the dependent transformations</param>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        /// <param name="persistor">A method that persists the result of the dependent transformation</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency Require<TRequiredInput, TRequiredOutput>(TransformationRuleBase<TRequiredInput, TRequiredOutput> rule, Func<TIn1, TIn2, TRequiredInput> selector, Action<TOut, TRequiredOutput> persistor)
            where TRequiredInput : class
            where TRequiredOutput : class
        {
            if (persistor == null) persistor = (p1, p2) => { };
            if (selector == null) throw new ArgumentNullException("selector");
            return Depend(null, c => DependencySelect(selector, c), rule, (s, t) => persistor((TOut)s, (TRequiredOutput)t), true, false);
        }

        /// <summary>
        /// Requires all transformation rules that transform S to T with all of the specified objects
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformations</typeparam>
        /// <param name="selector">A method that selects the inputs for the dependent transformations</param>
        /// <param name="persistor">A method that persists the result of the dependent transformation</param>
        
        public void RequireManyByType<TRequiredInput1, TRequiredInput2, TRequiredOutput>(Func<TIn1, TIn2, IEnumerable<Tuple<TRequiredInput1, TRequiredInput2>>> selector, Action<TOut, IEnumerable<TRequiredOutput>> persistor)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
            where TRequiredOutput : class
        {
            if (persistor == null) persistor = (p1, p2) => { };
            if (selector == null) throw new ArgumentNullException("selector");
            foreach (var rule in Transformation.GetRulesForTypeSignature(new Type[] { typeof(TRequiredInput1), typeof(TRequiredInput2) }, typeof(TRequiredOutput)))
            {
                DependMany(null, c => DependencySelectMany(selector, c), rule, (s, t) => persistor((TOut)s, (IEnumerable<TRequiredOutput>)t), true, false);
            }
        }

        /// <summary>
        /// Requires all transformation rules that transform S to T with all of the specified objects
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformations</typeparam>
        /// <param name="selector">A method that selects the inputs for the dependent transformations</param>
        /// <param name="persistor">A method that persists the result of the dependent transformation</param>
        
        public void RequireManyByType<TRequiredInput, TRequiredOutput>(Func<TIn1, TIn2, IEnumerable<TRequiredInput>> selector, Action<TOut, IEnumerable<TRequiredOutput>> persistor)
            where TRequiredInput : class
            where TRequiredOutput : class
        {
            if (persistor == null) persistor = (p1, p2) => { };
            if (selector == null) throw new ArgumentNullException("selector");
            foreach (var rule in Transformation.GetRulesForTypeSignature(new Type[] { typeof(TRequiredInput) }, typeof(TRequiredOutput)))
            {
                DependMany(null, c => DependencySelectMany(selector, c), rule, (s, t) => persistor((TOut)s, (IEnumerable<TRequiredOutput>)t), true, false);
            }
        }

        /// <summary>
        /// Requires the transformation rule with the given type with all of the specified inputs
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformation</typeparam>
        /// <param name="selector">A method that selects the inputs for the dependent transformations</param>
        /// <param name="persistor">A method that persists the result of the dependent transformation</param>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency RequireMany<TRequiredInput1, TRequiredInput2, TRequiredOutput>(TransformationRuleBase<TRequiredInput1, TRequiredInput2, TRequiredOutput> rule, Func<TIn1, TIn2, IEnumerable<Tuple<TRequiredInput1, TRequiredInput2>>> selector, Action<TOut, IEnumerable<TRequiredOutput>> persistor)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
            where TRequiredOutput : class
        {
            if (persistor == null) persistor = (p1, p2) => { };
            if (selector == null) throw new ArgumentNullException("selector");
            return DependMany(null, c => DependencySelectMany(selector, c), rule, (s, t) => persistor((TOut)s, (IEnumerable<TRequiredOutput>)t), true, false);
        }

        /// <summary>
        /// Requires the transformation rule with the given type with all of the specified inputs
        /// </summary>
        /// <typeparam name="TRequiredInput">The first input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformation</typeparam>
        /// <param name="selector">A method that selects the inputs for the dependent transformations</param>
        /// <param name="persistor">A method that persists the result of the dependent transformation</param>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency RequireMany<TRequiredInput, TRequiredOutput>(TransformationRuleBase<TRequiredInput, TRequiredOutput> rule, Func<TIn1, TIn2, IEnumerable<TRequiredInput>> selector, Action<TOut, IEnumerable<TRequiredOutput>> persistor)
            where TRequiredInput : class
            where TRequiredOutput : class
        {
            if (persistor == null) persistor = (p1, p2) => { };
            if (selector == null) throw new ArgumentNullException("selector");
            return DependMany(null, c => DependencySelectMany(selector, c), rule, (s, t) => persistor((TOut)s, (IEnumerable<TRequiredOutput>)t), true, false);
        }

        /// <summary>
        /// Calls all transformation rules that transform items from S to T after the current transformation rule
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformations</typeparam>
        /// <param name="persistor">A method that persists the result of the dependent transformations</param>
        /// <remarks>This version Always takes the input parameter as input for the dependent transformations. Thus, this method will throw an exception, if the types do not match</remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void CallByType<TRequiredInput1, TRequiredInput2, TRequiredOutput>(Action<TOut, TRequiredOutput> persistor)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
            where TRequiredOutput : class
        {
            if (typeof(TRequiredInput1).IsAssignableFrom(typeof(TIn1))
                && typeof(TRequiredInput2).IsAssignableFrom(typeof(TIn2)))
            {
                foreach (var rule in Transformation.GetRulesForTypeSignature(new Type[] { typeof(TRequiredInput1), typeof(TRequiredInput2)}, typeof(TRequiredOutput)))
                {
                    Depend(null, c => c.CreateInputArray(), rule, (o1, o2) => persistor((TOut)o1, (TRequiredOutput)o2), false, false);
                }
            }
            else
            {
                throw new InvalidOperationException(Resources.ErrCall2ArgNoSelectorMustInherit);
            }
        }

        /// <summary>
        /// Calls the transformation rule with the given type after the current transformation rule
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformation</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformation</typeparam>
        /// <param name="persistor">A method that persists the result of the dependent transformation</param>
        /// <remarks>This version Always takes the input parameter as input for the dependent transformations. Thus, this method will throw an exception, if the types do not match</remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public ITransformationRuleDependency Call<TRequiredInput1, TRequiredInput2, TRequiredOutput>(TransformationRuleBase<TRequiredInput1, TRequiredInput2, TRequiredOutput> rule, Action<TOut, TRequiredOutput> persistor)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
            where TRequiredOutput : class
        {
            var inputTypes = new Type[] { typeof(TRequiredInput1), typeof(TRequiredInput2) };
            if (inputTypes.IsAssignableArrayFrom(InputType))
            {
                return Depend(null, c => c.CreateInputArray(), rule, (o1, o2) => persistor((TOut)o1, (TRequiredOutput)o2), false, false);
            }
            else
            {
                throw new InvalidOperationException(Resources.ErrCallTransNoSelectorMustInherit);
            }
        }

        /// <summary>
        /// Calls all transformation rules that transform items from S to T after the current transformation rule
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformations</typeparam>
        /// <param name="selector1">A method that selects the first input for the dependent transformations</param>
        /// <param name="selector2">A method that selects the second input for the dependent transformation</param>
        /// <param name="persistor">A method that persists the result of the dependent transformations</param>
        public void CallByType<TRequiredInput1, TRequiredInput2, TRequiredOutput>(Func<TIn1, TIn2, TRequiredInput1> selector1, Func<TIn1, TIn2, TRequiredInput2> selector2, Action<TOut, TRequiredOutput> persistor)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
            where TRequiredOutput : class
        {
            if (persistor == null) persistor = (p1, p2) => { };
            if (selector1 == null) throw new ArgumentNullException("selector1");
            if (selector2 == null) throw new ArgumentNullException("selector2");
            foreach (var rule in Transformation.GetRulesForTypeSignature(new Type[] { typeof(TRequiredInput1), typeof(TRequiredInput2) }, typeof(TRequiredOutput)))
            {
                Depend(null, c => DependencySelect(selector1, selector2, c), rule, (s, t) => persistor((TOut)s, (TRequiredOutput)t), false, false);
            }
        }

        /// <summary>
        /// Calls all transformation rules that transform items from S to T after the current transformation rule
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformations</typeparam>
        /// <param name="selector">A method that selects the input for the dependent transformations</param>
        /// <param name="persistor">A method that persists the result of the dependent transformations</param>
        public void CallByType<TRequiredInput, TRequiredOutput>(Func<TIn1, TIn2, TRequiredInput> selector, Action<TOut, TRequiredOutput> persistor)
            where TRequiredInput : class
            where TRequiredOutput : class
        {
            if (persistor == null) persistor = (p1, p2) => { };
            if (selector == null) throw new ArgumentNullException("selector");
            foreach (var rule in Transformation.GetRulesForTypeSignature(new Type[] { typeof(TRequiredInput) }, typeof(TRequiredOutput)))
            {
                Depend(null, c => DependencySelect(selector, c), rule, (s, t) => persistor((TOut)s, (TRequiredOutput)t), false, false);
            }
        }

        /// <summary>
        /// Calls the transformation rule with the given type after the current transformation rule
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformation</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformation</typeparam>
        /// <param name="selector1">A method that selects the frist input for the dependent transformation</param>
        /// <param name="selector2">A method that selects the second input for the dependent transformation</param>
        /// <param name="persistor">A method that persists the result of the dependent transformation</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency Call<TRequiredInput1, TRequiredInput2, TRequiredOutput>(TransformationRuleBase<TRequiredInput1, TRequiredInput2, TRequiredOutput> rule, Func<TIn1, TIn2, TRequiredInput1> selector1, Func<TIn1, TIn2, TRequiredInput2> selector2, Action<TOut, TRequiredOutput> persistor)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
            where TRequiredOutput : class
        {
            if (persistor == null) persistor = (p1, p2) => { };
            if (selector1 == null) throw new ArgumentNullException("selector1");
            if (selector2 == null) throw new ArgumentNullException("selector2");
            return Depend(null, c => DependencySelect(selector1, selector2, c), rule, (s, t) => persistor((TOut)s, (TRequiredOutput)t), false, false);
        }

        /// <summary>
        /// Calls the transformation rule with the given type after the current transformation rule
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the dependent transformation</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformation</typeparam>
        /// <param name="selector">A method that selects the input for the dependent transformation</param>
        /// <param name="persistor">A method that persists the result of the dependent transformation</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency Call<TRequiredInput, TRequiredOutput>(TransformationRuleBase<TRequiredInput, TRequiredOutput> rule, Func<TIn1, TIn2, TRequiredInput> selector, Action<TOut, TRequiredOutput> persistor)
            where TRequiredInput : class
            where TRequiredOutput : class
        {
            if (persistor == null) persistor = (p1, p2) => { };
            if (selector == null) throw new ArgumentNullException("selector");
            return Depend(null, c => DependencySelect(selector, c), rule, (s, t) => persistor((TOut)s, (TRequiredOutput)t), false, false);
        }

        /// <summary>
        /// Calls all transformation rules that transform S to T with all of the specified objects after the current transformation rule
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformations</typeparam>
        /// <param name="selector">A method that selects the inputs for the dependent transformations</param>
        /// <param name="persistor">A method that persists the result of the dependent transformation</param>
        
        public void CallManyByType<TRequiredInput1, TRequiredInput2, TRequiredOutput>(Func<TIn1, TIn2, IEnumerable<Tuple<TRequiredInput1, TRequiredInput2>>> selector, Action<TOut, IEnumerable<TRequiredOutput>> persistor)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
            where TRequiredOutput : class
        {
            if (persistor == null) persistor = (p1, p2) => { };
            if (selector == null) throw new ArgumentNullException("selector");
            foreach (var rule in Transformation.GetRulesForTypeSignature(new Type[] { typeof(TRequiredInput1), typeof(TRequiredInput2) }, typeof(TRequiredOutput)))
            {
                DependMany(null, c => DependencySelectMany(selector, c), rule, (s, t) => persistor((TOut)s, (IEnumerable<TRequiredOutput>)t), false, false);
            }
        }

        /// <summary>
        /// Calls all transformation rules that transform S to T with all of the specified objects after the current transformation rule
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformations</typeparam>
        /// <param name="selector">A method that selects the inputs for the dependent transformations</param>
        /// <param name="persistor">A method that persists the result of the dependent transformation</param>
        
        public void CallManyByType<TRequiredInput, TRequiredOutput>(Func<TIn1, TIn2, IEnumerable<TRequiredInput>> selector, Action<TOut, IEnumerable<TRequiredOutput>> persistor)
            where TRequiredInput : class
            where TRequiredOutput : class
        {
            if (persistor == null) persistor = (p1, p2) => { };
            if (selector == null) throw new ArgumentNullException("selector");
            foreach (var rule in Transformation.GetRulesForTypeSignature(new Type[] { typeof(TRequiredInput) }, typeof(TRequiredOutput)))
            {
                DependMany(null, c => DependencySelectMany(selector, c), rule, (s, t) => persistor((TOut)s, (IEnumerable<TRequiredOutput>)t), false, false);
            }
        }

        /// <summary>
        /// Calls the transformation rule with the given type with all of the specified inputs after the current transformation rule
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformation</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformation</typeparam>
        /// <param name="selector">A method that selects the inputs for the dependent transformations</param>
        /// <param name="persistor">A method that persists the result of the dependent transformation</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency CallMany<TRequiredInput1, TRequiredInput2, TRequiredOutput>(TransformationRuleBase<TRequiredInput1, TRequiredInput2, TRequiredOutput> rule, Func<TIn1, TIn2, IEnumerable<Tuple<TRequiredInput1, TRequiredInput2>>> selector, Action<TOut, IEnumerable<TRequiredOutput>> persistor)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
            where TRequiredOutput : class
        {
            if (persistor == null) persistor = (p1, p2) => { };
            if (selector == null) throw new ArgumentNullException("selector");
            return DependMany(null, c => DependencySelectMany(selector, c), rule, (s, t) => persistor((TOut)s, (IEnumerable<TRequiredOutput>)t), false, false);
        }

        /// <summary>
        /// Calls the transformation rule with the given type with all of the specified inputs after the current transformation rule
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the dependent transformation</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformation</typeparam>
        /// <param name="selector">A method that selects the inputs for the dependent transformations</param>
        /// <param name="persistor">A method that persists the result of the dependent transformation</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency CallMany<TRequiredInput, TRequiredOutput>(TransformationRuleBase<TRequiredInput, TRequiredOutput> rule, Func<TIn1, TIn2, IEnumerable<TRequiredInput>> selector, Action<TOut, IEnumerable<TRequiredOutput>> persistor)
            where TRequiredInput : class
            where TRequiredOutput : class
        {
            if (persistor == null) persistor = (p1, p2) => { };
            if (selector == null) throw new ArgumentNullException("selector");
            return DependMany(null, c => DependencySelectMany(selector, c), rule, (s, t) => persistor((TOut)s, (IEnumerable<TRequiredOutput>)t), false, false);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation from S to T is made
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the source transformation rule</typeparam>
        /// <param name="selector1">A method that selects the frist input for this transformation rule</param>
        /// <param name="selector2">A method that selects the second input for this transformation rule</param>
        /// <param name="persistor">A method that persists the output of this rule back to the source instance</param>
        public void CallFor<TRequiredInput1, TRequiredInput2, TRequiredOutput>(Func<TRequiredInput1, TRequiredInput2, TIn1> selector1, Func<TRequiredInput1, TRequiredInput2, TIn2> selector2, Action<TOut, TRequiredOutput> persistor)
        {
            CallFor<TRequiredInput1, TRequiredInput2, TRequiredOutput>(selector1, selector2, (s1, s2) => true, persistor);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation from S to T is made
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the source transformation rule</typeparam>
        /// <param name="selector1">A method that selects the frist input for this transformation rule</param>
        /// <param name="selector2">A method that selects the second input for this transformation rule</param>
        /// <param name="persistor">A method that persists the output of this rule back to the source instance</param>
        public void CallFor<TRequiredInput, TRequiredOutput>(Func<TRequiredInput, TIn1> selector1, Func<TRequiredInput, TIn2> selector2, Action<TOut, TRequiredOutput> persistor)
        {
            CallFor<TRequiredInput, TRequiredOutput>(selector1, selector2, s => true, persistor);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation from S to T is made
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the source transformation rule</typeparam>
        /// <param name="selector1">A method that selects the first input for this transformation rule</param>
        /// <param name="selector2">A method that selects the second input for this transformation rule</param>
        /// <param name="persistor">A method that persists the output of this rule back to the source instance</param>
        /// <param name="filter">A method that filters the applicable instances of S</param>
        public void CallFor<TRequiredInput1, TRequiredInput2, TRequiredOutput>(Func<TRequiredInput1, TRequiredInput2, TIn1> selector1, Func<TRequiredInput1, TRequiredInput2, TIn2> selector2, Func<TRequiredInput1, TRequiredInput2, bool> filter, Action<TOut, TRequiredOutput> persistor)
        {
            if (persistor == null) persistor = (p1, p2) => { };
            if (filter == null) throw new ArgumentNullException("filter");
            if (selector1 == null) throw new ArgumentNullException("selector1");
            if (selector2 == null) throw new ArgumentNullException("selector2");
            CallForInternal(new Type[] { typeof(TRequiredInput1), typeof(TRequiredInput2) }, typeof(TRequiredOutput), c => CallFilter(filter, c), o => CallDependencySelect(selector1, selector2, o), (s, t) => persistor((TOut)s, (TRequiredOutput)t), false);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation from S to T is made
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the source transformation rule</typeparam>
        /// <param name="selector1">A method that selects the first input for this transformation rule</param>
        /// <param name="selector2">A method that selects the second input for this transformation rule</param>
        /// <param name="persistor">A method that persists the output of this rule back to the source instance</param>
        /// <param name="filter">A method that filters the applicable instances of S</param>
        public void CallFor<TRequiredInput, TRequiredOutput>(Func<TRequiredInput, TIn1> selector1, Func<TRequiredInput, TIn2> selector2, Func<TRequiredInput, bool> filter, Action<TOut, TRequiredOutput> persistor)
        {
            if (persistor == null) persistor = (p1, p2) => { };
            if (filter == null) throw new ArgumentNullException("filter");
            if (selector1 == null) throw new ArgumentNullException("selector1");
            if (selector2 == null) throw new ArgumentNullException("selector2");
            CallForInternal(new Type[] { typeof(TRequiredInput) }, typeof(TRequiredOutput), c => CallFilter(filter, c), o => CallDependencySelect(selector1, selector2, o), (s, t) => persistor((TOut)s, (TRequiredOutput)t), false);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation from S to T is made
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the source transformation rule</typeparam>
        /// <param name="selector1">A method that selects the frist input for this transformation rule</param>
        /// <param name="selector2">A method that selects the second input for this transformation rule</param>
        /// <param name="persistor">A method that persists the output of this rule back to the source instance</param>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        public void CallFor<TRequiredInput1, TRequiredInput2, TRequiredOutput>(TransformationRuleBase<TRequiredInput1, TRequiredInput2, TRequiredOutput> rule, Func<TRequiredInput1, TRequiredInput2, TIn1> selector1, Func<TRequiredInput1, TRequiredInput2, TIn2> selector2, Action<TOut, TRequiredOutput> persistor)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
            where TRequiredOutput : class
        {
            CallFor<TRequiredInput1, TRequiredInput2, TRequiredOutput>(rule, selector1, selector2, (s1, s2) => true, persistor);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation from S to T is made
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the source transformation rule</typeparam>
        /// <param name="selector1">A method that selects the frist input for this transformation rule</param>
        /// <param name="selector2">A method that selects the second input for this transformation rule</param>
        /// <param name="persistor">A method that persists the output of this rule back to the source instance</param>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        public void CallFor<TRequiredInput, TRequiredOutput>(TransformationRuleBase<TRequiredInput, TRequiredOutput> rule, Func<TRequiredInput, TIn1> selector1, Func<TRequiredInput, TIn2> selector2, Action<TOut, TRequiredOutput> persistor)
            where TRequiredInput : class
            where TRequiredOutput : class
        {
            CallFor<TRequiredInput, TRequiredOutput>(rule, selector1, selector2, s => true, persistor);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation from S to T is made
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the source transformation rule</typeparam>
        /// <param name="selector1">A method that selects the first input for this transformation rule</param>
        /// <param name="selector2">A method that selects the second input for this transformation rule</param>
        /// <param name="persistor">A method that persists the output of this rule back to the source instance</param>
        /// <param name="filter">A method that filters the applicable instances of S</param>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public void CallFor<TRequiredInput1, TRequiredInput2, TRequiredOutput>(TransformationRuleBase<TRequiredInput1, TRequiredInput2, TRequiredOutput> rule, Func<TRequiredInput1, TRequiredInput2, TIn1> selector1, Func<TRequiredInput1, TRequiredInput2, TIn2> selector2, Func<TRequiredInput1, TRequiredInput2, bool> filter, Action<TOut, TRequiredOutput> persistor)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
            where TRequiredOutput : class
        {
            if (persistor == null) persistor = (p1, p2) => { };
            if (filter == null) throw new ArgumentNullException("filter");
            if (selector1 == null) throw new ArgumentNullException("selector1");
            if (selector2 == null) throw new ArgumentNullException("selector2");
            CallForInternal(rule, c => CallFilter(filter, c), o => CallDependencySelect(selector1, selector2, o), (s, t) => persistor((TOut)s, (TRequiredOutput)t), false);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation from S to T is made
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the source transformation rule</typeparam>
        /// <param name="selector1">A method that selects the first input for this transformation rule</param>
        /// <param name="selector2">A method that selects the second input for this transformation rule</param>
        /// <param name="persistor">A method that persists the output of this rule back to the source instance</param>
        /// <param name="filter">A method that filters the applicable instances of S</param>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public void CallFor<TRequiredInput, TRequiredOutput>(TransformationRuleBase<TRequiredInput, TRequiredOutput> rule, Func<TRequiredInput, TIn1> selector1, Func<TRequiredInput, TIn2> selector2, Func<TRequiredInput, bool> filter, Action<TOut, TRequiredOutput> persistor)
            where TRequiredInput : class
            where TRequiredOutput : class
        {
            if (persistor == null) persistor = (p1, p2) => { };
            if (filter == null) throw new ArgumentNullException("filter");
            if (selector1 == null) throw new ArgumentNullException("selector1");
            if (selector2 == null) throw new ArgumentNullException("selector2");
            CallForInternal(rule, c => CallFilter(filter, c), o => CallDependencySelect(selector1, selector2, o), (s, t) => persistor((TOut)s, (TRequiredOutput)t), false);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation from S to T is made
        /// This version calls this transformation rule for a whol collection of input objects
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the source transformation rule</typeparam>
        /// <param name="selector">A method that selects the inputs for this transformation rule</param>
        /// <param name="persistor">A method that persists the outputs of this rule back to the source instance</param>
        
        public void CallForEach<TRequiredInput1, TRequiredInput2, TRequiredOutput>(Func<TRequiredInput1, TRequiredInput2, IEnumerable<Tuple<TIn1, TIn2>>> selector, Action<TRequiredOutput, IEnumerable<TOut>> persistor)
        {
            CallForEach<TRequiredInput1, TRequiredInput2, TRequiredOutput>(selector, (s1, s2) => true, persistor);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation from S to T is made
        /// This version calls this transformation rule for a whol collection of input objects
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the source transformation rule</typeparam>
        /// <param name="selector">A method that selects the inputs for this transformation rule</param>
        /// <param name="persistor">A method that persists the outputs of this rule back to the source instance</param>
        
        public void CallForEach<TRequiredInput, TRequiredOutput>(Func<TRequiredInput, IEnumerable<Tuple<TIn1, TIn2>>> selector, Action<TRequiredOutput, IEnumerable<TOut>> persistor)
        {
            CallForEach<TRequiredInput, TRequiredOutput>(selector, s => true, persistor);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation from S to T is made
        /// This version calls this transformation rule for a whol collection of input objects
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the source transformation rule</typeparam>
        /// <param name="selector">A method that selects the input for this transformation rule</param>
        /// <param name="persistor">A method that persists the output of this rule back to the source instance</param>
        /// <param name="filter">A method that filters the applicable instances of S</param>
        public void CallForEach<TRequiredInput1, TRequiredInput2, TRequiredOutput>(Func<TRequiredInput1, TRequiredInput2, IEnumerable<Tuple<TIn1, TIn2>>> selector, Func<TRequiredInput1, TRequiredInput2, bool> filter, Action<TRequiredOutput, IEnumerable<TOut>> persistor)
        {
            if (persistor == null) persistor = (p1, p2) => { };
            if (filter == null) throw new ArgumentNullException("filter");
            if (selector == null) throw new ArgumentNullException("selector");
            CallForEachInternal(new Type[] { typeof(TRequiredInput1), typeof(TRequiredInput2) }, typeof(TRequiredOutput), c => CallFilter(filter, c), o => CallDependencySelectMany(selector, o), (s, t) => persistor((TRequiredOutput)s, (IEnumerable<TOut>)t), false);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation from S to T is made
        /// This version calls this transformation rule for a whol collection of input objects
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the source transformation rule</typeparam>
        /// <param name="selector">A method that selects the input for this transformation rule</param>
        /// <param name="persistor">A method that persists the output of this rule back to the source instance</param>
        /// <param name="filter">A method that filters the applicable instances of S</param>
        
        public void CallForEach<TRequiredInput, TRequiredOutput>(Func<TRequiredInput, IEnumerable<Tuple<TIn1, TIn2>>> selector, Func<TRequiredInput, bool> filter, Action<TRequiredOutput, IEnumerable<TOut>> persistor)
        {
            if (persistor == null) persistor = (p1, p2) => { };
            if (filter == null) throw new ArgumentNullException("filter");
            if (selector == null) throw new ArgumentNullException("selector");
            CallForEachInternal(new Type[] { typeof(TRequiredInput) }, typeof(TRequiredOutput), c => CallFilter(filter, c), o => CallDependencySelectMany(selector, o), (s, t) => persistor((TRequiredOutput)s, (IEnumerable<TOut>)t), false);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation from S to T is made
        /// This version calls this transformation rule for a whol collection of input objects
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the source transformation rule</typeparam>
        /// <param name="selector">A method that selects the inputs for this transformation rule</param>
        /// <param name="persistor">A method that persists the outputs of this rule back to the source instance</param>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        
        public void CallForEach<TRequiredInput1, TRequiredInput2, TRequiredOutput>(TransformationRuleBase<TRequiredInput1, TRequiredInput2, TRequiredOutput> rule, Func<TRequiredInput1, TRequiredInput2, IEnumerable<Tuple<TIn1, TIn2>>> selector, Action<TRequiredOutput, IEnumerable<TOut>> persistor)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
            where TRequiredOutput : class
        {
            CallForEach<TRequiredInput1, TRequiredInput2, TRequiredOutput>(rule, selector, (s1, s2) => true, persistor);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation from S to T is made
        /// This version calls this transformation rule for a whol collection of input objects
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the source transformation rule</typeparam>
        /// <param name="selector">A method that selects the inputs for this transformation rule</param>
        /// <param name="persistor">A method that persists the outputs of this rule back to the source instance</param>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        
        public void CallForEach<TRequiredInput, TRequiredOutput>(TransformationRuleBase<TRequiredInput, TRequiredOutput> rule, Func<TRequiredInput, IEnumerable<Tuple<TIn1, TIn2>>> selector, Action<TRequiredOutput, IEnumerable<TOut>> persistor)
            where TRequiredInput : class
            where TRequiredOutput : class
        {
            CallForEach<TRequiredInput, TRequiredOutput>(rule, selector, s => true, persistor);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation from S to T is made
        /// This version calls this transformation rule for a whol collection of input objects
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the source transformation rule</typeparam>
        /// <param name="selector">A method that selects the input for this transformation rule</param>
        /// <param name="persistor">A method that persists the output of this rule back to the source instance</param>
        /// <param name="filter">A method that filters the applicable instances of S</param>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public void CallForEach<TRequiredInput1, TRequiredInput2, TRequiredOutput>(TransformationRuleBase<TRequiredInput1, TRequiredInput2, TRequiredOutput> rule, Func<TRequiredInput1, TRequiredInput2, IEnumerable<Tuple<TIn1, TIn2>>> selector, Func<TRequiredInput1, TRequiredInput2, bool> filter, Action<TRequiredOutput, IEnumerable<TOut>> persistor)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
            where TRequiredOutput : class
        {
            if (persistor == null) persistor = (p1, p2) => { };
            if (filter == null) throw new ArgumentNullException("filter");
            if (selector == null) throw new ArgumentNullException("selector");
            CallForEachInternal(rule, c => CallFilter(filter, c), o => CallDependencySelectMany(selector, o), (s, t) => persistor((TRequiredOutput)s, (IEnumerable<TOut>)t), false);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation from S to T is made
        /// This version calls this transformation rule for a whol collection of input objects
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the source transformation rule</typeparam>
        /// <param name="selector">A method that selects the input for this transformation rule</param>
        /// <param name="persistor">A method that persists the output of this rule back to the source instance</param>
        /// <param name="filter">A method that filters the applicable instances of S</param>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public void CallForEach<TRequiredInput, TRequiredOutput>(TransformationRuleBase<TRequiredInput, TRequiredOutput> rule, Func<TRequiredInput, IEnumerable<Tuple<TIn1, TIn2>>> selector, Func<TRequiredInput, bool> filter, Action<TRequiredOutput, IEnumerable<TOut>> persistor)
            where TRequiredInput : class
            where TRequiredOutput : class
        {
            if (persistor == null) persistor = (p1, p2) => { };
            if (filter == null) throw new ArgumentNullException("filter");
            if (selector == null) throw new ArgumentNullException("selector");
            CallForEachInternal(rule, c => CallFilter(filter, c), o => CallDependencySelectMany(selector, o), (s, t) => persistor((TRequiredOutput)s, (IEnumerable<TOut>)t), false);
        }


        /// <summary>
        /// Calls the transformation rule with the given type after the current transformation rule, but no earlier than it created its output
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument for the called transformation</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument for the called transformation</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        /// <param name="selector1">A method that selects the first input for the dependent transformation rule</param>
        /// <param name="selector2">A method that selects the second input for the dependent transformation rule</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency CallOutputSensitive<TRequiredInput1, TRequiredInput2>(GeneralTransformationRule<TRequiredInput1, TRequiredInput2> rule, Func<TIn1, TIn2, TOut, TRequiredInput1> selector1, Func<TIn1, TIn2, TOut, TRequiredInput2> selector2)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
        {
            return CallOutputSensitive(rule, selector1, selector2, null);
        }


        /// <summary>
        /// Calls the transformation rule with the given type after the current transformation rule, but no earlier than it created its output
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument for the called transformation</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument for the called transformation</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        /// <param name="selector1">A method that selects the first input for the dependent transformation rule</param>
        /// <param name="selector2">A method that selects the second input for the dependent transformation rule</param>
        /// <param name="filter">A method that filters the cases where the dependency should fire</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency CallOutputSensitive<TRequiredInput1, TRequiredInput2>(GeneralTransformationRule<TRequiredInput1, TRequiredInput2> rule, Func<TIn1, TIn2, TOut, TRequiredInput1> selector1, Func<TIn1, TIn2, TOut, TRequiredInput2> selector2, Func<TIn1, TIn2, TOut, bool> filter)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
        {
            if (selector1 == null) throw new ArgumentNullException("selector1");
            if (selector2 == null) throw new ArgumentNullException("selector2");
            return Depend(filter != null ?
                new Predicate<Computation>(c => filter(c.GetInput(0) as TIn1, c.GetInput(1) as TIn2, c.Output as TOut))
                : null,
                c => DependencySelect(selector1, selector2, c), rule, null, false, true);
        }


        /// <summary>
        /// Calls the transformation rule with the given type after the current transformation rule, but no earlier than it created its output
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument for the called transformation</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        /// <param name="selector">A method that selects the input for the dependent transformation rule</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency CallOutputSensitive<TRequiredInput>(GeneralTransformationRule<TRequiredInput> rule, Func<TIn1, TIn2, TOut, TRequiredInput> selector)
            where TRequiredInput : class
        {
            return CallOutputSensitive(rule, selector, null);
        }


        /// <summary>
        /// Calls the transformation rule with the given type after the current transformation rule, but no earlier than it created its output
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument for the called transformation</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        /// <param name="selector">A method that selects the input for the dependent transformation rule</param>
        /// <param name="filter">A method that filters the cases where the dependency should fire</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency CallOutputSensitive<TRequiredInput>(GeneralTransformationRule<TRequiredInput> rule, Func<TIn1, TIn2, TOut, TRequiredInput> selector, Func<TIn1, TIn2, TOut, bool> filter)
            where TRequiredInput : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            return Depend(filter != null ?
                new Predicate<Computation>(c => filter(c.GetInput(0) as TIn1, c.GetInput(1) as TIn2, c.Output as TOut))
                : null,
                c => DependencySelect(selector, c), rule, null, false, true);
        }

        /// <summary>
        /// Calls the transformation rule with the given type after the current transformation rule for all inputs, but no earlier than it created its output
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument for the called transformation</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument for the called transformation</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        /// <param name="selector">A method that selects the inputs for the dependent transformation rule</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency CallManyOutputSensitive<TRequiredInput1, TRequiredInput2>(GeneralTransformationRule<TRequiredInput1, TRequiredInput2> rule, Func<TIn1, TIn2, TOut, IEnumerable<Tuple<TRequiredInput1, TRequiredInput2>>> selector)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
        {
            return CallManyOutputSensitive(rule, selector, null);
        }

        /// <summary>
        /// Calls the transformation rule with the given type after the current transformation rule for all inputs, but no earlier than it created its output
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument for the called transformation</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument for the called transformation</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        /// <param name="selector">A method that selects the inputs for the dependent transformation rule</param>
        /// <param name="filter">A method that filters the cases where the dependency should fire</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency CallManyOutputSensitive<TRequiredInput1, TRequiredInput2>(GeneralTransformationRule<TRequiredInput1, TRequiredInput2> rule, Func<TIn1, TIn2, TOut, IEnumerable<Tuple<TRequiredInput1, TRequiredInput2>>> selector, Func<TIn1, TIn2, TOut, bool> filter)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            return DependMany(filter != null ?
                new Predicate<Computation>(c => filter(c.GetInput(0) as TIn1, c.GetInput(1) as TIn2, c.Output as TOut))
                : null,
                c => DependencySelectMany(selector, c), rule, null, false, true);
        }

        /// <summary>
        /// Calls the transformation rule with the given type after the current transformation rule for all inputs, but no earlier than it created its output
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument for the called transformation</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        /// <param name="selector">A method that selects the inputs for the dependent transformation rule</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency CallManyOutputSensitive<TRequiredInput>(GeneralTransformationRule<TRequiredInput> rule, Func<TIn1, TIn2, TOut, IEnumerable<TRequiredInput>> selector)
            where TRequiredInput : class
        {
            return CallManyOutputSensitive(rule, selector, null);
        }

        /// <summary>
        /// Calls the transformation rule with the given type after the current transformation rule for all inputs, but no earlier than it created its output
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument for the called transformation</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        /// <param name="selector">A method that selects the inputs for the dependent transformation rule</param>
        /// <param name="filter">A method that filters the cases where the dependency should fire</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency CallManyOutputSensitive<TRequiredInput>(GeneralTransformationRule<TRequiredInput> rule, Func<TIn1, TIn2, TOut, IEnumerable<TRequiredInput>> selector, Func<TIn1, TIn2, TOut, bool> filter)
            where TRequiredInput : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            return DependMany(filter != null ?
                new Predicate<Computation>(c => filter(c.GetInput(0) as TIn1, c.GetInput(1) as TIn2, c.Output as TOut))
                : null,
                c => DependencySelectMany(selector, c), rule, null, false, true);
        }


        /// <summary>
        /// Creates a trace entry for every computation with the key specified by the given selector method
        /// </summary>
        /// <typeparam name="TKey">The type of the trace entry key</typeparam>
        /// <param name="traceSelector">A method that selects for an input the appropriate key that should be added to the trace</param>
        /// <returns>A transformation rule that can be used as group or for direct tracing purposes</returns>
        public TraceEntryGroup<TKey, TOut> TraceOutput<TKey>(Func<TIn1, TIn2, TOut, TKey> traceSelector)
            where TKey : class
        {
            var traceRule = new TraceEntryGroup<TKey, TOut>();
            TraceOutput<TKey>(traceRule, traceSelector);
            return traceRule;
        }

        /// <summary>
        /// Creates a trace entry for every computation with the key specified by the given selector method
        /// </summary>
        /// <typeparam name="TKey">The type of the trace entry key</typeparam>
        /// <param name="traceKey">The transformation rule that is used as group for direct tracing purposes</param>
        /// <param name="traceSelector">A method that selects for an input the appropriate key that should be added to the trace</param>
        public ITransformationRuleDependency TraceOutput<TKey>(TraceEntryGroup<TKey, TOut> traceKey, Func<TIn1, TIn2, TOut, TKey> traceSelector)
            where TKey : class
        {
            if (traceSelector == null) throw new ArgumentNullException("traceSelector");
            if (traceKey == null) throw new ArgumentNullException("traceKey");

            var traceDependency = new TraceDependency<TIn1, TIn2, TOut, TKey, TOut>()
            {
                InputSelector = traceSelector,
                TraceKey = traceKey,
                OutputSelector = (i1, i2, o) => o
            };
            Dependencies.Add(traceDependency);

            return traceDependency;
        }


        internal static object[] DependencySelect<TRequiredInput1, TRequiredInput2>(Func<TIn1, TIn2, TOut, TRequiredInput1> selector1, Func<TIn1, TIn2, TOut, TRequiredInput2> selector2, Computation computation)
        {
            if (computation != null && computation.InputArguments == 2)
            {
                var t1 = (TIn1)computation.GetInput(0);
                var t2 = (TIn2)computation.GetInput(1);
                var t3 = (TOut)computation.Output;
                return new object[] {selector1(t1, t2, t3),
                        selector2(t1, t2, t3)};
            }
            else
            {
                return null;
            }
        }

        internal static object[] DependencySelect<TRequiredInput>(Func<TIn1, TIn2, TOut, TRequiredInput> selector, Computation computation)
        {
            if (computation != null && computation.InputArguments == 2)
            {
                var t1 = (TIn1)computation.GetInput(0);
                var t2 = (TIn2)computation.GetInput(1);
                var t3 = (TOut)computation.Output;
                return new object[] { selector(t1, t2, t3) };
            }
            else
            {
                return null;
            }
        }

        
        internal static IEnumerable<object[]> DependencySelectMany<TRequiredInput1, TRequiredInput2>(Func<TIn1, TIn2, TOut, IEnumerable<Tuple<TRequiredInput1, TRequiredInput2>>> selector, Computation computation)
        {
            if (computation != null && computation.InputArguments == 2)
            {
                var t1 = (TIn1)computation.GetInput(0);
                var t2 = (TIn2)computation.GetInput(1);
                var tc = selector(t1, t2, (TOut)computation.Output);
                if (tc != null) return tc.Select(tuple => new object[] { tuple.Item1, tuple.Item2 });
            }
            return null;
        }

        
        internal static IEnumerable<object[]> DependencySelectMany<TRequiredInput>(Func<TIn1, TIn2, TOut, IEnumerable<TRequiredInput>> selector, Computation computation)
        {
            if (computation != null && computation.InputArguments == 2)
            {
                var t1 = (TIn1)computation.GetInput(0);
                var t2 = (TIn2)computation.GetInput(1);
                var tc = selector(t1, t2, (TOut)computation.Output);
                if (tc != null) return tc.Select(tuple => new object[] { tuple });
            }
            return null;
        }

        private void CallForInternal(Type[] inputTypes, Type output, Predicate<Computation> filter, Func<Computation, object[]> selector, Action<object, object> persistor, bool needOutput)
        {
            foreach (var rule in Transformation.GetRulesForTypeSignature(inputTypes, output))
            {
                CallForInternal(rule, filter, selector, persistor, needOutput);
            }
        }

        private void CallForEachInternal(Type[] inputTypes, Type output, Predicate<Computation> filter, Func<Computation, IEnumerable<object[]>> selector, Action<object, IEnumerable> persistor, bool needOutput)
        {
            foreach (var rule in Transformation.GetRulesForTypeSignature(inputTypes, output))
            {
                CallForEachInternal(rule, filter, selector, persistor, needOutput);
            }
        }
    }
}
