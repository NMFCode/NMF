using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Expressions.Linq
{
    /// <summary>
    /// Denotes a class representing no balancing strategy at all
    /// </summary>
    public sealed class NoBalancingStrategy : IChunkBalancingStrategyProvider
    {
        
        /// <inheritdoc />
        public IChunkBalancingStrategy<T, TChunk> CreateStrategy<T, TChunk>( IObservableChunk<T, TChunk> observableChunk )
        {
            return NoBalancingStrategy<T, TChunk>.Instance.CreateStrategy( observableChunk );
        }
    }

    /// <summary>
    /// Denotes a class representing no balancing strategy at all
    /// </summary>

    public sealed class NoBalancingStrategy<T, TChunk> : IChunkBalancingStrategyProvider<T, TChunk>, IChunkBalancingStrategy<T, TChunk>
    {
        /// <summary>
        /// Gets the default instance
        /// </summary>
        public static readonly NoBalancingStrategy<T, TChunk> Instance = new NoBalancingStrategy<T, TChunk>();

        /// <inheritdoc />
        public void Balance( ref List<TChunk> addedChunks, ref List<TChunk> removedChunks )
        {
        }

        /// <inheritdoc />
        public IChunkBalancingStrategy<T, TChunk> CreateStrategy( IObservableChunk<T, TChunk> observableChunk )
        {
            return this;
        }

        /// <inheritdoc />
        public bool TryAddToExistingChunk( T item, int startingIndex )
        {
            return false;
        }
    }
}
