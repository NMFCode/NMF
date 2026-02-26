using LspTypes;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    public partial class LspServer
    {
        /// <inheritdoc />
        public LspTypes.SelectionRange[] QuerySelectionRanges(JToken arg)
        {
            var selectionRangeParams = arg.ToObject<SelectionRangeParams>();
            string uri = selectionRangeParams.TextDocument.Uri;

            if (!_documents.TryGetValue(uri, out var document))
            {
                return Array.Empty<LspTypes.SelectionRange>();
            }

            return GetSelectionRanges(selectionRangeParams, document);
        }

        private LspTypes.SelectionRange[] GetSelectionRanges(SelectionRangeParams selectionRangeParams, Parser document)
        {
            _readWriteLock.EnterReadLock();
            try
            {
                var selectionRanges = document.GetSelectionRanges(selectionRangeParams.Positions.Select(AsParsePosition));
                return selectionRanges.Select(MapToSelectionRange).ToArray();
            }
            finally
            {
                _readWriteLock.ExitReadLock();
            }
        }

        private LspTypes.SelectionRange MapToSelectionRange(SelectionRange selectionRange)
        {
            if (selectionRange == null) return null;

            return new LspTypes.SelectionRange()
            {
                Range = new LspTypes.Range
                {
                    Start = AsPosition(selectionRange.Range.Start),
                    End = AsPosition(selectionRange.Range.End)
                },
                Parent = MapToSelectionRange(selectionRange.Parent)
            };
        }
    }
}
