using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace NMF.Expressions
{
    /// <summary>
    /// Denotes a helper class to create bindings between collections
    /// </summary>
    public static class CollectionBinding
    {
        /// <summary>
        /// Creates a binding between the given collections
        /// </summary>
        /// <typeparam name="T">The type of the elements</typeparam>
        /// <param name="sourceCollection">The source collection</param>
        /// <param name="targetCollection">The target collection</param>
        /// <returns>An object that can be disposed to revoke the binding</returns>
        public static IDisposable Create<T>( IEnumerableExpression<T> sourceCollection, ICollection<T> targetCollection )
        {
            return Create( sourceCollection, targetCollection, true );
        }

        /// <summary>
        /// Creates a binding between the given collections
        /// </summary>
        /// <typeparam name="T">The type of the elements</typeparam>
        /// <param name="sourceCollection">The source collection</param>
        /// <param name="targetCollection">The target collection</param>
        /// <param name="force">True, if the contents of the target collection should be forced to the source collection at start</param>
        /// <returns>An object that can be disposed to revoke the binding</returns>
        public static IDisposable Create<T>( IEnumerableExpression<T> sourceCollection, ICollection<T> targetCollection, bool force )
        {
            if(sourceCollection == null) throw new ArgumentNullException( nameof( sourceCollection ) );
            if(targetCollection == null) throw new ArgumentNullException( nameof( targetCollection ) );
            return new CollectionBinding<T>( sourceCollection.AsNotifiable(), targetCollection, force );
        }
    }

    internal class CollectionBinding<T> : IDisposable
    {
        public static void SynchronizeCollectionsLeftToRight( IEnumerable<T> lefts, ICollection<T> rights, bool force )
        {
            if(rights.IsReadOnly) throw new InvalidOperationException( "Collection is read-only!" );
            IEnumerable<T> rightsSaved;
            HashSet<T> doubles;
            if(!force)
            {
                rightsSaved = null;
                doubles = null;
            }
            else
            {
                rightsSaved = rights.ToArray();
                doubles = new HashSet<T>();
            }
            foreach(var item in lefts)
            {
                if(!rights.Contains( item ))
                {
                    rights.Add( item );
                }
                else if(force)
                {
                    doubles.Add( item );
                }
            }
            if(force)
            {
                foreach(var item in rightsSaved.Except( doubles ))
                {
                    rights.Remove( item );
                }
            }
        }

        public static void ProcessLeftChangesForRights( IEnumerable<T> lefts, ICollection<T> rights, NotifyCollectionChangedEventArgs e )
        {
            if(e.Action != NotifyCollectionChangedAction.Reset)
            {
                if(e.OldItems != null)
                {
                    RemoveCore(rights, e);
                }
                if (e.NewItems != null)
                {
                    AddCore(rights, e);
                }
            }
            else
            {
                SynchronizeCollectionsLeftToRight( lefts, rights, true );
            }
        }

        private static void AddCore(ICollection<T> rights, NotifyCollectionChangedEventArgs e)
        {
            for (int i = 0; i < e.NewItems.Count; i++)
            {
                T item = (T)e.NewItems[i];
                rights.Add(item);
            }
        }

        private static void RemoveCore(ICollection<T> rights, NotifyCollectionChangedEventArgs e)
        {
            for (int i = e.OldItems.Count - 1; i >= 0; i--)
            {
                T item = (T)e.OldItems[i];
                if (item != null)
                {
                    rights.Remove(item);
                }
            }
        }

        private readonly INotifyEnumerable<T> _source;
        private readonly ICollection<T> _target;

        public CollectionBinding(INotifyEnumerable<T> source, ICollection<T> target, bool force)
        {
            _source = source;
            _target = target;

            SynchronizeCollectionsLeftToRight( _source, _target, force );
            _source.CollectionChanged += SourceCollectionChanged;
        }

        private void SourceCollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
        {
            ProcessLeftChangesForRights( _source, _target, e );
        }

        public void Dispose()
        {
            _source.CollectionChanged -= SourceCollectionChanged;
        }
    }
}
