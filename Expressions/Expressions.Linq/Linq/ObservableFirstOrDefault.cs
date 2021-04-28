using System;
using System.Collections.Generic;
using SL = System.Linq.Enumerable;
using System.Text;
using System.Collections.Specialized;
using System.Linq.Expressions;

namespace NMF.Expressions.Linq
{
    internal class ObservableFirstOrDefault<TSource> : NotifyValue<TSource>, INotifyReversableValue<TSource>, IValueChangedNotificationResult<TSource>
    {
        public override string ToString()
        {
            return "[FirstOrDefault]";
        }


        private TSource value;
        private TSource oldValue;
        private readonly INotifyEnumerable<TSource> source;

        public static ObservableFirstOrDefault<TSource> Create(INotifyEnumerable<TSource> source)
        {
            var observable = new ObservableFirstOrDefault<TSource>(source);
            observable.Successors.SetDummy();
            return observable;
        }

        public static ObservableFirstOrDefault<TSource> CreateForPredicate(INotifyEnumerable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            return Create(source.Where(predicate));
        }

        public static ObservableFirstOrDefault<TSource> CreateExpression(IEnumerableExpression<TSource> source)
        {
            return Create(source.AsNotifiable());
        }

        public static ObservableFirstOrDefault<TSource> CreateExpressionForPredicate(IEnumerableExpression<TSource> source, Expression<Func<TSource, bool>> predicate)
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

            var perform = Expression.IfThen(Expression.Not(Expression.Call(collection, ReflectionHelper.GetMethod(collectionType, "Contains", new Type[] { typeof(TSource) }), variable)),
                Expression.Call(collection, ReflectionHelper.GetMethod(collectionType, "Add", new Type[] { typeof(TSource) }), variable));

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

            Expression perform = Expression.IfThen(Expression.Not(Expression.Call(collection, ReflectionHelper.GetMethod(collectionType, "Contains", new Type[] { typeof(TSource) }), variable)),
                Expression.Call(collection, ReflectionHelper.GetMethod(collectionType, "Add", new Type[] { typeof(TSource) }), variable));


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
            if (predicate is Expression<Func<TSource, bool>> predicateCasted)
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

        private static void RemoveWhere(ICollection<TSource> collection, Func<TSource, bool> predicate)
        {
            if (collection != null)
            {
                if (collection is IList<TSource> list)
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

        public ObservableFirstOrDefault(INotifyEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            this.source = source;
        }

        public override TSource Value
        {
            get { return value; }
        }

        protected override void Attach()
        {
            foreach (var dep in Dependencies)
                dep.Successors.Set(this);
            value = SL.FirstOrDefault(source);
        }

        protected override void Detach()
        {
            foreach (var dep in Dependencies)
                dep.Successors.Unset(this);
        }

        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            var newValue = SL.FirstOrDefault(source);
            if (!EqualityComparer<TSource>.Default.Equals(value, newValue))
            {
                oldValue = value;
                value = newValue;
                OnValueChanged(oldValue, newValue);
                return this;
            }

            return UnchangedNotificationResult.Instance;
        }

        TSource INotifyReversableValue<TSource>.Value
        {
            get
            {
                return Value;
            }
            set
            {
                if (!EqualityComparer<TSource>.Default.Equals(Value, value))
                {
                    if (source is not ICollection<TSource> coll || coll.IsReadOnly) throw new InvalidOperationException("The underlying source is not a writable collection!");
                    if (value != null)
                    {
                        if (!coll.Contains(value))
                        {
                            coll.Add(value);
                            if (EqualityComparer<TSource>.Default.Equals(this.value, value)) return;
                        }
                        oldValue = this.value;
                        this.value = value;
                        OnValueChanged(oldValue, Value);
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
                return source is ICollection<TSource> coll && !coll.IsReadOnly;
            }
        }

        public override IEnumerable<INotifiable> Dependencies { get { yield return source; } }

        #region value change notification
        TSource IValueChangedNotificationResult<TSource>.OldValue { get { return oldValue; } }

        TSource IValueChangedNotificationResult<TSource>.NewValue { get { return value; } }

        object IValueChangedNotificationResult.OldValue { get { return oldValue; } }

        object IValueChangedNotificationResult.NewValue { get { return value; } }

        INotifiable INotificationResult.Source { get { return this; } }

        bool INotificationResult.Changed { get { return true; } }

        void INotificationResult.IncreaseReferences(int references) { }

        void INotificationResult.FreeReference() { }
        #endregion
    }
}
