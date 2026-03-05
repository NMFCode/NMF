using NMF.AnyText.Transformation;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnyText.Tests.CodeGeneration
{
    [TestFixture]
    public class RegexHelperTests
    {
        [Test]
        public void TransformRegex_NoChanges()
        {
            Assert.That(RegexHelper.TransformRegex(@"\w+", ""), Is.EqualTo(@"^\w+"));
        }

        [Test]
        public void TransformRegex_MarksBeginningOfChoice()
        {
            Assert.That(RegexHelper.TransformRegex(@"\w+|\d+", ""), Is.EqualTo(@"^\w+|^\d+"));
        }

        [Test]
        public void TransformRegex_UsesSurroundCharacters()
        {
            Assert.That(RegexHelper.TransformRegex(@"\w+", "\""), Is.EqualTo(@"^""\w+"""));
        }

        [Test]
        public void TransformRegex_DoesNotMarkBeginningOfChoiceInGroup()
        {
            Assert.That(RegexHelper.TransformRegex(@"a(\w+|\d+)", ""), Is.EqualTo(@"^a(\w+|\d+)"));
        }
    }
}
