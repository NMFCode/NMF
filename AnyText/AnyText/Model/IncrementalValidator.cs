using NMF.AnyText.Rules;
using NMF.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Model
{
    internal class IncrementalValidator<T> : ValidatorBase<T>
    {
        public ObservingFunc<T, string> Validator { get; init; }

        public override void Process(T element, RuleApplication ruleApplication, ParseContext context)
        {
            var diagnostic = ruleApplication.DiagnosticItems.OfType<Diagnostic>()
                .FirstOrDefault(e => e.Validator == Validator);
            if (diagnostic == null)
            {
                try
                {
                    var error = Validator.Observe(element);
                    error.Successors.SetDummy();
                    if (error.Value != null)
                    {
                        context.AddDiagnosticItem(new Diagnostic(error, ruleApplication, error.Value, Severity) { Validator = Validator });
                    }
                }
                catch (Exception ex)
                {
                    var message = $"Evaluating the validation resulted in an exception: {ex.Message}";
                    context.AddDiagnosticItem(new Diagnostic(Observable.Constant(message), ruleApplication, message, Severity));
                }
            }
        }

        private sealed class Diagnostic : DiagnosticItem
        {
            public INotifyValue<string> Value { get; }
            public ObservingFunc<T, string> Validator { get; init; }

            public Diagnostic(INotifyValue<string> value, RuleApplication ruleApplication, string message, DiagnosticSeverity severity) : base(DiagnosticSources.Validation, ruleApplication, message, severity)
            {
                Value = value;
            }

            public override bool CheckIfStillExist(ParseContext context)
            {
                Message = Value.Value;
                return true;
            }

            public override void Dispose()
            {
                Value.Successors.UnsetAll();
            }
        }
    }
}
