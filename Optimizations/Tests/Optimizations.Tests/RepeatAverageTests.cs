using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NMF.Optimizations.Tests
{
    [TestClass]
    public class RepeatAverageTests
    {
        private MockedBenchmark inner;

        [TestInitialize]
        public void LoadInnerBenchmark()
        {
            inner = new MockedBenchmark(
                time: n => 10.0 * n + 10,
                memory: n => 20.0 * n + 30);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Repeat_Null_Average()
        {
            var repeated = new RepeatAverageBenchmark<string>(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Repeat_0_Average()
        {
            var repeated = new RepeatAverageBenchmark<string>(inner, 0);
        }

        [TestMethod]
        public void Repeat_1_Average()
        {
            var repeated = new RepeatAverageBenchmark<string>(inner, 1);

            var result = repeated.MeasureConfiguration("Foo");
            Assert.AreEqual(10.0, result["Time"]);
            Assert.AreEqual(30.0, result["Memory"]);
        }

        [TestMethod]
        public void Repeat_10_Average()
        {
            var repeated = new RepeatAverageBenchmark<string>(inner, 10);

            var result = repeated.MeasureConfiguration("Bar");
            Assert.AreEqual(55.0, result["Time"]);
            Assert.AreEqual(120.0, result["Memory"]);
        }
    }
}
