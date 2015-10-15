using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using NMF.Utilities;
using NMF.Transformations.Properties;
using NMF.Transformations.Core;

namespace NMF.Transformations
{
    /// <summary>
    /// This class provides methods to execute transformations
    /// </summary>
    public static class TransformationEngine
    {

        /// <summary>
        /// Transforms the input argument into an output using the provided transformation
        /// </summary>
        /// <typeparam name="TOut">The desired output type</typeparam>
        /// <param name="input">The input parameter</param>
        /// <param name="transformation">The transformation that shall be used for this operation (must not be null, but can be uninitialized)</param>
        /// <returns>The output from the transformation</returns>
        public static TOut Transform<TOut>(object[] input, Transformation transformation)
            where TOut : class
        {
            if (transformation == null) throw new ArgumentNullException("transformation");
            return Transform<TOut>(input, new TransformationContext(transformation), null);
        }

        /// <summary>
        /// Transforms the input argument into an output using the provided transformation
        /// </summary>
        /// <typeparam name="TOut">The desired output type</typeparam>
        /// <param name="inputs">The input parameters</param>
        /// <param name="types">The types of the elements within the collection refered to in the inputs parameter</param>
        /// <param name="transformation">The transformation that shall be used for this operation (must not be null, but can be uninitialized)</param>
        /// <returns>The output from the transformation</returns>
        public static IEnumerable<TOut> TransformMany<TOut>(IEnumerable<object[]> inputs, Type[] types, Transformation transformation)
            where TOut : class
        {
            if (transformation == null) throw new ArgumentNullException("transformation");
            return TransformMany<TOut>(inputs, types, new TransformationContext(transformation), null);
        }

        /// <summary>
        /// Transforms the input argument into an output using the provided transformation
        /// </summary>
        /// <typeparam name="TOut">The desired output type</typeparam>
        /// <param name="input">The input parameter</param>
        /// <param name="context">The context that should be used (must not be null).</param>
        /// <returns>The output from the transformation</returns>
        public static TOut Transform<TOut>(object[] input, ITransformationEngineContext context)
            where TOut : class
        {
            return Transform<TOut>(input, context, null);
        }

        /// <summary>
        /// Transforms the input argument into an output using the provided transformation
        /// </summary>
        /// <typeparam name="TOut">The desired output type</typeparam>
        /// <param name="inputs">The input parameters</param>
        /// <param name="types">The types of the elements within the collection refered to in the inputs parameter</param>
        /// <param name="context">The context that should be used (must not be null).</param>
        /// <returns>The output from the transformation</returns>
        public static IEnumerable<TOut> TransformMany<TOut>(IEnumerable<object[]> inputs, Type[] types, ITransformationEngineContext context)
            where TOut : class
        {
            return TransformMany<TOut>(inputs, types, context, null);
        }

        /// <summary>
        /// Transforms the input argument into an output using the provided transformation
        /// </summary>
        /// <typeparam name="TOut">The desired output type</typeparam>
        /// <param name="input">The input parameter</param>
        /// <param name="startRule">The rule that should be started with. If this is null, an applicable rule is found.</param>
        /// <param name="context">The context that should be used (must not be null).</param>
        /// <returns>The output from the transformation</returns>
        public static TOut Transform<TOut>(object[] input, ITransformationEngineContext context, GeneralTransformationRule startRule)
            where TOut : class
        {
            if (context == null) throw new ArgumentNullException("context");
            if (input == null) throw new ArgumentNullException("input");
            var transformation = context.Transformation;
            if (!transformation.IsInitialized) transformation.Initialize();
            if (startRule == null)
            {
                startRule = context.Transformation.GetRuleForTypeSignature(input.GetTypes(), typeof(TOut));
            }
            else
            {
                if (startRule.Transformation != context.Transformation) ThrowRuleNotPartOfTransformation();
                CheckTransformationRule<TOut>(input, startRule);
            }
            return TransformationRunner.Transform(new object[] { input }, null, startRule, context).Output as TOut;
        }

