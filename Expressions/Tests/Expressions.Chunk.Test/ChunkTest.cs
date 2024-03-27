using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace NMF.Expressions.Linq.Chunk.Test
{
    [TestClass]
    public class ChunkTest
    {
        [TestMethod]
        public void Chunk_Classic_Correct()
        {
            var list = new List<int>() { 0, 8, 15, 23, 42 };
            var chunks = list.Chunk( 2, (numbers, index) => $"{index}:{string.Join(",", numbers.Select(n => n.ToString()))}" ).ToList();
            Assert.AreEqual( 3, chunks.Count );
            Assert.AreEqual( "0:0,8", chunks[0] );
            Assert.AreEqual( "1:15,23", chunks[1] );
            Assert.AreEqual( "2:42", chunks[2] );
        }

        [TestMethod]
        public void Chunk_Batch_Correct()
        {
            var list = new ObservableList<int>() { 0, 8, 15, 23, 42 };
            var chunks = list.Chunk( 2, ( numbers, index ) => $"{index}:{string.Join( ",", numbers.Select( n => n.ToString() ) )}" ).ToList();
            Assert.AreEqual( 3, chunks.Count );
            Assert.AreEqual( "0:0,8", chunks[0] );
            Assert.AreEqual( "1:15,23", chunks[1] );
            Assert.AreEqual( "2:42", chunks[2] );
        }

        [TestMethod]
        public void Chunk_BatchIncremental_Correct()
        {
            var list = new NotifyCollection<int>() { 0, 8, 15, 23, 42 };
            var chunks = (list as INotifyEnumerable<int>).Chunk( 2, ( numbers, index ) => $"{index}:{string.Join( ",", numbers.Select( n => n.ToString() ) )}" ).ToList();
            Assert.AreEqual( 3, chunks.Count );
            Assert.AreEqual( "0:0,8", chunks[0] );
            Assert.AreEqual( "1:15,23", chunks[1] );
            Assert.AreEqual( "2:42", chunks[2] );
        }

        [TestMethod]
        public void Chunk_IncrementalAdditions_Correct()
        {
            var list = new ObservableList<int>() { 0, 8, 15, 23, 42 };
            var chunks = list.Chunk( 2, ( numbers, index ) => (numbers, index) ).AsNotifiable();

            var chunksChanged = false;
            var changedChunk = null as int?;
            var chunkChanges = null as NotifyCollectionChangedEventArgs;
            chunks.CollectionChanged += ( o, e ) =>
            {
                chunksChanged = true;
            };

            foreach(var chunk in chunks)
            {
                var incrementalItems = chunk.numbers as INotifyCollectionChanged;
                incrementalItems.CollectionChanged += ( o, e ) =>
                {
                    changedChunk = chunk.index;
                    chunkChanges = e;
                };
            }

            Assert.AreEqual( 3, chunks.Count() );
            Assert.IsFalse( chunksChanged );
            Assert.IsFalse( changedChunk.HasValue );

            list.Add( 65 );
            Assert.IsFalse( chunksChanged );
            Assert.IsTrue( changedChunk.HasValue );
            Assert.AreEqual( 2, changedChunk.Value );
            Assert.IsTrue( chunkChanges.NewItems.Contains( 65 ) );

            chunksChanged = false;
            changedChunk = null;

            list.Add( 75 );
            Assert.IsTrue( chunksChanged );
            Assert.IsFalse( changedChunk.HasValue );
        }

        [TestMethod]
        public void Chunk_IncrementalDeletions_Correct()
        {
            var list = new ObservableList<int>() { 0, 8, 15, 23, 42 };
            var chunks = list.Chunk( 2, ( numbers, index ) => (numbers, index) ).AsNotifiable();

            var chunksChanged = false;
            var changedChunk = null as int?;
            var chunkChanges = null as NotifyCollectionChangedEventArgs;
            chunks.CollectionChanged += ( o, e ) =>
            {
                chunksChanged = true;
            };

            foreach(var chunk in chunks)
            {
                var incrementalItems = chunk.numbers as INotifyCollectionChanged;
                incrementalItems.CollectionChanged += ( o, e ) =>
                {
                    changedChunk = chunk.index;
                    chunkChanges = e;
                };
            }

            Assert.AreEqual( 3, chunks.Count() );
            Assert.IsFalse( chunksChanged );
            Assert.IsFalse( changedChunk.HasValue );

            list.Remove( 23 );
            Assert.IsFalse( chunksChanged );
            Assert.IsTrue( changedChunk.HasValue );
            Assert.AreEqual( 1, changedChunk.Value );
            Assert.IsTrue( chunkChanges.OldItems.Contains( 23 ) );

            chunksChanged = false;
            changedChunk = null;

            list.Remove( 42 );
            Assert.IsTrue( chunksChanged );
            Assert.IsTrue( changedChunk.HasValue );
            Assert.IsTrue( chunkChanges.OldItems.Contains( 42 ) );
        }

        [TestMethod]
        public void Chunk_IncrementalDeletions_LazyBalancing_Correct()
        {
            var list = new ObservableList<int>() { 0, 8, 15, 23, 42 };
            var chunks = list.Chunk( 2, ( numbers, index ) => Tuple.Create(numbers, index), new LazyBalancingOnRemoveStrategy<int, Tuple<IEnumerableExpression<int>, int>>() ).AsNotifiable();

            var chunksChanged = false;
            var changedChunk = null as int?;
            var chunkChanges = new List<NotifyCollectionChangedEventArgs>();
            chunks.CollectionChanged += ( o, e ) =>
            {
                chunksChanged = true;
            };

            foreach(var chunk in chunks)
            {
                var incrementalItems = chunk.Item1 as INotifyCollectionChanged;
                incrementalItems.CollectionChanged += ( o, e ) =>
                {
                    Assert.AreEqual( chunk.Item2, changedChunk.GetValueOrDefault( chunk.Item2 ) );
                    changedChunk = chunk.Item2;
                    chunkChanges.Add( e );
                };
            }

            Assert.AreEqual( 3, chunks.Count() );
            Assert.IsFalse( chunksChanged );
            Assert.IsFalse( changedChunk.HasValue );

            list.Remove( 23 );
            Assert.IsTrue( chunksChanged );
            Assert.IsTrue( changedChunk.HasValue );
            Assert.AreEqual( 1, changedChunk.Value );
            Assert.AreEqual( 2, chunkChanges.Count );
            Assert.IsTrue( chunkChanges[0].OldItems.Contains( 23 ) );
            Assert.IsTrue( chunkChanges[1].NewItems.Contains( 42 ) );
        }

        [TestMethod]
        public void Chunk_IncrementalAdditions_LazyBalancing_FillsExistingChunks()
        {
            var list = new ObservableList<int>() { 0, 8, 15, 23, 42, 1000 };
            var chunks = list.Chunk( 3, ( numbers, index ) => Tuple.Create( numbers, index ), new LazyBalancingOnRemoveStrategy<int, Tuple<IEnumerableExpression<int>, int>>() ).AsNotifiable();

            var chunksChanged = false;
            var changedChunk = null as int?;
            var chunkChanges = new List<NotifyCollectionChangedEventArgs>();
            chunks.CollectionChanged += ( o, e ) =>
            {
                chunksChanged = true;
            };

            foreach(var chunk in chunks)
            {
                var incrementalItems = chunk.Item1 as INotifyCollectionChanged;
                incrementalItems.CollectionChanged += ( o, e ) =>
                {
                    Assert.AreEqual( chunk.Item2, changedChunk.GetValueOrDefault( chunk.Item2 ) );
                    changedChunk = chunk.Item2;
                    chunkChanges.Add( e );
                };
            }

            Assert.AreEqual( 2, chunks.Count() );
            Assert.IsFalse( chunksChanged );
            Assert.IsFalse( changedChunk.HasValue );

            list.Remove( 23 );
            Assert.IsFalse( chunksChanged );
            Assert.IsTrue( changedChunk.HasValue );
            Assert.AreEqual( 1, changedChunk.Value );
            Assert.AreEqual( 1, chunkChanges.Count );
            Assert.IsTrue( chunkChanges[0].OldItems.Contains( 23 ) );

            changedChunk = null;
            chunkChanges.Clear();

            list.Add( 17 );
            Assert.IsFalse( chunksChanged );
            Assert.IsTrue( changedChunk.HasValue );
            Assert.AreEqual( 1, changedChunk.Value );
            Assert.AreEqual( 1, chunkChanges.Count );
            Assert.IsTrue( chunkChanges[0].NewItems.Contains( 17 ) );
        }
    }
}
