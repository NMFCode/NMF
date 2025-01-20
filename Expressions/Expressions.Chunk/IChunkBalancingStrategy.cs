using System.Collections.Generic;

namespace NMF.Expressions.Linq
{
    /// <summary>
    /// Denotes a balancing strategy when items are added or deleted dynamically
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TChunk"></typeparam>
    public interface IChunkBalancingStrategy<T, TChunk>
    {
        /// <summary>
        /// Tries to add the given item to an existing chunk
        /// </summary>
        /// <param name="item">The item that should be added</param>
        /// <param name="startingIndex">The starting index</param>
        /// <returns>True, if the item could be added to an existing chunk, otherwise False</returns>
        bool TryAddToExistingChunk( T item, int startingIndex );

        /// <summary>
        /// Balances the chunk collection after items have been removed
        /// </summary>
        /// <param name="addedChunks">A reference to a collection with added chunks</param>
        /// <param name="removedChunks">A reference to a collection with removed chunks</param>
        void Balance( ref List<TChunk> addedChunks, ref List<TChunk> removedChunks );
    }
}
