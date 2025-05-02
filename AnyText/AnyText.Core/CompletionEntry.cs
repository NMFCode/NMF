using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace NMF.AnyText
{
    /// <summary>
    /// Represents an entry for code completion.
    /// </summary>
    public struct CompletionEntry
    {

        /// <summary>
        /// Creates a new completion entry
        /// </summary>
        /// <param name="completion">the text of the completion</param>
        /// <param name="kind">the completion kind</param>
        /// <param name="startPosition">the start position</param>
        /// <param name="position">the position where the completion was requested</param>
        public CompletionEntry(string completion, SymbolKind kind, ParsePosition startPosition, ParsePosition position) : this()
        {
            Completion = completion;
            Label = completion;
            Kind = kind;
            StartPosition = startPosition;
            Length = position - startPosition;
        }

        /// <summary>
        /// The text of the completion
        /// </summary>
        public string Completion { get; init; }

        /// <summary>
        /// The label of the completion shown to the user
        /// </summary>
        public string Label { get; init; }

        /// <summary>
        /// A human-readable string with additional information about this item, like type or symbol information.
        /// </summary>
        public string Detail { get; init; }

        /// <summary>
        /// A string that should be used when comparing this item
        /// with other items.When omitted the label is used
        /// as the sort text for this item.
        /// </summary>
        public string SortText { get; init; }

        /// <summary>
        /// A string that should be used when filtering a set of
        /// completion items.When omitted the label is used as the
        /// filter text for this item.
        /// </summary>
        public string FilterText { get; init; }

        /// <summary>
        /// A human-readable string that represents a doc-comment.
        /// </summary>
        public string Documentation { get; init; }

        /// <summary>
        /// The symbol kind associated with the completion entry
        /// </summary>
        public SymbolKind Kind { get; init; }

        /// <summary>
        /// The start position of the completion
        /// </summary>
        public ParsePosition StartPosition { get; init; }

        /// <summary>
        /// The length of the completion
        /// </summary>
        public ParsePositionDelta Length { get; init; }
    }

}
