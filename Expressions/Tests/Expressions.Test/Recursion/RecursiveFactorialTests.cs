using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NMF.Expressions.Test.Recursion
{
    [TestClass]
    public class RecursiveFactorialTests
    {
        private ObservingFunc<int, int> fac;

        [TestInitialize]
        public void InitFactorial()
        {
            fac = Observable.Recurse<int, int>((rec, n) => n <= 1 ? 1 : n * rec(n - 1));
        }

        [TestMethod]
        public void Recursive_Factorial_Evaluate()
        {
            Assert.AreEqual(1, fac.Evaluate(0));
            Assert.AreEqual(1, fac.Evaluate(1));
            Assert.AreEqual(24, fac.Evaluate(4));
        }

        [TestMethod]
        public void Recursive_Factorial_ObserveDepth0()
        {
            var dummy = new ObservableDummy<int>(0);
            var test = Observable.Expression(() => fac.Evaluate(dummy.Item));
            var changed = false;

            test.ValueChanged += (o, e) =>
            {
                changed = true;
            };

            Assert.AreEqual(1, test.Value);
            Assert.IsFalse(changed);
            
            dummy.Item = 1;

            Assert.IsFalse(changed);
        }



        [TestMethod]
        public void Recursive_Factorial_ObserveDepth1()
        {
            var dummy = new ObservableDummy<int>(2);
            var test = Observable.Expression(() => fac.Evaluate(dummy.Item));
            var changed = false;

            test.ValueChanged += (o, e) =>
            {
                changed = true;
                Assert.AreEqual(2, e.OldValue);
                Assert.AreEqual(6, e.NewValue);
            };

            Assert.AreEqual(2, test.Value);
            Assert.IsFalse(changed);

            //test.Visualize();

            dummy.Item = 3;

            Assert.IsTrue(changed);
            Assert.AreEqual(6, test.Value);
        }

        [TestMethod]
        public void Recursive_Factorial_ObserveDepth2()
        {
            var dummy = new ObservableDummy<int>(2);
            var test = Observable.Expression(() => fac.Evaluate(dummy.Item));
            var changed = false;

            test.ValueChanged += (o, e) =>
            {
                changed = true;
                Assert.AreEqual(2, e.OldValue);
                Assert.AreEqual(24, e.NewValue);
            };

            Assert.AreEqual(2, test.Value);
            Assert.IsFalse(changed);

            dummy.Item = 4;

            Assert.IsTrue(changed);
            Assert.AreEqual(24, test.Value);
        }
    }
}
