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
        /// <returns>An <see cref="IEnumerable{T}"/> representing the mapped LSP text edits.</returns>
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
    }
}