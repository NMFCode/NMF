using NMF.Transformations;
using NMF.Transformations.Core;
using NMF.Utilities;
using NMF.Interop.Legacy.Cmof;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace NMF.Interop.Transformations
{
    internal class LegacyCmof2NMeta : ReflectiveTransformation
    {
        public class Package2Namespace : TransformationRule<IPackage, NMF.Models.Meta.INamespace>
        {
            public override void Transform(IPackage input, Models.Meta.INamespace output, ITransformationContext context)
            {
                if (Uri.TryCreate(input.URI, UriKind.RelativeOrAbsolute, out var uri))
                {
                    output.Uri = uri;
                }
            }

            public override void RegisterDependencies()
            {
                CallMany(this,
                    selector: p => p.OwnedMember.OfType<IPackage>(),
                    persistor: (ns, packages) => ns.ChildNamespaces.AddRange(packages));

                CallMany(Rule<Class2Class>(),
                    selector: p => p.OwnedMember.OfType<IClass>(),
                    persistor: (ns, classes) => ns.Types.AddRange(classes));

                CallMany(Rule<DataType2DataType>(),
                    selector: p => p.OwnedMember.OfType<IDataType>(),
                    persistor: (ns, classes) => ns.Types.AddRange(classes));

                CallMany(Rule<Enum2Enum>(),
                    selector: p => p.OwnedMember.OfType<IEnumeration>(),
                    persistor: (ns, classes) => ns.Types.AddRange(classes));

                MarkInstantiatingFor(Rule<Element2Element>());
            }
        }

        public class Class2Class : TransformationRule<IClass, NMF.Models.Meta.IClass>
        {
            public override void Transform(IClass input, Models.Meta.IClass output, ITransformationContext context)
            {
                output.Name = input.Name;
                output.IsAbstract = input.IsAbstract;
            }

            public override void RegisterDependencies()
            {
                CallMany(this,
                    selector: c => c.SuperClass,
                    persistor: (c, baseTypes) => c.BaseTypes.AddRange(baseTypes));

                CallMany(Rule<Property2Attribute>(),
                    selector: c => from p in c.OwnedAttribute
                                   where p.Type is IDataType && !p.IsDerived
                                   select p,
                    persistor: (c, attributes) => c.Attributes.AddRange(attributes));

                CallMany(Rule<Property2Reference>(),
                    selector: c => from p in c.OwnedAttribute
                                   where p.Type is IClass && !p.IsDerived
                                   select p,
                    persistor: (c, references) => c.References.AddRange(references));

                MarkInstantiatingFor(Rule<Classifier2Type>());
            }
        }

        public class DataType2DataType : TransformationRule<IDataType, NMF.Models.Meta.IDataType>
        {
            public override void Transform(IDataType input, Models.Meta.IDataType output, ITransformationContext context)
            {
                output.Name = input.Name;
            }

            public override void RegisterDependencies()
            {
                CallMany(Rule<Property2Attribute>(),
                    selector: c => from p in c.OwnedAttribute
                                   where p.Type is IDataType && !p.IsDerived
                                   select p,
                    persistor: (c, attributes) => c.Attributes.AddRange(attributes));

                MarkInstantiatingFor(Rule<Classifier2Type>());
            }
        }

        public class Element2Element : AbstractTransformationRule<INamedElement, NMF.Models.Meta.IMetaElement>
        {
            public override void Transform(INamedElement input, Models.Meta.IMetaElement output, ITransformationContext context)
            {
                output.Name = input.Name;

                foreach (var comment in input.OwnedComment)
                {
                    output.Summary += comment.Body;
                }
            }
        }

        public class Classifier2Type : AbstractTransformationRule<IType, NMF.Models.Meta.IType>
        {
            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<Element2Element>());
            }
        }

        public class Property2Feature : AbstractTransformationRule<IProperty, NMF.Models.Meta.ITypedElement>
        {
            public override void Transform(IProperty input, Models.Meta.ITypedElement output, ITransformationContext context)
            {
                output.IsOrdered = input.IsOrdered;
                output.IsUnique = input.IsUnique;
                output.LowerBound = input.Lower ?? 0;
                output.UpperBound = input.Upper ?? 1;
            }

            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<Element2Element>());
            }
        }

        public class Property2Attribute : TransformationRule<IProperty, NMF.Models.Meta.IAttribute>
        {
            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<Property2Feature>());

                Call(Rule<DataType2DataType>(),
                    selector: p => p.Type as IDataType,
                    persistor: (att, type) => att.Type = type);
            }
        }

        public class Property2Reference : TransformationRule<IProperty, NMF.Models.Meta.IReference>
        {
            public override void Transform(IProperty input, Models.Meta.IReference output, ITransformationContext context)
            {
                output.IsContainment = input.IsComposite;

                if (input.RedefinedProperty.Any())
                {
                    output.Refines = context.Trace.ResolveManyIn(this, input.RedefinedProperty).FirstOrDefault();
                }

                if (input.Association != null && input.Association.MemberEnd.Count == 2)
                {
                    output.Opposite = context.Trace.ResolveIn(this, input.Association.MemberEnd.Except(input).Single());
                }
            }

            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<Property2Feature>());

                Call(Rule<Class2Class>(),
                    selector: p => p.Type as IClass,
                    persistor: (p, type) => p.Type = type);
            }
        }

        public class Operation2Operation : TransformationRule<IOperation, NMF.Models.Meta.IOperation>
        {
            public override void RegisterDependencies()
            {
                CallMany(Rule<Parameter2Parameter>(),
                    selector: op => op.OwnedParameter.Where(p => p.Direction == ParameterDirectionKind.In),
                    persistor: (op, parameters) => op.Parameters.AddRange(parameters));

                Call(Rule<Classifier2Type>(),
                    selector: op => op.OwnedParameter.FirstOrDefault(p => p.Direction == ParameterDirectionKind.Return)?.Type,
                    persistor: (op, returnType) => op.Type = returnType);

                MarkInstantiatingFor(Rule<Element2Element>());
            }
        }

        public class Parameter2Parameter : TransformationRule<IParameter, NMF.Models.Meta.IParameter>
        {
            public override void RegisterDependencies()
            {
                Call(Rule<Classifier2Type>(),
                    selector: p => p.Type,
                    persistor: (p, type) => p.Type = type);

                MarkInstantiatingFor(Rule<Element2Element>());
            }
        }

        public class Enum2Enum : TransformationRule<IEnumeration, NMF.Models.Meta.IEnumeration>
        {

            public override void RegisterDependencies()
            {
                CallMany(Rule<Literal2Literal>(),
                    selector: en => en.OwnedLiteral,
                    persistor: (en, literals) => en.Literals.AddRange(literals));

                MarkInstantiatingFor(Rule<Element2Element>());
            }
        }

        public class Literal2Literal : TransformationRule<IEnumerationLiteral, NMF.Models.Meta.ILiteral>
        {
            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<Element2Element>());
            }
        }

        private static int? Retrieve(IValueSpecification valueSpecification)
        {
            if (valueSpecification is OpaqueExpression opaque && opaque.Body.Any() && int.TryParse(opaque.Body.First(), out var intValue))
            {
                return intValue;
            }
            return null;
        }
    }
}
