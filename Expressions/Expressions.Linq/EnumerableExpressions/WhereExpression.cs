using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using SL = System.Linq.Enumerable;
using NMF.Expressions.Linq;
using System.Diagnostics;

namespace NMF.Expressions
{
    internal class WhereExpression<T> : IEnumerableExpression<T>
    {
        public IEnumerableExpression<T> Source { get; private set; }
        public Expression<Func<T, bool>> PredicateExpression { get; private set; }
        public Func<T, bool> PredicateCompiled { get; private set; }

        public WhereExpression(IEnumerableExpression<T> source, Expression<Func<T, bool>> predicate, Func<T, bool> predicateCompiled)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (predicate == null) throw new ArgumentNullException("selector");
            if (predicateCompiled == null) predicateCompiled = ExpressionCompileRewriter.Compile(predicate);

            Source = source;
            PredicateExpression = predicate;
            PredicateCompiled = predicateCompiled;
        }

        public INotifyEnumerable<T> AsNotifiable()
        {
            return Source.AsNotifiable().Where(PredicateExpression);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return SL.Where(Source, PredicateCompiled).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        INotifyEnumerable IEnumerableExpression.AsNotifiable()
        {
            return AsNotifiable();
        }
    }

    internal class WhereCollectionExpression<T> : WhereExpression<T>, ICollectionExpression<T>
    {
        public Action<T, bool> PredicateSetter { get; private set; }

        public new ICollectionExpression<T> Source
        {
            get
            {
                return (ICollectionExpression<T>)base.Source;
            }
        }

        public WhereCollectionExpression(ICollectionExpression<T> source, Expression<Func<T, bool>> predicate, Func<T, bool> predicateCompiled, Action<T, bool> predicateReversed)
            : base(source, predicate, predicateCompiled)
        {
            if (predicateCompiled == null)
            {
                var predicateReverseExpression = SetExpressionRewriter.CreateSetter(predicate);
                if (predicateReverseExpression != null)
                {
                    predicateReversed = predicateReverseExpression.Compile();
                }
            }

            PredicateSetter = predicateReversed;
        }

        public new INotifyCollection<T> AsNotifiable()
        {
            return Source.AsNotifiable().Where(PredicateExpression);
        }

        public void Add(T item)
        {
            var source = Source;
            if (!source.Contains(item)) source.Add(item);
            if (!PredicateCompiled(item))
            {
                if (PredicateSetter != null)
                {
                    PredicateSetter(item, true);
                }
                else
                {
                    Debug.WriteLine("Cannot set predicate of item {0}", item);
                }
            }
        }

        public void Clear()
        {
            var list = base.Source as IList<T>;
            if (list != null)
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    if (PredicateCompiled(list[i])) list.RemoveAt(i);
                }
            }
            else
            {
                list = SL.ToList(SL.Where(base.Source, PredicateCompiled));
                var source = Source;
                if (list.Count != source.Count)
                {
                    foreach (var item in list)
                    {
                        source.Remove(item);
                    }
                }
                else
                {
                    source.Clear();
                }
            }
        }

        public bool Contains(T item)
        {
            return Source.Contains(item) && PredicateCompiled(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach (var item in base.Source)
            {
                if (PredicateCompiled(item))
                {
                    array[arrayIndex] = item;
                    arrayIndex++;
                }
            }
        }

        public int Count
        {
            get { return SL.Count(base.Source, PredicateCompiled); }
        }

        public bool IsReadOnly
        {
            get { return Source.IsReadOnly && PredicateSetter == null; }
        }

        public bool Remove(T item)
        {
            var source = Source;
            if (!source.IsReadOnly)
            {
                return source.Remove(item);
            }
            if (PredicateCompiled(item) && PredicateSetter != null)
            {
                PredicateSetter(item, false);
                return true;
            }
            return false;
        }
    }

}
