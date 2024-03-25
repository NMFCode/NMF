using NMF.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Analyses
{
    /// <summary>
    /// Denotes a class that represents a connectivity algorithm
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Connectivity<T>
    {
        private static class ConnectivityProxy
        {
            public static INotifyValue<bool> AreConnectedProxy(Connectivity<T> conn, T a, T b)
            {
                if (conn == null) return null;
                return conn.AreConnectedInc(a, b);
            }

            public static INotifyValue<Connectivity<T>> CreateHolmIncremental(Expression<Func<T, IEnumerableExpression<T>>> edges, IEnumerableExpression<T> elements)
            {
                var connectivity = new HolmConnectivity<T>(edges.Compile(), true, elements);
                return new ConstantValue<Connectivity<T>>(connectivity);
            }
        }

        /// <summary>
        /// Determines whether the given nodes are connected
        /// </summary>
        /// <param name="source">The first node</param>
        /// <param name="target">The second node</param>
        /// <returns>True, if there is a path from a to be, otherwise false</returns>
        [ObservableProxy(typeof(Connectivity<>.ConnectivityProxy), "AreConnectedProxy")]
        public abstract bool AreConnected(T source, T target);

        /// <summary>
        /// Incrementally determines whether a and b are connected
        /// </summary>
        /// <param name="source">The first node</param>
        /// <param name="target">The second node</param>
        /// <returns>An incremental value that determines whether a and b are connected</returns>
        protected abstract INotifyValue<bool> AreConnectedInc(T source, T target);

        /// <summary>
        /// Create a new connectivity analysis using Holms algorithm
        /// </summary>
        /// <param name="edges">A function that returns connected nodes</param>
        /// <param name="elements">The nodes of the graph</param>
        /// <returns>A connectivity implementation</returns>
        [ObservableProxy(typeof(Connectivity<>.ConnectivityProxy), "CreateHolmIncremental")]
        public static Connectivity<T> CreateHolm(Expression<Func<T, IEnumerableExpression<T>>> edges, IEnumerableExpression<T> elements)
        {
            return new HolmConnectivity<T>(edges.Compile(), false, elements);
        }

        /// <summary>
        /// Create a new connectivity analysis using Union-find
        /// </summary>
        /// <param name="edges">A function that returns connected nodes</param>
        /// <param name="elements">The nodes of the graph</param>
        /// <returns>A connectivity implementation</returns>
        [ObservableProxy(typeof(Connectivity<>.ConnectivityProxy), "CreateHolmIncremental")]
        public static Connectivity<T> Create(Expression<Func<T, IEnumerableExpression<T>>> edges, IEnumerableExpression<T> elements)
        {
            return new UnionFind<T>(edges.Compile(), elements);
        }
    }
}
