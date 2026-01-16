using LspTypes;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    public partial class LspServer
    {
        /// <inheritdoc cref="ILspServer.QueryFoldingRanges(JToken)"/>
        public LspTypes.FoldingRange[] QueryFoldingRanges(JToken arg)
        {
            var foldingRangeParams = arg.ToObject<FoldingRangeParams>();
            string uri = foldingRangeParams.TextDocument.Uri;

            if (!_documents.TryGetValue(uri, out var document))
            {
                return Array.Empty<LspTypes.FoldingRange>();
            }

            var foldingRanges = document.GetFoldingRangesFromRoot();

            return foldingRanges?.Select(foldingRange => new LspTypes.FoldingRange()
            {
                StartLine = foldingRange.StartLine,
                StartCharacter = foldingRange.StartCharacter,
                EndLine = foldingRange.EndLine,
                EndCharacter = foldingRange.EndCharacter,
                Kind = foldingRange.Kind
            }).ToArray();
        }
    }
}
