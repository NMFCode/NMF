using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NMF.Transformations.Core
{
    /// <summary>
    /// This class is used to represent delayed output creations
    /// </summary>
    internal struct DelayedOutputCreation
    {
        private List<Computation> Computations;
        private IEnumerable Context;

        public DelayedOutputCreation(List<Computation> computations, IEnumerable context)
        {
            Computations = computations;
            Context = context;
        }

        public void CreateDelayedOutput(bool callTransformations)
        {
            var peek = Computations[0];
            var output = peek.CreateOutput(Context);
            for (int i = Computations.Count - 1; i >= 0; i--)
            {
                Computations[i].InitializeOutput(output);
            }
            if (callTransformations)
            {
                for (int i = Computations.Count - 1; i >= 0; i--)
                {
                    Computations[i].Transform();
                }
            }
        }
    }

    /// <summary>
    /// Represents a delay of a computation
    /// </summary>
    internal class OutputDelay
    {
        /// <summary>
        /// Creates a new output delay with the given list of persistors
        /// </summary>
        public OutputDelay()
        {
            this.Persistors = new List<IPersistor>();
        }

        public void ExecutePersistors(object output)
        {
            foreach (var item in Persistors)
            {
                if (item != null) item.Persist(output);
            }
            Persistors.Clear();
        }

        /// <summary>
        /// Gets a list of persistors that need to be called as soon as the output has been created
        /// </summary>
        public List<IPersistor> Persistors { get; private set; }
        
        /// <summary>
        /// Gets the delay level of the current delay
        /// </summary>
        public byte DelayLevel { get; set; }
    }
}
