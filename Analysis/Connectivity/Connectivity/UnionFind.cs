using NMF.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Analyses
{
    /// <summary>
    /// Denotes a connectivity implementation based on UnionFind
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class UnionFind<T> : Connectivity<T>
    {
        private readonly Func<T, IEnumerable<T>> incidents;
        private readonly Dictionary<T, T> parents = new Dictionary<T, T>();

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="incidents">A function selecting the incidents</param>
        /// <param name="rootNodes">A collection of root nodes</param>
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

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="incidents">A function selecting the incidents</param>
        /// <param name="rootNodes">A collection of root nodes</param>
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

        /// <summary>
        /// Adds the given node
        /// </summary>
        /// <param name="value">The node to add</param>
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

        /// <inheritdoc />
        public override bool AreConnected(T source, T target)
        {
            return EqualityComparer<T>.Default.Equals(FindAndUpdate(source), FindAndUpdate(target));
        }

        /// <inheritdoc />
        protected override INotifyValue<bool> AreConnectedInc(T source, T target)
        {
            throw new NotSupportedException();
        }
    }
}
