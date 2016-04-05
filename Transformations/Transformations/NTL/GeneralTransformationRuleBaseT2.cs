using NMF.Transformations.Properties;
using NMF.Transformations.Linq;
using NMF.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using NMF.Transformations.Core;
using NMF.Expressions;

namespace NMF.Transformations
{
    /// <summary>
    /// Defines a transformation rule of a transformation that has one input argument
    /// </summary>
    /// <typeparam name="TIn1">The type of the first input argument</typeparam>
    /// <typeparam name="TIn2">The type of the second input argument</typeparam>
    public abstract class GeneralTransformationRule<TIn1, TIn2> : GeneralTransformationRule
        where TIn1 : class
        where TIn2 : class
    {
        private static readonly Type[] types = new Type[] { typeof(TIn1), typeof(TIn2) };

        /// <summary>
        /// Marks the current transformation rule instantiating for the specified rule
        /// </summary>
        /// <param name="filter">The filter that should filter the inputs where this transformation rule is marked instantiating</param>
        /// <param name="rule">The transformation rule</param>
        public void MarkInstantiatingFor(GeneralTransformationRule rule, Func<TIn1, TIn2, bool> filter)
        {
            if (rule == null) throw new ArgumentNullException("rule");

            if (rule.InputType.IsAssignableArrayFrom(InputType) && (rule.OutputType == OutputType || rule.OutputType.IsAssignableFrom(OutputType)))
            {
                Require(rule);
                if (filter != null)
                {
                    MarkInstantiatingFor(rule, (Computation c) => filter(c.GetInput(0) as TIn1, c.GetInput(1) as TIn2));
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
        /// Gets the type signature of the input arguments of this transformation rule
        /// </summary>
        public sealed override Type[] InputType
        {
            get { return types; }
        }

        #region Convenience

        /// <summary>
        /// Registers the given pattern for the current transformation rule
        /// </summary>
        /// <param name="pattern">The pattern that should be applied based on the current transformation rule</param>
        public void WithPattern(ITransformationRulePattern<TIn1, TIn2> pattern)
        {
            if (pattern == null) throw new ArgumentNullException("pattern");

            pattern.TargetRule = this;

            Transformation.Patterns.Add(pattern);
        }

        /// <summary>
        /// Registers the given incremental pattern for the current transformation rule
        /// </summary>
        /// <param name="patternCreator">The relation pattern that should be applied on the current transformation rule</param>
        public void WithPattern(Func<ITransformationContext, INotifyEnumerable<Tuple<TIn1, TIn2>>> patternCreator)
        {
            WithPattern(new Linq.IncrementalPattern<TIn1, TIn2>(patternCreator));
        }

        /// <summary>
        /// Registers the given incremental pattern for the current transformation rule
        /// </summary>
        /// <param name="patternCreator">The relation pattern that should be applied on the current transformation rule</param>
        public void WithPattern(Func<ITransformationContext, IEnumerable<Tuple<TIn1, TIn2>>> patternCreator)
        {
            WithPattern(new Linq.StaticPattern<TIn1, TIn2>(patternCreator));
        }

        /// <summary>
        /// Requires all transformation rules that transform items from S1 and S2 to T
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformations</typeparam>
        /// <remarks>This version Always takes the input parameter as input for the dependent transformations. Thus, this method will throw an exception, if the types do not match</remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void RequireByType<TRequiredInput1, TRequiredInput2>()
            where TRequiredInput1 : class
            where TRequiredInput2 : class
        {
            if (typeof(TRequiredInput1).IsAssignableFrom(typeof(TIn1)) && typeof(TRequiredInput2).IsAssignableFrom(typeof(TIn2)))
            {
                RequireByType<TRequiredInput1, TRequiredInput2>((t1, t2) => t1 as TRequiredInput1, (t1, t2) => t2 as TRequiredInput2);
            }
            else
            {
                throw new InvalidOperationException(Resources.ErrRequires2ArgNoSelectorMustInherit);
            }
            
        }

        /// <summary>
        /// Requires all transformation rules that transform items from S1, S2 to T
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredInput2">The first input argument type of the dependent transformations</typeparam>
        /// <param name="selector1">A method that selects the first input for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown when a null reference is passed to the selector1 parameter</exception>
        /// <param name="selector2">A method that selects the second input for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown when a null reference is passed to the selector2 parameter</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void RequireByType<TRequiredInput1, TRequiredInput2>(Func<TIn1, TIn2, TRequiredInput1> selector1, Func<TIn1, TIn2, TRequiredInput2> selector2)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
        {
            if (selector1 == null) throw new ArgumentNullException("selector1");
            if (selector2 == null) throw new ArgumentNullException("selector2");

            foreach (var rule in Transformation.GetRulesForInputTypes(new Type[] { typeof(TRequiredInput1), typeof(TRequiredInput2) }))
            {
                Depend(null, c =>
            {
                return DependencySelect<TRequiredInput1, TRequiredInput2>(selector1, selector2, c);
            }, rule, null, true, false);
            }
        }

        /// <summary>
        /// Requires all transformation rules that transform items from S1, S2 to T
        /// </summary>
        /// <typeparam name="TRequiredInput">The first input argument type of the dependent transformations</typeparam>
        /// <param name="selector">A method that selects the input for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void RequireByType<TRequiredInput>(Func<TIn1, TIn2, TRequiredInput> selector)
            where TRequiredInput : class
        {
            if (selector == null) throw new ArgumentNullException("selector");

            foreach (var rule in Transformation.GetRulesForInputTypes(new Type[] { typeof(TRequiredInput) }))
            {
                Depend(null, c =>
                {
                    return DependencySelect<TRequiredInput>(selector, c);
                }, rule, null, true, false);
            }
        }

        /// <summary>
        /// Requires the given transformation rule with the same input types and a filter
        /// </summary>
        /// <param name="rule">The transformation rule</param>
        /// <param name="filter">The filter method</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public void Require(GeneralTransformationRule<TIn1, TIn2> rule, Func<TIn1, TIn2, bool> filter)
        {
            Predicate<Computation> f = null;
            if (filter != null)
            {
                f = c => filter(c.GetInput(0) as TIn1, c.GetInput(1) as TIn2);
            }
            Depend(f, c => c.CreateInputArray(), rule, null, true, false);
        }

        /// <summary>
        /// Requires the transformation rule with the given type
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformation</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the rule parameter is passed a null instance</exception>
        /// <param name="selector1">A method that selects the first input for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown when a null reference is passed to the selector1 parameter</exception>
        /// <param name="selector2">A method that selects the second input for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown when a null reference is passed to the selector2 parameter</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency Require<TRequiredInput1, TRequiredInput2>(GeneralTransformationRule<TRequiredInput1, TRequiredInput2> rule, Func<TIn1, TIn2, TRequiredInput1> selector1, Func<TIn1, TIn2, TRequiredInput2> selector2)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
        {
            if (selector1 == null) throw new ArgumentNullException("selector1");
            if (selector2 == null) throw new ArgumentNullException("selector2");
            return Depend(null, c =>
            {
                return DependencySelect<TRequiredInput1, TRequiredInput2>(selector1, selector2, c);
            }, rule, null, true, false);
        }

        /// <summary>
        /// Requires the transformation rule with the given type
        /// </summary>
        /// <typeparam name="TRequiredInput">The first input argument type of the dependent transformation</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <param name="selector">A method that selects the input for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the rule parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency Require<TRequiredInput>(GeneralTransformationRule<TRequiredInput> rule, Func<TIn1, TIn2, TRequiredInput> selector)
            where TRequiredInput : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            return Depend(null, c =>
            {
                return DependencySelect<TRequiredInput>(selector, c);
            }, rule, null, true, false);
        }

        /// <summary>
        /// Requires the transformation rule with the given type
        /// </summary>
        /// <param name="rule">The dependent transformation rule</param>
        /// <param name="selector">A method that selects the first input for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the rule parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter"), EditorBrowsable(EditorBrowsableState.Advanced)]
        public ITransformationRuleDependency Require(GeneralTransformationRule rule, Func<TIn1, TIn2, object[]> selector)
        {
            if (selector == null) throw new ArgumentNullException("selector");
            return Depend(null, c => selector(c.GetInput(0) as TIn1, c.GetInput(1) as TIn2), rule, null, true, false);
        }

        /// <summary>
        /// Requires all transformation rules that transform S1, S2 to T with all of the specified objects
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformations</typeparam>
        /// <param name="selector">A method that selects the inputs for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void RequireManyByType<TRequiredInput1, TRequiredInput2>(Func<TIn1, TIn2, IEnumerable<Tuple<TRequiredInput1, TRequiredInput2>>> selector)
            where TRequiredInput1 : class
            where TRequiredInput2: class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            foreach (var rule in Transformation.GetRulesForInputTypes(new Type[] { typeof(TRequiredInput1), typeof(TRequiredInput2) }))
            {
                DependMany(null, c =>
            {
                return DependencySelectMany<TRequiredInput1, TRequiredInput2>(selector, c);
            }, rule, null, true, false);
            }
        }

        /// <summary>
        /// Requires all transformation rules that transform S1, S2 to T with all of the specified objects
        /// </summary>
        /// <typeparam name="TRequiredInput">The first input argument type of the dependent transformations</typeparam>
        /// <param name="selector">A method that selects the inputs for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void RequireManyByType<TRequiredInput>(Func<TIn1, TIn2, IEnumerable<TRequiredInput>> selector)
            where TRequiredInput : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            foreach (var rule in Transformation.GetRulesForInputTypes(new Type[] { typeof(TRequiredInput) }))
            {
                DependMany(null, c =>
                {
                    return DependencySelectMany<TRequiredInput>(selector, c);
                }, rule, null, true, false);
            }
        }

        /// <summary>
        /// Requires the transformation rule with the given type with all of the specified inputs
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformations</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <param name="selector">A method that selects the inputs for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the rule parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency RequireMany<TRequiredInput1, TRequiredInput2>(GeneralTransformationRule<TRequiredInput1, TRequiredInput2> rule, Func<TIn1, TIn2, IEnumerable<Tuple<TRequiredInput1, TRequiredInput2>>> selector)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            return DependMany(null, c =>
            {
                return DependencySelectMany<TRequiredInput1, TRequiredInput2>(selector, c);
            }, rule, null, true, false);
        }

