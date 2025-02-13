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
        public Location[] QueryReferences(JToken arg)
        {
            var referenceParams = arg.ToObject<ReferenceParams>();
            var uri = referenceParams.TextDocument.Uri;

            if (!_documents.TryGetValue(uri, out var document))
            {
                return null;
            }

            var references = document.GetReferences(AsParsePosition(referenceParams.Position));

            if (references == null)
            {
                return null;
            }

            return references.Select(reference => new Location()
            {
                Uri = uri,
                Range = AsRange(reference)
            }).ToArray();
        }
    }
}
