using NMF.Models;
using NMF.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    internal interface IBubbledChangeFetcher
    {
        void PropagateBubbledChange(BubbledChangeEventArgs e);

        void Attach();

        void Detach();
    }

    internal class BubbledChangeFetcher : IBubbledChangeFetcher
    {
        public IModelElement Element { get; set; }

        public Type Type { get; private set; }

        public IBubbledChangeFetcher Child { get; private set; }

        public IBubbledChangeFetcher Parent { get; private set; }

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
            if (Element == null) return;
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

        protected virtual void RenewChild()
        {
            if (Element is Model model && model.Repository != null)
            {
                Child = new BubbledChangeRepositoryFetcher(model.Repository, this);
                Child.Attach();
            }
            else if (Element.Parent == null)
            {
                Child = null;
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
            if (Element == null) return;
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

    internal class BubbledChangeRepositoryFetcher : IBubbledChangeFetcher
    {
        public IModelRepository Repository { get; private set; }

        public IBubbledChangeFetcher Parent { get; private set; }

        public BubbledChangeRepositoryFetcher(IModelRepository repository, IBubbledChangeFetcher parent)
        {
            Repository = repository;
            Parent = parent;
        }

        public void Attach()
        {
            Repository.BubbledChange += Repository_BubbledChange;
        }

        private void Repository_BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            Parent.PropagateBubbledChange(e);
        }

        public void Detach()
        {
            Repository.BubbledChange -= Repository_BubbledChange;
        }

        public void PropagateBubbledChange(BubbledChangeEventArgs e)
        {
            Parent.PropagateBubbledChange(e);
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
