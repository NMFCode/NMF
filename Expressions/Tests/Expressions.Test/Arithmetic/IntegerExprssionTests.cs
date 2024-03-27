using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NMF.Expressions.Test
{
    [TestClass]
    public class IntegerExpressionTests
    {

        [TestMethod]
        public void BitwiseXor_NoObservableSource_NoUpdate()
        {
            var update = false;
            var dummy = new Dummy<int>(7);

            var test = Observable.Expression(() => 21 ^ dummy.Item);

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(18, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 13;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void BitwiseXor_ObservableSource_Update()
        {
            var update = false;
            var dummy = new ObservableDummy<int>(7);

            var test = Observable.Expression(() => 21 ^ dummy.Item);

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(18, e.OldValue);
                Assert.AreEqual(24, e.NewValue);
            };

            Assert.AreEqual(18, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 13;

            Assert.IsTrue(update);
            Assert.AreEqual(24, test.Value);
        }
    }
}