        private static void CheckTransformationRule<TOut>(object[] input, GeneralTransformationRule startRule) where TOut : class
        {
            if (!(startRule is TransformationRuleBase<TOut>) && !startRule.OutputType.IsAssignableFrom(typeof(TOut)))
                throw new InvalidOperationException("The output type of the specified start rule does not match the expected result type! Please provide a different start rule.");
            if (!startRule.InputType.IsInstanceArrayOfType(input))
                throw new InvalidOperationException("The input parameter types of the specified start rule do not match the given inputs. Please choose a different start rule.");
        }

        /// <summary>
        /// Transforms the input argument into an output using the provided transformation
        /// </summary>
        /// <typeparam name="TOut">The desired output type</typeparam>
        /// <param name="inputs">The input parameters</param>
        /// <param name="types">The types of the elements within the collection refered to in the inputs parameter</param>
        /// <param name="startRule">The rule that should be started with. If this is null, an applicable rule is found.</param>
        /// <param name="context">The context that should be used (must not be null).</param>
        /// <returns>The output from the transformation</returns>
        public static IEnumerable<TOut> TransformMany<TOut>(IEnumerable<object[]> inputs, Type[] types, ITransformationEngineContext context, GeneralTransformationRule startRule)
            where TOut : class
        {
            if (context == null) throw new ArgumentNullException("context");
            if (types == null) throw new ArgumentNullException("types");
            var transformation = context.Transformation;
            if (!transformation.IsInitialized) transformation.Initialize();
            if (startRule == null)
            {
                startRule = context.Transformation.GetRuleForTypeSignature(types, typeof(TOut));
            }
            else
            {
                if (startRule.Transformation != context.Transformation) ThrowRuleNotPartOfTransformation();
            }
            return TransformationRunner.TransformMany(inputs.Select(item => new object[] { item }), null, startRule, context).Select(c => c.Output).OfType<TOut>();
        }


        /// <summary>
        /// Transforms the input arguments into an output using the provided transformation
        /// </summary>
        /// <typeparam name="TIn1">The type of the first input argument</typeparam>
        /// <typeparam name="TIn2">The type of the second input argument</typeparam>
        /// <typeparam name="TOut">The desired output type</typeparam>
        /// <param name="input1">The first input parameter</param>
        /// <param name="input2">The second input parameter</param>
        /// <param name="transformation">The transformation that shall be used for this operation (must not be null, but can be uninitialized)</param>
        /// <returns>The output from the transformation</returns>
        public static TOut Transform<TIn1, TIn2, TOut>(TIn1 input1, TIn2 input2, Transformation transformation)
            where TIn1 : class
            where TIn2 : class
            where TOut : class
        {
            if (transformation == null) throw new ArgumentNullException("transformation");
            return Transform<TIn1, TIn2, TOut>(input1, input2, new TransformationContext(transformation), null);
        }

        /// <summary>
        /// Transforms the input arguments into an output using the provided transformation
        /// </summary>
        /// <typeparam name="TIn1">The type of the first input argument</typeparam>
        /// <typeparam name="TIn2">The type of the second input argument</typeparam>
        /// <typeparam name="TOut">The desired output type</typeparam>
        /// <param name="inputs">The input parameters</param>
        /// <param name="transformation">The transformation that shall be used for this operation (must not be null, but can be uninitialized)</param>
        /// <returns>The output from the transformation</returns>
        public static IEnumerable<TOut> TransformMany<TIn1, TIn2, TOut>(IEnumerable<Tuple<TIn1, TIn2>> inputs, Transformation transformation)
            where TIn1 : class
            where TIn2 : class
            where TOut : class
        {
            if (transformation == null) throw new ArgumentNullException("transformation");
            return TransformMany<TIn1, TIn2, TOut>(inputs, new TransformationContext(transformation), null);
        }

