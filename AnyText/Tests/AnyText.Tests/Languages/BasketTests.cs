using AnyText.Test.Metamodel.Baskets;
using AnyText.Test.Metamodel.Expressions;
using AnyText.Tests.BasketsGrammar;
using NMF.AnyText;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyText.Tests.Languages
{
    [TestFixture]
    public class BasketTests
    {

        [Test]
        public void Baskets_ParsesAndPrintsBasketWithOneItem()
        {
            var baskets = new BasketsGrammar.BasketsGrammar();
            var parser = baskets.CreateParser();
            var parsed = parser.Initialize(new[] { "basket Test: Item1" }) as IBaskets;

            Assert.That(parsed, Is.Not.Null);
            Assert.That(parsed.Baskets_, Is.Not.Empty);
            var basket = parsed.Baskets_.Single();
            Assert.That(basket.Items, Is.EquivalentTo(new[] { "Item1" }));

            var synthesized = baskets.GetRule<BasketsGrammar.BasketsGrammar.BasketsRule>().Synthesize(parsed, null, "  ");
            Assert.That(synthesized, Is.EqualTo("basket Test: Item1" + Environment.NewLine));
        }

        [Test]
        public void Baskets_ParsesAndPrintsBasketWithTwoItems()
        {
            var baskets = new BasketsGrammar.BasketsGrammar();
            var parser = baskets.CreateParser();
            var parsed = parser.Initialize(new[] { "basket Test: Item1 and Item2" }) as IBaskets;

            Assert.That(parsed, Is.Not.Null);
            Assert.That(parsed.Baskets_, Is.Not.Empty);
            var basket = parsed.Baskets_.Single();
            Assert.That(basket.Items, Is.EquivalentTo(new[] { "Item1", "Item2" }));

            var synthesized = baskets.GetRule<BasketsGrammar.BasketsGrammar.BasketsRule>().Synthesize(parsed, null, "  ");
            Assert.That(synthesized, Is.EqualTo("basket Test: Item1 and Item2" + Environment.NewLine));
        }

        [Test]
        public void Baskets_ParsesAndPrintsBasketWithThreeItems()
        {
            var baskets = new BasketsGrammar.BasketsGrammar();
            var parser = baskets.CreateParser();
            var parsed = parser.Initialize(new[] { "basket Test: Item1, Item2 and Item3" }) as IBaskets;

            Assert.That(parsed, Is.Not.Null);
            Assert.That(parsed.Baskets_, Is.Not.Empty);
            var basket = parsed.Baskets_.Single();
            Assert.That(basket.Items, Is.EquivalentTo(new[] { "Item1", "Item2", "Item3" }));

            var synthesized = baskets.GetRule<BasketsGrammar.BasketsGrammar.BasketsRule>().Synthesize(parsed, null, "  ");
            Assert.That(synthesized, Is.EqualTo("basket Test: Item1, Item2 and Item3" + Environment.NewLine));
        }

        [Test]
        public void Baskets_AddedBasketCorrectlyProcessed()
        {
            var baskets = new BasketsGrammar.BasketsGrammar();
            var parser = baskets.CreateParser();
            var parsed = parser.Initialize(new[] { "basket Test: Item1,", "Item2", "and Item3" }) as IBaskets;

            Assert.That(parsed, Is.Not.Null);
            Assert.That(parsed.Baskets_, Is.Not.Empty);
            var basket = parsed.Baskets_.Single();
            var raised = false;
            basket.BubbledChange += (o, e) =>
            {
                raised = true;
                Assert.That(e.Element, Is.SameAs(basket));
                Assert.That(e.ChangeType == NMF.Models.ChangeType.CollectionChanged || e.ChangeType == NMF.Models.ChangeType.CollectionChanging);
                var originale = (NotifyCollectionChangedEventArgs)e.OriginalEventArgs;
                Assert.That(originale.NewStartingIndex, Is.EqualTo(1));
                Assert.That(originale.NewItems![0], Is.EqualTo("Item1b"));
                Assert.That(originale.Action, Is.EqualTo(NotifyCollectionChangedAction.Add));
            };

            var parsed2 = parser.Update(new TextEdit(new ParsePosition(1, 0), new ParsePosition(1, 0), new[] { "Item1b,", "" }));
            Assert.That(parser.Context.Errors, Is.Empty);
            Assert.That(raised);
            Assert.That(parsed2, Is.EqualTo(parsed));
            Assert.That(basket.Items, Is.EquivalentTo(new[] { "Item1", "Item1b", "Item2", "Item3" }));
        }

        [Test]
        public void Baskets_AddedBasketCorrectlyProcessed2()
        {
            var baskets = new BasketsGrammar.BasketsGrammar();
            var parser = baskets.CreateParser();
            var parsed = parser.Initialize(new[] { "basket Test: Item1,", "Item2", "and Item3" }) as IBaskets;

            Assert.That(parsed, Is.Not.Null);
            Assert.That(parsed.Baskets_, Is.Not.Empty);
            var basket = parsed.Baskets_.Single();
            var raised = false;
            basket.BubbledChange += (o, e) =>
            {
                raised = true;
                Assert.That(e.Element, Is.SameAs(basket));
                Assert.That(e.ChangeType == NMF.Models.ChangeType.CollectionChanged || e.ChangeType == NMF.Models.ChangeType.CollectionChanging);
                var originale = (NotifyCollectionChangedEventArgs)e.OriginalEventArgs;
            };

            var parsed2 = parser.Update(new TextEdit(new ParsePosition(1, 5), new ParsePosition(2, 0), new[] { ",", "Item2b", "" }));
            Assert.That(parser.Context.Errors, Is.Empty);
            Assert.That(raised);
            Assert.That(parsed2, Is.EqualTo(parsed));
            Assert.That(basket.Items, Is.EquivalentTo(new[] { "Item1", "Item2", "Item2b", "Item3" }));
        }

        [Test]
        public void Baskets_ReplacedBasketCorrectlyProcessed()
        {
            var baskets = new BasketsGrammar.BasketsGrammar();
            var parser = baskets.CreateParser();
            var parsed = parser.Initialize(new[] { "basket Test: Item1,", "Item2", "and Item3" }) as IBaskets;

            Assert.That(parsed, Is.Not.Null);
            Assert.That(parsed.Baskets_, Is.Not.Empty);
            var basket = parsed.Baskets_.Single();
            var raised = false;
            basket.BubbledChange += (o, e) =>
            {
                raised = true;
                Assert.That(e.Element, Is.SameAs(basket));
                Assert.That(e.ChangeType == NMF.Models.ChangeType.CollectionChanged || e.ChangeType == NMF.Models.ChangeType.CollectionChanging);
                var originale = (NotifyCollectionChangedEventArgs)e.OriginalEventArgs;
                Assert.That(originale.NewStartingIndex, Is.EqualTo(1));
                Assert.That(originale.NewItems![0], Is.EqualTo("Item2b"));
                Assert.That(originale.Action, Is.EqualTo(NotifyCollectionChangedAction.Replace));
            };

            var parsed2 = parser.Update(new TextEdit(new ParsePosition(1, 0), new ParsePosition(2, 0), new[] { "Item2b", "" }));
            Assert.That(parser.Context.Errors, Is.Empty);
            Assert.That(raised);
            Assert.That(parsed2, Is.EqualTo(parsed));
            Assert.That(basket.Items, Is.EquivalentTo(new[] { "Item1", "Item2b", "Item3" }));
        }

        [Test]
        public void Baskets_UpdatedBasketCorrectlyProcessed()
        {
            var baskets = new BasketsGrammar.BasketsGrammar();
            var parser = baskets.CreateParser();
            var parsed = parser.Initialize(new[] { "basket Test: Item1,", "Item2", "and Item3" }) as IBaskets;

            Assert.That(parsed, Is.Not.Null);
            Assert.That(parsed.Baskets_, Is.Not.Empty);
            var basket = parsed.Baskets_.Single();
            var raised = false;
            basket.BubbledChange += (o, e) =>
            {
                raised = true;
                Assert.That(e.Element, Is.SameAs(basket));
                Assert.That(e.ChangeType == NMF.Models.ChangeType.CollectionChanged || e.ChangeType == NMF.Models.ChangeType.CollectionChanging);
                var originale = (NotifyCollectionChangedEventArgs)e.OriginalEventArgs;
                Assert.That(originale.NewStartingIndex, Is.EqualTo(1));
                Assert.That(originale.NewItems![0], Is.EqualTo("Item2b"));
                Assert.That(originale.Action, Is.EqualTo(NotifyCollectionChangedAction.Replace));
            };

            var parsed2 = parser.Update(new TextEdit(new ParsePosition(1, 5), new ParsePosition(1, 5), new[] { "b" }));
            Assert.That(parser.Context.Errors, Is.Empty);
            Assert.That(raised);
            Assert.That(parsed2, Is.EqualTo(parsed));
            Assert.That(basket.Items, Is.EquivalentTo(new[] { "Item1", "Item2b", "Item3" }));
        }

        [Test]
        public void Baskets_DeletedBasketCorrectlyProcessed()
        {
            var baskets = new BasketsGrammar.BasketsGrammar();
            var parser = baskets.CreateParser();
            var parsed = parser.Initialize(new[] { "basket Test: Item1,", "Item2", "and Item3" }) as IBaskets;

            Assert.That(parsed, Is.Not.Null);
            Assert.That(parsed.Baskets_, Is.Not.Empty);
            var basket = parsed.Baskets_.Single();
            var raised = false;
            basket.BubbledChange += (o, e) =>
            {
                raised = true;
                Assert.That(e.Element, Is.SameAs(basket));
                Assert.That(e.ChangeType == NMF.Models.ChangeType.CollectionChanged || e.ChangeType == NMF.Models.ChangeType.CollectionChanging);
                var originale = (NotifyCollectionChangedEventArgs)e.OriginalEventArgs;
            };

            var parsed2 = parser.Update(new TextEdit(new ParsePosition(0, 18), new ParsePosition(2, 0), new[] { "", "" }));
            Assert.That(parser.Context.Errors, Is.Empty);
            Assert.That(raised);
            Assert.That(parsed2, Is.EqualTo(parsed));
            Assert.That(basket.Items, Is.EquivalentTo(new[] { "Item1", "Item3" }));
        }

        [Test]
        public void Baskets_DeletedBasketCorrectlyProcessed2()
        {
            var baskets = new BasketsGrammar.BasketsGrammar();
            var parser = baskets.CreateParser();
            var parsed = parser.Initialize(new[] { "basket Test: Item1,", "Item2", "and Item3" }) as IBaskets;

            Assert.That(parsed, Is.Not.Null);
            Assert.That(parsed.Baskets_, Is.Not.Empty);
            var basket = parsed.Baskets_.Single();
            var raised = false;
            basket.BubbledChange += (o, e) =>
            {
                raised = true;
                Assert.That(e.Element, Is.SameAs(basket));
                Assert.That(e.ChangeType == NMF.Models.ChangeType.CollectionChanged || e.ChangeType == NMF.Models.ChangeType.CollectionChanging);
                var originale = (NotifyCollectionChangedEventArgs)e.OriginalEventArgs;
                Assert.That(originale.OldStartingIndex, Is.EqualTo(0));
                Assert.That(originale.OldItems![0], Is.EqualTo("Item1"));
                Assert.That(originale.Action, Is.EqualTo(NotifyCollectionChangedAction.Remove));
            };

            var parsed2 = parser.Update(new TextEdit(new ParsePosition(0, 12), new ParsePosition(0, 19), new[] { "" }));
            Assert.That(parser.Context.Errors, Is.Empty);
            Assert.That(raised);
            Assert.That(parsed2, Is.EqualTo(parsed));
            Assert.That(basket.Items, Is.EquivalentTo(new[] { "Item2", "Item3" }));
        }
    }
}
