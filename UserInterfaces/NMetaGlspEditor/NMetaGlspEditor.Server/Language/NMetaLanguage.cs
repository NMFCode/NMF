using NMF.Collections;
using NMF.Collections.ObjectModel;
using NMF.Expressions;
using NMF.Expressions.Linq;
using NMF.Glsp.Language;
using NMF.Glsp.Language.Layouting;
using NMF.Models.Meta;
using NMF.Models.Repository;
using System.Text.RegularExpressions;
using Attribute = NMF.Models.Meta.Attribute;

namespace NMetaEditor.Language
{
    internal partial class NMetaLanguage : GraphicalLanguage
    {
        public override string DiagramType => "nmeta";
        public override DescriptorBase StartRule => Descriptor<RootDescriptor>();

        public class RootDescriptor : NodeDescriptor<INamespace>
        {
            protected override void DefineLayout()
            {
                Nodes(D<TypeDescriptor>(), n => n.Types);

                Edges(D<ReferenceDescriptor>(), n => n.Types.OfType<IReferenceType>().SelectMany(t => t.References).IgnoreUpdates());
                Edges(D<ClassDescriptor>(), D<ClassDescriptor>(), ns => new BaseClassCollection(ns))
                    .WithLabel("New Base-Class");
            }

            private class BaseClassCollection : CustomCollection<(IClass Clazz, IClass BaseClazz)>
            {
                public BaseClassCollection(INamespace ns) : base(ns.Types.OfType<IClass>().SelectMany(c => c.BaseTypes, (c,baseClass) => ValueTuple.Create(c, baseClass)))
                {
                }

                public override void Add((IClass Clazz, IClass BaseClazz) item)
                {
                    item.Clazz.BaseTypes.Add(item.BaseClazz);
                }

                public override void Clear()
                {
                    throw new NotImplementedException();
                }

                public override bool Remove((IClass Clazz, IClass BaseClazz) item)
                {
                    return item.Clazz.BaseTypes.Remove(item.BaseClazz);
                }
            }
        }

        public class NamespaceDescriptor : NodeDescriptor<INamespace>
        {
            protected override void DefineLayout()
            {
                using (Compartment("comp:header"))
                {
                    Label(n => n.Name);
                }
                using (Compartment("comp:types"))
                {
                    Nodes(D<TypeDescriptor>(), n => n.Types).HideInPalette();
                    Edges(D<ReferenceDescriptor>(), n => n.Types.OfType<IReferenceType>().SelectMany(t => t.References).IgnoreUpdates()).HideInPalette();
                }
            }

            public override string ElementTypeId => "ChildNamespace";
        }

        public class TypeDescriptor : AbstractNodeDescriptor<IType> { }

        public class ReferenceTypeDescriptor : AbstractNodeDescriptor<IReferenceType>
        {
            protected override void DefineLayout()
            {
                Refine(D<TypeDescriptor>());
            }
        }

        public class EnumerationDescriptor : NodeDescriptor<IEnumeration>
        {
            protected override void DefineLayout()
            {
                Refine(D<TypeDescriptor>());

                Layout(LayoutStrategy.Vbox);

                using (Compartment("comp:header"))
                {
                    Label(e => e.Name);
                }
                using(Compartment("comp:literals"))
                {
                    Labels(D<LiteralDescriptor>(), e => e.Literals);
                }
            }

            public override IEnumeration CreateElement(string profile, object parent)
            {
                var name = "NewEnumeration";
                if (parent is INamespace ns)
                {
                    name = GetUnassignedName(name, ns.Types.AsEnumerable().Select(t => t.Name));
                }
                return new Enumeration { Name = name };
            }
        }

        public class LiteralDescriptor : LabelDescriptor<ILiteral>
        {
            protected override void DefineLayout()
            {
                Label(l => l.Name);
            }

            public override ILiteral CreateElement(string profile, object parent)
            {
                var name = "NewLiteral";
                if (parent is IEnumeration en)
                {
                    name = GetUnassignedName(name, en.Literals.AsEnumerable().Select(t => t.Name));
                }
                return new Literal { Name = name };
            }
        }

