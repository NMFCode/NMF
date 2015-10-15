using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Transformations.Core
{
    /// <summary>
    /// This class is used to call a persistor of a single dependency
    /// </summary>
    internal class SingleItemPersistor : IPersistor
    {
        /// <summary>
        /// The persistor that needs to be called
        /// </summary>
        public Action<object, object> Persistor { get; set; }

        /// <summary>
        /// The output that is passed to the persistor
        /// </summary>
        public object Output { get; set; }

        /// <summary>
        /// Persists the output of a computation
        /// </summary>
        /// <param name="output">The output of a computation</param>
        /// <remarks>This method is called by nmf transformations core</remarks>
        public void Persist(object output)
        {
            Persistor(output, Output);
        }
    }
}
