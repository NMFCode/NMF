using System;
using System.Collections.Generic;
using System.Linq;

namespace NMF.Expressions.Linq
{
    internal class ObservableIndexedChunkCollection<T, TChunk> : ObservableChunkCollectionBase<T, TChunk>, IObservableChunk<(T, int), TChunk>
    {
        private readonly Func<IEnumerableExpression<(T, int)>, int, TChunk> _resultSelector;
        private readonly List<NotifyCollection<(T, int)>> _chunkItems = new List<NotifyCollection<(T, int)>>();
        private readonly IChunkBalancingStrategy<(T, int), TChunk> _balancingStrategy;

        public ObservableIndexedChunkCollection( INotifyEnumerable<T> source, int chunkSize, Func<IEnumerableExpression<(T, int)>, int, TChunk> resultSelector, IChunkBalancingStrategyProvider<(T, int), TChunk> balancingStrategy )
            : base(source, chunkSize)
        {
            _resultSelector = resultSelector;
            _balancingStrategy = balancingStrategy?.CreateStrategy(this) ?? NoBalancingStrategy<(T, int), TChunk>.Instance.CreateStrategy( this );

            Reset();
            source.Successors.Set( this );
        }

        protected override void Reset()
        {
            _chunkItems.Clear();
            _chunks.Clear();
            var index = 0;
            var chunkIndex = -1;
            NotifyCollection<(T, int)> currentChunk = null;
            foreach(var item in _source)
            {
                if(index % _chunkSize == 0)
                {
                    if(currentChunk != null)
                    {
                        var chunk = _resultSelector( currentChunk, chunkIndex );
                        _chunkItems.Add( currentChunk );
                        _chunks.Add( chunk );
                        _elements.Add( index );
                    }
                    currentChunk = new NotifyCollection<(T, int)>();
                    chunkIndex++;
                }
#pragma warning disable S2259 // Null pointers should not be dereferenced
                currentChunk.Add( (item, index) );
#pragma warning restore S2259 // Null pointers should not be dereferenced
                index++;
            }
            if (currentChunk != null)
            {
                var chunk = _resultSelector( currentChunk, chunkIndex );
                _chunkItems.Add( currentChunk );
                _chunks.Add( chunk );
                _elements.Add( index );
            }
            OnCleared();
        }

        protected override void Balance( ref List<TChunk> addedChunks, ref List<TChunk> removedChunks )
        {
            _balancingStrategy.Balance( ref addedChunks, ref removedChunks );
        }

        protected override bool RemoveChunkItem( int chunkIndex, int chunkPosition )
        {
            var items = _chunkItems[chunkIndex];
            items.RemoveAt( chunkPosition );
            if(items.Count == 0)
            {
                _chunkItems.RemoveAt( chunkIndex );
                return true;
            }
            return false;
        }

        protected override bool IsItemAtPosition( T item, int chunkIndex, int chunkPosition )
        {
            return EqualityComparer<T>.Default.Equals( _chunkItems[chunkIndex][chunkPosition].Item1, item );
        }

        public int GetChunkSize( int chunkIndex )
        {
            return _chunkItems[chunkIndex].Count;
        }

        public void AddToChunk( int chunkIndex, (T, int) item )
        {
            _chunkItems[chunkIndex].Add( item );
        }

        public void RemoveFromChunk( int chunkIndex, int chunkPosition )
        {
            _chunkItems[chunkIndex].RemoveAt( chunkPosition );
        }

        public (T, int) GetChunkItemAt( int chunkIndex, int chunkPosition )
        {
            return _chunkItems[chunkIndex][chunkPosition];
        }

        protected override bool TryAddToExistingChunk( T item, int startingIndex )
        {
            if(_chunkItems.Count > 0 && _chunkItems[_chunkItems.Count - 1].Count < _chunkSize)
            {
                var count = _elements.LastOrDefault();
                _chunkItems[_chunkItems.Count - 1].Add( (item, count) );
                _elements[_chunkItems.Count - 1] = count + 1;
                return true;
            }
            return false;
        }

        public override TChunk CreateChunkForItem( T item )
        {
            return CreateChunkForItem( (item, _elements.LastOrDefault()) );
        }

        public TChunk CreateChunkForItem((T, int) item)
        {
            var count = _elements.LastOrDefault();
            var collection = new NotifyCollection<(T, int)>
            {
                item
            };
            var chunk = _resultSelector(collection, _chunks.Count);
            _chunkItems.Add(collection);
            _elements.Add(count + 1);
            return chunk;
        }

        public override string ToString()
        {
            return $"[Indexed Chunk {ChunkSize}]";
        }
    }