        /// <summary>
        /// Transforms the input arguments into an output using the provided transformation
        /// </summary>
        /// <typeparam name="TIn1">The type of the first input argument</typeparam>
        /// <typeparam name="TIn2">The type of the second input argument</typeparam>
        /// <typeparam name="TOut">The desired output type</typeparam>
        /// <param name="input1">The first input parameter</param>
        /// <param name="input2">The second input parameter</param>
        /// <param name="context">The context that should be used (must not be null)</param>
        /// <returns>The output from the transformation</returns>
        public static TOut Transform<TIn1, TIn2, TOut>(TIn1 input1, TIn2 input2, ITransformationEngineContext context)
            where TIn1 : class
            where TIn2 : class
            where TOut : class
        {
            return Transform<TIn1, TIn2, TOut>(input1, input2, context, null);
        }

        /// <summary>
        /// Transforms the input arguments into an output using the provided transformation
        /// </summary>
        /// <typeparam name="TIn1">The type of the first input argument</typeparam>
        /// <typeparam name="TIn2">The type of the second input argument</typeparam>
        /// <typeparam name="TOut">The desired output type</typeparam>
        /// <param name="inputs">The input parameters</param>
        /// <param name="context">The context that should be used (must not be null)</param>
        /// <returns>The output from the transformation</returns>
        public static IEnumerable<TOut> TransformMany<TIn1, TIn2, TOut>(IEnumerable<Tuple<TIn1, TIn2>> inputs, ITransformationEngineContext context)
            where TIn1 : class
            where TIn2 : class
            where TOut : class
        {
            return TransformMany<TIn1, TIn2, TOut>(inputs, context, null);
        }

        /// <summary>
        /// Transforms the input arguments into an output using the provided transformation
        /// </summary>
        /// <typeparam name="TIn1">The type of the first input argument</typeparam>
        /// <typeparam name="TIn2">The type of the second input argument</typeparam>
        /// <typeparam name="TOut">The desired output type</typeparam>
        /// <param name="input1">The first input parameter</param>
        /// <param name="input2">The second input parameter</param>
        /// <param name="startRule">The rule that should be started with. If this is null, an applicable rule is found.</param>
        /// <param name="context">The context that should be used (must not be null)</param>
        /// <returns>The output from the transformation</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static TOut Transform<TIn1, TIn2, TOut>(TIn1 input1, TIn2 input2, ITransformationEngineContext context, TransformationRuleBase<TIn1, TIn2, TOut> startRule)
            where TIn1 : class
            where TIn2 : class
            where TOut : class
        {
            if (context == null) throw new ArgumentNullException("context");
            if (!context.Transformation.IsInitialized) context.Transformation.Initialize();
            if (startRule == null)
            {
                startRule = context.Transformation.GetRuleForTypeSignature(new Type[] { typeof(TIn1), typeof(TIn2)}, typeof(TOut)) as TransformationRuleBase<TIn1, TIn2, TOut>;
            }
            else
            {
                if (startRule.Transformation != context.Transformation) ThrowRuleNotPartOfTransformation();
            }
            return TransformationRunner.Transform(new object[] { input1, input2 }, null, startRule, context).Output as TOut;
        }

        /// <summary>
        /// Transforms the input arguments into an output using the provided transformation
        /// </summary>
        /// <typeparam name="TIn1">The type of the first input argument</typeparam>
        /// <typeparam name="TIn2">The type of the second input argument</typeparam>
        /// <typeparam name="TOut">The desired output type</typeparam>
        /// <param name="inputs">The first input parameters</param>
        /// <param name="startRule">The rule that should be started with. If this is null, an applicable rule is found.</param>
        /// <param name="context">The context that should be used (must not be null)</param>
        /// <returns>The output from the transformation</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static IEnumerable<TOut> TransformMany<TIn1, TIn2, TOut>(IEnumerable<Tuple<TIn1, TIn2>> inputs, ITransformationEngineContext context, TransformationRuleBase<TIn1, TIn2, TOut> startRule)
            where TIn1 : class
            where TIn2 : class
            where TOut : class
        {
            if (context == null) throw new ArgumentNullException("context");
            if (!context.Transformation.IsInitialized) context.Transformation.Initialize();
            if (startRule == null)
            {
                startRule = context.Transformation.GetRuleForTypeSignature(new Type[] { typeof(TIn1), typeof(TIn2) }, typeof(TOut)) as TransformationRuleBase<TIn1, TIn2, TOut>;
            }
            else
            {
                if (startRule.Transformation != context.Transformation) ThrowRuleNotPartOfTransformation();
            }
            return TransformationRunner.TransformMany(inputs.Select((it1, it2) => new object[] { it1, it2 }), null, startRule, context).Select(c => c.Output).OfType<TOut>();
        }

