using System;
using System.Collections;
using System.Collections.Generic;

namespace NMF.Models.Repository
{
    /// <summary>
    /// Denotes a collection of models
    /// </summary>
    public class ModelCollection : IDictionary<Uri, Model>
    {
        private readonly Dictionary<Uri, Model> items = new Dictionary<Uri, Model>();
        private readonly IModelRepository parent;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="repo">the parent repository</param>
        public ModelCollection(IModelRepository repo)
        {
            parent = repo;
        }

        /// <summary>
        /// Gets the parent repository
        /// </summary>
        public IModelRepository Repository
        {
            get
            {
                return parent;
            }
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public int Count
        {
            get
            {
                return items.Count;
            }
        }

        /// <inheritdoc />
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <inheritdoc />
        public ICollection<Uri> Keys
        {
            get
            {
                return items.Keys;
            }
        }

        /// <inheritdoc />
        public ICollection<Model> Values
        {
            get
            {
                return items.Values;
            }
        }

        /// <inheritdoc />
        public void Add(KeyValuePair<Uri, Model> item)
        {
            Add(item.Key, item.Value);
        }

        /// <inheritdoc />
        public virtual void Add(Uri key, Model value)
        {
            items.Add(key, value);
            value.Repository = parent;
        }

        /// <inheritdoc />
        public void Clear()
        {
            throw new InvalidOperationException("Repository must not be cleared. Please create a new one.");
        }

        /// <inheritdoc />
        public bool Contains(KeyValuePair<Uri, Model> item)
        {
            return items.ContainsKey(item.Key);
        }

        /// <inheritdoc />
        public bool ContainsKey(Uri key)
        {
            return items.ContainsKey(key);
        }

        /// <inheritdoc />
        public void CopyTo(KeyValuePair<Uri, Model>[] array, int arrayIndex)
        {
            (items as IDictionary<Uri, Model>).CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<Uri, Model>> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        /// <inheritdoc />
        public bool Remove(KeyValuePair<Uri, Model> item)
        {
            throw new InvalidOperationException("Models must not be removed from the repository. Please create a new one.");
        }

        /// <inheritdoc />
        public bool Remove(Uri key)
        {
            throw new InvalidOperationException("Models must not be removed from the repository. Please create a new one.");
        }

        /// <inheritdoc />
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
