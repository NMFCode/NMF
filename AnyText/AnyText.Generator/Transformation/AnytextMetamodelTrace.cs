using NMF.Analyses;
using NMF.AnyText.Metamodel;
using NMF.Models;
using NMF.Models.Meta;
using NMF.Models.Repository;
using NMF.Utilities;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NMF.AnyText.Transformation.AnytextCodeGenerator;
using Attribute = NMF.Models.Meta.Attribute;

namespace NMF.AnyText.Transformation
{
    internal class AnytextMetamodelTrace
    {
        private readonly Dictionary<string, INamespace> _nsDict = new Dictionary<string, INamespace>();
        private readonly Dictionary<IRule, IType> _typeLookup = new Dictionary<IRule, IType>();
        private readonly Dictionary<IFeatureExpression, ITypedElement> _featureLookup = new Dictionary<IFeatureExpression, ITypedElement>();

        public IType LookupType(IRule rule) => _typeLookup.TryGetValue(rule, out var type) ? type : null;

        public ITypedElement LookupFeature(IFeatureExpression featureExpression) => _featureLookup.TryGetValue(featureExpression, out var feature) ? feature : null;

        public Namespace CreateNamespace(IGrammar grammar, IModelRepository repository)
        {
            _nsDict["nmeta"] = Class.ClassInstance.Namespace;
            LoadImports(grammar, repository);
            var ns = new Namespace { Name = grammar.Name, Prefix = grammar.LanguageId, Uri = new Uri($"anytext:{grammar.LanguageId}") };
            _nsDict[string.Empty] = ns;

            LoadTypesFromClassRules(grammar, ns);
            LoadTypesFromFragments(grammar, ns);
            LoadTypesFromDataRules(grammar, ns);
            LoadTypesFromEnumRules(grammar, ns);
            RegisterInheritance(grammar);
            CheckCyclicInheritance(ns);
            RegisterAssignments(grammar);

            if (ns.Types.Count == 0)
            {
                return null;
            }

            return ns;
        }

        private static void CheckCyclicInheritance(Namespace ns)
        {
            var layering = Layering<IClass>.CreateLayers(ns.Types.OfType<IClass>(), c => c.BaseTypes);
            foreach (var layer in layering)
            {
                if (layer.Count > 1)
                {
                    throw new InvalidOperationException($"The following classes would form a cyclic inheritance relation: {string.Join(", ", layer.Select(c => c.Name))}");
                }
            }
        }

        private void RegisterAssignments(IGrammar grammar)
        {
            foreach (var rule in grammar.Rules.OfType<IModelRule>())
            {
                var cl = FindClass(rule);
                if (rule.Expression is IFeatureExpression featureAssignment)
                {
                    RegisterAssignment(cl, featureAssignment);
                }
                else
                {
                    foreach (var assignment in rule.Expression.Descendants().OfType<IFeatureExpression>())
                    {
                        RegisterAssignment(cl, assignment);
                    }
                }
            }
            foreach (var rule in grammar.Rules.OfType<IFragmentRule>())
            {
                var cl = FindClass(rule);
                if (rule.Expression is IFeatureExpression featureAssignment)
                {
                    RegisterAssignment(cl, featureAssignment);
                }
                else
                {
                    foreach (var assignment in rule.Expression.Descendants().OfType<IFeatureExpression>())
                    {
                        RegisterAssignment(cl, assignment);
                    }
                }
            }
        }

        private void RegisterInheritance(IGrammar grammar)
        {
            foreach (var inheritance in grammar.Rules.OfType<IInheritanceRule>())
            {
                var baseClass = FindClass(inheritance);
                foreach (var subType in inheritance.Subtypes)
                {
                    var derived = FindClass(subType);
                    if (baseClass == derived)
                    {
                        continue;
                    }
                    if (!derived.Closure(c => c.BaseTypes).Contains(baseClass))
                    {
                        if (!derived.IsLocked)
                        {
                            derived.BaseTypes.Add(baseClass);
                        }
                        else
                        {
                            throw new InvalidOperationException($"{derived} has to inherit from {baseClass} but no inheritance relation was found.");
                        }
                    }
                }
            }
        }

