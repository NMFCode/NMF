using LspTypes;
using System.Collections.Generic;
using System.Linq;

namespace NMF.AnyText
{
    /// <summary>
    /// Class To Map Types used In Parser To LSP-Types
    /// </summary>
    public static class LspTypesMapper
    {
        /// <summary>
        /// Maps a collection of <see cref="TextEdit"/> objects to a collection of <see cref="LspTypes.TextEdit"/> objects.
        /// </summary>
        /// <param name="edits">The collection of <see cref="TextEdit"/> objects to be mapped.</param>
        /// <returns>An <see cref="IEnumerable{LspTypes.TextEdit}"/> representing the mapped LSP text edits.</returns>
        public static IEnumerable<LspTypes.TextEdit> MapToLspTextEdits(IEnumerable<TextEdit> edits)
        {
            return edits.Select(e => new LspTypes.TextEdit
            {
                Range = new Range
                {
                    Start = new Position
                    {
                        Line = (uint)e.Start.Line,
                        Character = (uint)e.Start.Col
                    },
                    End = new Position
                    {
                        Line = (uint)e.End.Line,
                        Character = (uint)e.End.Col
                    }
                },
                NewText = string.Join("\n", e.NewText)
            });
        }

        public static readonly Dictionary<SymbolKind, CompletionItemKind> KindMapper = new Dictionary<SymbolKind, CompletionItemKind>()
        {
            { SymbolKind.File, CompletionItemKind.File },
            { SymbolKind.Module, CompletionItemKind.Module},
            { SymbolKind.Namespace, CompletionItemKind.Text },
            { SymbolKind.Package, CompletionItemKind.Folder },
            { SymbolKind.Class, CompletionItemKind.Class },
            { SymbolKind.Method, CompletionItemKind.Method },
            { SymbolKind.Property, CompletionItemKind.Property },
            { SymbolKind.Field, CompletionItemKind.Field },
            { SymbolKind.Constructor,CompletionItemKind.Constructor },
            { SymbolKind.Enum, CompletionItemKind.Enum },
            { SymbolKind.Interface, CompletionItemKind.Interface },
            { SymbolKind.Function, CompletionItemKind.Function },
            { SymbolKind.Variable, CompletionItemKind.Variable },
            { SymbolKind.Constant, CompletionItemKind.Constant },
            { SymbolKind.String, CompletionItemKind.Text },
            { SymbolKind.Number, CompletionItemKind.Value },
            { SymbolKind.Boolean, CompletionItemKind.Value },
            { SymbolKind.Array, CompletionItemKind.Unit },
            { SymbolKind.Object, CompletionItemKind.Struct },
            { SymbolKind.Key, CompletionItemKind.Keyword },
            { SymbolKind.Null, CompletionItemKind.Text },
            { SymbolKind.EnumMember, CompletionItemKind.EnumMember },
            { SymbolKind.Struct, CompletionItemKind.Struct },
            { SymbolKind.Event, CompletionItemKind.Event },
            { SymbolKind.Operator, CompletionItemKind.Operator },
            { SymbolKind.TypeParameter, CompletionItemKind.TypeParameter },
        };
    }
}