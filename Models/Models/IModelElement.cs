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
    [ModelRepresentationClass("http://nmf.codeplex.com/nmeta/#//ModelElement/")]
    public interface IModelElement : INotifyPropertyChanged, INotifyPropertyChanging
    {
        /// <summary>
        /// Deletes the current model element
        /// </summary>
        void Delete();

        /// <summary>
        /// This event is fired after the model element is deleted
        /// </summary>
        event EventHandler<UriChangedEventArgs> Deleted;

        /// <summary>
        /// This event is fired before the model element is deleted
        /// </summary>
        event EventHandler<UriChangedEventArgs> Deleting;

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
        /// Gets the container reference for the given child element
        /// </summary>
        /// <param name="child">The child element</param>
        /// <param name="index">The index of the child in the returned reference</param>
        /// <returns>A composition reference or null, if the child is not contained in the model element</returns>
        IReference GetContainerReference(IModelElement child, out int index);

        /// <summary>
        /// Gets the model that contains the current model element
        /// </summary>
        Model Model { get; }

        /// <summary>
        /// Gets the container of the current model element
        /// </summary>
        IModelElement Parent { get; set; }

        /// <summary>
        /// Gets fired when the container of the current model element has changed
        /// </summary>
        event EventHandler<ValueChangedEventArgs> ParentChanged;

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

        /// <summary>
        /// Gets the value for the given attribute and index
        /// </summary>
        /// <param name="attribute">The attribute</param>
        /// <param name="index">The index within the attribute</param>
        /// <returns>The value for the given attribute. If this is a collection and the index parameter is specified, the method returns the value at the given index</returns>
        object GetAttributeValue(IAttribute attribute, int index = 0);

        /// <summary>
        /// Gets the values of the given attribute as a list
        /// </summary>
        /// <param name="attribute">The attribute</param>
        /// <returns>A non-generic list of values</returns>
        IList GetAttributeValues(IAttribute attribute);

        /// <summary>
        /// Calls the given operation
        /// </summary>
        /// <param name="operation">The operation that should be called</param>
        /// <param name="arguments">The arguments used to call the operation</param>
        /// <returns>The operation result or null, if the operation does not return any value</returns>
        object CallOperation(IOperation operation, params object[] arguments);

        /// <summary>
        /// Gets the referenced model element for the given reference and index
        /// </summary>
        /// <param name="reference">The reference</param>
        /// <param name="index">The index within the reference</param>
        /// <returns>The value for the given reference. If this is a collection and the index parameter is specified, the method returns the referenced element at the given index</returns>
        IModelElement GetReferencedElement(IReference reference, int index = 0);

        /// <summary>
        /// Sets the referenced element of the current model element for the given reference
        /// </summary>
        /// <param name="reference">The reference</param>
        /// <param name="element">The element that should be set</param>
        void SetReferencedElement(IReference reference, IModelElement element);

        /// <summary>
        /// Gets the referen
        /// </summary>
        /// <param name="reference"></param>
        /// <returns></returns>
        IList GetReferencedElements(IReference reference);

        /// <summary>
        /// Sets the value of the current model element for the given attribute
        /// </summary>
        /// <param name="attribute">The attribute</param>
        /// <param name="value">The value that should be set</param>
        void SetAttributeValue(IAttribute attribute, object value);

        /// <summary>
        /// Freezes this model element such that it becomes immutable.
        /// </summary>
        void Freeze();

        /// <summary>
        /// Locks this model element against any changes (can be undone)
        /// </summary>
        void Lock();

        /// <summary>
        /// Unlocks this model element.
        /// </summary>
        /// <exception cref="LockedException">thrown if the model element could not be unlocked</exception>
        void Unlock();

        /// <summary>
        /// Tries to unlock the current model element in order to make changes possible
        /// </summary>
        /// <returns>True, if unlocking the model element succeeds, otherwise False</returns>
        bool TryUnlock();

        /// <summary>
        /// Determines whether the model elements and all elements underneath are frozen
        /// </summary>
        bool IsFrozen { get; }

        /// <summary>
        /// Determines whether the model elements and all elements underneath are locked
        /// </summary>
        bool IsLocked { get; }

        /// <summary>
        /// Gets fired when an elementary change happens in the composition hierarchy rooted at the current element. The original elementary change can be retrieved in the event data
        /// </summary>
        event EventHandler<BubbledChangeEventArgs> BubbledChange;
    }
}
