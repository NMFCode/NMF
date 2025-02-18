using System.Collections.Generic;

namespace NMF.AnyText.Workspace
{
    /// <summary>
    ///     Represents changes to a workspace, including text edits, document changes, and change annotations.
    /// </summary>
    public class WorkspaceEdit
    {
        /// <summary>
        ///     A dictionary of changes to text documents, keyed by document URI, with the value being the text edits for that
        ///     document.
        /// </summary>
        public Dictionary<string, TextEdit[]> Changes { get; set; }

        /// <summary>
        ///     A list of document-level changes (e.g., file creation, renaming, deletion).
        /// </summary>
        public List<DocumentChange> DocumentChanges { get; set; }

        /// <summary>
        ///     A dictionary of annotations associated with changes, keyed by annotation ID.
        /// </summary>
        public Dictionary<string, ChangeAnnotation> ChangeAnnotations { get; set; }
    }
}