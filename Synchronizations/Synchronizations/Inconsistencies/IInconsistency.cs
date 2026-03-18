using System;

namespace NMF.Synchronizations.Inconsistencies
{
    /// <summary>
    /// Describes inconsistencies that can occur between the right hand and the left hand of a synchronization
    /// </summary>
    public interface IInconsistency : IEquatable<IInconsistency>
    {
        /// <summary>
        /// The object that this inconsistency is anchored at on the left side
        /// </summary>
        object LeftElement { get; }

        /// <summary>
        /// The object that this inconsistency is anchored at on the right side
        /// </summary>
        object RightElement { get; }

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

        /// <summary>
        /// Gets a string describing the inconsistency from the point of view of the left side
        /// </summary>
        /// <returns>a human-readable description of the inconsistency</returns>
        string DescribeLeft();

        /// <summary>
        /// Gets a string describing the inconsistency from the point of view of the right side
        /// </summary>
        /// <returns>a human-readable description of the inconsistency</returns>
        string DescribeRight();
    }
}
