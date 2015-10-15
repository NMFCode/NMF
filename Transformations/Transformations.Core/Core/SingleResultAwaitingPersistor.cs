using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Transformations.Core
{
    /// <summary>
    /// This class is used for single dependencies when something is delayed to wait for the result of the base computation or the output of the dependent computation
    /// </summary>
    internal class SingleResultAwaitingPersistor : IPersistor
    {
        /// <summary>
        /// Gets the method that persists the output of the dependent computation
        /// </summary>
        public Action<object, object> Persistor { get; private set; }

        /// <summary>
        /// Gets the output of the dependent computation
        /// </summary>
        public object Result { get; private set; }

        /// <summary>
        /// Gets the output of the base computation
        /// </summary>
        public object Target { get; private set; }

        /// <summary>
        /// Creates a new awaitor with the given persist method
        /// </summary>
        /// <param name="persistor">A method that has to be called as soon as the outputs are available</param>
        public SingleResultAwaitingPersistor(Action<object, object> persistor)
        {
            this.Persistor = persistor;
            waitCounter = 2;
        }

        /// <summary>
        /// Creates a new awaitor with the given persist method
        /// </summary>
        /// <param name="persistor">A method that has to be called as soon as the outputs are available</param>
        /// <param name="target">The output of the base computation</param>
        public SingleResultAwaitingPersistor(Action<object, object> persistor, object target)
        {
            this.Persistor = persistor;
            this.Target = target;
            waitCounter = 1;
        }

        private sbyte waitCounter;

        /// <summary>
        /// Persists the output of a computation
        /// </summary>
        /// <param name="output">The output of a computation</param>
        /// <remarks>This method is called by nmf transformations core</remarks>
        public void Persist(object output)
        {
            Target = output;
            waitCounter--;
            EventuallyPersist();
        }

        private void EventuallyPersist()
        {
            if (waitCounter == 0)
                Persistor(Target, Result);
        }

        /// <summary>
        /// Waits for the given computation to initialize its output
        /// </summary>
        /// <param name="comp">The computation to wait for</param>
        public void WaitFor(Computation comp)
        {
            comp.OutputInitialized += comp_OutputInitialized;
        }

        void comp_OutputInitialized(object sender, EventArgs e)
        {
            var comp = sender as Computation;
            comp.OutputInitialized -= comp_OutputInitialized;
            Result = comp.Output;
            waitCounter--;
            EventuallyPersist();
        }
    }
}
