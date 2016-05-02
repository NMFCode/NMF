using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using NMF.Models.Meta;
using NMF.Tests;
using NMF.Transformations;
using System.CodeDom;
using NMF.Models.Repository.Serialization;
using NMF.Models;
using System;

namespace NMF.Interop.Ecore.Tests
{
    [TestClass]
    public class NMMTests
    {
        INamespace metaNamespace;

        IClass type;
        IClass @class;
        IClass structuredType;
        IClass typedElement;
        IClass metaElement;
        IClass modelElement;
        IClass referenceType;
        IClass dataType;
        IClass primitiveType;
        IClass @namespace;
        IClass extension;
        IClass @event;
        IClass operation;
        IClass parameter;
        IClass enumeration;
        IClass literal;
        IClass reference;
        IClass attribute;

        IEnumeration direction;

        [TestMethod]
        public void LoadNMeta()
        {
            EPackage package;

            package = EcoreInterop.LoadPackageFromFile("NMeta.ecore");

            Assert.IsNotNull(package);

            metaNamespace = EcoreInterop.Transform2Meta(package);

            var boolean = metaNamespace.Resolve(new Uri("Boolean", UriKind.Relative));
            var isInterface = metaNamespace.Resolve(new Uri("Class/IsInterface", UriKind.Relative)) as IAttribute;

            Assert.IsNotNull(metaNamespace);

            Assert.AreEqual(20, metaNamespace.Types.OfType<IClass>().Count());

            type = GetClass("Type");
            @class = GetClass("Class");
            structuredType = GetClass("StructuredType");
            typedElement = GetClass("ITypedElement");
            metaElement = GetClass("MetaElement");
            attribute = GetClass("Attribute");
            reference = GetClass("Reference");
            referenceType = GetClass("ReferenceType");
            dataType = GetClass("DataType");
            primitiveType = GetClass("PrimitiveType");
            @namespace = GetClass("Namespace");
            extension = GetClass("Extension");
            @event = GetClass("Event");
            operation = GetClass("Operation");
            parameter = GetClass("Parameter");
            enumeration = GetClass("Enumeration");
            literal = GetClass("Literal");
            modelElement = GetClass("ModelElement");

            direction = metaNamespace.Types.OfType<IEnumeration>().FirstOrDefault(en => en.Name == "Direction");
            Assert.IsNotNull(direction);
            direction.Literals.Select(l => l.Name).AssertSequence("In", "Out", "InOut");

            AssertBaseTypes();
            AssertProperties();

            var model = metaNamespace.Model;
            Assert.IsNotNull(model);

            Assert.AreSame(type, model.Resolve("#//Type"));
            Assert.AreSame(@class, model.Resolve("#//Class"));
            Assert.AreSame(structuredType, model.Resolve("#//StructuredType"));
        }

