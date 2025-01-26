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
        public WorkspaceEdit GetRenameWorkspaceEdit(JToken arg)
        {
            var renameParams = arg.ToObject<RenameParams>();
            string uri = renameParams.TextDocument.Uri;

            if (!_documents.TryGetValue(uri, out var document))
            {
                return null;
            }

            var textEdits = document.GetRenameTextEdits(AsParsePosition(renameParams.Position), renameParams.NewName);

            var changes = new Dictionary<string, LspTypes.TextEdit[]>();
            changes.Add(uri, textEdits.Select(textEdit => new LspTypes.TextEdit()
            {
                Range = new LspTypes.Range()
                {
                    Start = AsPosition(textEdit.Start),
                    End = AsPosition(textEdit.End)
                },
                NewText = String.Concat(textEdit.NewText)
            }).ToArray());

            return new WorkspaceEdit() { Changes = changes };
        }
    }
}
