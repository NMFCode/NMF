namespace NMF.Models.Meta
{
    /// <summary>
    /// Denotes extension method for identifier scopes
    /// </summary>
    public static class IdentifierScopeExtensions
    {
        /// <summary>
        /// Gets the actual identifier scope
        /// </summary>
        /// <param name="current">The identifier scope declared</param>
        /// <param name="inherited">The inherited identifier scope</param>
        /// <returns>The actual identifier scope</returns>
        public static IdentifierScope GetActual(this IdentifierScope current, IdentifierScope inherited)
        {
            if (current == IdentifierScope.Inherit)
            {
                return inherited;
            }
            else
            {
                return current;
            }
        }
    }
}
