using NMF.Transformations;
using NMF.Models.Meta;
using System.Collections.Generic;
using System.Linq;
using NMF.Transformations.Core;
using NMF.Utilities;
using NMF.Models;
using NMF.Models.Repository;
using System;

namespace NMF.Interop.Ecore.Transformations
{
    /// <summary>
    /// Denotes the transformation from Ecore to NMeta
    /// </summary>
    public class Ecore2MetaTransformation : ReflectiveTransformation
    {
        /// <summary>
        /// True, if primitives types should be generated, otherwise False
        /// </summary>
        public static bool GeneratePrimitiveTypes { get; set; }

        /// <summary>
        /// Gets or sets a dictionary with custom types
        /// </summary>
        public static IDictionary<string, string> CustomTypesMap { get; set; }

        private static readonly Dictionary<string, IPrimitiveType> classesDict = new Dictionary<string, IPrimitiveType>();
        private static readonly IType eObject = MetaRepository.Instance.ResolveClass(typeof(EObject));

        static Ecore2MetaTransformation()
        {
#pragma warning disable S1075 // URIs should not be hardcoded
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
#pragma warning restore S1075 // URIs should not be hardcoded
        }

        /// <summary>
        /// Denotes the transformation rule from named elements to meta elements
        /// </summary>
        public class ENamedElement2MetaElement : AbstractTransformationRule<IENamedElement, IMetaElement>
        {
            /// <inheritdoc />
            public override void Transform(IENamedElement input, IMetaElement output, ITransformationContext context)
            {
                if (!(input is IEDataType) || (input is IEEnum))
                {
                    output.Name = input.Name.ToString();

                    var extendedMetaData = input.EAnnotations.FirstOrDefault(o => o.Source.Equals("http:///org/eclipse/emf/ecore/util/ExtendedMetaData"));
                    if (extendedMetaData != null)
                    {
                        var name = extendedMetaData.Details.FirstOrDefault(o => o.Key.Equals("name"));
                        if (name != null)
                        {
                            if (!string.IsNullOrEmpty(name.Value) && !name.Value.Contains(":"))
                            {
                                output.Extensions.Add(new SerializationInformation
                                {
                                    SerializationName = name.Value.Replace("_._type", "")
                                });
                            }
                        }
                    }
                }

                if (input.EAnnotations.Any() && output != null)
                {
                    var annotations = AnnotationSet.FromModelElement(output).Annotations;
                    AddAnnotations(input, annotations);

                    var genModelAnnotation = input.EAnnotations.FirstOrDefault(ann => ann.Source == "http://www.eclipse.org/emf/2002/GenModel");
                    if (genModelAnnotation != null)
                    {
                        var documentation = genModelAnnotation.Details.FirstOrDefault(dt => dt.Key == "documentation");
                        if (documentation != null)
                        {
                            output.Summary = documentation.Value;
                        }
                    }
                }
            }

            private static void AddAnnotations(IEModelElement input, ICollection<IAnnotationEntry> annotations)
            {
                foreach (var a in input.EAnnotations)
                {
                    var annotated = new AnnotationEntry
                    {
                        Source = a.Source
                    };
                    foreach (var detail in a.Details)
                    {
                        annotated.Details.Add($"{detail.Key}={detail.Value}");
                    }
                    AddAnnotations(a, annotated.Annotations);
                    annotations.Add(annotated);
                }
            }
        }

        /// <summary>
        /// Denotes the transformation rule from Ecore packages to NMeta namespaces
        /// </summary>
        public class EPackage2Namespace : TransformationRule<IEPackage, INamespace>
        {
            /// <inheritdoc />
            public override void Transform(IEPackage input, INamespace output, ITransformationContext context)
            {
                System.Uri uri;
                if (System.Uri.TryCreate(input.NsURI, System.UriKind.RelativeOrAbsolute, out uri))
                {
                    output.Uri = uri;
                    output.Prefix = input.NsPrefix;

                    if (input.ESuperPackage == null)
                    {
                        var model = new Model();
                        model.ModelUri = uri;
                        model.RootElements.Add(output);
                    }
                }
            }

            /// <inheritdoc />
            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<ENamedElement2MetaElement>());

                CallMany(Rule<EPackage2Namespace>(),
                    selector: package => package.ESubpackages,
                    persistor: (ns, subNamespaces) => ns.ChildNamespaces.AddRange(subNamespaces));

                CallMany(Rule<EClassifier2Type>(),
                    selector: package => package.EClassifiers,
                    persistor: (ns, types) => ns.Types.AddRange(types));

