using AnyText.Test.Metamodel.Baskets;
using AnyText.Test.Metamodel.Expressions;
using AnyText.Tests.BasketsGrammar;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyText.Tests
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
            Assert.That(synthesized, Is.EqualTo("basket Test: Item1"));
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
            Assert.That(synthesized, Is.EqualTo("basket Test: Item1 and Item2"));
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
            Assert.That(synthesized, Is.EqualTo("basket Test: Item1, Item2 and Item3"));
        }
    }
}
