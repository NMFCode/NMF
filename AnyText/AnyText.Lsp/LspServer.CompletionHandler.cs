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
        public CompletionList HandleCompletion(JToken arg)
        {
            try
            {
                var completionParams = arg.ToObject<CompletionParams>();
                var uri = completionParams.TextDocument.Uri;

                if (!_documents.TryGetValue(uri, out var document))
                {
                    return new CompletionList { };
                }

                var position = new ParsePosition
                {
                    Col = Convert.ToInt32(completionParams.Position.Character),
                    Line = Convert.ToInt32(completionParams.Position.Line)
                };
                var completionItems = document.GetCompletionList(position);

                return new CompletionList
                {
                    Items = completionItems.Select(suggestion => new CompletionItem { Label = suggestion, Kind = CompletionItemKind.Text, }).ToArray()
                };
            }
            catch (Exception ex)
            {
                SendLogMessage(MessageType.Error, ex.ToString());
                return new CompletionList { };
            }
        }
    }
}
