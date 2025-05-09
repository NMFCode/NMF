using NMF.AnyText;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyText.Tests
{
    [TestFixture]
    public class TextEditTests
    {
        [Test]
        public void TextEdit_ApplyInline_LastLineInsert()
        {
            var input = new[]
            {
                "This",
                "is",
                "some",
                "text"
            };
            var edit = new TextEdit(new ParsePosition(3, 0), new ParsePosition(3, 0), new[] { "new " });
            var result = edit.Apply(input);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(input));
            Assert.That(result[3], Is.EqualTo("new text"));
        }
        [Test]
        public void TextEdit_ApplyInline_LastLineDelete()
        {
            var input = new[]
            {
                "This",
                "is",
                "some",
                "text"
            };
            var edit = new TextEdit(new ParsePosition(3, 0), new ParsePosition(3, 4), new[] { string.Empty });
            var result = edit.Apply(input);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(input));
            Assert.That(result[3], Is.EqualTo(string.Empty));
        }

        [Test]
        public void TextEdit_ApplyInline_LastLineAdd()
        {
            var input = new[]
            {
                "This",
                "is",
                "some",
                string.Empty
            };
            var edit = new TextEdit(new ParsePosition(3, 0), new ParsePosition(3, 0), new[] { "text" });
            var result = edit.Apply(input);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(input));
            Assert.That(result[3], Is.EqualTo("text"));
        }

        [Test]
        public void TextEdit_ApplyInline_MiddleAdd()
        {
            var input = new[]
            {
                "This",
                "is",
                "some",
                "text"
            };
            var edit = new TextEdit(new ParsePosition(2, 4), new ParsePosition(2, 4), new[] { " other" });
            var result = edit.Apply(input);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(input));
            Assert.That(result[2], Is.EqualTo("some other"));
        }

        [Test]
        public void TextEdit_ApplyInline_MiddleInsert()
        {
            var input = new[]
            {
                "This",
                "is",
                "some",
                "text"
            };
            var edit = new TextEdit(new ParsePosition(1, 1), new ParsePosition(1, 1), new[] { "s thi" });
            var result = edit.Apply(input);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(input));
            Assert.That(result[1], Is.EqualTo("is this"));
        }

        [Test]
        public void TextEdit_ApplyReconstruct_InsertNewLine()
        {
            var input = new[]
            {
                "This",
                "is",
                "some",
                "text"
            };
            var edit = new TextEdit(new ParsePosition(2, 0), new ParsePosition(2, 0), new[] { string.Empty, string.Empty });
            var result = edit.Apply(input);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.EqualTo(input));
            Assert.That(result.Length, Is.EqualTo(5));
            Assert.That(result[1], Is.EqualTo("is"));
            Assert.That(result[2], Is.EqualTo(string.Empty));
            Assert.That(result[3], Is.EqualTo("some"));
        }

        [Test]
        public void TextEdit_ApplyReconstruct_DeleteLine()
        {
            var input = new[]
            {
                "This",
                "is",
                string.Empty,
                "some",
                "text"
            };
            var edit = new TextEdit(new ParsePosition(2, 0), new ParsePosition(3, 0), Array.Empty<string>());
            var result = edit.Apply(input);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.EqualTo(input));
            Assert.That(result.Length, Is.EqualTo(4));
            Assert.That(result[1], Is.EqualTo("is"));
            Assert.That(result[2], Is.EqualTo("some"));
        }

        [Test]
        public void TextEdit_ApplyReconstruct_DeleteLineAlternative()
        {
            var input = new[]
            {
                "This",
                "is",
                string.Empty,
                "some",
                "text"
            };
            var edit = new TextEdit(new ParsePosition(1, 2), new ParsePosition(3, 0), new string[] { string.Empty, string.Empty });
            var result = edit.Apply(input);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.EqualTo(input));
            Assert.That(result.Length, Is.EqualTo(4));
            Assert.That(result[1], Is.EqualTo("is"));
            Assert.That(result[2], Is.EqualTo("some"));
        }

        [Test]
        public void TextEdit_ApplyReconstruct_DeleteLineAlternative2()
        {
            var input = new[]
            {
                "This",
                "is",
                "some",
                "text"
            };
            var edit = new TextEdit(new ParsePosition(1, 2), new ParsePosition(2, 0), new string[] { " " });
            var result = edit.Apply(input);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.EqualTo(input));
            Assert.That(result.Length, Is.EqualTo(3));
            Assert.That(result[1], Is.EqualTo("is some"));
        }

        [Test]
        public void TextEdit_ApplyReconstruct_InsertNewLineBreak()
        {
            var input = new[]
            {
                "This",
                "is some",
                "text"
            };
            var edit = new TextEdit(new ParsePosition(1, 2), new ParsePosition(1, 3), new[] { string.Empty, string.Empty });
            var result = edit.Apply(input);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.EqualTo(input));
            Assert.That(result.Length, Is.EqualTo(4));
            Assert.That(result[1], Is.EqualTo("is"));
            Assert.That(result[2], Is.EqualTo("some"));
        }

        [Test]
        public void TextEdit_ApplyReconstruct_DeleteLineBreak()
        {
            var input = new[]
            {
                "This",
                "is",
                "some",
                "text"
            };
            var edit = new TextEdit(new ParsePosition(1, 2), new ParsePosition(2, 0), new[] { " " } );
            var result = edit.Apply(input);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.EqualTo(input));
            Assert.That(result.Length, Is.EqualTo(3));
            Assert.That(result[1], Is.EqualTo("is some"));
        }

        [Test]
        public void TextEdit_ApplyInline_NewLastLineInsert()
        {
            var input = new[]
            {
                "This",
                "is",
                "some",
                "text"
            };
            var edit = new TextEdit(new ParsePosition(4, 0), new ParsePosition(4, 0), new[] { "testing things" });
            var result = edit.Apply(input);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.EqualTo(input));
            Assert.That(result[4], Is.EqualTo("testing things"));
        }
    }
}
