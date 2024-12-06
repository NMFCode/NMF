using NMF.AnyText;
using NMF.AnyText.Grammars;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyText.Tests
{
    [TestFixture]
    class CommentChangeTests
    {
        [Test]
        public void CommentChange_RespondToLineAddition()
        {
            var anyText = new AnyTextGrammar();
            var parser = new Parser(new ModelParseContext(anyText));
            var grammar = @"grammar AnyText ( anytext )
root Grammar
imports https://github.com/NMFCode/NMF/AnyText

comment '//'
comment '/*' to '*/'

/*

this is a multiline comment

*/

Grammar:
  'grammar' Name=ID ( '(' LanguageId=ID ')' )? <nl> 'root' StartRule=[ClassRule] <nl> Imports+=MetamodelImport* <nl> Comments+=CommentRule <nl>* <nl> Rules+=Rule <nl>+;

CommentRule:
  MultilineCommentRule | SinglelineCommentRule;

SinglelineCommentRule:
  'comment' Start=Keyword;

MultilineCommentRule:
  'comment' Start=Keyword 'to' End=Keyword;

MetamodelImport:
  'imports' ( Prefix=ID 'from' )? File=Uri <nl>;

Rule:
  ClassRule | DataRule | FragmentRule | ParanthesisRule | EnumRule;

ClassRule:
  InheritanceRule | ModelRule;

InheritanceRule:
  Name=ID RuleTypeFragment ':' <nl> <ind> Subtypes+=[ClassRule] ( '|' Subtypes+=[ClassRule] )+ <nsp> ';' <unind> <nl>;

ModelRule:
  Name=ID RuleTypeFragment ':' <nl> <ind> Expression=ParserExpression <nsp> ';' <unind> <nl>;

DataRule:
  'terminal' Name=ID RuleTypeFragment ':' <nl> <ind> Regex=Regex ( 'surround' 'with' SurroundCharacter=Char )? ( 'escape' EscapeRules+=EscapeRule ( ',' EscapeRules+=EscapeRule )* )? <nsp> ';' <unind> <nl>;

EscapeRule:
  Character=Char 'as' Escape=Keyword;

FragmentRule:
  'fragment' Name=ID 'processes' ( Prefix=ID <nsp> '.' <nsp> )? TypeName=ID ':' <nl> <ind> Expression=ParserExpression <nsp> ';' <unind> <nl>;

ParanthesisRule:
  'parantheses' Name=ID ':' <nl> <ind> OpeningParanthesis=KeywordExpression InnerRule=[ClassRule] ClosingParanthesis=KeywordExpression <nsp> ';' <unind> <nl>;

EnumRule:
  'enum' Name=ID RuleTypeFragment ':' <nl> <ind> Literals+=LiteralRule+ <nsp> ';' <unind> <nl>;

LiteralRule:
  Literal=ID '=>' Keyword=Keyword <nl>;

fragment RuleTypeFragment processes Rule :
  ( 'returns' ( Prefix=ID <nsp> '.' <nsp> )? TypeName=ID )? <nsp>;

fragment FormattingInstructionFragment processes ParserExpression :
  FormattingInstructions+=FormattingInstruction*;

enum FormattingInstruction:
  Newline => '<nl>'
  Indent => '<ind>'
  Unindent => '<unind>'
  AvoidSpace => '<nsp>'
  ForbidSpace => '<!nsp>'
  ;

ParserExpression:
  ChoiceExpression | SequenceExpression | ConjunctiveParserExpression;

ConjunctiveParserExpression returns ParserExpression:
  PlusExpression | StarExpression | MaybeExpression | BasicParserExpression;

BasicParserExpression returns ParserExpression:
  NegativeLookaheadExpression | KeywordExpression | ReferenceExpression | AssignExpression | AddAssignExpression | ExistsAssignExpression | RuleExpression | ParanthesisExpression;

parantheses ParanthesisExpression :
  '(' ParserExpression ')';

SequenceExpression:
  InnerExpressions+=ConjunctiveParserExpression InnerExpressions+=ConjunctiveParserExpression+;

PlusExpression:
  Inner=BasicParserExpression <nsp> '+' FormattingInstructionFragment;

StarExpression:
  Inner=BasicParserExpression <nsp> '*' FormattingInstructionFragment;

MaybeExpression:
  Inner=BasicParserExpression <nsp> '?' FormattingInstructionFragment;

KeywordExpression:
  Keyword=Keyword FormattingInstructionFragment;

ChoiceExpression:
  ( Alternatives+=ConjunctiveParserExpression '|' )+ Alternatives+=ConjunctiveParserExpression;

AssignExpression:
  Feature=ID <nsp> '=' <nsp> Assigned=BasicParserExpression FormattingInstructionFragment;

AddAssignExpression:
  Feature=ID <nsp> '+=' <nsp> Assigned=BasicParserExpression FormattingInstructionFragment;

ExistsAssignExpression:
  Feature=ID <nsp> '?=' <nsp> Assigned=BasicParserExpression FormattingInstructionFragment;

NegativeLookaheadExpression:
  '!' <nsp> Inner=BasicParserExpression;

RuleExpression:
  Rule=[Rule] FormattingInstructionFragment !'='!'+='!'?=';

ReferenceExpression:
  '[' <nsp> ReferencedRule=[Rule] <nsp> ']';

terminal ID:
  /[a-zA-Z]\w*/;

terminal Keyword:
  /(\\\\|\\'|[^'\\])+/ surround with ' escape \ as '\\\\' , ' as '\\\'';

terminal Regex:
  /(\\\/|[^\/])*/ surround with / escape / as '\\/';

terminal Uri:
  /.+/;

terminal Char:
  /\S/;

";

            var parsed = parser.Initialize(SplitIntoLines(grammar));
            Assert.IsNotNull(parsed);
            Assert.That(parser.Context.Errors, Is.Empty);

            parser.Update([new TextEdit(new ParsePosition(10, 0), new ParsePosition(10, 0), ["\r\n"])]);
            Assert.IsNotNull(parsed);
            Assert.That(parser.Context.Errors, Is.Empty);
            //var examinedUpdate = new ParsePositionDelta(13, 7);
            //AssertAtLeast(parser.Context.RootRuleApplication.ExaminedTo, examinedUpdate);
        }

        [Test]
        public void CommentChange_RespondToLineRemoval()
        {
            var anyText = new AnyTextGrammar();
            var parser = new Parser(new ModelParseContext(anyText));
            var grammar = @"grammar AnyText ( anytext )
root Grammar
imports https://github.com/NMFCode/NMF/AnyText

comment '//'
comment '/*' to '*/'

/*

this is a multiline comment

*/

Grammar:
  'grammar' Name=ID ( '(' LanguageId=ID ')' )? <nl> 'root' StartRule=[ClassRule] <nl> Imports+=MetamodelImport* <nl> Comments+=CommentRule <nl>* <nl> Rules+=Rule <nl>+;

CommentRule:
  MultilineCommentRule | SinglelineCommentRule;

SinglelineCommentRule:
  'comment' Start=Keyword;

MultilineCommentRule:
  'comment' Start=Keyword 'to' End=Keyword;

MetamodelImport:
  'imports' ( Prefix=ID 'from' )? File=Uri <nl>;

Rule:
  ClassRule | DataRule | FragmentRule | ParanthesisRule | EnumRule;

ClassRule:
  InheritanceRule | ModelRule;

InheritanceRule:
  Name=ID RuleTypeFragment ':' <nl> <ind> Subtypes+=[ClassRule] ( '|' Subtypes+=[ClassRule] )+ <nsp> ';' <unind> <nl>;

ModelRule:
  Name=ID RuleTypeFragment ':' <nl> <ind> Expression=ParserExpression <nsp> ';' <unind> <nl>;

DataRule:
  'terminal' Name=ID RuleTypeFragment ':' <nl> <ind> Regex=Regex ( 'surround' 'with' SurroundCharacter=Char )? ( 'escape' EscapeRules+=EscapeRule ( ',' EscapeRules+=EscapeRule )* )? <nsp> ';' <unind> <nl>;

EscapeRule:
  Character=Char 'as' Escape=Keyword;

FragmentRule:
  'fragment' Name=ID 'processes' ( Prefix=ID <nsp> '.' <nsp> )? TypeName=ID ':' <nl> <ind> Expression=ParserExpression <nsp> ';' <unind> <nl>;

ParanthesisRule:
  'parantheses' Name=ID ':' <nl> <ind> OpeningParanthesis=KeywordExpression InnerRule=[ClassRule] ClosingParanthesis=KeywordExpression <nsp> ';' <unind> <nl>;

EnumRule:
  'enum' Name=ID RuleTypeFragment ':' <nl> <ind> Literals+=LiteralRule+ <nsp> ';' <unind> <nl>;

LiteralRule:
  Literal=ID '=>' Keyword=Keyword <nl>;

fragment RuleTypeFragment processes Rule :
  ( 'returns' ( Prefix=ID <nsp> '.' <nsp> )? TypeName=ID )? <nsp>;

fragment FormattingInstructionFragment processes ParserExpression :
  FormattingInstructions+=FormattingInstruction*;

enum FormattingInstruction:
  Newline => '<nl>'
  Indent => '<ind>'
  Unindent => '<unind>'
  AvoidSpace => '<nsp>'
  ForbidSpace => '<!nsp>'
  ;

ParserExpression:
  ChoiceExpression | SequenceExpression | ConjunctiveParserExpression;

ConjunctiveParserExpression returns ParserExpression:
  PlusExpression | StarExpression | MaybeExpression | BasicParserExpression;

BasicParserExpression returns ParserExpression:
  NegativeLookaheadExpression | KeywordExpression | ReferenceExpression | AssignExpression | AddAssignExpression | ExistsAssignExpression | RuleExpression | ParanthesisExpression;

parantheses ParanthesisExpression :
  '(' ParserExpression ')';

SequenceExpression:
  InnerExpressions+=ConjunctiveParserExpression InnerExpressions+=ConjunctiveParserExpression+;

PlusExpression:
  Inner=BasicParserExpression <nsp> '+' FormattingInstructionFragment;

StarExpression:
  Inner=BasicParserExpression <nsp> '*' FormattingInstructionFragment;

MaybeExpression:
  Inner=BasicParserExpression <nsp> '?' FormattingInstructionFragment;

KeywordExpression:
  Keyword=Keyword FormattingInstructionFragment;

ChoiceExpression:
  ( Alternatives+=ConjunctiveParserExpression '|' )+ Alternatives+=ConjunctiveParserExpression;

AssignExpression:
  Feature=ID <nsp> '=' <nsp> Assigned=BasicParserExpression FormattingInstructionFragment;

AddAssignExpression:
  Feature=ID <nsp> '+=' <nsp> Assigned=BasicParserExpression FormattingInstructionFragment;

ExistsAssignExpression:
  Feature=ID <nsp> '?=' <nsp> Assigned=BasicParserExpression FormattingInstructionFragment;

NegativeLookaheadExpression:
  '!' <nsp> Inner=BasicParserExpression;

RuleExpression:
  Rule=[Rule] FormattingInstructionFragment !'='!'+='!'?=';

ReferenceExpression:
  '[' <nsp> ReferencedRule=[Rule] <nsp> ']';

terminal ID:
  /[a-zA-Z]\w*/;

terminal Keyword:
  /(\\\\|\\'|[^'\\])+/ surround with ' escape \ as '\\\\' , ' as '\\\'';

terminal Regex:
  /(\\\/|[^\/])*/ surround with / escape / as '\\/';

terminal Uri:
  /.+/;

terminal Char:
  /\S/;

";

            var parsed = parser.Initialize(SplitIntoLines(grammar));
            Assert.IsNotNull(parsed);
            Assert.That(parser.Context.Errors, Is.Empty);

            parser.Update([new TextEdit(new ParsePosition(9, 27), new ParsePosition(10, 0), [""])]);
            Assert.IsNotNull(parsed);
            Assert.That(parser.Context.Errors, Is.Empty);
            //var examinedUpdate = new ParsePositionDelta(13, 7);
            //AssertAtLeast(parser.Context.RootRuleApplication.ExaminedTo, examinedUpdate);
        }

        private static string[] SplitIntoLines(string grammar)
        {
            var lines = grammar.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].TrimEnd('\r');
            }
            return lines;
        }

        private static void AssertAtLeast(ParsePositionDelta actual, ParsePositionDelta expected)
        {
            Assert.That(actual.Line, Is.AtLeast(expected.Line));
            if (actual.Line == expected.Line)
            {
                Assert.That(actual.Col, Is.AtLeast(expected.Col));
            }
        }
    }
}
