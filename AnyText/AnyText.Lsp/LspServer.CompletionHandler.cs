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
            var completionParams = arg.ToObject<CompletionParams>();
            var uri = completionParams.TextDocument.Uri;

            if (!_documents.TryGetValue(uri, out var document))
            {
                return new CompletionList {};
            }

            var position = new ParsePosition
            {
                Col = Convert.ToInt32(completionParams.Position.Character),
                Line = Convert.ToInt32(completionParams.Position.Line)
            };
            var completionItems = document.GetCompletionList(position);
            /*
                        {
                            new CompletionItem { Label = "Console", Kind = CompletionItemKind.Class, Detail = "System.Console" },
                            new CompletionItem { Label = "WriteLine", Kind = CompletionItemKind.Method, Detail = "Writes to the console." },
                            new CompletionItem { Label = "ReadLine", Kind = CompletionItemKind.Method, Detail = "Reads from the console." }
                        };
            */
            return new CompletionList
            {
                Items = completionItems.Select(suggestion => new CompletionItem { Label = suggestion}).ToArray()
            };
        }
    }
}
