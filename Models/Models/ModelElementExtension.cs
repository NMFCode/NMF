using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMF.Expressions;
using NMF.Models.Meta;

namespace NMF.Models
{

    [ModelRepresentationClassAttribute("http://nmf.codeplex.com/nmeta/#//ModelElementExtension/")]
    public abstract class ModelElementExtension : ModelElement, IModelElementExtension
    {
        protected internal ModelElementExtension(IModelElement extended)
        {
            if (extended == null) throw new ArgumentNullException("extended");
        }

        protected abstract IModelElement ExtendedElementInternal
        {
            get;
        }

        event EventHandler<ValueChangedEventArgs> Meta.IModelElementExtension.ExtendedElementChanged
        {
            add { }
            remove { }
        }

        event EventHandler<ValueChangedEventArgs> Meta.IModelElementExtension.ExtendedElementChanging
        {
            add { }
            remove { }
        }

        public abstract IExtension GetExtension();

        IModelElement Meta.IModelElementExtension.ExtendedElement
        {
            get { return ExtendedElementInternal; }
            set { }
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

        protected override IModelElement ExtendedElementInternal
        {
            get
            {
                return ExtendedElement;
            }
        }
    }
}
