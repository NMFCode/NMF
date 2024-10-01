using NMF.AnyText.Model;
using NMF.AnyText.Rules;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NMF.AnyText.Tests
{
    [TestFixture]
    public partial class ExpressionGrammarTests
    {
        public abstract class Expr { }

        public class BinExpr : Expr
        {
            public Expr Left { get; set; }
            public Expr Right { get; set; }

            public BinOp Op { get; set; }
        }

        public enum BinOp
        {
            Add,
            Mul
        }

        public class Lit : Expr
        {
            public int Val { get; set; }
        }

        private class AddRule : ModelElementRule<BinExpr>
        {
            protected override BinExpr CreateElement(IEnumerable<RuleApplication> inner)
            {
                return new BinExpr { Op = BinOp.Add };
            }
        }

        private class MulRule : ModelElementRule<BinExpr>
        {
            protected override BinExpr CreateElement(IEnumerable<RuleApplication> inner)
            {
                return new BinExpr { Op = BinOp.Mul };
            }
        }

        private class ConvertToLitRule : ConvertRule<Lit>
        {
            public override Lit Convert(string text, ParseContext context)
            {
                return new Lit { Val = int.Parse(text) };
            }
        }

        private class AssignLeft : AssignRule<BinExpr, Expr>
        {
            protected override void OnChangeValue(BinExpr semanticElement, Expr propertyValue, ParseContext context)
            {
                semanticElement.Left = propertyValue;
            }
        }

        private class AssignRight : AssignRule<BinExpr, Expr>
        {
            protected override void OnChangeValue(BinExpr semanticElement, Expr propertyValue, ParseContext context)
            {
                semanticElement.Right = propertyValue;
            }
        }

        private static Rule CreateParseRule()
        {
            var expr = new ChoiceRule();
            var add = new AddRule();
            var multiply = new MulRule();
            var factor = new ChoiceRule();
            var summand = new ChoiceRule();
            var lit = new ConvertToLitRule();
            var parantheses = new ParanthesesRule();

            expr.Alternatives = new Rule[]
            {
                add,
                summand
            };
            add.Rules = new Rule[]
            {
                new AssignLeft { Inner = summand },
                new LiteralRule("+"),
                new AssignRight { Inner = summand },
            };
            multiply.Rules = new Rule[]
            {
                new AssignLeft { Inner = factor },
                new LiteralRule("*"),
                new AssignRight { Inner = factor },
            };
            factor.Alternatives = new Rule[]
            {
                lit,
                parantheses
            };
            summand.Alternatives = new Rule[]
            {
                multiply,
                factor
            };
            lit.Regex = LitRegex();
            parantheses.Rules = new Rule[]
            {
                new LiteralRule("("),
                expr,
                new LiteralRule(")")
            };

            return expr;
        }

        [GeneratedRegex("^\\d+", RegexOptions.Compiled)]
        private static partial Regex LitRegex();

        [Test]
        public void Parser_CanParseSimpleArithmetics()
        {
            var parser = new Parser(CreateParseRule());
            Assert.IsNotNull(parser.Initialize(new string[] { "1 + 2 * (3 + 4)" }));
        }

        [Test]
        public void Parser_CreatesCorrectModel()
        {
            var parser = new Parser(CreateParseRule());
            var expr = parser.Initialize(new string[] { "1 + 2 * (3 + 4)" });
            Assert.That(expr, Is.InstanceOf<BinExpr>());
            var bin1 = (BinExpr)expr;
            AssertLit(1, bin1.Left);
            Assert.That(bin1.Op, Is.EqualTo(BinOp.Add));
            Assert.That(bin1.Right, Is.InstanceOf<BinExpr>());
            var bin2 = (BinExpr)bin1.Right;
            AssertLit(2, bin2.Left);
            Assert.That(bin2.Op, Is.EqualTo(BinOp.Mul));
            Assert.That(bin2.Right, Is.InstanceOf<BinExpr>());
            var bin3 = (BinExpr)bin2.Right;
            AssertLit(3, bin3.Left);
            AssertLit(4, bin3.Right);
            Assert.That(bin3.Op, Is.EqualTo(BinOp.Add));
        }

        [Test]
        public void Parser_ProcessesDeleteCorrectly()
        {
            var parser = new Parser(CreateParseRule());
            var expr1 = parser.Initialize(new string[] { "1 + 2 * (3 + 4)" });
            var expr2 = parser.Update(new[]
            {
                new TextEdit(new ParsePosition(0, 1), new ParsePosition(0,4), new string[] { "5" })
            });
            Assert.That(expr1, Is.Not.EqualTo(expr2));
            Assert.That(expr2, Is.InstanceOf<BinExpr>());
            var bin1 = (BinExpr)expr2;
            AssertLit(152, bin1.Left);
            Assert.That(bin1.Right, Is.InstanceOf<BinExpr>());
        }

        [Test]
        public void Parser_ReusesElements()
        {
            var parser = new Parser(CreateParseRule());
            var expr1 = parser.Initialize(new string[] { "1 + 2 * (3 + 4)" });
            var expr2 = parser.Update(new[]
            {
                new TextEdit(new ParsePosition(0, 10), new ParsePosition(0,13), new string[] { "5" })
            });
            Assert.That(expr1, Is.EqualTo(expr2));
            Assert.That(expr2, Is.InstanceOf<BinExpr>());
            var bin1 = (BinExpr)expr2;
            AssertLit(1, bin1.Left);
            Assert.That(bin1.Right, Is.InstanceOf<BinExpr>());
            var bin2 = (BinExpr)bin1.Right;
            AssertLit(354, bin2.Right);
        }

        [Test]
        public void Parser_RefusesIncorrectInput()
        {
            var parser = new Parser(CreateParseRule());
            var expr = parser.Initialize(new string[] { "1 + 2 * (3 + 4" });
            Assert.That(expr, Is.Null);
        }

        [Test]
        public void Parser_KeepsPreviousAfterFailingChange()
        {
            var parser = new Parser(CreateParseRule());
            var expr = parser.Initialize(new string[] { "1 + 2 * (3 + 4)" });
            Assert.That(expr, Is.Not.Null);
            var expr2 = parser.Update(new[]
            {
                new TextEdit(new ParsePosition(0,4), new ParsePosition(0,15), null)
            });
            Assert.That(expr, Is.EqualTo(expr2));
        }

        private static void AssertLit(int lit, Expr actual)
        {
            Assert.That(actual, Is.InstanceOf<Lit>());
            Assert.That(((Lit)actual).Val, Is.EqualTo(lit));
        }
    }
}