        private void AssertProperties()
        {
            AssertReference(type, "Namespace", @namespace, 1, true);
            Assert.AreEqual(1, type.References.Count);
            AssertAttribute(@class, "IsInterface", typeof(bool));
            AssertAttribute(@class, "IsAbstract", typeof(bool));
            AssertReference(@class, "BaseTypes", @class, -1);
            AssertReference(@class, "Identifier", attribute, 1);
            Assert.AreEqual(2, @class.References.Count);
            Assert.AreEqual(2, @class.Attributes.Count);
            AssertReference(referenceType, "References", reference, -1, true, true);
            AssertReference(referenceType, "Events", @event, -1, true, true);
            Assert.AreEqual(2, referenceType.References.Count);
            Assert.AreEqual(0, dataType.References.Count);
            AssertAttribute(reference, "IsContainment", typeof(bool));
            AssertReference(reference, "DeclaringType", referenceType, 1, true);
            AssertReference(reference, "Opposite", reference, 1, true);
            AssertReference(reference, "Implements", reference, 1);
            Assert.AreEqual(4, reference.References.Count);
            AssertReference(@namespace, "ParentNamespace", @namespace, 1, true);
            AssertReference(@namespace, "ChildNamespaces", @namespace, -1, true, true);
            AssertReference(@namespace, "Types", type, -1, true, true);
            Assert.AreEqual(3, @namespace.References.Count);
            AssertAttribute(metaElement, "Name", typeof(string));
            AssertAttribute(metaElement, "Summary", typeof(string));
            AssertAttribute(metaElement, "Remarks", typeof(string));
            Assert.AreEqual(3, metaElement.Attributes.Count);
            AssertReference(structuredType, "Attributes", attribute, -1, true, true);
            AssertReference(structuredType, "Operations", operation, -1, true, true);
            Assert.AreEqual(2, structuredType.References.Count);
            AssertReference(enumeration, "Literals", literal, -1, true, true);
            AssertAttribute(enumeration, "IsFlagged", typeof(bool));
            Assert.AreEqual(1, enumeration.Attributes.Count);
            Assert.AreEqual(1, enumeration.References.Count);
            AssertReference(literal, "Enumeration", enumeration, 1, true);
            AssertAttribute(literal, "Value", typeof(int));
            Assert.AreEqual(1, literal.Attributes.Count);
            Assert.AreEqual(1, literal.References.Count);
            AssertAttribute(primitiveType, "SystemType", typeof(string));
            Assert.AreEqual(1, primitiveType.Attributes.Count);
            AssertReference(extension, "AdornedClass", @class, 1);
            Assert.AreEqual(1, extension.References.Count);
            AssertReference(@event, "Type", type, 1);
            AssertReference(@event, "DeclaringType", referenceType, 1, true);
            Assert.AreEqual(2, @event.References.Count);
            AssertReference(operation, "Parameters", parameter, -1, true, true);
            AssertReference(operation, "DeclaringType", structuredType, 1, true);
            AssertReference(operation, "Implements", operation, 1);
            Assert.AreEqual(3, operation.References.Count);
            AssertReference(typedElement, "Type", type, 1);
            AssertAttribute(typedElement, "IsOrdered", typeof(bool));
            AssertAttribute(typedElement, "IsUnique", typeof(bool));
            AssertAttribute(typedElement, "LowerBound", typeof(int));
            AssertAttribute(typedElement, "UpperBound", typeof(int));
            Assert.AreEqual(4, typedElement.Attributes.Count);
            AssertAttribute(parameter, "Direction", null);
            AssertReference(parameter, "Operation", operation, 1, true);
        }

        private void AssertBaseTypes()
        {
            type.BaseTypes.AssertSequence(metaElement);
            @class.BaseTypes.AssertSequence(referenceType);
            referenceType.BaseTypes.AssertSequence(structuredType);
            structuredType.BaseTypes.AssertSequence(type);
            typedElement.BaseTypes.AssertSequence();
            metaElement.BaseTypes.AssertSequence(modelElement);
            attribute.BaseTypes.AssertSequence(metaElement, typedElement);
            reference.BaseTypes.AssertSequence(metaElement, typedElement);
            dataType.BaseTypes.AssertSequence(structuredType);
            primitiveType.BaseTypes.AssertSequence(type);
            @namespace.BaseTypes.AssertSequence(metaElement);
            extension.BaseTypes.AssertSequence(referenceType);
            @event.BaseTypes.AssertSequence(metaElement);
            operation.BaseTypes.AssertSequence(metaElement, typedElement);
            parameter.BaseTypes.AssertSequence(metaElement, typedElement);
            enumeration.BaseTypes.AssertSequence(type);
            literal.BaseTypes.AssertSequence(metaElement);
        }

        private IClass GetClass(string name)
        {
            Assert.IsNotNull(metaNamespace);
            var type = metaNamespace.Types.OfType<IClass>().FirstOrDefault(t => t.Name == name);
            Assert.IsNotNull(type);
            return type;
        }

        private void AssertAttribute(IClass declaringClass, string name, System.Type systemType)
        {
            var property = declaringClass.Attributes.FirstOrDefault(p => p.Name == name);
            Assert.IsNotNull(property);
            if (systemType != null)
            {
                var primitive = property.Type as IPrimitiveType;
                Assert.IsNotNull(primitive);
                Assert.IsTrue(systemType.FullName == primitive.SystemType || primitive.SystemType == systemType.Name);
            }
        }

        private void AssertReference(IClass declaringClass, string name, IType type, int upperBound, bool hasOpposite = false, bool isContainment = false)
        {
            var property = declaringClass.References.FirstOrDefault(p => p.Name == name);
            Assert.IsNotNull(property);
            Assert.AreSame(property, declaringClass.Resolve(new Uri(name, UriKind.Relative)));
            if (type != null) Assert.AreSame(type, property.Type);
            Assert.AreEqual(upperBound, property.UpperBound);
            Assert.AreEqual(isContainment, property.IsContainment);
            if (hasOpposite)
            {
                Assert.IsNotNull(property.Opposite);
            }
            else
            {
                Assert.IsNull(property.Opposite);
            }
        }
    }
}
