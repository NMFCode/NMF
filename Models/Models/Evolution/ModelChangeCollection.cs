using NMF.Models.Evolution.Minimizing;
using NMF.Models.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Models.Evolution
{
    public class ModelChangeCollection : IList<IModelChange>, ICollection
    {
        private readonly List<IModelChange> list;

        public ModelChangeCollection() : this(new List<IModelChange>()) { }

        internal ModelChangeCollection(List<IModelChange> list)
        {
            this.list = list;
        }

        public ModelChangeCollection Minimize()
        {
            var strategies = new[] { new MultiplePropertyChanges() };

            var localList = list.ToList();
            foreach (var strat in strategies)
                localList = strat.Execute(localList);

            return new ModelChangeCollection(localList);
        }

        public Task<ModelChangeCollection> MinimizeAsync()
        {
            return Task.Factory.StartNew(Minimize);
        }

        public void Apply(IModelRepository repository)
        {
            foreach (var change in list)
                change.Apply(repository);
        }

        #region Interface implementation

        public IModelChange this[int index]
        {
            get { return list[index]; }
            set { list[index] = value; }
        }

        public int Count { get { return list.Count; } }

        public bool IsReadOnly { get { return false; } }

        public object SyncRoot
        {
            get
            {
                return ((ICollection)list).SyncRoot;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return ((ICollection)list).IsSynchronized;
            }
        }

        public void Add(IModelChange item)
        {
            list.Add(item);
        }

        public void Clear()
        {
            list.Clear();
        }

        public bool Contains(IModelChange item)
        {
            return list.Contains(item);
        }

        public void CopyTo(IModelChange[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public IEnumerator<IModelChange> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public int IndexOf(IModelChange item)
        {
            return list.IndexOf(item);
        }

        public void Insert(int index, IModelChange item)
        {
            list.Insert(index, item);
        }

        public bool Remove(IModelChange item)
        {
            return list.Remove(item);
        }

        public void RemoveAt(int index)
        {
            list.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public void CopyTo(Array array, int index)
        {
            ((ICollection)list).CopyTo(array, index);
        }

        #endregion
    }
}
