using NMF.Transformations;
using NMF.Models.Meta;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMF.Transformations.Core;
using NMF.Utilities;
using NMF.Models;
using System.Diagnostics;
using NMF.Models.Repository;

namespace NMF.Interop.Ecore.Transformations
{
    public class Ecore2MetaTransformation : ReflectiveTransformation
    {
        private static Dictionary<string, IPrimitiveType> classesDict = new Dictionary<string, IPrimitiveType>();

        static Ecore2MetaTransformation()
        {
            classesDict.Add("byte", (IPrimitiveType)MetaRepository.Instance.Resolve("http://nmf.codeplex.com/nmeta/#//Byte"));
            classesDict.Add("byte[]", (IPrimitiveType)MetaRepository.Instance.Resolve("http://nmf.codeplex.com/nmeta/#//ByteArray"));
            classesDict.Add("int", (IPrimitiveType)MetaRepository.Instance.Resolve("http://nmf.codeplex.com/nmeta/#//Integer"));
            classesDict.Add("long", (IPrimitiveType)MetaRepository.Instance.Resolve("http://nmf.codeplex.com/nmeta/#//Long"));
            classesDict.Add("boolean", (IPrimitiveType)MetaRepository.Instance.Resolve("http://nmf.codeplex.com/nmeta/#//Boolean"));
            classesDict.Add("double", (IPrimitiveType)MetaRepository.Instance.Resolve("http://nmf.codeplex.com/nmeta/#//Double"));
            classesDict.Add("float", (IPrimitiveType)MetaRepository.Instance.Resolve("http://nmf.codeplex.com/nmeta/#//Float"));
            classesDict.Add("decimal", (IPrimitiveType)MetaRepository.Instance.Resolve("http://nmf.codeplex.com/nmeta/#//Decimal"));
            classesDict.Add("char", (IPrimitiveType)MetaRepository.Instance.Resolve("http://nmf.codeplex.com/nmeta/#//Char"));
            classesDict.Add("short", (IPrimitiveType)MetaRepository.Instance.Resolve("http://nmf.codeplex.com/nmeta/#//Short"));
            classesDict.Add("java.util.Date", (IPrimitiveType)MetaRepository.Instance.Resolve("http://nmf.codeplex.com/nmeta/#//DateTime"));
            classesDict.Add("java.lang.String", (IPrimitiveType)MetaRepository.Instance.Resolve("http://nmf.codeplex.com/nmeta/#//String"));
            classesDict.Add("java.lang.Object", (IPrimitiveType)MetaRepository.Instance.Resolve("http://nmf.codeplex.com/nmeta/#//Object"));
            classesDict.Add("java.lang.Byte", (IPrimitiveType)MetaRepository.Instance.Resolve("http://nmf.codeplex.com/nmeta/#//Byte"));
            classesDict.Add("java.lang.Integer", (IPrimitiveType)MetaRepository.Instance.Resolve("http://nmf.codeplex.com/nmeta/#//Integer"));
            classesDict.Add("java.lang.Long", (IPrimitiveType)MetaRepository.Instance.Resolve("http://nmf.codeplex.com/nmeta/#//Long"));
            classesDict.Add("java.lang.Boolean", (IPrimitiveType)MetaRepository.Instance.Resolve("http://nmf.codeplex.com/nmeta/#//Boolean"));
            classesDict.Add("java.lang.Double", (IPrimitiveType)MetaRepository.Instance.Resolve("http://nmf.codeplex.com/nmeta/#//Double"));
            classesDict.Add("java.lang.Float", (IPrimitiveType)MetaRepository.Instance.Resolve("http://nmf.codeplex.com/nmeta/#//Float"));
            classesDict.Add("java.lang.Decimal", (IPrimitiveType)MetaRepository.Instance.Resolve("http://nmf.codeplex.com/nmeta/#//Decimal"));
            classesDict.Add("java.lang.Character", (IPrimitiveType)MetaRepository.Instance.Resolve("http://nmf.codeplex.com/nmeta/#//Char"));
            classesDict.Add("java.lang.Short", (IPrimitiveType)MetaRepository.Instance.Resolve("http://nmf.codeplex.com/nmeta/#//Short"));
            classesDict.Add("java.net.URI", (IPrimitiveType)MetaRepository.Instance.Resolve("http://nmf.codeplex.com/nmeta/#//Uri"));
        }

        public class ENamedElement2MetaElement : AbstractTransformationRule<IENamedElement, IMetaElement>
        {
            public override void Transform(IENamedElement input, IMetaElement output, ITransformationContext context)
            {
                if (!(input is IEDataType) || (input is IEEnum))
                {
                    output.Name = input.Name.ToString();
                }
            }
        }

        public class EPackage2Namespace : TransformationRule<IEPackage, INamespace>
        {
            public override void Transform(IEPackage input, INamespace output, ITransformationContext context)
            {
                System.Uri uri;
                if (System.Uri.TryCreate(input.NsURI, System.UriKind.RelativeOrAbsolute, out uri))
                {
                    output.Uri = uri;
                    output.Prefix = input.NsPrefix;
                }
            }

            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<ENamedElement2MetaElement>());

                CallMany(Rule<EPackage2Namespace>(),
                    selector: package => package.ESubpackages,
                    persistor: (ns, subNamespaces) => ns.ChildNamespaces.AddRange(subNamespaces));

