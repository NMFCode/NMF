using NMF.AnyText.Grammars;
using NMF.AnyText.Metamodel;
using NMF.AnyText.Model;
using NMF.AnyText.Rules;
using NMF.Models;
using NMF.Models.Meta;
using NMF.Transformations;
using NMF.Transformations.Core;
using NMF.Utilities;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using NMF.CodeGen;
using System.Reflection;
using NMF.AnyText.PrettyPrinting;
using FormattingInstruction = NMF.AnyText.Metamodel.FormattingInstruction;

namespace NMF.AnyText.Transformation
{
#pragma warning disable S3265 // Non-flags enums should not be used in bitwise operations
    internal class AnytextCodeGenerator : ReflectiveTransformation
    {
        private static CodeTypeReference CreateReference(IRule rule, bool interfaceType, ITransformationContext context)
        {
            if (rule is IDataRule) interfaceType = false;
            var type = CodeGenerator._trace.LookupType(rule);
            if (type != null)
            {
                var mappedType = type.GetExtension<MappedType>();
                if (mappedType != null)
                {
                    var typeReference = mappedType.SystemType.ToTypeReference();
                    if (!interfaceType && mappedType.SystemType.IsInterface)
                    {
                        typeReference.BaseType = typeReference.BaseType.Substring(1);
                    }
                    return typeReference;
                }
                return new CodeTypeReference((interfaceType ? "I" : string.Empty) + type.Name.ToPascalCase());
            }
            if (rule is IParanthesisRule paranthesisRule)
            {
                return CreateReference(paranthesisRule.InnerRule, interfaceType, context);
            }
            if (rule.TypeName != null)
            {
                return new CodeTypeReference((interfaceType ? "I" : string.Empty) + rule.TypeName);
            }
            if (rule is IDataRule)
            {
                return new CodeTypeReference(typeof(string));
            }
            return new CodeTypeReference((interfaceType ? "I" : string.Empty) + rule.Name);
        }
        private static CodeTypeReference CreateRuleReference(IRule rule, RuleToClass ruleTransformatio, ITransformationContext context)
        {
            var generatedClass = context.Trace.ResolveIn(ruleTransformatio, rule);
            return new CodeTypeReference(generatedClass.Name);
        }

        private static CodeTypeReference CreateAssignmentReference(IFeatureExpression assignExpression, AssignmentToClass assignTransformation, ITransformationContext context)
        {
            var generatedClass = context.Trace.ResolveIn(assignTransformation, assignExpression);
            var lookupResult = CodeGenerator._trace.LookupFeature(assignExpression);
            var debugString = GetNameString(assignExpression.Assigned);
            if (generatedClass == null)
            {
                generatedClass = context.Trace.ResolveInWhere(assignTransformation, feat => CodeGenerator._trace.LookupFeature(feat) == lookupResult && GetNameString(feat.Assigned) == debugString).First();
            }
            return new CodeTypeReference(generatedClass.Name);
        }

        private static CodeMemberMethod CreateInitializeMethod()
        {
            var init = new CodeMemberMethod
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Override,
                Name = nameof(AnyText.Rules.Rule.Initialize),
                Parameters =
                {
                    new CodeParameterDeclarationExpression
                    {
                        Name = "context",
                        Type = typeof(GrammarContext).ToTypeReference()
                    }
                }
            };
            init.WriteDocumentation("Initializes the current grammar rule", null, new Dictionary<string, string>
            {
                ["context"] = "the grammar context in which the rule is initialized"
            }, "Do not modify the contents of this method as it will be overridden as the contents of the AnyText file change.");
            return init;
        }

        private static CodeExpression _contextRef = new CodeArgumentReferenceExpression("context");

        private static CodeExpression CreateResolveKeyword(string keyword, IEnumerable<FormattingInstruction> formattingInstructions)
        {
            var unescaped = keyword?.Replace(@"\'", "'");
            var res = new CodeMethodInvokeExpression(_contextRef, nameof(GrammarContext.ResolveKeyword), new CodePrimitiveExpression(unescaped));
            if (formattingInstructions != null)
            {
                foreach (var instruction in formattingInstructions)
                {
                    res.Parameters.Add(CreateFormattingInstruction(instruction));
                }
            }
            return res;
        }

