using Models.Tests.Shared;
using NMF.Models;
using NUnit.Framework;
using System;

namespace Models.Tests.NUnit
{
    /// <summary>
    /// Performs a test series for the given concrete model element type
    /// </summary>
    /// <typeparam name="T">The type of model elements</typeparam>
    public class ModelTest<T> : ModelTestBase<T> where T : IModelElement, new()
    {
        /// <inheritdoc />
        protected override void AssertAreEqual(object expected, object actual, string message = null)
        {
            Assert.AreEqual(expected, actual, message);
        }

        /// <inheritdoc />
        protected override void AssertIsFalse(bool condition, string message = null)
        {
            Assert.IsFalse(condition, message);
        }

        /// <inheritdoc />
        protected override void AssertIsTrue(bool condition, string message = null)
        {
            Assert.IsTrue(condition, message);
        }

        /// <inheritdoc />
        protected override void AssertThrowsException<TException>(Action toPerform, string message = null)
        {
            Assert.Throws<TException>(() => toPerform(), message);
        }

        /// <inheritdoc />
        [Test]
#pragma warning disable S2699 // Tests should include assertions
        public override void AttributeChange_StateFixed_ThrowsException()
        {
            base.AttributeChange_StateFixed_ThrowsException();
        }

        /// <inheritdoc />
        [Test]
        public override void AttributeChange_StateLockedNoUnlock_ThrowsException()
        {
            base.AttributeChange_StateLockedNoUnlock_ThrowsException();
        }

        /// <inheritdoc />
        [Test]
        public override void AttributeChange_StateLocked_RequestsUnlockAndSucceeds()
        {
            base.AttributeChange_StateLocked_RequestsUnlockAndSucceeds();
        }

        /// <inheritdoc />
        [Test]
        public override void AttributeChange_StateNormal_SucceedsAndFiresChangeEvents()
        {
            base.AttributeChange_StateNormal_SucceedsAndFiresChangeEvents();
        }

        /// <inheritdoc />
        [Test]
        public override void ReferenceChange_StateFixed_ThrowsException()
        {
            base.ReferenceChange_StateFixed_ThrowsException();
        }

        /// <inheritdoc />
        [Test]
        public override void ReferenceChange_StateLockedNoUnlock_ThrowsException()
        {
            base.ReferenceChange_StateLockedNoUnlock_ThrowsException();
        }

        /// <inheritdoc />
        [Test]
        public override void ReferenceChange_StateLocked_RequestsUnlockAndSucceeds()
        {
            base.ReferenceChange_StateLocked_RequestsUnlockAndSucceeds();
        }

        /// <inheritdoc />
        [Test]
        public override void ReferenceChange_StateNormal_SucceedsAndFiresChangeEvents()
        {
            base.ReferenceChange_StateNormal_SucceedsAndFiresChangeEvents();
        }

        /// <inheritdoc />
        [Test]
        public override void ResolveOfRelativeUri_SucceedsAndFindsCorrectModelElement()
        {
            base.ResolveOfRelativeUri_SucceedsAndFindsCorrectModelElement();
        }
#pragma warning restore S2699 // Tests should include assertions
    }
}
