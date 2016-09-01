using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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
            new BenchmarkSwitcher(Assembly.GetExecutingAssembly()).Run();
#else
            Profile();
#endif
        }

        private static void Profile()
        {
            var bench = new SwitchSet();
            var watch = Stopwatch.StartNew();
            for (int i = 0; ; i++)
            {
                bench.Immediate();
                if ((i & 0xFF) == 0 && watch.ElapsedMilliseconds > 5000)
                    break;
            }
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
