using NMF.AnyText;
using NMF.AnyText.Grammars;
using NMF.AnyText.Model;
using NMF.AnyText.PrettyPrinting;
using NMF.AnyText.Rules;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AnyText.Tests.Languages
{
    [TestFixture]
    public partial class ExpressionGrammarTests
    {
        public interface IExpr { }

        public class BinExpr : IExpr
        {
            public IExpr? Left { get; set; }
            public IExpr? Right { get; set; }

            public BinOp Op { get; set; }
        }

        public enum BinOp
        {
            Add,
            Mul
        }

        public class Lit : IExpr
        {
            public int Val { get; set; }
        }

        private class AddRule : ElementRule<BinExpr>
        {
            protected override BinExpr CreateElement(IEnumerable<RuleApplication> inner)
            {
                return new BinExpr { Op = BinOp.Add };
            }

            public override bool CanSynthesize(object semanticElement, ParseContext context, SynthesisPlan synthesisPlan)
            {
                return semanticElement is BinExpr binExpr && binExpr.Op == BinOp.Add;
            }
        }

        private class MulRule : ElementRule<BinExpr>
        {
            protected override BinExpr CreateElement(IEnumerable<RuleApplication> inner)
            {
                return new BinExpr { Op = BinOp.Mul };
            }

            public override bool CanSynthesize(object semanticElement, ParseContext context, SynthesisPlan synthesisPlan)
            {
                return semanticElement is BinExpr bin && bin.Op == BinOp.Mul;
            }
        }

        private class ConvertToLitRule : ConvertRule<Lit>
        {
            public override Lit Convert(string text, ParseContext context)
            {
                return new Lit { Val = int.Parse(text) };
            }

            public override string ConvertToString(Lit semanticElement, ParseContext context)
            {
                return semanticElement.Val.ToString();
            }
        }

        private class AssignLeft : AssignRule<BinExpr, IExpr?>
        {
            protected override string Feature => "Left";

            protected override IExpr? GetValue(BinExpr semanticElement, ParseContext context)
            {
                return semanticElement.Left;
            }

            protected override void SetValue(BinExpr semanticElement, IExpr? propertyValue, ParseContext context)
            {
                semanticElement.Left = propertyValue;
            }
        }

        private class AssignRight : AssignRule<BinExpr, IExpr?>
        {
            protected override string Feature => "Right";

            protected override IExpr? GetValue(BinExpr semanticElement, ParseContext context)
            {
                return semanticElement.Right;
            }

            protected override void SetValue(BinExpr semanticElement, IExpr? propertyValue, ParseContext context)
            {
                semanticElement.Right = propertyValue;
            }
        }

        private static AdHocGrammar CreateParseGrammar()
        {
            var expr = new ChoiceRule();
            var add = new AddRule();
            var multiply = new MulRule();
            var multiplicative = new ChoiceRule();
            var simple = new ChoiceRule();
            var lit = new ConvertToLitRule();
            var parantheses = new ParanthesesRule();

            var al1 = new AssignLeft { Inner = expr };
            var ar1 = new AssignRight { Inner = multiplicative };
            var al2 = new AssignLeft { Inner = multiplicative };
            var ar2 = new AssignRight { Inner = simple };

            expr.Alternatives =
            [
                add,
                multiplicative
            ];
            add.Rules =
            [
                al1,
                new LiteralRule("+"),
                ar1,
            ];
            multiply.Rules =
            [
                al2,
                new LiteralRule("*"),
                ar2,
            ];
            multiplicative.Alternatives =
            [
                multiply,
                simple
            ];
            simple.Alternatives =
            [
                lit,
                parantheses
            ];
            lit.Regex = LitRegex();
            parantheses.Rules =
            [
                new LiteralRule("("),
                expr,
                new LiteralRule(")")
            ];

            return new AdHocGrammar(expr, new Rule[]
            {
                add,
                multiply,
                multiplicative,
                lit,
                simple,
                parantheses,
                al1,
                al2,
                ar1,
                ar2
            });
        }

        [GeneratedRegex("^\\d+", RegexOptions.Compiled)]
        private static partial Regex LitRegex();

        [Test]
        public void Parser_CanParseSimpleArithmetics()
        {
            var parser = new Parser(CreateParseGrammar());
            Assert.That(parser.Initialize(new string[] { "1 + 2 * (3 + 4)" }), Is.Not.Null);
        }

        [Test]
        public void Parser_CanParseLeftRecursion()
        {
            var parser = new Parser(CreateParseGrammar());
            var result = parser.Initialize(new string[] { "1 + 2 + 3 + 4" });
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void Parser_CreatesCorrectModel()
        {
            var parser = new Parser(CreateParseGrammar());
            var expr = parser.Initialize(new string[] { "1 + 2 * (3 + 4)" });
            Assert.That(expr, Is.InstanceOf<BinExpr>());
            var bin1 = (BinExpr)expr;
            AssertLit(1, bin1.Left!);
            Assert.That(bin1.Op, Is.EqualTo(BinOp.Add));
            Assert.That(bin1.Right, Is.InstanceOf<BinExpr>());

            var bin2 = (BinExpr)bin1.Right;
            AssertLit(2, bin2!.Left!);
            Assert.That(bin2.Op, Is.EqualTo(BinOp.Mul));
            Assert.That(bin2.Right, Is.InstanceOf<BinExpr>());

            var bin3 = (BinExpr)bin2.Right;
            AssertLit(3, bin3!.Left!);
            AssertLit(4, bin3.Right!);
            Assert.That(bin3.Op, Is.EqualTo(BinOp.Add));
        }

        [Test]
        public void Parser_ProcessesDeleteCorrectly()
        {
            var parser = new Parser(CreateParseGrammar());
            var expr1 = parser.Initialize(new string[] { "1 + 2 * (3 + 4)" });
            var expr2 = parser.Update(new[]
            {
                new TextEdit(new ParsePosition(0, 1), new ParsePosition(0,4), new string[] { "5" })
            });
            Assert.That(expr1, Is.Not.EqualTo(expr2));
            Assert.That(expr2, Is.InstanceOf<BinExpr>());
            var bin1 = (BinExpr)expr2;
            AssertLit(152, bin1.Left!);
            Assert.That(bin1.Right, Is.InstanceOf<BinExpr>());
        }

        [Test]
        public void Parser_ProcessesLineInsertionCorrectly()
        {
            var parser = new Parser(CreateParseGrammar());
            var expr1 = parser.Initialize(new string[] { "1 + 2 * (3 + 4)" });
            var expr2 = parser.Update(new[]
            {
                new TextEdit(new ParsePosition(0, 1), new ParsePosition(0,2), new string[] { string.Empty, string.Empty, string.Empty })
            });
            Assert.That(expr1, Is.EqualTo(expr2));
            var updatedInput = parser.Context.Input;
            Assert.That(updatedInput.Length, Is.EqualTo(3));
            Assert.That(updatedInput[0], Is.EqualTo("1"));
            Assert.That(updatedInput[1], Is.EqualTo(string.Empty));
            Assert.That(updatedInput[2], Is.EqualTo("+ 2 * (3 + 4)"));
        }

        [Test]
        public void Parser_ProcessesLineDeletionCorrectly()
        {
            var parser = new Parser(CreateParseGrammar());
            var expr1 = parser.Initialize(new string[] { "1", string.Empty, "+ 2 * (3 + 4)" });
            var expr2 = parser.Update(new[]
            {
                new TextEdit(new ParsePosition(0, 1), new ParsePosition(2,0), new string[] { " " })
            });
            Assert.That(expr1, Is.EqualTo(expr2));
            var updatedInput = parser.Context.Input;
            Assert.That(updatedInput.Length, Is.EqualTo(1));
            Assert.That(updatedInput[0], Is.EqualTo("1 + 2 * (3 + 4)"));
        }

        [Test]
        public void Parser_ReusesElements()
        {
            var parser = new Parser(CreateParseGrammar());
            var expr1 = parser.Initialize(new string[] { "1 + 2 * (3 + 4)" });
            var expr2 = parser.Update(new[]
            {
                new TextEdit(new ParsePosition(0, 10), new ParsePosition(0,13), new string[] { "5" })
            });
            Assert.That(expr1, Is.EqualTo(expr2));
            Assert.That(expr2, Is.InstanceOf<BinExpr>());
            var bin1 = (BinExpr)expr2;
            AssertLit(1, bin1.Left!);
            Assert.That(bin1.Right, Is.InstanceOf<BinExpr>());
            var bin2 = (BinExpr)bin1.Right;
            AssertLit(354, bin2!.Right!);
        }

        [Test]
        public void Parser_RefusesIncorrectInput()
        {
            var parser = new Parser(CreateParseGrammar());
            var expr = parser.Initialize(new string[] { "1 + 2 * (3 + 4" });
            Assert.That(expr, Is.Null);
        }

        [Test]
        public void Parser_KeepsPreviousAfterFailingChange()
        {
            var parser = new Parser(CreateParseGrammar());
            var expr = parser.Initialize(new string[] { "1 + 2 * (3 + 4)" });
            Assert.That(expr, Is.Not.Null);
            var expr2 = parser.Update(new[]
            {
                new TextEdit(new ParsePosition(0,4), new ParsePosition(0,15), null)
            });
            Assert.That(expr, Is.EqualTo(expr2));
            Assert.That(parser.Context.Errors, Is.Not.Empty);
        }

        [Test]
        public void Parser_CanSynthesizeString()
        {
            var expression = new BinExpr
            {
                Left = new BinExpr
                {
                    Left = new Lit { Val = 1 },
                    Op = BinOp.Add,
                    Right = new Lit { Val = 2 }
                },
                Op = BinOp.Mul,
                Right = new BinExpr
                {
                    Left = new Lit { Val = 3 },
                    Op = BinOp.Add,
                    Right = new Lit { Val = 4 }
                }
            };
            var grammar = CreateParseGrammar();
            var parser = new Parser(grammar);
            var addRule = grammar.Rules.OfType<MulRule>().First();
            var synthesized = addRule.Synthesize(expression, parser.Context);
            Assert.That(synthesized, Is.EqualTo("( 1 + 2 ) * ( 3 + 4 )"));
        }

        private static void AssertLit(int lit, IExpr actual)
        {
            Assert.That(actual, Is.InstanceOf<Lit>());
            Assert.That(((Lit)actual).Val, Is.EqualTo(lit));
        }
    }
}
