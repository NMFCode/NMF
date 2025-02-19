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
        public LspTypes.DocumentHighlight[] QueryDocumentHighlights(JToken arg)
        {
            var documentHighlightParams = arg.ToObject<DocumentHighlightParams>();
            var uri = documentHighlightParams.TextDocument.Uri;

            if (!_documents.TryGetValue(uri, out var document))
            {
                return null;
            }

            var highlights = document.GetDocumentHighlights(AsParsePosition(documentHighlightParams.Position));

            return highlights?.Select(highlight => new LspTypes.DocumentHighlight()
            {
                Range = AsRange(highlight.Range),
                Kind = (LspTypes.DocumentHighlightKind)highlight.Kind
            }).ToArray();
        }
    }
}