    internal class ObservableChunkCollection<T, TChunk> : ObservableChunkCollectionBase<T, TChunk>, IObservableChunk<T, TChunk>
    {
        private readonly Func<IEnumerableExpression<T>, int, TChunk> _resultSelector;
        private readonly List<NotifyCollection<T>> _chunkItems = new List<NotifyCollection<T>>();
        private readonly IChunkBalancingStrategy<T, TChunk> _balancingStrategy;

        public ObservableChunkCollection( INotifyEnumerable<T> source, int chunkSize, Func<IEnumerableExpression<T>, int, TChunk> resultSelector, IChunkBalancingStrategyProvider<T, TChunk> balancingStrategy ) : base( source, chunkSize )
        {
            _resultSelector = resultSelector;
            _balancingStrategy = balancingStrategy?.CreateStrategy(this) ?? NoBalancingStrategy<T, TChunk>.Instance.CreateStrategy( this );

            Reset();
            source.Successors.Set( this );
        }

        protected override void Reset()
        {
            _chunkItems.Clear();
            _chunks.Clear();
            _elements.Clear();
            var index = 0;
            var chunkIndex = -1;
            NotifyCollection<T> currentChunk = null;
            foreach(var item in _source)
            {
                if(index % _chunkSize == 0)
                {
                    if(currentChunk != null)
                    {
                        var chunk = _resultSelector( currentChunk, chunkIndex );
                        _chunkItems.Add( currentChunk );
                        _chunks.Add( chunk );
                        _elements.Add( index );
                    }
                    currentChunk = new NotifyCollection<T>();
                    chunkIndex++;
                }
#pragma warning disable S2259 // Null pointers should not be dereferenced
                currentChunk.Add( item );
#pragma warning restore S2259 // Null pointers should not be dereferenced
                index++;
            }
            if(currentChunk != null)
            {
                var chunk = _resultSelector( currentChunk, chunkIndex );
                _chunkItems.Add( currentChunk );
                _chunks.Add( chunk );
                _elements.Add( index );
            }
            OnCleared();
        }
        protected override bool IsItemAtPosition( T item, int chunkIndex, int chunkPosition )
        {
            return EqualityComparer<T>.Default.Equals( _chunkItems[chunkIndex][chunkPosition], item );
        }

        protected override bool RemoveChunkItem( int chunkIndex, int chunkPosition )
        {
            var items = _chunkItems[chunkIndex];
            items.RemoveAt( chunkPosition );
            if (items.Count == 0)
            {
                _chunkItems.RemoveAt( chunkIndex );
                return true;
            }
            return false;
        }

        protected override void Balance( ref List<TChunk> addedChunks, ref List<TChunk> removedChunks )
        {
            _balancingStrategy.Balance( ref addedChunks, ref removedChunks );
        }

        public int GetChunkSize( int chunkIndex )
        {
            return _chunkItems[chunkIndex].Count;
        }

        public void AddToChunk( int chunkIndex, T item )
        {
            _chunkItems[chunkIndex].Add( item );
        }

        public void RemoveFromChunk( int chunkIndex, int chunkPosition )
        {
            _chunkItems[chunkIndex].RemoveAt( chunkPosition );
        }

        public T GetChunkItemAt( int chunkIndex, int chunkPosition )
        {
            return _chunkItems[chunkIndex][chunkPosition];
        }

        public override TChunk CreateChunkForItem( T item )
        {
            var collection = new NotifyCollection<T>
            {
                item
            };
            var chunk = _resultSelector( collection, _chunks.Count );
            _chunkItems.Add( collection );
            _elements.Add( _elements.LastOrDefault() + 1 );
            return chunk;
        }

        protected override bool TryAddToExistingChunk( T item, int startingIndex )
        {
            if(_chunkItems.Count > 0 && _chunkItems[_chunkItems.Count - 1].Count < _chunkSize)
            {
                _chunkItems[_chunkItems.Count - 1].Add( item );
                _elements[_elements.Count - 1]++;
                return true;
            }
            return _balancingStrategy.TryAddToExistingChunk(item, startingIndex);
        }

        public override string ToString()
        {
            return $"[Chunk {ChunkSize}]";
        }
    }

    internal abstract class ObservableChunkCollectionBase<T, TChunk> : ObservableEnumerable<TChunk>
    {
        protected readonly INotifyEnumerable<T> _source;
        protected readonly int _chunkSize;
        protected readonly List<TChunk> _chunks = new List<TChunk>();
        protected readonly List<int> _elements = new List<int>();

        protected ObservableChunkCollectionBase( INotifyEnumerable<T> source, int chunkSize )
        {
            _source = source;
            _chunkSize = chunkSize;
        }