        /// <summary>
        /// Requires the transformation rule with the given type with all of the specified inputs
        /// </summary>
        /// <typeparam name="TRequiredInput">The first input argument type of the dependent transformations</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <param name="selector">A method that selects the inputs for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the rule parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency RequireMany<TRequiredInput>(GeneralTransformationRule<TRequiredInput> rule, Func<TIn1, TIn2, IEnumerable<TRequiredInput>> selector)
            where TRequiredInput : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            return DependMany(null, c =>
            {
                return DependencySelectMany<TRequiredInput>(selector, c);
            }, rule, null, true, false);
        }

        /// <summary>
        /// Calls the transformation rules that match the given input types with the same inputs
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input type of the dependent transformation rule</typeparam>
        /// <typeparam name="TRequiredInput2">The second input type of the dependent transformation rule</typeparam>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void CallByType<TRequiredInput1, TRequiredInput2>()
            where TRequiredInput1 : class
            where TRequiredInput2 : class
        {
            CallByType<TRequiredInput1, TRequiredInput2>((t1, t2) => t1 as TRequiredInput1, (t1, t2) => t2 as TRequiredInput2);
        }

        /// <summary>
        /// Calls the given transformation rule with the same inputs
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input type of the dependent transformation rule</typeparam>
        /// <typeparam name="TRequiredInput2">The second input type of the dependent transformation rule</typeparam>
        /// <param name="rule">The transformation rule that needs to be called</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the rule parameter is passed a null instance</exception>
        public ITransformationRuleDependency Call<TRequiredInput1, TRequiredInput2>(GeneralTransformationRule<TRequiredInput1, TRequiredInput2> rule)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
        {
            return Call<TRequiredInput1, TRequiredInput2>(rule, (t1, t2) => t1 as TRequiredInput1, (t1, t2) => t2 as TRequiredInput2);
        }

        /// <summary>
        /// Calls the transformation rules that match the given input type signature
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input type of the dependent transformation rules</typeparam>
        /// <typeparam name="TRequiredInput2">The second input type of the dependent transformation rules</typeparam>
        /// <param name="selector1">A method that selects the first input for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown when a null reference is passed to the selector1 parameter</exception>
        /// <param name="selector2">A method that selects the second input for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown when a null reference is passed to the selector2 parameter</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void CallByType<TRequiredInput1, TRequiredInput2>(Func<TIn1, TIn2, TRequiredInput1> selector1, Func<TIn1, TIn2, TRequiredInput2> selector2)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
        {
            if (selector1 == null) throw new ArgumentNullException("selector1");
            if (selector2 == null) throw new ArgumentNullException("selector2"); 
            
            foreach (var rule in Transformation.GetRulesForInputTypes(new Type[] { typeof(TRequiredInput1), typeof(TRequiredInput2) }))
            {
                Depend(null, c => DependencySelect<TRequiredInput1, TRequiredInput2>(selector1, selector2, c), rule, null, false, false);   
            }
        }

        /// <summary>
        /// Calls all transformation rules that match the given input type signature
        /// </summary>
        /// <typeparam name="TRequiredInput">The type of the input argument of dependent transformation rules</typeparam>
        /// <param name="selector">A method that selects the input arguzment for the dependent transformation rules</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void CallByType<TRequiredInput>(Func<TIn1, TIn2, TRequiredInput> selector)
            where TRequiredInput : class
        {
            if (selector == null) throw new ArgumentNullException("selector");

            foreach (var rule in Transformation.GetRulesForInputTypes(new Type[] { typeof(TRequiredInput) }))
            {
                Depend(null, c => DependencySelect<TRequiredInput>(selector, c), rule, null, false, false);
            }
        }

        /// <summary>
        /// Calls the given transformation rule
        /// </summary>
        /// <typeparam name="TRequiredInput1">The type of the first input argument of the dependent transformation rule</typeparam>
        /// <typeparam name="TRequiredInput2">The type of the second input argument of the dependent transformation rule</typeparam>
        /// <param name="rule">The transformation rule that needs to be called</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the rule parameter is passed a null instance</exception>
        /// <param name="selector1">A method that selects the first input for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown when a null reference is passed to the selector1 parameter</exception>
        /// <param name="selector2">A method that selects the second input for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown when a null reference is passed to the selector2 parameter</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency Call<TRequiredInput1, TRequiredInput2>(GeneralTransformationRule<TRequiredInput1, TRequiredInput2> rule, Func<TIn1, TIn2, TRequiredInput1> selector1, Func<TIn1, TIn2, TRequiredInput2> selector2)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
        {
            if (selector1 == null) throw new ArgumentNullException("selector1");
            if (selector2 == null) throw new ArgumentNullException("selector2");

            return Depend(null, c => DependencySelect<TRequiredInput1, TRequiredInput2>(selector1, selector2, c), rule, null, false, false);
        }


        /// <summary>
        /// Requires the given transformation rule with the same input types and a filter
        /// </summary>
        /// <param name="rule">The transformation rule</param>
        /// <param name="filter">The filter method</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public void Call(GeneralTransformationRule<TIn1, TIn2> rule, Func<TIn1, TIn2, bool> filter)
        {
            Predicate<Computation> f = null;
            if (filter != null)
            {
                f = c => filter(c.GetInput(0) as TIn1, c.GetInput(1) as TIn2);
            }
            Depend(f, c => c.CreateInputArray(), rule, null, false, false);
        }


        /// <summary>
        /// Calls the given transformation rule
        /// </summary>
        /// <typeparam name="TRequiredInput">The type of the dependent transformation rules input argument</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <param name="selector">A method that selects the input parameter for the dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the rule parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency Call<TRequiredInput>(GeneralTransformationRule<TRequiredInput> rule, Func<TIn1, TIn2, TRequiredInput> selector)
            where TRequiredInput : class
        {
            if (selector == null) throw new ArgumentNullException("selector");

            return Depend(null, c => DependencySelect<TRequiredInput>(selector, c), rule, null, false, false);
        }

        /// <summary>
        /// Calls the transformation rules that match the given input type signature multiple times
        /// </summary>
        /// <typeparam name="TRequiredInput1">The type of the first input parameter of the dependent transformation rules</typeparam>
        /// <typeparam name="TRequiredInput2">The type of the second input parameter of the dependent transformation rules</typeparam>
        /// <param name="selector">A method that selects the input parameters for the dependent transformation rules</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void CallManyByType<TRequiredInput1, TRequiredInput2>(Func<TIn1, TIn2, IEnumerable<Tuple<TRequiredInput1, TRequiredInput2>>> selector)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            foreach (var rule in Transformation.GetRulesForInputTypes(new Type[] { typeof(TRequiredInput1), typeof(TRequiredInput2) }))
            {
                DependMany(null, c => DependencySelectMany(selector, c), rule, null, false, false);
            }
        }


        /// <summary>
        /// Calls the transformation rules that match the given input type signature multiple times
        /// </summary>
        /// <typeparam name="TRequiredInput">The type of the input parameter of the dependent transformation rules</typeparam>
        /// <param name="selector">A method that selects the input parameter for the dependent transformation rules</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void CallManyByType<TRequiredInput>(Func<TIn1, TIn2, IEnumerable<TRequiredInput>> selector)
            where TRequiredInput : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            foreach (var rule in Transformation.GetRulesForInputTypes(new Type[] { typeof(TRequiredInput) }))
            {
                DependMany(null, c => DependencySelectMany(selector, c), rule, null, false, false);
            }
        }

        /// <summary>
        /// Calls the given transformation rule multiple times
        /// </summary>
        /// <typeparam name="TRequiredInput1">The type of the first input parameter of the dependent transformation rules</typeparam>
        /// <typeparam name="TRequiredInput2">The type of the second input parameter of the dependent transformation rules</typeparam>
        /// <param name="selector">A method that selects the input parameters for the dependent transformation rules</param>
        /// <param name="rule">The transformation rule that needs to be called</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the rule parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency CallMany<TRequiredInput1, TRequiredInput2>(GeneralTransformationRule<TRequiredInput1, TRequiredInput2> rule, Func<TIn1, TIn2, IEnumerable<Tuple<TRequiredInput1, TRequiredInput2>>> selector)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            return DependMany(null, c =>
            {
                return DependencySelectMany(selector, c);
            }, rule, null, false, false);
        }

        /// <summary>
        /// Calls the given transformation rule multiple times
        /// </summary>
        /// <typeparam name="TRequiredInput">The type of the input parameter of the dependent transformation rules</typeparam>
        /// <param name="selector">A method that selects the input parameter for the dependent transformation rules</param>
        /// <param name="rule">The transformation rule that needs to be called</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the rule parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency CallMany<TRequiredInput>(GeneralTransformationRule<TRequiredInput> rule, Func<TIn1, TIn2, IEnumerable<TRequiredInput>> selector)
            where TRequiredInput : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            return DependMany(null, c =>
            {
                return DependencySelectMany(selector, c);
            }, rule, null, false, false);
        }

        /// <summary>
        /// Specify that the current transformation rule is called whenenver the given types are transformed
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input type parameter of the transformation rule acting as trigger</typeparam>
        /// <typeparam name="TRequiredInput2">The second input type parameter of the transformation rule acting as trigger</typeparam>
        /// <param name="selector1">A method that selects the first input parameter for the current transformation rule</param>
        /// <param name="selector2">A method that selects the second input parameter for the current transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector1 parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector2 parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void CallFor<TRequiredInput1, TRequiredInput2>(Func<TRequiredInput1, TRequiredInput2, TIn1> selector1, Func<TRequiredInput1, TRequiredInput2, TIn2> selector2)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
        {
            CallFor<TRequiredInput1, TRequiredInput2>(selector1, selector2, null);
        }

        /// <summary>
        /// Specify that the current transformation rule is called whenenver the given types are transformed
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input type parameter of the transformation rule acting as trigger</typeparam>
        /// <typeparam name="TRequiredInput2">The second input type parameter of the transformation rule acting as trigger</typeparam>
        /// <param name="selector1">A method that selects the first input parameter for the current transformation rule</param>
        /// <param name="selector2">A method that selects the second input parameter for the current transformation rule</param>
        /// <param name="filter">A method that filters the objects where the dependency should be applied</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector1 parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector2 parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void CallFor<TRequiredInput1, TRequiredInput2>(Func<TRequiredInput1, TRequiredInput2, TIn1> selector1, Func<TRequiredInput1, TRequiredInput2, TIn2> selector2, Func<TRequiredInput1, TRequiredInput2, bool> filter)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
        {
            if (selector1 == null) throw new ArgumentNullException("selector1");
            if (selector2 == null) throw new ArgumentNullException("selector2"); 

            CallForInternal(new Type[] { typeof(TRequiredInput1), typeof(TRequiredInput2) }, 
                filter != null ?
                    new Predicate<Computation>(c => filter(c.GetInput(0) as TRequiredInput1, c.GetInput(1) as TRequiredInput2))
                    : null, c => {
                return CallDependencySelect<TRequiredInput1, TRequiredInput2>(selector1, selector2, c);
            }, null, false);
        }

        /// <summary>
        /// Specify that the current transformation rule is called whenenver objects are transformed with the given transformation rule
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input type parameter of the transformation rule acting as trigger</typeparam>
        /// <typeparam name="TRequiredInput2">The second input type parameter of the transformation rule acting as trigger</typeparam>
        /// <param name="selector1">A method that selects the first input parameter for the current transformation rule</param>
        /// <param name="selector2">A method that selects the second input parameter for the current transformation rule</param>
        /// <param name="rule">The transformation rule that act as trigger for the current transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector1 parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector2 parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the rule parameter is passed a null instance</exception>
        public void CallFor<TRequiredInput1, TRequiredInput2>(GeneralTransformationRule<TRequiredInput1, TRequiredInput2> rule, Func<TRequiredInput1, TRequiredInput2, TIn1> selector1, Func<TRequiredInput1, TRequiredInput2, TIn2> selector2)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
        {
            CallFor<TRequiredInput1, TRequiredInput2>(rule, selector1, selector2, (s1, s2) => true);
        }

        /// <summary>
        /// Specify that the current transformation rule is called whenenver objects are transformed with the given transformation rule
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input type parameter of the transformation rule acting as trigger</typeparam>
        /// <typeparam name="TRequiredInput2">The second input type parameter of the transformation rule acting as trigger</typeparam>
        /// <param name="selector1">A method that selects the first input parameter for the current transformation rule</param>
        /// <param name="selector2">A method that selects the second input parameter for the current transformation rule</param>
        /// <param name="filter">A method to filter the objects where the reversed dependency is applicable</param>
        /// <param name="rule">The transformation rule that act as trigger for the current transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector1 parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector2 parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the rule parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public void CallFor<TRequiredInput1, TRequiredInput2>(GeneralTransformationRule<TRequiredInput1, TRequiredInput2> rule, Func<TRequiredInput1, TRequiredInput2, TIn1> selector1, Func<TRequiredInput1, TRequiredInput2, TIn2> selector2, Func<TRequiredInput1, TRequiredInput2, bool> filter)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
        {
            if (selector1 == null) throw new ArgumentNullException("selector1");
            if (selector2 == null) throw new ArgumentNullException("selector2");

            CallForInternal(rule, c =>
            {
                return CallFilter<TRequiredInput1, TRequiredInput2>(filter, c);
            }, c =>
            {
                return CallDependencySelect<TRequiredInput1, TRequiredInput2>(selector1, selector2, c);
            }, null, false);
        }

        /// <summary>
        /// Specify that the current transformation rule is called whenenver the given types are transformed
        /// </summary>
        /// <typeparam name="TRequiredInput">The input type of the transformation rule acting as trigger</typeparam>
        /// <param name="selector1">A method that selects the first input parameter for the current transformation rule</param>
        /// <param name="selector2">A method that selects the second input parameter for the current transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector1 parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector2 parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void CallFor<TRequiredInput>(Func<TRequiredInput, TIn1> selector1, Func<TRequiredInput, TIn2> selector2)
        {
            CallFor<TRequiredInput>(selector1, selector2, s => true);
        }

        /// <summary>
        /// Specify that the current transformation rule is called whenenver the given types are transformed
        /// </summary>
        /// <typeparam name="TRequiredInput">The input type of the transformation rule acting as trigger</typeparam>
        /// <param name="selector1">A method that selects the first input parameter for the current transformation rule</param>
        /// <param name="selector2">A method that selects the second input parameter for the current transformation rule</param>
        /// <param name="filter">A method that filters the objects where the dependency should be applied</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector1 parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector2 parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void CallFor<TRequiredInput>(Func<TRequiredInput, TIn1> selector1, Func<TRequiredInput, TIn2> selector2, Func<TRequiredInput, bool> filter)
        {
            if (selector1 == null) throw new ArgumentNullException("selector1");
            if (selector2 == null) throw new ArgumentNullException("selector2");

            CallForInternal(new Type[] { typeof(TRequiredInput) }, c =>
            {
                return CallFilter<TRequiredInput>(filter, c);
            }, o =>
            {
                return CallDependencySelect<TRequiredInput>(selector1, selector2, o);
            }, null, false);
        }

        /// <summary>
        /// Specify that the current transformation rule is called whenenver objects are transformed with the given transformation rule
        /// </summary>
        /// <typeparam name="TRequiredInput">The input type of the transformation rule acting as trigger</typeparam>
        /// <param name="selector1">A method that selects the first input parameter for the current transformation rule</param>
        /// <param name="selector2">A method that selects the second input parameter for the current transformation rule</param>
        /// <param name="rule">The transformation rule that act as trigger for the current transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector1 parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector2 parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the rule parameter is passed a null instance</exception>
        public void CallFor<TRequiredInput>(GeneralTransformationRule<TRequiredInput> rule, Func<TRequiredInput, TIn1> selector1, Func<TRequiredInput, TIn2> selector2)
            where TRequiredInput : class
        {
            CallFor<TRequiredInput>(rule, selector1, selector2, s => true);
        }

        /// <summary>
        /// Specify that the current transformation rule is called whenenver objects are transformed with the given transformation rule
        /// </summary>
        /// <typeparam name="TRequiredInput">The input type of the transformation rule acting as trigger</typeparam>
        /// <param name="selector1">A method that selects the first input parameter for the current transformation rule</param>
        /// <param name="selector2">A method that selects the second input parameter for the current transformation rule</param>
        /// <param name="filter">A method to filter the objects where the reversed dependency is applicable</param>
        /// <param name="rule">The transformation rule that act as trigger for the current transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector1 parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector2 parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the rule parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public void CallFor<TRequiredInput>(GeneralTransformationRule<TRequiredInput> rule, Func<TRequiredInput, TIn1> selector1, Func<TRequiredInput, TIn2> selector2, Func<TRequiredInput, bool> filter)
            where TRequiredInput : class
        {
            if (selector1 == null) throw new ArgumentNullException("selector1");
            if (selector2 == null) throw new ArgumentNullException("selector2");

            CallForInternal(rule, c =>
            {
                return CallFilter<TRequiredInput>(filter, c);
            }, o =>
            {
                return CallDependencySelect<TRequiredInput>(selector1, selector2, o);
            }, null, false);
        }

        /// <summary>
        /// Specify that the current transformation rule is called whenenver the given types are transformed
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input type parameter of the transformation rule acting as trigger</typeparam>
        /// <typeparam name="TRequiredInput2">The second input type parameter of the transformation rule acting as trigger</typeparam>
        /// <typeparam name="TRequiredOutput">The output type parameter of the transformation rule acting as trigger</typeparam>
        /// <param name="selector1">A method that selects the first input parameter for the current transformation rule</param>
        /// <param name="selector2">A method that selects the second input parameter for the current transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector1 parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector2 parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void CallForOutputSensitive<TRequiredInput1, TRequiredInput2, TRequiredOutput>(Func<TRequiredInput1, TRequiredInput2, TRequiredOutput, TIn1> selector1, Func<TRequiredInput1, TRequiredInput2, TRequiredOutput, TIn2> selector2)
        {
            CallForOutputSensitive<TRequiredInput1, TRequiredInput2, TRequiredOutput>(selector1, selector2, null);
        }

        /// <summary>
        /// Specify that the current transformation rule is called whenenver the given types are transformed
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input type parameter of the transformation rule acting as trigger</typeparam>
        /// <typeparam name="TRequiredInput2">The second input type parameter of the transformation rule acting as trigger</typeparam>
        /// <typeparam name="TRequiredOutput">The output type parameter of the transformation rule acting as trigger</typeparam>
        /// <param name="selector1">A method that selects the first input parameter for the current transformation rule</param>
        /// <param name="selector2">A method that selects the second input parameter for the current transformation rule</param>
        /// <param name="filter">A method that filters the objects where the dependency should be applied</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector1 parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector2 parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void CallForOutputSensitive<TRequiredInput1, TRequiredInput2, TRequiredOutput>(Func<TRequiredInput1, TRequiredInput2, TRequiredOutput, TIn1> selector1, Func<TRequiredInput1, TRequiredInput2, TRequiredOutput, TIn2> selector2, Func<TRequiredInput1, TRequiredInput2, TRequiredOutput, bool> filter)
        {
            if (selector1 == null) throw new ArgumentNullException("selector1");
            if (selector2 == null) throw new ArgumentNullException("selector2");

            CallForInternal(new Type[] { typeof(TRequiredInput1), typeof(TRequiredInput2) }, c =>
            {
                return CallFilterOutputSensitive<TRequiredInput1, TRequiredInput2, TRequiredOutput>(filter, c);
            }, c =>
            {
                return CallDependencySelectOutputSensitive<TRequiredInput1, TRequiredInput2, TRequiredOutput>(selector1, selector2, c);
            }, null, true);
        }

        /// <summary>
        /// Specify that the current transformation rule is called whenenver objects are transformed with the given transformation rule
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input type parameter of the transformation rule acting as trigger</typeparam>
        /// <typeparam name="TRequiredInput2">The second input type parameter of the transformation rule acting as trigger</typeparam>
        /// <typeparam name="TRequiredOutput">The output type parameter of the transformation rule acting as trigger</typeparam>
        /// <param name="selector1">A method that selects the first input parameter for the current transformation rule</param>
        /// <param name="selector2">A method that selects the second input parameter for the current transformation rule</param>
        /// <param name="rule">The transformation rule that act as trigger for the current transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector1 parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector2 parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the rule parameter is passed a null instance</exception>
        public void CallForOutputSensitive<TRequiredInput1, TRequiredInput2, TRequiredOutput>(TransformationRuleBase<TRequiredInput1, TRequiredInput2, TRequiredOutput> rule, Func<TRequiredInput1, TRequiredInput2, TRequiredOutput, TIn1> selector1, Func<TRequiredInput1, TRequiredInput2, TRequiredOutput, TIn2> selector2)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
            where TRequiredOutput : class
        {
            CallForOutputSensitive<TRequiredInput1, TRequiredInput2, TRequiredOutput>(rule, selector1, selector2, null);
        }

        /// <summary>
        /// Specify that the current transformation rule is called whenenver objects are transformed with the given transformation rule
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input type parameter of the transformation rule acting as trigger</typeparam>
        /// <typeparam name="TRequiredInput2">The second input type parameter of the transformation rule acting as trigger</typeparam>
        /// <typeparam name="TRequiredOutput">The output type parameter of the transformation rule acting as trigger</typeparam>
        /// <param name="selector1">A method that selects the first input parameter for the current transformation rule</param>
        /// <param name="selector2">A method that selects the second input parameter for the current transformation rule</param>
        /// <param name="filter">A method to filter the objects where the reversed dependency is applicable</param>
        /// <param name="rule">The transformation rule that act as trigger for the current transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector1 parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector2 parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the rule parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public void CallForOutputSensitive<TRequiredInput1, TRequiredInput2, TRequiredOutput>(TransformationRuleBase<TRequiredInput1, TRequiredInput2, TRequiredOutput> rule, Func<TRequiredInput1, TRequiredInput2, TRequiredOutput, TIn1> selector1, Func<TRequiredInput1, TRequiredInput2, TRequiredOutput, TIn2> selector2, Func<TRequiredInput1, TRequiredInput2, TRequiredOutput, bool> filter)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
            where TRequiredOutput : class
        {
            if (selector1 == null) throw new ArgumentNullException("selector1");
            if (selector2 == null) throw new ArgumentNullException("selector2");

            CallForInternal(rule, c =>
            {
                return CallFilterOutputSensitive<TRequiredInput1, TRequiredInput2, TRequiredOutput>(filter, c);
            }, c =>
            {
                return CallDependencySelectOutputSensitive<TRequiredInput1, TRequiredInput2, TRequiredOutput>(selector1, selector2, c);
            }, null, true);
        }

        /// <summary>
        /// Specify that the current transformation rule is called whenenver the given types are transformed
        /// </summary>
        /// <typeparam name="TRequiredInput">The input type of the transformation rule acting as trigger</typeparam>
        /// <typeparam name="TRequiredOutput">The output type parameter of the transformation rule acting as trigger</typeparam>
        /// <param name="selector1">A method that selects the first input parameter for the current transformation rule</param>
        /// <param name="selector2">A method that selects the second input parameter for the current transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector1 parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector2 parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void CallForOutputSensitive<TRequiredInput, TRequiredOutput>(Func<TRequiredInput, TRequiredOutput, TIn1> selector1, Func<TRequiredInput, TRequiredOutput, TIn2> selector2)
        {
            CallForOutputSensitive<TRequiredInput, TRequiredOutput>(selector1, selector2, null);
        }

        /// <summary>
        /// Specify that the current transformation rule is called whenenver the given types are transformed
        /// </summary>
        /// <typeparam name="TRequiredInput">The input type of the transformation rule acting as trigger</typeparam>
        /// <typeparam name="TRequiredOutput">The output type parameter of the transformation rule acting as trigger</typeparam>
        /// <param name="selector1">A method that selects the first input parameter for the current transformation rule</param>
        /// <param name="selector2">A method that selects the second input parameter for the current transformation rule</param>
        /// <param name="filter">A method that filters the objects where the dependency should be applied</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector1 parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector2 parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void CallForOutputSensitive<TRequiredInput, TRequiredOutput>(Func<TRequiredInput, TRequiredOutput, TIn1> selector1, Func<TRequiredInput, TRequiredOutput, TIn2> selector2, Func<TRequiredInput, TRequiredOutput, bool> filter)
        {
            if (selector1 == null) throw new ArgumentNullException("selector1");
            if (selector2 == null) throw new ArgumentNullException("selector2");

            CallForInternal(new Type[] { typeof(TRequiredInput) }, c =>
            {
                return CallFilterOutputSensitive<TRequiredInput, TRequiredOutput>(filter, c);
            }, c =>
            {
                return CallDependencySelectOutputSensitive<TRequiredInput, TRequiredOutput>(selector1, selector2, c);
            }, null, true);
        }

        /// <summary>
        /// Specify that the current transformation rule is called whenenver objects are transformed with the given transformation rule
        /// </summary>
        /// <typeparam name="TRequiredInput">The input type of the transformation rule acting as trigger</typeparam>
        /// <typeparam name="TRequiredOutput">The output type parameter of the transformation rule acting as trigger</typeparam>
        /// <param name="selector1">A method that selects the first input parameter for the current transformation rule</param>
        /// <param name="selector2">A method that selects the second input parameter for the current transformation rule</param>
        /// <param name="rule">The transformation rule that act as trigger for the current transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector1 parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector2 parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the rule parameter is passed a null instance</exception>
        public void CallForOutputSensitive<TRequiredInput, TRequiredOutput>(TransformationRuleBase<TRequiredInput, TRequiredOutput> rule, Func<TRequiredInput, TRequiredOutput, TIn1> selector1, Func<TRequiredInput, TRequiredOutput, TIn2> selector2)
            where TRequiredInput : class
            where TRequiredOutput : class
        {
            CallForOutputSensitive<TRequiredInput, TRequiredOutput>(rule, selector1, selector2, null);
        }

        /// <summary>
        /// Specify that the current transformation rule is called whenenver objects are transformed with the given transformation rule
        /// </summary>
        /// <typeparam name="TRequiredInput">The input type of the transformation rule acting as trigger</typeparam>
        /// <typeparam name="TRequiredOutput">The output type parameter of the transformation rule acting as trigger</typeparam>
        /// <param name="selector1">A method that selects the first input parameter for the current transformation rule</param>
        /// <param name="selector2">A method that selects the second input parameter for the current transformation rule</param>
        /// <param name="filter">A method to filter the objects where the reversed dependency is applicable</param>
        /// <param name="rule">The transformation rule that act as trigger for the current transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector1 parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector2 parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the rule parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public void CallForOutputSensitive<TRequiredInput, TRequiredOutput>(TransformationRuleBase<TRequiredInput, TRequiredOutput> rule, Func<TRequiredInput, TRequiredOutput, TIn1> selector1, Func<TRequiredInput, TRequiredOutput, TIn2> selector2, Func<TRequiredInput, TRequiredOutput, bool> filter)
            where TRequiredInput : class
            where TRequiredOutput : class
        {
            if (selector1 == null) throw new ArgumentNullException("selector1");
            if (selector2 == null) throw new ArgumentNullException("selector2");

            CallForInternal(rule, c =>
            {
                return CallFilterOutputSensitive<TRequiredInput, TRequiredOutput>(filter, c);
            }, c =>
            {
                return CallDependencySelectOutputSensitive<TRequiredInput, TRequiredOutput>(selector1, selector2, c);
            }, null, true);
        }

        /// <summary>
        /// Specify that the current transformation rule is called whenenver the given types are transformed multiple times
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input type parameter of the transformation rule acting as trigger</typeparam>
        /// <typeparam name="TRequiredInput2">The second input type parameter of the transformation rule acting as trigger</typeparam>
        /// <param name="selector">A method that selects the input parameters for the current transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        public void CallForEach<TRequiredInput1, TRequiredInput2>(Func<TRequiredInput1, TRequiredInput2, IEnumerable<Tuple<TIn1, TIn2>>> selector)
        {
            CallForEach<TRequiredInput1, TRequiredInput2>(selector, (s1, s2) => true);
        }

        /// <summary>
        /// Specify that the current transformation rule is called whenenver the given types are transformed multiple times
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input type parameter of the transformation rule acting as trigger</typeparam>
        /// <typeparam name="TRequiredInput2">The second input type parameter of the transformation rule acting as trigger</typeparam>
        /// <param name="selector">A method that selects the input parameters for the current transformation rule</param>
        /// <param name="filter">A method that filters the inputs of the transformation rule acting as trigger</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        public void CallForEach<TRequiredInput1, TRequiredInput2>(Func<TRequiredInput1, TRequiredInput2, IEnumerable<Tuple<TIn1, TIn2>>> selector, Func<TRequiredInput1, TRequiredInput2, bool> filter)
        {
            if (selector == null) throw new ArgumentNullException("selector");
            CallForEachInternal(new Type[] { typeof(TRequiredInput1), typeof(TRequiredInput2) }, c =>
            {
                return CallFilter(filter, c);
            }, o =>
            {
                return CallDependencySelectMany<TRequiredInput1, TRequiredInput2>(selector, o);
            }, null, false);
        }

        /// <summary>
        /// Specify that the current transformation rule is called whenenver the given types are transformed multiple times
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input type parameter of the transformation rule acting as trigger</typeparam>
        /// <typeparam name="TRequiredInput2">The second input type parameter of the transformation rule acting as trigger</typeparam>
        /// <param name="selector">A method that selects the input parameters for the current transformation rule</param>
        /// <param name="rule">The transformation rule acting as trigger for the current transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the rule parameter is passed a null instance</exception>
        public void CallForEach<TRequiredInput1, TRequiredInput2>(GeneralTransformationRule<TRequiredInput1, TRequiredInput2> rule, Func<TRequiredInput1, TRequiredInput2, IEnumerable<Tuple<TIn1, TIn2>>> selector)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
        {
            CallForEach<TRequiredInput1, TRequiredInput2>(rule, selector, (s1, s2) => true);
        }

        /// <summary>
        /// Specify that the current transformation rule is called whenenver the given types are transformed multiple times
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input type parameter of the transformation rule acting as trigger</typeparam>
        /// <typeparam name="TRequiredInput2">The second input type parameter of the transformation rule acting as trigger</typeparam>
        /// <param name="selector">A method that selects the input parameters for the current transformation rule</param>
        /// <param name="filter">A method that filters the inputs of the transformation rule acting as trigger</param>
        /// <param name="rule">The transformation rule acting as trigger for the current transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the rule parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public void CallForEach<TRequiredInput1, TRequiredInput2>(GeneralTransformationRule<TRequiredInput1, TRequiredInput2> rule, Func<TRequiredInput1, TRequiredInput2, IEnumerable<Tuple<TIn1, TIn2>>> selector, Func<TRequiredInput1, TRequiredInput2, bool> filter)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            CallForEachInternal(rule, c =>
            {
                return CallFilter(filter, c);
            }, c =>
            {
                return CallDependencySelectMany<TRequiredInput1, TRequiredInput2>(selector, c);
            }, null, false);
        }


        /// <summary>
        /// Specify that the current transformation rule is called whenenver the given types are transformed multiple times
        /// </summary>
        /// <typeparam name="TRequiredInput">The input type parameter of the transformation rule acting as trigger</typeparam>
        /// <param name="selector">A method that selects the input parameters for the current transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        public void CallForEach<TRequiredInput>(Func<TRequiredInput, IEnumerable<Tuple<TIn1, TIn2>>> selector)
        {
            CallForEach<TRequiredInput>(selector, s => true);
        }

        /// <summary>
        /// Specify that the current transformation rule is called whenenver the given types are transformed multiple times
        /// </summary>
        /// <typeparam name="TRequiredInput">The input type parameter of the transformation rule acting as trigger</typeparam>
        /// <param name="selector">A method that selects the input parameters for the current transformation rule</param>
        /// <param name="filter">A method that filters the inputs of the transformation rule acting as trigger</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        public void CallForEach<TRequiredInput>(Func<TRequiredInput, IEnumerable<Tuple<TIn1, TIn2>>> selector, Func<TRequiredInput, bool> filter)
        {
            if (selector == null) throw new ArgumentNullException("selector");
            CallForEachInternal(new Type[] { typeof(TRequiredInput) }, c =>
            {
                return CallFilter(filter, c);
            }, c =>
            {
                return CallDependencySelectMany<TRequiredInput>(selector, c);
            }, null, false);
        }

        /// <summary>
        /// Specify that the current transformation rule is called whenenver the given types are transformed multiple times
        /// </summary>
        /// <typeparam name="TRequiredInput">The input type parameter of the transformation rule acting as trigger</typeparam>
        /// <param name="selector">A method that selects the input parameters for the current transformation rule</param>
        /// <param name="rule">The transformation rule acting as trigger for the current transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the rule parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        public void CallForEach<TRequiredInput>(GeneralTransformationRule<TRequiredInput> rule, Func<TRequiredInput, IEnumerable<Tuple<TIn1, TIn2>>> selector)
            where TRequiredInput : class
        {
            CallForEach<TRequiredInput>(rule, selector, null);
        }

        /// <summary>
        /// Specify that the current transformation rule is called whenenver the given types are transformed multiple times
        /// </summary>
        /// <typeparam name="TRequiredInput">The input type parameter of the transformation rule acting as trigger</typeparam>
        /// <param name="selector">A method that selects the input parameters for the current transformation rule</param>
        /// <param name="filter">A method that filters the inputs of the transformation rule acting as trigger</param>
        /// <param name="rule">The transformation rule acting as trigger for the current transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the rule parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public void CallForEach<TRequiredInput>(GeneralTransformationRule<TRequiredInput> rule, Func<TRequiredInput, IEnumerable<Tuple<TIn1, TIn2>>> selector, Func<TRequiredInput, bool> filter)
            where TRequiredInput : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            CallForEachInternal(rule, c =>
            {
                return CallFilter(filter, c);
            }, c =>
            {
                return CallDependencySelectMany<TRequiredInput>(selector, c);
            }, null, false);
        }

        /// <summary>
        /// Specify that the current transformation rule is called whenenver the given types are transformed multiple times
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input type parameter of the transformation rule acting as trigger</typeparam>
        /// <typeparam name="TRequiredInput2">The second input type parameter of the transformation rule acting as trigger</typeparam>
        /// <typeparam name="TRequiredOutput">The output type parameter of the transformation rule acting as trigger</typeparam>
        /// <param name="selector">A method that selects the input parameters for the current transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        public void CallForEachOutputSensitive<TRequiredInput1, TRequiredInput2, TRequiredOutput>(Func<TRequiredInput1, TRequiredInput2, TRequiredOutput, IEnumerable<Tuple<TIn1, TIn2>>> selector)
        {
            CallForEachOutputSensitive<TRequiredInput1, TRequiredInput2, TRequiredOutput>(selector, null);
        }

        /// <summary>
        /// Specify that the current transformation rule is called whenenver the given types are transformed multiple times
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input type parameter of the transformation rule acting as trigger</typeparam>
        /// <typeparam name="TRequiredInput2">The second input type parameter of the transformation rule acting as trigger</typeparam>
        /// <typeparam name="TRequiredOutput">The output type parameter of the transformation rule acting as trigger</typeparam>
        /// <param name="selector">A method that selects the input parameters for the current transformation rule</param>
        /// <param name="filter">A method that filters the inputs of the transformation rule acting as trigger</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        public void CallForEachOutputSensitive<TRequiredInput1, TRequiredInput2, TRequiredOutput>(Func<TRequiredInput1, TRequiredInput2, TRequiredOutput, IEnumerable<Tuple<TIn1, TIn2>>> selector, Func<TRequiredInput1, TRequiredInput2, TRequiredOutput, bool> filter)
        {
            if (selector == null) throw new ArgumentNullException("selector");
            CallForEachInternal(new Type[] { typeof(TRequiredInput1), typeof(TRequiredInput2) }, c =>
            {
                return CallFilterOutputSensitive(filter, c);
            }, o =>
            {
                return CallDependencySelectManyOutputSensitive<TRequiredInput1, TRequiredInput2, TRequiredOutput>(selector, o);
            }, null, true);
        }

        /// <summary>
        /// Specify that the current transformation rule is called whenenver the given types are transformed multiple times
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input type parameter of the transformation rule acting as trigger</typeparam>
        /// <typeparam name="TRequiredInput2">The second input type parameter of the transformation rule acting as trigger</typeparam>
        /// <typeparam name="TRequiredOutput">The output type parameter of the transformation rule acting as trigger</typeparam>
        /// <param name="selector">A method that selects the input parameters for the current transformation rule</param>
        /// <param name="rule">The transformation rule acting as trigger for the current transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the rule parameter is passed a null instance</exception>
        public void CallForEachOutputSensitive<TRequiredInput1, TRequiredInput2, TRequiredOutput>(TransformationRuleBase<TRequiredInput1, TRequiredInput2, TRequiredOutput> rule, Func<TRequiredInput1, TRequiredInput2, TRequiredOutput, IEnumerable<Tuple<TIn1, TIn2>>> selector)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
            where TRequiredOutput : class
        {
            CallForEachOutputSensitive<TRequiredInput1, TRequiredInput2, TRequiredOutput>(rule, selector, null);
        }

        /// <summary>
        /// Specify that the current transformation rule is called whenenver the given types are transformed multiple times
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input type parameter of the transformation rule acting as trigger</typeparam>
        /// <typeparam name="TRequiredInput2">The second input type parameter of the transformation rule acting as trigger</typeparam>
        /// <typeparam name="TRequiredOutput">The output type parameter of the transformation rule acting as trigger</typeparam>
        /// <param name="selector">A method that selects the input parameters for the current transformation rule</param>
        /// <param name="filter">A method that filters the inputs of the transformation rule acting as trigger</param>
        /// <param name="rule">The transformation rule acting as trigger for the current transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the rule parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public void CallForEachOutputSensitive<TRequiredInput1, TRequiredInput2, TRequiredOutput>(TransformationRuleBase<TRequiredInput1, TRequiredInput2, TRequiredOutput> rule, Func<TRequiredInput1, TRequiredInput2, TRequiredOutput, IEnumerable<Tuple<TIn1, TIn2>>> selector, Func<TRequiredInput1, TRequiredInput2, TRequiredOutput, bool> filter)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
            where TRequiredOutput : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            CallForEachInternal(rule, c =>
            {
                return CallFilterOutputSensitive(filter, c);
            }, c =>
            {
                return CallDependencySelectManyOutputSensitive<TRequiredInput1, TRequiredInput2, TRequiredOutput>(selector, c);
            }, null, true);
        }


        /// <summary>
        /// Specify that the current transformation rule is called whenenver the given types are transformed multiple times
        /// </summary>
        /// <typeparam name="TRequiredInput">The input type parameter of the transformation rule acting as trigger</typeparam>
        /// <typeparam name="TRequiredOutput">The output type parameter of the transformation rule acting as trigger</typeparam>
        /// <param name="selector">A method that selects the input parameters for the current transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        public void CallForEachOutputSensitive<TRequiredInput, TRequiredOutput>(Func<TRequiredInput, TRequiredOutput, IEnumerable<Tuple<TIn1, TIn2>>> selector)
        {
            CallForEachOutputSensitive<TRequiredInput, TRequiredOutput>(selector, null);
        }

        /// <summary>
        /// Specify that the current transformation rule is called whenenver the given types are transformed multiple times
        /// </summary>
        /// <typeparam name="TRequiredInput">The input type parameter of the transformation rule acting as trigger</typeparam>
        /// <typeparam name="TRequiredOutput">The output type parameter of the transformation rule acting as trigger</typeparam>
        /// <param name="selector">A method that selects the input parameters for the current transformation rule</param>
        /// <param name="filter">A method that filters the inputs of the transformation rule acting as trigger</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        public void CallForEachOutputSensitive<TRequiredInput, TRequiredOutput>(Func<TRequiredInput, TRequiredOutput, IEnumerable<Tuple<TIn1, TIn2>>> selector, Func<TRequiredInput, TRequiredOutput, bool> filter)
        {
            if (selector == null) throw new ArgumentNullException("selector");
            CallForEachInternal(new Type[] { typeof(TRequiredInput) }, c =>
            {
                return CallFilterOutputSensitive(filter, c);
            }, c =>
            {
                return CallDependencySelectManyOutputSensitive<TRequiredInput, TRequiredOutput>(selector, c);
            }, null, true);
        }

        /// <summary>
        /// Specify that the current transformation rule is called whenenver the given types are transformed multiple times
        /// </summary>
        /// <typeparam name="TRequiredInput">The input type parameter of the transformation rule acting as trigger</typeparam>
        /// <typeparam name="TRequiredOutput">The output type parameter of the transformation rule acting as trigger</typeparam>
        /// <param name="selector">A method that selects the input parameters for the current transformation rule</param>
        /// <param name="rule">The transformation rule acting as trigger for the current transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the rule parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        public void CallForEachOutputSensitive<TRequiredInput, TRequiredOutput>(TransformationRuleBase<TRequiredInput, TRequiredOutput> rule, Func<TRequiredInput, TRequiredOutput, IEnumerable<Tuple<TIn1, TIn2>>> selector)
            where TRequiredInput : class
            where TRequiredOutput : class
        {
            CallForEachOutputSensitive<TRequiredInput, TRequiredOutput>(rule, selector, null);
        }

        /// <summary>
        /// Specify that the current transformation rule is called whenenver the given types are transformed multiple times
        /// </summary>
        /// <typeparam name="TRequiredInput">The input type parameter of the transformation rule acting as trigger</typeparam>
        /// <typeparam name="TRequiredOutput">The output type parameter of the transformation rule acting as trigger</typeparam>
        /// <param name="selector">A method that selects the input parameters for the current transformation rule</param>
        /// <param name="filter">A method that filters the inputs of the transformation rule acting as trigger</param>
        /// <param name="rule">The transformation rule acting as trigger for the current transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the rule parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public void CallForEachOutputSensitive<TRequiredInput, TRequiredOutput>(TransformationRuleBase<TRequiredInput, TRequiredOutput> rule, Func<TRequiredInput, TRequiredOutput, IEnumerable<Tuple<TIn1, TIn2>>> selector, Func<TRequiredInput, TRequiredOutput, bool> filter)
            where TRequiredInput : class
            where TRequiredOutput : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            CallForEachInternal(rule, c =>
            {
                return CallFilterOutputSensitive(filter, c);
            }, c =>
            {
                return CallDependencySelectManyOutputSensitive<TRequiredInput, TRequiredOutput>(selector, c);
            }, null, true);
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
        public void CallFor(GeneralTransformationRule rule, Func<Computation, Tuple<TIn1, TIn2>> selector, bool needOutput)
        {
            CallFor(rule, selector, null, needOutput);
        }

        /// <summary>
        /// Specify that the current transformation rule is called whenenver objects are transformed with the given transformation rule
        /// </summary>
        /// <param name="selector">A method that selects the input parameters for the current transformation rule</param>
        /// <param name="filter">A method to filter the objects where the reversed dependency is applicable</param>
        /// <param name="rule">The transformation rule that act as trigger for the current transformation rule</param>
        /// <param name="needOutput">True, if the call must be made after the output of the trigger rule is created, otherwise false</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the rule parameter is passed a null instance</exception>
        public void CallFor(GeneralTransformationRule rule, Func<Computation, Tuple<TIn1, TIn2>> selector, Predicate<Computation> filter, bool needOutput)
        {
            CallForInternal(rule, filter, c =>
            {
                var tuple = selector(c);
                return new object[] { tuple.Item1, tuple.Item2 };
            }, null, needOutput);
        }

        /// <summary>
        /// Specify that the current transformation rule is called whenenver the given types are transformed multiple times
        /// </summary>
        /// <param name="selector">A method that selects the input parameters for the current transformation rule</param>
        /// <param name="rule">The transformation rule acting as trigger for the current transformation rule</param>
        /// <param name="needOutput">True, if the call must be made after the output of the trigger rule is created, otherwise false</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the rule parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        public void CallForEach(GeneralTransformationRule rule, Func<Computation, IEnumerable<Tuple<TIn1, TIn2>>> selector, bool needOutput)
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
        public void CallForEach(GeneralTransformationRule rule, Func<Computation, IEnumerable<Tuple<TIn1, TIn2>>> selector, Predicate<Computation> filter, bool needOutput)
        {
            CallForEachInternal(rule, filter, c => selector(c).Select(t => new object[] { t.Item1, t.Item2 }), null, needOutput);
        }

