using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Metamodel
{
    public class Grammar
    {
        public string Name { get; set; }

        public List<Rule> Rules { get; } = new List<Rule>();

        public ModelRule StartRule { get; }

        public List<MetamodelImport> Imports { get; } = new List<MetamodelImport> { };
    }

    public class MetamodelImport
    {
        public string File { get; set; }

        public string Prefix { get; set; }
    }

    public abstract class Rule
    {
        public string Name { get; set; }
    }

    public class ModelRule : Rule
    {
        public ParserExpression Expression { get; set; }
    }

    public class DataRule : Rule
    {
        public string Regex { get; set; }

        public string TypeName { get; set; }
    }

    public class InheritanceRule : Rule
    {
        public List<ModelRule> Subtypes { get; } = new List<ModelRule>();
    }

    public abstract class ParserExpression
    {

    }

    public class SequenceExpression : ParserExpression
    {
        public List<ParserExpression> InnerExpressions { get; } = new List<ParserExpression>();
    }

    public class UnaryParserExpression : ParserExpression
    {
        public ParserExpression Inner { get; set; }
    }

    public class PlusExpression : UnaryParserExpression
    {
    }

    public class StarExpression : UnaryParserExpression
    {
    }

    public class MaybeExpression : UnaryParserExpression
    {
    }

    public class KeywordExpression : ParserExpression
    {
        public string Keyword { get; set; }
    }

    public class ChoiceExpression : ParserExpression
    {
        public List<ParserExpression> Alternatives { get; } = new List<ParserExpression>();
    }

    public abstract class FeatureExpression : ParserExpression
    {
        public ParserExpression Assigned { get; set; }

        public string Feature { get; set; }

    }

    public class AssignExpression : FeatureExpression
    {
    }

    public class AddAssignExpression : FeatureExpression
    {
    }

    public class ExistsAssignExpression : FeatureExpression
    {
    }

    public class RuleExpression : ParserExpression
    {
        public Rule Referenced { get; set; }
    }

    public class ReferenceExpression : ParserExpression
    {
        public ModelRule ReferencedRule { get; set; }
    }
}
