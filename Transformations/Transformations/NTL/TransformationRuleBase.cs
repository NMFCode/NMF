using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMF.Utilities;
using NMF.Transformations.Properties;
using System.Collections;
using NMF.Transformations.Core;

namespace NMF.Transformations
{
    /// <summary>
    /// Defines a transformation rule of a transformation that has one input argument and an output
    /// </summary>
    /// <typeparam name="TIn">The type of the input argument</typeparam>
    /// <typeparam name="TOut">The type of the output</typeparam>
    public abstract class TransformationRuleBase<TIn, TOut> : GeneralTransformationRule<TIn>
        where TIn : class
        where TOut : class
    {
        /// <summary>
        /// Marks the current transformation rule instantiating for every rule with the specified signature
        /// </summary>
        /// <typeparam name="TBaseIn">The input argument type of the base transformation rule</typeparam>
        /// <typeparam name="TBaseOut">The output argument type of the base transformation rule</typeparam>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void MarkInstantiatingFor<TBaseIn, TBaseOut>()
            where TBaseIn : class
            where TBaseOut : class
        {
            MarkInstantiatingFor<TBaseIn, TBaseOut>(null);
        }
        
        /// <summary>
        /// Marks the current transformation rule instantiating for every rule with the specified signature
        /// </summary>
        /// <param name="filter">The filter that should be used to filter the inputs</param>
        /// <typeparam name="TBaseIn">The input argument type of the base transformation rule</typeparam>
        /// <typeparam name="TBaseOut">The output argument type of the base transformation rule</typeparam>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void MarkInstantiatingFor<TBaseIn, TBaseOut>(Predicate<TIn> filter)
            where TBaseIn : class
            where TBaseOut : class
        {
            var inputTypes = new Type[] { typeof(TBaseIn) };
            var outputType = typeof(TBaseOut);
            if (inputTypes.IsAssignableArrayFrom(InputType) && (outputType == OutputType || outputType.IsAssignableFrom(OutputType)))
            {
                var rule = Transformation.GetRulesExact(inputTypes, outputType).FirstOrDefault();
                if (rule != null)
                {
                    if (filter != null)
                    {
                        MarkInstantiatingFor(rule, c => HasCompliantInput(c) && filter(c.GetInput(0) as TIn));
                    }
                    else
                    {
                        MarkInstantiatingFor(rule);
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("No suitable rule found to instantiate!");
            }
        }

        /// <summary>
        /// Requires all transformation rules that transform items with the specified signature
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformations</typeparam>
        /// <param name="persistor">A method that persists the result of the dependent transformations</param>
        /// <remarks>This version Always takes the input parameter as input for the dependent transformations. Thus, this method will throw an exception, if the types do not match</remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void RequireByType<TRequiredInput, TRequiredOutput>(Action<TOut, TRequiredOutput> persistor)
            where TRequiredInput : class
            where TRequiredOutput : class
        {
            if (typeof(TRequiredInput).IsAssignableFrom(typeof(TIn)) && typeof(TRequiredOutput).IsAssignableFrom(OutputType))
            {
                RequireByType<TRequiredInput, TRequiredOutput>(t => t as TRequiredInput, persistor);
            }
            else
            {
                throw new InvalidOperationException(Resources.ErrRequires1ArgNoSelectorMustInherit);
            }
        }

        /// <summary>
        /// Requires the transformation rule with the given type
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformation</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        /// <param name="persistor">A method that persists the result of the dependent transformation</param>
        /// <remarks>This version Always takes the input parameter as input for the dependent transformations. Thus, this method will throw an exception, if the types do not match</remarks>

        public ITransformationRuleDependency Require<TRequiredInput, TRequiredOutput>(TransformationRuleBase<TRequiredInput, TRequiredOutput> rule, Action<TOut, TRequiredOutput> persistor)
            where TRequiredInput : class
            where TRequiredOutput : class
        {
            return Require<TRequiredInput, TRequiredOutput>(rule, null as Predicate<TIn>, persistor);
        }

        /// <summary>
        /// Requires the transformation rule with the given type
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformation</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        /// <param name="filter">A predicate that filters the data</param>
        /// <param name="persistor">A method that persists the result of the dependent transformation</param>
        /// <remarks>This version Always takes the input parameter as input for the dependent transformations. Thus, this method will throw an exception, if the types do not match</remarks>

        public ITransformationRuleDependency Require<TRequiredInput, TRequiredOutput>(TransformationRuleBase<TRequiredInput, TRequiredOutput> rule, Predicate<TIn> filter, Action<TOut, TRequiredOutput> persistor)
            where TRequiredInput : class
            where TRequiredOutput : class
        {
            if (typeof(TRequiredInput).IsAssignableFrom(typeof(TIn)))
            {
                Predicate<Computation> f = null;
                if (filter != null)
                {
                    f = c =>
                    {
                        var inp = c.GetInput(0) as TIn;
                        return inp != null && filter(inp);
                    };
                }
                return Depend(f, c => c.CreateInputArray(), rule, persistor == null ? null : new Action<object, object>((o1, o2) => persistor(o1 as TOut, o2 as TRequiredOutput)), true, false);
            }
            else
            {
                throw new InvalidOperationException(Resources.ErrRequiresTransNoSelectorMustInherit);
            }
        }

        /// <summary>
        /// Requires all transformation rules that transform items with the specified signature
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformations</typeparam>
        /// <param name="selector">A method that selects the input for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector parameter</exception>
        /// <param name="persistor">A method that persists the result of the dependent transformations</param>
        public void RequireByType<TRequiredInput, TRequiredOutput>(Func<TIn, TRequiredInput> selector, Action<TOut, TRequiredOutput> persistor)
            where TRequiredInput : class
            where TRequiredOutput : class
        {
            if (persistor == null) persistor = (o1, o2) => { };
            if (selector == null) throw new ArgumentNullException("selector");
            Predicate<Computation> filter = c => selector((TIn)c.GetInput(0)) != null;
            foreach (var rule in Transformation.GetRulesForTypeSignature(new Type[] { typeof(TRequiredInput) }, typeof(TRequiredOutput)))
            {
                Depend(filter, c => new object[] { selector((TIn)c.GetInput(0)) }, rule, (s, t) => persistor((TOut)s, (TRequiredOutput)t), true, false);
            }
        }

        /// <summary>
        /// Requires all transformation rules that transform items with the specified signature
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformations</typeparam>
        /// <param name="selector1">A method that selects the first input for the dependent transformations</param>
        /// <param name="selector2">A method that selects the second input for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector1 parameter</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector2 parameter</exception>
        /// <param name="persistor">A method that persists the result of the dependent transformations</param>
        
        public void RequireByType<TRequiredInput1, TRequiredInput2, TRequiredOutput>(Func<TIn, TRequiredInput1> selector1, Func<TIn, TRequiredInput2> selector2, Action<TOut, TRequiredOutput> persistor)
        {
            if (persistor == null) persistor = (o1, o2) => { };
            if (selector1 == null) throw new ArgumentNullException("selector1");
            if (selector2 == null) throw new ArgumentNullException("selector2");
            foreach (var rule in Transformation.GetRulesForTypeSignature(new Type[] { typeof(TRequiredInput1), typeof(TRequiredInput2) }, typeof(TRequiredOutput)))
            {
                Depend(null, c => new object[] { selector1((TIn)c.GetInput(0)), selector2((TIn)c.GetInput(0)) }, rule, (s, t) => persistor((TOut)s, (TRequiredOutput)t), true, false);
            }
        }

        /// <summary>
        /// Requires the transformation rule with the given type
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformation</typeparam>
        /// <param name="selector">A method that selects the input for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector parameter</exception>
        /// <param name="persistor">A method that persists the result of the dependent transformation</param>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency Require<TRequiredInput, TRequiredOutput>(TransformationRuleBase<TRequiredInput, TRequiredOutput> rule, Func<TIn, TRequiredInput> selector, Action<TOut, TRequiredOutput> persistor)
            where TRequiredInput : class
            where TRequiredOutput : class
        {
            if (persistor == null) persistor = (o1, o2) => { };
            if (selector == null) throw new ArgumentNullException("selector");
            return Depend(c => selector((TIn)c.GetInput(0)) != null, c => new object[] { selector((TIn)c.GetInput(0)) }, rule, (s, t) => persistor((TOut)s, (TRequiredOutput)t), true, false);
        }

        /// <summary>
        /// Requires the transformation rule with the given type
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformation</typeparam>
        /// <param name="selector1">A method that selects the first input for the dependent transformations</param>
        /// <param name="selector2">A method that selects the second input for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector1 parameter</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector2 parameter</exception>
        /// <param name="persistor">A method that persists the result of the dependent transformation</param>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency Require<TRequiredInput1, TRequiredInput2, TRequiredOutput>(TransformationRuleBase<TRequiredInput1, TRequiredInput2, TRequiredOutput> rule, Func<TIn, TRequiredInput1> selector1, Func<TIn, TRequiredInput2> selector2, Action<TOut, TRequiredOutput> persistor)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
            where TRequiredOutput : class
        {
            if (persistor == null) persistor = (o1, o2) => { };
            if (selector1 == null) throw new ArgumentNullException("selector1");
            if (selector2 == null) throw new ArgumentNullException("selector2");
            return Depend(null, c => new object[] { selector1((TIn)c.GetInput(0)), selector2((TIn)c.GetInput(0)) }, rule, (s, t) => persistor((TOut)s, (TRequiredOutput)t), true, false);
        }

        /// <summary>
        /// Requires all transformation rules that transform S to T with all of the specified objects
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformations</typeparam>
        /// <param name="selector">A method that selects the inputs for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector parameter</exception>
        /// <param name="persistor">A method that persists the result of the dependent transformation</param>
        public void RequireManyByType<TRequiredInput, TRequiredOutput>(Func<TIn, IEnumerable<TRequiredInput>> selector, Action<TOut, IEnumerable<TRequiredOutput>> persistor)
            where TRequiredInput : class
            where TRequiredOutput : class
        {
            if (persistor == null) persistor = (o1, o2) => { };
            if (selector == null) throw new ArgumentNullException("selector");
            foreach (var rule in Transformation.GetRulesForTypeSignature(new Type[] { typeof(TRequiredInput) }, typeof(TRequiredOutput)))
            {
                DependMany(null, c => SelectArrays(selector, c), rule, (s, t) => persistor((TOut)s, (IEnumerable<TRequiredOutput>)t), true, false);
            }
        }

        /// <summary>
        /// Requires all transformation rules that transform S to T with all of the specified objects
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformation rule</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformation rule</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformations</typeparam>
        /// <param name="selector">A method that selects the inputs for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector parameter</exception>
        /// <param name="persistor">A method that persists the result of the dependent transformation</param>
        public void RequireManyByType<TRequiredInput1, TRequiredInput2, TRequiredOutput>(Func<TIn, IEnumerable<Tuple<TRequiredInput1, TRequiredInput2>>> selector, Action<TOut, IEnumerable<TRequiredOutput>> persistor)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
            where TRequiredOutput : class
        {
            if (persistor == null) persistor = (o1, o2) => { };
            if (selector == null) throw new ArgumentNullException("selector");
            foreach (var rule in Transformation.GetRulesForTypeSignature(new Type[] { typeof(TRequiredInput1), typeof(TRequiredInput2)}, typeof(TRequiredOutput)))
            {
                DependMany(null, c => SelectArraysT2(selector, c), rule, (s, t) => persistor((TOut)s, (IEnumerable<TRequiredOutput>)t), true, false);
            }
        }

        /// <summary>
        /// Requires the transformation rule with the given type with all of the specified inputs
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformation</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        /// <param name="selector">A method that selects the inputs for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector parameter</exception>
        /// <param name="persistor">A method that persists the result of the dependent transformation</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency RequireMany<TRequiredInput, TRequiredOutput>(TransformationRuleBase<TRequiredInput, TRequiredOutput> rule, Func<TIn, IEnumerable<TRequiredInput>> selector, Action<TOut, IEnumerable<TRequiredOutput>> persistor)
            where TRequiredInput : class
            where TRequiredOutput : class
        {
            if (persistor == null) persistor = (o1, o2) => { };
            if (selector == null) throw new ArgumentNullException("selector");
            return DependMany(null, c => SelectArrays(selector, c), rule, (s, t) => persistor((TOut)s, (IEnumerable<TRequiredOutput>)t), true, false);
        }

        /// <summary>
        /// Requires the transformation rule with the given type with all of the specified inputs
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformation rule</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformation rule</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformation</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        /// <param name="selector">A method that selects the inputs for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector parameter</exception>
        /// <param name="persistor">A method that persists the result of the dependent transformation</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency RequireMany<TRequiredInput1, TRequiredInput2, TRequiredOutput>(TransformationRuleBase<TRequiredInput1, TRequiredInput2, TRequiredOutput> rule, Func<TIn, IEnumerable<Tuple<TRequiredInput1, TRequiredInput2>>> selector, Action<TOut, IEnumerable<TRequiredOutput>> persistor)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
            where TRequiredOutput : class
        {
            if (persistor == null) persistor = (o1, o2) => { };
            if (selector == null) throw new ArgumentNullException("selector");
            return DependMany(null, c => SelectArraysT2(selector, c), rule, (s, t) => persistor((TOut)s, (IEnumerable<TRequiredOutput>)t), true, false);
        }

        /// <summary>
        /// Calls all transformation rules that transform items with the specified signature after the current transformation rule
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformations</typeparam>
        /// <param name="persistor">A method that persists the result of the dependent transformations</param>
        /// <remarks>This version Always takes the input parameter as input for the dependent transformations. Thus, this method will throw an exception, if the types do not match</remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void CallByType<TRequiredInput, TRequiredOutput>(Action<TOut, TRequiredOutput> persistor)
            where TRequiredInput : class
            where TRequiredOutput : class
        {
            if (typeof(TRequiredInput).IsAssignableFrom(typeof(TIn)))
            {
                CallByType<TRequiredInput, TRequiredOutput>(t => t as TRequiredInput, persistor);
            }
            else
            {
                throw new InvalidOperationException(Resources.ErrCall1ArgNoSelectorMustInherit);
            }
        }

        /// <summary>
        /// Calls the transformation rule with the given type after the current transformation rule
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the dependent transformation</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformation</typeparam>
        /// <param name="persistor">A method that persists the result of the dependent transformation</param>
        /// <remarks>This version Always takes the input parameter as input for the dependent transformations. Thus, this method will throw an exception, if the types do not match</remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public ITransformationRuleDependency Call<TRequiredInput, TRequiredOutput>(TransformationRuleBase<TRequiredInput, TRequiredOutput> rule, Action<TOut, TRequiredOutput> persistor)
            where TRequiredInput : class
            where TRequiredOutput : class
        {
            var inputTypes = new Type[] { typeof(TRequiredInput) };
            if (inputTypes.IsAssignableArrayFrom(InputType))
            {
                return Depend(null, c => c.CreateInputArray(), rule, persistor == null ? null : new Action<object, object>((o1, o2) => persistor(o1 as TOut, o2 as TRequiredOutput)), false, false);
            }
            else
            {
                throw new InvalidOperationException(Resources.ErrCallTransNoSelectorMustInherit);
            }
        }

        /// <summary>
        /// Calls all transformation rules that transform items with the specified signature after the current transformation rule
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformations</typeparam>
        /// <param name="selector">A method that selects the input for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector parameter</exception>
        /// <param name="persistor">A method that persists the result of the dependent transformations</param>
        public void CallByType<TRequiredInput, TRequiredOutput>(Func<TIn, TRequiredInput> selector, Action<TOut, TRequiredOutput> persistor)
            where TRequiredInput : class
            where TRequiredOutput : class
        {
            if (persistor == null) persistor = (o1, o2) => { };
            if (selector == null) throw new ArgumentNullException("selector");
            Predicate<Computation> filter = c => selector((TIn)c.GetInput(0)) != null;
            var inputTypes = new Type[] { typeof(TRequiredInput) };
            foreach (var rule in Transformation.GetRulesForTypeSignature(inputTypes, typeof(TRequiredOutput)))
            {
                Depend(filter, c => new object[] { selector((TIn)c.GetInput(0)) }, rule, (s, t) => persistor((TOut)s, (TRequiredOutput)t), false, false);
            }
        }

        /// <summary>
        /// Calls all transformation rules that transform items with the specified signature after the current transformation rule
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformation rule</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformation rule</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformations</typeparam>
        /// <param name="selector1">A method that selects the first input for the dependent transformations</param>
        /// <param name="selector2">A method that selects the second input for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector1 parameter</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector2 parameter</exception>
        /// <param name="persistor">A method that persists the result of the dependent transformations</param>
        public void CallByType<TRequiredInput1, TRequiredInput2, TRequiredOutput>(Func<TIn, TRequiredInput1> selector1, Func<TIn, TRequiredInput2> selector2, Action<TOut, TRequiredOutput> persistor)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
            where TRequiredOutput : class
        {
            if (persistor == null) persistor = (o1, o2) => { };
            if (selector1 == null) throw new ArgumentNullException("selector1");
            if (selector2 == null) throw new ArgumentNullException("selector2");
            var inputTypes = new Type[] { typeof(TRequiredInput1), typeof(TRequiredInput2) };
            foreach (var rule in Transformation.GetRulesForTypeSignature(inputTypes, typeof(TRequiredOutput)))
            {
                Depend(null, c => new object[] { selector1((TIn)c.GetInput(0)), selector2((TIn)c.GetInput(0)) }, rule, (s, t) => persistor((TOut)s, (TRequiredOutput)t), false, false);
            }
        }

        /// <summary>
        /// Calls the transformation rule with the given type after the current transformation rule
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the dependent transformation</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformation</typeparam>
        /// <param name="selector">A method that selects the input for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector parameter</exception>
        /// <param name="persistor">A method that persists the result of the dependent transformation</param>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the rule parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency Call<TRequiredInput, TRequiredOutput>(TransformationRuleBase<TRequiredInput, TRequiredOutput> rule, Func<TIn, TRequiredInput> selector, Action<TOut, TRequiredOutput> persistor)
            where TRequiredInput : class
            where TRequiredOutput : class
        {
            if (persistor == null) persistor = (o1, o2) => { };
            if (selector == null) throw new ArgumentNullException("selector");
            return Depend(c => selector((TIn)c.GetInput(0)) != null, c => new object[] { selector((TIn)c.GetInput(0)) }, rule, (s, t) => persistor((TOut)s, (TRequiredOutput)t), false, false);
        }

        /// <summary>
        /// Calls the transformation rule with the given type after the current transformation rule
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformation rule</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformation rule</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformation</typeparam>
        /// <param name="selector1">A method that selects the first input for the dependent transformations</param>
        /// <param name="selector2">A method that selects the second input for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector1 parameter</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector2 parameter</exception>
        /// <param name="persistor">A method that persists the result of the dependent transformation</param>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the rule parameter is passed a null instance</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency Call<TRequiredInput1, TRequiredInput2, TRequiredOutput>(TransformationRuleBase<TRequiredInput1, TRequiredInput2, TRequiredOutput> rule, Func<TIn, TRequiredInput1> selector1, Func<TIn, TRequiredInput2> selector2, Action<TOut, TRequiredOutput> persistor)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
            where TRequiredOutput : class
        {
            if (persistor == null) persistor = (o1, o2) => { };
            if (selector1 == null) throw new ArgumentNullException("selector1");
            if (selector2 == null) throw new ArgumentNullException("selector2");
            return Depend(null, c => new object[] { selector1((TIn)c.GetInput(0)), selector2((TIn)c.GetInput(0)) }, rule, (s, t) => persistor((TOut)s, (TRequiredOutput)t), false, false);
        }

        /// <summary>
        /// Calls all transformation rules that transform S to T with all of the specified objects after the current transformation rule
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the dependent transformations</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformations</typeparam>
        /// <param name="selector">A method that selects the inputs for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector parameter</exception>
        /// <param name="persistor">A method that persists the result of the dependent transformation</param>
        public void CallManyByType<TRequiredInput, TRequiredOutput>(Func<TIn, IEnumerable<TRequiredInput>> selector, Action<TOut, IEnumerable<TRequiredOutput>> persistor)
            where TRequiredInput : class
            where TRequiredOutput : class
        {
            if (persistor == null) persistor = (o1, o2) => { };
            if (selector == null) throw new ArgumentNullException("selector");
            foreach (var rule in Transformation.GetRulesForTypeSignature(new Type[] { typeof(TRequiredInput) }, typeof(TRequiredOutput)))
            {
                DependMany(null, c => SelectArrays(selector, c), rule, (s, t) => persistor((TOut)s, (IEnumerable<TRequiredOutput>)t), false, false);
            }
        }

        /// <summary>
        /// Calls all transformation rules that transform S to T with all of the specified objects after the current transformation rule
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformation rule</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformation rule</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformations</typeparam>
        /// <param name="selector">A method that selects the inputs for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector parameter</exception>
        /// <param name="persistor">A method that persists the result of the dependent transformation</param>
        public void CallManyByType<TRequiredInput1, TRequiredInput2, TRequiredOutput>(Func<TIn, IEnumerable<Tuple<TRequiredInput1, TRequiredInput2>>> selector, Action<TOut, IEnumerable<TRequiredOutput>> persistor)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
            where TRequiredOutput : class
        {
            if (persistor == null) persistor = (o1, o2) => { };
            if (selector == null) throw new ArgumentNullException("selector");
            foreach (var rule in Transformation.GetRulesForTypeSignature(new Type[] { typeof(TRequiredInput1), typeof(TRequiredInput2) }, typeof(TRequiredOutput)))
            {
                DependMany(null, c => SelectArraysT2(selector, c), rule, (s, t) => persistor((TOut)s, (IEnumerable<TRequiredOutput>)t), false, false);
            }
        }

        /// <summary>
        /// Calls the transformation rule with the given type with all of the specified inputs after the current transformation rule
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the dependent transformation</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformation</typeparam>
        /// <param name="selector">A method that selects the inputs for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector parameter</exception>
        /// <param name="persistor">A method that persists the result of the dependent transformation</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency CallMany<TRequiredInput, TRequiredOutput>(TransformationRuleBase<TRequiredInput, TRequiredOutput> rule, Func<TIn, IEnumerable<TRequiredInput>> selector, Action<TOut, IEnumerable<TRequiredOutput>> persistor)
            where TRequiredInput : class
            where TRequiredOutput : class
        {
            if (persistor == null) persistor = (o1, o2) => { };
            if (selector == null) throw new ArgumentNullException("selector");
            return DependMany(null, c => SelectArrays(selector, c), rule, (s, t) => persistor((TOut)s, (IEnumerable<TRequiredOutput>)t), false, false);
        }

        /// <summary>
        /// Calls the transformation rule with the given type with all of the specified inputs after the current transformation rule
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the dependent transformation rule</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the dependent transformation rule</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        /// <typeparam name="TRequiredOutput">The output type of the dependent transformation</typeparam>
        /// <param name="selector">A method that selects the inputs for the dependent transformations</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector parameter</exception>
        /// <param name="persistor">A method that persists the result of the dependent transformation</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency CallMany<TRequiredInput1, TRequiredInput2, TRequiredOutput>(TransformationRuleBase<TRequiredInput1, TRequiredInput2, TRequiredOutput> rule, Func<TIn, IEnumerable<Tuple<TRequiredInput1, TRequiredInput2>>> selector, Action<TOut, IEnumerable<TRequiredOutput>> persistor)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
            where TRequiredOutput : class
        {
            if (persistor == null) persistor = (o1, o2) => { };
            if (selector == null) throw new ArgumentNullException("selector");
            return DependMany(null, c => SelectArraysT2(selector, c), rule, (s, t) => persistor((TOut)s, (IEnumerable<TRequiredOutput>)t), false, false);
        }

        /// <summary>
        /// Calls the transformation rule with the given type after the current transformation rule, but no earlier than it created its output
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument for the called transformation</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        /// <param name="selector">A method that selects the input for the dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector parameter</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency CallOutputSensitive<TRequiredInput>(GeneralTransformationRule<TRequiredInput> rule, Func<TIn, TOut, TRequiredInput> selector)
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
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector parameter</exception>
        /// <param name="filter">A method that filters the cases where the dependency should fire</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency CallOutputSensitive<TRequiredInput>(GeneralTransformationRule<TRequiredInput> rule, Func<TIn, TOut, TRequiredInput> selector, Func<TIn, TOut, bool> filter)
            where TRequiredInput : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            if (rule == null) throw new ArgumentNullException("rule");
            return Depend(filter != null ?
                new Predicate<Computation>(c => filter(c.GetInput(0) as TIn, c.Output as TOut))
                : null, 
                c => new object[] { selector((TIn)c.GetInput(0), (TOut)c.Output) }, rule, null, false, true);
        }

        /// <summary>
        /// Calls the transformation rule with the given type after the current transformation rule, but no earlier than it created its output
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument for the called transformation</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        /// <param name="filter">A method that filters the cases where the dependency should fire</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency CallOutputSensitive<TRequiredInput>(GeneralTransformationRule<TRequiredInput> rule, Func<TIn, TOut, bool> filter)
            where TRequiredInput : class
        {
            if (!typeof(TRequiredInput).IsAssignableFrom(typeof(TIn))) throw new InvalidOperationException(Resources.ErrCall1ArgNoSelectorMustInherit);
            if (rule == null) throw new ArgumentNullException("rule");
            return Depend(filter != null ?
                new Predicate<Computation>(c => filter(c.GetInput(0) as TIn, c.Output as TOut))
                : null,
                c => c.CreateInputArray(), rule, null, false, true);
        }

        /// <summary>
        /// Calls the transformation rule with the given type after the current transformation rule, but no earlier than it created its output
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument for the called transformation</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument for the called transformation</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        /// <param name="selector1">A method that selects the first input for the dependent transformation</param>
        /// <param name="selector2">A method that selects the second input for the dependent transformation</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector1 parameter</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector2 parameter</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency CallOutputSensitive<TRequiredInput1, TRequiredInput2>(GeneralTransformationRule<TRequiredInput1, TRequiredInput2> rule, Func<TIn, TOut, TRequiredInput1> selector1, Func<TIn, TOut, TRequiredInput2> selector2)
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
        /// <param name="selector1">A method that selects the first input for the dependent transformation</param>
        /// <param name="selector2">A method that selects the second input for the dependent transformation</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector1 parameter</exception>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector2 parameter</exception>
        /// <param name="filter">A method that filters the cases where the dependency should fire</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency CallOutputSensitive<TRequiredInput1, TRequiredInput2>(GeneralTransformationRule<TRequiredInput1, TRequiredInput2> rule, Func<TIn, TOut, TRequiredInput1> selector1, Func<TIn, TOut, TRequiredInput2> selector2, Func<TIn, TOut, bool> filter)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
        {
            if (selector1 == null) throw new ArgumentNullException("selector1");
            if (selector2 == null) throw new ArgumentNullException("selector2");
            if (rule == null) throw new ArgumentNullException("rule");
            return Depend(filter != null ?
                new Predicate<Computation>(c => filter(c.GetInput(0) as TIn, c.Output as TOut))
                : null,
                c => new object[] { selector1((TIn)c.GetInput(0), (TOut)c.Output), selector2((TIn)c.GetInput(0), (TOut)c.Output) }, rule, null, false, true);
        }

        /// <summary>
        /// Calls the transformation rule with the given type after the current transformation rule, but no earlier than it created its output
        /// </summary>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        public ITransformationRuleDependency CallOutputSensitive(GeneralTransformationRule<TIn, TOut> rule)
        {
            return CallOutputSensitive(rule, null);
        }

        /// <summary>
        /// Calls the transformation rule with the given type after the current transformation rule, but no earlier than it created its output
        /// </summary>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        /// <param name="filter">A method that filters the cases where the dependency should fire</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency CallOutputSensitive(GeneralTransformationRule<TIn, TOut> rule, Func<TIn, TOut, bool> filter)
        {
            if (rule == null) throw new ArgumentNullException("rule");
            return Depend(filter != null ?
                new Predicate<Computation>(c => filter(c.GetInput(0) as TIn, c.Output as TOut))
                : null,
                c => new object[] { c.GetInput(0), c.Output }, rule, null, false, true);
        }

        /// <summary>
        /// Calls the transformation rule with the given type after the current transformation rule for all inputs, but no earlier than it created its output
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument for the called transformation</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        /// <param name="selector">A method that selects the inputs for the dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector parameter</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency CallManyOutputSensitive<TRequiredInput>(GeneralTransformationRule<TRequiredInput> rule, Func<TIn, TOut, IEnumerable<TRequiredInput>> selector)
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
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector parameter</exception>
        /// <param name="filter">A method that filters the cases where the dependency should fire</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency CallManyOutputSensitive<TRequiredInput>(GeneralTransformationRule<TRequiredInput> rule, Func<TIn, TOut, IEnumerable<TRequiredInput>> selector, Func<TIn, TOut, bool> filter)
            where TRequiredInput : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            if (rule == null) throw new ArgumentNullException("rule");
            return DependMany(filter != null ?
                new Predicate<Computation>(c => filter(c.GetInput(0) as TIn, c.Output as TOut))
                : null,
                c => SelectArrays(selector, c), rule, null, false, true);
        }

        /// <summary>
        /// Calls the transformation rule with the given type after the current transformation rule for all inputs, but no earlier than it created its output
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument for the called transformation</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument for the called transformation</typeparam>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        /// <param name="selector">A method that selects the inputs for the dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector parameter</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency CallManyOutputSensitive<TRequiredInput1, TRequiredInput2>(GeneralTransformationRule<TRequiredInput1, TRequiredInput2> rule, Func<TIn, TOut, IEnumerable<Tuple<TRequiredInput1, TRequiredInput2>>> selector)
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
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector parameter</exception>
        /// <param name="filter">A method that filters the cases where the dependency should fire</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public ITransformationRuleDependency CallManyOutputSensitive<TRequiredInput1, TRequiredInput2>(GeneralTransformationRule<TRequiredInput1, TRequiredInput2> rule, Func<TIn, TOut, IEnumerable<Tuple<TRequiredInput1, TRequiredInput2>>> selector, Func<TIn, TOut, bool> filter)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            if (rule == null) throw new ArgumentNullException("rule");
            return DependMany(filter != null ?
                new Predicate<Computation>(c => filter(c.GetInput(0) as TIn, c.Output as TOut))
                : null,
                c => SelectArrays(selector, c), rule, null, false, true);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the specified signature is made
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the source transformation rule</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the source transformation rule</typeparam>
        /// <param name="selector">A method that selects the input for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector parameter</exception>
        /// <param name="persistor">A method that persists the output of this rule back to the source instance</param>
        public void CallFor<TRequiredInput, TRequiredOutput>(Func<TRequiredInput, TIn> selector, Action<TOut, TRequiredOutput> persistor)
        {
            CallFor<TRequiredInput, TRequiredOutput>(selector, persistor, s => true);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the specified signature is made
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the source transformation rule</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the source transformation rule</typeparam>
        /// <param name="selector">A method that selects the input for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector parameter</exception>
        /// <param name="persistor">A method that persists the output of this rule back to the source instance</param>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        public void CallFor<TRequiredInput, TRequiredOutput>(TransformationRuleBase<TRequiredInput, TRequiredOutput> rule, Func<TRequiredInput, TIn> selector, Action<TOut, TRequiredOutput> persistor)
            where TRequiredInput : class
            where TRequiredOutput : class
        {
            CallFor<TRequiredInput, TRequiredOutput>(rule, selector, persistor, s => true);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the specified signature is made
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the source transformation rule</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the source transformation rule</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the source transformation rule</typeparam>
        /// <param name="selector">A method that selects the input for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector parameter</exception>
        /// <param name="persistor">A method that persists the output of this rule back to the source instance</param>
        public void CallFor<TRequiredInput1, TRequiredInput2, TRequiredOutput>(Func<TRequiredInput1, TRequiredInput2, TIn> selector, Action<TOut, TRequiredOutput> persistor)
        {
            CallFor<TRequiredInput1, TRequiredInput2, TRequiredOutput>(selector, persistor, (s1, s2) => true);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the specified signature is made
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the source transformation rule</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the source transformation rule</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the source transformation rule</typeparam>
        /// <param name="selector">A method that selects the input for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector parameter</exception>
        /// <param name="persistor">A method that persists the output of this rule back to the source instance</param>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        public void CallFor<TRequiredInput1, TRequiredInput2, TRequiredOutput>(TransformationRuleBase<TRequiredInput1, TRequiredInput2, TRequiredOutput> rule, Func<TRequiredInput1, TRequiredInput2, TIn> selector, Action<TOut, TRequiredOutput> persistor)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
            where TRequiredOutput : class
        {
            CallFor<TRequiredInput1, TRequiredInput2, TRequiredOutput>(rule, selector, persistor, (s1, s2) => true);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the specified signature is made
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the source transformation rule</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the source transformation rule</typeparam>
        /// <param name="selector">A method that selects the input for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector parameter</exception>
        /// <param name="persistor">A method that persists the output of this rule back to the source instance</param>
        /// <param name="filter">A method that filters the applicable instances of S</param>
        public void CallFor<TRequiredInput, TRequiredOutput>(Func<TRequiredInput, TIn> selector, Action<TOut, TRequiredOutput> persistor, Predicate<TRequiredInput> filter)
        {
            if (persistor == null) persistor = (o1, o2) => { };
            if (filter == null) filter = i => true;
            if (selector == null) throw new ArgumentNullException("selector");
            CallForInternal(new Type[] { typeof(TRequiredInput) }, typeof(TRequiredOutput), o => o.Output is TRequiredOutput && filter((TRequiredInput)o.GetInput(0)), o => new object[] { selector((TRequiredInput)o.GetInput(0)) }, (t, v) => persistor((TOut)v, (TRequiredOutput)t), false);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the specified signature is made
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the source transformation rule</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the source transformation rule</typeparam>
        /// <param name="selector">A method that selects the input for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector parameter</exception>
        /// <param name="persistor">A method that persists the output of this rule back to the source instance</param>
        /// <param name="filter">A method that filters the applicable instances of S</param>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public void CallFor<TRequiredInput, TRequiredOutput>(TransformationRuleBase<TRequiredInput, TRequiredOutput> rule, Func<TRequiredInput, TIn> selector, Action<TOut, TRequiredOutput> persistor, Predicate<TRequiredInput> filter)
            where TRequiredInput : class
            where TRequiredOutput : class
        {
            if (persistor == null) persistor = (o1, o2) => { };
            if (filter == null) filter = o => true;
            if (selector == null) throw new ArgumentNullException("selector");
            if (rule == null) throw new ArgumentNullException("rule");
            CallForInternal(rule, o => filter((TRequiredInput)o.GetInput(0)), o => new object[] { selector((TRequiredInput)o.GetInput(0)) }, (t, v) => persistor((TOut)v, (TRequiredOutput)t), false);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the specified signature is made
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the source transformation rule</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the source transformation rule</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the source transformation rule</typeparam>
        /// <param name="selector">A method that selects the input for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector parameter</exception>
        /// <param name="persistor">A method that persists the output of this rule back to the source instance</param>
        /// <param name="filter">A method that filters the applicable instances of S</param>
        public void CallFor<TRequiredInput1, TRequiredInput2, TRequiredOutput>(Func<TRequiredInput1, TRequiredInput2, TIn> selector, Action<TOut, TRequiredOutput> persistor, Func<TRequiredInput1, TRequiredInput2, bool> filter)
        {
            if (persistor == null) persistor = (o1, o2) => { };
            if (filter == null) filter = (s1, s2) => true;
            if (selector == null) throw new ArgumentNullException("selector");
            CallForInternal(new Type[] { typeof(TRequiredInput1), typeof(TRequiredInput2) }, typeof(TRequiredOutput), o => filter((TRequiredInput1)o.GetInput(0), (TRequiredInput2)o.GetInput(1)), o => new object[] { selector((TRequiredInput1)o.GetInput(0), (TRequiredInput2)o.GetInput(1)) }, (t, v) => persistor((TOut)v, (TRequiredOutput)t), false);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the specified signature is made
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the source transformation rule</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the source transformation rule</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the source transformation rule</typeparam>
        /// <param name="selector">A method that selects the input for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector parameter</exception>
        /// <param name="persistor">A method that persists the output of this rule back to the source instance</param>
        /// <param name="filter">A method that filters the applicable instances of S</param>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public void CallFor<TRequiredInput1, TRequiredInput2, TRequiredOutput>(TransformationRuleBase<TRequiredInput1, TRequiredInput2, TRequiredOutput> rule, Func<TRequiredInput1, TRequiredInput2, TIn> selector, Action<TOut, TRequiredOutput> persistor, Func<TRequiredInput1, TRequiredInput2, bool> filter)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
            where TRequiredOutput : class
        {
            if (persistor == null) persistor = (o1, o2) => { };
            if (filter == null) filter = (o1, o2) => true;
            if (selector == null) throw new ArgumentNullException("selector");
            if (rule == null) throw new ArgumentNullException("rule");
            CallForInternal(rule, o => filter((TRequiredInput1)o.GetInput(0), (TRequiredInput2)o.GetInput(1)), o => new object[] { selector((TRequiredInput1)o.GetInput(0), (TRequiredInput2)o.GetInput(1)) }, (t, v) => persistor((TOut)v, (TRequiredOutput)t), false);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the specified signature is made
        /// This version calls this transformation rule for a whol collection of input objects
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the source transformation rule</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the source transformation rule</typeparam>
        /// <param name="selector">A method that selects the inputs for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector parameter</exception>
        /// <param name="persistor">A method that persists the outputs of this rule back to the source instance</param>
        public void CallForEach<TRequiredInput, TRequiredOutput>(Func<TRequiredInput, IEnumerable<TIn>> selector, Action<TRequiredOutput, IEnumerable<TOut>> persistor)
        {
            CallForEach<TRequiredInput, TRequiredOutput>(selector, s => true, persistor);
        }


        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the specified signature is made
        /// This version calls this transformation rule for a whol collection of input objects
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the source transformation rule</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the source transformation rule</typeparam>
        /// <param name="selector">A method that selects the inputs for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector parameter</exception>
        /// <param name="persistor">A method that persists the outputs of this rule back to the source instance</param>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        public void CallForEach<TRequiredInput, TRequiredOutput>(TransformationRuleBase<TRequiredInput, TRequiredOutput> rule, Func<TRequiredInput, IEnumerable<TIn>> selector, Action<TRequiredOutput, IEnumerable<TOut>> persistor)
            where TRequiredInput : class
            where TRequiredOutput : class
        {
            CallForEach<TRequiredInput, TRequiredOutput>(rule, selector, s => true, persistor);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the specified signature is made
        /// This version calls this transformation rule for a whol collection of input objects
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the source transformation rule</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the source transformation rule</typeparam>
        /// <param name="selector">A method that selects the input for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector parameter</exception>
        /// <param name="persistor">A method that persists the output of this rule back to the source instance</param>
        /// <param name="filter">A method that filters the applicable instances of S</param>
        public void CallForEach<TRequiredInput, TRequiredOutput>(Func<TRequiredInput, IEnumerable<TIn>> selector, Predicate<TRequiredInput> filter, Action<TRequiredOutput, IEnumerable<TOut>> persistor)
        {
            if (persistor == null) persistor = (o1, o2) => { };
            if (filter == null) throw new ArgumentNullException("filter");
            if (selector == null) throw new ArgumentNullException("selector");
            CallForEachInternal(new Type[] { typeof(TRequiredInput) }, typeof(TRequiredOutput), o => filter((TRequiredInput)o.GetInput(0)), o => SelectCallArrays(selector, o), (t, v) => persistor((TRequiredOutput)t, (IEnumerable<TOut>)v), false);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the specified signature is made
        /// This version calls this transformation rule for a whol collection of input objects
        /// </summary>
        /// <typeparam name="TRequiredInput">The input argument type of the source transformation rule</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the source transformation rule</typeparam>
        /// <param name="selector">A method that selects the input for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector parameter</exception>
        /// <param name="persistor">A method that persists the output of this rule back to the source instance</param>
        /// <param name="filter">A method that filters the applicable instances of S</param>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public void CallForEach<TRequiredInput, TRequiredOutput>(TransformationRuleBase<TRequiredInput, TRequiredOutput> rule, Func<TRequiredInput, IEnumerable<TIn>> selector, Predicate<TRequiredInput> filter, Action<TRequiredOutput, IEnumerable<TOut>> persistor)
            where TRequiredInput : class
            where TRequiredOutput : class
        {
            if (persistor == null) persistor = (o1, o2) => { };
            if (filter == null) filter = o => true;
            if (selector == null) throw new ArgumentNullException("selector");
            if (rule == null) throw new ArgumentNullException("rule");
            CallForEachInternal(rule, o => filter((TRequiredInput)o.GetInput(0)), o => SelectCallArrays(selector, o), (t, v) => persistor((TRequiredOutput)t, (IEnumerable<TOut>)v), false);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the specified signature is made
        /// This version calls this transformation rule for a whol collection of input objects
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the source transformation rule</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the source transformation rule</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the source transformation rule</typeparam>
        /// <param name="selector">A method that selects the inputs for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector parameter</exception>
        /// <param name="persistor">A method that persists the outputs of this rule back to the source instance</param>
        public void CallForEach<TRequiredInput1, TRequiredInput2, TRequiredOutput>(Func<TRequiredInput1, TRequiredInput2, IEnumerable<TIn>> selector, Action<TRequiredOutput, IEnumerable<TOut>> persistor)
        {
            CallForEach<TRequiredInput1, TRequiredInput2, TRequiredOutput>(selector, (s1,s2) => true, persistor);
        }


        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the specified signature is made
        /// This version calls this transformation rule for a whol collection of input objects
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the source transformation rule</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the source transformation rule</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the source transformation rule</typeparam>
        /// <param name="selector">A method that selects the inputs for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector parameter</exception>
        /// <param name="persistor">A method that persists the outputs of this rule back to the source instance</param>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        public void CallForEach<TRequiredInput1, TRequiredInput2, TRequiredOutput>(TransformationRuleBase<TRequiredInput1, TRequiredInput2, TRequiredOutput> rule, Func<TRequiredInput1, TRequiredInput2, IEnumerable<TIn>> selector, Action<TRequiredOutput, IEnumerable<TOut>> persistor)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
            where TRequiredOutput : class
        {
            CallForEach<TRequiredInput1, TRequiredInput2, TRequiredOutput>(rule, selector, (s1,s2) => true, persistor);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the specified signature is made
        /// This version calls this transformation rule for a whol collection of input objects
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the source transformation rule</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the source transformation rule</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the source transformation rule</typeparam>
        /// <param name="selector">A method that selects the input for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector parameter</exception>
        /// <param name="persistor">A method that persists the output of this rule back to the source instance</param>
        /// <param name="filter">A method that filters the applicable instances of S</param>
        public void CallForEach<TRequiredInput1, TRequiredInput2, TRequiredOutput>(Func<TRequiredInput1, TRequiredInput2, IEnumerable<TIn>> selector, Func<TRequiredInput1, TRequiredInput2, bool> filter, Action<TRequiredOutput, IEnumerable<TOut>> persistor)
        {
            if (persistor == null) persistor = (o1, o2) => { };
            if (filter == null) filter = (o1, o2) => true;
            if (selector == null) throw new ArgumentNullException("selector");
            CallForEachInternal(new Type[] { typeof(TRequiredInput1), typeof(TRequiredInput2) }, typeof(TRequiredOutput), o => filter((TRequiredInput1)o.GetInput(0), (TRequiredInput2)o.GetInput(1)), o => SelectCallArraysT2(selector, o), (t, v) => persistor((TRequiredOutput)t, (IEnumerable<TOut>)v), false);
        }

        /// <summary>
        /// Create a call dependency, i.e., let this transformation be called as soon as a transformation with the specified signature is made
        /// This version calls this transformation rule for a whol collection of input objects
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first input argument type of the source transformation rule</typeparam>
        /// <typeparam name="TRequiredInput2">The second input argument type of the source transformation rule</typeparam>
        /// <typeparam name="TRequiredOutput">The output type of the source transformation rule</typeparam>
        /// <param name="selector">A method that selects the input for this transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector parameter</exception>
        /// <param name="persistor">A method that persists the output of this rule back to the source instance</param>
        /// <param name="filter">A method that filters the applicable instances of S</param>
        /// <param name="rule">The dependent transformation rule</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the rule parameter</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public void CallForEach<TRequiredInput1, TRequiredInput2, TRequiredOutput>(TransformationRuleBase<TRequiredInput1, TRequiredInput2, TRequiredOutput> rule, Func<TRequiredInput1, TRequiredInput2, IEnumerable<TIn>> selector, Func<TRequiredInput1, TRequiredInput2, bool> filter, Action<TRequiredOutput, IEnumerable<TOut>> persistor)
            where TRequiredInput1 : class
            where TRequiredInput2 : class
            where TRequiredOutput : class
        {
            if (persistor == null) persistor = (o1, o2) => { };
            if (filter == null) filter = (o1, o2) => true;
            if (selector == null) throw new ArgumentNullException("selector");
            if (rule == null) throw new ArgumentNullException("rule");
            CallForEachInternal(rule, o => filter((TRequiredInput1)o.GetInput(0), (TRequiredInput2)o.GetInput(1)), o => SelectCallArraysT2(selector, o), (t, v) => persistor((TRequiredOutput)t, (IEnumerable<TOut>)v), false);
        }

        /// <summary>
        /// Gets the output type of this transformation rule
        /// </summary>
        public sealed override Type OutputType
        {
            get { return typeof(TOut); }
        }

        /// <summary>
        /// This method is a helper function that converts the given selector of tuples to a selector of object[]
        /// </summary>
        /// <typeparam name="TRequiredInput1">The first type argument of the selector output tuple</typeparam>
        /// <typeparam name="TRequiredInput2">The second type argument pf the selector output tuple</typeparam>
        /// <param name="selector">The source selector that selects the output of dependant transformation rules as typed tuples</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector parameter</exception>
        /// <param name="inputs">The inputs for the selector</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if the inputs parameter is passed a null instance</exception>
        /// <param name="output">The output of this transformation rule</param>
        /// <returns>A collection of inputs for other transformation rules</returns>
        /// <remarks>This method is used as helper function for DependMany. This version is output sensitive. Use it only with the needOutput-parameter of DependMany set to true!</remarks>  
        protected internal static IEnumerable<object[]> SelectArrays<TRequiredInput1, TRequiredInput2>(Func<TIn, TOut, IEnumerable<Tuple<TRequiredInput1, TRequiredInput2>>> selector, Computation computation) 
            where TRequiredInput1 : class
            where TRequiredInput2 : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            if (computation == null) throw new ArgumentNullException("computation");

            var tc = selector((TIn)computation.GetInput(0), (TOut)computation.Output);
            if (tc != null)
            {
                return tc.Select(s => new object[] { s.Item1, s.Item2 });
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// This method is a helper function that converts the given selector to a selector of object[]
        /// </summary>
        /// <typeparam name="TRequiredInput">The type argument of the selector output</typeparam>
        /// <param name="selector">The source selector that selects the output of dependant transformation rules as typed tuples</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the selector parameter</exception>
        /// <param name="inputs">The inputs for the selector</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever a null reference is passed to the inputs parameter</exception>
        /// <param name="output">The output of this transformation rule</param>
        /// <returns>A collection of inputs for other transformation rules</returns>
        /// <remarks>This method is used as helper function for DependMany. This version is output sensitive. Use it only with the needOutput-parameter of DependMany set to true!</remarks>      
        protected internal static IEnumerable<object[]> SelectArrays<TRequiredInput>(Func<TIn, TOut, IEnumerable<TRequiredInput>> selector, Computation computation)
            where TRequiredInput : class
        {
            if (selector == null) throw new ArgumentNullException("selector");
            if (computation == null) throw new ArgumentNullException("computation");

            var tc = selector((TIn)computation.GetInput(0), (TOut)computation.Output);
            if (tc != null)
            {
                return tc.Select(s => new object[] { s });
            }
            else
            {
                return null;
            }
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

        /// <summary>
        /// Creates a trace entry for every computation with the key specified by the given selector method
        /// </summary>
        /// <typeparam name="TKey">The type of the trace entry key</typeparam>
        /// <param name="traceSelector">A method that selects for an input the appropriate key that should be added to the trace</param>
        /// <returns>A transformation rule that can be used as group or for direct tracing purposes</returns>
        public TraceEntryGroup<TKey, TOut> TraceOutput<TKey>(Func<TIn, TOut, TKey> traceSelector)
            where TKey : class
        {
            if (traceSelector == null) throw new ArgumentNullException("traceSelector");

            var traceRule = new TraceEntryGroup<TKey, TOut>();
            TraceOutput(traceRule, traceSelector);
            return traceRule;
        }

        /// <summary>
        /// Creates a trace entry for every computation with the key specified by the given selector method
        /// </summary>
        /// <typeparam name="TKey">The type of the trace entry key</typeparam>
        /// <param name="traceSelector">A method that selects for an input the appropriate key that should be added to the trace</param>
        /// <param name="traceKey">The transformation rule that is used as group for direct tracing purposes</param>
        public ITransformationRuleDependency TraceOutput<TKey>(TraceEntryGroup<TKey, TOut> traceKey, Func<TIn, TOut, TKey> traceSelector)
            where TKey : class
        {
            if (traceSelector == null) throw new ArgumentNullException("traceSelector");
            if (traceKey == null) throw new ArgumentNullException("traceKey");

            var traceDependency = new TraceDependency<TIn, TOut, TKey, TOut>()
            {
                InputSelector = traceSelector,
                OutputSelector = (i, o) => o,
                TraceKey = traceKey
            };
            Dependencies.Add(traceDependency);

            return traceDependency;
        }

        /// <summary>
        /// Creates a trace entry for every computation of the current transformation rule
        /// </summary>
        /// <typeparam name="TTraceInput">The input type for the newly created trace entries</typeparam>
        /// <typeparam name="TTraceOutput">The output type for the newly created trace entries</typeparam>
        /// <param name="traceKey">A transformation rule that should be used as a key for tracing purposes</param>
        /// <remarks>Using this overload requires that the type parameter TTraceOutput is a base class of TOut and the type parameter TTraceInput is a base class of TIn. Sadly, this cannot be checked by the C# compiler</remarks>
        public ITransformationRuleDependency TraceAs<TTraceInput, TTraceOutput>(TraceEntryGroup<TTraceInput, TTraceOutput> traceKey)
            where TTraceInput : class
            where TTraceOutput : class
        {
            return TraceAs<TTraceInput, TTraceOutput>(traceKey, null, null);
        }

        /// <summary>
        /// Creates a trace entry for every computation of the current transformation rule
        /// </summary>
        /// <typeparam name="TTraceInput">The input type for the newly created trace entries</typeparam>
        /// <typeparam name="TTraceOutput">The output type for the newly created trace entries</typeparam>
        /// <param name="traceKey">A transformation rule that should be used as a key for tracing purposes</param>
        /// <param name="inputSelector">A method that selects the input for the trace entry. If null is specified, the input is taken by default. If this is not possible, an exception is thrown.</param>
        /// <remarks>Using this overload requires that the type parameter TTraceOutput is a base class of TOut. Sadly, this cannot be checked by the C# compiler</remarks>
        public ITransformationRuleDependency TraceAs<TTraceInput, TTraceOutput>(TraceEntryGroup<TTraceInput, TTraceOutput> traceKey, Func<TIn, TOut, TTraceInput> inputSelector)
            where TTraceInput : class
            where TTraceOutput : class
        {
            return TraceAs<TTraceInput, TTraceOutput>(traceKey, inputSelector, null);
        }

        /// <summary>
        /// Creates a trace entry for every computation of the current transformation rule
        /// </summary>
        /// <typeparam name="TTraceInput">The input type for the newly created trace entries</typeparam>
        /// <typeparam name="TTraceOutput">The output type for the newly created trace entries</typeparam>
        /// <param name="traceKey">A transformation rule that should be used as a key for tracing purposes</param>
        /// <remarks>Using this overload requires that the type parameter TTraceInput is a base class of TIn. Sadly, this cannot be checked by the C# compiler</remarks>
        /// <param name="outputSelector">A method that selects the output for the trace entry. If null is specified, the output is taken by default. If this is not possible, an exception is thrown.</param>
        public ITransformationRuleDependency TraceAs<TTraceInput, TTraceOutput>(TraceEntryGroup<TTraceInput, TTraceOutput> traceKey, Func<TIn, TOut, TTraceOutput> outputSelector)
            where TTraceInput : class
            where TTraceOutput : class
        {
            return TraceAs<TTraceInput, TTraceOutput>(traceKey, null, outputSelector);
        }

        /// <summary>
        /// Creates a trace entry for every computation of the current transformation rule
        /// </summary>
        /// <typeparam name="TTraceInput">The input type for the newly created trace entries</typeparam>
        /// <typeparam name="TTraceOutput">The output type for the newly created trace entries</typeparam>
        /// <param name="traceKey">A transformation rule that should be used as a key for tracing purposes</param>
        /// <param name="inputSelector">A method that selects the input for the trace entry. If null is specified, the input is taken by default. If this is not possible, an exception is thrown.</param>
        /// <param name="outputSelector">A method that selects the output for the trace entry. If null is specified, the output is taken by default. If this is not possible, an exception is thrown.</param>
        public ITransformationRuleDependency TraceAs<TTraceInput, TTraceOutput>(TraceEntryGroup<TTraceInput, TTraceOutput> traceKey, Func<TIn, TOut, TTraceInput> inputSelector, Func<TIn, TOut, TTraceOutput> outputSelector)
            where TTraceInput : class
            where TTraceOutput : class
        {
            if (traceKey == null) throw new ArgumentNullException("traceKey");

            if (inputSelector == null)
            {
                if (typeof(TTraceInput).IsAssignableFrom(typeof(TIn)))
                {
                    inputSelector = (i, o) => i as TTraceInput;
                }
                else
                {
                    throw new ArgumentNullException("inputSelector");
                }
            }

            if (outputSelector == null)
            {
                if (typeof(TTraceOutput).IsAssignableFrom(typeof(TOut)))
                {
                    outputSelector = (i, o) => o as TTraceOutput;
                }
                else
                {
                    throw new ArgumentNullException("outputSelector");
                }
            }

            var traceDependency = new TraceDependency<TIn, TOut, TTraceInput, TTraceOutput>()
            {
                InputSelector = inputSelector,
                OutputSelector = outputSelector,
                TraceKey = traceKey
            };

            Dependencies.Add(traceDependency);

            return traceDependency;
        }
    }
}
