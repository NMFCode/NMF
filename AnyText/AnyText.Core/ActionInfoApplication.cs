using NMF.AnyText.Rules;

namespace NMF.AnyText
{
    /// <summary>
    /// Denotes an Action applied at a rule application
    /// </summary>
    /// <param name="Action">the action that is applied</param>
    /// <param name="RuleApplication">the rule application that is applied</param>
    public record struct ActionInfoApplication(CodeActionInfo Action, RuleApplication RuleApplication)
    {
    }
}
