namespace NMF.AnyText.Workspace
{
    /// <summary>
    ///     Represents metadata or instructions for an annotation associated with a change.
    /// </summary>
    public class ChangeAnnotation
    {
        /// <summary>
        ///     A label for the annotation (e.g., "Refactor").
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        ///     Indicates if the change requires user confirmation.
        /// </summary>
        public bool? NeedsConfirmation { get; set; }

        /// <summary>
        ///     A description or explanation of the annotation.
        /// </summary>
        public string Description { get; set; }
    }
}