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
    public class EcoreXMLSchemaTest
    {
        private INamespace ns;

        [TestMethod]
        public void TestSerializedName()
        {
            EPackage package = EcoreInterop.LoadPackageFromFile("EcoreXMLSchemaTest.ecore");
            Assert.IsNotNull(package);

            ns = EcoreInterop.Transform2Meta(package);
            Assert.IsNotNull(ns);
            
            /* Check Types */
            var AnnotationDefaultsType = ns.Types.OfType<IClass>().FirstOrDefault(t => t.Name == "AnnotationDefaultsType");
            AssertSerializationName(AnnotationDefaultsType, "AnnotationDefaults");

            var PType = ns.Types.OfType<IClass>().FirstOrDefault(t => t.Name == "PType");
            AssertSerializationName(PType, "P");

            var ModelInformation = ns.Types.OfType<IClass>().FirstOrDefault(t => t.Name == "ModelInformation");
            AssertNoSerializationName(ModelInformation);

            /* Check References */
            var pRef = AnnotationDefaultsType.References.FirstOrDefault(r => r.Name == "p");
            AssertSerializationName(pRef, "P");

            /* Check Attributes */
            var valueAttr = PType.Attributes.FirstOrDefault(a => a.Name == "value");
            AssertNoSerializationName(valueAttr);

            var sizeAttr = PType.Attributes.FirstOrDefault(a => a.Name == "size");
            AssertSerializationName(sizeAttr, "size");

            var mixedAttr = ModelInformation.Attributes.FirstOrDefault(a => a.Name == "mixed");
            AssertNoSerializationName(mixedAttr);
        }

        private void AssertSerializationName(IMetaElement type, string serializedName)
        {
            Assert.IsNotNull(type);
            var serializationInfo = type.GetExtension<SerializationInformation>();
            Assert.IsNotNull(serializationInfo);
            Assert.AreEqual(serializationInfo.SerializationName, serializedName);
        }

        private void AssertNoSerializationName(IMetaElement type)
        {
            Assert.IsNotNull(type);
            var serializationInfo = type.GetExtension<SerializationInformation>();
            Assert.IsNull(serializationInfo);
        }
    }
}