        private static CodeExpression CreateResolveRule(CodeTypeReference ruleType, IEnumerable<FormattingInstruction> formattingInstructions)
        {
            if (formattingInstructions == null)
            {
                return new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(_contextRef, nameof(GrammarContext.ResolveRule), ruleType));
            }
            else
            {
                var invoke = new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(_contextRef, nameof(GrammarContext.ResolveFormattedRule), ruleType));
                return AddFormattingInstructions(invoke, formattingInstructions);
            }
        }

        private static IFeatureExpression GetIdAssignment(IClassRule rule)
        {
            if (rule is IModelRule modelRule)
            {
                return modelRule.Descendants().OfType<IFeatureExpression>()
                    .FirstOrDefault(ft => string.Equals("Name", ft.Feature, StringComparison.OrdinalIgnoreCase));
            }
            else if (rule is IInheritanceRule inheritanceRule)
            {
                return GetIdAssignment(inheritanceRule.Subtypes[0]);
            }
            throw new NotSupportedException();
        }

        private static CodeTypeReferenceExpression ruleFormatterReference = new CodeTypeReferenceExpression(typeof(RuleFormatter).ToTypeReference());

        private static CodeExpression CreateParserExpression(IParserExpression parserExpression, IEnumerable<FormattingInstruction> formattingInstructions, RuleToClass ruleTransformation, AssignmentToClass assignTransformation, ITransformationContext context)
        {
            switch (parserExpression)
            {
                case IKeywordExpression keywordExpression:
                    return CreateResolveKeyword(keywordExpression.Keyword, formattingInstructions);
                case IReferenceExpression referenceExpression:
                    if (referenceExpression.Format != null)
                    {
                        return CreateResolveRule(CreateRuleReference(referenceExpression.Format, ruleTransformation, context), formattingInstructions);
                    }
                    var assignment = GetIdAssignment(referenceExpression.ReferencedRule);
                    return CreateParserExpression(assignment.Assigned, formattingInstructions, ruleTransformation, assignTransformation, context);
                case IRuleExpression ruleExpression:
                    return CreateResolveRule(CreateRuleReference(ruleExpression.Rule, ruleTransformation, context), formattingInstructions);
                case IFeatureExpression assignExpression:
                    return CreateResolveRule(CreateAssignmentReference(assignExpression, assignTransformation, context), formattingInstructions);
                case IMaybeExpression maybe:
                    return AddFormattingInstructions(new CodeMethodInvokeExpression(ruleFormatterReference, nameof(RuleFormatter.ZeroOrOne), CreateParserExpression(maybe.Inner, maybe.FormattingInstructions, ruleTransformation, assignTransformation, context)), formattingInstructions);
                case IPlusExpression plus:
                    return AddFormattingInstructions(new CodeMethodInvokeExpression(ruleFormatterReference, nameof(RuleFormatter.OneOrMore), CreateParserExpression(plus.Inner, plus.FormattingInstructions, ruleTransformation, assignTransformation, context)), formattingInstructions);
                case IStarExpression star:
                    return AddFormattingInstructions(new CodeMethodInvokeExpression(ruleFormatterReference, nameof(RuleFormatter.ZeroOrMore), CreateParserExpression(star.Inner, star.FormattingInstructions, ruleTransformation, assignTransformation, context)), formattingInstructions);
                case ISequenceExpression sequence:
                    var sequenceExpression = new CodeObjectCreateExpression(typeof(SequenceRule).ToTypeReference());
                    foreach (var item in sequence.InnerExpressions)
                    {
                        sequenceExpression.Parameters.Add(CreateParserExpression(item.Expression, item.FormattingInstructions, ruleTransformation, assignTransformation, context));
                    }
                    return sequenceExpression;
                case IChoiceExpression choice:
                    var choiceExpression = new CodeObjectCreateExpression(typeof(ChoiceRule).ToTypeReference());
                    foreach (var item in choice.Alternatives)
                    {
                        choiceExpression.Parameters.Add(CreateParserExpression(item.Expression, item.FormattingInstructions, ruleTransformation, assignTransformation, context));
                    }
                    return choiceExpression;
                case INegativeLookaheadExpression negative:
                    return new CodeObjectCreateExpression(typeof(NegativeLookaheadRule).ToTypeReference(), CreateParserExpression(negative.Inner, negative.FormattingInstructions, ruleTransformation, assignTransformation, context));
            }
            throw new NotSupportedException();
        }

        private static CodeMethodInvokeExpression AddFormattingInstructions(CodeMethodInvokeExpression createRuleExpression, IEnumerable<FormattingInstruction> formattingInstructions)
        {
            foreach (var instruction in formattingInstructions)
            {
                createRuleExpression.Parameters.Add(CreateFormattingInstruction(instruction));
            }
            return createRuleExpression;
        }

        private static CodeExpression CreateFormattingInstruction(FormattingInstruction formattingInstruction)
        {
            switch (formattingInstruction)
            {
                case FormattingInstruction.Indent:
                    return new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(PrettyPrinting.FormattingInstruction).ToTypeReference()), nameof(PrettyPrinting.FormattingInstruction.Indent));
                case FormattingInstruction.Unindent:
                    return new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(PrettyPrinting.FormattingInstruction).ToTypeReference()), nameof(PrettyPrinting.FormattingInstruction.Unindent));
                case FormattingInstruction.Newline:
                    return new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(PrettyPrinting.FormattingInstruction).ToTypeReference()), nameof(PrettyPrinting.FormattingInstruction.Newline));
                case FormattingInstruction.ForbidSpace:
                case FormattingInstruction.AvoidSpace:
                    return new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(PrettyPrinting.FormattingInstruction).ToTypeReference()), nameof(PrettyPrinting.FormattingInstruction.SupressSpace));
                default:
                    throw new ArgumentOutOfRangeException(nameof(formattingInstruction));
            }
        }

        private static CodeTypeReference SynthesizeType(IParserExpression expression, RuleToClass ruleToClass, ITransformationContext context)
        {
            switch (expression)
            {
                case IRuleExpression ruleExpression:
                    return CreateReference(ruleExpression.Rule, true, context);
                case IUnaryParserExpression unary:
                    return SynthesizeType(unary.Inner, ruleToClass, context);
                case IChoiceExpression choice:
                    return SynthesizeType(choice.Alternatives[0].Expression, ruleToClass, context);
                case ISequenceExpression sequence:
                    return SynthesizeType(sequence.InnerExpressions[0].Expression, ruleToClass, context);
                case IFeatureExpression feature:
                    return SynthesizeType(feature.Assigned, ruleToClass, context);
                case IKeywordExpression:
                    return new CodeTypeReference(typeof(string));

            }
            return new CodeTypeReference(typeof(string));
        }

        private static (CodeTypeReference semanticType, CodeTypeReference propertyType) GetSemanticTypeForFeature(IFeatureExpression input, ITransformationContext context)
        {
            var feature = CodeGenerator._trace.LookupFeature(input);
            var cl = feature?.Parent as IType;
            if (cl != null)
            {
                var semanticType = new CodeTypeReference("I" + cl.Name.ToPascalCase());
                var mappedSemanticType = cl.GetExtension<MappedType>();
                if (mappedSemanticType != null)
                {
                    semanticType = mappedSemanticType.SystemType.ToTypeReference();
                }
                return (semanticType, GetElementType(feature));
            }
            var modelRule = input.Ancestors().OfType<IModelRule>().FirstOrDefault();
            if (modelRule != null)
            {
                var semanticType = CreateReference(modelRule, true, context);
                return (semanticType, SynthesizeType(input.Assigned, null, context));
            }
            var fragment = input.Ancestors().OfType<IFragmentRule>().FirstOrDefault();
            if (fragment != null)
            {
                var semanticType = CreateReference(fragment, true, context);
                return (semanticType, SynthesizeType(input.Assigned, null, context));
            }
            throw new NotSupportedException();
        }

        private static CodeTypeReference GetElementType(ITypedElement feature)
        {
            if (feature.Type == null)
            {
                if (feature is IReference)
                {
                    return typeof(IModelElement).ToTypeReference();
                }
                else
                {
                    return new CodeTypeReference(typeof(string));
                }
            }
            var mappedType = feature.Type.GetExtension<MappedType>();
            if (mappedType != null)
            {
                var elementType = mappedType.SystemType.ToTypeReference();
                if (feature.LowerBound == 0 && feature.UpperBound == 1 && mappedType.SystemType.IsValueType)
                {
                    elementType = new CodeTypeReference(typeof(Nullable<>).Name, elementType);
                }
                return elementType;
            }
            if (feature.Type is IPrimitiveType primitiveType)
            {
                var elementType = new CodeTypeReference(primitiveType.SystemType);
                if (feature.LowerBound == 0 && feature.UpperBound == 1 && IsValueType(primitiveType))
                {
                    elementType = new CodeTypeReference(typeof(Nullable<>).Name, elementType);
                }
                return elementType;
            }
            return CreateDefaultReference(feature);
        }

        private static bool IsValueType(IPrimitiveType primitiveType)
        {
            return primitiveType.SystemType != "System.String";
        }

        private static CodeTypeReference CreateDefaultReference(ITypedElement feature)
        {
            var name = feature.Type.Name.ToPascalCase();
            if (feature is IReference)
            {
                name = "I" + name;
            }
            return new CodeTypeReference(name);
        }

        public class GrammarToNamespace : NamespaceGenerator<IGrammar>
        {
            public override IEnumerable<string> DefaultImports
            {
                get
                {
                    return CodeGenerator._settings?.ImportedNamespaces ?? Enumerable.Empty<string>();
                }
            }

            protected override string GetName(IGrammar input)
            {
                return CodeGenerator._settings?.Namespace ?? "Generated";
            }

            protected override IEnumerable<Assembly> AssembliesToCheck
            {
                get
                {
                    yield return typeof(ReflectiveGrammar).Assembly;
                    yield return typeof(Parser).Assembly;
                }
            }

            public override void RegisterDependencies()
            {
                RequireType(Rule<GrammarToClass>(), g => g);
            }
        }

        private static string GetNameString(IParserExpression expression)
        {
            switch (expression)
            {
                case IRuleExpression ruleExp:
                    return ruleExp.Rule.Name.ToPascalCase();
                case IChoiceExpression choice:
                    return string.Join("Or", choice.Alternatives.Select(a => GetNameString(a.Expression)));
                case ISequenceExpression sequence:
                    return string.Join("Then", sequence.InnerExpressions.Select(a => GetNameString(a.Expression)));
                case IStarExpression star:
                    return "Many" + GetNameString(star.Inner);
                case IMaybeExpression maybe:
                    return "Optional" + GetNameString(maybe.Inner);
                case IPlusExpression plus:
                    return "Plus" + GetNameString(plus.Inner);
                case IKeywordExpression keyword:
                    return keyword.Keyword.ToPascalCase();
                case IFeatureExpression feature:
                    return GetNameString(feature.Assigned);
                case IReferenceExpression reference:
                    return reference.ReferencedRule.Name.ToPascalCase();
                default:
                    throw new NotImplementedException();
            }
        }

        public class GrammarToClass : TransformationRule<IGrammar, CodeTypeDeclaration>
        {
            public override void Transform(IGrammar input, CodeTypeDeclaration output, ITransformationContext context)
            {
                output.Name = input.Name.ToPascalCase() + "Grammar";
                output.IsPartial = true;
                output.BaseTypes.Add(typeof(ReflectiveGrammar).ToTypeReference());
                output.WriteDocumentation($"Denotes a class capable to parse the language {input.LanguageId}");

                var languageId = new CodeMemberProperty
                {
                    Name = nameof(Grammars.Grammar.LanguageId),
                    Type = new CodeTypeReference(typeof(string)),
                    Attributes = MemberAttributes.Public | MemberAttributes.Override,
                    HasGet = true,
                    HasSet = false
                };
                languageId.WriteDocumentation("Gets the language id for this grammar");
                languageId.GetStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(input.LanguageId)));
                output.Members.Add(languageId);

                if (input.StartRule != null)
                {
                    var getRootRule = new CodeMemberMethod
                    {
                        Name = "GetRootRule",
                        Attributes = MemberAttributes.Family | MemberAttributes.Override,
                        ReturnType = typeof(Rules.Rule).ToTypeReference(),
                        Parameters =
                        {
                            new CodeParameterDeclarationExpression(typeof(GrammarContext).ToTypeReference(), "context")
                        }
                    };
                    getRootRule.Statements.Add(new CodeMethodReturnStatement(CreateResolveRule(CreateRuleReference(input.StartRule, Rule<RuleToClass>(), context), null)));
                    getRootRule.WriteDocumentation("Gets the root rule", "the root rule for this grammar", new Dictionary<string, string>
                    {
                        ["context"] = "a context to resolve the root rule"
                    });
                    output.Members.Add(getRootRule);
                }
            }

            public override void RegisterDependencies()
            {
                CallMany(Rule<RuleToClass>(), g => g.Rules, AddChildClasses);
                CallMany(Rule<AssignmentToClass>(),
                    GetAssignments,
                    AddChildClasses);
                CallMany(Rule<CommentToClass>(), g => g.Comments, AddChildClasses);
            }

            private static IEnumerable<IFeatureExpression> GetAssignments(IGrammar grammar)
            {
                return grammar.Rules
                   .OfType<IModelRule>().Concat<IRule>(grammar.Rules.OfType<IFragmentRule>())
                   .SelectMany(r => r.Descendants().OfType<IFeatureExpression>())
                   .DistinctBy(f => (CodeGenerator._trace.LookupFeature(f), GetNameString(f.Assigned)));
            }

            private static void AddChildClasses(CodeTypeDeclaration cl, IEnumerable<CodeTypeDeclaration> rules)
            {
                cl.Members.AddRange(rules.ToArray());
            }
        }

        public class CommentToClass : AbstractTransformationRule<ICommentRule, CodeTypeDeclaration>
        {
            public override void Transform(ICommentRule input, CodeTypeDeclaration output, ITransformationContext context)
            {
                output.Attributes = MemberAttributes.Public;
                output.IsClass = true;
                output.IsPartial = true;
            }
        }

        public class SinglelineCommentToClass : TransformationRule<ISinglelineCommentRule, CodeTypeDeclaration>
        {
            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<CommentToClass>());
            }

            public override void Transform(ISinglelineCommentRule input, CodeTypeDeclaration output, ITransformationContext context)
            {
                if (input.Parent is IGrammar grammar)
                {
                    var index = grammar.Comments.OfType<ISinglelineCommentRule>().ToList().IndexOf(input);
                    output.Name = $"SinglelineComment{index + 1}Rule";
                }

                output.BaseTypes.Add(typeof(Rules.CommentRule).ToTypeReference());
                output.WriteDocumentation($"Denotes a rule to parse single-line comments starting with '{input.Start}'");

                var init = CreateInitializeMethod();
                init.Statements.Add(new CodeAssignStatement(
                    new CodePropertyReferenceExpression(null, nameof(AnyText.Rules.CommentRule.CommentStart)),
                    new CodePrimitiveExpression(input.Start)));
                output.Members.Add(init);
            }
        }

        public class MultilineCommentToClass : TransformationRule<IMultilineCommentRule, CodeTypeDeclaration>
        {
            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<CommentToClass>());
            }

            public override void Transform(IMultilineCommentRule input, CodeTypeDeclaration output, ITransformationContext context)
            {
                if (input.Parent is IGrammar grammar)
                {
                    var index = grammar.Comments.OfType<IMultilineCommentRule>().ToList().IndexOf(input);
                    output.Name = $"MultilineComment{index + 1}Rule";
                }

                output.BaseTypes.Add(typeof(Rules.MultilineCommentRule).ToTypeReference());
                output.WriteDocumentation($"Denotes a rule to parse multi-line comments starting with '{input.Start}' and ending with '{input.End}'");

                var init = CreateInitializeMethod();
                init.Statements.Add(new CodeAssignStatement(
                    new CodePropertyReferenceExpression(null, nameof(AnyText.Rules.CommentRule.CommentStart)),
                    new CodePrimitiveExpression(input.Start)));
                init.Statements.Add(new CodeAssignStatement(
                    new CodePropertyReferenceExpression(null, nameof(AnyText.Rules.MultilineCommentRule.CommentEnd)),
                    new CodePrimitiveExpression(input.End)));
                output.Members.Add(init);
            }
        }

        public class RuleToClass : AbstractTransformationRule<IRule,  CodeTypeDeclaration>
        {
            public override void Transform(IRule input, CodeTypeDeclaration output, ITransformationContext context)
            {
                output.Attributes = MemberAttributes.Public;
                output.IsClass = true;
                output.IsPartial = true;

                output.WriteDocumentation($"A rule class representing the rule '{input.Name}'");
            }
        }

        public class ModelRuleToClass : TransformationRule<IModelRule, CodeTypeDeclaration>
        {
            private RuleToClass _ruleTransformation;

            public override CodeTypeDeclaration CreateOutput(IModelRule input, ITransformationContext context)
            {
                return new CodeTypeDeclaration { Name = input.Name.ToPascalCase() + "Rule" };
            }

            public override void RegisterDependencies()
            {
                _ruleTransformation = Rule<RuleToClass>();
                MarkInstantiatingFor(_ruleTransformation);
            }

            public override void Transform(IModelRule input, CodeTypeDeclaration output, ITransformationContext context)
            {
                output.BaseTypes.Add(new CodeTypeReference(typeof(ModelElementRule<>).Name, CreateReference(input, false, context)));

                var initialize = CreateInitializeMethod();
                IEnumerable<IFormattedExpression> innerExpressions;
                if (input.Expression is ISequenceExpression sequence)
                {
                    innerExpressions = sequence.InnerExpressions;
                }
                else
                {
                    var formattedExpression = new FormattedExpression { Expression = input.Expression };
                    formattedExpression.FormattingInstructions.AddRange(input.FormattingInstructions);
                    innerExpressions = Enumerable.Repeat(formattedExpression, 1);
                }
                var rules = new CodeArrayCreateExpression(typeof(FormattedRule).ToTypeReference());
                var assignTransformation = Rule<AssignmentToClass>();
                foreach (var exp in innerExpressions)
                {
                    rules.Initializers.Add(CreateParserExpression(exp.Expression, exp.FormattingInstructions, _ruleTransformation, assignTransformation, context));
                }
                initialize.Statements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(null, nameof(SequenceRule.Rules)), rules));
                output.Members.Add(initialize);
            }
        }

        public class ParanthesisRuleToClass : TransformationRule<IParanthesisRule, CodeTypeDeclaration>
        {
            private RuleToClass _ruleTransformation;

            public override CodeTypeDeclaration CreateOutput(IParanthesisRule input, ITransformationContext context)
            {
                return new CodeTypeDeclaration { Name = input.Name.ToPascalCase() + "Rule" };
            }

            public override void RegisterDependencies()
            {
                _ruleTransformation = Rule<RuleToClass>();
                MarkInstantiatingFor(_ruleTransformation);
            }

            public override void Transform(IParanthesisRule input, CodeTypeDeclaration output, ITransformationContext context)
            {
                output.BaseTypes.Add(typeof(ParanthesesRule).ToTypeReference());

                var initialize = CreateInitializeMethod();
                var rules = new CodeArrayCreateExpression(typeof(FormattedRule).ToTypeReference());
                var assignTransformation = Rule<AssignmentToClass>();
                rules.Initializers.Add(CreateParserExpression(input.OpeningParanthesis, input.FormattingInstructionsAfterOpening, _ruleTransformation, assignTransformation, context));
                rules.Initializers.Add(CreateResolveRule(CreateRuleReference(input.InnerRule, _ruleTransformation, context), input.FormattingInstructionsInner));
                rules.Initializers.Add(CreateParserExpression(input.ClosingParanthesis, input.FormattingInstructionsAfterClosing, _ruleTransformation, assignTransformation, context));
                initialize.Statements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(null, nameof(SequenceRule.Rules)), rules));
                output.Members.Add(initialize);
            }

        }

        public class DataRuleToClass : TransformationRule<IDataRule, CodeTypeDeclaration>
        {
            public override CodeTypeDeclaration CreateOutput(IDataRule input, ITransformationContext context)
            {
                return new CodeTypeDeclaration { Name = input.Name.ToPascalCase() + "Rule" };
            }

            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<RuleToClass>());
            }

            public override void Transform(IDataRule input, CodeTypeDeclaration output, ITransformationContext context)
            {
                if (input.TypeName == null)
                {
                    if (input.EscapeRules.Count == 0)
                    {
                        output.BaseTypes.Add(typeof(RegexRule).ToTypeReference());
                    }
                    else
                    {
                        output.BaseTypes.Add(typeof(EscapedRegexRule).ToTypeReference());
                        output.Members.Add(CreateEscape(input));
                        output.Members.Add(CreateUnescape(input));
                    }
                }
                else
                {
                    if (input.EscapeRules.Count == 0)
                    {
                        output.BaseTypes.Add(new CodeTypeReference(typeof(ConvertRule<>).Name, CreateReference(input, false, context)));
                    }
                    else
                    {
                        output.BaseTypes.Add(new CodeTypeReference(typeof(EscapedConvertRule<>).Name, CreateReference(input, false, context)));
                        output.Members.Add(CreateEscape(input));
                        output.Members.Add(CreateUnescape(input));
                    }
                }

                var innerRegex = "^" + input.SurroundCharacter + input.Regex + input.SurroundCharacter;

                var initialize = CreateInitializeMethod();
                initialize.Statements.Add(new CodeAssignStatement(
                    new CodePropertyReferenceExpression(null, nameof(RegexRule.Regex)),
                    new CodeObjectCreateExpression(typeof(Regex).ToTypeReference(), new CodePrimitiveExpression(innerRegex), new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(nameof(RegexOptions)), nameof(RegexOptions.Compiled)))));
                output.Members.Add(initialize);
            }

            private CodeMemberMethod CreateEscape(IDataRule dataRule)
            {
                var escape = new CodeMemberMethod
                {
                    Name = nameof(EscapedRegexRule.Escape),
                    Attributes = MemberAttributes.Public | MemberAttributes.Override,
                    ReturnType = new CodeTypeReference(typeof(string)),
                    Parameters =
                    {
                        new CodeParameterDeclarationExpression(typeof(string), "value")
                    }
                };
                escape.WriteDocumentation("Escapes the given string", "the escaped string", new Dictionary<string, string>
                {
                    ["value"] = "the unescaped string"
                });
                CodeExpression ret = new CodeArgumentReferenceExpression("value");
                foreach (var escapeRule in dataRule.EscapeRules)
                {
                    ret = new CodeMethodInvokeExpression(ret, nameof(string.Replace), new CodePrimitiveExpression(escapeRule.Character), new CodePrimitiveExpression(escapeRule.Escape));
                }
                if (!string.IsNullOrEmpty(dataRule.SurroundCharacter))
                {
                    ret = new CodeBinaryOperatorExpression(new CodeBinaryOperatorExpression(new CodePrimitiveExpression(dataRule.SurroundCharacter), CodeBinaryOperatorType.Add, ret), CodeBinaryOperatorType.Add, new CodePrimitiveExpression(dataRule.SurroundCharacter));
                }
                escape.Statements.Add(new CodeMethodReturnStatement(ret));
                return escape;
            }

            private CodeMemberMethod CreateUnescape(IDataRule dataRule)
            {
                var unescape = new CodeMemberMethod
                {
                    Name = nameof(EscapedRegexRule.Unescape),
                    Attributes = MemberAttributes.Public | MemberAttributes.Override,
                    ReturnType = new CodeTypeReference(typeof(string)),
                    Parameters =
                    {
                        new CodeParameterDeclarationExpression(typeof(string), "value")
                    }
                };
                unescape.WriteDocumentation("Unescapes the given string", "the unescaped string", new Dictionary<string, string>
                {
                    ["value"] = "the escaped string"
                });
                CodeExpression ret = new CodeArgumentReferenceExpression("value");
                if (!string.IsNullOrEmpty(dataRule.SurroundCharacter))
                {
                    ret = new CodeMethodInvokeExpression(ret, nameof(string.Substring),
                        new CodePrimitiveExpression(dataRule.SurroundCharacter.Length),
                        new CodeBinaryOperatorExpression(new CodePropertyReferenceExpression(ret, nameof(string.Length)), CodeBinaryOperatorType.Subtract, new CodePrimitiveExpression(2*dataRule.SurroundCharacter.Length)));
                }
                foreach (var escapeRule in dataRule.EscapeRules)
                {
                    ret = new CodeMethodInvokeExpression(ret, nameof(string.Replace), new CodePrimitiveExpression(escapeRule.Escape), new CodePrimitiveExpression(escapeRule.Character));
                }
                unescape.Statements.Add(new CodeMethodReturnStatement(ret));
                return unescape;
            }
        }

        public class FragmentRuleToClass : TransformationRule<IFragmentRule, CodeTypeDeclaration>
        {

            private RuleToClass _ruleTransformation;

            public override CodeTypeDeclaration CreateOutput(IFragmentRule input, ITransformationContext context)
            {
                return new CodeTypeDeclaration { Name = input.Name.ToPascalCase() + "Rule" };
            }

            public override void RegisterDependencies()
            {
                _ruleTransformation = Rule<RuleToClass>();
                MarkInstantiatingFor(_ruleTransformation);
            }

            public override void Transform(IFragmentRule input, CodeTypeDeclaration output, ITransformationContext context)
            {
                output.BaseTypes.Add(typeof(QuoteRule).ToTypeReference());

                var initialize = CreateInitializeMethod();
                var assignTransformation = Rule<AssignmentToClass>();
                var rule = CreateParserExpression(input.Expression, input.FormattingInstructions, _ruleTransformation, assignTransformation, context);
                initialize.Statements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(null, nameof(QuoteRule.Inner)), rule));
                output.Members.Add(initialize);
            }
        }

        public class InheritanceRuleToClass : TransformationRule<IInheritanceRule, CodeTypeDeclaration>
        {
            public override CodeTypeDeclaration CreateOutput(IInheritanceRule input, ITransformationContext context)
            {
                return new CodeTypeDeclaration { Name = input.Name.ToPascalCase() + "Rule" };
            }

            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<RuleToClass>());
            }

            public override void Transform(IInheritanceRule input, CodeTypeDeclaration output, ITransformationContext context)
            {
                output.BaseTypes.Add(typeof(ChoiceRule).ToTypeReference());

                var initialize = CreateInitializeMethod();
                var rules = new CodeArrayCreateExpression(typeof(FormattedRule).ToTypeReference());
                var ruleToClass = Rule<RuleToClass>();
                foreach (var exp in input.Subtypes)
                {
                    rules.Initializers.Add(CreateResolveRule(CreateRuleReference(exp, ruleToClass, context), null));
                }
                initialize.Statements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(null, nameof(ChoiceRule.Alternatives)), rules));
                output.Members.Add(initialize);
            }
        }

        public class EnumRuleToClass : TransformationRule<IEnumRule, CodeTypeDeclaration>
        {
            public override CodeTypeDeclaration CreateOutput(IEnumRule input, ITransformationContext context)
            {
                return new CodeTypeDeclaration { Name = input.Name.ToPascalCase() + "Rule" };
            }

            private RuleToClass RuleToClass;
            private AssignmentToClass AssignmentToClass;

            public override void RegisterDependencies()
            {
                RuleToClass = Rule<RuleToClass>();
                AssignmentToClass = Rule<AssignmentToClass>();
                MarkInstantiatingFor(RuleToClass);
            }

            public override void Transform(IEnumRule input, CodeTypeDeclaration output, ITransformationContext context)
            {
                var enumType = CreateReference(input, false, context);
                output.BaseTypes.Add(new CodeTypeReference(typeof(EnumRule<>).Name, enumType));

                var initialize = CreateInitializeMethod();
                var literals = new CodeArrayCreateExpression(typeof(FormattedRule).ToTypeReference());
                var values = new CodeArrayCreateExpression(enumType);
                initialize.Statements.Add(new CodeAssignStatement(
                    new CodePropertyReferenceExpression(null, nameof(ChoiceRule.Alternatives)),
                    literals));
                initialize.Statements.Add(new CodeAssignStatement(
                    new CodePropertyReferenceExpression(null, "Values"),
                    values));

                foreach (var lit in input.Literals)
                {
                    literals.Initializers.Add(CreateParserExpression(lit.Keyword.Expression, lit.Keyword.FormattingInstructions, RuleToClass, AssignmentToClass, context));
                    values.Initializers.Add(new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(enumType), lit.Literal.ToPascalCase()));
                }

                output.Members.Add(initialize);
            }
        }

        public class AssignmentToClass : AbstractTransformationRule<IFeatureExpression, CodeTypeDeclaration>
        {

            public override void Transform(IFeatureExpression input, CodeTypeDeclaration output, ITransformationContext context)
            {
                var initialize = CreateInitializeMethod();
                initialize.Statements.Add(new CodeAssignStatement(
                    new CodePropertyReferenceExpression(null, nameof(QuoteRule.Inner)),
                    CreateParserExpression(input.Assigned, Enumerable.Empty<FormattingInstruction>(), Rule<RuleToClass>(), Rule<AssignmentToClass>(), context)));
                output.Members.Add(initialize);
                output.IsPartial = true;

                var feature = new CodeMemberProperty
                {
                    Attributes = MemberAttributes.Family | MemberAttributes.Override,
                    Name = "Feature",
                    Type = typeof(string).ToTypeReference(),
                    HasGet = true,
                    HasSet = false
                };
                feature.WriteDocumentation("Gets the name of the feature that is assigned");
                feature.GetStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(input.Feature)));
                output.Members.Add(feature);
                output.WriteDocumentation($"Rule to assign the contents of the inner rule to {input.Feature}");

                var semFeature = CodeGenerator._trace.LookupFeature(input);
                if (semFeature != null && semFeature.Parent is IClass cl && cl.Identifier == semFeature)
                {
                    var isIdentifier = new CodeMemberProperty
                    {
                        Attributes = MemberAttributes.Public | MemberAttributes.Override,
                        Name = "IsIdentifier",
                        Type = typeof(bool).ToTypeReference(),
                        HasGet = true,
                        HasSet = false
                    };
                    isIdentifier.WriteDocumentation("Gets the first contained rule application that represents an identifier");
                    isIdentifier.GetStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(true)));
                    output.Members.Add(isIdentifier);
                }
            }
        }

        public class ExistsAssignToClass : TransformationRule<IExistsAssignExpression, CodeTypeDeclaration>
        {
            public override CodeTypeDeclaration CreateOutput(IExistsAssignExpression input, ITransformationContext context)
            {
                var semanticType = GetSemanticTypeForFeature(input, context);
                return new CodeTypeDeclaration
                {
                    Name = $"{semanticType.semanticType.BaseType.Substring(1)}{input.Feature.ToPascalCase()}Rule"
                };
            }

            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<AssignmentToClass>());
            }


            public override void Transform(IExistsAssignExpression input, CodeTypeDeclaration output, ITransformationContext context)
            {
                var (semanticType, _) = GetSemanticTypeForFeature(input, context);
                var propertyType = new CodeTypeReference(typeof(bool));

                output.BaseTypes.Add(new CodeTypeReference(typeof(ExistsAssignRule<>).Name, semanticType));

                var semanticElementRef = new CodeArgumentReferenceExpression("semanticElement");
                var propertyValueRef = new CodeArgumentReferenceExpression("propertyValue");
                var property = new CodePropertyReferenceExpression(semanticElementRef, input.Feature.ToPascalCase());

                var getValue = new CodeMemberMethod
                {
                    Name = "GetValue",
                    Attributes = MemberAttributes.Family | MemberAttributes.Override,
                    Parameters =
                    {
                        new CodeParameterDeclarationExpression(semanticType, semanticElementRef.ParameterName),
                        new CodeParameterDeclarationExpression(typeof(ParseContext).ToTypeReference(), "context")
                    },
                    ReturnType = propertyType
                };
                getValue.WriteDocumentation("Gets the value of the given property", "the property value", new Dictionary<string, string>
                {
                    [semanticElementRef.ParameterName] = "the context element",
                    ["context"] = "the parsing context"
                });
                CodeExpression getValueReturn = property;
                if (Helper.IsNullable(input))
                {
                    getValueReturn = new CodeMethodInvokeExpression(property, nameof(Nullable<bool>.GetValueOrDefault));
                }
                getValue.Statements.Add(new CodeMethodReturnStatement(getValueReturn));
                output.Members.Add(getValue);
                var setValue = new CodeMemberMethod
                {
                    Name = "SetValue",
                    Attributes = MemberAttributes.Family | MemberAttributes.Override,
                    Parameters =
                    {
                        new CodeParameterDeclarationExpression(semanticType, semanticElementRef.ParameterName),
                        new CodeParameterDeclarationExpression(propertyType, propertyValueRef.ParameterName),
                        new CodeParameterDeclarationExpression(typeof(ParseContext).ToTypeReference(), "context")
                    }
                };
                setValue.WriteDocumentation("Assigns the value to the given semantic element", null, new Dictionary<string, string>
                {
                    [semanticElementRef.ParameterName] = "the context element",
                    [propertyValueRef.ParameterName] = "the value to assign",
                    ["context"] = "the parsing context"
                });
                setValue.Statements.Add(new CodeAssignStatement(
                    property,
                    propertyValueRef));
                output.Members.Add(setValue);
            }
        }

        public class AssignRuleToClass : TransformationRule<IAssignExpression, CodeTypeDeclaration>
        {
            public override CodeTypeDeclaration CreateOutput(IAssignExpression input, ITransformationContext context)
            {
                var semanticType = GetSemanticTypeForFeature(input, context);
                return new CodeTypeDeclaration
                {
                    Name = $"{semanticType.semanticType.BaseType.Substring(1)}{input.Feature.ToPascalCase()}{GetNameString(input.Assigned)}Rule"
                };
            }

            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<AssignmentToClass>());
            }

            public override void Transform(IAssignExpression input, CodeTypeDeclaration output, ITransformationContext context)
            {
                var (semanticType, propertyType) = GetSemanticTypeForFeature(input, context);


                if (input.Assigned is IReferenceExpression)
                {
                    output.BaseTypes.Add(new CodeTypeReference(typeof(AssignModelReferenceRule<,>).Name, semanticType, propertyType));
                }
                else
                {
                    output.BaseTypes.Add(new CodeTypeReference(typeof(AssignRule<,>).Name, semanticType, propertyType));
                }

                var semanticElementRef = new CodeArgumentReferenceExpression("semanticElement");
                var property = new CodePropertyReferenceExpression(semanticElementRef, input.Feature.ToPascalCase());
                if (input.Feature.StartsWith("context."))
                {
                    property = new CodePropertyReferenceExpression(new CodeArgumentReferenceExpression("context"), input.Feature.Substring(8).ToPascalCase());
                }

                output.Members.Add(CreateGetValue(input, semanticType, propertyType, semanticElementRef, property));
                output.Members.Add(CreateSetValue(semanticType, propertyType, semanticElementRef, property));
            }

            private static CodeMemberMethod CreateSetValue(CodeTypeReference semanticType, CodeTypeReference propertyType, CodeArgumentReferenceExpression semanticElementRef, CodePropertyReferenceExpression property)
            {
                var propertyValueRef = new CodeArgumentReferenceExpression("propertyValue");
                var setValue = new CodeMemberMethod
                {
                    Name = "SetValue",
                    Attributes = MemberAttributes.Family | MemberAttributes.Override,
                    Parameters =
                    {
                        new CodeParameterDeclarationExpression(semanticType, semanticElementRef.ParameterName),
                        new CodeParameterDeclarationExpression(propertyType, propertyValueRef.ParameterName),
                        new CodeParameterDeclarationExpression(typeof(ParseContext).ToTypeReference(), "context")
                    }
                };
                setValue.WriteDocumentation("Assigns the value to the given semantic element", null, new Dictionary<string, string>
                {
                    [semanticElementRef.ParameterName] = "the context element",
                    [propertyValueRef.ParameterName] = "the value to assign",
                    ["context"] = "the parsing context"
                });
                setValue.Statements.Add(new CodeAssignStatement(
                    property,
                    propertyValueRef));
                return setValue;
            }

            private static CodeMemberMethod CreateGetValue(IAssignExpression input, CodeTypeReference semanticType, CodeTypeReference propertyType, CodeArgumentReferenceExpression semanticElementRef, CodePropertyReferenceExpression property)
            {
                var getValue = new CodeMemberMethod
                {
                    Name = "GetValue",
                    Attributes = MemberAttributes.Family | MemberAttributes.Override,
                    Parameters =
                    {
                        new CodeParameterDeclarationExpression(semanticType, semanticElementRef.ParameterName),
                        new CodeParameterDeclarationExpression(typeof(ParseContext).ToTypeReference(), "context")
                    },
                    ReturnType = propertyType
                };
                getValue.WriteDocumentation("Gets the value of the given property", "the property value", new Dictionary<string, string>
                {
                    [semanticElementRef.ParameterName] = "the context element",
                    ["context"] = "the parsing context"
                });
                CodeExpression getValueReturn = property;
                if (Helper.IsNullable(input))
                {
                    getValueReturn = new CodeMethodInvokeExpression(property, nameof(Nullable<int>.GetValueOrDefault));
                }
                getValue.Statements.Add(new CodeMethodReturnStatement(getValueReturn));
                return getValue;
            }
        }

        public class AddAssignRuleToClass : TransformationRule<IAddAssignExpression, CodeTypeDeclaration>
        {
            public override CodeTypeDeclaration CreateOutput(IAddAssignExpression input, ITransformationContext context)
            {
                var semanticType = GetSemanticTypeForFeature(input, context);
                return new CodeTypeDeclaration
                {
                    Name = $"{semanticType.semanticType.BaseType.Substring(1)}{input.Feature.ToPascalCase()}{GetNameString(input.Assigned)}Rule"
                };
            }

            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<AssignmentToClass>());
            }

            public override void Transform(IAddAssignExpression input, CodeTypeDeclaration output, ITransformationContext context)
            {
                var (semanticType, propertyType) = GetSemanticTypeForFeature(input, context);

                if (input.Assigned is IReferenceExpression)
                {
                    output.BaseTypes.Add(new CodeTypeReference(typeof(AddAssignModelReferenceRule<,>).Name, semanticType, propertyType));
                }
                else
                {
                    output.BaseTypes.Add(new CodeTypeReference(typeof(AddAssignRule<,>).Name, semanticType, propertyType));
                }

                var semanticElementRef = new CodeArgumentReferenceExpression("semanticElement");

                var getCollection = new CodeMemberMethod
                {
                    Name = "GetCollection",
                    Attributes = MemberAttributes.Public | MemberAttributes.Override,
                    Parameters =
                    {
                        new CodeParameterDeclarationExpression(semanticType, semanticElementRef.ParameterName),
                        new CodeParameterDeclarationExpression(typeof(ParseContext).ToTypeReference(), "context")
                    },
                    ReturnType = new CodeTypeReference(typeof(ICollection<>).Name, propertyType)
                };
                getCollection.WriteDocumentation("Obtains the child collection", "a collection of values", new Dictionary<string, string>
                {
                    [semanticElementRef.ParameterName] = "the context element",
                    ["context"] = "the parse context in which the collection is obtained"
                });
                var property = new CodePropertyReferenceExpression(semanticElementRef, input.Feature.ToPascalCase());
                if (input.Feature.StartsWith("context."))
                {
                    property = new CodePropertyReferenceExpression(new CodeArgumentReferenceExpression("context"), input.Feature.Substring(8).ToPascalCase());
                }
                getCollection.Statements.Add(new CodeMethodReturnStatement(property));
                output.Members.Add(getCollection);
            }

        }
#pragma warning restore S3265 // Non-flags enums should not be used in bitwise operations
    }
}
