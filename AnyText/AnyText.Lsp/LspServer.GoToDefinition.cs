using LspTypes;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    public partial class LspServer
    {
        /// <inheritdoc />
        public Location QueryDefinition(JToken arg)
        {
            var definitionParams = arg.ToObject<TextDocumentPositionParams>();
            var uri = definitionParams.TextDocument.Uri;

            if (!_documents.TryGetValue(uri, out var document))
            {
                return null;
            }

            var definition = document.GetDefinition(AsParsePosition(definitionParams.Position));

            return new Location()
            {
                Uri = uri,
                Range = AsRange(definition)
            };
        }
    }
}
