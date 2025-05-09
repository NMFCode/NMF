using AnyText.Test.Metamodel.Expressions;
using AnyText.Tests.ExpressionGrammar;
using NMF.AnyText;
using NMF.AnyText.Grammars;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyText.Tests.Languages
{
    [TestFixture]
    public class ExpressionTests
    {

        [Test]
        public void AnyText_CanLoadExpressionsGrammar()
        {
            var anyText = new AnyTextGrammar();
            var parser = new Parser(new ModelParseContext(anyText));
            var grammar = File.ReadAllLines("Expressions.anytext");
            var parsed = parser.Initialize(grammar);
            Assert.That(parsed, Is.Not.Null);
            Assert.That(parser.Context.Errors, Is.Empty);
        }

        [Test]
        public void AnyText_PrettyPrintExpressionsMatchesItself()
        {
            var anyText = new AnyTextGrammar();
            var parser = new Parser(new ModelParseContext(anyText));
            var grammar = File.ReadAllLines("Expressions.anytext");
            var parsed = parser.Initialize(grammar) as NMF.AnyText.Metamodel.Grammar;

            var synthesized = anyText.GetRule<AnyTextGrammar.GrammarRule>().Synthesize(parsed, parser.Context);
            Assert.That(synthesized, Is.Not.Null);
            var joined = string.Join(Environment.NewLine, grammar);
            Assert.That(synthesized, Is.EqualTo(joined));
        }

        [Test]
        public void Expressions_ParsesAndPrintsSumOfProducts()
        {
            var expressions = new ExpressionsGrammar();
            var parser = expressions.CreateParser();
            var parsed = parser.Initialize(new[] { "1 * 2 + 3 * 4" }) as IBinaryExpression;
            Assert.That(parsed, Is.Not.Null);
            Assert.That(parsed!.Operator, Is.EqualTo(BinaryOperator.Add));
            Assert.That(parsed.Left, Is.InstanceOf<IBinaryExpression>());
            Assert.That(parsed.Right, Is.InstanceOf<IBinaryExpression>());
            var left = (IBinaryExpression)parsed.Left;
            var right = (IBinaryExpression)parsed.Right;
            Assert.That(left.Operator, Is.EqualTo(BinaryOperator.Multiply));
            Assert.That(right.Operator, Is.EqualTo(BinaryOperator.Multiply));
            Assert.That((left.Left as LiteralExpression)!.Value, Is.EqualTo(1));
            Assert.That((left.Right as LiteralExpression)!.Value, Is.EqualTo(2));
            Assert.That((right.Left as LiteralExpression)!.Value, Is.EqualTo(3));
            Assert.That((right.Right as LiteralExpression)!.Value, Is.EqualTo(4));

            var synthesized = expressions.GetRule<ExpressionsGrammar.ExpressionRule>().Synthesize(parsed, null, "  ");
            Assert.That(synthesized, Is.EqualTo("1 * 2 + 3 * 4"));
        }

        [Test]
        public void Expressions_ParsesAndPrintsProductOfSums()
        {
            var expressions = new ExpressionsGrammar();
            var parser = expressions.CreateParser();
            var parsed = parser.Initialize(new[] { "( 1 + 2 ) * ( 3 + 4 )" }) as IBinaryExpression;
            Assert.That(parsed, Is.Not.Null);
            Assert.That(parsed!.Operator, Is.EqualTo(BinaryOperator.Multiply));
            Assert.That(parsed.Left, Is.InstanceOf<IBinaryExpression>());
            Assert.That(parsed.Right, Is.InstanceOf<IBinaryExpression>());
            var left = (IBinaryExpression)parsed.Left;
            var right = (IBinaryExpression)parsed.Right;
            Assert.That(left.Operator, Is.EqualTo(BinaryOperator.Add));
            Assert.That(right.Operator, Is.EqualTo(BinaryOperator.Add));
            Assert.That((left.Left as LiteralExpression)!.Value, Is.EqualTo(1));
            Assert.That((left.Right as LiteralExpression)!.Value, Is.EqualTo(2));
            Assert.That((right.Left as LiteralExpression)!.Value, Is.EqualTo(3));
            Assert.That((right.Right as LiteralExpression)!.Value, Is.EqualTo(4));

            var synthesized = expressions.GetRule<ExpressionsGrammar.ExpressionRule>().Synthesize(parsed, null, "  ");
            Assert.That(synthesized, Is.EqualTo("( 1 + 2 ) * ( 3 + 4 )"));
        }
    }
}
