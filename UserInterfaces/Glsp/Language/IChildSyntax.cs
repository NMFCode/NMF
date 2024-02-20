using NMF.Glsp.Protocol.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Language
{
    /// <summary>
    /// Denotes the syntax used to customize how elements appear in menus
    /// </summary>
    public interface IChildSyntax<TSyntax>
    {
        /// <summary>
        /// Defines that the option should not be shown in the contexts defined by the given predicate
        /// </summary>
        /// <param name="contextIdPredicate">A predicate to check the context ids</param>
        /// <returns>A child syntax instance for chaining purposes</returns>
        TSyntax HideIn(Func<string, bool> contextIdPredicate);
    }

    /// <summary>
    /// Denotes a simple child syntax
    /// </summary>
    public interface IChildSyntax : IChildSyntax<IChildSyntax> { }

    /// <summary>
    /// Provides convenience methods for <see cref="IChildSyntax{TSyntax}"/>
    /// </summary>
    public static class ChildSyntaxExtensions
    {

        /// <summary>
        /// Defines that the option should not be visible in the palette
        /// </summary>
        /// <param name="syntax">The syntax</param>
        /// <returns>A child syntax instance for chaining purposes</returns>
        public static TSyntax HideInPalette<TSyntax>(this IChildSyntax<TSyntax> syntax)
        {
            if (syntax == null) throw new ArgumentNullException(nameof(syntax));
            return syntax.HideIn(context => context == Contexts.ToolPalette);
        }

        /// <summary>
        /// Defines that the option should not be visible in the given context id
        /// </summary>
        /// <param name="syntax">The syntax</param>
        /// <param name="contextId">The id of the context for which the option should be hidden</param>
        /// <returns>A child syntax instance for chaining purposes</returns>
        public static TSyntax HideIn<TSyntax>(this IChildSyntax<TSyntax> syntax, string contextId)
        {
            if (syntax == null) throw new ArgumentNullException(nameof(syntax));
            return syntax.HideIn(context => context == contextId);
        }
    }
}
