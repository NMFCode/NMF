using NMF.Models;
using NMF.Models.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NMF.Controls.ContextMenu
{
    public class AddChildMenuItem : ICommand
    {
        private IReference containment;
        private System.Type newElementType;
        private IModelElement element;

        public AddChildMenuItem(IReference containment, System.Type newElementType, IModelElement element)
        {
            this.containment = containment;
            this.newElementType = newElementType;
            this.element = element;
        }

        public event EventHandler CanExecuteChanged { add { } remove { } }

        public bool CanExecute(object parameter)
        {
            return true;
        }

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
        }

        public override string ToString()
        {
            return $"Add {newElementType.Name} to {containment.Name}";
        }
    }
}
