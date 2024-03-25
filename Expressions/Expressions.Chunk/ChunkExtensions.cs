using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Expressions.Linq
{
    /// <summary>
    /// Denotes chunking extensions
    /// </summary>
    public static class ChunkExtensions
    {

        internal static IEnumerable<TResult> ChunkIndexedCore<T, TResult>( IEnumerable<T> source, int chunkSize, Func<IEnumerableExpression<(T, int)>, int, TResult> resultConverter )
        {
            var index = 0;
            var chunkIndex = -1;
            DummyExpression<(T, int)> chunk = null;
            foreach(var item in source)
            {
                if(index % chunkSize == 0)
                {
                    if(chunk != null)
                    {
                        yield return resultConverter( chunk, chunkIndex );
                    }
                    chunk = new DummyExpression<(T, int)>();
                    chunkIndex++;
                }
#pragma warning disable S2259 // Null pointers should not be dereferenced
                chunk.Add( (item, index) );
#pragma warning restore S2259 // Null pointers should not be dereferenced
                index++;
            }
            if(chunk != null)
            {
                yield return resultConverter( chunk, chunkIndex );
            }
        }

        /// <summary>
        /// Chunks the given collection and keeps the index of the elements in the original collection
        /// </summary>
        /// <typeparam name="T">The type of elements</typeparam>
        /// <typeparam name="TResult">The type of chunks</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="chunkSize">The size of the chunks</param>
        /// <param name="resultSelector">A function that converts a collection of elements and the index of the chunk into a new chunk</param>
        /// <returns>A collection of chunks</returns>
        public static IEnumerable<TResult> ChunkIndexed<T, TResult>( this IEnumerable<T> source, int chunkSize, Func<IEnumerableExpression<(T, int)>, int, TResult> resultSelector )
        {
            if(source is IEnumerableExpression<T> sourceExpression)
            {
                return ChunkIndexed( sourceExpression, chunkSize, resultSelector );
            }
            else if(source is INotifyEnumerable<T> notifiable)
            {
                return ChunkIndexed( notifiable, chunkSize, resultSelector );
            }
            else
            {
                return ChunkIndexedCore( source, chunkSize, resultSelector );
            }
        }

        /// <summary>
        /// Chunks the given collection and keeps the index of the elements in the original collection
        /// </summary>
        /// <typeparam name="T">The type of elements</typeparam>
        /// <typeparam name="TResult">The type of chunks</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="chunkSize">The size of the chunks</param>
        /// <param name="resultSelector">A function that converts a collection of elements and the index of the chunk into a new chunk</param>
        /// <returns>A collection of chunks</returns>
        public static IEnumerableExpression<TResult> ChunkIndexed<T, TResult>( this IEnumerableExpression<T> source, int chunkSize, Func<IEnumerableExpression<(T, int)>, int, TResult> resultSelector )
        {
            return new ChunkIndexedExpression<T, TResult>( source, chunkSize, resultSelector, null );
        }

        /// <summary>
        /// Chunks the given collection and keeps the index of the elements in the original collection
        /// </summary>
        /// <typeparam name="T">The type of elements</typeparam>
        /// <typeparam name="TResult">The type of chunks</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="chunkSize">The size of the chunks</param>
        /// <param name="resultSelector">A function that converts a collection of elements and the index of the chunk into a new chunk</param>
        /// <returns>A collection of chunks</returns>
        public static INotifyEnumerable<TResult> ChunkIndexed<T, TResult>( this INotifyEnumerable<T> source, int chunkSize, Func<IEnumerableExpression<(T, int)>, int, TResult> resultSelector )
        {
            return new ObservableIndexedChunkCollection<T, TResult>( source, chunkSize, resultSelector, null );
        }

        /// <summary>
        /// Chunks the given collection and keeps the index of the elements in the original collection
        /// </summary>
        /// <typeparam name="T">The type of elements</typeparam>
        /// <typeparam name="TResult">The type of chunks</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="chunkSize">The size of the chunks</param>
        /// <param name="resultSelector">A function that converts a collection of elements and the index of the chunk into a new chunk</param>
        /// <param name="balancingStrategyProvider">A component that defines how to balance the chunk collection when items are added or deleted in the source collection</param>
        /// <returns>A collection of chunks</returns>
        public static IEnumerableExpression<TResult> ChunkIndexed<T, TResult>( this IEnumerableExpression<T> source, int chunkSize, Func<IEnumerableExpression<(T, int)>, int, TResult> resultSelector, IChunkBalancingStrategyProvider<(T, int), TResult> balancingStrategyProvider )
        {
            return new ChunkIndexedExpression<T, TResult>( source, chunkSize, resultSelector, balancingStrategyProvider );
        }

        /// <summary>
        /// Chunks the given collection and keeps the index of the elements in the original collection
        /// </summary>
        /// <typeparam name="T">The type of elements</typeparam>
        /// <typeparam name="TResult">The type of chunks</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="chunkSize">The size of the chunks</param>
        /// <param name="resultSelector">A function that converts a collection of elements and the index of the chunk into a new chunk</param>
        /// <param name="balancingStrategyProvider">A component that defines how to balance the chunk collection when items are added or deleted in the source collection</param>
        /// <returns>A collection of chunks</returns>
        public static INotifyEnumerable<TResult> ChunkIndexed<T, TResult>( this INotifyEnumerable<T> source, int chunkSize, Func<IEnumerableExpression<(T, int)>, int, TResult> resultSelector, IChunkBalancingStrategyProvider<(T, int), TResult> balancingStrategyProvider )
        {
            return new ObservableIndexedChunkCollection<T, TResult>( source, chunkSize, resultSelector, balancingStrategyProvider );
        }

        /// <summary>
        /// Chunks the given collection and keeps the index of the elements in the original collection
        /// </summary>
        /// <typeparam name="T">The type of elements</typeparam>
        /// <typeparam name="TResult">The type of chunks</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="chunkSize">The size of the chunks</param>
        /// <param name="resultSelector">A function that converts a collection of elements and the index of the chunk into a new chunk</param>
        /// <param name="balancingStrategyProvider">A component that defines how to balance the chunk collection when items are added or deleted in the source collection</param>
        /// <returns>A collection of chunks</returns>
        public static IEnumerableExpression<TResult> ChunkIndexed<T, TResult>( this IEnumerableExpression<T> source, int chunkSize, Func<IEnumerableExpression<(T, int)>, int, TResult> resultSelector, IChunkBalancingStrategyProvider balancingStrategyProvider )
        {
            return new ChunkIndexedExpression<T, TResult>( source, chunkSize, resultSelector, new BalancingStrategyProvider<(T, int), TResult>( balancingStrategyProvider ) );
        }

        /// <summary>
        /// Chunks the given collection and keeps the index of the elements in the original collection
        /// </summary>
        /// <typeparam name="T">The type of elements</typeparam>
        /// <typeparam name="TResult">The type of chunks</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="chunkSize">The size of the chunks</param>
        /// <param name="resultSelector">A function that converts a collection of elements and the index of the chunk into a new chunk</param>
        /// <param name="balancingStrategyProvider">A component that defines how to balance the chunk collection when items are added or deleted in the source collection</param>
        /// <returns>A collection of chunks</returns>
        public static INotifyEnumerable<TResult> ChunkIndexed<T, TResult>( this INotifyEnumerable<T> source, int chunkSize, Func<IEnumerableExpression<(T, int)>, int, TResult> resultSelector, IChunkBalancingStrategyProvider balancingStrategyProvider )
        {
            return new ObservableIndexedChunkCollection<T, TResult>( source, chunkSize, resultSelector, new BalancingStrategyProvider<(T, int), TResult>( balancingStrategyProvider ) );
        }

        internal static IEnumerable<TResult> ChunkCore<T, TResult>(IEnumerable<T> source, int chunkSize, Func<IEnumerableExpression<T>, int, TResult> resultConverter)
        {
            var index = 0;
            var chunkIndex = -1;
            DummyExpression<T> chunk = null;
            foreach (var item in source)
            {
                if (index % chunkSize == 0)
                {
                    if (chunk != null)
                    {
                        yield return resultConverter(chunk, chunkIndex);
                    }
                    chunk = new DummyExpression<T>();
                    chunkIndex++;
                }
#pragma warning disable S2259 // Null pointers should not be dereferenced
                chunk.Add(item);
#pragma warning restore S2259 // Null pointers should not be dereferenced
                index++;
            }
            if (chunk != null)
            {
                yield return resultConverter(chunk, chunkIndex);
            }
        }

        /// <summary>
        /// Chunks the given collection
        /// </summary>
        /// <typeparam name="T">The type of elements</typeparam>
        /// <typeparam name="TResult">The type of chunks</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="chunkSize">The size of the chunks</param>
        /// <param name="resultSelector">A function that converts a collection of elements and the index of the chunk into a new chunk</param>
        /// <returns>A collection of chunks</returns>
        public static IEnumerable<TResult> Chunk<T, TResult>( this IEnumerable<T> source, int chunkSize, Func<IEnumerableExpression<T>, int, TResult> resultSelector )
        {
            if(source is IEnumerableExpression<T> sourceExpression)
            {
                return Chunk( sourceExpression, chunkSize, resultSelector );
            }
            else if(source is INotifyEnumerable<T> notifiable)
            {
                return Chunk( notifiable, chunkSize, resultSelector );
            }
            else
            {
                return ChunkCore( source, chunkSize, resultSelector );
            }
        }

        /// <summary>
        /// Chunks the given collection
        /// </summary>
        /// <typeparam name="T">The type of elements</typeparam>
        /// <typeparam name="TResult">The type of chunks</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="chunkSize">The size of the chunks</param>
        /// <param name="resultSelector">A function that converts a collection of elements and the index of the chunk into a new chunk</param>
        /// <returns>A collection of chunks</returns>
        public static IEnumerableExpression<TResult> Chunk<T, TResult>( this IEnumerableExpression<T> source, int chunkSize, Func<IEnumerableExpression<T>, int, TResult> resultSelector )
        {
            return new ChunkExpression<T, TResult>( source, chunkSize, resultSelector, null );
        }

        /// <summary>
        /// Chunks the given collection
        /// </summary>
        /// <typeparam name="T">The type of elements</typeparam>
        /// <typeparam name="TResult">The type of chunks</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="chunkSize">The size of the chunks</param>
        /// <param name="resultSelector">A function that converts a collection of elements and the index of the chunk into a new chunk</param>
        /// <returns>A collection of chunks</returns>
        public static INotifyEnumerable<TResult> Chunk<T, TResult>( this INotifyEnumerable<T> source, int chunkSize, Func<IEnumerableExpression<T>, int, TResult> resultSelector )
        {
            return new ObservableChunkCollection<T, TResult>( source, chunkSize, resultSelector, null );
        }

        /// <summary>
        /// Chunks the given collection
        /// </summary>
        /// <typeparam name="T">The type of elements</typeparam>
        /// <typeparam name="TResult">The type of chunks</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="chunkSize">The size of the chunks</param>
        /// <param name="resultSelector">A function that converts a collection of elements and the index of the chunk into a new chunk</param>
        /// <param name="balancingStrategyProvider">A component that defines how to balance the chunk collection when items are added or deleted in the source collection</param>
        /// <returns>A collection of chunks</returns>
        public static IEnumerableExpression<TResult> Chunk<T, TResult>( this IEnumerableExpression<T> source, int chunkSize, Func<IEnumerableExpression<T>, int, TResult> resultSelector, IChunkBalancingStrategyProvider<T, TResult> balancingStrategyProvider )
        {
            return new ChunkExpression<T, TResult>( source, chunkSize, resultSelector, balancingStrategyProvider );
        }

        /// <summary>
        /// Chunks the given collection
        /// </summary>
        /// <typeparam name="T">The type of elements</typeparam>
        /// <typeparam name="TResult">The type of chunks</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="chunkSize">The size of the chunks</param>
        /// <param name="resultSelector">A function that converts a collection of elements and the index of the chunk into a new chunk</param>
        /// <param name="balancingStrategyProvider">A component that defines how to balance the chunk collection when items are added or deleted in the source collection</param>
        /// <returns>A collection of chunks</returns>
        public static INotifyEnumerable<TResult> Chunk<T, TResult>( this INotifyEnumerable<T> source, int chunkSize, Func<IEnumerableExpression<T>, int, TResult> resultSelector, IChunkBalancingStrategyProvider<T, TResult> balancingStrategyProvider )
        {
            return new ObservableChunkCollection<T, TResult>( source, chunkSize, resultSelector, balancingStrategyProvider );
        }

        /// <summary>
        /// Chunks the given collection
        /// </summary>
        /// <typeparam name="T">The type of elements</typeparam>
        /// <typeparam name="TResult">The type of chunks</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="chunkSize">The size of the chunks</param>
        /// <param name="resultSelector">A function that converts a collection of elements and the index of the chunk into a new chunk</param>
        /// <param name="balancingStrategyProvider">A component that defines how to balance the chunk collection when items are added or deleted in the source collection</param>
        /// <returns>A collection of chunks</returns>
        public static IEnumerableExpression<TResult> Chunk<T, TResult>( this IEnumerableExpression<T> source, int chunkSize, Func<IEnumerableExpression<T>, int, TResult> resultSelector, IChunkBalancingStrategyProvider balancingStrategyProvider )
        {
            return new ChunkExpression<T, TResult>( source, chunkSize, resultSelector, new BalancingStrategyProvider<T, TResult>( balancingStrategyProvider ) );
        }

        /// <summary>
        /// Chunks the given collection
        /// </summary>
        /// <typeparam name="T">The type of elements</typeparam>
        /// <typeparam name="TResult">The type of chunks</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="chunkSize">The size of the chunks</param>
        /// <param name="resultSelector">A function that converts a collection of elements and the index of the chunk into a new chunk</param>
        /// <param name="balancingStrategyProvider">A component that defines how to balance the chunk collection when items are added or deleted in the source collection</param>
        /// <returns>A collection of chunks</returns>
        public static INotifyEnumerable<TResult> Chunk<T, TResult>( this INotifyEnumerable<T> source, int chunkSize, Func<IEnumerableExpression<T>, int, TResult> resultSelector, IChunkBalancingStrategyProvider balancingStrategyProvider )
        {
            return new ObservableChunkCollection<T, TResult>( source, chunkSize, resultSelector, new BalancingStrategyProvider<T, TResult>( balancingStrategyProvider ) );
        }
    }
}
