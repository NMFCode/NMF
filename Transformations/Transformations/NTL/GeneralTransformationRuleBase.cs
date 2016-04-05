using NMF.Transformations.Core;
using NMF.Transformations.Properties;
using NMF.Utilities;
using NMF.Expressions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NMF.Transformations
{
    /// <summary>
    /// Defines a transformation rule of a transformation that has one input argument
    /// </summary>
    /// <typeparam name="TIn">The type of the input argument</typeparam>
    public abstract class GeneralTransformationRule<TIn> : GeneralTransformationRule
        where TIn : class
    {
        /// <summary>
        /// Marks the current transformation rule instantiating for the specified rule
        /// </summary>
        /// <param name="filter">The filter that should filter the inputs where this transformation rule is marked instantiating</param>
        /// <param name="rule">The transformation rule</param>
        public void MarkInstantiatingFor(GeneralTransformationRule rule, Predicate<TIn> filter)
        {
            if (rule == null) throw new ArgumentNullException("rule");
            if (rule.InputType.IsAssignableArrayFrom(InputType) && (rule.OutputType == OutputType || rule.OutputType.IsAssignableFrom(OutputType)))
            {
                Require(rule);
                if (filter != null)
                {
                    MarkInstantiatingFor(rule, c => filter(c.GetInput(0) as TIn));
                }
                else
                {
                    MarkInstantiatingFor(rule);
                }
            }
            else
            {
                throw new InvalidOperationException(Resources.ErrMarkInstantiatingMustInherit);
            }
        }


        /// <summary>
        /// Gets the first rule within the rules associated transformation that has the given rule type
        /// </summary>
        /// <typeparam name="TRule">The type of the transformation rule that is looked for</typeparam>
        /// <returns>The first transformation rule within the associated transformation or null, if there is none.</returns>
        public TRule Rule<TRule>()
            where TRule : GeneralTransformationRule
        {
            return Transformation.GetRuleForRuleType(typeof(TRule)) as TRule;
        }
        
        /// <summary>
        /// Registers the given pattern for the current transformation rule
        /// </summary>
        /// <param name="pattern">The pattern that should be applied based on the current transformation rule</param>
        public void WithPattern(ITransformationRulePattern<TIn> pattern)
        {
            if (pattern == null) throw new ArgumentNullException("pattern");

            pattern.TargetRule = this;

            Transformation.Patterns.Add(pattern);
        }

        /// <summary>
        /// Registers the given incremental pattern for the current transformation rule
        /// </summary>
        /// <param name="patternCreator">The relation pattern that should be applied on the current transformation rule</param>
        public void WithPattern(Func<ITransformationContext, INotifyEnumerable<TIn>> patternCreator)
        {
            if (patternCreator == null) throw new ArgumentNullException("patternCreator");

            Transformation.Patterns.Add(new Linq.IncrementalPattern<TIn>(patternCreator) { TargetRule = this });
        }

        /// <summary>
        /// Registers the given static pattern for the current transformation rule
        /// </summary>
        /// <param name="patternCreator">The relation pattern that should be applied on the current transformation rule</param>
        public void WithPattern(Func<ITransformationContext, IEnumerable<TIn>> patternCreator)
        {
            if (patternCreator == null) throw new ArgumentNullException("patternCreator");

            Transformation.Patterns.Add(new Linq.StaticPattern<TIn>(patternCreator) { TargetRule = this });
        }

        /// <summary>
        /// Requires all transformation rules that transform items with the input type
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the dependent transformations</typeparam>
        /// <remarks>This version Always takes the input parameter as input for the dependent transformations. Thus, this method will throw an exception, if the types do not match</remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void RequireByType<TRequiredInput>()
            where TRequiredInput : class
        {
            if (typeof(TRequiredInput).IsAssignableFrom(typeof(TIn)))
            {
                RequireByType<TRequiredInput>(t => t as TRequiredInput);
            }
            else
            {
                throw new InvalidOperationException(Resources.ErrRequires1ArgNoSelectorMustInherit);
            }
        }

        /// <summary>
        /// Requires all transformation rules that transform items with the input type
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the dependent transformations</typeparam>
        /// <param name="selector">A method that selects the input for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void RequireByType<TRequiredInput>(Func<TIn, TRequiredInput> selector)
            where TRequiredInput : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            foreach (var rule in Transformation.GetRulesForInputTypes(new Type[] { typeof(TRequiredInput) }))
            {
                Depend(null, c => CreateInputArray(selector, c), rule, null, true, false);
            }
        }

        /// <summary>
        /// Requires all transformation rules that transform items with the input type
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformations</typeparam>
        /// <param name="selector1">A method that selects the first input for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector1 parameter is passed a null instance</exception>
        /// <param name="selector2">A method that selects the second input for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector2 parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void RequireByType<TRequiredInput1, TRequiredInput2>(Func<TIn, TRequiredInput1> selector1, Func<TIn, TRequiredInput2> selector2)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
        {
            if (selector1 == null) throw new ArgumentNullException("selector1");
            if (selector2 == null) throw new ArgumentNullException("selector2");
            foreach (var rule in Transformation.GetRulesForInputTypes(new Type[] { typeof(TRequiredInput1), typeof(TRequiredInput2) }))
            {
                Depend(null, c => new object[] { selector1((TIn)c.GetInput(0)), selector2((TIn)c.GetInput(0)) }, rule, null, true, false);
            }
        }

        /// <summary>
        /// Requires the transformation rule with the given type
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the dependent transformation</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">This exception is thrown if the rule parameter is passed a null instance.</exception>
        /// <param name="selector">A method that selects the input for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency Require<TRequiredInput>(GeneralTransformationRule<TRequiredInput> rule, Func<TIn, TRequiredInput> selector)
            where TRequiredInput : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            return Depend(null, c => CreateInputArray<TRequiredInput>(selector, c), rule, null, true, false);
        }

        private static object[] CreateInputArray<TRequiredInput>(Func<TIn, TRequiredInput> selector, Computation c) where TRequiredInput : class
        {
            var item = selector((TIn)c.GetInput(0));
            if (item == null) return null;
            return new object[] { item };
        }

        /// <summary>
        /// Requires the transformation rule with the given type
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformations</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">This exception is thrown if the rule parameter is passed a null instance.</exception>
        /// <param name="selector1">A method that selects the first input for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector1 parameter is passed a null instance</exception>
        /// <param name="selector2">A method that selects the second input for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector2 parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency Require<TRequiredInput1, TRequiredInput2>(GeneralTransformationRule<TRequiredInput1, TRequiredInput2> rule, Func<TIn, TRequiredInput1> selector1, Func<TIn, TRequiredInput2> selector2)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
        {
            if (selector1 == null) throw new ArgumentNullException("selector1");
            if (selector2 == null) throw new ArgumentNullException("selector2");
            return Depend(null, c => new object[] { selector1((TIn)c.GetInput(0)), selector2((TIn)c.GetInput(0)) }, rule, null, true, false);
        }

        /// <summary>
        /// Requires all transformation rules that transform S to T with all of the specified objects
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the dependent transformations</typeparam>
        /// <param name="selector">A method that selects the inputs for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void RequireManyByType<TRequiredInput>(Func<TIn, IEnumerable<TRequiredInput>> selector)
            where TRequiredInput : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            foreach (var rule in Transformation.GetRulesForInputTypes(new Type[] { typeof(TRequiredInput) }))
            {
                DependMany(null, c => SelectArrays(selector, c), rule, null, true, false);
            }
        }

        /// <summary>
        /// Requires all transformation rules that transform S to T with all of the specified objects
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformations</typeparam>
        /// <param name="selector">A method that selects the inputs for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void RequireManyByType<TRequiredInput1, TRequiredInput2>(Func<TIn, IEnumerable<Tuple<TRequiredInput1, TRequiredInput2>>> selector)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            foreach (var rule in Transformation.GetRulesForInputTypes(new Type[] { typeof(TRequiredInput1), typeof(TRequiredInput2) }))
            {
                DependMany(null, c => SelectArrays(selector, c), rule, null, true, false);
            }
        }

        /// <summary>
        /// Requires the transformation rule with the given type
        /// </summary>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">This exception is thrown if the rule parameter is passed a null instance.</exception>
        /// <param name="selector">A method that returns the input of the required transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        public ITransformationRuleDependency Require(GeneralTransformationRule rule, Func<TIn, object[]> selector)
        {
            return Depend(null, c => selector(c.GetInput(0) as TIn), rule, null, true, false);
        }



        /// <summary>
        /// Requires the transformation rule with the given type
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the dependent transformation</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">This exception is thrown if the rule parameter is passed a null instance.</exception>
        /// <param name="filter">A filter that filters the input arguments that need the specified requirement</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency Require<TRequiredInput>(GeneralTransformationRule<TRequiredInput> rule, Predicate<TIn> filter)
            where TRequiredInput : class
        {
            if (!typeof(TRequiredInput).IsAssignableFrom(typeof(TIn))) throw new InvalidOperationException(Resources.ErrCall1ArgNoSelectorMustInherit);
            return Depend(filter != null ? new Predicate<Computation>(c => filter(c.GetInput(0) as TIn)) : null, c => c.CreateInputArray(), rule, null, true, false);
        }


        /// <summary>
        /// Requires the transformation rule with the given type
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the dependent transformation</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">This exception is thrown if the rule parameter is passed a null instance.</exception>
        /// <param name="filter">A filter that filters the input arguments that need the specified requirement</param>
        /// <param name="selector">A method that selects the input for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency Require<TRequiredInput>(GeneralTransformationRule<TRequiredInput> rule, Func<TIn, TRequiredInput> selector, Predicate<TIn> filter)
            where TRequiredInput : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            if (filter == null) filter = s => true;
            return Depend(c => filter(c.GetInput(0) as TIn), c => CreateInputArray(selector, c), rule, null, true, false);
        }


        /// <summary>
        /// Requires the transformation rule with the given type
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformations</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">This exception is thrown if the rule parameter is passed a null instance.</exception>
        /// <param name="filter">A filter that filters the input arguments that need the specified requirement</param>
        /// <param name="selector1">A method that selects the first input for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector1 parameter is passed a null instance</exception>
        /// <param name="selector2">A method that selects the second input for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector2 parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency Require<TRequiredInput1, TRequiredInput2>(GeneralTransformationRule<TRequiredInput1, TRequiredInput2> rule, Func<TIn, TRequiredInput1> selector1, Func<TIn, TRequiredInput2> selector2, Predicate<TIn> filter)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
        {
            if (selector1 == null) throw new ArgumentNullException("selector1");
            if (selector2 == null) throw new ArgumentNullException("selector2");
            if (filter == null) filter = s => true;
            return Depend(c => filter(c.GetInput(0) as TIn), c => new object[] { selector1((TIn)c.GetInput(0)), selector2((TIn)c.GetInput(0)) }, rule, null, true, false);
        }


        /// <summary>
        /// Requires all transformation rules that transform S to T with all of the specified objects
        /// </summary>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">This exception is thrown if the rule parameter is passed a null instance.</exception>
        /// <param name="selector">A method that selects the input arguments for the dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        public ITransformationRuleDependency RequireMany(GeneralTransformationRule rule, Func<TIn, IEnumerable<object[]>> selector)
        {
            return DependMany(null, c => selector(c.GetInput(0) as TIn), rule, null, true, false);
        }

        /// <summary>
        /// Requires the transformation rule with the given type with all of the specified inputs
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the dependent transformation</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">This exception is thrown if the rule parameter is passed a null instance.</exception>
        /// <param name="selector">A method that selects the inputs for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency RequireMany<TRequiredInput>(GeneralTransformationRule<TRequiredInput> rule, Func<TIn, IEnumerable<TRequiredInput>> selector)
            where TRequiredInput : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            return DependMany(null, c =>
            {
                return SelectArrays(selector, c);
            }, rule, null, true, false);
        }

        /// <summary>
        /// Requires the transformation rule with the given type with all of the specified inputs
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformations</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">This exception is thrown if the rule parameter is passed a null instance.</exception>
        /// <param name="selector">A method that selects the inputs for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency RequireMany<TRequiredInput1, TRequiredInput2>(GeneralTransformationRule<TRequiredInput1, TRequiredInput2> rule, Func<TIn, IEnumerable<Tuple<TRequiredInput1, TRequiredInput2>>> selector)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            return DependMany(null, c =>
            {
                return SelectArraysT2(selector, c);
            }, rule, null, true, false);
        }

        /// <summary>
        /// Calls the transformation rule with the given type after the current transformation rule
        /// </summary>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">This exception is thrown if the rule parameter is passed a null instance.</exception>
        /// <param name="selector">A method that selects the input for the dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        public ITransformationRuleDependency Call(GeneralTransformationRule rule, Func<TIn, object[]> selector)
        {
            return Depend(null, c => selector(c.GetInput(0) as TIn), rule, null, false, false);
        }


        /// <summary>
        /// Calls the transformation rule with the given type with all of the specified inputs after the current transformation rule
        /// </summary>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">This exception is thrown if the rule parameter is passed a null instance.</exception>
        /// <param name="selector">A method that selects the input for the dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        public ITransformationRuleDependency CallMany(GeneralTransformationRule rule, Func<TIn, IEnumerable<object[]>> selector)
        {
            if (selector == null) throw new ArgumentNullException("selector");
            return DependMany(null, c => selector(c.GetInput(0) as TIn), rule, null, false, false);
        }

        /// <summary>
        /// Calls all transformation rules that transform items with the input type after the current transformation rule
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the dependent transformations</typeparam>
        /// <param name="selector">A method that selects the input for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        public void CallByType<TRequiredInput>(Func<TIn, TRequiredInput> selector)
            where TRequiredInput : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            foreach (var rule in Transformation.GetRulesForInputTypes(new Type[] { typeof(TRequiredInput) }))
            {
                Depend(null, c => CreateInputArray(selector, c), rule, null, false, false);
            }
        }

        /// <summary>
        /// Calls all transformation rules that transform items with the input type after the current transformation rule
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformations</typeparam>
        /// <param name="selector1">A method that selects the first input for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector1 parameter is passed a null instance</exception>
        /// <param name="selector2">A method that selects the second input for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector2 parameter is passed a null instance</exception>
        public void CallByType<TRequiredInput1, TRequiredInput2>(Func<TIn, TRequiredInput1> selector1, Func<TIn, TRequiredInput2> selector2)
        {
            if (selector1 == null) throw new ArgumentNullException("selector1");
            if (selector2 == null) throw new ArgumentNullException("selector2");
            foreach (var rule in Transformation.GetRulesForInputTypes(new Type[] { typeof(TRequiredInput1), typeof(TRequiredInput2) }))
            {
                Depend(null, c => new object[] { selector1((TIn)c.GetInput(0)), selector2((TIn)c.GetInput(0)) }, rule, null, false, false);
            }
        }

        /// <summary>
        /// Calls the transformation rule with the given type after the current transformation rule
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the dependent transformation</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">This exception is thrown if the rule parameter is passed a null instance.</exception>
        /// <param name="selector">A method that selects the input for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency Call<TRequiredInput>(GeneralTransformationRule<TRequiredInput> rule, Func<TIn, TRequiredInput> selector)
            where TRequiredInput : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            return Depend(null, c => CreateInputArray(selector, c), rule, null, false, false);
        }

        /// <summary>
        /// Calls the transformation rule with the given type after the current transformation rule
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformations</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">This exception is thrown if the rule parameter is passed a null instance.</exception>
        /// <param name="selector1">A method that selects the first input for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector1 parameter is passed a null instance</exception>
        /// <param name="selector2">A method that selects the second input for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector2 parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency Call<TRequiredInput1, TRequiredInput2>(GeneralTransformationRule<TRequiredInput1, TRequiredInput2> rule, Func<TIn, TRequiredInput1> selector1, Func<TIn, TRequiredInput2> selector2)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
        {
            if (selector1 == null) throw new ArgumentNullException("selector1");
            if (selector2 == null) throw new ArgumentNullException("selector2");
            return Depend(null, c => new object[] { selector1((TIn)c.GetInput(0)), selector2((TIn)c.GetInput(0)) }, rule, null, false, false);
        }

        /// <summary>
        /// Calls the transformation rule with the given type after the current transformation rule
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the dependent transformation</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">This exception is thrown if the rule parameter is passed a null instance.</exception>
        /// <param name="filter">A filter that filters the input arguments that need the specified requirement</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency Call<TRequiredInput>(GeneralTransformationRule<TRequiredInput> rule, Predicate<TIn> filter)
            where TRequiredInput : class
        {
            if (!typeof(TRequiredInput).IsAssignableFrom(typeof(TIn))) throw new InvalidOperationException(Resources.ErrCall1ArgNoSelectorMustInherit);
            return Depend(filter != null ? new Predicate<Computation>(c => filter(c.GetInput(0) as TIn)) : null, c => c.CreateInputArray(), rule, null, false, false);
        }


        /// <summary>
        /// Calls the transformation rule with the given type after the current transformation rule
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the dependent transformation</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">This exception is thrown if the rule parameter is passed a null instance.</exception>
        /// <param name="filter">A filter that filters the input arguments that need the specified requirement</param>
        /// <param name="selector">A method that selects the input for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency Call<TRequiredInput>(GeneralTransformationRule<TRequiredInput> rule, Func<TIn, TRequiredInput> selector, Predicate<TIn> filter)
            where TRequiredInput : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            if (filter == null) filter = input => true;
            return Depend(c => filter(c.GetInput(0) as TIn), c => CreateInputArray(selector, c), rule, null, false, false);
        }


        /// <summary>
        /// Calls the transformation rule with the given type after the current transformation rule
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformations</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">This exception is thrown if the rule parameter is passed a null instance.</exception>
        /// <param name="filter">A filter that filters the input arguments that need the specified requirement</param>
        /// <param name="selector1">A method that selects the first input for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector1 parameter is passed a null instance</exception>
        /// <param name="selector2">A method that selects the second input for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector2 parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency Call<TRequiredInput1, TRequiredInput2>(GeneralTransformationRule<TRequiredInput1, TRequiredInput2> rule, Func<TIn, TRequiredInput1> selector1, Func<TIn, TRequiredInput2> selector2, Predicate<TIn> filter)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
        {
            if (selector1 == null) throw new ArgumentNullException("selector1");
            if (selector2 == null) throw new ArgumentNullException("selector2");
            if (filter == null) filter = s => true;
            return Depend(c => filter(c.GetInput(0) as TIn), c => new object[] { selector1((TIn)c.GetInput(0)), selector2((TIn)c.GetInput(0)) }, rule, null, false, false);
        }

        /// <summary>
        /// Calls all transformation rules that transform S to T with all of the specified objects after the current transformation rule
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the dependent transformations</typeparam>
        /// <param name="selector">A method that selects the inputs for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void CallManyByType<TRequiredInput>(Func<TIn, IEnumerable<TRequiredInput>> selector)
            where TRequiredInput : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            foreach (var rule in Transformation.GetRulesForInputTypes(new Type[] { typeof(TRequiredInput) }))
            {
                DependMany(null, c => SelectArrays(selector, c), rule, null, false, false);
            }
        }

        /// <summary>
        /// Calls all transformation rules that transform S to T with all of the specified objects after the current transformation rule
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformations</typeparam>
        /// <param name="selector">A method that selects the inputs for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void CallManyByType<TRequiredInput1, TRequiredInput2>(Func<TIn, IEnumerable<Tuple<TRequiredInput1, TRequiredInput2>>> selector)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            foreach (var rule in Transformation.GetRulesForInputTypes(new Type[] { typeof(TRequiredInput1), typeof(TRequiredInput2) }))
            {
                DependMany(null, c => SelectArraysT2(selector, c), rule, null, false, false);
            }
        }

        /// <summary>
        /// Calls the transformation rule with the given type with all of the specified inputs after the current transformation rule
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the dependent transformation</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the rule parameter is passed a null instance</exception>
        /// <param name="selector">A method that selects the inputs for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency CallMany<TRequiredInput>(GeneralTransformationRule<TRequiredInput> rule, Func<TIn, IEnumerable<TRequiredInput>> selector)
            where TRequiredInput : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            return DependMany(null, c =>
            {
                return SelectArrays(selector, c);
            }, rule, null, false, false);
        }

        /// <summary>
        /// Calls the transformation rule with the given type with all of the specified inputs after the current transformation rule
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformations</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">This exception is thrown if the rule parameter is passed a null instance.</exception>
        /// <param name="selector">A method that selects the inputs for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency CallMany<TRequiredInput1, TRequiredInput2>(GeneralTransformationRule<TRequiredInput1, TRequiredInput2> rule, Func<TIn, IEnumerable<Tuple<TRequiredInput1, TRequiredInput2>>> selector)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            return DependMany(null, c =>
            {
                return SelectArrays(selector, c);
            }, rule, null, false, false);
        }

        private static Type[] types = new Type[] { typeof(TIn) };

        /// <summary>
        /// Gets the type signature of the input arguments of this transformation rule
        /// </summary>
        public sealed override Type[] InputType
        {
            get { return types; }
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the input type is made
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the source transformation rule</typeparam>
        /// <param name="selector">A method that selects the input for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void CallFor<TRequiredInput>(Func<TRequiredInput, TIn> selector)
        {
            if (selector == null) throw new ArgumentNullException("selector");
            CallFor<TRequiredInput>(selector, s => true);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the input type is made
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformations</typeparam>
        /// <param name="selector">A method that selects the input for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void CallFor<TRequiredInput1, TRequiredInput2>(Func<TRequiredInput1, TRequiredInput2, TIn> selector)
        {
            if (selector == null) throw new ArgumentNullException("selector");
            CallFor<TRequiredInput1, TRequiredInput2>(selector, (s1,s2) => true);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the input type is made
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the source transformation rule</typeparam>
        /// <param name="selector">A method that selects the input for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">This exception is thrown if the rule parameter is passed a null instance.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void CallFor<TRequiredInput>(GeneralTransformationRule<TRequiredInput> rule, Func<TRequiredInput, TIn> selector)
            where TRequiredInput : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            CallFor<TRequiredInput>(rule, selector, s => true);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the input type is made
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformations</typeparam>
        /// <param name="selector">A method that selects the input for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">This exception is thrown if the rule parameter is passed a null instance.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void CallFor<TRequiredInput1, TRequiredInput2>(GeneralTransformationRule<TRequiredInput1, TRequiredInput2> rule, Func<TRequiredInput1, TRequiredInput2, TIn> selector)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            CallFor<TRequiredInput1, TRequiredInput2>(rule, selector, (s1, s2) => true);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the input type is made
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the source transformation rule</typeparam>
        /// <param name="selector">A method that selects the input for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        /// <param name="filter">A method that filters the applicable instances of S</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void CallFor<TRequiredInput>(Func<TRequiredInput, TIn> selector, Predicate<TRequiredInput> filter)
        {
            if (filter == null) filter = o => true;
            if (selector == null) throw new ArgumentNullException("selector");
            CallForInternal(new Type[] { typeof(TRequiredInput) }, o => filter((TRequiredInput)o.GetInput(0)), c => new object[] { selector((TRequiredInput)c.GetInput(0)) }, null, false);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the input type is made
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformations</typeparam>
        /// <param name="selector">A method that selects the input for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        /// <param name="filter">A method that filters the applicable instances of S</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void CallFor<TRequiredInput1, TRequiredInput2>(Func<TRequiredInput1, TRequiredInput2, TIn> selector, Func<TRequiredInput1, TRequiredInput2, bool> filter)
        {
            if (filter == null) filter = (o1, o2) => true;
            if (selector == null) throw new ArgumentNullException("selector");
            CallForInternal(new Type[] { typeof(TRequiredInput1), typeof(TRequiredInput2) }, o => filter((TRequiredInput1)o.GetInput(0), (TRequiredInput2)o.GetInput(1)), c => new object[] { selector((TRequiredInput1)c.GetInput(0), (TRequiredInput2)c.GetInput(1)) }, null, false);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the input type is made
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the source transformation rule</typeparam>
        /// <param name="selector">A method that selects the input for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        /// <param name="filter">A method that filters the applicable instances of S</param>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">This exception is thrown if the rule parameter is passed a null instance.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public void CallFor<TRequiredInput>(GeneralTransformationRule<TRequiredInput> rule, Func<TRequiredInput, TIn> selector, Predicate<TRequiredInput> filter)
            where TRequiredInput : class
        {
            if (filter == null) filter = o => true;
            if (selector == null) throw new ArgumentNullException("selector");
            CallForInternal(rule, o => filter((TRequiredInput)o.GetInput(0)), c => new object[] { selector((TRequiredInput)c.GetInput(0)) }, null, false);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the input type is made
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformations</typeparam>
        /// <param name="selector">A method that selects the input for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        /// <param name="filter">A method that filters the applicable instances of S</param>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">This exception is thrown if the rule parameter is passed a null instance.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public void CallFor<TRequiredInput1, TRequiredInput2>(GeneralTransformationRule<TRequiredInput1, TRequiredInput2> rule, Func<TRequiredInput1, TRequiredInput2, TIn> selector, Func<TRequiredInput1, TRequiredInput2, bool> filter)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
        {
            if (filter == null) filter = (o1, o2) => true;
            if (selector == null) throw new ArgumentNullException("selector");
            CallForInternal(rule, o => filter((TRequiredInput1)o.GetInput(0), (TRequiredInput2)o.GetInput(1)), c => new object[] { selector((TRequiredInput1)c.GetInput(0), (TRequiredInput2)c.GetInput(1)) }, null, false);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the input type is made
        /// This version calls this transformation rule for a whole collection of input objects
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the source transformation rule</typeparam>
        /// <param name="selector">A method that selects the inputs for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public void CallForEach<TRequiredInput>(Func<TRequiredInput, IEnumerable<TIn>> selector)
        {
            if (selector == null) throw new ArgumentNullException("selector");
            CallForEach<TRequiredInput>(selector, s => true);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the input type is made
        /// This version calls this transformation rule for a whol collection of input objects
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformations</typeparam>
        /// <param name="selector">A method that selects the inputs for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public void CallForEach<TRequiredInput1, TRequiredInput2>(Func<TRequiredInput1, TRequiredInput2, IEnumerable<TIn>> selector)
        {
            if (selector == null) throw new ArgumentNullException("selector");
            CallForEach<TRequiredInput1, TRequiredInput2>(selector, (s1,s2) => true);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the input type is made
        /// This version calls this transformation rule for a whol collection of input objects
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the source transformation rule</typeparam>
        /// <param name="selector">A method that selects the input for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        /// <param name="filter">A method that filters the applicable instances of S</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public void CallForEach<TRequiredInput>(Func<TRequiredInput, IEnumerable<TIn>> selector, Predicate<TRequiredInput> filter)
        {
            if (filter == null) filter = o => true;
            if (selector == null) throw new ArgumentNullException("selector");
            CallForEachInternal(new Type[] { typeof(TRequiredInput) }, o => filter((TRequiredInput)o.GetInput(0)), o => SelectCallArrays<TRequiredInput>(selector, o), null, false);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the input type is made
        /// This version calls this transformation rule for a whol collection of input objects
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformations</typeparam>
        /// <param name="selector">A method that selects the input for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        /// <param name="filter">A method that filters the applicable instances of S</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void CallForEach<TRequiredInput1, TRequiredInput2>(Func<TRequiredInput1, TRequiredInput2, IEnumerable<TIn>> selector, Func<TRequiredInput1, TRequiredInput2, bool> filter)
        {
            if (filter == null) filter = (o1, o2) => true;
            if (selector == null) throw new ArgumentNullException("selector");
            CallForEachInternal(new Type[] { typeof(TRequiredInput1), typeof(TRequiredInput2) }, o => filter((TRequiredInput1)o.GetInput(0), (TRequiredInput2)o.GetInput(1)), o => SelectCallArraysT2(selector, o), null, false);
        }



        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the input type is made
        /// This version calls this transformation rule for a whol collection of input objects
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the source transformation rule</typeparam>
        /// <param name="selector">A method that selects the inputs for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">This exception is thrown if the rule parameter is passed a null instance.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void CallForEach<TRequiredInput>(GeneralTransformationRule<TRequiredInput> rule, Func<TRequiredInput, IEnumerable<TIn>> selector)
            where TRequiredInput : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            CallForEach<TRequiredInput>(rule, selector, s => true);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the input type is made
        /// This version calls this transformation rule for a whol collection of input objects
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformations</typeparam>
        /// <param name="selector">A method that selects the inputs for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">This exception is thrown if the rule parameter is passed a null instance.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void CallForEach<TRequiredInput1, TRequiredInput2>(GeneralTransformationRule<TRequiredInput1, TRequiredInput2> rule, Func<TRequiredInput1, TRequiredInput2, IEnumerable<TIn>> selector)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            CallForEach<TRequiredInput1, TRequiredInput2>(rule, selector, (s1, s2) => true);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the input type is made
        /// This version calls this transformation rule for a whol collection of input objects
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the source transformation rule</typeparam>
        /// <param name="selector">A method that selects the input for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        /// <param name="filter">A method that filters the applicable instances of S</param>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">This exception is thrown if the rule parameter is passed a null instance.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public void CallForEach<TRequiredInput>(GeneralTransformationRule<TRequiredInput> rule, Func<TRequiredInput, IEnumerable<TIn>> selector, Predicate<TRequiredInput> filter)
            where TRequiredInput : class
        {
            if (filter == null) filter = o => true;
            if (selector == null) throw new ArgumentNullException("selector");
            CallForEachInternal(rule, o => filter((TRequiredInput)o.GetInput(0)), o => SelectCallArrays<TRequiredInput>(selector, o), null, false);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the input type is made
        /// This version calls this transformation rule for a whol collection of input objects
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformations</typeparam>
        /// <param name="selector">A method that selects the input for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        /// <param name="filter">A method that filters the applicable instances of S</param>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">This exception is thrown if the rule parameter is passed a null instance.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public void CallForEach<TRequiredInput1, TRequiredInput2>(GeneralTransformationRule<TRequiredInput1, TRequiredInput2> rule, Func<TRequiredInput1, TRequiredInput2, IEnumerable<TIn>> selector, Func<TRequiredInput1, TRequiredInput2, bool> filter)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
        {
            if (filter == null) filter = (o1, o2) => true;
            if (selector == null) throw new ArgumentNullException("selector");
            CallForEachInternal(rule, o => filter((TRequiredInput1)o.GetInput(0), (TRequiredInput2)o.GetInput(1)), o => SelectCallArraysT2(selector, o), null, false);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the input type is made
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the source transformation rule</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformation</typeparam>
        /// <param name="selector">A method that selects the input for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void CallForOutputSensitive<TRequiredInput, TRequiredOutput>(Func<TRequiredInput, TRequiredOutput, TIn> selector)
        {
            if (selector == null) throw new ArgumentNullException("selector");
            CallForOutputSensitive<TRequiredInput, TRequiredOutput>(selector, null);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the input type is made
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformation</typeparam>
        /// <param name="selector">A method that selects the input for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void CallForOutputSensitive<TRequiredInput1, TRequiredInput2, TRequiredOutput>(Func<TRequiredInput1, TRequiredInput2, TRequiredOutput, TIn> selector)
        {
            if (selector == null) throw new ArgumentNullException("selector");
            CallForOutputSensitive<TRequiredInput1, TRequiredInput2, TRequiredOutput>(selector, null);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the input type is made
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the source transformation rule</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformation</typeparam>
        /// <param name="selector">A method that selects the input for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">This exception is thrown if the rule parameter is passed a null instance.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void CallForOutputSensitive<TRequiredInput, TRequiredOutput>(TransformationRuleBase<TRequiredInput, TRequiredOutput> rule, Func<TRequiredInput, TRequiredOutput, TIn> selector)
            where TRequiredOutput : class
            where TRequiredInput : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            CallForOutputSensitive<TRequiredInput, TRequiredOutput>(rule, selector, null);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the input type is made
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformation</typeparam>
        /// <param name="selector">A method that selects the input for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">This exception is thrown if the rule parameter is passed a null instance.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void CallForOutputSensitive<TRequiredInput1, TRequiredInput2, TRequiredOutput>(TransformationRuleBase<TRequiredInput1, TRequiredInput2, TRequiredOutput> rule, Func<TRequiredInput1, TRequiredInput2, TRequiredOutput, TIn> selector)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
            where TRequiredOutput : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            CallForOutputSensitive<TRequiredInput1, TRequiredInput2, TRequiredOutput>(rule, selector, null);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the input type is made
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the source transformation rule</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformation</typeparam>
        /// <param name="selector">A method that selects the input for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        /// <param name="filter">A method that filters the applicable instances of S</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void CallForOutputSensitive<TRequiredInput, TRequiredOutput>(Func<TRequiredInput, TRequiredOutput, TIn> selector, Func<TRequiredInput, TRequiredOutput, bool> filter)
        {
            if (selector == null) throw new ArgumentNullException("selector");
            CallForInternal(new Type[] { typeof(TRequiredInput) }, 
                filter != null ?
                    new Predicate<Computation>(o => filter((TRequiredInput)o.GetInput(0), (TRequiredOutput)o.Output))
                    : null, 
                c => new object[] { selector((TRequiredInput)c.GetInput(0), (TRequiredOutput)c.Output) }, null, true);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the input type is made
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformation</typeparam>
        /// <param name="selector">A method that selects the input for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        /// <param name="filter">A method that filters the applicable instances of S</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void CallForOutputSensitive<TRequiredInput1, TRequiredInput2, TRequiredOutput>(Func<TRequiredInput1, TRequiredInput2, TRequiredOutput, TIn> selector, Func<TRequiredInput1, TRequiredInput2, TRequiredOutput, bool> filter)
        {
            if (selector == null) throw new ArgumentNullException("selector");
            CallForInternal(new Type[] { typeof(TRequiredInput1), typeof(TRequiredInput2) }, 
                filter != null ?
                    new Predicate<Computation>(o => filter((TRequiredInput1)o.GetInput(0), (TRequiredInput2)o.GetInput(1), (TRequiredOutput)o.Output))
                    : null, 
                c => new object[] { selector((TRequiredInput1)c.GetInput(0), (TRequiredInput2)c.GetInput(1), (TRequiredOutput)c.Output) }, null, true);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the input type is made
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the source transformation rule</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformation</typeparam>
        /// <param name="selector">A method that selects the input for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        /// <param name="filter">A method that filters the applicable instances of S</param>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">This exception is thrown if the rule parameter is passed a null instance.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public void CallForOutputSensitive<TRequiredInput, TRequiredOutput>(TransformationRuleBase<TRequiredInput, TRequiredOutput> rule, Func<TRequiredInput, TRequiredOutput, TIn> selector, Func<TRequiredInput, TRequiredOutput, bool> filter)
            where TRequiredInput : class
            where TRequiredOutput : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            CallForInternal(rule, 
                filter != null ?
                    new Predicate<Computation>(o => filter((TRequiredInput)o.GetInput(0), (TRequiredOutput)o.Output))
                    : null, 
                c => new object[] { selector((TRequiredInput)c.GetInput(0), (TRequiredOutput)c.Output) }, null, true);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the input type is made
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformation</typeparam>
        /// <param name="selector">A method that selects the input for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        /// <param name="filter">A method that filters the applicable instances of S</param>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">This exception is thrown if the rule parameter is passed a null instance.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public void CallForOutputSensitive<TRequiredInput1, TRequiredInput2, TRequiredOutput>(TransformationRuleBase<TRequiredInput1, TRequiredInput2, TRequiredOutput> rule, Func<TRequiredInput1, TRequiredInput2, TRequiredOutput, TIn> selector, Func<TRequiredInput1, TRequiredInput2, TRequiredOutput, bool> filter)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
            where TRequiredOutput : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            CallForInternal(rule, 
                filter != null ?
                    new Predicate<Computation>(o => filter((TRequiredInput1)o.GetInput(0), (TRequiredInput2)o.GetInput(1), (TRequiredOutput)o.Output))
                    : null, 
                c => new object[] { selector((TRequiredInput1)c.GetInput(0), (TRequiredInput2)c.GetInput(1), (TRequiredOutput)c.Output) }, null, true);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the input type is made
        /// This version calls this transformation rule for a whole collection of input objects
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the source transformation rule</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformation</typeparam>
        /// <param name="selector">A method that selects the inputs for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public void CallForEachOutputSensitive<TRequiredInput, TRequiredOutput>(Func<TRequiredInput, TRequiredOutput, IEnumerable<TIn>> selector)
        {
            if (selector == null) throw new ArgumentNullException("selector");
            CallForEachOutputSensitive<TRequiredInput, TRequiredOutput>(selector, null);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the input type is made
        /// This version calls this transformation rule for a whol collection of input objects
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformation</typeparam>
        /// <param name="selector">A method that selects the inputs for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public void CallForEachOutputSensitive<TRequiredInput1, TRequiredInput2, TRequiredOutput>(Func<TRequiredInput1, TRequiredInput2, TRequiredOutput, IEnumerable<TIn>> selector)
        {
            if (selector == null) throw new ArgumentNullException("selector");
            CallForEachOutputSensitive<TRequiredInput1, TRequiredInput2, TRequiredOutput>(selector, null);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the input type is made
        /// This version calls this transformation rule for a whol collection of input objects
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the source transformation rule</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformation</typeparam>
        /// <param name="selector">A method that selects the input for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        /// <param name="filter">A method that filters the applicable instances of S</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public void CallForEachOutputSensitive<TRequiredInput, TRequiredOutput>(Func<TRequiredInput, TRequiredOutput, IEnumerable<TIn>> selector, Func<TRequiredInput, TRequiredOutput, bool> filter)
        {
            if (selector == null) throw new ArgumentNullException("selector");
            CallForEachInternal(new Type[] { typeof(TRequiredInput) }, 
                filter != null ?
                    new Predicate<Computation>(o => filter((TRequiredInput)o.GetInput(0), (TRequiredOutput)o.Output))
                    : null, 
                c => SelectOutputSensitiveCallArrays<TRequiredInput, TRequiredOutput>(selector, c), null, true);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the input type is made
        /// This version calls this transformation rule for a whol collection of input objects
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformation</typeparam>
        /// <param name="selector">A method that selects the input for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        /// <param name="filter">A method that filters the applicable instances of S</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void CallForEachOutputSensitive<TRequiredInput1, TRequiredInput2, TRequiredOutput>(Func<TRequiredInput1, TRequiredInput2, TRequiredOutput, IEnumerable<TIn>> selector, Func<TRequiredInput1, TRequiredInput2, TRequiredOutput, bool> filter)
        {
            if (selector == null) throw new ArgumentNullException("selector");
            CallForEachInternal(new Type[] { typeof(TRequiredInput1), typeof(TRequiredInput2) }, 
                filter != null ?
                    new Predicate<Computation>(o => filter((TRequiredInput1)o.GetInput(0), (TRequiredInput2)o.GetInput(1), (TRequiredOutput)o.Output))
                    : null, 
                c => SelectOutputSensitiveCallArraysT2(selector, c), null, true);
        }



        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the input type is made
        /// This version calls this transformation rule for a whol collection of input objects
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the source transformation rule</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformation</typeparam>
        /// <param name="selector">A method that selects the inputs for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">This exception is thrown if the rule parameter is passed a null instance.</exception>
        public void CallForEachOutputSensitive<TRequiredInput, TRequiredOutput>(TransformationRuleBase<TRequiredInput, TRequiredOutput> rule, Func<TRequiredInput, TRequiredOutput, IEnumerable<TIn>> selector)
            where TRequiredInput : class
            where TRequiredOutput : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            CallForEachOutputSensitive<TRequiredInput, TRequiredOutput>(rule, selector, null);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the input type is made
        /// This version calls this transformation rule for a whol collection of input objects
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformation</typeparam>
        /// <param name="selector">A method that selects the inputs for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">This exception is thrown if the rule parameter is passed a null instance.</exception>
        public void CallForEachOutputSensitive<TRequiredInput1, TRequiredInput2, TRequiredOutput>(TransformationRuleBase<TRequiredInput1, TRequiredInput2, TRequiredOutput> rule, Func<TRequiredInput1, TRequiredInput2, TRequiredOutput, IEnumerable<TIn>> selector)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
            where TRequiredOutput : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            CallForEachOutputSensitive<TRequiredInput1, TRequiredInput2, TRequiredOutput>(rule, selector, null);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the input type is made
        /// This version calls this transformation rule for a whol collection of input objects
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the source transformation rule</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformation</typeparam>
        /// <param name="selector">A method that selects the input for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        /// <param name="filter">A method that filters the applicable instances of S</param>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">This exception is thrown if the rule parameter is passed a null instance.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public void CallForEachOutputSensitive<TRequiredInput, TRequiredOutput>(TransformationRuleBase<TRequiredInput, TRequiredOutput> rule, Func<TRequiredInput, TRequiredOutput, IEnumerable<TIn>> selector, Func<TRequiredInput, TRequiredOutput, bool> filter)
            where TRequiredInput : class
            where TRequiredOutput : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            CallForEachInternal(rule, 
                filter != null ?
                    new Predicate<Computation>(o => filter((TRequiredInput)o.GetInput(0), (TRequiredOutput)o.Output))
                    : null, 
                c => SelectOutputSensitiveCallArrays<TRequiredInput, TRequiredOutput>(selector, c), null, true);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the input type is made
        /// This version calls this transformation rule for a whol collection of input objects
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformation</typeparam>
        /// <param name="selector">A method that selects the input for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        /// <param name="filter">A method that filters the applicable instances of S</param>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">This exception is thrown if the rule parameter is passed a null instance.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public void CallForEachOutputSensitive<TRequiredInput1, TRequiredInput2, TRequiredOutput>(TransformationRuleBase<TRequiredInput1, TRequiredInput2, TRequiredOutput> rule, Func<TRequiredInput1, TRequiredInput2, TRequiredOutput, IEnumerable<TIn>> selector, Func<TRequiredInput1, TRequiredInput2, TRequiredOutput, bool> filter)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
            where TRequiredOutput : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            CallForEachInternal(rule, 
                filter != null ?
                    new Predicate<Computation>(o => filter((TRequiredInput1)o.GetInput(0), (TRequiredInput2)o.GetInput(1), (TRequiredOutput)o.Output))
                    : null, 
                c => SelectOutputSensitiveCallArraysT2(selector, c), null, true);
        }

        /// <summary>
        /// Creates a trace entry for every computation of the current rule for the computation input
        /// </summary>
        /// <typeparam name="TKey">The type that should be used as key for the trace entry</typeparam>
        /// <param name="traceSelector">A method that returns the trace key for the input of a computation</param>
        /// <returns>A trace group that can be used as a key for the trace functionality</returns>
        public TraceEntryGroup<TKey, TIn> TraceInput<TKey>(Func<TIn, TKey> traceSelector) where TKey : class
        {
            if (traceSelector == null) throw new ArgumentNullException("traceSelector");

            var traceRule = new TraceEntryGroup<TKey, TIn>();
            var traceDependency = new InputTraceDependency<TIn, TKey>()
            {
                KeySelector = traceSelector,
                TraceRule = traceRule
            };
            Dependencies.Add(traceDependency);
            return traceRule;
        }

        /// <summary>
        /// Creates a trace entry for every computation of the current rule for the computation input
        /// </summary>
        /// <typeparam name="TKey">The type that should be used as key for the trace entry</typeparam>
        /// <param name="traceSelector">A method that returns the trace key for the input of a computation</param>
        /// <param name="traceKey">A trace group used as a key for the trace functionality</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public void TraceInput<TKey>(TraceEntryGroup<TKey, TIn> traceKey, Func<TIn, TKey> traceSelector) where TKey : class
        {
            if (traceSelector == null) throw new ArgumentNullException("traceSelector");
            if (traceKey == null) throw new ArgumentNullException("traceKey");

            var traceDependency = new InputTraceDependency<TIn, TKey>()
            {
                KeySelector = traceSelector,
                TraceRule = traceKey
            };
            Dependencies.Add(traceDependency);
        }
        
        internal static IEnumerable<object[]> SelectCallArrays<TRequiredInput>(Func<TRequiredInput, IEnumerable<TIn>> selector, Computation computation)
        {
            var tc = selector((TRequiredInput)computation.GetInput(0));
            if (tc != null)
            {
                return tc.Select(t => new object[] { t });
            }
            else
            {
                return null;
            }
        }

        
        internal static IEnumerable<object[]> SelectCallArraysT2<TRequiredInput1, TRequiredInput2>(Func<TRequiredInput1, TRequiredInput2, IEnumerable<TIn>> selector, Computation computation)
        {
            var tc = selector((TRequiredInput1)computation.GetInput(0), (TRequiredInput2)computation.GetInput(1));
            if (tc != null)
            {
                return tc.Select(t => new object[] { t });
            }
            else
            {
                return null;
            }
        }

        internal static IEnumerable<object[]> SelectOutputSensitiveCallArrays<TRequiredInput, TRequiredOutput>(Func<TRequiredInput, TRequiredOutput, IEnumerable<TIn>> selector, Computation computation)
        {
            var tc = selector((TRequiredInput)computation.GetInput(0), (TRequiredOutput)computation.Output);
            if (tc != null)
            {
                return tc.Select(t => new object[] { t });
            }
            else
            {
                return null;
            }
        }


        internal static IEnumerable<object[]> SelectOutputSensitiveCallArraysT2<TRequiredInput1, TRequiredInput2, TRequiredOutput>(Func<TRequiredInput1, TRequiredInput2, TRequiredOutput, IEnumerable<TIn>> selector, Computation computation)
        {
            var tc = selector((TRequiredInput1)computation.GetInput(0), (TRequiredInput2)computation.GetInput(1), (TRequiredOutput)computation.Output);
            if (tc != null)
            {
                return tc.Select(t => new object[] { t });
            }
            else
            {
                return null;
            }
        }

        private void CallForInternal(Type[] inputTypes, Predicate<Computation> filter, Func<Computation, object[]> selector, Action<object, object> persistor, bool needOutput)
        {
            foreach (var rule in Transformation.GetRulesForInputTypes(inputTypes))
            {
                CallForInternal(rule, filter, selector, persistor, needOutput);
            }
        }

        private void CallForEachInternal(Type[] inputTypes, Predicate<Computation> filter, Func<Computation, IEnumerable<object[]>> selector, Action<object, IEnumerable> persistor, bool needOutput)
        {
            foreach (var rule in Transformation.GetRulesForInputTypes(inputTypes))
            {
                CallForEachInternal(rule, filter, selector, persistor, needOutput);
            }
        }

        /// <summary>
        /// This method is a helper function that converts the given selector of tuples to a selector of object[]
        /// </summary>
        /// <param name="selector">The source selector that selects the output of dependant transformation rules as typed tuples</param>
        /// <param name="computation">The inputs for the selector</param>
        /// <returns>A collection of inputs for other transformation rules</returns>
        /// <remarks>This method is used as helper function for DependMany.</remarks>
        internal static IEnumerable<object[]> SelectArrays<TRequiredInput>(Func<TIn, IEnumerable<TRequiredInput>> selector, Computation computation) where TRequiredInput : class
        {
            var tc = selector((TIn)computation.GetInput(0));
            if (tc != null)
            {
                return tc.Select(s => new object[] { s });
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// This method is a helper function that converts the given selector of tuples to a selector of object[]
        /// </summary>
        /// <param name="selector">The source selector that selects the output of dependant transformation rules as typed tuples</param>
        /// <param name="computation">The inputs for the selector</param>
        /// <returns>A collection of inputs for other transformation rules</returns>
        /// <remarks>This method is used as helper function for DependMany.</remarks>
        internal static IEnumerable<object[]> SelectArraysT2<TRequiredInput1, TRequiredInput2>(Func<TIn, IEnumerable<Tuple<TRequiredInput1, TRequiredInput2>>> selector, Computation computation) 
            where TRequiredInput1 : class
            where TRequiredInput2 : class
        {
            var tc = selector((TIn)computation.GetInput(0));
            if (tc != null)
            {
                return tc.Select(s => new object[] { s.Item1, s.Item2 });
            }
            else
            {
                return null;
            }
        }



        #region Computation-based

        /// <summary>
        /// Specify that the current transformation rule is called whenenver objects are transformed with the given transformation rule
        /// </summary>
        /// <param name="selector">A method that selects the input parameters for the current transformation rule</param>
        /// <param name="rule">The transformation rule that act as trigger for the current transformation rule</param>
        /// <param name="needOutput">True, if the call must be made after the output of the trigger rule is created, otherwise false</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the rule parameter is passed a null instance</exception>
        public void CallFor(GeneralTransformationRule rule, Func<Computation, TIn> selector, bool needOutput)
        {
            CallFor(rule, selector, null, needOutput);
        }

        /// <summary>
        /// Specify that the current transformation rule is called whenenver objects are transformed with the given transformation rule
        /// </summary>
        /// <param name="selector">A method that selects the input parameters for the current transformation rule</param>
        /// <param name="filter">A method to filter the objects where the reversed dependency is applicable</param>
        /// <param name="needOutput">True, if the call must be made after the output of the trigger rule is created, otherwise false</param>
        /// <param name="rule">The transformation rule that act as trigger for the current transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the rule parameter is passed a null instance</exception>
        public void CallFor(GeneralTransformationRule rule, Func<Computation, TIn> selector, Predicate<Computation> filter, bool needOutput)
        {
            CallForInternal(rule, filter, c => new object[] { selector(c) }, null, needOutput);
        }

        /// <summary>
        /// Specify that the current transformation rule is called whenenver the given types are transformed multiple times
        /// </summary>
        /// <param name="selector">A method that selects the input parameters for the current transformation rule</param>
        /// <param name="rule">The transformation rule acting as trigger for the current transformation rule</param>
        /// <param name="needOutput">True, if the call must be made after the output of the trigger rule is created, otherwise false</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the rule parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        public void CallForEach(GeneralTransformationRule rule, Func<Computation, IEnumerable<TIn>> selector, bool needOutput)
        {
            CallForEach(rule, selector, null, needOutput);
        }

        /// <summary>
        /// Specify that the current transformation rule is called whenenver the given types are transformed multiple times
        /// </summary>
        /// <param name="selector">A method that selects the input parameters for the current transformation rule</param>
        /// <param name="filter">A method that filters the inputs of the transformation rule acting as trigger</param>
        /// <param name="rule">The transformation rule acting as trigger for the current transformation rule</param>
        /// <param name="needOutput">True, if the call must be made after the output of the trigger rule is created, otherwise false</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the rule parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        public void CallForEach(GeneralTransformationRule rule, Func<Computation, IEnumerable<TIn>> selector, Predicate<Computation> filter, bool needOutput)
        {
            CallForEachInternal(rule, filter, c => selector(c).Select(t => new object[] { t }), null, needOutput);
        }

        #endregion

        /// <summary>
        /// Gets the name of the transformation rule
        /// </summary>
        /// <returns>The name of the transformation rule</returns>
        public override string ToString()
        {
            return this.GetType().Name;
        }
    }
}
