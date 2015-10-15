using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using NMF.Transformations.Core.Properties;
using System.Collections;
using System.Threading;

namespace NMF.Transformations.Core
{
    /// <summary>
    /// This class represents a single computation within the transformation engine of NMF.Transformations
    /// </summary>
    /// <remarks>This class is visible to public as it provides reaction to delayness</remarks>
    public abstract class Computation : IPersistor, ITraceEntry
    {
        /// <summary>
        /// Creates a new computation for the given transformation rule with the given input arguments
        /// </summary>
        /// <param name="transformationRule">The transformation rule responsible for the transformation of the input data</param>
        /// <param name="context">The transformation context, in which the computation is done</param>
        protected Computation(GeneralTransformationRule transformationRule, IComputationContext context)
        {
            if (transformationRule == null) throw new ArgumentNullException("transformationRule");
            if (context == null) throw new ArgumentNullException("context");

            TransformationRule = transformationRule;
            Context = context;

            context.ConnectWith(this);
        }

        /// <summary>
        /// Gets the transformation rule, which has been used to compute this computation
        /// </summary>
        public GeneralTransformationRule TransformationRule { get; private set; }

        /// <summary>
        /// Gets the context, in which the computation has been made
        /// </summary>
        public ITransformationContext TransformationContext { get { return Context.TransformationContext; } }

        /// <summary>
        /// Gets the computation context for this computation
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public IComputationContext Context { get; private set; }

        /// <summary>
        /// Gets the number of input arguments
        /// </summary>
        public int InputArguments { get { return TransformationRule.InputType.Length; } }

        /// <summary>
        /// Gets the input argument for this computation with the given index
        /// </summary>
        /// <param name="index">The index of the input parameter</param>
        /// <returns>The input parameter at the ith position</returns>
        public abstract object GetInput(int index);

        /// <summary>
        /// Copies the input of this computation into an array
        /// </summary>
        /// <returns>An array with the inputs</returns>
        public object[] CreateInputArray()
        {
            object[] inputs = new object[TransformationRule.InputType.Length];
            for (int i = 0; i < TransformationRule.InputType.Length; i++)
            {
                inputs[i] = GetInput(i);
            }
            return inputs;
        }
        
        /// <summary>
        /// Gets the output of this computation
        /// </summary>
        /// <exception cref="InvalidOperationException">This property may throw a DelayedOutputCreationException in case that the output has been tried to access, although the output creation was delayed</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public object Output
        {
            get
            {
                if (Context.IsDelayed) ThrowDelayedException();
                return OutputCore;
            }
        }

        /// <summary>
        /// Throws an exception that the output has been accessed although the output of teh computation is delayed
        /// </summary>
        [DebuggerStepThrough]
        protected static void ThrowDelayedException()
        {
            throw new InvalidOperationException(Resources.ErrComputationOutputDelayed);
        }

        /// <summary>
        /// Marks that this computations requires another to be transformed.
        /// </summary>
        /// <param name="other">The other computation</param>
        /// <param name="isRequired">A value indicating whether the other computation must be execute before or after the current computation</param>
        /// <param name="dependency">The dependency that required this</param>
        /// <remarks>The default implementation does nothing, so feel free to override. This method is intended to be called by NMF.Transformations, only.</remarks>
        public virtual void MarkRequire(Computation other, bool isRequired, ITransformationRuleDependency dependency) { }

        internal void MarkRequireInternal(Computation other, bool isRequired, ITransformationRuleDependency dependency)
        {
            if (other != null)
            {
                MarkRequire(other, isRequired, dependency);
                Context.MarkRequire(other, isRequired);
            }
        }

        /// <summary>
        /// Gets or sets the output in a derived class
        /// </summary>
        protected abstract object OutputCore { get; set; }

        /// <summary>
        /// This event is fired as soon as the output of this computation is initialized
        /// </summary>
        public event EventHandler OutputInitialized;

        /// <summary>
        /// This event is fired as soon as the computation has been computed,
        /// i.e., the computation has been processed in the computation list
        /// </summary>
        public event EventHandler Computed;

        /// <summary>
        /// Configures the computation to create its output at minimum with the given delay level
        /// </summary>
        /// <param name="delayLevel">The minimum delay level for this computation</param>
        public void DelayOutputAtLeast(byte delayLevel)
        {
            Context.DelayOutputAtLeast(delayLevel);
        }

        /// <summary>
        /// Configures the computation to be transformed at least with the given delay level
        /// </summary>
        /// <param name="delayLevel"></param>
        public void DelayTransformationAtLeast(byte delayLevel)
        {
            Context.DelayTransformationAtLeast(delayLevel);
        }

        /// <summary>
        /// Fires the <see cref="OutputInitialized"/>-event
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected virtual void OnOutputInitialized(EventArgs e)
        {
            var handler = Interlocked.Exchange(ref OutputInitialized, null);
            if (handler != null)
            {
                handler(this, e);
            }
        }

        internal OutputDelay OutputDelay
        {
            get
            {
                var cc = Context as ComputationContext;
                return cc.OutputDelay;
            }
        }


        /// <summary>
        /// Gets the output delay level
        /// </summary>
        internal byte OutputDelayLevel
        {
            get
            {
                var cc = Context as ComputationContext;
                return cc.OutputDelayLevel;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the output creation for this computation is delayed
        /// </summary>
        public bool IsDelayed { get { return Context.IsDelayed; } }

        /// <summary>
        /// Fires the <see cref="Computed"/>-event
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected virtual void OnComputed(EventArgs e)
        {
            var handler = Interlocked.Exchange(ref Computed, null);
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Initializes the transformation output
        /// </summary>
        /// <remarks>At this point, all the transformation outputs are created (also the delayed ones), thus, the trace is fully reliable</remarks>
        public abstract void Transform();

        /// <summary>
        /// Creates the output of this transformation rule
        /// </summary>
        /// <returns>The output for this transformation under this input</returns>
        /// <remarks>At this point, not all of the computations have created their output and thus, the trace is not complete. Use the OutputDelayLevel-feature to have the trace contain all elements created in earlier levels</remarks>
        public abstract object CreateOutput(IEnumerable context);

        /// <summary>
        /// Initializes the output for the actual computation
        /// </summary>
        /// <param name="output">The intended output</param>
        public void InitializeOutput(object output)
        {
            OutputCore = output;
            Context.InitializeOutput(output);
            OnOutputInitialized(EventArgs.Empty);
        }

        public virtual void SetBaseComputation(Computation baseComputation) { }

        void IPersistor.Persist(object output)
        {
            InitializeOutput(output);
        }
    }
}
