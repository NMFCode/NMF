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
        const int Repeats = 1000000;

        static void Main(string[] args)
        {
            Immediate(false);
            Immediate(true);
            OnDemand(false);
            OnDemand(true);
        }

        public static void OnDemand(bool write)
        {
            var engine = new OnDemandExecutionEngine();
            ExecutionEngine.Current = engine;
            var dummy = new Dummy<int>(42);

            var watch = Stopwatch.StartNew();
            Expression<Func<bool>> expression = () => dummy.Item == dummy.Item && (AppDomain.CurrentDomain.IsFullyTrusted && dummy.Item == 42);
            var test = NotifySystem.CreateExpression<bool>(expression.Body, null);
            test.Successors.Add(null);
            watch.Stop();
            if (write)
                Console.WriteLine("On Demand initialize: " + watch.Elapsed.TotalMilliseconds + "ms");

            watch.Restart();
            for (int i = 0; i < Repeats; i++)
            {
                dummy.Item = 42 - dummy.Item;
                engine.Execute();
                if (test.Value == ((i & 1) == 0))
                    Console.WriteLine("Wrong result!");
            }
            watch.Stop();
            if (write)
                Console.WriteLine("On Demand increment: " + watch.Elapsed.TotalMilliseconds / Repeats + "ms");
        }
        
        public static void Immediate(bool write)
        {
            ExecutionEngine.Current = new ImmediateExecutionEngine();
            var dummy = new Dummy<int>(42);

            var watch = Stopwatch.StartNew();
            Expression<Func<bool>> expression = () => dummy.Item == dummy.Item && (AppDomain.CurrentDomain.IsFullyTrusted && dummy.Item == 42);
            var test = NotifySystem.CreateExpression<bool>(expression.Body, null);
            test.Successors.Add(null);
            watch.Stop();
            if (write)
                Console.WriteLine("Immediate initialize: " + watch.Elapsed.TotalMilliseconds + "ms");

            watch.Restart();
            for (int i = 0; i < Repeats; i++)
            {
                dummy.Item = 42 - dummy.Item;
                if (test.Value == ((i & 1) == 0))
                    Console.WriteLine("Wrong result!");
            }
            watch.Stop();
            if (write)
                Console.WriteLine("Immediate increment: " + watch.Elapsed.TotalMilliseconds / Repeats + "ms");
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
