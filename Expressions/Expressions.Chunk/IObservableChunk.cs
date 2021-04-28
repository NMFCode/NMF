using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Expressions.Linq
{
    /// <summary>
    /// Denotes an abstract observable chunk collection
    /// </summary>
    /// <typeparam name="T">The type of items</typeparam>
    /// <typeparam name="TChunk">The type of chunks</typeparam>
    public interface IObservableChunk<T, TChunk>
    {
        /// <summary>
        /// Creates a chunk for the given item
        /// </summary>
        /// <param name="item">The item that should start a new chunk</param>
        /// <returns>The created chunk</returns>
        TChunk CreateChunkForItem( T item );

        /// <summary>
        /// Gets the amount of chunks
        /// </summary>
        int ChunkCount { get; }

        /// <summary>
        /// Gets the size of the chunks
        /// </summary>
        int ChunkSize { get; }

        /// <summary>
        /// The the total number of elements
        /// </summary>
        int ElementCount { get; }

        /// <summary>
        /// Gets the current size of the chunk with the given index
        /// </summary>
        /// <param name="chunkIndex">The index of the chunk</param>
        /// <returns>The number of elements in the chunk</returns>
        int GetChunkSize( int chunkIndex );

        /// <summary>
        /// Adds the given element to the chunk with the provided index
        /// </summary>
        /// <param name="chunkIndex">The index of the chunk</param>
        /// <param name="item">The item that should be added</param>
        void AddToChunk( int chunkIndex, T item );

        /// <summary>
        /// Removes the element with the given position from the chunk
        /// </summary>
        /// <param name="chunkIndex">The index of the chunk</param>
        /// <param name="chunkPosition">The index within the chunk</param>
        void RemoveFromChunk( int chunkIndex, int chunkPosition );

        /// <summary>
        /// Removes the provided chunk completely
        /// </summary>
        /// <param name="chunkIndex">The index of the chunk</param>
        /// <param name="removeList">A reference to a list used for notifications</param>
        void RemoveChunk( int chunkIndex, ref List<TChunk> removeList );

        /// <summary>
        /// Gets the cumulative number of elements up the given chunk index
        /// </summary>
        /// <param name="chunkIndex">The index of the chunk</param>
        /// <returns>The number of all elements up to and including the provided chunk</returns>
        int GetAccumulatedElementCount( int chunkIndex );

        /// <summary>
        /// Gets the item at the provided position
        /// </summary>
        /// <param name="chunkIndex">The index of the chunk</param>
        /// <param name="chunkPosition">The position within the chunk</param>
        /// <returns>The item</returns>
        T GetChunkItemAt( int chunkIndex, int chunkPosition );

        /// <summary>
        /// Gets the collection of chunks
        /// </summary>
        IReadOnlyList<TChunk> Chunks { get; }
    }
}
