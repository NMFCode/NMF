using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Protocol.Context;
using NMF.Glsp.Protocol.Types;
using NMF.Glsp.Server;
using System.Text.Json;

namespace GlspTests
{
    public class Tests
    {

        [Test]
        public void RequestContextActions_DeserializedCorrectly()
        {
            var action = "{\"kind\":\"requestContextActions\",\"requestId\":\"_1\",\"contextId\":\"tool-palette\",\"editorContext\":{\"selectedElementIds\":[]}}";
            var resolved = JsonSerializer.Deserialize<BaseAction>(action, CreateOptions());
            Assert.That(resolved, Is.Not.Null);
        }

        [Test]
        public void RequestContextActions_SerializationWorks()
        {
            var action = new RequestContextActions
            {
                ContextId = "context",
                RequestId = "request",
                EditorContext = new EditorContext()
                {
                    LastMousePosition = new Point(0, 0),
                }
            };

            var json = JsonSerializer.Serialize(action, CreateOptions());
            Assert.That(json, Is.Not.Null);
        }

        private static JsonSerializerOptions CreateOptions()
        {
            return new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IncludeFields = false,
                Converters =
                {
                    new BaseActionConverter()
                }
            };
        }
    }
}