        private void RegisterAssignment(IClass ruleClass, IFeatureExpression assignment)
        {
            if (assignment.Feature.StartsWith("context."))
            {
                return;
            }
            var isCollection = assignment is IAddAssignExpression;
            var type = SynthesizeType(assignment.Assigned, out var isContainment);
            if (assignment is IExistsAssignExpression)
            {
                type = MetaRepository.Instance.ResolveType("http://nmf.codeplex.com/nmeta/#//Boolean");
                isContainment = null;
            }
            if (isContainment.HasValue)
            {
                var reference = ruleClass.LookupReference(assignment.Feature);
                if (reference == null)
                {
                    if (ruleClass.IsLocked)
                    {
                        throw new InvalidOperationException($"{ruleClass} should have a reference {assignment.Feature} but no such reference was found.");
                    }
                    reference = new Reference
                    {
                        Name = assignment.Feature,
                        ReferenceType = type as IReferenceType,
                        IsContainment = isContainment.Value,
                        UpperBound = isCollection ? -1 : 1
                    };
                    ruleClass.References.Add(reference);
                }
                _featureLookup.Add(assignment, reference);
            }
            else
            {
                var attribute = ruleClass.LookupAttribute(assignment.Feature);
                if (attribute == null)
                {
                    if (ruleClass.IsLocked)
                    {
                        throw new InvalidOperationException($"{ruleClass} should have an attribute {assignment.Feature} but no such attribute was found.");
                    }
                    attribute = new Attribute
                    {
                        Name = assignment.Feature,
                        Type = type,
                        LowerBound = isCollection || IsOptional(assignment) ? 0 : 1,
                        UpperBound = isCollection ? -1 : 1
                    };
                    ruleClass.Attributes.Add(attribute);
                }
                _featureLookup.Add(assignment, attribute);
            }
        }

