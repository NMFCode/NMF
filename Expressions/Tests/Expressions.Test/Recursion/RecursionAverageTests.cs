using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Expressions.Test.Recursion
{
    [TestClass]
    public class RecursionAverageTests
    {
        private Root root;

        private int Init(int size)
        {
            var sum = 23;
            root = new Root
            {
                Head = new Item
                {
                    Value = sum
                }
            };
            root.Tail = root.Head;
            var random = new Random();
            for (int i = 1; i < size; i++)
            {
                var item = new Item { Value = random.Next() };
                sum += item.Value;
                root.Tail.Next = item;
                root.Tail = item;
            }
            return sum;
        }

        private INotifyValue<double> CreateTest()
        {
            var computeAverage = Observable.Func(((int, int) tuple) => ((double)tuple.Item1) / tuple.Item2);
            var recurse = Observable.Recurse<Item, (int, int), (int, int)>((rec, item, tuple) => item == null ? tuple : Add(rec(item.Next, tuple), item.Value));
            return Observable.Expression(() => computeAverage.Evaluate(recurse.Evaluate(root.Head, ValueTuple.Create(0, 0))));
        }

        [TestMethod]
        public void Recursion_Depth1_Works()
        {
            var sum = Init(1);
            var test = CreateTest();
            var changed = false;
            test.ValueChanged += (o, e) =>
            {
                changed = true;
            };

            Assert.AreEqual((double)sum, test.Value);
            Assert.IsFalse(changed);

            root.Head.Value = 42;

            Assert.IsTrue(changed);
            Assert.AreEqual(42.0, test.Value);
        }

        private static (int, int) Add((int, int) before, int value)
        {
            return (before.Item1 + value, before.Item2 + 1);
        }
    }
}
