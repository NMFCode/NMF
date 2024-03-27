using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace NMF.Expressions.Linq.Chunk.Test
{
    [TestClass]
    public class ChunkIndexedTest
    {
        [TestMethod]
        public void ChunkIndexed_Classic_Correct()
        {
            var list = new List<int>() { 0, 8, 15, 23, 42 };
            var chunks = list.ChunkIndexed( 2, ( numbers, index ) => $"{index}:{string.Join( ",", numbers.Select( n => n.ToString() ) )}" ).ToList();
            Assert.AreEqual( 3, chunks.Count );
            Assert.AreEqual( "0:(0, 0),(8, 1)", chunks[0] );
            Assert.AreEqual( "1:(15, 2),(23, 3)", chunks[1] );
            Assert.AreEqual( "2:(42, 4)", chunks[2] );
        }

        [TestMethod]
        public void ChunkIndexed_Batch_Correct()
        {
            var list = new ObservableList<int>() { 0, 8, 15, 23, 42 };
            var chunks = list.ChunkIndexed( 2, ( numbers, index ) => $"{index}:{string.Join( ",", numbers.Select( n => n.ToString() ) )}" ).ToList();
            Assert.AreEqual( 3, chunks.Count );
            Assert.AreEqual( "0:(0, 0),(8, 1)", chunks[0] );
            Assert.AreEqual( "1:(15, 2),(23, 3)", chunks[1] );
            Assert.AreEqual( "2:(42, 4)", chunks[2] );
        }

        [TestMethod]
        public void ChunkIndexed_BatchIncremental_Correct()
        {
            var list = new NotifyCollection<int>() { 0, 8, 15, 23, 42 };
            var chunks = (list as INotifyEnumerable<int>).ChunkIndexed( 2, ( numbers, index ) => $"{index}:{string.Join( ",", numbers.Select( n => n.ToString() ) )}" ).ToList();
            Assert.AreEqual( 3, chunks.Count );
            Assert.AreEqual( "0:(0, 0),(8, 1)", chunks[0] );
            Assert.AreEqual( "1:(15, 2),(23, 3)", chunks[1] );
            Assert.AreEqual( "2:(42, 4)", chunks[2] );
        }

        [TestMethod]
        public void ChunkIndexed_IncrementalAdditions_Correct()
        {
            var list = new ObservableList<int>() { 0, 8, 15, 23, 42 };
            var chunks = list.ChunkIndexed( 2, ( numbers, index ) => (numbers, index) ).AsNotifiable();

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
            Assert.IsTrue( chunkChanges.NewItems.Contains( (65, 5) ) );

            chunksChanged = false;
            changedChunk = null;

            list.Add( 75 );
            Assert.IsTrue( chunksChanged );
            Assert.IsFalse( changedChunk.HasValue );
        }

        [TestMethod]
        public void ChunkIndexed_IncrementalDeletions_Correct()
        {
            var list = new ObservableList<int>() { 0, 8, 15, 23, 42 };
            var chunks = list.ChunkIndexed( 2, ( numbers, index ) => (numbers, index) ).AsNotifiable();

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
            Assert.IsTrue( chunkChanges.OldItems.Contains( (23, 3) ) );

            chunksChanged = false;
            changedChunk = null;

            list.Remove( 42 );
            Assert.IsTrue( chunksChanged );
            Assert.IsTrue( changedChunk.HasValue );
            Assert.IsTrue( chunkChanges.OldItems.Contains( (42, 4) ) );
        }
    }
}
