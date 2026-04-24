using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace NMF.Models.Repository
{
    /// <summary>
    /// Denotes a collection of models
    /// </summary>
    public class ModelCollection : IDictionary<Uri, Model>
    {
        private readonly Dictionary<Uri, Model> items = new Dictionary<Uri, Model>();
        private readonly IModelRepository parent;
        private readonly ValuesCollection values = new ValuesCollection();

        /// <summary>
        /// true, if elements in the model collection may be overridden, otherwise false
        /// </summary>
        protected virtual bool AllowOverride => false;

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
                if (AllowOverride)
                {
                    items[key] = value;
                }
                else
                {
                    throw new InvalidOperationException("Loaded models must not be exchanged once loaded. Please create a new repository");
                }
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
                return !AllowOverride;
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
                return values;
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
            values.AddModelInternal(value);
        }

        /// <inheritdoc />
        public void Clear()
        {
            if (!AllowOverride)
            {
                throw new InvalidOperationException("Repository must not be cleared. Please create a new one.");
            }
            items.Clear();
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
            if (AllowOverride)
            {
                return items.Remove(item.Key);
            }
            throw new InvalidOperationException("Models must not be removed from the repository. Please create a new one.");
        }

        /// <inheritdoc />
        public bool Remove(Uri key)
        {
            if (AllowOverride)
            {
                return items.Remove(key);
            }
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

        private class ValuesCollection : ICollection<Model>, INotifyCollectionChanged
        {
            private readonly HashSet<Model> _models = new HashSet<Model>();

            public int Count => _models.Count;

            public bool IsReadOnly => true;

            public event NotifyCollectionChangedEventHandler CollectionChanged;

            public void Add(Model item)
            {
                throw new NotSupportedException();
            }

            public void Clear()
            {
                throw new NotSupportedException();
            }

            public bool Contains(Model item)
            {
                return _models.Contains(item);
            }

            public void CopyTo(Model[] array, int arrayIndex)
            {
                _models.CopyTo(array, arrayIndex);
            }

            public IEnumerator<Model> GetEnumerator()
            {
                return _models.GetEnumerator();
            }

            public bool Remove(Model item)
            {
                throw new NotSupportedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public void AddModelInternal(Model model)
            {
                if (_models.Add(model))
                {
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, model));
                }
            }
        }
    }
}
