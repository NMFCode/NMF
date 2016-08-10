using NMF.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Analyses
{
    public abstract class Connectivity<T>
    {
        private class ConnectivityProxy
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

        [ObservableProxy(typeof(Connectivity<>.ConnectivityProxy), "AreConnectedProxy")]
        public abstract bool AreConnected(T a, T b);

        protected abstract INotifyValue<bool> AreConnectedInc(T a, T b);

        [ObservableProxy(typeof(Connectivity<>.ConnectivityProxy), "CreateHolmIncremental")]
        public static Connectivity<T> CreateHolm(Expression<Func<T, IEnumerableExpression<T>>> edges, IEnumerableExpression<T> elements)
        {
            return new HolmConnectivity<T>(edges.Compile(), false, elements);
        }

        [ObservableProxy(typeof(Connectivity<>.ConnectivityProxy), "CreateHolmIncremental")]
        public static Connectivity<T> Create(Expression<Func<T, IEnumerableExpression<T>>> edges, IEnumerableExpression<T> elements)
        {
            return new UnionFind<T>(edges.Compile(), elements);
        }
    }
}
