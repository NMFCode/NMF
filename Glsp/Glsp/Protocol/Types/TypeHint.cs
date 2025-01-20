namespace NMF.Glsp.Protocol.Types
{
    /// <summary>
    /// Type hints are used to define what modifications are supported on the different element types. Conceptually 
    /// type hints are similar to features of a model elements but define the functionality on a type level. The 
    /// rationale is to avoid a client-server round-trip for user feedback of each synchronous user interaction.
    /// </summary>
    public class TypeHint
    {
        /// <summary>
        ///  The identifier of an element.
        /// </summary>
        public string ElementTypeId { get; init; }

        /// <summary>
        ///  Specifies whether the element can be relocated.
        /// </summary>
        public bool Repositionable { get; init; }

        /// <summary>
        ///  Specifies whether the element can be deleted
        /// </summary>
        public bool Deletable { get; init; }
    }
}
