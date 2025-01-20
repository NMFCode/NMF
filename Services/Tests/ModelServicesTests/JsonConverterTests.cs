using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Serialization;
using NMF.Models.Meta;
using NMF.Models.Repository;
using NMF.Models.Services;
using NMF.Models.Services.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ModelServicesTests
{
    [TestClass]
    public class JsonConverterTests
    {
        [TestMethod]
        public void JsonShallowConverter_Works()
        {
            var serializedClass = "{\"data\":{\"$type\": \"http://nmf.codeplex.com/nmeta/$Class\", \"Name\": \"NewClass\" }, \"identifier\": \"test\"}";
            var serializer = new JsonModelSerializer(MetaRepository.Instance.Serializer);
            var server = new ModelServer();
            var converter = new ShallowModelElementConverter(serializer, server);
            var options = new JsonSerializerOptions();
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.Converters.Add(converter);
            var modelElement = JsonSerializer.Deserialize<ModelElementInfo>(serializedClass, options);
            Assert.IsNotNull(modelElement);
            Assert.IsInstanceOfType(modelElement.Data, typeof(Class));
            var cl = modelElement.Data as Class;
            Assert.AreEqual("NewClass", cl.Name);
        }

        [TestMethod]
        public void JsonSchemaConverter_Works()
        {
            var serializedClass = "{\"data\":{\"$type\": \"http://nmf.codeplex.com/nmeta/$Class\", \"Name\": \"NewClass\" }, \"schema\": {}, \"identifier\": \"test\"}";
            var serializer = new JsonModelSerializer(MetaRepository.Instance.Serializer);
            var server = new ModelServer();
            var converter = new ShallowModelElementConverter(serializer, server);
            var options = new JsonSerializerOptions();
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.Converters.Add(converter);
            options.Converters.Add(new SchemaConverter());
            var modelElement = JsonSerializer.Deserialize<ModelElementInfo>(serializedClass, options);
            Assert.IsNotNull(modelElement);
            Assert.IsInstanceOfType(modelElement.Data, typeof(Class));
            var cl = modelElement.Data as Class;
            Assert.AreEqual("NewClass", cl.Name);
        }
    }
}
