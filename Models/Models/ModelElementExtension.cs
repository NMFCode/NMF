using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models
{
    public abstract class ModelElementExtension : ModelElement
    {
        protected internal ModelElementExtension(IModelElement extended)
        {
            if (extended == null) throw new ArgumentNullException("extended");
        }
    }

    public abstract class ModelElementExtension<T> : ModelElementExtension where T : IModelElement
    {
        public ModelElementExtension(T parent)
            : base(parent)
        {
            ExtendedElement = parent;
        }

        public T ExtendedElement { get; private set; }

        public static implicit operator T(ModelElementExtension<T> extension)
        {
            if (extension == null) return default(T);
            return extension.ExtendedElement;
        }

        public override Meta.IClass GetClass()
        {
            return ExtendedElement.GetClass();
        }
    }
}
