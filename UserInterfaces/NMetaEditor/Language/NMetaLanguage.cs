using NMF.Expressions;
using NMF.Glsp.Language;
using NMF.Models.Meta;
using NMF.Models.Repository;
using System.Text.RegularExpressions;

namespace NMetaEditor.Language
{
    internal partial class NMetaLanguage : GraphicalLanguage
    {
        public override string DiagramType => "nmeta";
        public override DescriptorBase StartRule => Descriptor<NamespaceDescriptor>();

        public class NamespaceDescriptor : NodeDescriptor<INamespace>
        {
            protected override void DefineLayout()
            {
                Label(n => n.Name);
                using (Compartment("comp:types"))
                {
                    Nodes(D<TypeDescriptor>(), n => n.Types);
                }
            }
        }

        public class TypeDescriptor : NodeDescriptor<IType>
        {

            protected override void DefineLayout()
            {
                // intentionally left empty
            }
        }

        public class ReferenceTypeDescriptor : NodeDescriptor<IReferenceType>
        {
            protected override void DefineLayout()
            {
                Refine(D<TypeDescriptor>());

                // intentionally left empty
            }
        }

        public class EnumerationDescriptor : NodeDescriptor<IEnumeration>
        {
            protected override void DefineLayout()
            {
                Refine(D<TypeDescriptor>());
                
                Label(e => e.Name);
                using(Compartment("literals"))
                {
                    Nodes(D<LiteralDescriptor>(), e => e.Literals);
                }
            }

            public override IEnumeration CreateElement(string profile)
            {
                return new Enumeration { Name = "NewEnumeration" };
            }
        }

        public class LiteralDescriptor : NodeDescriptor<ILiteral>
        {
            protected override void DefineLayout()
            {
                Label(l => l.Name);
            }

            public override ILiteral CreateElement(string profile)
            {
                return new Literal { Name = "Literal" };
            }
        }

        public class ClassDescriptor : NodeDescriptor<IClass>
        {
            protected override void DefineLayout()
            {
                Refine(D<ReferenceTypeDescriptor>());

                Label(e => e.Name);
                using (Compartment("attributes"))
                {
                    Nodes(D<AttributeDescriptor>(), e => e.Attributes);
                }
            }
        }

        public partial class AttributeDescriptor : NodeDescriptor<IAttribute>
        {

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
        }

        public partial class ReferenceDescriptor : EdgeDescriptor<IReference>
        {

            protected override void DefineLayout()
            {
                Label(r => r.Name).At(0.5);
                Label(r => GetBoundsString.Evaluate(r))
                    .At(0.75)
                    .Validate(BoundsRegex(), "not a valid bounds string")
                    .WithSetter(UpdateBounds);

                SourceNode(D<ReferenceTypeDescriptor>(), r => r.DeclaringType);
                TargetNode(D<ReferenceTypeDescriptor>(), r => r.ReferenceType);
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
    }
}
