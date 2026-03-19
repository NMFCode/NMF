using NMF.AnyText.Rules;
using System.Collections;
using System.Collections.Generic;

namespace NMF.AnyText
{
    /// <summary>
    /// Denotes a code lens applied to a rule application
    /// </summary>
    /// <param name="CodeLens">The code lens</param>
    /// <param name="RuleApplication">The rule application instance</param>
    /// <param name="Arguments">Additional arguments</param>
    public record struct CodeLensApplication(CodeLensInfo CodeLens, RuleApplication RuleApplication, IEnumerable<string> Arguments)
    {
    }
}
