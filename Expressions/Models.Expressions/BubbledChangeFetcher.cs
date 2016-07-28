using NMF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    internal class BubbledChangeFetcher<T>
    {
        public IModelElement Element { get; private set; }

        public BubbledChangeFetcher<T> Child { get; private set; }

        public BubbledChangeFetcher<T> Parent { get; private set; }

        public BubbledChangeFetcher(BubbledChangeFetcher<T> parent)
        {
            Element = parent.Element.Parent;
            Parent = parent;
        }

        protected BubbledChangeFetcher(IModelElement element)
        {
            Element = element;
        }

        public void Attach()
        {
            if (Element is T)
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
                Child = new BubbledChangeFetcher<T>(this);
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
            if (Element is T)
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

    internal class BubbledChangeListener<T> : BubbledChangeFetcher<T>
    {
        public BubbledChangeListener(IModelElement element) : base(element)
        {
        }

        public override void PropagateBubbledChange(BubbledChangeEventArgs e)
        {
            BubbledChange?.Invoke(this, e);
        }

        public event EventHandler<BubbledChangeEventArgs> BubbledChange;
    }
}
