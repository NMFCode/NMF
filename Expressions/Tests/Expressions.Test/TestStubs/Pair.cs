using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace NMF.Expressions.Test
{
    public class Pair<T1, T2>
    {
        public Pair() { }

        public Pair(T1 item1, T2 item2)
        {
            Item1 = item1;
            Item2 = item2;
        }

        public virtual T1 Item1 { get; set; }

        public virtual T2 Item2 { get; set; }
    }

    public class ObservablePair<T1, T2> : Pair<T1, T2>, INotifyPropertyChanged
    {
        public ObservablePair() { }

        public ObservablePair(T1 item1, T2 item2) : base(item1, item2) { }

        #region Item1

        public override T1 Item1
        {
            get
            {
                return base.Item1;
            }
            set
            {
                if (!EqualityComparer<T1>.Default.Equals(base.Item1, value))
                {
                    base.Item1 = value;
                    OnItem1Changed(EventArgs.Empty);
                }
            }
        }

        protected virtual void OnItem1Changed(EventArgs e)
        {
            Item1Changed?.Invoke(this, e);
            OnPropertyChanged(nameof(Item1));
        }

        public event EventHandler Item1Changed;

        #endregion

        #region Item2

        public override T2 Item2
        {
            get
            {
                return base.Item2;
            }
            set
            {
                if (!EqualityComparer<T2>.Default.Equals(base.Item2, value))
                {
                    base.Item2 = value;
                    OnItem2Changed(EventArgs.Empty);
                }
            }
        }

        protected virtual void OnItem2Changed(EventArgs e)
        {
            Item2Changed?.Invoke(this, e);
            OnPropertyChanged(nameof(Item2));
        }

        public event EventHandler Item2Changed;

        #endregion



        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
