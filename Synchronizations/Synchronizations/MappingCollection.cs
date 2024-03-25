using NMF.Expressions;
using NMF.Expressions.Linq;
using NMF.Transformations;
using NMF.Transformations.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Synchronizations
{
    internal class MappingCollection<TLeft, TRight> : IEnumerableExpression<TRight>
    {
        private readonly IEnumerableExpression<TLeft> _source;
        private readonly TransformationRuleBase<TLeft, TRight> _transformationRule;
        private readonly ITransformationContext _context;
        private Notifiable _notifiable;

        public MappingCollection(IEnumerableExpression<TLeft> sources, TransformationRuleBase<TLeft, TRight> transformationRule, ITransformationContext context )
        {
            _source = sources;
            _transformationRule = transformationRule;
            _context = context;
        }

        public INotifyEnumerable<TRight> AsNotifiable()
        {
            if (_notifiable == null)
            {
                _notifiable = new Notifiable( _source.AsNotifiable(), _context, _transformationRule );
                _notifiable.Successors.SetDummy();
            }
            return _notifiable;
        }

        public IEnumerator<TRight> GetEnumerator()
        {
            if (_notifiable != null)
            {
                return _notifiable.GetEnumerator();
            }
            return _source.AsEnumerable().Select( l =>
             {
                 var computation = _context.CallTransformation( _transformationRule, new object[] { l }, null );
                 return (TRight)computation.Output;
             } ).GetEnumerator();
        }

        INotifyEnumerable IEnumerableExpression.AsNotifiable()
        {
            return AsNotifiable();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class Notifiable : ObservableEnumerable<TRight>
        {
            private readonly INotifyEnumerable<TLeft> _sources;
            private readonly ITransformationContext _context;
            private readonly TransformationRuleBase<TLeft, TRight> _rule;
            private readonly List<SynchronizationComputation<TLeft, TRight>> _cache = new List<SynchronizationComputation<TLeft, TRight>>();

            public Notifiable(INotifyEnumerable<TLeft> sources, ITransformationContext context, TransformationRuleBase<TLeft, TRight> rule)
            {
                _sources = sources;
                _context = context;
                _rule = rule;
            }

            protected override void OnAttach()
            {
                _cache.Clear();
                _sources.Successors.Set( this );
                foreach(var item in _sources)
                {
                    var computation = _context.CallTransformation( _rule, new object[] { item }, null ) as SynchronizationComputation<TLeft, TRight>;
                    if (computation != null)
                    {
                        _cache.Add( computation );
                        computation.Successors.Set( this );
                    }
                }
            }

            protected override void OnDetach()
            {
                _sources.Successors.Unset( this );
                foreach(var computation in _cache)
                {
                    computation.Successors.Unset( this );
                }
            }

            public override IEnumerable<INotifiable> Dependencies
            {
                get
                {
                    yield return _sources;
                    foreach(var computation in _cache)
                    {
                        yield return computation;
                    }
                }
            }

            public override IEnumerator<TRight> GetEnumerator()
            {
                return _cache.Select( c => c.Opposite.Input ).GetEnumerator();
            }

            public override INotificationResult Notify( IList<INotificationResult> sources )
            {
                var result = CollectionChangedNotificationResult<TRight>.Create( this );
                var hasChanges = false;
                foreach(var change in sources)
                {
                    if(change.Source == _sources && change is ICollectionChangedNotificationResult<TLeft> collectionChange)
                    {
                        if(collectionChange.IsReset)
                        {
                            result.TurnIntoReset();
                            OnDetach();
                            OnAttach();
                            return result;
                        }

                        foreach(var oldLeftValue in collectionChange.RemovedItems)
                        {
                            ProcessRemoval(result, oldLeftValue);
                            hasChanges = true;
                        }
                        foreach (var newLeftValue in collectionChange.AddedItems)
                        {
                            ProcessAddition(result, newLeftValue);
                            hasChanges = true;
                        }
                    }
                    else if(change is IValueChangedNotificationResult<TRight> valueChange)
                    {
                        ProcessValueChange(result, valueChange);
                        hasChanges = true;
                    }
                }
                if (hasChanges)
                {
                    return result;
                }
                result.FreeReference();
                return UnchangedNotificationResult.Instance;
            }

            private static void ProcessValueChange(CollectionChangedNotificationResult<TRight> result, IValueChangedNotificationResult<TRight> valueChange)
            {
                if (!result.AddedItems.Remove(valueChange.OldValue))
                {
                    result.RemovedItems.Add(valueChange.OldValue);
                }
                if (!result.RemovedItems.Remove(valueChange.NewValue))
                {
                    result.AddedItems.Add(valueChange.NewValue);
                }
            }

            private void ProcessAddition(CollectionChangedNotificationResult<TRight> result, TLeft newLeftValue)
            {
                var computation = _context.CallTransformation(_rule, newLeftValue) as SynchronizationComputation<TLeft, TRight>;
                var newValue = computation.Opposite.Input;

                if (!result.RemovedItems.Remove(newValue))
                {
                    result.AddedItems.Add(newValue);
                }

                computation.Successors.Set(this);
                _cache.Add(computation);
            }

            private void ProcessRemoval(CollectionChangedNotificationResult<TRight> result, TLeft oldLeftValue)
            {
                var computation = _context.Trace.TraceIn(_rule, oldLeftValue).OfType<SynchronizationComputation<TLeft, TRight>>().FirstOrDefault();
                if (computation != null)
                {
                    var oldValue = computation.Opposite.Input;

                    if (!result.AddedItems.Remove(oldValue))
                    {
                        result.RemovedItems.Add(oldValue);
                    }

                    computation.Successors.Unset(this);
                    _cache.Remove(computation);
                }
            }
        }
    }
}
