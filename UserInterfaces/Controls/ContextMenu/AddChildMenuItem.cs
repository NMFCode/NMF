using NMF.Models;
using NMF.Models.Meta;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace NMF.Controls.ContextMenu
{
    /// <summary>
    /// Denotes a command to add a child
    /// </summary>
    public class AddChildMenuItem : ICommand
    {
        private readonly IReference containment;
        private readonly System.Type newElementType;
        private readonly IModelElement element;

        /// <summary>
        /// Creates a new command
        /// </summary>
        /// <param name="containment">The containment reference on which to add a child</param>
        /// <param name="newElementType">The type of elements to be added</param>
        /// <param name="element">The element to which new children should be added</param>
        public AddChildMenuItem(IReference containment, System.Type newElementType, IModelElement element)
        {
            this.containment = containment;
            this.newElementType = newElementType;
            this.element = element;
        }

        /// <inheritdoc />
        public event EventHandler CanExecuteChanged { add { } remove { } }

        /// <inheritdoc />
        public bool CanExecute(object parameter)
        {
            return !element.IsFrozen;
        }

        /// <inheritdoc />
        public void Execute(object parameter)
        {
            var newElement = Activator.CreateInstance(newElementType);
            if (containment.UpperBound == 1)
            {
                element.SetReferencedElement(containment, (IModelElement)newElement);
            }
            else
            {
                var container = element.GetReferencedElements(containment);
                container.Add(newElement);
            }
            if (parameter is TreeViewItem treeView)
            {
                treeView.ExpandSubtree();
                if (treeView.ItemContainerGenerator.ContainerFromItem(newElement) is TreeViewItem treeItem) treeItem.Focus();
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"Add {newElementType.Name} to {containment.Name}";
        }
    }
}
