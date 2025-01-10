using System.Collections.Generic;

namespace NMF.AnyText
{
    /// <summary>
    /// Represents changes to a workspace, including text edits, document changes, and change annotations.
    /// </summary>
    public class WorkspaceEdit
    {
        /// <summary>
        /// A dictionary of changes to text documents, keyed by document URI, with the value being the text edits for that document.
        /// </summary>
        public Dictionary<string, TextEdit[]> Changes { get; set; }

        /// <summary>
        /// A list of document-level changes (e.g., file creation, renaming, deletion).
        /// </summary>
        public List<DocumentChange> DocumentChanges { get; set; }

        /// <summary>
        /// A dictionary of annotations associated with changes, keyed by annotation ID.
        /// </summary>
        public Dictionary<string, ChangeAnnotation> ChangeAnnotations { get; set; }
    }

    /// <summary>
    /// Represents a change to a document, including text edits, file creation, renaming, or deletion.
    /// </summary>
    public class DocumentChange
    {
        /// <summary>
        /// Text document edits (e.g., line insertions, deletions).
        /// </summary>
        public TextDocumentEdit TextDocumentEdit { get; set; }

        /// <summary>
        /// Information for creating a new file.
        /// </summary>
        public CreateFile CreateFile { get; set; }

        /// <summary>
        /// Information for renaming an existing file.
        /// </summary>
        public RenameFile RenameFile { get; set; }

        /// <summary>
        /// Information for deleting an existing file.
        /// </summary>
        public DeleteFile DeleteFile { get; set; }
    }

    /// <summary>
    /// Represents metadata or instructions for an annotation associated with a change.
    /// </summary>
    public class ChangeAnnotation
    {
        /// <summary>
        /// A label for the annotation (e.g., "Refactor").
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Indicates if the change requires user confirmation.
        /// </summary>
        public bool? NeedsConfirmation { get; set; }

        /// <summary>
        /// A description or explanation of the annotation.
        /// </summary>
        public string Description { get; set; }
    }

    /// <summary>
    /// Represents the text document edit instructions, including the document and the edits.
    /// </summary>
    public class TextDocumentEdit
    {
        /// <summary>
        /// Identifies the text document to edit, including optional version information.
        /// </summary>
        public OptionalVersionedTextDocumentIdentifier TextDocument { get; set; }

        /// <summary>
        /// An Array of edits to perform on the document (e.g., insertions, deletions).
        /// </summary>
        public TextEdit[] Edits { get; set; }
    }

    /// <summary>
    /// Identifies a text document with optional versioning.
    /// </summary>
    public class OptionalVersionedTextDocumentIdentifier
    {
        /// <summary>
        /// The URI of the text document.
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// An optional version number for the document, if versioning is supported.
        /// </summary>
        public int? Version { get; set; }
    }

    /// <summary>
    /// Represents the information needed to create a new file.
    /// </summary>
    public class CreateFile
    {
        /// <summary>
        /// The type of file creation (e.g., "create").
        /// </summary>
        public string Kind { get; set; }

        /// <summary>
        /// The URI of the file to be created.
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// File options (e.g., whether to overwrite an existing file).
        /// </summary>
        public FileOptions Options { get; set; }

        /// <summary>
        /// An optional annotation ID related to the file creation.
        /// </summary>
        public string AnnotationId { get; set; }
    }

    /// <summary>
    /// Represents the information needed to rename an existing file.
    /// </summary>
    public class RenameFile
    {
        /// <summary>
        /// The type of file operation (e.g., "rename").
        /// </summary>
        public string Kind { get; set; }

        /// <summary>
        /// The URI of the old file name.
        /// </summary>
        public string OldUri { get; set; }

        /// <summary>
        /// The URI of the new file name.
        /// </summary>
        public string NewUri { get; set; }

        /// <summary>
        /// File options (e.g., whether to overwrite).
        /// </summary>
        public FileOptions Options { get; set; }

        /// <summary>
        /// An optional annotation ID related to the file rename.
        /// </summary>
        public string AnnotationId { get; set; }
    }

    /// <summary>
    /// Represents the information needed to delete an existing file.
    /// </summary>
    public class DeleteFile
    {
        /// <summary>
        /// The type of file operation (e.g., "delete").
        /// </summary>
        public string Kind { get; set; }

        /// <summary>
        /// The URI of the file to be deleted.
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// File deletion options (e.g., whether to delete recursively).
        /// </summary>
        public DeleteFileOptions Options { get; set; }

        /// <summary>
        /// An optional annotation ID related to the file deletion.
        /// </summary>
        public string AnnotationId { get; set; }
    }

    /// <summary>
    /// Options for creating or renaming files, such as overwrite behavior.
    /// </summary>
    public class FileOptions
    {
        /// <summary>
        /// If true, overwrite an existing file.
        /// </summary>
        public bool? Overwrite { get; set; }

        /// <summary>
        /// If true, ignore the operation if the file already exists.
        /// </summary>
        public bool? IgnoreIfExists { get; set; }
    }

    /// <summary>
    /// Options for deleting files, such as recursive deletion and handling missing files.
    /// </summary>
    public class DeleteFileOptions
    {
        /// <summary>
        /// If true, delete directories recursively.
        /// </summary>
        public bool? Recursive { get; set; }

        /// <summary>
        /// If true, ignore the operation if the file does not exist.
        /// </summary>
        public bool? IgnoreIfNotExists { get; set; }
    }
}
