using NMF.Transformations.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace NMF.Transformations.Tests
{
    /// <summary>
    /// Represents a mock for the transformation context that does not execute any dependencies
    /// </summary>
    public class MockContext : ITransformationContext
    {
        /// <summary>
        /// Creates a new MockContext for the given transformation
        /// </summary>
        /// <param name="transformation">The transformation for which the mock context should be created</param>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown whenever the transformation parameter is passed a null reference.</exception>
        public MockContext(Transformation transformation)
        {
            if (transformation == null) throw new ArgumentNullException("transformation");

            this.transformation = transformation;
            transformation.Initialize();
            computations = new MockComputationCollection(this);
            trace = new Trace(computations);
        }

        private Transformation transformation;
        private ExpandoObject bag = new ExpandoObject();
        private Dictionary<object, object> data = new Dictionary<object, object>();
        private MockComputationCollection computations;
        private Trace trace;

        private List<object[]> inputs = new List<object[]>();
        private List<object> outputs = new List<object>();

        /// <summary>
        /// Gets a Bag, where dynamic data can be added
        /// </summary>
        /// <remarks>The value of this property is an ExpandoObject, so that the bag can be easily extended with new properties</remarks>
        public dynamic Bag
        {
            get { return bag; }
        }

        /// <summary>
        /// Gets a data dictionary, where data set during the transformation can be added
        /// </summary>
        public IDictionary<object, object> Data
        {
            get { return data; }
        }

        /// <summary>
        /// Gets the parent transformation, that the context is based upon
        /// </summary>
        public Transformation Transformation
        {
            get { return transformation; }
        }

        /// <summary>
        /// Gets all computations (for custom trace purposes)
        /// </summary>
        public MockComputationCollection Computations
        {
            get { return computations; }
        }

        /// <summary>
        /// Gets all computations (for custom trace purposes)
        /// </summary>
        IEnumerable<Computation> ITransformationContext.Computations
        {
            get { return computations.OfType<Computation>(); }
        }

        /// <summary>
        /// Executes the dependencies of the given computation
        /// </summary>
        /// <param name="computation">The computation whose dependencies should be executed</param>
        /// <param name="before">A value indicating whether the dependencies before the computation or the dependencies after the computation should be executed</param>
        public virtual void ExecuteDependencies(Computation computation, bool before)
        {
            if (computation == null) throw new ArgumentNullException("computation");
            foreach (var dep in computation.TransformationRule.Dependencies)
            {
                if (dep.ExecuteBefore == before) dep.HandleDependency(computation);
            }
        }

        /// <summary>
        /// Calls the given transformation with the specified input
        /// </summary>
        /// <param name="input">The input for the transformation rule</param>
        /// <param name="transformationRule">The rule that should be applied</param>
        /// <returns>The computation that handles this request</returns>
        public Computation CallTransformation(GeneralTransformationRule transformationRule, object[] input)
        {
            return CallTransformation(transformationRule, null, input);
        }

        /// <summary>
        /// Calls the given transformation with the specified input
        /// </summary>
        /// <param name="input">The input for the transformation rule</param>
        /// <param name="transformationRule">The rule that should be applied</param>
        /// <returns>The computation that handles this request</returns>
        public virtual Computation CallTransformation(GeneralTransformationRule transformationRule, object[] input, IEnumerable context)
        {
            if (transformationRule == null) throw new ArgumentNullException("transformationRule");

            var c = Trace.TraceIn(transformationRule, input).OfType<Computation>().FirstOrDefault();
            if (c == null)
            {
                c = transformationRule.CreateComputation(input, new ComputationContext(this));
                computations.Add(c);
            }
            return c;
        }

        /// <summary>
        /// Calls the given transformation with the specified input
        /// </summary>
        /// <typeparam name="TIn">The type of the first input parameter</typeparam>
        /// <param name="input">The input for the transformation rule</param>
        /// <param name="transformationRule">The rule that should be applied</param>
        /// <returns>The computation that handles this request</returns>
        public Computation CallTransformation<TIn>(GeneralTransformationRule<TIn> transformationRule, TIn input)
            where TIn : class
        {
            return CallTransformation(transformationRule, null, new object[] { input });
        }

        /// <summary>
        /// Calls the given transformation with the specified input
        /// </summary>
        /// <typeparam name="TIn1">The type of the first input parameter</typeparam>
        /// <typeparam name="TIn2">The type of the second input parameter</typeparam>
        /// <param name="input1">The first input for the transformation rule</param>
        /// <param name="input2">The second input for the transformation rule</param>
        /// <param name="transformationRule">The rule that should be applied</param>
        /// <returns>The computation that handles this request</returns>
        public Computation CallTransformation<TIn1, TIn2>(GeneralTransformationRule<TIn1, TIn2> transformationRule, TIn1 input1, TIn2 input2)
            where TIn1 : class
            where TIn2 : class
        {
            return CallTransformation(transformationRule, null, new object[] { input1, input2 });
        }

        /// <summary>
        /// Gets the object responsible for trace operations for this transformation context
        /// </summary>
        public ITransformationTrace Trace
        {
            get { return trace; }
        }

        /// <summary>
        /// Gets the input of the transformation context
        /// </summary>
        /// <remarks>If the transformation has multiple inputs, this returns the first input</remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public object[] Input
        {
            get { return Inputs.FirstOrDefault(); }
            set
            {
                inputs.Clear();
                inputs.Add(value);
            }
        }

        /// <summary>
        /// Gets a collection of inputs
        /// </summary>
        public IList<object[]> Inputs
        {
            get { return inputs; }
        }

        /// <summary>
        /// Gets the output of the transformation context
        /// </summary>
        /// <remarks>If the transformation has multiple outputs, this property returns the first output</remarks>
        public object Output
        {
            get { return outputs.FirstOrDefault(); }
        }

        /// <summary>
        /// Gets a collection of outputs
        /// </summary>
        public IList<object> Outputs
        {
            get { return outputs; }
        }


        public event EventHandler<ComputationEventArgs> ComputationCompleted
        {
            add { }
            remove { }
        }


        public void RegisterComputationDependency(Computation computation, Computation dependency, bool isRequired)
        {
        }

        public bool IsThreadSafe
        {
            get { return false; }
        }
    }
}
