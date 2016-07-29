using NMF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    internal class BubbledChangeFetcher
    {
        public IModelElement Element { get; set; }

        public Type Type { get; private set; }

        public BubbledChangeFetcher Child { get; private set; }

        public BubbledChangeFetcher Parent { get; private set; }

        public BubbledChangeFetcher(BubbledChangeFetcher parent)
        {
            Element = parent.Element.Parent;
            Parent = parent;
            Type = parent.Type;
        }

        protected BubbledChangeFetcher(IModelElement element, Type type)
        {
            Element = element;
            Type = type;
        }

        public void Attach()
        {
            if (Type.IsInstanceOfType(Element))
            {
                Element.BubbledChange += Element_BubbledChange;
            }
            else
            {
                RenewChild();
                Element.ParentChanged += Element_ParentChanged;
            }
        }

        private void RenewChild()
        {
            if (Element.Parent == null)
            {
                Child = null;
            }
            else if (Element.Parent is Model)
            {
                throw new NotImplementedException();
            }
            else
            {
                Child = new BubbledChangeFetcher(this);
                Child.Attach();
            }
        }

        private void Element_ParentChanged(object sender, ValueChangedEventArgs e)
        {
            if (Child != null)
            {
                Child.Detach();
            }
            RenewChild();
        }

        private void Element_BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            PropagateBubbledChange(e);
        }

        public virtual void PropagateBubbledChange(BubbledChangeEventArgs e)
        {
            Parent.PropagateBubbledChange(e);
        }

        public void Detach()
        {
            if (Type.IsInstanceOfType(Element))
            {
                Element.BubbledChange -= Element_BubbledChange;
            }
            else
            {
                if (Child != null)
                {
                    Child.Detach();
                }
                Element.ParentChanged -= Element_ParentChanged;
            }
        }
    }

    internal class BubbledChangeListener : BubbledChangeFetcher
    {
        public BubbledChangeListener(IModelElement element, Type type) : base(element, type)
        {
        }

        public override void PropagateBubbledChange(BubbledChangeEventArgs e)
        {
            BubbledChange?.Invoke(this, e);
        }

        public event EventHandler<BubbledChangeEventArgs> BubbledChange;
    }
}
