using NMF.AnyText.Rules;

namespace NMF.AnyText
{
    /// <summary>
    /// Denotes a code lens applied to a rule application
    /// </summary>
    /// <param name="CodeLens">The code lens</param>
    /// <param name="RuleApplication">The rule application instance</param>
    public record struct CodeLensApplication(CodeLensInfo CodeLens, RuleApplication RuleApplication)
    {
    }
}
