using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Transformations.Core
{
    /// <summary>
    /// This interface represents the transformation engine context information attached to a computation
    /// </summary>
    public interface IComputationContext
    {
        /// <summary>
        /// The transformation context in which the computation is made
        /// </summary>
        ITransformationContext TransformationContext { get; }

        /// <summary>
        /// Mark that this computation context requires another computation context to be done
        /// </summary>
        /// <param name="other">The other computation context</param>
        /// <param name="isRequired">True, if the other context is a strict requirement</param>
        void MarkRequire(Computation other, bool isRequired);

        /// <summary>
        /// Gets a value indicating whether this computation is delayed
        /// </summary>
        bool IsDelayed { get; }

        /// <summary>
        /// Initializes the output of this computation
        /// </summary>
        /// <param name="output"></param>
        void InitializeOutput(object output);


        /// <summary>
        /// Configures the computation to create its output at minimum with the given delay level
        /// </summary>
        /// <param name="delayLevel">The minimum delay level for this computation</param>
        void DelayOutputAtLeast(byte delayLevel);

        /// <summary>
        /// Configures the computation to be transformed at least with the given delay level
        /// </summary>
        /// <param name="delayLevel"></param>
        void DelayTransformationAtLeast(byte delayLevel);

        /// <summary>
        /// Gets the minimum viable output delay level
        /// </summary>
        byte MinOutputDelayLevel { get; }

        /// <summary>
        /// Gets the minimum viable transformation delay level
        /// </summary>
        byte MinTransformDelayLevel { get; }

        /// <summary>
        /// Connects the computation context with the given computation
        /// </summary>
        /// <param name="computation">The computation thst is handled by this computation context</param>
        void ConnectWith(Computation computation);
    }

    /// <summary>
    /// The default implementation for a computation context
    /// </summary>
    public class ComputationContext : IComputationContext
    {
        private ITransformationContext context;

        /// <summary>
        /// Creates a new computation context for the given transformation context
        /// </summary>
        /// <param name="context">The transformation context</param>
        public ComputationContext(ITransformationContext context)
        {
            this.context = context;
        }


        /// <summary>
        /// The transformation context in which the computation is made
        /// </summary>
        public ITransformationContext TransformationContext
        {
            get { return context; }
        }


        /// <summary>
        /// Mark that this computation context requires another computation context to be done
        /// </summary>
        /// <param name="other">The other computation context</param>
        /// <param name="isRequired">True, if the other context is a strict requirement</param>
        public virtual void MarkRequire(Computation other, bool isRequired)
        {
            if (other == null) throw new ArgumentNullException("other");
            var otherContext = other.Context;
            if (isRequired && otherContext != this)
            {
                DelayOutputAtLeast(otherContext.MinOutputDelayLevel);
                DelayTransformationAtLeast(otherContext.MinTransformDelayLevel);
            }
        }

        internal OutputDelay OutputDelay { get; private set; }


        /// <summary>
        /// Gets a value indicating whether the output creation for this computation is delayed
        /// </summary>
        public bool IsDelayed { get { return OutputDelay != null; } }



        /// <summary>
        /// Gets the output delay level
        /// </summary>
        internal byte OutputDelayLevel
        {
            get
            {
                if (OutputDelay == null)
                {
                    return 0;
                }
                else
                {
                    return OutputDelay.DelayLevel;
                }
            }
        }

        /// <summary>
        /// Configures the computation to create its output at minimum with the given delay level
        /// </summary>
        /// <param name="delayLevel">The minimum delay level for this computation</param>
        public void DelayOutputAtLeast(byte delayLevel)
        {
            MinOutputDelayLevel = Math.Max(MinOutputDelayLevel, delayLevel);
        }

        /// <summary>
        /// Configures the computation to be transformed at least with the given delay level
        /// </summary>
        /// <param name="delayLevel"></param>
        public void DelayTransformationAtLeast(byte delayLevel)
        {
            MinTransformDelayLevel = Math.Max(MinTransformDelayLevel, delayLevel);
        }

        /// <summary>
        /// Gets the minimum output delay level for this cmputation
        /// </summary>
        public byte MinOutputDelayLevel { get; private set; }

        /// <summary>
        /// Gets the minimum transformation delay level for this computation
        /// </summary>
        public byte MinTransformDelayLevel { get; private set; }

        /// <summary>
        /// Delays the ouput of this computation with the given output delay
        /// </summary>
        /// <param name="delay"></param>
        internal void DelayOutput(OutputDelay delay)
        {
            if (delay == null) throw new ArgumentNullException("delay");

            if (OutputDelay != null)
            {
                delay.Persistors.AddRange(OutputDelay.Persistors);
            }
            OutputDelay = delay;
        }


        public void InitializeOutput(object output)
        {
            if (OutputDelay != null)
            {
                OutputDelay.ExecutePersistors(output);
            }
            OutputDelay = null;
        }

        /// <summary>
        /// Connects the computation context with the given computation
        /// </summary>
        /// <param name="computation">The computation thst is handled by this computation context</param>
        public virtual void ConnectWith(Computation computation) { }
    }
}
