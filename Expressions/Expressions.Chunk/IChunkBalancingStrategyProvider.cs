using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Expressions.Linq
{
    /// <summary>
    /// Denotes a component that can generically provide chunk balancing strategies
    /// </summary>
    public interface IChunkBalancingStrategyProvider
    {
        /// <summary>
        /// Create a strategy for the given observable chunk collection
        /// </summary>
        /// <typeparam name="T">The type of items</typeparam>
        /// <typeparam name="TChunk">The type of the chunks</typeparam>
        /// <param name="observableChunk">The observable chunk collection</param>
        /// <returns>A chunk balancing strategy</returns>
        IChunkBalancingStrategy<T, TChunk> CreateStrategy<T, TChunk>( IObservableChunk<T, TChunk> observableChunk );
    }

    /// <summary>
    /// Denotes a component that can provide chunk balancing strategies for specific types
    /// </summary>
    /// <typeparam name="T">The type of items</typeparam>
    /// <typeparam name="TChunk">The type of the chunks</typeparam>
    public interface IChunkBalancingStrategyProvider<T, TChunk>
    {
        /// <summary>
        /// Create a strategy for the given observable chunk collection
        /// </summary>
        /// <param name="observableChunk">The observable chunk collection</param>
        /// <returns>A chunk balancing strategy</returns>
        IChunkBalancingStrategy<T, TChunk> CreateStrategy( IObservableChunk<T, TChunk> observableChunk );
    }
}
