using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models.Repository
{
    public class ModelCollection : IDictionary<Uri, Model>
    {
        private readonly Dictionary<Uri, Model> items = new Dictionary<Uri, Model>();
        private readonly IModelRepository parent;

        public ModelCollection(IModelRepository repo)
        {
            parent = repo;
        }

        public IModelRepository Repository
        {
            get
            {
                return parent;
            }
        }

        public Model this[Uri key]
        {
            get
            {
                return items[key];
            }

            set
            {
                throw new InvalidOperationException("Loaded models must not be exchanged once loaded. Please create a new repository");
            }
        }

        public int Count
        {
            get
            {
                return items.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public ICollection<Uri> Keys
        {
            get
            {
                return items.Keys;
            }
        }

        public ICollection<Model> Values
        {
            get
            {
                return items.Values;
            }
        }

        public void Add(KeyValuePair<Uri, Model> item)
        {
            Add(item.Key, item.Value);
        }

        public virtual void Add(Uri key, Model value)
        {
            items.Add(key, value);
            value.Repository = parent;
        }

        public void Clear()
        {
            throw new InvalidOperationException("Repository must not be cleared. Please create a new one.");
        }

        public bool Contains(KeyValuePair<Uri, Model> item)
        {
            return items.ContainsKey(item.Key);
        }

        public bool ContainsKey(Uri key)
        {
            return items.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<Uri, Model>[] array, int arrayIndex)
        {
            (items as IDictionary<Uri, Model>).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<Uri, Model>> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        public bool Remove(KeyValuePair<Uri, Model> item)
        {
            throw new InvalidOperationException("Models must not be removed from the repository. Please create a new one.");
        }

        public bool Remove(Uri key)
        {
            throw new InvalidOperationException("Models must not be removed from the repository. Please create a new one.");
        }

        public bool TryGetValue(Uri key, out Model value)
        {
            return items.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
