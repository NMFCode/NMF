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

            return references?.Select(reference => new Location()
            {
                Uri = uri,
                Range = AsRange(reference)
            }).ToArray();
        }

        /// <summary>
        /// Sends a <c>custom/showReferences</c> notification to the client without expecting a response
        /// </summary>
        /// <param name="position">The position of the symbol in the document to show references for</param>
        protected internal Task ShowReferencesNotifyAsync(ParsePosition position)
        {
            return _rpc.NotifyWithParameterObjectAsync("custom/showReferences", AsPosition(position));
        }
    }
}
