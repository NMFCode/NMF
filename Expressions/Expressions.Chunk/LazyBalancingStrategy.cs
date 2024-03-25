using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Expressions.Linq
{
    /// <summary>
    /// Denotes the strategy to lazily balance a chunk collection
    /// </summary>
    /// <remarks>Lazy in this context means that elements are only reassigned to chunks when a chunk can be saved</remarks>
    public sealed class LazyBalancingStrategy : IChunkBalancingStrategyProvider
    {
        /// <summary>
        /// The static instance
        /// </summary>
        public static readonly LazyBalancingStrategy Instance = new LazyBalancingStrategy();

        /// <inheritdoc />
        public IChunkBalancingStrategy<T, TChunk> CreateStrategy<T, TChunk>( IObservableChunk<T, TChunk> observableChunk )
        {
            return LazyBalancingOnRemoveStrategy<T, TChunk>.Default.CreateStrategy(observableChunk);
        }
    }

    /// <summary>
    /// Denotes a base class for lazy balancing strategies for chunks
    /// </summary>
    /// <typeparam name="T">The type of elements</typeparam>
    /// <typeparam name="TChunk">The type of chunks</typeparam>
    public class LazyBalancingOnRemoveStrategy<T, TChunk> : IChunkBalancingStrategyProvider<T, TChunk>
    {
        /// <summary>
        /// Denotes the default lazy balancing strategy
        /// </summary>
        public static readonly LazyBalancingOnRemoveStrategy<T, TChunk> Default = new LazyBalancingOnRemoveStrategy<T, TChunk>();

        /// <inheritdoc />
        public IChunkBalancingStrategy<T, TChunk> CreateStrategy( IObservableChunk<T, TChunk> observableChunk )
        {
            return new Strategy( observableChunk, this );
        }

        /// <summary>
        /// Determines whether the given chunk can be adjusted
        /// </summary>
        /// <param name="chunk">The chunk to adjust</param>
        /// <returns>True, if the chunk can be modified, otherwise False</returns>
        protected virtual bool CanAdjust(TChunk chunk)
        {
            return true;
        }

        /// <summary>
        /// Determines whether elements can be moved between the given chunks
        /// </summary>
        /// <param name="sourceChunk">The source chunk</param>
        /// <param name="targetChunk">The target chunk</param>
        /// <returns>True, if moves are allowed, otherwise False</returns>
        protected virtual bool CanMove(TChunk sourceChunk, TChunk targetChunk)
        {
            return true;
        }

        /// <summary>
        /// Determines whether the items shall be removed from their original chunk
        /// </summary>
        protected virtual bool RemoveFromOriginal => false;

        /// <summary>
        /// Moves the given element from one chunk to the other
        /// </summary>
        /// <param name="sourceChunk">The index of the source chunk</param>
        /// <param name="targetChunks">The index of the target chunk</param>
        /// <param name="observable">The observable chunk collection</param>
        protected virtual void Move(int sourceChunk, List<int> targetChunks, IObservableChunk<T, TChunk> observable)
        {
            for(int sourceChunkPosition = targetChunks.Count - 1; sourceChunkPosition >= 0; sourceChunkPosition--)
            {
                var targetChunk = targetChunks[sourceChunkPosition];
                var item = observable.GetChunkItemAt( sourceChunk, sourceChunkPosition );
                observable.AddToChunk( targetChunk, item );
                if(RemoveFromOriginal)
                {
                    observable.RemoveFromChunk( sourceChunk, sourceChunkPosition );
                }
            }
        }

        /// <summary>
        /// Gets the index of a chunk that can be targeted to remove
        /// </summary>
        /// <param name="observable">The observable chunk collection</param>
        /// <param name="chunkStartIndex">The start chunk index to look from</param>
        /// <returns>The index of a chunk that could be targeted to save</returns>
        protected virtual int GetChunkToRemove( IObservableChunk<T, TChunk> observable, int chunkStartIndex )
        {
            var index = chunkStartIndex;
            while(index < observable.ChunkCount && !CanAdjust( observable.Chunks[index] ))
            {
                index++;
            }
            var selectedIndex = index;
            var itemCount = observable.GetChunkSize( index );
            index++;
            while(index < observable.ChunkCount)
            {
                if(CanAdjust( observable.Chunks[index] ))
                {
                    var currentCount = observable.GetChunkSize( index );
                    if(currentCount <= itemCount)
                    {
                        selectedIndex = index;
                        itemCount = currentCount;
                    }
                }
                index++;
            }
            return selectedIndex;
        }

        /// <summary>
        /// Tries to add the item to an existing chunk
        /// </summary>
        /// <param name="observable">The observable chunk collection</param>
        /// <param name="item">The item that shall be added</param>
        /// <param name="sourceStartIndex">The index of the item in the source collection</param>
        /// <returns>True, if the item could be added to a chunk, otherwise False</returns>
        protected virtual bool TryAddToExistingChunk( IObservableChunk<T, TChunk> observable, T item, int sourceStartIndex )
        {
            for(int i = (sourceStartIndex / observable.ChunkSize); i < observable.ChunkCount; i++)
            {
                if(observable.GetChunkSize( i ) < observable.ChunkSize && CanAdjust(observable.Chunks[i]))
                {
                    observable.AddToChunk( i, item );
                    return true;
                }
            }
            return false;
        }

        private sealed class Strategy : IChunkBalancingStrategy<T, TChunk>
        {
            private readonly IObservableChunk<T, TChunk> _instance;
            private readonly LazyBalancingOnRemoveStrategy<T, TChunk> _parent;
            private int _earliestChangeableChunk;

            public Strategy(IObservableChunk<T, TChunk> instance, LazyBalancingOnRemoveStrategy<T, TChunk> parent)
            {
                _instance = instance;
                _parent = parent;
            }

            public void Balance( ref List<TChunk> addedChunks, ref List<TChunk> removedChunks )
            {
                var capacityOfChangeable = (_instance.ChunkCount - _earliestChangeableChunk) * _instance.ChunkSize;
                var elementsInChangeable = _instance.ElementCount;
                if (_earliestChangeableChunk > 0)
                {
                    elementsInChangeable -= _instance.GetAccumulatedElementCount(_earliestChangeableChunk - 1);
                }
                if (capacityOfChangeable - elementsInChangeable < _instance.ChunkSize)
                {
                    return;
                }
                var currentChunkIndex = _earliestChangeableChunk;
                var removeCandidate = _parent.GetChunkToRemove(_instance, _earliestChangeableChunk);
                if (removeCandidate >= _instance.ChunkCount)
                {
                    return;
                }
                var removeChunk = _instance.Chunks[removeCandidate];
                var (elementsToRemove, plan) = BalanceCore(currentChunkIndex, removeCandidate, removeChunk);
                if (elementsToRemove == 0)
                {
                    _parent.Move(removeCandidate, plan, _instance);
                    _instance.RemoveChunk(removeCandidate, ref removedChunks);
                }
            }

            private (int elementsToRemove, List<int> plan) BalanceCore(int currentChunkIndex, int removeCandidate, TChunk removeChunk)
            {
                var elementsToRemove = _instance.GetChunkSize(removeCandidate);
                var plan = new List<int>();
                while (elementsToRemove > 0 && currentChunkIndex < _instance.ChunkCount)
                {
                    if (currentChunkIndex != removeCandidate)
                    {
                        var elementsInCurrent = _instance.GetChunkSize(currentChunkIndex);
                        if (elementsInCurrent < _instance.ChunkSize)
                        {
                            PlanMove(currentChunkIndex, removeCandidate, removeChunk, plan, ref elementsToRemove, ref elementsInCurrent);
                        }
                    }
                    currentChunkIndex++;
                }

                return (elementsToRemove, plan);
            }

            private void PlanMove(int currentChunkIndex, int removeCandidate, TChunk removeChunk, List<int> plan, ref int elementsToRemove, ref int elementsInCurrent)
            {
                if (_parent.CanMove(removeChunk, _instance.Chunks[currentChunkIndex]))
                {
                    while (elementsInCurrent < _instance.ChunkSize)
                    {
                        elementsInCurrent++;
                        elementsToRemove--;
                        plan.Add(currentChunkIndex);
                    }
                }
                else
                {
                    _earliestChangeableChunk = currentChunkIndex + 1;
                    plan.Clear();
                    elementsToRemove = _instance.GetChunkSize(removeCandidate);
                }
            }

            public bool TryAddToExistingChunk( T item, int startingIndex )
            {
                var capacityOfChangeable = (_instance.ChunkCount - _earliestChangeableChunk) * _instance.ChunkSize;
                var elementsInChangeable = _instance.ElementCount;
                if(_earliestChangeableChunk > 0)
                {
                    elementsInChangeable -= _instance.GetAccumulatedElementCount( _earliestChangeableChunk - 1 );
                }
                if(capacityOfChangeable - elementsInChangeable == 0)
                {
                    return false;
                }
                return _parent.TryAddToExistingChunk( _instance, item, startingIndex );
            }
        }
    }
}
