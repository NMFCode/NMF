using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models.Evolution
{
    public class ModelPropertyChange : IModelChange
    {
        public ModelPropertyChange(IModelElement element, string reference, object value)
        {
            if (element == null) throw new ArgumentNullException("element");
            if (reference == null) throw new ArgumentNullException("reference");

            Element = element;
            Reference = reference;
            NewValue = value;
        }

        public IModelElement Element { get; private set; }

        public string Reference { get; private set; }

        public object NewValue { get; private set; }

        public void Do()
        {
            var prop = Element.GetType().GetProperty(Reference);
            if (prop != null)
            {
                prop.SetValue(Element, NewValue, null);
            }
        }

        public virtual void Undo()
        {
            throw new NotSupportedException();
        }

        public virtual bool CanUndo
        {
            get { return false; }
        }
    }
}
