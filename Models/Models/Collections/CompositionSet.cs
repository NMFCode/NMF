using NMF.Collections.ObjectModel;
using NMF.Expressions;

namespace NMF.Models.Collections
{
    /// <summary>
    /// Denotes the base class for a composition implemented as a set
    /// </summary>
    /// <typeparam name="T">The element type</typeparam>
    public class CompositionSet<T> : OppositeSet<IModelElement, T> where T : class, IModelElement
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="parent">The parent model element</param>
        public CompositionSet(IModelElement parent) : base(parent)
        {
        }

        /// <inheritdoc />
        protected override void SetOpposite(T item, IModelElement newParent)
        {
            if (newParent == null)
            {
                item.ParentChanged -= RemoveItem;
                if (item.Parent == Parent) item.Delete();
            }
            else
            {
                item.ParentChanged += RemoveItem;
                item.Parent = Parent;
            }
        }

        private void RemoveItem(object sender, ValueChangedEventArgs e)
        {
            if (e.NewValue != Parent)
            {
                var item = sender as T;
                base.Remove(item);
            }
        }
    }

    /// <summary>
    /// Denotes the base class for a composition implemented as a set
    /// </summary>
    /// <typeparam name="T">The element type</typeparam>
    public class ObservableCompositionSet<T> : ObservableOppositeSet<IModelElement, T> where T : class, IModelElement
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="parent">The parent model element</param>
        public ObservableCompositionSet(IModelElement parent) : base(parent)
        {
        }

        /// <inheritdoc />
        protected override void SetOpposite(T item, IModelElement newParent)
        {
            if (newParent == null)
            {
                item.ParentChanged -= RemoveItem;
                if (item.Parent == Parent) item.Delete();
            }
            else
            {
                item.ParentChanged += RemoveItem;
                item.Parent = Parent;
            }
        }

        private void RemoveItem(object sender, ValueChangedEventArgs e)
        {
            if (e.NewValue != Parent)
            {
                var item = sender as T;
                base.Remove(item);
            }
        }
    }
}
