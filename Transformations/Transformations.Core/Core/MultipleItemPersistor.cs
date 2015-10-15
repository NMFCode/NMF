using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Transformations.Core
{
    /// <summary>
    /// This class is used for persistance of multiple dependencies
    /// </summary>
    internal class MultipleItemPersistor : IPersistor
    {
        public Action<object, IEnumerable> Persistor { get; set; }
        public IList List { get; set; }

        public void Persist(object output)
        {
            Persistor(output, List);
        }
    }
}
