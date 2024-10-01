using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    /// <summary>
    /// The context in which a text is parsed
    /// </summary>
    public class ParseContext
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="rootRule">the root rule</param>
        /// <param name="matcher">the matcher for the context</param>
        /// <param name="stringComparison">the string comparison mode</param>
        public ParseContext(Rule rootRule, Matcher matcher, StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
        {
            RootRule = rootRule;
            Matcher = matcher;
            StringComparison = stringComparison;
        }

        /// <summary>
        /// Gets the root rule of this parse context
        /// </summary>
        public Rule RootRule { get; }

        /// <summary>
        /// Gets or sets the input text in lines
        /// </summary>
        public string[] Input { get; internal set; }

        /// <summary>
        /// Gets the matcher used in this parse context
        /// </summary>
        public Matcher Matcher { get; }

        /// <summary>
        /// Gets the string comparison mode
        /// </summary>
        public StringComparison StringComparison { get; }
    }
}
