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
        public FoldingRange[] QueryFoldingRanges(JToken arg)
        {
            var foldingRangeParams = arg.ToObject<FoldingRangeParams>();
            string uri = foldingRangeParams.TextDocument.Uri;

            if (!_documents.TryGetValue(uri, out var document))
            {
                return Array.Empty<FoldingRange>();
            }
            
            var parsedFoldingRanges = document.GetFoldingRangesFromRoot().ToList();

            var foldingRanges = parsedFoldingRanges.Select(parsedFoldingRange => new FoldingRange()
            {
                StartLine = parsedFoldingRange.StartLine,
                StartCharacter = parsedFoldingRange.StartCharacter,
                EndLine = parsedFoldingRange.EndLine,
                EndCharacter = parsedFoldingRange.EndCharacter,
                Kind = parsedFoldingRange.Kind
            });

            return foldingRanges.Distinct(new FoldingRangeEqualityComparer()).ToArray();
        }

        private class FoldingRangeEqualityComparer : IEqualityComparer<FoldingRange>
        {
            public bool Equals(FoldingRange x, FoldingRange y)
            {
                if (ReferenceEquals(x, y)) return true;
                else if (x == null || y == null) return false;

                return x.StartLine == y.StartLine
                    && x.StartCharacter == y.StartCharacter
                    && x.EndLine == y.EndLine
                    && x.EndCharacter == y.EndCharacter
                    && x.Kind.Equals(y.Kind);
            }

            public int GetHashCode([DisallowNull] FoldingRange obj)
            {
                return 0; // always use equals over hash code
            }
        }
    }
}
