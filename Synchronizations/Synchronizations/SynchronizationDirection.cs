namespace NMF.Synchronizations
{
    /// <summary>
    /// Denotes the direction of a synchronization
    /// </summary>
    public enum SynchronizationDirection
    {
        /// <summary>
        /// Elements existing in LHS are translated to RHS
        /// </summary>
        LeftToRight,
        /// <summary>
        /// Elements existing in RHS are translated to LHS
        /// </summary>
        RightToLeft,
        /// <summary>
        /// Elements existing in LHS are translated to RHS, RHS elements with no existing corresponding LHS element are deleted
        /// </summary>
        LeftToRightForced,
        /// <summary>
        /// Elements existing in RHS are translated to LHS, LHS elements with no existing corresponding HS element are deleted
        /// </summary>
        RightToLeftForced,
        /// <summary>
        /// Elements existing in LHS are translated to RHS and vice versa, in case of conflicts the LHS wins
        /// </summary>
        LeftWins,
        /// <summary>
        /// Elements existing in LHS are translated to RHS and vice versa, in case of conflicts the RHS wins
        /// </summary>
        RightWins,
        /// <summary>
        /// No changes are made, but only inconsistencies are found and reported
        /// </summary>
        CheckOnly
    }

    /// <summary>
    /// Denotes extension methods for synchronization directions
    /// </summary>
    public static class SynchronizationDirectionExtensions
    {
        /// <summary>
        /// Determines whether the basic direction is LHS to RHS
        /// </summary>
        /// <param name="direction">The direction</param>
        /// <returns>True, if the direction is mostly LHS to RHS, otherwise false</returns>
        public static bool IsLeftToRight(this SynchronizationDirection direction)
        {
            return direction == SynchronizationDirection.LeftToRight || direction == SynchronizationDirection.LeftToRightForced || direction == SynchronizationDirection.LeftWins;
        }

        /// <summary>
        /// Determines whether the basic direction is RHS to LHS
        /// </summary>
        /// <param name="direction">The direction</param>
        /// <returns>True, if the direction is mostly RHS to LHS, otherwise false</returns>
        public static bool IsRightToLeft(this SynchronizationDirection direction)
        {
            return direction == SynchronizationDirection.RightToLeft || direction == SynchronizationDirection.RightToLeftForced || direction == SynchronizationDirection.RightWins;
        }
    }
}
