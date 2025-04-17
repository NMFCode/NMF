using NMF.AnyText.Rules;
using System.Runtime.Serialization;

namespace NMF.AnyText
{
    /// <summary>
    /// Denotes an inlay
    /// </summary>
    public class InlayEntry
    {

        /// <summary>
        /// The rule application associated with this hint
        /// </summary>
        public RuleApplication RuleApplication { get; internal set; }

        /// <summary>
        /// The label of this hint. A human readable string or an array of
        /// InlayHintLabelPart label parts.
        ///
        /// *Note* that neither the string nor the label part can be empty.
        /// </summary>
        public string Label { get; init; }

        /// <summary>
        /// True, if the inlay should be rendered after the rule application, otherwise false
        /// </summary>
        public bool IsTrailing { get; init; }

        /// <summary>
        /// Gets the position of this inlay entry
        /// </summary>
        public ParsePosition Position
        {
            get
            {
                if (IsTrailing)
                {
                    var lastInner = RuleApplication.GetLastInnerLiteral();
                    if (lastInner != null)
                    {
                        return lastInner.CurrentPosition + lastInner.Length;
                    }
                }
                return RuleApplication.CurrentPosition;
            }
        }
    }
}