        protected abstract void Reset();

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return _source;
            }
        }

        public override int Count => _chunks.Count;

        public int ChunkCount => Count;

        public int ChunkSize => _chunkSize;

        public int ElementCount => _elements.LastOrDefault();

        public override IEnumerator<TChunk> GetEnumerator()
        {
            return _chunks.GetEnumerator();
        }

        public override INotificationResult Notify( IList<INotificationResult> sources )
        {
            var isReset = false;
            var balanceNeeded = false;
            List<TChunk> added = null;
            List<TChunk> removed = null;
            foreach(var change in sources.OfType<ICollectionChangedNotificationResult<T>>())
            {
                if(change.IsReset)
                {
                    isReset = true;
                    break;
                }
                else
                {
                    foreach(var item in change.RemovedItems)
                    {
                        NotifyRemovedItem(item, change.OldItemsStartIndex, ref removed, ref balanceNeeded);
                    }

                    foreach (var item in change.AddedItems)
                    {
                        NotifyAddedItem(item, change.NewItemsStartIndex, ref added);
                    }
                }
            }
            if(balanceNeeded)
            {
                Balance(ref added, ref removed);
            }
            if(isReset || added != null || removed != null)
            {
                return NotifyCore(isReset, added, removed);
            }
            return UnchangedNotificationResult.Instance;
        }

        private INotificationResult NotifyCore(bool isReset, List<TChunk> added, List<TChunk> removed)
        {
            var notification = CollectionChangedNotificationResult<TChunk>.Create(this, isReset);
            if (isReset)
            {
                Reset();
            }
            else
            {
                if (added != null)
                {
                    notification.AddedItems.AddRange(added);
                    OnAddItems(added);
                }
                if (removed != null)
                {
                    notification.RemovedItems.AddRange(removed);
                    OnRemoveItems(removed);
                }
            }
            return notification;
        }

        private void NotifyAddedItem(T item, int newItemsStartIndex, ref List<TChunk> added)
        {
            if (!TryAddToExistingChunk(item, newItemsStartIndex))
            {
                var chunk = CreateChunkForItem(item);
                if (added == null)
                {
                    added = new List<TChunk>();
                }
                _chunks.Add(chunk);
                added.Add(chunk);
            }
        }

        private void NotifyRemovedItem(T item, int oldItemsStartIndex, ref List<TChunk> removed, ref bool balanceNeeded)
        {
            var (chunkIndex, chunkPosition) = FindChunkItem(item, oldItemsStartIndex);
            if (chunkIndex != -1)
            {
                balanceNeeded = true;
                for (int i = chunkIndex; i < _elements.Count; i++)
                {
                    _elements[i]--;
                }
                if (RemoveChunkItem(chunkIndex, chunkPosition))
                {
                    if (removed == null)
                    {
                        removed = new List<TChunk>();
                    }
                    removed.Add(_chunks[chunkIndex]);
                    _chunks.RemoveAt(chunkIndex);
                    _elements.RemoveAt(chunkIndex);
                }
            }
        }

        protected virtual void Balance(ref List<TChunk> addedChunks, ref List<TChunk> removedChunks)
        {
        }

        protected abstract bool TryAddToExistingChunk( T item, int startingIndex );

        public abstract TChunk CreateChunkForItem( T item );

        protected abstract bool RemoveChunkItem( int chunkIndex, int chunkPosition );

        public void RemoveChunk(int chunkIndex, ref List<TChunk> removeList)
        {
            var chunk = _chunks[chunkIndex];
            _chunks.RemoveAt( chunkIndex );
            if (removeList == null)
            {
                removeList = new List<TChunk>();
            }
            removeList.Add( chunk );
        }

        private (int, int) FindChunkItem(T item, int startIndex)
        {
            if (_elements.Count == 0)
            {
                return (-1, -1);
            }

            int chunkIndex;
            int chunkPosition;

            var likelyElement = startIndex >= 0 ? _elements.BinarySearch( startIndex ) : -1;
            if (likelyElement >= 0)
            {
                chunkIndex = likelyElement + 1;
                chunkPosition = 0;
            }
            else
            {
                chunkIndex = ~likelyElement;
                chunkPosition = startIndex - (chunkIndex > 0 ? _elements[chunkIndex - 1] : 0);
            }

            while(chunkIndex < _chunks.Count)
            {
                while(chunkPosition < _chunkSize)
                {
                    if(IsItemAtPosition( item, chunkIndex, chunkPosition ))
                    {
                        return (chunkIndex, chunkPosition);
                    }
                }
                chunkIndex++;
            }
            return (-1, -1);
        }

        protected abstract bool IsItemAtPosition( T item, int chunkIndex, int chunkPosition );

        public int GetAccumulatedElementCount( int chunkIndex )
        {
            return _elements[chunkIndex];
        }

        public IReadOnlyList<TChunk> Chunks => _chunks.AsReadOnly();
    }
}
