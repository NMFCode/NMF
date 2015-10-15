using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Transformations.Core
{
    /// <summary>
    /// This class is used to persist call dependencies on multiple objects, in case anything is delayed
    /// </summary>
    internal class MultipleResultAwaitingPersistor : IPersistor
    {
        /// <summary>
        /// Gets the method that needs to be called
        /// </summary>
        public Action<object, IEnumerable> Persistor { get; set; }

        /// <summary>
        /// Gets the collection of dependent outputs
        /// </summary>
        public IList List { get; set; }

        /// <summary>
        /// Gets a value indicating how many outputs need to complete before the selector is called
        /// </summary>
        public int Remaining { get; set; }

        /// <summary>
        /// The output of the base computation
        /// </summary>
        public object Target { get; set; }

        /// <summary>
        /// Persists the output of a computation
        /// </summary>
        /// <param name="output">The output of a computation</param>
        /// <remarks>This method is called by nmf transformations core</remarks>
        public void Persist(object output)
        {
            Target = output;
            EventuallyPersist();
        }

        private void EventuallyPersist()
        {
            if (Remaining == 0 && Target != null)
            {
                Persistor(Target, List);
            }
        }

        /// <summary>
        /// Waits for the given computation to initialize its output
        /// </summary>
        /// <param name="comp">The computation to wait for</param>
        public void WaitFor(Computation comp)
        {
            Remaining++;
            comp.OutputInitialized += ItemInitialized;
        }

        private void ItemInitialized(object sender, EventArgs e)
        {
            var comp = sender as Computation;
            comp.OutputInitialized -= ItemInitialized;
            if (comp.Output != null)
            {
                List.Add(comp.Output);
            }
            Remaining--;
            EventuallyPersist();
        }
    }
}
