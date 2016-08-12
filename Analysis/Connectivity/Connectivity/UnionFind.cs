using NMF.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Analyses
{
    public class UnionFind<T> : Connectivity<T>
    {
        private Func<T, IEnumerable<T>> incidents;
        private Dictionary<T, T> parents = new Dictionary<T, T>();

        public UnionFind(Func<T, IEnumerable<T>> incidents, IEnumerable<T> rootNodes)
        {
            this.incidents = incidents;
            if (rootNodes != null)
            {
                foreach (var node in rootNodes)
                {
                    AddNode(node);
                }
            }
        }

        public UnionFind(Func<T, IEnumerable<T>> incidents, params T[] rootNodes)
            : this(incidents, (IEnumerable<T>)rootNodes) { }

        private T FindAndUpdate(T value)
        {
            T saved;
            if (parents.TryGetValue(value, out saved))
            {
                if (EqualityComparer<T>.Default.Equals(value, saved))
                {
                    return value;
                }
                var result = FindAndUpdate(saved);
                if (!EqualityComparer<T>.Default.Equals(saved, result))
                {
                    parents[value] = result;
                }
                return result;
            }
            else
            {
                return value;
            }
        }

        private int NotConnectedToSelf
        {
            get
            {
                return parents.Where(pair => EqualityComparer<T>.Default.Equals(pair.Key, pair.Value)).Count();
            }
        }

        public void AddNode(T value)
        {
            if (!parents.ContainsKey(value))
            {
                parents.Add(value, value);
                var targets = incidents(value);
                if (targets != null)
                {
                    foreach (var target in targets)
                    {
                        AddNode(target);
                        parents[value] = FindAndUpdate(target);
                    }
                }
            }
        }

        public override bool AreConnected(T source, T target)
        {
            return EqualityComparer<T>.Default.Equals(FindAndUpdate(source), FindAndUpdate(target));
        }

        protected override INotifyValue<bool> AreConnectedInc(T a, T b)
        {
            throw new NotSupportedException();
        }
    }
}
