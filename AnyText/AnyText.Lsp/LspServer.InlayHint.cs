using Newtonsoft.Json.Linq;
using NMF.AnyText.InlayClasses;
using NMF.Utilities;
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
        public InlayHint[] ProvideInlayHints(JToken arg)
        {
            var inlayHintParams = arg.ToObject<InlayHintParams>();
            string uri = inlayHintParams.TextDocument.Uri;


            if (!_documents.TryGetValue(uri, out var document))
            {
                return Array.Empty<InlayHint>();
            }

            var range = inlayHintParams.Range;

            IEnumerable<InlayEntry> inlayEntries = document.GetInlayEntriesInRange(range);

            if (inlayEntries.IsNullOrEmpty())
            {
                return Array.Empty<InlayHint>();
            }

            var inlayHints = inlayEntries.Select(suggestion => new InlayHint { Label = suggestion.Label, Position = AsPosition(suggestion.Position) });

            return inlayHints.ToArray();
        }
    }
}