        public class ClassDescriptor : NodeDescriptor<IClass>
        {
            protected override void DefineLayout()
            {
                Refine(D<ReferenceTypeDescriptor>());

                Layout(LayoutStrategy.Vbox);

                using (Compartment("comp:header"))
                {
                    Label(e => e.Name);
                }
                using (Compartment("comp:attributes"))
                {
                    Labels(D<AttributeDescriptor>(), e => e.Attributes);
                }
            }

            public override IClass CreateElement(string profile, object parent)
            {
                var name = "NewClass";
                if (parent is INamespace ns)
                {
                    name = GetUnassignedName(name, ns.Types.AsEnumerable().Select(t => t.Name));
                }
                return new Class { Name = name };
            }
        }

        public partial class AttributeDescriptor : LabelDescriptor<IAttribute>
        {
            private static readonly IType _stringType = MetaRepository.Instance.ResolveType("http://nmf.codeplex.com/nmeta/#//String");

            [GeneratedRegex(@"(?<name>\w+)\s*(:\s*(?<type>\w+)\s*)?(?<bounds>\[(\d+\.\.)?(\d+|\*)\])?(\s*=\s*(?<default>.*))?", RegexOptions.Compiled)]
            private static partial Regex AttributeRegex();

            private static bool CheckAttributeString(IAttribute attribute, string attString) => AttributeRegex().IsMatch(attString);

            protected override void DefineLayout()
            {
                Label(a => a.Name + (a.Type != null ? (" : " + a.Type.Name) : "") + " [" + GetBoundsString.Evaluate(a) + "]")
                    .Validate(CheckAttributeString, "not a valid attribute string")
                    .WithSetter(SetAttributeFromString);
            }

            private static void SetAttributeFromString(IAttribute attribute, string attributeString)
            {
                var match = AttributeRegex().Match(attributeString);
                if (match.Success)
                {
                    var name = match.Groups["name"].Value;
                    var type = match.Groups["type"]?.Value;
                    var bounds = match.Groups["bounds"]?.Value;
                    var defaultValue = match.Groups["default"]?.Value;

                    attribute.Name = name;
                    if (type != null)
                    {
                        attribute.Type = ResolveType(attribute.DeclaringType?.Namespace, type);
                    }
                    if (bounds != null && bounds.Length > 2)
                    {
                        UpdateBounds(attribute, bounds.Substring(1, bounds.Length - 2));
                    }
                    if (defaultValue != null)
                    {
                        attribute.DefaultValue = defaultValue;
                    }
                }
            }

            public override IAttribute CreateElement(string profile, object parent)
            {
                var name = "NewAttribute";
                if (parent is IStructuredType structuredType)
                {
                    name = GetUnassignedName(name, structuredType.Attributes.AsEnumerable().Select(a => a.Name));
                }
                return new Attribute
                {
                    Name = name,
                    Type = _stringType
                };
            }
        }

        public partial class ReferenceDescriptor : EdgeDescriptor<IReference>
        {
            private const string Bidirectional = "Bidirectional Reference";
            private const string Containment = "Containment";

            protected override void DefineLayout()
            {
                Label(r => r.Name)
                    .At(0.5, EdgeSide.Top)
                    .MoveMode(EdgeMoveMode.Edge)
                    .Validate(IdentifierRegex(), "not a valid identifier");
                Label(r => r.Opposite.Name)
                    .If(r => r.Opposite != null)
                    .At(0.5, EdgeSide.Bottom)
                    .MoveMode(EdgeMoveMode.Edge)
                    .Validate(IdentifierRegex(), "not a valid identifier");

                Label(r => GetBoundsString.Evaluate(r))
                    .At(0.75)
                    .MoveMode(EdgeMoveMode.Edge)
                    .Validate(BoundsRegex(), "not a valid bounds string")
                    .WithSetter(UpdateBounds);
                Label(r => GetBoundsString.Evaluate(r.Opposite))
                    .If(r => r.Opposite != null)
                    .MoveMode(EdgeMoveMode.Edge)
                    .Validate(BoundsRegex(), "not a valid bounds string")
                    .WithSetter(UpdateOppositeBounds);

                SourceNode(D<ReferenceTypeDescriptor>(), r => r.DeclaringType);
                TargetNode(D<ReferenceTypeDescriptor>(), r => r.ReferenceType);

                Forward("renderEndArrow", r => r.Opposite == null);
                Forward("renderComposition", r => r.IsContainment);

                Profile("Bidirectional Reference");
                Profile("Containment");
            }

