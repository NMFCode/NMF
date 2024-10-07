using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Rules
{
    /// <summary>
    /// Denotes a rule for parsing rules
    /// </summary>
    public abstract class Rule
    {
        /// <summary>
        /// Matches the the context at the provided position
        /// </summary>
        /// <param name="context">the context in which the rule is matched</param>
        /// <param name="position">the position in the input</param>
        /// <returns>the rule application for the provided position</returns>
        public abstract RuleApplication Match(ParseContext context, ref ParsePosition position);

        /// <summary>
        /// Gets called when a rule application is activated
        /// </summary>
        /// <param name="application">the rule application that is activated</param>
        /// <param name="context">the context in which the rule application is activated</param>
        protected internal virtual void OnActivate(RuleApplication application, ParseContext context) { }

        /// <summary>
        /// Gets called when a rule application is deactivated
        /// </summary>
        /// <param name="application">the rule application that is deactivated</param>
        /// <param name="context">the context in which the rule application is deactivated</param>
        protected internal virtual void OnDeactivate(RuleApplication application, ParseContext context) { }

        /// <summary>
        /// Gets called when the value of a rule application changes
        /// </summary>
        /// <param name="application">the rule application for which the value changed</param>
        /// <param name="context">the context in which the value changed</param>
        /// <returns>true, if the rule processed the value change, otherwise false (in which case the value change is propagated)</returns>
        protected internal virtual bool OnValueChange(RuleApplication application, ParseContext context) => false;

        /// <summary>
        /// True, if the rule permits trailing whitespaces, otherwise false
        /// </summary>
        public virtual bool TrailingWhitespaces => true;

        public virtual void Initialize(GrammarContext context) { }
    }
}
