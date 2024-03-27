using NMF.Models.Meta;

namespace NMF.Models
{
    /// <summary>
    /// Denotes the abstract base class for an extension (aka stereotype)
    /// </summary>
    [ModelRepresentationClassAttribute("http://nmf.codeplex.com/nmeta/#//ModelElementExtension/")]
    public abstract class ModelElementExtension : ModelElement, IModelElementExtension
    {
        /// <summary>
        /// Gets the actual extension
        /// </summary>
        /// <returns></returns>
        public abstract IExtension GetExtension();

        IModelElement IModelElementExtension.ExtendedElement
        {
            get { return Parent; }
            set { /* intentionally left blank */ }
        }

        /// <inheritdoc />
        public override Meta.IClass GetClass()
        {
            return Parent.GetClass();
        }
    }

    /// <summary>
    /// Denotes an abstract base class for a typed model extension
    /// </summary>
    /// <typeparam name="T">The type of the extended element</typeparam>
    /// <typeparam name="T2">The type of the extension</typeparam>
    public abstract class ModelElementExtension<T, T2> : ModelElementExtension where T : IModelElement where T2 : ModelElementExtension<T, T2>
    {
        /// <summary>
        /// Converts the extension to the extended element
        /// </summary>
        /// <param name="extension">the extension</param>
        public static implicit operator T(ModelElementExtension<T, T2> extension)
        {
            if (extension == null) return default(T);
            return (T)extension.Parent;
        }

        /// <summary>
        /// Converts the extended element to the extension
        /// </summary>
        /// <param name="element">the extended element</param>
        public static implicit operator ModelElementExtension<T, T2>(T element)
        {
            var me = element as ModelElement;
            return me?.GetExtension<ModelElementExtension<T, T2>>();
        }
    }
}
