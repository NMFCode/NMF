using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Model
{
    internal abstract class ValidatorBase<T>
    {
        public DiagnosticSeverity Severity { get; init; }

        public abstract void Process(T element, RuleApplication ruleApplication, ParseContext context);
    }
}
