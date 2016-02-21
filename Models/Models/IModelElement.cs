using NMF.Expressions;
using NMF.Models.Meta;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace NMF.Models
{
    /// <summary>
    /// Defines the interface of a model element in NMF
    /// </summary>
    public interface IModelElement : INotifyPropertyChanged
    {
        /// <summary>
        /// Deletes the current model element
        /// </summary>
        void Delete();

        /// <summary>
        /// This event is fired when the model element is deleted
        /// </summary>
        event EventHandler Deleted;

        /// <summary>
        /// Gets a value indicating whether this item can be identified through its ToString value
        /// </summary>
        bool IsIdentified { get; }


        /// <summary>
        /// Gets an identification string of the model element
        /// </summary>
        string ToIdentifierString();

        /// <summary>
        /// Gets the relative Uri of this model element
        /// </summary>
        Uri RelativeUri { get; }

        /// <summary>
        /// Gets the absolute Uri for this model element
        /// </summary>
        Uri AbsoluteUri { get; }

        /// <summary>
        /// Resolves the given relative Uri from the current model element
        /// </summary>
        /// <param name="relativeUri">A relative uri describing the path to the desired child element</param>
        /// <returns>The corresponding child element or null, if no such was found</returns>
        IModelElement Resolve(Uri relativeUri);

        /// <summary>
        /// Gets the model that contains the current model element
        /// </summary>
        Model Model { get; }

        /// <summary>
        /// Gets the container of the current model element
        /// </summary>
        IModelElement Parent { get; set; }

        /// <summary>
        /// Gets the children of the current model element
        /// </summary>
        [Constant]
        IEnumerableExpression<IModelElement> Children { get; }

        /// <summary>
        /// Gets the model elements referenced by the current model element
        /// </summary>
        [Constant]
        IEnumerableExpression<IModelElement> ReferencedElements { get; }

        /// <summary>
        /// Gets the extensions for the current model element
        /// </summary>
        ICollectionExpression<ModelElementExtension> Extensions { get; }


        /// <summary>
        /// Gets the extension of the given extension type
        /// </summary>
        /// <typeparam name="T">The extension type</typeparam>
        /// <returns>The extension instance for this model element or null, if the extension type was not applied to the current model element</returns>
        T GetExtension<T>() where T : ModelElementExtension;

        /// <summary>
        /// Gets the class of the model element
        /// </summary>
        /// <returns>The class of the model element</returns>
        IClass GetClass();


        object GetAttributeValue(IAttribute attribute, int index = 0);


        IList GetAttributeValues(IAttribute attribute);

        IModelElement GetReferencedElement(IReference reference, int index = 0);

        IList GetReferencedElements(IReference reference);

        event EventHandler<BubbledChangeEventArgs> BubbledChange;
    }
}
