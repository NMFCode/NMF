using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using NMF.Models.Repository;
using NMF.Serialization;

namespace NMF.Models.Evolution
{
    [XmlConstructor(2)]
    public abstract class CollectionResetBase<T> : IModelChange
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private ModelChangeRecorder _recorder;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private BubbledChangeEventArgs _resetEvent;

        [XmlConstructorParameter(0)]
        public Uri AbsoluteUri { get; }

        [XmlConstructorParameter(1)]
        public string CollectionPropertyName { get; set; }

        public CollectionResetBase(Uri absoluteUri, string collectionPropertyName)
        {
            AbsoluteUri = absoluteUri;
            CollectionPropertyName = collectionPropertyName;
        }

        //Use this only if Invert functionality is necessary
        public CollectionResetBase(Uri absoluteUri, string collectionPropertyName, ModelChangeRecorder recorder, BubbledChangeEventArgs resetEvent)
        {
            AbsoluteUri = absoluteUri;
            CollectionPropertyName = collectionPropertyName;
            _recorder = recorder;
            _resetEvent = resetEvent;
        }

        public void Apply(IModelRepository repository)
        {
            var parent = repository.Resolve(AbsoluteUri);
            var property = parent.GetType().GetProperty(CollectionPropertyName);
            var coll = property.GetValue(parent, null) as ICollection<T>;

            var newElements = GetNewState(repository);
            coll.Clear();
            foreach (var element in newElements)
            {
                coll.Add(element);
            }
        }

        public void Invert(IModelRepository repository)
        {
            var items = _recorder.GetItemsBeforeReset(_resetEvent); //TODO Serialisierung
            var parent = repository.Resolve(AbsoluteUri);
            var property = parent.GetType().GetProperty(CollectionPropertyName);
            var coll = property.GetValue(parent, null) as ICollection<T>;

            coll.Clear();
            foreach (var item in items.Cast<T>().ToList())
            {
                coll.Add(item);
            }
        }

        protected abstract ICollection<T> GetNewState(IModelRepository repository);
    }

    [XmlConstructor(2)]
    public class CollectionResetComposition<T> : CollectionResetBase<T>
    {
        public ICollection<T> NewState { get; set; }

        //Use this only if Invert functionality is necessary
        [EditorBrowsable(EditorBrowsableState.Never)]
        public CollectionResetComposition(Uri absoluteUri, string collectionPropertyName)
            : base(absoluteUri, collectionPropertyName)
        {
            NewState = new Collection<T>();
        }
        
        public CollectionResetComposition(Uri absoluteUri, string collectionPropertyName, ICollection<T> newState)
            : base(absoluteUri, collectionPropertyName)
        {
            NewState = newState;
        }

        //Use this only if Invert functionality is necessary
        [EditorBrowsable(EditorBrowsableState.Never)]
        public CollectionResetComposition(Uri absoluteUri, string collectionPropertyName, ModelChangeRecorder recorder, BubbledChangeEventArgs resetEvent)
            : base(absoluteUri, collectionPropertyName, recorder, resetEvent)
        {
            NewState = new Collection<T>();
        }

        //Use this only if Invert functionality is necessary
        public CollectionResetComposition(Uri absoluteUri, string collectionPropertyName, ModelChangeRecorder recorder, BubbledChangeEventArgs resetEvent, ICollection<T> newState)
            : base(absoluteUri, collectionPropertyName, recorder, resetEvent)
        {
            NewState = newState;
        }

        protected override ICollection<T> GetNewState(IModelRepository repository)
        {
            return NewState;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            var other = obj as CollectionResetComposition<T>;
            if (other == null)
                return false;
            else
                return this.AbsoluteUri.Equals(other.AbsoluteUri)
                       && this.CollectionPropertyName.Equals(other.CollectionPropertyName)
                       && this.NewState.SequenceEqual(other.NewState);
        }

        public override int GetHashCode()
        {
            return AbsoluteUri?.GetHashCode() ?? 0
                   ^ CollectionPropertyName?.GetHashCode() ?? 0
                   ^ NewState.GetHashCode();
        }
    }

    [XmlConstructor(2)]
    public class CollectionResetAssociation<T> : CollectionResetBase<T> where T : class, IModelElement
    {
        public ICollection<Uri> NewElementUris { get; set; }
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        public CollectionResetAssociation(Uri absoluteUri, string collectionPropertyName)
            : base(absoluteUri, collectionPropertyName)
        {
            NewElementUris = new Collection<Uri>();
        }
        
        public CollectionResetAssociation(Uri absoluteUri, string collectionPropertyName, ICollection<Uri> newElementUris)
            : base(absoluteUri, collectionPropertyName)
        {
            NewElementUris = newElementUris;
        }

        //Use this only if Invert functionality is necessary
        [EditorBrowsable(EditorBrowsableState.Never)]
        public CollectionResetAssociation(Uri absoluteUri, string collectionPropertyName, ModelChangeRecorder recorder, BubbledChangeEventArgs resetEvent)
            : base(absoluteUri, collectionPropertyName, recorder, resetEvent)
        {
            NewElementUris = new Collection<Uri>();
        }

        //Use this only if Invert functionality is necessary
        public CollectionResetAssociation(Uri absoluteUri, string collectionPropertyName, ModelChangeRecorder recorder, BubbledChangeEventArgs resetEvent, ICollection<Uri> newElementUris)
            : base(absoluteUri, collectionPropertyName, recorder, resetEvent)
        {
            NewElementUris = newElementUris;
        }

        protected override ICollection<T> GetNewState(IModelRepository repository)
        {
            return NewElementUris.Select(u => repository.Resolve(u)).Cast<T>().ToList();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            var other = obj as CollectionResetAssociation<T>;
            if (other == null)
                return false;
            else
                return this.AbsoluteUri.Equals(other.AbsoluteUri)
                    && this.CollectionPropertyName.Equals(other.CollectionPropertyName)
                    && this.NewElementUris.SequenceEqual(other.NewElementUris);
        }

        public override int GetHashCode()
        {
            return AbsoluteUri?.GetHashCode() ?? 0
                ^ CollectionPropertyName?.GetHashCode() ?? 0
                ^ NewElementUris.GetHashCode();
        }
    }
}
