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
        public LocationLink[] QueryDefinition(JToken arg)
        {
            var definitionParams = arg.ToObject<TextDocumentPositionParams>();
            var uri = definitionParams.TextDocument.Uri;

            if (!_documents.TryGetValue(uri, out var document))
            {
                return null;
            }

            var definitions = document.GetDefinitions(AsParsePosition(definitionParams.Position));

            return definitions.Select(definition =>
            {
                var identifier = definition.GetIdentifier();

                return new LocationLink()
                {
                    TargetUri = uri,
                    TargetRange = new LspTypes.Range()
                    {
                        Start = AsPosition(definition.CurrentPosition),
                        End = AsPosition(definition.CurrentPosition + definition.Length)
                    },
                    TargetSelectionRange = identifier == null ? null : new LspTypes.Range()
                    {
                        Start = AsPosition(identifier.CurrentPosition),
                        End = AsPosition(identifier.CurrentPosition + identifier.Length)
                    }
                };
            }).ToArray();
        }
    }
}
