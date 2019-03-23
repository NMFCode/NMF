using Models.Tests.Shared;
using NMF.Models;
using NUnit.Framework;
using System;

namespace Models.Tests.NUnit
{
    public class ModelTest<T> : ModelTestBase<T> where T : IModelElement, new()
    {
        protected override void AssertAreEqual(object expected, object actual, string message = null)
        {
            Assert.AreEqual(expected, actual, message);
        }

        protected override void AssertIsFalse(bool condition, string message = null)
        {
            Assert.IsFalse(condition, message);
        }

        protected override void AssertIsTrue(bool condition, string message = null)
        {
            Assert.IsTrue(condition, message);
        }

        protected override void AssertThrowsException<TException>(Action toPerform, string message = null)
        {
            Assert.Throws<TException>(() => toPerform(), message);
        }

        [Test]
        public override void AttributeChange_StateFixed_ThrowsException()
        {
            base.AttributeChange_StateFixed_ThrowsException();
        }

        [Test]
        public override void AttributeChange_StateLockedNoUnlock_ThrowsException()
        {
            base.AttributeChange_StateLockedNoUnlock_ThrowsException();
        }

        [Test]
        public override void AttributeChange_StateLocked_RequestsUnlockAndSucceeds()
        {
            base.AttributeChange_StateLocked_RequestsUnlockAndSucceeds();
        }

        [Test]
        public override void AttributeChange_StateNormal_SucceedsAndFiresChangeEvents()
        {
            base.AttributeChange_StateNormal_SucceedsAndFiresChangeEvents();
        }

        [Test]
        public override void ReferenceChange_StateFixed_ThrowsException()
        {
            base.ReferenceChange_StateFixed_ThrowsException();
        }

        [Test]
        public override void ReferenceChange_StateLockedNoUnlock_ThrowsException()
        {
            base.ReferenceChange_StateLockedNoUnlock_ThrowsException();
        }

        [Test]
        public override void ReferenceChange_StateLocked_RequestsUnlockAndSucceeds()
        {
            base.ReferenceChange_StateLocked_RequestsUnlockAndSucceeds();
        }

        [Test]
        public override void ReferenceChange_StateNormal_SucceedsAndFiresChangeEvents()
        {
            base.ReferenceChange_StateNormal_SucceedsAndFiresChangeEvents();
        }

        [Test]
        public override void ResolveOfRelativeUri_SucceedsAndFindsCorrectModelElement()
        {
            base.ResolveOfRelativeUri_SucceedsAndFindsCorrectModelElement();
        }
    }
}
