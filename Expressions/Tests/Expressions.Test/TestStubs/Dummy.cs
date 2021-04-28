using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Test
{
    public class Dummy<T>
    {
        public Dummy() { }

        public Dummy(T a) { Item = a; }

        public virtual T Item { get; set; }

        public static implicit operator Dummy<T>(T item)
        {
            return new Dummy<T>(item);
        }

        public override string ToString()
        {
            return string.Format("Dummy({0})", Item != null ? Item.ToString() : "(null)");
        }
    }

    public class ObservableDummy<T> : Dummy<T>, INotifyPropertyChanged
    {
        public ObservableDummy() { }

        public ObservableDummy(T item) { Item = item; }

        #region Item

        public override T Item
        {
            get
            {
                return base.Item;
            }
            set
            {
                if (!object.Equals(base.Item, value))
                {
                    base.Item = value;
                    OnItemChanged(EventArgs.Empty);
                }
            }
        }

        protected virtual void OnItemChanged(EventArgs e)
        {
            ItemChanged?.Invoke(this, e);
            OnPropertyChanged("Item");
        }

        public event EventHandler ItemChanged;


        #endregion

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
