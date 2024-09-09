using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Models.Meta;
using NMF.Models.Repository;
using NMF.Models.Services;
using NMF.Serialization.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ModelServicesTests
{
    [TestClass]
    public class JsonSerializerTests
    {
        [TestMethod]
        public void JsonSerializer_CanDeserializeClass()
        {
            var serializedClass = "{\"$type\": \"http://nmf.codeplex.com/nmeta/$Class\", \"Name\": \"NewClass\" }";
            var serializer = new JsonModelSerializer(MetaRepository.Instance.Serializer);
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(serializedClass));
            var reader = new Utf8JsonStreamReader(stream, (int)stream.Length);
            var deserialized = serializer.Deserialize(ref reader, false);
            Assert.IsNotNull(deserialized);
            Assert.IsInstanceOfType(deserialized, typeof(Class));
            var cl = deserialized as Class;
            Assert.AreEqual("NewClass", cl.Name);
        }
    }
}