        private bool IsOptional(IParserExpression expression)
        {
            switch (expression.Parent)
            {
                case IModelRule:
                    return false;
                case IStarExpression:
                    return true;
                case IMaybeExpression:
                    return true;
                case IParserExpression otherExpression:
                    return IsOptional(otherExpression);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private IType SynthesizeType(IParserExpression expression, out bool? isContainment)
        {
            switch (expression)
            {
                case IRuleExpression ruleExpression:
                    if (ruleExpression.Rule == null)
                    {
                        isContainment = false;
                        return null;
                    }
                    var type = _typeLookup[ruleExpression.Rule];
                    if (type is IClass)
                    {
                        isContainment = true;
                    }
                    else
                    {
                        isContainment = null;
                    }
                    return type;
                case IReferenceExpression referenceExpression:
                    isContainment = false;
                    if (referenceExpression.ReferencedRule == null)
                    {
                        return null;
                    }
                    return _typeLookup[referenceExpression.ReferencedRule];
                case IUnaryParserExpression unary:
                    return SynthesizeType(unary.Inner, out isContainment);
                case IChoiceExpression choice:
                    return SynthesizeType(choice.Alternatives[0], out isContainment);
                case ISequenceExpression sequence:
                    return SynthesizeType(sequence.InnerExpressions[0], out isContainment);
                case IFeatureExpression feature:
                    return SynthesizeType(feature.Assigned, out isContainment);
                case IKeywordExpression:
                    isContainment = null;
                    return MetaRepository.Instance.ResolveType("http://nmf.codeplex.com/nmeta/#//String");
            }
            throw new NotImplementedException();
        }

        private void LoadTypesFromDataRules(IGrammar grammar, Namespace ns)
        {
            foreach (var rule in grammar.Rules.OfType<IDataRule>())
            {
                var cl = FindType(rule);
                if (cl == null)
                {
                    cl = new PrimitiveType { Name = rule.TypeName ?? rule.Name };
                    ns.Types.Add(cl);
                    rule.Prefix = string.Empty;
                }
                _typeLookup.Add(rule, cl);
            }
        }

        private void LoadTypesFromEnumRules(IGrammar grammar, Namespace ns)
        {
            foreach (var rule in grammar.Rules.OfType<IEnumRule>())
            {
                var cl = FindType(rule);
                IEnumeration enumeration;
                if (cl == null)
                {
                    enumeration = new Enumeration { Name = rule.TypeName ?? rule.Name };
                    cl = enumeration;
                    ns.Types.Add(cl);
                    rule.Prefix = string.Empty;
                }
                else if (cl is IEnumeration en)
                {
                    enumeration = en;
                }
                else
                {
                    throw new InvalidOperationException($"Type {cl.Name} already exists, but is not an enumeration.");
                }
                foreach (var lit in rule.Literals)
                {
                    if (!enumeration.Literals.Any(l => l.Name == lit.Literal))
                    {
                        enumeration.Literals.Add(new Literal
                        {
                            Name = lit.Literal,
                            Value = lit.Value
                        });
                    }
                }
                _typeLookup.Add(rule, cl);
            }
        }

        private void LoadTypesFromFragments(IGrammar grammar, Namespace ns)
        {
            foreach (var rule in grammar.Rules.OfType<IFragmentRule>())
            {
                var cl = FindClass(rule);
                if (cl == null)
                {
                    cl = new Class { Name = rule.TypeName ?? rule.Name };
                    ns.Types.Add(cl);
                    rule.Prefix = string.Empty;
                }
                _typeLookup.Add(rule, cl);
            }
        }

        private void LoadTypesFromClassRules(IGrammar grammar, Namespace ns)
        {
            foreach (var rule in grammar.Rules.OfType<IClassRule>())
            {
                var cl = FindClass(rule);
                if (cl == null)
                {
                    cl = new Class { Name = rule.TypeName ?? rule.Name };
                    ns.Types.Add(cl);
                    rule.Prefix = string.Empty;
                }
                _typeLookup.Add(rule, cl);
            }
        }

        private IClass FindClass(IRule rule)
        {
            if (rule is IParanthesisRule paranthesisRule)
            {
                rule = paranthesisRule.InnerRule;
            }
            var name = rule.TypeName ?? rule.Name;
            if (rule.Prefix == null)
            {
                return _nsDict.Values.SelectMany(n => n.Types).OfType<IClass>()
                    .FirstOrDefault(c => c.Name == name);
            }
            else if (_nsDict.TryGetValue(rule.Prefix, out var ns))
            {
                return ns.Types.OfType<IClass>().FirstOrDefault(c => c.Name == name);
            }
            throw new NotImplementedException();
        }

        private IType FindType(IDataRule rule)
        {
            var name = rule.TypeName ?? "String";
            if (rule.Prefix == null)
            {
                return _nsDict.Values.SelectMany(n => n.Types).OfType<IPrimitiveType>()
                    .FirstOrDefault(c => c.Name == name);
            }
            else if (_nsDict.TryGetValue(rule.Prefix, out var ns))
            {
                return ns.Types.OfType<IPrimitiveType>().FirstOrDefault(c => c.Name == name);
            }
            throw new NotImplementedException();
        }

        private IType FindType(IEnumRule rule)
        {
            var name = rule.TypeName ?? rule.Name;
            if (rule.Prefix == null)
            {
                return _nsDict.Values.SelectMany(n => n.Types).OfType<IEnumeration>()
                    .FirstOrDefault(c => c.Name == name);
            }
            else if (_nsDict.TryGetValue(rule.Prefix, out var ns))
            {
                return ns.Types.OfType<IEnumeration>().FirstOrDefault(c => c.Name == name);
            }
            throw new NotImplementedException();
        }

        private void LoadImports(IGrammar grammar, IModelRepository repository)
        {
            foreach (var import in grammar.Imports)
            {
                var resolved = LoadNamespace(repository, import);
                if (resolved != null)
                {
                    _nsDict[import.Prefix ?? resolved.Prefix] = resolved;
                }
                else
                {
                    throw new InvalidOperationException($"File {import.File} was not found or does not contain a namespace.");
                }
            }
        }

        private static INamespace LoadNamespace(IModelRepository repository, IMetamodelImport import)
        {
            var file = import.File;
            if (!Uri.TryCreate(file, UriKind.Absolute, out var uri))
            {
            if (string.IsNullOrEmpty(Path.GetExtension(file)))
            {
                file = file + ".nmeta";
            }
                uri = new Uri(Path.GetFullPath(file));
            }
            var resolved = repository.Resolve(uri);
            if (resolved is INamespace ns)
            {
                return ns;
            }
            return resolved?.Model.RootElements.FirstOrDefault() as INamespace;
        }
    }
}
