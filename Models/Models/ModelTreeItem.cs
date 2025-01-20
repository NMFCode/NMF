using System;

namespace NMF.Models
{
    /// <summary>
    /// Denotes a pair of a model element and its parent
    /// </summary>
    public readonly struct ModelTreeItem : IEquatable<ModelTreeItem>
    {
        /// <summary>
        /// Gets the parent model element
        /// </summary>
        public IModelElement Parent { get; }

        /// <summary>
        /// Gets the child model element
        /// </summary>
        public IModelElement Child { get; }

        /// <summary>
        /// Creates a new pair
        /// </summary>
        /// <param name="parent">the parent model element</param>
        /// <param name="child">the child model element</param>
        public ModelTreeItem(IModelElement parent, IModelElement child) : this()
        {
            Parent = parent;
            Child = child;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is ModelTreeItem treeItem)
            {
                return Equals(treeItem);
            }
            return false;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var hashCode = 0;
            if (Parent != null) hashCode = Parent.GetHashCode() + 17;
            if (Child != null) hashCode = 23 * hashCode + Child.GetHashCode();
            return hashCode;
        }

        /// <inheritdoc />
        public bool Equals(ModelTreeItem other)
        {
            return Parent == other.Parent && Child == other.Child;
        }

        /// <inheritdoc />
        public static bool operator ==(ModelTreeItem left, ModelTreeItem right)
        {
            return left.Equals(right);
        }

        /// <inheritdoc />
        public static bool operator !=(ModelTreeItem left, ModelTreeItem right)
        {
            return !(left == right);
        }
    }
}
