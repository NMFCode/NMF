using System;
using System.Collections.Generic;
using SL = System.Linq.Enumerable;
using System.Text;
using System.Collections.Specialized;
using System.Linq.Expressions;

namespace NMF.Expressions.Linq
{
    internal class ObservableSingleOrDefault<TSource> : INotifyValue<TSource>, INotifyReversableValue<TSource>
    {
        private readonly SuccessorList successors = new SuccessorList();
        private TSource value;
        private INotifyEnumerable<TSource> source;

        public static ObservableSingleOrDefault<TSource> Create(INotifyEnumerable<TSource> source)
        {
            var observable = new ObservableSingleOrDefault<TSource>(source);
            observable.Successors.Add(null);
            source.Successors.Remove(null);
            return observable;
        }

        public static ObservableSingleOrDefault<TSource> CreateForPredicate(INotifyEnumerable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            return Create(source.Where(predicate));
        }

        public static ObservableSingleOrDefault<TSource> CreateExpression(IEnumerableExpression<TSource> source)
        {
            return Create(source.AsNotifiable());
        }

        public static ObservableSingleOrDefault<TSource> CreateExpressionForPredicate(IEnumerableExpression<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            return Create(source.AsNotifiable().Where(predicate));
        }
        
        public static Expression CreateSetExpression(MethodCallExpression node, SetExpressionRewriter rewriter)
        {
            if (node == null || rewriter == null) return null;
            var collectionType = typeof(ICollection<TSource>);
            if (!ReflectionHelper.IsAssignableFrom(collectionType, node.Arguments[0].Type)) return null;

            var variable = Expression.Variable(typeof(TSource));
            var collection = Expression.Variable(typeof(ICollection<TSource>));

            Expression perform = Expression.Call(null, ReflectionHelper.GetAction<ICollection<TSource>, TSource>((source, exc) => RemoveAllExcept(source, exc)), collection, variable);

            var trueBlock = Expression.Block(
                new ParameterExpression[] {
                    variable
                },
                Expression.Assign(variable, rewriter.Value),
                perform);

            return
                Expression.Block(
                    new ParameterExpression[] { collection },
                    Expression.Assign(collection, Expression.Convert(node.Arguments[0], collectionType)),
                    Expression.IfThenElse(
                        Expression.NotEqual(rewriter.Value, Expression.Constant(null, typeof(object))),
                        trueBlock,
                        Expression.Call(collection, ReflectionHelper.GetMethod(collectionType, "Clear", new Type[] {})))
                );
        }

        public static Expression CreateSetExpressionWithPredicate(MethodCallExpression node, SetExpressionRewriter rewriter)
        {
            if (node == null || rewriter == null) return null;
            if (node.Arguments[1] == null ||
                (node.Arguments[1].NodeType != ExpressionType.Constant
                && node.Arguments[1].NodeType != ExpressionType.Quote
                && node.Arguments[1].NodeType != ExpressionType.Lambda)) return null;
            var collectionType = typeof(ICollection<TSource>);
            if (!ReflectionHelper.IsAssignableFrom(collectionType, node.Arguments[0].Type)) return null;

            var variable = Expression.Variable(typeof(TSource));
            var collection = Expression.Variable(typeof(ICollection<TSource>));

            Expression perform = Expression.Call(null, ReflectionHelper.GetAction<ICollection<TSource>, TSource>((source, exc) => RemoveAllExcept(source, exc)), collection, variable);

            var predicate = node.Arguments[1];
            while (predicate.NodeType == ExpressionType.Quote)
            {
                predicate = (predicate as UnaryExpression).Operand;
            }
            if (predicate.NodeType == ExpressionType.Constant)
            {
                predicate = (predicate as ConstantExpression).Value as Expression;
            }
            var predicateRewriter = new SetExpressionRewriter(Expression.Constant(true));
            var predicateCasted = predicate as Expression<Func<TSource, bool>>;
            if (predicateCasted != null)
            {
                var fulfillPredicate = predicateRewriter.Visit(predicateCasted.Body);

                if (fulfillPredicate != null)
                {
                    perform = Expression.Block(perform, fulfillPredicate);
                }
            }

            var trueBlock = Expression.Block(
                new ParameterExpression[]
                {
                    variable
                },
                Expression.Assign(variable, rewriter.Value),
                perform);

            var removeMethod = ReflectionHelper.GetAction<ICollection<TSource>, Func<TSource, bool>>((source, pred) => RemoveWhere(source, pred));

            return
                Expression.Block(
                    new ParameterExpression[] { collection },
                    Expression.Assign(collection, Expression.Convert(node.Arguments[0], collectionType)),
                    Expression.IfThenElse(
                        Expression.NotEqual(rewriter.Value, Expression.Constant(null, typeof(object))),
                        trueBlock,
                        Expression.Call(removeMethod, collection,
                            Expression.Constant((predicate as Expression<Func<TSource, bool>>).Compile())))
                );
        }