                Call(this, package => package.ESuperPackage);
            }
        }

        /// <summary>
        /// Denotes the transformation rule from classifiers to types
        /// </summary>
        public class EClassifier2Type : AbstractTransformationRule<IEClassifier, IType>
        {
            /// <inheritdoc />
            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<ENamedElement2MetaElement>());

                Require(Rule<EPackage2Namespace>(), selector: classifier => classifier.EPackage,
                    filter: classifier => !(classifier is IEDataType) || (classifier is IEEnum));
            }
        }


        private static IEnumerable<IEReference> SubsetsReferences(IEStructuralFeature f)
        {
            if (f is not EReference r) return Enumerable.Empty<IEReference>();
            var subset = r.EAnnotations.FirstOrDefault(a => a.Source == "subsets");
            if (subset != null)
            {
                return subset.References.OfType<IEReference>();
            }
            return Enumerable.Empty<IEReference>();
        }

        private static bool IsUnion(IEStructuralFeature f)
        {
            return f.EAnnotations.Any(a => a.Source == "union");
        }

        /// <summary>
        /// Denotes the transformation rule from classes to NMeta classes
        /// </summary>
        public class EClass2Class : TransformationRule<IEClass, IClass>
        {
            /// <inheritdoc />
            public override void Transform(IEClass input, IClass output, ITransformationContext context)
            {
                output.IsAbstract = input.Abstract.GetValueOrDefault() || input.Interface.GetValueOrDefault();

                var identifier = input.EStructuralFeatures.OfType<EAttribute>().FirstOrDefault(att => att.ID.GetValueOrDefault());
                if (identifier != null)
                {
                    output.Identifier = context.Trace.ResolveIn(Rule<EAttribute2Attribute>(), identifier);
                    output.IdentifierScope = IdentifierScope.Global;
                }

                if (output.Identifier == null && output.RetrieveIdentifier().Identifier == null)
                {
                    // if no attribute is the xmi id and the class has a name attribute, this attribute is used as a local identifier
                    var nameAttribute = input.EStructuralFeatures.OfType<EAttribute>().FirstOrDefault(att => string.Equals(att.Name, "Name",
                        System.StringComparison.InvariantCultureIgnoreCase));

                    if (nameAttribute != null)
                    {
                        output.Identifier = context.Trace.ResolveIn(Rule<EAttribute2Attribute>(), nameAttribute);
                        output.IdentifierScope = IdentifierScope.Local;
                    }
                }
            }

            private bool IsContainerReference(IEStructuralFeature f)
            {
                if (f is not EReference r) return false;
                return r.EOpposite != null && r.EOpposite.Containment.GetValueOrDefault();
            }

            /// <inheritdoc />
            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<EClassifier2Type>());

                CallMany(Rule<EStructuralFeature2Property>(),
                    selector: cl => cl.EStructuralFeatures.Where(f => !f.Derived.GetValueOrDefault() || IsContainerReference(f) || SubsetsReferences(f).Any(r => r.Containment.GetValueOrDefault())),
                    persistor: (cl, properties) => {
                        cl.Attributes.AddRange(properties.OfType<IAttribute>());
                        cl.References.AddRange(properties.OfType<IReference>());
                    });

                Call(Rule<CleanupRefines>());

                RequireMany(Rule<EClass2Class>(),
                    selector: cl => cl.ESuperTypes,
                    persistor: (cl, superTypes) => cl.BaseTypes.AddRange(superTypes));

                CallMany(Rule<EOperation2Operation>(),
                    selector: cl => cl.EOperations.Where(op => !IsNameClash(op, cl)),
                    persistor: (cl, operations) => cl.Operations.AddRange(operations));

                Call(Rule<EPackage2Namespace>(), cl => cl.EPackage);
            }

            private bool IsNameClash(IEOperation operation, IEClass cl)
            {
                return cl.Closure(c => c.ESuperTypes)
                    .SelectMany(c => c.EStructuralFeatures)
                    .Any(f => string.Equals(f.Name, operation.Name, StringComparison.OrdinalIgnoreCase));
            }
        }

        /// <summary>
        /// Denotes a rule to cleanup refines references
        /// </summary>
        public class CleanupRefines : InPlaceTransformationRule<IEClass>
        {
            /// <inheritdoc />
            public override void RegisterDependencies()
            {
                TransformationDelayLevel = 1;
            }

            /// <inheritdoc />
            public override void Transform(IEClass input, ITransformationContext context)
            {
                var c = context.Trace.ResolveIn(Rule<EClass2Class>(), input);
                foreach (var r in c.References.Where(r => r.Refines != null))
                {
                    r.Refines.IsUnique = false;
                    r.IsContainment = r.Refines.IsContainment;
                }
            }
        }

        /// <summary>
        /// Denotes the transformation rule from enumerations to NMeta enumerations
        /// </summary>
        public class EEnum2Enumeration : TransformationRule<IEEnum, IEnumeration>
        {
            /// <inheritdoc />
            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<EClassifier2Type>());

                RequireMany(Rule<EEnumLiteral2Literal>(),
                    selector: en => en.ELiterals,
                    persistor: (en, literals) => en.Literals.AddRange(literals));
            }
        }

        /// <summary>
        /// Denotes the transformation rule from data types to primitive types
        /// </summary>
        public class EDataType2PrimitiveType : TransformationRule<IEDataType, IPrimitiveType>
        {
            /// <inheritdoc />
            public override IPrimitiveType CreateOutput(IEDataType input, ITransformationContext context)
            {
                if (GeneratePrimitiveTypes && (input.EPackage == null || input.EPackage.NsURI != "http://www.eclipse.org/emf/2002/Ecore"))
                {
                    return new PrimitiveType();
                }
                return null;
            }

            /// <inheritdoc />
            public override void Transform(IEDataType input, IPrimitiveType output, ITransformationContext context)
            {
                if (output != null)
                {
                    IPrimitiveType primType;
                    if (classesDict.TryGetValue(input.InstanceClassName, out primType))
                    {
                        output.SystemType = primType.SystemType;
                    }
                    else
                    {
                        string custom;
                        if (CustomTypesMap != null && input.InstanceClassName != null && CustomTypesMap.TryGetValue(input.InstanceClassName, out custom))
                        {
                            output.SystemType = custom;
                        }
                        else
                        {
                            output.SystemType = input.InstanceClassName;
                        }
                    }
                }
            }

            /// <inheritdoc />
            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<EClassifier2Type>());
            }
        }

        /// <summary>
        /// Denotes the transformation rule from literals to NMeta literals
        /// </summary>
        public class EEnumLiteral2Literal : TransformationRule<IEEnumLiteral, ILiteral>
        {
            /// <inheritdoc />
            public override void Transform(IEEnumLiteral input, ILiteral output, ITransformationContext context)
            {
                output.Value = input.Value;
            }

            /// <inheritdoc />
            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<ENamedElement2MetaElement>());
            }
        }

        /// <summary>
        /// Denotes the abstract transformation rule for typed elements
        /// </summary>
        public class ETypedElement2TypedElement : AbstractTransformationRule<IETypedElement, ITypedElement>
        {
            /// <inheritdoc />
            public override void Transform(IETypedElement input, ITypedElement output, ITransformationContext context)
            {
                output.IsOrdered = input.Ordered.GetValueOrDefault(true);
                output.IsUnique = input.Unique.GetValueOrDefault();
                output.LowerBound = input.LowerBound.GetValueOrDefault(0);
                output.UpperBound = input.UpperBound.GetValueOrDefault(1);
            }

            /// <inheritdoc />
            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<ENamedElement2MetaElement>());

                Require(Rule<EClassifier2Type>(),
                    selector: el => el.EType,
                    persistor: (el, type) => el.Type = type);
            }
        }

        /// <summary>
        /// Denotes the transformation rule from structural features to attributes or references
        /// </summary>
        public class EStructuralFeature2Property : AbstractTransformationRule<IEStructuralFeature, ITypedElement>
        {
            /// <inheritdoc />
            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<ETypedElement2TypedElement>());

                Call(Rule<EClassifier2Type>(), f => f.EContainingClass);
            }
        }

        /// <summary>
        /// Denotes the transformation rule for references
        /// </summary>
        public class EReference2Property : TransformationRule<IEReference, IReference>
        {
            /// <inheritdoc />
            public override void Transform(IEReference input, IReference output, ITransformationContext context)
            {
                output.IsContainment = input.Containment.GetValueOrDefault();
                // if output is a containment, we set ordered to true, because EMF tends to use indices also for unordered collections
                if (output.IsContainment)
                {
                    output.IsOrdered = true;
                }

                if (output.DeclaringType == null)
                {
                    output.DeclaringType = context.Trace.ResolveIn(Rule<EClass2Class>(), input.EContainingClass);
                }

                foreach (var baseInterface in input.EContainingClass.ESuperTypes.Where(t => t.Interface.GetValueOrDefault()))
                {
                    var baseReference = baseInterface.EStructuralFeatures.OfType<EReference>().FirstOrDefault(r => r.Name == input.Name);
                    if (baseReference != null)
                    {
                        SetRefines(output, context.Trace.ResolveIn(this, baseReference));
                    }
                }

                if (IsUnion(input))
                {
                    output.IsUnique = false;
                }

                foreach (var subsetted in SubsetsReferences(input))
                {
                    SetRefines(output, context.Trace.ResolveIn(this, subsetted));
                }

                if (output.Type == eObject || (output.Type != null && output.Type.Name == eObject.Name)) output.Type = null;
            }

            private void SetRefines(IReference input, IReference refined)
            {
                if (refined == null) return;

                if (input.DeclaringType.As<IClass>().Closure(t => t.BaseTypes).Contains(refined.DeclaringType.As<IClass>())) 
                {
                    input.Refines = refined;
                }
                else
                {
                    Console.Error.WriteLine($"{input.Name} defined in {input.DeclaringType.Name} is supposed to refine {refined.Name} defined in {refined.DeclaringType.Name}, but {refined.DeclaringType.Name} is not a base type of {input.DeclaringType.Name}!");
                }
            }

            /// <inheritdoc />
            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<EStructuralFeature2Property>());

                Call(Rule<EReference2Property>(),
                    selector: r => r.EOpposite,
                    persistor: (prop, opposite) => prop.Opposite = opposite);
            }
        }

        /// <summary>
        /// Denotes the transformation rule for attributes
        /// </summary>
        public class EAttribute2Attribute : TransformationRule<IEAttribute, IAttribute>
        {
            /// <inheritdoc />
            public override void Transform(IEAttribute input, IAttribute output, ITransformationContext context)
            {
                output.DefaultValue = input.DefaultValueLiteral;

                if (input.EType is IEDataType eDataType && eDataType.InstanceClassName != null && classesDict.TryGetValue(eDataType.InstanceClassName, out IPrimitiveType type))
                {
                    output.Type = type;
                }
                output.Type ??= classesDict["java.lang.Object"];

                var extendedMetaData = input.EAnnotations.FirstOrDefault(o => o.Source.Equals("http:///org/eclipse/emf/ecore/util/ExtendedMetaData"));
                if (extendedMetaData != null)
                {
                    var name = extendedMetaData.Details.FirstOrDefault(o => o.Key == "name");
                    var kind = extendedMetaData.Details.FirstOrDefault(o => o.Key == "kind");
                    if (name != null && kind != null && kind.Value == "simple")
                    {
                        var serializationInfo = output.GetExtension<SerializationInformation>();
                        if (serializationInfo == null)
                        {
                            serializationInfo = new SerializationInformation();
                            output.Extensions.Add(serializationInfo);
                        }
                        serializationInfo.IsDefault = true;
                    }
                }
            }

            /// <inheritdoc />
            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<EStructuralFeature2Property>());
            }
        }

        /// <summary>
        /// Denotes the transformation rule for operations
        /// </summary>
        public class EOperation2Operation : TransformationRule<IEOperation, IOperation>
        {
            /// <inheritdoc />
            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<ETypedElement2TypedElement>());

                RequireMany(Rule<EParameter2Parameter>(),
                    selector: op => op.EParameters,
                    persistor: (op, parameters) => op.Parameters.AddRange(parameters));
            }

            /// <inheritdoc />
            public override void Transform(IEOperation input, IOperation output, ITransformationContext context)
            {
                if (input.EType is IEDataType eDataType && eDataType.InstanceClassName != null)
                {
                    IPrimitiveType type;
                    if (classesDict.TryGetValue(eDataType.InstanceClassName, out type))
                    {
                        output.Type = type;
                    }
                }
            }
        }

        /// <summary>
        /// Denotes the transformation rule for parameters
        /// </summary>
        public class EParameter2Parameter : TransformationRule<IEParameter, IParameter>
        {
            /// <inheritdoc />
            public override void Transform(IEParameter input, IParameter output, ITransformationContext context)
            {
                output.Direction = Direction.In;

                if (input.EType is IEDataType eDataType && eDataType.InstanceClassName != null && classesDict.TryGetValue(eDataType.InstanceClassName, out IPrimitiveType type))
                {
                    output.Type = type;
                }
            }

            /// <inheritdoc />
            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<ETypedElement2TypedElement>());
            }
        }
    }
}
