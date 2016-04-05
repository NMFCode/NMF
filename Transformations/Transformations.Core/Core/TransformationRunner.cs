using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace NMF.Transformations.Core
{
    /// <summary>
    /// Service class to run transformations
    /// </summary>
    public static class TransformationRunner
    {

        /// <summary>
        /// Transforms the input argument into an output using the provided transformation
        /// </summary>
        /// <param name="input">The input arguments as an array. This must not be null. The correct amount of parameters depends on the rule to start with.</param>
        /// <param name="inputContext">The context in which the transformation rule is executed</param>
        /// <param name="startRule">The start rule to begin with (must not be null)</param>
        /// <param name="context">The transformation context (must not be null)</param>
        /// <returns>The transformation computation</returns>
        public static Computation Transform(object[] input, IEnumerable inputContext, GeneralTransformationRule startRule, ITransformationEngineContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (startRule == null) throw new InvalidOperationException("Could not find transaction rule to start with.");
            if (!context.Transformation.IsInitialized) throw new InvalidOperationException("Could not initialize transformation");
            context.Inputs.Add(input);
            var patternObjects = new List<ITransformationPatternContext>();
            foreach (var pattern in context.Transformation.Patterns)
            {
                var obj = pattern.CreatePattern(context);
                if (obj != null) patternObjects.Add(obj);
            }
            var comp = context.CallTransformation(startRule, input, inputContext);
            if (startRule.OutputType != typeof(void))
            {
                if (!comp.IsDelayed)
                {
                    context.Outputs.Add(comp.Output);
                }
                else
                {
                    comp.OutputInitialized += (s, e) => context.Outputs.Add(comp.Output);
                }
            }
            context.ExecutePending();
            foreach (var pattern in patternObjects)
            {
                pattern.Begin();
            }
            context.ExecutePending();
            foreach (var pattern in patternObjects)
            {
                pattern.Finish();
            }
            return comp;
        }

        /// <summary>
        /// Transforms the input argument into an output using the provided transformation
        /// </summary>
        /// <param name="inputs">The input arguments as an array. This must not be null. The correct amount of parameters depends on the rule to start with.</param>
        /// <param name="inputContext">The context object in which the transformation is run</param>
        /// <param name="startRule">The start rule to begin with (must not be null)</param>
        /// <param name="context">The transformation context (must not be null)</param>
        /// <returns>The transformation computations</returns>
        public static IEnumerable<Computation> TransformMany(IEnumerable<object[]> inputs, IEnumerable inputContext, GeneralTransformationRule startRule, ITransformationEngineContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (inputs == null) throw new ArgumentNullException("inputs");
            if (startRule == null) throw new InvalidOperationException("Could not find transaction rule to start with.");
            if (!context.Transformation.IsInitialized) throw new InvalidOperationException("Could not initialize transformation");
            var patternObjects = new List<ITransformationPatternContext>();
            foreach (var pattern in context.Transformation.Patterns)
            {
                var obj = pattern.CreatePattern(context);
                if (obj != null) patternObjects.Add(obj);
            }
            var list = new List<Computation>();
            foreach (var input in inputs)
            {
                var comp = context.CallTransformation(startRule, input, inputContext);
                list.Add(comp);
                if (!comp.IsDelayed)
                {
                    context.Outputs.Add(comp.Output);
                }
                else
                {
                    comp.OutputInitialized += (o,e) => context.Outputs.Add(comp.Output);
                }
            }
            context.ExecutePending();
            foreach (var pattern in patternObjects)
            {
                pattern.Begin();
                context.ExecutePending();
            }
            foreach (var pattern in patternObjects)
            {
                pattern.Finish();
            }
            return list;
        }

    }
}
