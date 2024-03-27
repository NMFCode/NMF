using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using NMF.Models.Meta;

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
            AssertDefaultProperty(valueAttr);

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
            Assert.IsFalse(serializationInfo.IsDefault);
        }

        private void AssertNoSerializationName(IMetaElement type)
        {
            Assert.IsNotNull(type);
            var serializationInfo = type.GetExtension<SerializationInformation>();
            Assert.IsNull(serializationInfo);
        }

        private void AssertDefaultProperty(ITypedElement feature)
        {
            Assert.IsNotNull(feature);
            var serializationInfo = feature.GetExtension<SerializationInformation>();
            Assert.IsNotNull(serializationInfo);
            Assert.IsTrue(serializationInfo.IsDefault);
        }
    }
}
