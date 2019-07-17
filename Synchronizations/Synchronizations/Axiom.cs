using System.Linq;
using System.Collections;

namespace NMF.Synchronizations
{
    internal class Axiom : IEnumerable
    {
        private object obj;

        public Axiom(object obj)
        {
            this.obj = obj;
        }

        public object Object
        {
            get { return obj; }
        }

        public IEnumerator GetEnumerator()
        {
            return Enumerable.Repeat(obj, 1).GetEnumerator();
        }
    }
}