#endregion


        /// <summary>
        /// Requires all transformation rules that transform S to T with all of the specified objects
        /// </summary>
        /// <param name="rule">The dependent transformation rule</param>
        /// <param name="selector">A method that selects the inputs for the dependent computations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the rule parameter is passed a null instance</exception>
        public ITransformationRuleDependency RequireMany(GeneralTransformationRule rule, Func<TIn1, TIn2, IEnumerable<object[]>> selector)
        {
            if (selector == null) throw new ArgumentNullException("selector");
            return DependMany(null, c => selector(c.GetInput(0) as TIn1, c.GetInput(1) as TIn2), rule, null, true, false);
        }

        /// <summary>
        /// Calls the transformation rule with the given type after the current transformation rule
        /// </summary>
        /// <param name="rule">The dependent transformation rule</param>
        /// <param name="selector">A method that selects the input for the dependent computations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the rule parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        public ITransformationRuleDependency Call(GeneralTransformationRule rule, Func<TIn1, TIn2, object[]> selector)
        {
            if (selector == null) throw new ArgumentNullException("selector");
            return Depend(null, c => selector(c.GetInput(0) as TIn1, c.GetInput(1) as TIn2), rule, null, false, false);
        }


        /// <summary>
        /// Calls the transformation rule with the given type with all of the specified inputs after the current transformation rule
        /// </summary>
        /// <param name="rule">The dependent transformation rule</param>
        /// <param name="selector">A method that selects the inputs for the dependent computations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the rule parameter is passed a null instance</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the selector parameter is passed a null instance</exception>
        public ITransformationRuleDependency CallMany(GeneralTransformationRule rule, Func<TIn1, TIn2, IEnumerable<object[]>> selector)
        {
            if (selector == null) throw new ArgumentNullException("selector");
            return DependMany(null, c => selector(c.GetInput(0) as TIn1, c.GetInput(1) as TIn2), rule, null, false, false);
        }

        #endregion

        #region Helpers


        internal static IEnumerable<object[]> CallDependencySelectMany<TRequiredInput>(Func<TRequiredInput, IEnumerable<Tuple<TIn1, TIn2>>> selector, Computation computation)
        {
            if (computation != null && computation.InputArguments == 1)
            {
                var t1 = (TRequiredInput)computation.GetInput(0);
                var tc = selector(t1);
                if (tc != null) return tc.Select(tuple => new object[] { tuple.Item1, tuple.Item2 });
            }
            return null;
        }


        internal static IEnumerable<object[]> CallDependencySelectMany<TRequiredInput1, TRequiredInput2>(Func<TRequiredInput1, TRequiredInput2, IEnumerable<Tuple<TIn1, TIn2>>> selector, Computation computation)
        {
            if (computation != null && computation.InputArguments == 2)
            {
                var t1 = (TRequiredInput1)computation.GetInput(0);
                var t2 = (TRequiredInput2)computation.GetInput(1);
                var tc = selector(t1, t2);
                if (tc != null) return tc.Select(tuple => new object[] { tuple.Item1, tuple.Item2 });
            }
            return null;
        }


        internal static IEnumerable<object[]> CallDependencySelectManyOutputSensitive<TRequiredInput, TRequiredOutput>(Func<TRequiredInput, TRequiredOutput, IEnumerable<Tuple<TIn1, TIn2>>> selector, Computation computation)
        {
            if (computation != null && computation.InputArguments == 1)
            {
                var t1 = (TRequiredInput)computation.GetInput(0);
                var tc = selector(t1, (TRequiredOutput)computation.Output);
                if (tc != null) return tc.Select(tuple => new object[] { tuple.Item1, tuple.Item2 });
            }
            return null;
        }


        internal static IEnumerable<object[]> CallDependencySelectManyOutputSensitive<TRequiredInput1, TRequiredInput2, TRequiredOutput>(Func<TRequiredInput1, TRequiredInput2, TRequiredOutput, IEnumerable<Tuple<TIn1, TIn2>>> selector, Computation computation)
        {
            if (computation != null && computation.InputArguments == 2)
            {
                var t1 = (TRequiredInput1)computation.GetInput(0);
                var t2 = (TRequiredInput2)computation.GetInput(1);
                var tc = selector(t1, t2, (TRequiredOutput)computation.Output);
                if (tc != null) return tc.Select(tuple => new object[] { tuple.Item1, tuple.Item2 });
            }
            return null;
        }

        internal static object[] CallDependencySelect<TRequiredInput1, TRequiredInput2>(Func<TRequiredInput1, TRequiredInput2, TIn1> selector1, Func<TRequiredInput1, TRequiredInput2, TIn2> selector2, Computation computation)
        {
            if (computation != null && computation.InputArguments == 2)
            {
                var t1 = (TRequiredInput1)computation.GetInput(0);
                var t2 = (TRequiredInput2)computation.GetInput(1);
                return new object[] {
                        selector1(t1, t2),
                        selector2(t1, t2)
                    };
            }
            else
            {
                return null;
            }
        }

        internal static object[] CallDependencySelect<TRequiredInput>(Func<TRequiredInput, TIn1> selector1, Func<TRequiredInput, TIn2> selector2, Computation computation)
        {
            if (computation != null && computation.InputArguments == 1)
            {
                var t1 = (TRequiredInput)computation.GetInput(0);
                return new object[] {
                        selector1(t1),
                        selector2(t1)
                    };
            }
            else
            {
                return null;
            }
        }

        internal static object[] CallDependencySelectOutputSensitive<TRequiredInput1, TRequiredInput2, TRequiredOutput>(Func<TRequiredInput1, TRequiredInput2, TRequiredOutput, TIn1> selector1, Func<TRequiredInput1, TRequiredInput2, TRequiredOutput, TIn2> selector2, Computation computation)
        {
            if (computation != null && computation.InputArguments == 2)
            {
                var t1 = (TRequiredInput1)computation.GetInput(0);
                var t2 = (TRequiredInput2)computation.GetInput(1);
                var to = (TRequiredOutput)computation.Output;
                return new object[] {
                        selector1(t1, t2, to),
                        selector2(t1, t2, to)
                    };
            }
            else
            {
                return null;
            }
        }

        internal static object[] CallDependencySelectOutputSensitive<TRequiredInput, TRequiredOutput>(Func<TRequiredInput, TRequiredOutput, TIn1> selector1, Func<TRequiredInput, TRequiredOutput, TIn2> selector2, Computation computation)
        {
            if (computation != null && computation.InputArguments == 1)
            {
                var t1 = (TRequiredInput)computation.GetInput(0);
                var to = (TRequiredOutput)computation.Output;
                return new object[] {
                        selector1(t1, to),
                        selector2(t1, to)
                    };
            }
            else
            {
                return null;
            }
        }

        internal static bool CallFilter<TRequiredInput1, TRequiredInput2>(Func<TRequiredInput1, TRequiredInput2, bool> filter, Computation computation)
        {
            if (computation != null && computation.InputArguments == 2)
            {
                var t1 = (TRequiredInput1)computation.GetInput(0);
                var t2 = (TRequiredInput2)computation.GetInput(1);
                return filter(t1, t2);
            }
            else
            {
                return false;
            }
        }

        internal static bool CallFilter<TRequiredInput>(Func<TRequiredInput, bool> filter, Computation computation)
        {
            if (computation != null && computation.InputArguments == 1)
            {
                var t1 = (TRequiredInput)computation.GetInput(0);
                return filter(t1);
            }
            else
            {
                return false;
            }
        }

        internal static bool CallFilterOutputSensitive<TRequiredInput1, TRequiredInput2, TRequiredOutput>(Func<TRequiredInput1, TRequiredInput2, TRequiredOutput, bool> filter, Computation computation)
        {
            if (computation != null && computation.InputArguments == 2)
            {
                var t1 = (TRequiredInput1)computation.GetInput(0);
                var t2 = (TRequiredInput2)computation.GetInput(1);
                return filter(t1, t2, (TRequiredOutput)computation.Output);
            }
            else
            {
                return false;
            }
        }

        internal static bool CallFilterOutputSensitive<TRequiredInput, TRequiredOutput>(Func<TRequiredInput, TRequiredOutput, bool> filter, Computation computation)
        {
            if (computation != null && computation.InputArguments == 1)
            {
                var t1 = (TRequiredInput)computation.GetInput(0);
                return filter(t1, (TRequiredOutput)computation.Output);
            }
            else
            {
                return false;
            }
        }

        internal static object[] DependencySelect<TRequiredInput1, TRequiredInput2>(Func<TIn1, TIn2, TRequiredInput1> selector1, Func<TIn1, TIn2, TRequiredInput2> selector2, Computation computation)
        {
            if (computation != null && computation.InputArguments == 2)
            {
                var t1 = (TIn1)computation.GetInput(0);
                var t2 = (TIn2)computation.GetInput(1);
                return new object[] {selector1(t1, t2),
                        selector2(t1, t2)};
            }
            else
            {
                return null;
            }
        }

        internal static object[] DependencySelect<TRequiredInput>(Func<TIn1, TIn2, TRequiredInput> selector, Computation computation)
        {
            if (computation != null && computation.InputArguments == 2)
            {
                var t1 = (TIn1)computation.GetInput(0);
                var t2 = (TIn2)computation.GetInput(1);
                return new object[] {selector(t1, t2)};
            }
            else
            {
                return null;
            }
        }


        internal static IEnumerable<object[]> DependencySelectMany<TRequiredInput1, TRequiredInput2>(Func<TIn1, TIn2, IEnumerable<Tuple<TRequiredInput1, TRequiredInput2>>> selector, Computation computation)
        {
            if (computation != null && computation.InputArguments == 2)
            {
                var t1 = (TIn1)computation.GetInput(0);
                var t2 = (TIn2)computation.GetInput(1);
                var tc = selector(t1, t2);
                if (tc != null) return tc.Select(tuple => new object[] { tuple.Item1, tuple.Item2 });
            }
            return null;
        }


        internal static IEnumerable<object[]> DependencySelectMany<TRequiredInput>(Func<TIn1, TIn2, IEnumerable<TRequiredInput>> selector, Computation computation)
        {
            if (computation != null && computation.InputArguments == 2)
            {
                var t1 = (TIn1)computation.GetInput(0);
                var t2 = (TIn2)computation.GetInput(1);
                var tc = selector(t1, t2);
                if (tc != null) return tc.Select(tuple => new object[] { tuple });
            }
            return null;
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