        /// <summary>
        /// Processes the input arguments using the provided transformation
        /// </summary>
        /// <typeparam name="TIn1">The type of the first input argument</typeparam>
        /// <typeparam name="TIn2">The type of the second input argument</typeparam>
        /// <param name="input1">The first input parameter</param>
        /// <param name="input2">The second input parameter</param>
        /// <param name="transformation">The transformation that shall be used for this operation (must not be null, but can be uninitialized)</param>
        public static void Process<TIn1, TIn2>(TIn1 input1, TIn2 input2, Transformation transformation)
            where TIn1 : class
            where TIn2 : class
        {
            if (transformation == null) throw new ArgumentNullException("transformation");
            Process<TIn1, TIn2>(input1, input2, new TransformationContext(transformation), null);
        }

        /// <summary>
        /// Processes the input arguments using the provided transformation
        /// </summary>
        /// <typeparam name="TIn1">The type of the first input argument</typeparam>
        /// <typeparam name="TIn2">The type of the second input argument</typeparam>
        /// <param name="inputs">The input parameters</param>
        /// <param name="transformation">The transformation that shall be used for this operation (must not be null, but can be uninitialized)</param>
        public static void ProcessMany<TIn1, TIn2>(IEnumerable<Tuple<TIn1, TIn2>> inputs, Transformation transformation)
            where TIn1 : class
            where TIn2 : class
        {
            if (transformation == null) throw new ArgumentNullException("transformation");
            ProcessMany<TIn1, TIn2>(inputs, new TransformationContext(transformation), null);
        }

        /// <summary>
        /// Processes the input arguments using the provided transformation
        /// </summary>
        /// <typeparam name="TIn1">The type of the first input argument</typeparam>
        /// <typeparam name="TIn2">The type of the second input argument</typeparam>
        /// <param name="input1">The first input parameter</param>
        /// <param name="input2">The second input parameter</param>
        /// <param name="context">The context that should be used (must not be null).</param>
        public static void Process<TIn1, TIn2>(TIn1 input1, TIn2 input2, ITransformationEngineContext context)
            where TIn1 : class
            where TIn2 : class
        {
            Process(input1, input2, context, null);
        }

        /// <summary>
        /// Processes the input arguments using the provided transformation
        /// </summary>
        /// <typeparam name="TIn1">The type of the first input argument</typeparam>
        /// <typeparam name="TIn2">The type of the second input argument</typeparam>
        /// <param name="inputs">The input parameters</param>
        /// <param name="context">The context that should be used (must not be null).</param>
        public static void ProcessMany<TIn1, TIn2>(IEnumerable<Tuple<TIn1, TIn2>> inputs, ITransformationEngineContext context)
            where TIn1 : class
            where TIn2 : class
        {
            ProcessMany(inputs, context, null as GeneralTransformationRule<TIn1, TIn2>);
        }

