using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using NMF.Models.Meta;

namespace NMF.Interop.Ecore.Tests
{
    [TestClass]
    public class XMLTypeTest
    {
        [TestMethod]
        public void LoadXMLType()
        {
            EPackage package = EcoreInterop.LoadPackageFromFile("XMLTypeTest.ecore");
            Assert.IsNotNull(package);

            var xmlTypeNamespace = EcoreInterop.Transform2Meta(package);
            Assert.IsNotNull(xmlTypeNamespace);

            var DataTypes = xmlTypeNamespace.Types.OfType<IClass>().FirstOrDefault(t => t.Name == "DataTypes");
            Assert.IsNotNull(DataTypes);

            AssertAttribute(DataTypes, "AnySimpleType", typeof(object));
            AssertAttribute(DataTypes, "AnyURI", typeof(string));
            AssertAttribute(DataTypes, "Base64Binary", typeof(byte[]));
            AssertAttribute(DataTypes, "Boolean", typeof(bool));
            AssertAttribute(DataTypes, "Byte", typeof(byte));
            AssertAttribute(DataTypes, "Date", typeof(object));
            AssertAttribute(DataTypes, "DateTime", typeof(object));
            //AssertAttribute(DataTypes, "Decimal", typeof(java.math.BigDecimal));
            AssertAttribute(DataTypes, "Double", typeof(double));
            AssertAttribute(DataTypes, "Duration", typeof(object));
            //AssertAttribute(DataTypes, "ENTITIES", typeof(java.util.List));
            AssertAttribute(DataTypes, "ENTITY", typeof(string));
            AssertAttribute(DataTypes, "Float", typeof(float));
            AssertAttribute(DataTypes, "GDay", typeof(object));
            AssertAttribute(DataTypes, "GMonth", typeof(object));
            AssertAttribute(DataTypes, "GMonthDay", typeof(object));
            AssertAttribute(DataTypes, "GYear", typeof(object));
            AssertAttribute(DataTypes, "GYearMonth", typeof(object));
            AssertAttribute(DataTypes, "HexBinary", typeof(byte[]));
            AssertAttribute(DataTypes, "ID", typeof(string));
            AssertAttribute(DataTypes, "IDREF", typeof(string));
            //AssertAttribute(DataTypes, "IDREFS", typeof(java.util.List));
            AssertAttribute(DataTypes, "Int", typeof(int));
            //AssertAttribute(DataTypes, "Integer", typeof(java.math.BigInteger));
            AssertAttribute(DataTypes, "Language", typeof(string));
            AssertAttribute(DataTypes, "Long", typeof(long));
            AssertAttribute(DataTypes, "Name", typeof(string));
            AssertAttribute(DataTypes, "NCName", typeof(string));
            //AssertAttribute(DataTypes, "NegativeInteger", typeof(java.math.BigInteger));
            AssertAttribute(DataTypes, "NMToken", typeof(string));
            //AssertAttribute(DataTypes, "NMTOKENS", typeof(java.util.List));
            //AssertAttribute(DataTypes, "NonNegativeInteger", typeof(java.math.BigInteger));
            //AssertAttribute(DataTypes, "NonPositiveInteger", typeof(java.math.BigInteger));
            AssertAttribute(DataTypes, "NormalizedString", typeof(string));
            AssertAttribute(DataTypes, "NOTATION", typeof(object));
            //AssertAttribute(DataTypes, "PositiveInteger", typeof(java.math.BigInteger));
            AssertAttribute(DataTypes, "QName", typeof(object));
            AssertAttribute(DataTypes, "Short", typeof(short));
            AssertAttribute(DataTypes, "String", typeof(string));
            AssertAttribute(DataTypes, "Time", typeof(object));
            AssertAttribute(DataTypes, "Token", typeof(string));
            AssertAttribute(DataTypes, "UnsignedByte", typeof(short));
            AssertAttribute(DataTypes, "UnsignedInt", typeof(long));
            //AssertAttribute(DataTypes, "UnsignedLong", typeof(java.math.BigInteger));
            AssertAttribute(DataTypes, "UnsignedShort", typeof(int));
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
    }
}
