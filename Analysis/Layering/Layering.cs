using System;
using System.Collections.Generic;
using System.Linq;

namespace NMF.Analyses
{
    /// <summary>
    /// Tarjans algorithm
    /// </summary>
    public class Layering<T>
    {
        private readonly Func<T, IEnumerable<T>> edgeSelector;
        private readonly Dictionary<T, int> lowlink = new Dictionary<T, int>();
        private readonly Dictionary<T, int> indices = new Dictionary<T, int>();
        private readonly Stack<T> stack = new Stack<T>();
        private int index;

        private readonly List<ICollection<T>> layers = new List<ICollection<T>>();

        private Layering(Func<T, IEnumerable<T>> edgeSelector)
        {
            this.edgeSelector = edgeSelector;
        }

        private void Tarjan(T node)
        {
            indices.Add(node, index);
            lowlink.Add(node, index);
            index++;
            stack.Push(node);

            var connected = edgeSelector(node);
            if (connected != null)
            {
                foreach (var n2 in connected)
                {
                    if (!indices.ContainsKey(n2))
                    {
                        Tarjan(n2);
                        lowlink[node] = Math.Min(lowlink[node], indices[n2]);
                    }
                    else if (stack.Contains(n2))
                    {
                        lowlink[node] = Math.Min(lowlink[node], indices[n2]);
                    }
                }
            }

            if (lowlink[node] == indices[node])
            {
                var layer = new List<T>();
                T w = default(T);
                do
                {
                    w = stack.Pop();
                    layer.Add(w);
                } while (!EqualityComparer<T>.Default.Equals(node, w));
                layers.Add(layer);
            }
        }

        /// <summary>
        /// Creates a layering of the given elements
        /// </summary>
        /// <param name="nodes">The collection of nodes that make up the graph</param>
        /// <param name="edges">A function that selects for each node the connected nodes</param>
        /// <returns>A list of strongly connected components in topological order. This means, components without edges to other components come first.</returns>
        public static IList<ICollection<T>> CreateLayers(IEnumerable<T> nodes, Func<T, IEnumerable<T>> edges)
        {
            if (edges == null) throw new ArgumentNullException("edges");

            var layering = new Layering<T>(edges);
            foreach (var node in nodes)
            {
                if (!layering.indices.ContainsKey(node))
                {
                    layering.Tarjan(node);
                }
            }

            return layering.layers;
        }

        /// <summary>
        /// Creates a layering of the given elements
        /// </summary>
        /// <param name="root">The root element of the graph</param>
        /// <param name="edges">A function that selects for each node the connected nodes</param>
        /// <returns>A list of strongly connected components in topological order. This means, components without edges to other components come first.</returns>
        public static IList<ICollection<T>> CreateLayers(T root, Func<T, IEnumerable<T>> edges)
        {
            return CreateLayers(Enumerable.Repeat(root, 1), edges);
        }
    }
}
