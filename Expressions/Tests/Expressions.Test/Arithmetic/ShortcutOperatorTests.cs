using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NMF.Expressions.Test
{
    [TestClass]
    public class ShortcutOperatorTests
    {
        [TestMethod]
        public void OrElse_RightHandThrowsException_NoExceptionStatic()
        {
            var dummy = new Dummy<bool>(true);
            var test = Observable.Expression(() => dummy.Item || ThrowException());
            Assert.IsTrue(test.Value);
        }

        [TestMethod]
        public void OrElse_RightHandThrowsException_NoExceptionOnUpdate()
        {
            var dummy = new ObservableDummy<bool>(false);
            var test = Observable.Expression(() => dummy.Item || ThrowExceptionIf(dummy.Item));
            Assert.IsTrue(test.Value);

            dummy.Item = true;

            Assert.IsTrue(test.Value);
        }

        [TestMethod]
        public void AndAlso_RightHandThrowsException_NoExceptionStatic()
        {
            var dummy = new Dummy<bool>(false);
            var test = Observable.Expression(() => dummy.Item && ThrowException());
            Assert.IsFalse(test.Value);
        }

        [TestMethod]
        public void AndAlso_RightHandThrowsException_NoExceptionOnUpdate()
        {
            var dummy = new ObservableDummy<bool>(true);
            var test = Observable.Expression(() => dummy.Item && ThrowExceptionIf(!dummy.Item));
            Assert.IsTrue(test.Value);

            dummy.Item = false;

            Assert.IsFalse(test.Value);
        }

        private bool ThrowException()
        {
            throw new AssertFailedException("This exception should not have been thrown");
        }

        private bool ThrowExceptionIf(bool value)
        {
            if (value)
            {
                return ThrowException();
            }
            else
            {
                return true;
            }
        }
    }
}
