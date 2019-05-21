using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Models
{
    public struct ModelTreeItem : IEquatable<ModelTreeItem>
    {
        public IModelElement Parent { get; }
        public IModelElement Child { get; }

        public ModelTreeItem(IModelElement parent, IModelElement child) : this()
        {
            Parent = parent;
            Child = child;
        }

        public override bool Equals(object obj)
        {
            if (obj is ModelTreeItem treeItem)
            {
                return Equals(treeItem);
            }
            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = 0;
            if (Parent != null) hashCode = Parent.GetHashCode() + 17;
            if (Child != null) hashCode = 23 * hashCode + Child.GetHashCode();
            return hashCode;
        }

        public bool Equals(ModelTreeItem other)
        {
            return Parent == other.Parent && Child == other.Child;
        }
    }
}
