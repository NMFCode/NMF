namespace NMF.Glsp.Protocol.ModelData
{
    /// <summary>
    /// Container for edit modes
    /// </summary>
    public static class ModelEditModes
    {
        /// <summary>
        /// Denotes that the user cannot make any changes to the semantic model
        /// </summary>
        public const string Readonly = "readonly";

        /// <summary>
        /// Denotes that the user performs changes
        /// </summary>
        public const string Editable = "editable";
    }
}
