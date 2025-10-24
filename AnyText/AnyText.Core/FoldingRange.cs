namespace NMF.AnyText
{
    /// <summary>
    /// Denotes a part in a parsed document that can be folded away (hidden).
    /// Analogous to the LspTypes FoldingRange interface.
    /// </summary>
    public class FoldingRange
    {
        /// <summary>
        /// The zero-based start line of the range to fold. The folded area starts
        /// after the line's last character. To be valid, the end must be zero or
        /// larger and smaller than the number of lines in the document.
        /// </summary>
        public uint StartLine { get; set; }

        /// <summary>
        /// The zero-based character offset from where the folded range starts.
	    /// If not defined, defaults to the length of the start line. 
        /// </summary>
        public uint StartCharacter { get; set; }

        /// <summary>
        /// The zero-based end line of the range to fold. The folded area ends with
	    /// the line's last character. To be valid, the end must be zero or larger
        /// and smaller than the number of lines in the document.
        /// </summary>
        public uint EndLine { get; set; }

        /// <summary>
        /// The zero-based character offset before the folded range ends.
        /// If not defined, defaults to the length of the end line.
        /// </summary>
        public uint EndCharacter { get; set; }

        /// <summary>
        /// Describes the kind of the folding range.
        /// Supports values "comment", "imports" and "region".
        /// </summary>
        public string Kind { get; set; }
    }
}
