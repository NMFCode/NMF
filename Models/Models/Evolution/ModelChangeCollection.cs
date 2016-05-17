using NMF.Models.Evolution.Minimizing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Models.Evolution
{
    public class ModelChangeCollection : IList<IModelChange>
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

        #region IList implementation

        public IModelChange this[int index]
        {
            get { return list[index]; }
            set { list[index] = value; }
        }

        public int Count { get { return list.Count; } }

        public bool IsReadOnly { get { return false; } }

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

        #endregion
    }
}