        /// <summary>
        /// Processes the input arguments using the provided transformation
        /// </summary>
        /// <typeparam name="TIn1">The type of the first input argument</typeparam>
        /// <typeparam name="TIn2">The type of the second input argument</typeparam>
        /// <param name="input1">The first input parameter</param>
        /// <param name="input2">The second input parameter</param>
        /// <param name="startRule">The rule that should be started with. If this is null, an applicable rule is found.</param>
        /// <param name="context">The context that should be used (must not be null).</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static void Process<TIn1, TIn2>(TIn1 input1, TIn2 input2, ITransformationEngineContext context, GeneralTransformationRule<TIn1, TIn2> startRule)
            where TIn1 : class
            where TIn2 : class
        {
            if (context == null) throw new ArgumentNullException("context");
            var transformation = context.Transformation;
            if (!transformation.IsInitialized) transformation.Initialize();
            if (startRule == null)
            {
                startRule = context.Transformation.GetRulesForInputTypes(new Type[] { typeof(TIn1), typeof(TIn2) }).FirstOrDefault() as GeneralTransformationRule<TIn1, TIn2>;
            }
            else
            {
                if (startRule.Transformation != context.Transformation) ThrowRuleNotPartOfTransformation();
            }
            TransformationRunner.Transform(new object[] { input1, input2 }, null, startRule, context);
        }

        private static void ThrowRuleNotPartOfTransformation()
        {
            throw new InvalidOperationException(Resources.ErrTransformationEngineStartRuleNotRuleOfTransformation);
        }

        /// <summary>
        /// Processes the input arguments using the provided transformation
        /// </summary>
        /// <typeparam name="TIn1">The type of the first input argument</typeparam>
        /// <typeparam name="TIn2">The type of the second input argument</typeparam>
        /// <param name="inputs">The input parameters</param>
        /// <param name="startRule">The rule that should be started with. If this is null, an applicable rule is found.</param>
        /// <param name="context">The context that should be used (must not be null).</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static void ProcessMany<TIn1, TIn2>(IEnumerable<Tuple<TIn1, TIn2>> inputs, ITransformationEngineContext context, GeneralTransformationRule<TIn1, TIn2> startRule)
            where TIn1 : class
            where TIn2 : class
        {
            if (context == null) throw new ArgumentNullException("context");
            var transformation = context.Transformation;
            if (!transformation.IsInitialized) transformation.Initialize();
            if (startRule == null)
            {
                startRule = context.Transformation.GetRulesForInputTypes(new Type[] { typeof(TIn1), typeof(TIn2) }).FirstOrDefault() as GeneralTransformationRule<TIn1, TIn2>;
            }
            else
            {
                if (startRule.Transformation != context.Transformation) ThrowRuleNotPartOfTransformation();
            }
            TransformationRunner.TransformMany(inputs.Select((it1, it2) => new object[] { it1, it2 }), null, startRule, context);
        }

        /// <summary>
        /// Transforms the input argument into an output using the provided transformation
        /// </summary>
        /// <typeparam name="TIn">The type of the input argument</typeparam>
        /// <typeparam name="TOut">The desired output type</typeparam>
        /// <param name="input">The input parameter</param>
        /// <param name="transformation">The transformation that shall be used for this operation (must not be null, but can be uninitialized)</param>
        /// <returns>The output from the transformation</returns>
        public static TOut Transform<TIn, TOut>(TIn input, Transformation transformation)
            where TIn : class
            where TOut : class
        {
            if (transformation == null) throw new ArgumentNullException("transformation");
            return Transform<TIn, TOut>(input, new TransformationContext(transformation), null);
        }

        /// <summary>
        /// Transforms the input argument into an output using the provided transformation
        /// </summary>
        /// <typeparam name="TIn">The type of the input argument</typeparam>
        /// <typeparam name="TOut">The desired output type</typeparam>
        /// <param name="inputs">The input parameters</param>
        /// <param name="transformation">The transformation that shall be used for this operation (must not be null, but can be uninitialized)</param>
        /// <returns>The output from the transformation</returns>
        public static IEnumerable<TOut> TransformMany<TIn, TOut>(IEnumerable<TIn> inputs, Transformation transformation)
            where TIn : class
            where TOut : class
        {
            if (transformation == null) throw new ArgumentNullException("transformation");
            return TransformMany<TIn, TOut>(inputs, new TransformationContext(transformation), null);
        }

