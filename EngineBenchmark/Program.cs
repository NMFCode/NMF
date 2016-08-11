using NMF.Expressions;
using NMF.Expressions.Execution;
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
            const int Repeats = 1000000;

            Benchmark(false, false, 1);
            Benchmark(false, true, Repeats);

            Benchmark(true, false, 1);
            Benchmark(true, true, Repeats);
        }

        public static void Benchmark(bool useTransaction, bool write, int repeats)
        {
            var dummy = new Dummy<int>(42);

            var watch = Stopwatch.StartNew();
            var test = Observable.Expression(() => dummy.Item == dummy.Item && (AppDomain.CurrentDomain.IsFullyTrusted && dummy.Item == 42));
            watch.Stop();

            if (write)
                Console.WriteLine((useTransaction ? "Transaction" : "Immediate") + " initialize: " + watch.Elapsed.TotalMilliseconds + "ms");

            watch.Restart();
            for (int i = 0; i < repeats; i++)
            {
                if (useTransaction)
                    ExecutionEngine.Current.BeginTransaction();

                dummy.Item = 42 - dummy.Item;

                if (useTransaction)
                    ExecutionEngine.Current.CommitTransaction();

                if (test.Value == ((i & 1) == 0))
                    Console.WriteLine("Wrong result!");
            }
            watch.Stop();
            test.Successors.Clear();
            if (write)
                Console.WriteLine((useTransaction ? "Transaction" : "Immediate") + " increment: " + watch.Elapsed.TotalMilliseconds / repeats + "ms");
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