            private void UpdateOppositeBounds(IReference reference, string boundsString)
            {
                if (reference.Opposite != null)
                {
                    UpdateBounds(reference.Opposite, boundsString);
                }
            }

            public override IReference CreateElement(string profile, object parent)
            {
                var reference = new Reference
                {
                    Name = "NewReference",
                    IsContainment = profile == Containment,
                    Opposite = profile == Bidirectional ? new Reference { Name = "Opposite" } : null,
                };
                
                SetHooks(reference);
                SetHooks(reference.Opposite);

                return reference;
            }

            private void SetHooks(IReference? reference)
            {
                if (reference == null)
                {
                    return;
                }
                reference.ParentChanged += OnParentChanged;
                reference.ReferenceTypeChanged += OnReferencyTypeChanged;
                reference.OppositeChanged += OnOppositeChanged;
            }

            private void OnOppositeChanged(object? sender, ValueChangedEventArgs e)
            {
                if (e.OldValue is IReference oldOpposite)
                {
                    oldOpposite.ParentChanged -= OnParentChanged;
                    oldOpposite.ReferenceTypeChanged -= OnReferencyTypeChanged;
                    oldOpposite.OppositeChanged -= OnOppositeChanged;
                }
                SetHooks(e.NewValue as IReference);
            }

            private void OnReferencyTypeChanged(object? sender, ValueChangedEventArgs e)
            {
                if (sender is IReference reference && reference.Opposite != null)
                {
                    reference.Opposite.DeclaringType = reference.ReferenceType;
                }
            }

            private void OnParentChanged(object? sender, ValueChangedEventArgs e)
            {
                if (sender is IReference reference && reference.Opposite != null)
                {
                    reference.Opposite.ReferenceType = reference.DeclaringType;
                }
            }
        }

        private static IType? ResolveType(INamespace? ns, string type)
        {
            if (type == null) return null;
            if (type.IndexOf(':') == -1)
            {
                if (ns != null)
                {
                    var localType = ns.Types.FirstOrDefault(t => string.Equals(t.Name, type, StringComparison.OrdinalIgnoreCase));
                    if (localType != null)
                    {
                        return localType;
                    }
                }
                return MetaRepository.Instance.ResolveType("http://nmf.codeplex.com/nmeta/#//" + type);
            }
            else
            {
                return MetaRepository.Instance.ResolveType(type);
            }
        }


        private static ObservingFunc<ITypedElement, string> GetBoundsString = Observable.Func<ITypedElement, string>
            (t => (t.LowerBound == t.UpperBound ? "" : (t.LowerBound.ToString() + "..")) + (t.UpperBound == -1 ? "*" : t.UpperBound.ToString()));

        private static void UpdateBounds(ITypedElement reference, string bounds)
        {
            if (bounds == null)
            {
                return;
            }
            bounds = bounds.Trim();
            var sep = bounds.IndexOf("..");
            if (sep == -1)
            {
                if (bounds == "*")
                {
                    reference.UpperBound = -1;
                    reference.LowerBound = 0;
                }
                else if (int.TryParse(bounds, out var upperBound))
                {
                    reference.UpperBound = upperBound;
                    reference.LowerBound = upperBound;
                }
            }
            else if (int.TryParse(bounds.Substring(0, sep), out var lowerBound) && sep < bounds.Length + 2)
            {
                reference.LowerBound = lowerBound;
                var upperBoundString = bounds.Substring(sep + 2);
                if (upperBoundString == "*")
                {
                    reference.UpperBound = -1;
                }
                else if (int.TryParse(upperBoundString, out var upperBound))
                {
                    reference.UpperBound = upperBound;
                }
            }
        }

        [GeneratedRegex(@"(\d+\.\.)?(\d+|\*)", RegexOptions.Compiled)]
        private static partial Regex BoundsRegex();

        [GeneratedRegex(@"\w+")]
        private static partial Regex IdentifierRegex();

        private static string GetUnassignedName(string template, IEnumerable<string> assignedNames)
        {
            if (!assignedNames.Contains(template))
            {
                return template;
            }

            var index = 1;
            var attempt = template + index;
            while (assignedNames.Contains(attempt))
            {
                index++;
                attempt = template + index;
            }
            return attempt;
        }
    }
}