        /// <summary>
        /// Transforms the input argument into an output using the provided transformation
        /// </summary>
        /// <typeparam name="TIn">The type of the input argument</typeparam>
        /// <typeparam name="TOut">The desired output type</typeparam>
        /// <param name="input">The input parameter</param>
        /// <param name="context">The context that should be used (must not be null).</param>
        /// <returns>The output from the transformation</returns>
        public static TOut Transform<TIn, TOut>(TIn input, ITransformationEngineContext context)
            where TIn : class
            where TOut : class
        {
            return Transform<TIn, TOut>(input, context, null);
        }

        /// <summary>
        /// Transforms the input argument into an output using the provided transformation
        /// </summary>
        /// <typeparam name="TIn">The type of the input argument</typeparam>
        /// <typeparam name="TOut">The desired output type</typeparam>
        /// <param name="inputs">The input parameters</param>
        /// <param name="context">The context that should be used (must not be null).</param>
        /// <returns>The output from the transformation</returns>
        public static IEnumerable<TOut> TransformMany<TIn, TOut>(IEnumerable<TIn> inputs, ITransformationEngineContext context)
            where TIn : class
            where TOut : class
        {
            return TransformMany<TIn, TOut>(inputs, context, null);
        }

        /// <summary>
        /// Transforms the input argument into an output using the provided transformation
        /// </summary>
        /// <typeparam name="TIn">The type of the input argument</typeparam>
        /// <typeparam name="TOut">The desired output type</typeparam>
        /// <param name="input">The input parameter</param>
        /// <param name="startRule">The rule that should be started with. If this is null, an applicable rule is found.</param>
        /// <param name="context">The context that should be used (must not be null).</param>
        /// <returns>The output from the transformation</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static TOut Transform<TIn, TOut>(TIn input, ITransformationEngineContext context, TransformationRuleBase<TIn, TOut> startRule)
            where TIn : class
            where TOut : class
        {
            if (context == null) throw new ArgumentNullException("context");
            var transformation = context.Transformation;
            if (!transformation.IsInitialized) transformation.Initialize();
            if (startRule == null)
            {
                startRule = context.Transformation.GetRuleForTypeSignature(new Type[] { typeof(TIn) }, typeof(TOut)) as TransformationRuleBase<TIn, TOut>;
            }
            else
            {
                if (startRule.Transformation != context.Transformation) ThrowRuleNotPartOfTransformation();
            }
            return TransformationRunner.Transform(new object[] { input }, null, startRule, context).Output as TOut;
        }

        /// <summary>
        /// Transforms the input argument into an output using the provided transformation
        /// </summary>
        /// <typeparam name="TIn">The type of the input argument</typeparam>
        /// <typeparam name="TOut">The desired output type</typeparam>
        /// <param name="inputs">The input parameters</param>
        /// <param name="startRule">The rule that should be started with. If this is null, an applicable rule is found.</param>
        /// <param name="context">The context that should be used (must not be null).</param>
        /// <returns>The output from the transformation</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static IEnumerable<TOut> TransformMany<TIn, TOut>(IEnumerable<TIn> inputs, ITransformationEngineContext context, TransformationRuleBase<TIn, TOut> startRule)
            where TIn : class
            where TOut : class
        {
            if (context == null) throw new ArgumentNullException("context");
            var transformation = context.Transformation;
            if (!transformation.IsInitialized) transformation.Initialize();
            if (startRule == null)
            {
                startRule = context.Transformation.GetRuleForTypeSignature(new Type[] { typeof(TIn) }, typeof(TOut)) as TransformationRuleBase<TIn, TOut>;
            }
            else
            {
                if (startRule.Transformation != context.Transformation) ThrowRuleNotPartOfTransformation();
            }
            return TransformationRunner.TransformMany(inputs.Select(item => new object[] { item }), null, startRule, context).Select(c => c.Output).OfType<TOut>();
        }

