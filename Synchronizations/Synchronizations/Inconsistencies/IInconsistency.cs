using System;

namespace NMF.Synchronizations.Inconsistencies
{
    /// <summary>
    /// Describes inconsistencies that can occur between the right hand and the left hand of a synchronization
    /// </summary>
    public interface IInconsistency : IEquatable<IInconsistency>
    {
        /// <summary>
        /// Gets a value indicating whether the inconsistency can be automatically resolved modifying the left hand model only
        /// </summary>
        bool CanResolveLeft { get; }

        /// <summary>
        /// Gets a value indicating whether the inconsistency can be automatically resolved modifying the right hand model only
        /// </summary>
        bool CanResolveRight { get; }

        /// <summary>
        /// Resolves the inconsistency modifying the left hand side model
        /// </summary>
        void ResolveLeft();

        /// <summary>
        /// Resolves the inconsistency modifying the right hand side model
        /// </summary>
        void ResolveRight();
    }
}
