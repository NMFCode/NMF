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
        public FoldingRange[] QueryFoldingRanges(JToken arg)
        {
            var foldingRangeParams = arg.ToObject<FoldingRangeParams>();
            string uri = foldingRangeParams.TextDocument.Uri;

            if (!_documents.TryGetValue(uri, out var document))
            {
                return Array.Empty<FoldingRange>();
            }

            var parsedFoldingRanges = document.GetFoldingRangesFromRoot();

            return parsedFoldingRanges.Select(parsedFoldingRange => new FoldingRange()
            {
                StartLine = parsedFoldingRange.StartLine,
                StartCharacter = parsedFoldingRange.StartCharacter,
                EndLine = parsedFoldingRange.EndLine,
                EndCharacter = parsedFoldingRange.EndCharacter,
                Kind = parsedFoldingRange.Kind
            }).ToArray();
        }
    }
}
