using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using NMF.Models;

namespace PortV3Namespace
{
    public class OutputModelCollection<T> : ICollection<T> where T : IModelElement
    {
        private readonly ICollection<T> collection;

        public OutputModelCollection(ICollection<T> collection)
        {
            this.collection = collection;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            if (item.Parent == null)
            {
                collection.Add(item);
            }
        }

        public void Clear()
        {
            collection.Clear();
        }

        public bool Contains(T item)
        {
            return collection.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException("The CopyTo function of the OutputModelCollection Class is not implemented yet");
        }

        public bool Remove(T item)
        {
            if (collection.Remove(item))
            {
                item.Delete();
                return true;
            }

            return false;
        }

        public int Count
        {
            get { return collection.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }
    }
}
