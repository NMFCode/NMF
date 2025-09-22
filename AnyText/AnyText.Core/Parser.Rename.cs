using System.Collections.Generic;
using System.Linq;

namespace NMF.AnyText
{
    public partial class Parser
    {
        /// <summary>
        /// Gets the text edits for a rename operation of a symbol
        /// </summary>
        /// <param name="position">The position of the symbol in the document</param>
        /// <param name="newName">The new name of the symbol</param>
        /// <returns>An IEnumerable of <see cref="TextEdit"/> objects, each containing details on a text edit to be performed.</returns>
        public IEnumerable<TextEdit> GetRenameTextEdits(ParsePosition position, string newName)
        {
            var references = GetReferences(position);
            if (references == null)
            {
                return Enumerable.Empty<TextEdit>();
            }
            return references.Select(reference => new TextEdit(reference.Start, reference.End, new string[] { newName }));
        }
    }
}
