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
            LoadImports(grammar, repository);

            var ns = new Namespace { Name = grammar.Name };
            _nsDict.Add(string.Empty, ns);
            _nsDict.Add("nmeta", Class.ClassInstance.Namespace);

            LoadTypesFromClassRules(grammar, ns);
            LoadTypesFromFragments(grammar, ns);
            LoadTypesFromDataRules(grammar, ns);
            RegisterInheritance(grammar);
            var layering = Layering<IClass>.CreateLayers(ns.Types.OfType<IClass>(), c => c.BaseTypes);
            foreach (var layer in layering)
            {
                if (layer.Count > 1)
                {
                    throw new InvalidOperationException($"The following classes would form a cyclic inheritance relation: {string.Join(", ", layer.Select(c => c.Name))}");
                }
            }
            RegisterAssignments(grammar);

            if (ns.Types.Count == 0)
            {
                return null;
            }

            return ns;
        }

        private void RegisterAssignments(IGrammar grammar)
        {
            foreach (var rule in grammar.Rules.OfType<IModelRule>())
            {
                var cl = FindClass(rule);
                foreach (var assignment in rule.Expression.Descendants().OfType<IFeatureExpression>())
                {
                    RegisterAssignment(cl, assignment);
                }
            }
            foreach (var rule in grammar.Rules.OfType<IFragmentRule>())
            {
                var cl = FindClass(rule);
                foreach (var assignment in rule.Expression.Descendants().OfType<IFeatureExpression>())
                {
                    RegisterAssignment(cl, assignment);
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
                    if (!derived.BaseTypes.Contains(baseClass))
                    {
                        if (!derived.IsLocked)
                        {
                            derived.BaseTypes.Add(baseClass);
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                    }
                }
            }
        }

        private void RegisterAssignment(IClass ruleClass, IFeatureExpression assignment)
        {
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
                    attribute = new Attribute
                    {
                        Name = assignment.Feature,
                        Type = type,
                        UpperBound = isCollection ? -1 : 1
                    };
                    ruleClass.Attributes.Add(attribute);
                }
                _featureLookup.Add(assignment, attribute);
            }
        }

        private IType SynthesizeType(IParserExpression expression, out bool? isContainment)
        {
            switch (expression)
            {
                case IRuleExpression ruleExpression:
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

        private void LoadImports(IGrammar grammar, IModelRepository repository)
        {
            foreach (var import in grammar.Imports)
            {
                var resolved = LoadNamespace(repository, import);
                if (resolved != null)
                {
                    _nsDict.Add(import.Prefix ?? resolved.Prefix, resolved);
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
            if (string.IsNullOrEmpty(Path.GetExtension(file)))
            {
                file = file + ".nmeta";
            }
            var resolved = repository.Resolve(new Uri(Path.GetFullPath(file)));
            if (resolved is INamespace ns)
            {
                return ns;
            }
            return resolved?.Model.RootElements.FirstOrDefault() as INamespace;
        }
    }
}
