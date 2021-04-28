using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions.Linq
{
    internal class ObservableTopX<TItem, TKey> : NotifyValue<KeyValuePair<TItem, TKey>[]>
    {
        public override string ToString()
        {
            return $"[Top {X}]";
        }

        public static INotifyValue<KeyValuePair<TItem, TKey>[]> CreateSelector(INotifyEnumerable<TItem> source, int x, Expression<Func<TItem, TKey>> selector)
        {
            return CreateSelectorComparer(source, x, selector, null);
        }

        public static INotifyValue<KeyValuePair<TItem, TKey>[]> CreateExpressionSelector(IEnumerableExpression<TItem> source, int x, Expression<Func<TItem, TKey>> selector)
        {
            return CreateSelectorComparer(source.AsNotifiable(), x, selector, null);
        }

        public static INotifyValue<KeyValuePair<TItem, TKey>[]> CreateSelectorComparer(INotifyEnumerable<TItem> source, int x, Expression<Func<TItem, TKey>> selector, IComparer<TKey> comparer)
        {
            var ordered = new ObservableOrderBy<TItem, TKey>(source, selector, new ReverseComparer<TKey>(comparer));
            var topx = new ObservableTopX<TItem, TKey>(ordered, x);
            topx.Successors.SetDummy();
            return topx;
        }

        public static INotifyValue<KeyValuePair<TItem, TKey>[]> CreateExpressionSelectorComparer(IEnumerableExpression<TItem> source, int x, Expression<Func<TItem, TKey>> selector, IComparer<TKey> comparer)
        {
            return CreateSelectorComparer(source.AsNotifiable(), x, selector, comparer);
        }

        public int X { get; set; }
        private KeyValuePair<TItem, TKey>[] value;
        private readonly ObservableOrderBy<TItem, TKey> source;

        private ObservableTopX(ObservableOrderBy<TItem, TKey> source, int x)
        {
            this.source = source;
            X = x;
            value = new KeyValuePair<TItem, TKey>[0];
        }

        protected override void Detach()
        {
            source.Successors.Unset(this);
        }

        protected override void Attach()
        {
            source.Successors.Set(this);
            if (source.Count() < X)
            {
                value = System.Linq.Enumerable.ToArray(source.KeyedItems);
            }
            else
            {
                value = new KeyValuePair<TItem, TKey>[X];
                using (var en = source.KeyedItems.GetEnumerator())
                {
                    for (int i = 0; i < X; i++)
                    {
                        en.MoveNext();
                        value[i] = en.Current;
                    }
                }
            }
        }

        public override KeyValuePair<TItem, TKey>[] Value
        {
            get
            {
                return value;
            }
        }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return source;
            }
        }

        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            var oldValue = value;
            KeyValuePair<TItem, TKey>[] newValue = null;
            var comparer = EqualityComparer<TItem>.Default;
            using (var en = source.KeyedItems.GetEnumerator())
            {
                for (int i = 0; i < X; i++)
                {
                    if (en.MoveNext())
                    {
                        if (value.Length <= i || !comparer.Equals(value[i].Key, en.Current.Key))
                        {
                            if (newValue == null)
                            {
                                newValue = new KeyValuePair<TItem, TKey>[X];
                                Array.Copy(value, 0, newValue, 0, i);
                            }
                            newValue[i] = en.Current;
                        }
                        else if (newValue != null)
                        {
                            newValue[i] = en.Current;
                        }
                    }
                    else
                    {
                        if (value.Length == i)
                        {
                            return UnchangedNotificationResult.Instance;
                        }
                        else if (newValue == null)
                        {
                            newValue = new KeyValuePair<TItem, TKey>[i];
                            Array.Copy(value, 0, newValue, 0, i);
                            break;
                        }
                        else
                        {
                            Array.Resize(ref newValue, i);
                            break;
                        }
                    }
                }
            }
            if (newValue != null)
            {
                var e = new ValueChangedNotificationResult<KeyValuePair<TItem, TKey>[]>(this, value, newValue);
                value = newValue;
                OnValueChanged(oldValue, newValue);
                return e;
            }
            else
            {
                return UnchangedNotificationResult.Instance;
            }
        }
    }
}
