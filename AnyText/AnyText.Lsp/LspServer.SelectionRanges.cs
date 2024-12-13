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
        public LspTypes.SelectionRange[] QuerySelectionRanges(JToken arg)
        {
            Debugger.Break();
            var selectionRangeParams = arg.ToObject<SelectionRangeParams>();
            string uri = selectionRangeParams.TextDocument.Uri;

            if (!_documents.TryGetValue(uri, out var document))
            {
                return Array.Empty<LspTypes.SelectionRange>();
            }

            var parsedSelectionRanges = document.GetSelectionRangesFromRoot();

            return parsedSelectionRanges.Select(selectionRange => MapToSelectionRange(selectionRange)).ToArray();
        }

        private LspTypes.SelectionRange MapToSelectionRange(ParsedSelectionRange parsedSelectionRange)
        {
            return new LspTypes.SelectionRange()
            {
                Range = new LspTypes.Range
                {
                    Start = new Position
                    {
                        Line = parsedSelectionRange.Range.Start.Line,
                        Character = parsedSelectionRange.Range.Start.Character
                    },
                    End = new Position
                    {
                        Line = parsedSelectionRange.Range.End.Line,
                        Character = parsedSelectionRange.Range.End.Character
                    }
                },
                Parent = MapToSelectionRange(parsedSelectionRange.Parent)
            };
        }
    }
}
