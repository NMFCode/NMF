using System;
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
                return Array.Empty<LspTypes.TextEdit>();
            var indentation = options.InsertSpaces ? new string(' ', (int)options.TabSize) : "\t";

            var edits = document.Format(
                indentationString: indentation,
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
                return Array.Empty<LspTypes.TextEdit>();

            var indentation = options.InsertSpaces ? new string(' ', (int)options.TabSize) : "\t";
            
            var edits = document.Format(AsParsePosition(range.Start), AsParsePosition(range.End), indentation,
                options.OtherOptions, options.TrimTrailingWhitespace ?? false,
                options.InsertFinalNewline ?? false, options.TrimFinalNewlines ?? false);

            var lspEdits = LspTypesMapper.MapToLspTextEdits(edits);
            return lspEdits.ToArray();
        }
    }
}