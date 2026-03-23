using AnyText.Tests.ListExpressions;
using NMF.AnyText;
using NMF.Models;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
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

        [Test]
        public void ListExpressions_FirstItemRemoved()
        {
            var grammar = new ListExpressionsGrammar();
            var parser = grammar.CreateParser();
            var input = new string[] { "1,", "2.0,", "'3'" };
            var parsed = parser.Initialize(input);

            Assert.That(parser.Context.Errors, Is.Empty);
            Assert.That(parsed, Is.InstanceOf<List>());

            var list = (List)parsed;
            var item1 = list.Values.ElementAt(0);
            var item2 = list.Values.ElementAt(1);
            var item3 = list.Values.ElementAt(2);
            var triggered = false;
            var replaceTriggered = false;
            list.BubbledChange += (o, e) =>
            {
                if (e.ChangeType == ChangeType.CollectionChanged)
                {
                    triggered = true;
                    Assert.That(e.Element, Is.EqualTo(list));
                    var ev = e.OriginalEventArgs as NotifyCollectionChangedEventArgs;
                    if (ev!.Action == NotifyCollectionChangedAction.Replace)
                    {
                        replaceTriggered = true;
                        Assert.That(ev.OldStartingIndex, Is.EqualTo(0));
                        Assert.That(ev.OldItems![0], Is.SameAs(item1));
                        Assert.That(ev.NewItems![0], Is.SameAs(item2));
                    }
                    else
                    {
                        Assert.That(ev!.Action, Is.EqualTo(NotifyCollectionChangedAction.Remove));
                        Assert.That(ev.OldItems![0], Is.SameAs(item2));
                    }
                }                
            };

            parser.Update(new TextEdit(default, new ParsePosition(1, 0), Array.Empty<string>()));
            Assert.That(list.Values.Count, Is.EqualTo(2));
            Assert.That(triggered, Is.True);
            Assert.That(replaceTriggered, Is.True);

            Assert.That(list.Values.ElementAt(0), Is.SameAs(item2));
            Assert.That(list.Values.ElementAt(1), Is.SameAs(item3));
        }

        [Test]
        public void ListExpressions_SecondItemRemoved()
        {
            var grammar = new ListExpressionsGrammar();
            var parser = grammar.CreateParser();
            var input = new string[] { "1,", "2.0,", "'3'" };
            var parsed = parser.Initialize(input);

            Assert.That(parser.Context.Errors, Is.Empty);
            Assert.That(parsed, Is.InstanceOf<List>());

            var list = (List)parsed;
            var item1 = list.Values.ElementAt(0);
            var item2 = list.Values.ElementAt(1);
            var item3 = list.Values.ElementAt(2);
            var triggered = false;
            list.BubbledChange += (o, e) =>
            {
                if (e.ChangeType == ChangeType.CollectionChanged)
                {
                    triggered = true;
                    Assert.That(e.Element, Is.EqualTo(list));
                }
            };

            parser.Update(new TextEdit(new ParsePosition(1, 0), new ParsePosition(2, 0), Array.Empty<string>()));
            Assert.That(list.Values.Count, Is.EqualTo(2));
            Assert.That(triggered, Is.True);

            Assert.That(list.Values.ElementAt(0), Is.SameAs(item1));
            Assert.That(list.Values.ElementAt(1), Is.SameAs(item3));
        }

        [Test]
        public void ListExpressions_MultipleItemsRemoved()
        {
            var grammar = new ListExpressionsGrammar();
            var parser = grammar.CreateParser();
            var input = new string[] { "1,", "2.0,", "2.5,", "'3'" };
            var parsed = parser.Initialize(input);

            Assert.That(parser.Context.Errors, Is.Empty);
            Assert.That(parsed, Is.InstanceOf<List>());

            var list = (List)parsed;
            var item1 = list.Values.ElementAt(0);
            var item2 = list.Values.ElementAt(1);
            var item3 = list.Values.ElementAt(3);
            var triggered = false;
            list.BubbledChange += (o, e) =>
            {
                if (e.ChangeType == ChangeType.CollectionChanged)
                {
                    triggered = true;
                    //Assert.That(e.Element, Is.EqualTo(list));
                    //var ev = e.OriginalEventArgs as NotifyCollectionChangedEventArgs;

                    //Assert.That(ev!.Action, Is.EqualTo(NotifyCollectionChangedAction.Remove));
                }
            };

            parser.Update(new TextEdit(new ParsePosition(1, 0), new ParsePosition(3, 0), Array.Empty<string>()));
            Assert.That(list.Values.Count, Is.EqualTo(2));
            Assert.That(triggered, Is.True);

            Assert.That(list.Values.ElementAt(0), Is.SameAs(item1));
            Assert.That(list.Values.ElementAt(1), Is.SameAs(item3));
        }

        [Test]
        public void ListExpressions_SecondItemRemoved_WithAdjacentChange()
        {
            var grammar = new ListExpressionsGrammar();
            var parser = grammar.CreateParser();
            var input = new string[] { "1,", "2.0,", "'3',", "'4'" };
            var parsed = parser.Initialize(input);

            Assert.That(parser.Context.Errors, Is.Empty);
            Assert.That(parsed, Is.InstanceOf<List>());

            var list = (List)parsed;
            var item1 = list.Values.ElementAt(0);
            var item2 = list.Values.ElementAt(1);
            var item3 = list.Values.ElementAt(2);
            var item4 = list.Values.ElementAt(3);
            var triggered = false;
            list.BubbledChange += (o, e) =>
            {
                if (e.ChangeType == ChangeType.CollectionChanged)
                {
                    triggered = true;
                    Assert.That(e.Element, Is.EqualTo(list));
                }
            };

            parser.Update(
                [new TextEdit(new ParsePosition(1, 0), new ParsePosition(2, 0), Array.Empty<string>()),
                 new TextEdit(new ParsePosition(2, 2), new ParsePosition(2, 2), ["2"])]);
            Assert.That(list.Values.Count, Is.EqualTo(3));
            Assert.That(triggered, Is.True);

            Assert.That(list.Values.ElementAt(0), Is.SameAs(item1));
            Assert.That(list.Values.ElementAt(1), Is.SameAs(item3));
            Assert.That(list.Values.ElementAt(2), Is.SameAs(item4));
        }

        [Test]
        public void ListExpressions_ThirdItemRemoved()
        {
            var grammar = new ListExpressionsGrammar();
            var parser = grammar.CreateParser();
            var input = new string[] { "1, 2.0, '3'" };
            var parsed = parser.Initialize(input);

            Assert.That(parser.Context.Errors, Is.Empty);
            Assert.That(parsed, Is.InstanceOf<List>());

            var list = (List)parsed;
            var item1 = list.Values.ElementAt(0);
            var item2 = list.Values.ElementAt(1);
            var item3 = list.Values.ElementAt(2);
            var triggered = false;
            list.BubbledChange += (o, e) =>
            {
                if (e.ChangeType == ChangeType.CollectionChanged)
                {
                    triggered = true;
                    Assert.That(e.Element, Is.EqualTo(list));
                    var ev = e.OriginalEventArgs as NotifyCollectionChangedEventArgs;
                    Assert.That(ev!.Action, Is.EqualTo(NotifyCollectionChangedAction.Remove));
                    Assert.That(ev.OldStartingIndex, Is.EqualTo(2));
                    Assert.That(ev.OldItems![0], Is.SameAs(item3));
                }
            };

            parser.Update(new TextEdit(new ParsePosition(0, 6), new ParsePosition(0, 11), Array.Empty<string>()));
            Assert.That(list.Values.Count, Is.EqualTo(2));
            Assert.That(triggered, Is.True);

            Assert.That(list.Values.ElementAt(0), Is.SameAs(item1));
            Assert.That(list.Values.ElementAt(1), Is.SameAs(item2));
        }


        [Test]
        public void ListExpressions_FirstItemAdded()
        {
            var grammar = new ListExpressionsGrammar();
            var parser = grammar.CreateParser();
            var input = new string[] { "2.0,", "'3'" };
            var parsed = parser.Initialize(input);

            Assert.That(parser.Context.Errors, Is.Empty);
            Assert.That(parsed, Is.InstanceOf<List>());

            var list = (List)parsed;
            var item2 = list.Values.ElementAt(0);
            var item3 = list.Values.ElementAt(1);
            var triggered = false;
            var replaceTriggered = false;
            list.BubbledChange += (o, e) =>
            {
                if (e.ChangeType == ChangeType.CollectionChanged)
                {
                    triggered = true;
                    Assert.That(e.Element, Is.EqualTo(list));
                    var ev = e.OriginalEventArgs as NotifyCollectionChangedEventArgs;
                    if (ev!.Action == NotifyCollectionChangedAction.Replace)
                    {
                        replaceTriggered = true;
                        Assert.That(ev.OldStartingIndex, Is.EqualTo(0));
                        Assert.That(ev.OldItems![0], Is.SameAs(item2));
                    }
                    else
                    {
                        Assert.That(ev!.Action, Is.EqualTo(NotifyCollectionChangedAction.Add));
                        Assert.That(ev.NewItems![0], Is.InstanceOf<DoubleNumber>());
                    }
                }
            };

            parser.Update(new TextEdit(default, default, ["1,",""]));
            Assert.That(list.Values.Count, Is.EqualTo(3));
            Assert.That(triggered, Is.True);
            Assert.That(replaceTriggered, Is.True);
            Assert.That(list.Values.ElementAt(2), Is.SameAs(item3));
        }

        [Test]
        public void ListExpressions_SecondItemAdded()
        {
            var grammar = new ListExpressionsGrammar();
            var parser = grammar.CreateParser();
            var input = new string[] { "1,", "'3'" };
            var parsed = parser.Initialize(input);

            Assert.That(parser.Context.Errors, Is.Empty);
            Assert.That(parsed, Is.InstanceOf<List>());

            var list = (List)parsed;
            var item1 = list.Values.ElementAt(0);
            var item3 = list.Values.ElementAt(1);
            var triggered = false;
            list.BubbledChange += (o, e) =>
            {
                if (e.ChangeType == ChangeType.CollectionChanged)
                {
                    triggered = true;
                    Assert.That(e.Element, Is.EqualTo(list));
                }
            };

            parser.Update(new TextEdit(new ParsePosition(1, 0), new ParsePosition(1, 0), ["2.0,", ""]));
            Assert.That(list.Values.Count, Is.EqualTo(3));
            Assert.That(triggered, Is.True);

            Assert.That(list.Values.ElementAt(0), Is.SameAs(item1));
            Assert.That(list.Values.ElementAt(1), Is.InstanceOf<DoubleNumber>());
            Assert.That(list.Values.ElementAt(2), Is.InstanceOf<Text>());
        }

        [Test]
        public void ListExpressions_MultipleItemsAdded()
        {
            var grammar = new ListExpressionsGrammar();
            var parser = grammar.CreateParser();
            var input = new string[] { "1,", "'3'" };
            var parsed = parser.Initialize(input);

            Assert.That(parser.Context.Errors, Is.Empty);
            Assert.That(parsed, Is.InstanceOf<List>());

            var list = (List)parsed;
            var item1 = list.Values.ElementAt(0);
            var item3 = list.Values.ElementAt(1);
            var triggered = false;
            list.BubbledChange += (o, e) =>
            {
                if (e.ChangeType == ChangeType.CollectionChanged)
                {
                    triggered = true;
                    Assert.That(e.Element, Is.EqualTo(list));
                }
            };

            parser.Update(new TextEdit(new ParsePosition(1, 0), new ParsePosition(1, 0), ["2.0, 2.5,", ""]));
            Assert.That(list.Values.Count, Is.EqualTo(4));
            Assert.That(triggered, Is.True);

            Assert.That(list.Values.ElementAt(0), Is.SameAs(item1));
            Assert.That(list.Values.ElementAt(1), Is.InstanceOf<DoubleNumber>());
            Assert.That(list.Values.ElementAt(2), Is.InstanceOf<DoubleNumber>());
            Assert.That(list.Values.ElementAt(3), Is.InstanceOf<Text>());
        }

        [Test]
        public void ListExpressions_HeterogeneousChanges()
        {
            var grammar = new ListExpressionsGrammar();
            var parser = grammar.CreateParser();
            var input = new string[] { "1,", "2,", "3,", "'3'" };
            var parsed = parser.Initialize(input);

            Assert.That(parser.Context.Errors, Is.Empty);
            Assert.That(parsed, Is.InstanceOf<List>());

            var list = (List)parsed;
            var item1 = list.Values.ElementAt(0);
            var item3 = list.Values.ElementAt(3);
            var triggered = false;
            list.BubbledChange += (o, e) =>
            {
                if (e.ChangeType == ChangeType.CollectionChanged)
                {
                    triggered = true;
                    Assert.That(e.Element, Is.EqualTo(list));
                }
            };

            parser.Update(new TextEdit(new ParsePosition(1, 0), new ParsePosition(3, 0), ["2.0, 2.5,", ""]));
            Assert.That(list.Values.Count, Is.EqualTo(4));
            Assert.That(triggered, Is.True);

            Assert.That(list.Values.ElementAt(0), Is.SameAs(item1));
            Assert.That(list.Values.ElementAt(1), Is.InstanceOf<DoubleNumber>());
            Assert.That(list.Values.ElementAt(2), Is.InstanceOf<DoubleNumber>());
            Assert.That(list.Values.ElementAt(3), Is.SameAs(item3));
        }

        [Test]
        public void ListExpressions_SecondItemAdded_WithAdjacentChange()
        {
            var grammar = new ListExpressionsGrammar();
            var parser = grammar.CreateParser();
            var input = new string[] { "1,", "'3',", "'4'" };
            var parsed = parser.Initialize(input);

            Assert.That(parser.Context.Errors, Is.Empty);
            Assert.That(parsed, Is.InstanceOf<List>());

            var list = (List)parsed;
            var item1 = list.Values.ElementAt(0);
            var item3 = list.Values.ElementAt(1);
            var item4 = list.Values.ElementAt(2);
            var triggered = false;
            list.BubbledChange += (o, e) =>
            {
                if (e.ChangeType == ChangeType.CollectionChanged)
                {
                    triggered = true;
                    Assert.That(e.Element, Is.EqualTo(list));
                }
            };

            parser.Update(
                [new TextEdit(new ParsePosition(1, 0), new ParsePosition(1, 0), ["2.0,", ""]),
                 new TextEdit(new ParsePosition(2, 2), new ParsePosition(2, 2), ["2"])]);
            Assert.That(list.Values.Count, Is.EqualTo(4));
            Assert.That(triggered, Is.True);

            Assert.That(list.Values.ElementAt(0), Is.SameAs(item1));
            Assert.That(list.Values.ElementAt(1), Is.InstanceOf<DoubleNumber>());
            Assert.That(list.Values.ElementAt(2), Is.InstanceOf<Text>());
            Assert.That(list.Values.ElementAt(3), Is.SameAs(item4));
        }

        [Test]
        public void ListExpressions_ThirdItemAdded()
        {
            var grammar = new ListExpressionsGrammar();
            var parser = grammar.CreateParser();
            var input = new string[] { "1, 2.0" };
            var parsed = parser.Initialize(input);

            Assert.That(parser.Context.Errors, Is.Empty);
            Assert.That(parsed, Is.InstanceOf<List>());

            var list = (List)parsed;
            var item1 = list.Values.ElementAt(0);
            var item2 = list.Values.ElementAt(1);
            var triggered = false;
            list.BubbledChange += (o, e) =>
            {
                if (e.ChangeType == ChangeType.CollectionChanged)
                {
                    triggered = true;
                    Assert.That(e.Element, Is.EqualTo(list));
                    var ev = e.OriginalEventArgs as NotifyCollectionChangedEventArgs;
                    Assert.That(ev!.Action, Is.EqualTo(NotifyCollectionChangedAction.Add));
                    Assert.That(ev.NewStartingIndex, Is.EqualTo(2));
                }
            };

            parser.Update(new TextEdit(new ParsePosition(0, 6), new ParsePosition(0, 6), [", '3'"]));
            Assert.That(list.Values.Count, Is.EqualTo(3));
            Assert.That(triggered, Is.True);

            Assert.That(list.Values.ElementAt(0), Is.SameAs(item1));
            Assert.That(list.Values.ElementAt(1), Is.SameAs(item2));
            Assert.That(list.Values.ElementAt(2), Is.InstanceOf<Text>());
        }



        [Test]
        public void ListExpressions_FirstItemRemoved_Synthesized()
        {
            var item1 = new IntegerNumber { Value = 1 };
            var item2 = new DoubleNumber { Value = 2.0 };
            var item3 = new Text { Value = "test" };
            var list = new List
            {
                Values = {item1, item2, item3}
            };

            var grammar = new ListExpressionsGrammar();
            var parser = grammar.CreateParser();
            parser.Initialize(list);

            Assert.That(parser.Context.Errors, Is.Empty);
            var triggered = false;
            var replaceTriggered = 0;
            list.BubbledChange += (o, e) =>
            {
                if (e.ChangeType == ChangeType.CollectionChanged)
                {
                    triggered = true;
                    Assert.That(e.Element, Is.EqualTo(list));
                    var ev = e.OriginalEventArgs as NotifyCollectionChangedEventArgs;
                    if (ev!.Action == NotifyCollectionChangedAction.Replace)
                    {
                        Assert.That(ev.OldStartingIndex, Is.EqualTo(replaceTriggered));
                        replaceTriggered++;
                    }
                    else
                    {
                        Assert.That(ev!.Action, Is.EqualTo(NotifyCollectionChangedAction.Remove));
                    }
                }
            };

            // input is '1 , 2.0 , "test"'
            parser.Update(new TextEdit(default, new ParsePosition(0, 4), Array.Empty<string>()));
            Assert.That(list.Values.Count, Is.EqualTo(2));
            Assert.That(triggered, Is.True);
            Assert.That(replaceTriggered, Is.EqualTo(2));

            Assert.That(list.Values.ElementAt(0), Is.InstanceOf<DoubleNumber>());
            Assert.That(list.Values.ElementAt(1), Is.InstanceOf<Text>());
        }

        [Test]
        public void ListExpressions_SecondItemRemoved_Synthesized()
        {
            var item1 = new IntegerNumber { Value = 1 };
            var item2 = new DoubleNumber { Value = 2.0 };
            var item3 = new Text { Value = "test" };
            var list = new List
            {
                Values = { item1, item2, item3 }
            };

            var grammar = new ListExpressionsGrammar();
            var parser = grammar.CreateParser();
            parser.Initialize(list);

            Assert.That(parser.Context.Errors, Is.Empty);
            var triggered = false;
            list.BubbledChange += (o, e) =>
            {
                if (e.ChangeType == ChangeType.CollectionChanged)
                {
                    Assert.That(e.Element, Is.EqualTo(list));
                    var ev = e.OriginalEventArgs as NotifyCollectionChangedEventArgs;
                    if (ev!.Action == NotifyCollectionChangedAction.Remove)
                    {
                        triggered = true;
                        //Assert.That(ev.OldStartingIndex, Is.EqualTo(1));
                        //Assert.That(ev.OldItems![0], Is.SameAs(item2));
                    }
                }
            };

            // input is '1 , 2.0 , "test"'
            parser.Update(new TextEdit(new ParsePosition(0, 1), new ParsePosition(0, 8), Array.Empty<string>()));
            Assert.That(list.Values.Count, Is.EqualTo(2));
            Assert.That(triggered, Is.True);

            Assert.That(list.Values.ElementAt(0), Is.SameAs(item1));
            //Assert.That(list.Values.ElementAt(1), Is.SameAs(item3));
        }

        [Test]
        public void ListExpressions_ThirdItemRemoved_Synthesized()
        {
            var item1 = new IntegerNumber { Value = 1 };
            var item2 = new DoubleNumber { Value = 2.0 };
            var item3 = new Text { Value = "test" };
            var list = new List
            {
                Values = { item1, item2, item3 }
            };

            var grammar = new ListExpressionsGrammar();
            var parser = grammar.CreateParser();
            parser.Initialize(list);

            Assert.That(parser.Context.Errors, Is.Empty);
            var triggered = false;
            list.BubbledChange += (o, e) =>
            {
                if (e.ChangeType == ChangeType.CollectionChanged)
                {
                    triggered = true;
                    Assert.That(e.Element, Is.EqualTo(list));
                    var ev = e.OriginalEventArgs as NotifyCollectionChangedEventArgs;
                    Assert.That(ev!.Action, Is.EqualTo(NotifyCollectionChangedAction.Remove));
                    Assert.That(ev.OldStartingIndex, Is.EqualTo(2));
                    Assert.That(ev.OldItems![0], Is.SameAs(item3));
                }
            };

            // input is '1 , 2.0 , "test"'
            parser.Update(new TextEdit(new ParsePosition(0, 7), new ParsePosition(0, 16), Array.Empty<string>()));
            Assert.That(list.Values.Count, Is.EqualTo(2));
            Assert.That(triggered, Is.True);

            Assert.That(list.Values.ElementAt(0), Is.SameAs(item1));
            Assert.That(list.Values.ElementAt(1), Is.SameAs(item2));
        }
    }
}
