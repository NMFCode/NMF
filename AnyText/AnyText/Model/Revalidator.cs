using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Model
{
    internal class Revalidator<T> : ValidatorBase<T>
    {
        public Func<T, ParseContext, string> Validator { get; init; }

        public override void Process(T element, RuleApplication ruleApplication, ParseContext context)
        {
            var error = Validator(element, context);
            if (error != null)
            {
                ruleApplication = ruleApplication.GetIdentifier() ?? ruleApplication;

                var diagnostic = ruleApplication.DiagnosticItems.OfType<Diagnostic>()
                    .FirstOrDefault(e => e.Validator == Validator);
                if (diagnostic != null)
                {
                    if (!diagnostic.CheckIfStillExist(context))
                    {
                        context.RemoveDiagnosticItem(diagnostic);
                    }
                }
                else
                {
                    context.AddDiagnosticItem(new Diagnostic(Validator, ruleApplication, error, Severity));
                }
            }
        }

        private sealed class Diagnostic : DiagnosticItem
        {
            public Func<T, ParseContext, string> Validator { get; }

            public Diagnostic(Func<T, ParseContext, string> validator, RuleApplication ruleApplication, string message, DiagnosticSeverity severity) : base(DiagnosticSources.Validation, ruleApplication, message, severity)
            {
                Validator = validator;
            }

            public override bool CheckIfStillExist(ParseContext context)
            {
                if (RuleApplication.ContextElement is T element)
                {
                    var error = Validator(element, context);
                    Message = error;
                }
                return Message != null;
            }
        }
    }
}