        public static void RemoveAllExcept(ICollection<TSource> collection, TSource exception)
        {
            if (!collection.Contains(exception))
            {
                collection.Clear();
                collection.Add(exception);
            }
            else if (collection.Count > 1)
            {
                RemoveWhere(collection, item => !EqualityComparer<TSource>.Default.Equals(item, exception));
            }
        }

        public static void RemoveWhere(ICollection<TSource> collection, Func<TSource, bool> predicate)
        {
            if (collection != null)
            {
                var list = collection as IList<TSource>;
                if (list != null)
                {
                    for (int i = list.Count - 1; i >= 0; i--)
                    {
                        if (predicate(list[i]))
                        {
                            list.RemoveAt(i);
                        }
                    }
                }
                else
                {
                    var toRemove = SL.ToList(SL.Where(collection, predicate));
                    foreach (var item in toRemove)
                    {
                        collection.Remove(item);
                    }
                }
            }
        }

        public ObservableSingleOrDefault(INotifyEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            this.source = source;

            successors.CollectionChanged += (obj, e) =>
            {
                if (successors.Count == 0)
                    Detach();
                else if (e.Action == NotifyCollectionChangedAction.Add && successors.Count == 1)
                    Attach();
            };
        }

        public TSource Value
        {
            get { return value; }
        }

        protected void OnValueChanged(ValueChangedEventArgs e)
        {
            if (ValueChanged != null)
            {
                ValueChanged(this, e);
            }
        }

        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        public void Attach()
        {
            foreach (var dep in Dependencies)
                dep.Successors.Add(this);
            value = SL.SingleOrDefault(source);
        }

        public void Detach()
        {
            foreach (var dep in Dependencies)
                dep.Successors.Remove(this);
        }

        public INotificationResult Notify(IList<INotificationResult> sources)
        {
            var change = (CollectionChangedNotificationResult<TSource>)sources[0];

            if (change.IsReset || change.RemovedItems != null || change.AddedItems != null)
            {
                var newValue = SL.SingleOrDefault(source);

                if (!EqualityComparer<TSource>.Default.Equals(value, newValue))
                {
                    var oldValue = value;
                    value = newValue;
                    OnValueChanged(new ValueChangedEventArgs(oldValue, newValue));
                    return new ValueChangedNotificationResult<TSource>(this, oldValue, newValue);
                }
            }

            return new UnchangedNotificationResult(this);
        }

        public void Dispose()
        {
            Detach();
        }

        TSource INotifyReversableValue<TSource>.Value
        {
            get
            {
                return this.value;
            }
            set
            {
                if (!EqualityComparer<TSource>.Default.Equals(Value, value))
                {
                    var coll = source as ICollection<TSource>;
                    if (coll == null || coll.IsReadOnly) throw new InvalidOperationException("The underlying source is not a writable collection!");
                    if (value != null)
                    {
                        if (!coll.Contains(value))
                        {
                            coll.Clear();
                            coll.Add(value);
                        }
                        else
                        {
                            if (coll.Count > 1)
                            {
                                RemoveWhere(coll, item => !EqualityComparer<TSource>.Default.Equals(item, value));
                            }
                        }
                        var old = this.value;
                        this.value = value;
                        OnValueChanged(new ValueChangedEventArgs(old, value));
                    }
                    else
                    {
                        coll.Clear();
                    }
                }
            }
        }

        public bool IsReversable
        {
            get
            {
                var coll = source as ICollection<TSource>;
                return coll != null && !coll.IsReadOnly;
            }
        }

        public IList<INotifiable> Successors { get { return successors; } }

        public IEnumerable<INotifiable> Dependencies { get { yield return source; } }

        public ExecutionMetaData ExecutionMetaData { get; } = new ExecutionMetaData();
    }
}
