using NMF.AnyText;
using NMF.AnyText.Grammars;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyText.Tests.Features
{
    [TestFixture]
    class SelectionRangeTests
    {
        [Test]
        public void GetSelectionRanges()
        {
            var anyText = new AnyTextGrammar();
            var parser = new Parser(new ModelParseContext(anyText));
            var grammar = File.ReadAllLines("AnyText.anytext");
            var parsed = parser.Initialize(grammar);
            Assert.That(parsed, Is.Not.Null);
            Assert.That(parser.Context.Errors, Is.Empty);

            var actual = parser.GetSelectionRanges(new List<ParsePosition>() { new ParsePosition(38, 40) }).First();
            var expected = new SelectionRange()
            {
                Range = new ParseRange()
                {
                    Start = new ParsePosition(38, 36),
                    End = new ParsePosition(38, 89)
                },
                Parent = new SelectionRange()
                {
                    Range = new ParseRange()
                    {
                        Start = new ParsePosition(37, 10),
                        End = new ParsePosition(38, 105)
                    },
                    Parent = new SelectionRange()
                    {
                        Range = new ParseRange()
                        {
                            Start = new ParsePosition(37, 2),
                            End = new ParsePosition(38, 105)
                        },
                        Parent = new SelectionRange()
                        {
                            Range = new ParseRange()
                            {
                                Start = new ParsePosition(36, 0),
                                End = new ParsePosition(40, 0)
                            },
                            Parent = new SelectionRange()
                            {
                                Range = new ParseRange()
                                {
                                    Start = new ParsePosition(7, 0),
                                    End = new ParsePosition(156, 0)
                                },
                                Parent = new SelectionRange()
                                {
                                    Range = new ParseRange()
                                    {
                                        Start = new ParsePosition(0, 0),
                                        End = new ParsePosition(156, 0)
                                    }
                                }
                            }
                        }
                    }
                }
            };

            AssertAreEqual(actual, expected);
        }

        private static void AssertAreEqual(SelectionRange actual, SelectionRange expected)
        {
            Assert.That(actual.Range, Is.EqualTo(expected.Range));
            if (actual.Parent == null || expected.Parent == null)
            {
                Assert.That(actual.Parent, Is.EqualTo(expected.Parent));
                return;
            }
            AssertAreEqual(actual.Parent, expected.Parent);
        }
    }
}
