using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace NMF.Transformations.Core
{
    /// <summary>
    /// This is the basic interface for a transformation context, providing a full trace
    /// </summary>
    public interface ITransformationContext
    {
        bool IsThreadSafe { get; }

        /// <summary>
        /// Gets a Bag, where dynamic data can be added
        /// </summary>
        /// <remarks>The value of this property is an ExpandoObject, so that the bag can be easily extended with new properties</remarks>
        dynamic Bag { get; }

        /// <summary>
        /// Gets a data dictionary, where data set during the transformation can be added
        /// </summary>
        IDictionary<object, object> Data { get; }

        /// <summary>
        /// Gets the parent transformation, that the context is based upon
        /// </summary>
        Transformation Transformation { get; }

        /// <summary>
        /// Gets all computations (for custom trace purposes)
        /// </summary>
        IEnumerable<Computation> Computations { get; }

        /// <summary>
        /// Calls the given transformation with the specified input
        /// </summary>
        /// <param name="transformationRule">The rule that should be applied</param>
        /// <param name="context">The context in which the transformation rule is executed</param>
        /// <param name="input">The input for the transformation rule</param>
        /// <returns>The computation that handles this request</returns>
        Computation CallTransformation(GeneralTransformationRule transformationRule, object[] input, IEnumerable context);

        /// <summary>
        /// Gets the object responsible for trace operations for this transformation context
        /// </summary>
        ITransformationTrace Trace { get; }

        /// <summary>
        /// Gets a collection of inputs
        /// </summary>
        IList<object[]> Inputs { get; }

        /// <summary>
        /// Gets or sets the single input collection of this transformation
        /// </summary>
        object[] Input { get; set; }

        /// <summary>
        /// Gets a collection of outputs
        /// </summary>
        IList<object> Outputs { get; }

        /// <summary>
        /// Gets fired when a computation is done
        /// </summary>
        event EventHandler<ComputationEventArgs> ComputationCompleted;
    }

    /// <summary>
    /// Extensions for the transformation context
    /// </summary>
    public static class TransformationContextExtensions
    {
        /// <summary>
        /// Calls the given transformation with the specified input
        /// </summary>
        /// <param name="context">The current transformation context</param>
        /// <param name="transformationRule">The rule that should be applied</param>
        /// <param name="input">The input for the transformation rule</param>
        /// <returns>The computation that handles this request</returns>
        public static Computation CallTransformation(this ITransformationContext context, GeneralTransformationRule transformationRule, params object[] input)
        {
            return context.CallTransformation(transformationRule, input, null);
        }
    }

    /// <summary>
    /// Represents the data for events that belong to certain computations
    /// </summary>
    public class ComputationEventArgs : EventArgs
    {
        /// <summary>
        /// Creates new event data for the given computation
        /// </summary>
        /// <param name="computation">The computation</param>
        public ComputationEventArgs(Computation computation)
        {
            Computation = computation;
        }

        /// <summary>
        /// Gets the computation
        /// </summary>
        public Computation Computation { get; private set; }
    }
}
