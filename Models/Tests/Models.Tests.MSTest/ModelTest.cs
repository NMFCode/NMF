using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Tests.Shared;
using NMF.Collections.ObjectModel;
using NMF.Models;
using System;
using System.Collections.Specialized;
using System.Linq;

namespace Models.Tests.MSTest
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
            Assert.ThrowsException<TException>(toPerform, message ?? $"should have thrown a {typeof(TException).Name}");
        }

        [TestMethod]
        public override void ResolveOfRelativeUri_SucceedsAndFindsCorrectModelElement()
        {
            base.ResolveOfRelativeUri_SucceedsAndFindsCorrectModelElement();
        }

        [TestMethod]
        public override void AttributeChange_StateFixed_ThrowsException()
        {
            base.AttributeChange_StateFixed_ThrowsException();
        }

        [TestMethod]
        public override void AttributeChange_StateLockedNoUnlock_ThrowsException()
        {
            base.AttributeChange_StateLockedNoUnlock_ThrowsException();
        }

        [TestMethod]
        public override void AttributeChange_StateLocked_RequestsUnlockAndSucceeds()
        {
            base.AttributeChange_StateLocked_RequestsUnlockAndSucceeds();
        }

        [TestMethod]
        public override void AttributeChange_StateNormal_SucceedsAndFiresChangeEvents()
        {
            base.AttributeChange_StateNormal_SucceedsAndFiresChangeEvents();
        }

        [TestMethod]
        public override void ReferenceChange_StateFixed_ThrowsException()
        {
            base.ReferenceChange_StateFixed_ThrowsException();
        }

        [TestMethod]
        public override void ReferenceChange_StateLockedNoUnlock_ThrowsException()
        {
            base.ReferenceChange_StateLockedNoUnlock_ThrowsException();
        }

        [TestMethod]
        public override void ReferenceChange_StateNormal_SucceedsAndFiresChangeEvents()
        {
            base.ReferenceChange_StateNormal_SucceedsAndFiresChangeEvents();
        }

        [TestMethod]
        public override void ReferenceChange_StateLocked_RequestsUnlockAndSucceeds()
        {
            base.ReferenceChange_StateLocked_RequestsUnlockAndSucceeds();
        }
    }
}
