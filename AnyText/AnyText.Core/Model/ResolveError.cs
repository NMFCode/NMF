using NMF.AnyText.Rules;

namespace NMF.AnyText.Model
{
    internal sealed class ResolveError<TSemanticElement, TReference> : DiagnosticItem
        {
            public void UpdateMessage(string message)
            {
                Message = message;
            }

            public ResolveError(string source, RuleApplication ruleApplication, string message) : base(source, ruleApplication, message)
            {
            }

            public override bool CheckIfStillExist(ParseContext context)
            {
                var contextElement = RuleApplication.ContextElement;
                var resolveString = RuleApplication.GetValue(context) as string;
                var parent = (ResolveRule<TSemanticElement, TReference>)(RuleApplication.Rule);
                if (parent.InvalidateCore(RuleApplication, context, (TSemanticElement)contextElement, resolveString))
                {
                    (RuleApplication as ResolveRuleApplication<TSemanticElement, TReference>)?.ResetResolveError();
                    return false;
                }
                Message = parent.GetResolveErrorMessage(resolveString);
                return true;
            }
        }
}