                CallMany(Rule<EClassifier2Type>(),
                    selector: package => package.EClassifiers,
                    persistor: (ns, types) => ns.Types.AddRange(types));
            }
        }

        public class EClassifier2Type : AbstractTransformationRule<IEClassifier, IType>
        {
            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<ENamedElement2MetaElement>());

                Require(Rule<EPackage2Namespace>(), selector: classifier => classifier.EPackage,
                    filter: classifier => !(classifier is IEDataType) || (classifier is IEEnum));
            }
        }

        public class EClass2Class : TransformationRule<IEClass, IClass>
        {
            public override void Transform(IEClass input, IClass output, ITransformationContext context)
            {
                output.IsAbstract = input.Abstract.GetValueOrDefault() || input.Interface.GetValueOrDefault();

                var identifier = input.EStructuralFeatures.OfType<EAttribute>().FirstOrDefault(att => att.ID.GetValueOrDefault());
                if (identifier != null)
                {
                    output.Identifier = context.Trace.ResolveIn(Rule<EAttribute2Attribute>(), identifier);
                }

                if (output.Identifier == null)
                {
                    var nameAttribute = input.EStructuralFeatures.OfType<EAttribute>().FirstOrDefault(att => string.Equals(att.Name, "Name",
                        System.StringComparison.InvariantCultureIgnoreCase));

                    if (nameAttribute != null)
                    {
                        output.Identifier = context.Trace.ResolveIn(Rule<EAttribute2Attribute>(), nameAttribute);
                    }
                }
            }

            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<EClassifier2Type>());

                CallMany(Rule<EStructuralFeature2Property>(),
                    selector: cl => cl.EStructuralFeatures.Where(f => !f.Derived.GetValueOrDefault()),
                    persistor: (cl, properties) => {
                        cl.Attributes.AddRange(properties.OfType<IAttribute>());
                        cl.References.AddRange(properties.OfType<IReference>());
                    });

                RequireMany(Rule<EClass2Class>(),
                    selector: cl => cl.ESuperTypes,
                    persistor: (cl, superTypes) => cl.BaseTypes.AddRange(superTypes));

                CallMany(Rule<EOperation2Operation>(),
                    selector: cl => cl.EOperations,
                    persistor: (cl, operations) => cl.Operations.AddRange(operations));
            }
        }

        public class EEnum2Enumeration : TransformationRule<IEEnum, IEnumeration>
        {
            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<EClassifier2Type>());

                RequireMany(Rule<EEnumLiteral2Literal>(),
                    selector: en => en.ELiterals,
                    persistor: (en, literals) => en.Literals.AddRange(literals));
            }
        }

        public class EDataType2PrimitiveType : TransformationRule<IEDataType, IPrimitiveType>
        {
            public override IPrimitiveType CreateOutput(IEDataType input, ITransformationContext context)
            {
                IPrimitiveType type;
                if (input.InstanceClassName != null && classesDict.TryGetValue(input.InstanceClassName, out type))
                {
                    return type;
                }
                else
                {
                    return classesDict["java.lang.Object"];
                }
            }

            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<EClassifier2Type>());
            }
        }

        public class EEnumLiteral2Literal : TransformationRule<IEEnumLiteral, ILiteral>
        {
            public override void Transform(IEEnumLiteral input, ILiteral output, ITransformationContext context)
            {
                output.Value = input.Value;
            }

            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<ENamedElement2MetaElement>());
            }
        }

        public class ETypedElement2TypedElement : AbstractTransformationRule<IETypedElement, ITypedElement>
        {
            public override void Transform(IETypedElement input, ITypedElement output, ITransformationContext context)
            {
                output.IsOrdered = input.Ordered.GetValueOrDefault(true);
                output.IsUnique = input.Unique.GetValueOrDefault();
                output.LowerBound = input.LowerBound.GetValueOrDefault(0);
                output.UpperBound = input.UpperBound.GetValueOrDefault(1);
            }

            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<ENamedElement2MetaElement>());

                Require(Rule<EClassifier2Type>(),
                    selector: el => el.EType,
                    persistor: (el, type) => el.Type = type);
            }
        }

        public class EStructuralFeature2Property : AbstractTransformationRule<IEStructuralFeature, ITypedElement>
        {
            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<ETypedElement2TypedElement>());
            }
        }

        public class EReference2Property : TransformationRule<IEReference, IReference>
        {
            public override void Transform(IEReference input, IReference output, ITransformationContext context)
            {
                output.IsContainment = input.Containment.GetValueOrDefault();

                foreach (var baseInterface in input.EContainingClass.ESuperTypes.Where(t => t.Interface.GetValueOrDefault()))
                {
                    var baseReference = baseInterface.EStructuralFeatures.OfType<EReference>().FirstOrDefault(r => r.Name == input.Name);
                    if (baseReference != null)
                    {
                        output.Refines = context.Trace.ResolveIn(this, baseReference);
                    }
                }
            }

            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<EStructuralFeature2Property>());

                Call(Rule<EReference2Property>(),
                    selector: r => r.EOpposite,
                    persistor: (prop, opposite) => prop.Opposite = opposite);
            }
        }

        public class EAttribute2Attribute : TransformationRule<IEAttribute, IAttribute>
        {
            public override void Transform(IEAttribute input, IAttribute output, ITransformationContext context)
            {
                output.DefaultValue = input.DefaultValueLiteral;
            }

            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<EStructuralFeature2Property>());
            }
        }

        public class EOperation2Operation : TransformationRule<IEOperation, IOperation>
        {
            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<ETypedElement2TypedElement>());

                RequireMany(Rule<EParameter2Parameter>(),
                    selector: op => op.EParameters,
                    persistor: (op, parameters) => op.Parameters.AddRange(parameters));
            }
        }

        public class EParameter2Parameter : TransformationRule<IEParameter, IParameter>
        {
            public override void Transform(IEParameter input, IParameter output, ITransformationContext context)
            {
                output.Direction = Direction.In;
            }

            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<ETypedElement2TypedElement>());
            }
        }
    }
}
