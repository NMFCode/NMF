using AnyText.Tests.ListExpressions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyText.Tests.Languages
{
    [TestFixture]
    public class ListExpressionTests
    {
        [TestCase(1)]
        [TestCase(42)]
        [TestCase(0)]
        public void ListExpressions_ParsesSimpleInteger(int value)
        {
            var grammar = new ListExpressionsGrammar();
            var parser = grammar.CreateParser();
            var input = new string[] { value.ToString(CultureInfo.InvariantCulture) };
            var parsed = parser.Initialize(input);

            var expected = new IntegerNumber { Value = value };

            Assert.That(parser.Context.Errors, Is.Empty);
            Assert.That(parsed, Is.EqualTo(expected).Using<IntegerNumber>((a,b) =>
            {
                Assert.That(a.Value, Is.EqualTo(b.Value));
                return 0;
            }));
        }

        [TestCase(1.0)]
        [TestCase(42.0)]
        [TestCase(0.0)]
        public void ListExpressions_ParsesSimpleDouble(double value)
        {
            var grammar = new ListExpressionsGrammar();
            var parser = grammar.CreateParser();
            var input = new string[] { value.ToString("0.0", CultureInfo.InvariantCulture) };
            var parsed = parser.Initialize(input);

            var expected = new DoubleNumber { Value = value };

            Assert.That(parser.Context.Errors, Is.Empty);
            Assert.That(parsed, Is.EqualTo(expected).Using<DoubleNumber>((a, b) =>
            {
                Assert.That(a.Value, Is.EqualTo(b.Value));
                return 0;
            }));
        }

        [TestCase("foo")]
        [TestCase("bar")]
        [TestCase("a word")]
        public void ListExpressions_ParsesSimpleString(string value)
        {
            var grammar = new ListExpressionsGrammar();
            var parser = grammar.CreateParser();
            var input = new string[] { $"'{value}'" };
            var parsed = parser.Initialize(input);

            var expected = new Text { Value = value };

            Assert.That(parser.Context.Errors, Is.Empty);
            Assert.That(parsed, Is.EqualTo(expected).Using<Text>((a, b) =>
            {
                Assert.That(a.Value, Is.EqualTo(b.Value));
                return 0;
            }));
        }

        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void ListExpressions_ParsesListOfInteger(int length)
        {
            var grammar = new ListExpressionsGrammar();
            var parser = grammar.CreateParser();
            var input = new string[] { string.Join(", ", Enumerable.Range(1, length)) };
            var parsed = parser.Initialize(input);

            Assert.That(parser.Context.Errors, Is.Empty);
            Assert.That(parsed, Is.InstanceOf<List>());

            var list = (List) parsed;
            Assert.That(list.Values.Count, Is.EqualTo(length));
            for (var i = 0; i < length; i++)
            {
                var item = list.Values.ElementAt(i);
                Assert.That(item, Is.InstanceOf<IntegerNumber>());
                Assert.That((item as IntegerNumber)!.Value, Is.EqualTo(i + 1));
            }
        }


        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void ListExpressions_ParsesListOfDouble(int length)
        {
            var grammar = new ListExpressionsGrammar();
            var parser = grammar.CreateParser();
            var input = new string[] { string.Join(", ", Enumerable.Range(1, length).Select(i => $"{i}.5")) };
            var parsed = parser.Initialize(input);

            Assert.That(parser.Context.Errors, Is.Empty);
            Assert.That(parsed, Is.InstanceOf<List>());

            var list = (List)parsed;
            Assert.That(list.Values.Count, Is.EqualTo(length));
            for (var i = 0; i < length; i++)
            {
                var item = list.Values.ElementAt(i);
                Assert.That(item, Is.InstanceOf<DoubleNumber>());
                Assert.That((item as DoubleNumber)!.Value, Is.EqualTo(i + 1.5));
            }
        }


        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void ListExpressions_ParsesListOfString(int length)
        {
            var grammar = new ListExpressionsGrammar();
            var parser = grammar.CreateParser();
            var input = Enumerable.Range(1, length).Select(i => $"'{i}'" + (i != length ? "," : "")).ToArray();
            var parsed = parser.Initialize(input);

            Assert.That(parser.Context.Errors, Is.Empty);
            Assert.That(parsed, Is.InstanceOf<List>());

            var list = (List)parsed;
            Assert.That(list.Values.Count, Is.EqualTo(length));
            for (var i = 0; i < length; i++)
            {
                var item = list.Values.ElementAt(i);
                Assert.That(item, Is.InstanceOf<Text>());
                Assert.That((item as Text)!.Value, Is.EqualTo((i + 1).ToString()));
            }
        }

        [Test]
        public void ListExpressions_ParsesHeterogeneousList()
        {
            var grammar = new ListExpressionsGrammar();
            var parser = grammar.CreateParser();
            var input = new string[] { "1, 2.0, '3'" };
            var parsed = parser.Initialize(input);

            Assert.That(parser.Context.Errors, Is.Empty);
            Assert.That(parsed, Is.InstanceOf<List>());

            var list = (List)parsed;

            Assert.That(list.Values.ElementAt(0), Is.InstanceOf<IntegerNumber>());
            Assert.That(list.Values.ElementAt(1), Is.InstanceOf<DoubleNumber>());
            Assert.That(list.Values.ElementAt(2), Is.InstanceOf<Text>());
        }
    }
}
