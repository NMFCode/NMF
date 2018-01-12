using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions.Linq
{
    internal sealed class ObservableTopX<TItem> : NotifyValue<TItem[]>
    {
        public static INotifyValue<TItem[]> CreateSelector<TKey>(INotifyEnumerable<TItem> source, int x, Expression<Func<TItem, TKey>> selector)
        {
            return CreateSelectorComparer(source, x, selector, null);
        }

        public static INotifyValue<TItem[]> CreateExpressionSelector<TKey>(IEnumerableExpression<TItem> source, int x, Expression<Func<TItem, TKey>> selector)
        {
            return CreateSelectorComparer(source.AsNotifiable(), x, selector, null);
        }

        public static INotifyValue<TItem[]> CreateSelectorComparer<TKey>(INotifyEnumerable<TItem> source, int x, Expression<Func<TItem, TKey>> selector, IComparer<TKey> comparer)
        {
            var topx = new ObservableTopX<TItem>(source.OrderByDescending(selector, comparer), x);
            topx.Successors.SetDummy();
            return topx;
        }

        public static INotifyValue<TItem[]> CreateExpressionSelectorComparer<TKey>(IEnumerableExpression<TItem> source, int x, Expression<Func<TItem, TKey>> selector, IComparer<TKey> comparer)
        {
            return CreateSelectorComparer(source.AsNotifiable(), x, selector, comparer);
        }

        public int X { get; set; }
        private TItem[] value;
        private INotifyEnumerable<TItem> source;

        public ObservableTopX(INotifyEnumerable<TItem> source, int x)
        {
            this.source = source;
            X = x;
            value = new TItem[0];

            Successors.Attached += (obj, e) => Attach();
            Successors.Detached += (obj, e) => Detach();
        }

        private void Detach()
        {
            source.Successors.Unset(this);
        }

        private void Attach()
        {
            source.Successors.Set(this);
            if (source.Count() < X)
            {
                value = System.Linq.Enumerable.ToArray(source);
            }
            else
            {
                value = new TItem[X];
                using (var en = source.GetEnumerator())
                {
                    for (int i = 0; i < X; i++)
                    {
                        en.MoveNext();
                        value[i] = en.Current;
                    }
                }
            }
        }

        public override TItem[] Value
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
            TItem[] newValue = null;
            var comparer = EqualityComparer<TItem>.Default;
            using (var en = source.GetEnumerator())
            {
                for (int i = 0; i < X; i++)
                {
                    if (en.MoveNext())
                    {
                        if (!comparer.Equals(value[i], en.Current))
                        {
                            if (newValue == null)
                            {
                                newValue = new TItem[X];
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
                        else
                        {
                            newValue = new TItem[i];
                            Array.Copy(oldValue, 0, newValue, 0, i);
                            break;
                        }
                    }
                }
            }
            if (newValue != null)
            {
                var e = new ValueChangedNotificationResult<TItem[]>(this, value, newValue);
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
