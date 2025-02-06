using NMF.AnyText.Grammars;
using NMF.AnyText.Rules;
using NMF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    /// <summary>
    /// Denotes a parse context using model
    /// </summary>
    public class ModelParseContext : ParseContext
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="grammar">the grammar for this context</param>
        /// <param name="stringComparison">the string comparison mode</param>
        public ModelParseContext(Grammar grammar, StringComparison stringComparison = StringComparison.OrdinalIgnoreCase) : this(grammar, new Matcher(grammar.CommentRules), stringComparison)
        {
        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="grammar">the grammar for this context</param>
        /// <param name="matcher">the matcher for the context</param>
        /// <param name="stringComparison">the string comparison mode</param>
        public ModelParseContext(Grammar grammar, Matcher matcher, StringComparison stringComparison = StringComparison.OrdinalIgnoreCase) : base(grammar, matcher, stringComparison)
        {
        }

        /// <inheritdoc />
        public override bool TryResolveReference<T>(object contextElement, string input, out T resolved)
        {
            if (contextElement is IModelElement modelElement)
            {
                while (modelElement != null)
                {
                    var childWithIdentifier = modelElement.Children.Where(c => c.ToIdentifierString() == input).OfType<T>().FirstOrDefault();
#pragma warning disable S2955 // Generic parameters not constrained to reference types should not be compared to "null"
                    if (childWithIdentifier != null)
                    {
                        resolved = childWithIdentifier;
                        return true;
                    }
#pragma warning restore S2955 // Generic parameters not constrained to reference types should not be compared to "null"
                    modelElement = modelElement.Parent;
                }
            }
            return base.TryResolveReference(contextElement, input, out resolved);
        }

        /// <summary>
        /// Gets a collection of potential objects that could be resolved to the given reference.
        /// </summary>
        /// <typeparam name="T">The type of the reference to resolve.</typeparam>
        /// <param name="contextElement">The element providing the context for resolution.</param>
        /// <returns>A collection of potential objects that match the type and context.</returns>
        public override IEnumerable<T> GetPotentialReferences<T>(object contextElement)
        {
            var potentialReferences = new List<T>();

            if (contextElement is IModelElement modelElement)
            {
                while (modelElement != null)
                {
                    var childrenWithIdentifier = modelElement.Children.OfType<T>();
                    potentialReferences.AddRange(childrenWithIdentifier);
                    modelElement = modelElement.Parent;
                }

            }

            return potentialReferences;
        }
    }
}
