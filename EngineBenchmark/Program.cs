using BenchmarkDotNet.Running;
using NMF.Expressions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EngineBenchmark
{
    class Program
    {
        static void Main(string[] args)
        {
#if !DEBUG
            BenchmarkRunner.Run<SequentialLambda>();
#else
            Profile();
#endif
        }

        private static void Profile()
        {
            var bench = new SequentialLambda();
            for (int i=0; i<1000000; i++)
                bench.Immediate();
        }
    }

    public class Dummy<T> : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private T item;
        public T Item
        {
            get
            {
                return item;
            }
            set
            {
                if (!item.Equals(value))
                {
                    item = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("Item"));
                }
            }
        }

        public Dummy(T item)
        {
            this.item = item;
        }
    }
}
