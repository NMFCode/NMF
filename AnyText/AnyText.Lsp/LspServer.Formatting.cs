using System.Linq;
using LspTypes;
using Range = LspTypes.Range;

namespace NMF.AnyText
{
    public partial class LspServer
    {
        /// <inheritdoc cref="ILspServer.Formatting" />
        public LspTypes.TextEdit[] Formatting(TextDocumentIdentifier textDocument, FormattingOptions options)
        {
            if (!_documents.TryGetValue(textDocument.Uri, out var document))
                return null;
            var edits = document.Format(
                tabsize: options.TabSize,
                insertSpaces: options.InsertSpaces,
                insertFinalNewline: options.InsertFinalNewline ?? false,
                otherOptions: options.OtherOptions,
                trimFinalNewlines: options.TrimFinalNewlines ?? false,
                trimTrailingWhitespace: options.TrimTrailingWhitespace ?? false
            );
            
            var lspEdits = LspTypesMapper.MapToLspTextEdits(edits);
            return lspEdits.ToArray();
        }

        /// <inheritdoc cref="ILspServer.FormattingRange" />
        public LspTypes.TextEdit[] FormattingRange(TextDocumentIdentifier textDocument, Range range,
            FormattingOptions options)
        {
            if (!_documents.TryGetValue(textDocument.Uri, out var document))
                return null;
            var edits = document.Format(AsParsePosition(range.Start), AsParsePosition(range.End), options.TabSize,
                options.InsertSpaces, options.OtherOptions, options.TrimTrailingWhitespace ?? false,
                options.InsertFinalNewline ?? false, options.TrimFinalNewlines ?? false);
            
            var lspEdits = LspTypesMapper.MapToLspTextEdits(edits);
            return lspEdits.ToArray();
        }
    }
}