        /// <summary>
        /// Processes the input argument using the provided transformation
        /// </summary>
        /// <typeparam name="TIn">The type of the input argument</typeparam>
        /// <param name="input">The input parameter</param>
        /// <param name="transformation">The transformation that shall be used for this operation (must not be null, but can be uninitialized)</param>
        public static void Process<TIn>(TIn input, Transformation transformation)
            where TIn : class
        {
            if (transformation == null) throw new ArgumentNullException("transformation");
            Process<TIn>(input, new TransformationContext(transformation), null);
        }

        /// <summary>
        /// Processes the input argument using the provided transformation
        /// </summary>
        /// <typeparam name="TIn">The type of the input argument</typeparam>
        /// <param name="inputs">The input parameter</param>
        /// <param name="transformation">The transformation that shall be used for this operation (must not be null, but can be uninitialized)</param>
        public static void ProcessMany<TIn>(IEnumerable<TIn> inputs, Transformation transformation)
            where TIn : class
        {
            if (transformation == null) throw new ArgumentNullException("transformation");
            ProcessMany<TIn>(inputs, new TransformationContext(transformation), null);
        }

        /// <summary>
        /// Processes the input argument using the provided transformation
        /// </summary>
        /// <typeparam name="TIn">The type of the input argument</typeparam>
        /// <param name="input">The input parameter</param>
        /// <param name="context">The context that should be used (must not be null)</param>
        public static void Process<TIn>(TIn input, ITransformationEngineContext context)
            where TIn : class
        {
            Process<TIn>(input, context, null);
        }

        /// <summary>
        /// Processes the input argument using the provided transformation
        /// </summary>
        /// <typeparam name="TIn">The type of the input argument</typeparam>
        /// <param name="inputs">The input parameters</param>
        /// <param name="context">The context that should be used (must not be null)</param>
        public static void ProcessMany<TIn>(IEnumerable<TIn> inputs, ITransformationEngineContext context)
            where TIn : class
        {
            ProcessMany<TIn>(inputs, context, null);
        }

        /// <summary>
        /// Processes the input argument using the provided transformation
        /// </summary>
        /// <typeparam name="TIn">The type of the input argument</typeparam>
        /// <param name="input">The input parameter</param>
        /// <param name="startRule">The rule that should be started with. If this is null, an applicable rule is found.</param>
        /// <param name="context">The context that should be used (must not be null)</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static void Process<TIn>(TIn input, ITransformationEngineContext context, GeneralTransformationRule<TIn> startRule)
            where TIn : class
        {
            if (context == null) throw new ArgumentNullException("context");
            var transformation = context.Transformation;
            if (!transformation.IsInitialized) transformation.Initialize();
            if (startRule == null)
            {
                startRule = context.Transformation.GetRulesForInputTypes(new Type[] { typeof(TIn) }).FirstOrDefault() as GeneralTransformationRule<TIn>;
            }
            else
            {
                if (startRule.Transformation != context.Transformation) ThrowRuleNotPartOfTransformation();
            }
            TransformationRunner.Transform(new object[] { input }, null, startRule, context);
        }

        /// <summary>
        /// Processes the input argument using the provided transformation
        /// </summary>
        /// <typeparam name="TIn">The type of the input argument</typeparam>
        /// <param name="inputs">The input parameters</param>
        /// <param name="startRule">The rule that should be started with. If this is null, an applicable rule is found.</param>
        /// <param name="context">The context that should be used (must not be null)</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static void ProcessMany<TIn>(IEnumerable<TIn> inputs, ITransformationEngineContext context, GeneralTransformationRule<TIn> startRule)
            where TIn : class
        {
            if (context == null) throw new ArgumentNullException("context");
            var transformation = context.Transformation;
            if (!transformation.IsInitialized) transformation.Initialize();
            if (startRule == null)
            {
                startRule = context.Transformation.GetRulesForInputTypes(new Type[] { typeof(TIn) }).FirstOrDefault() as GeneralTransformationRule<TIn>;
            }
            else
            {
                if (startRule.Transformation != context.Transformation) ThrowRuleNotPartOfTransformation();
            }
            TransformationRunner.TransformMany(inputs.Select(item => new object[] { item }), null, startRule, context);
        }
    }
}
