using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Model
{

    /// <summary>
    /// Denotes a class for a rule that applies escaping to a conversion
    /// </summary>
    public abstract class EscapedConvertRule<T> : ConvertRule<T>
    {
        /// <summary>
        /// Escapes the given string
        /// </summary>
        /// <param name="value">the unescaped string</param>
        /// <returns>the escaped string</returns>
        public abstract string Escape(string value);

        /// <summary>
        /// Unescapes the given string
        /// </summary>
        /// <param name="value">the escaped string</param>
        /// <returns>the unescaped string</returns>
        public abstract string Unescape(string value);

        /// <inheritdoc />
        public override RuleApplication CreateRuleApplication(string matched, ParsePosition position, ParsePositionDelta examined, ParseContext context)
        {
            try
            {
                var converted = Convert(Unescape(matched), context);
                return new EscapedConvertRuleApplication(this, matched, converted, position, examined);
            }
            catch (Exception ex)
            {
                return new FailedRuleApplication(this, position, examined, ex.Message);
            }
        }

        /// <inheritdoc />
        public override RuleApplication Synthesize(object semanticElement, ParsePosition position, ParseContext context)
        {
            if (semanticElement is T typedElement)
            {
                return new EscapedConvertRuleApplication(this, Escape(ConvertToString(typedElement, context)), typedElement, position, default);
            }
            return new FailedRuleApplication(this, position, default, "ConversionError");
        }

        private sealed class EscapedConvertRuleApplication : LiteralRuleApplication
        {
            private T _converted;

            public EscapedConvertRuleApplication(Rule rule, string literal, T converted, ParsePosition currentPosition, ParsePositionDelta examinedTo) : base(rule, literal, currentPosition, examinedTo)
            {
                _converted = converted;
            }

            public override object GetValue(ParseContext context)
            {
                return _converted;
            }

            protected override void OnMigrate(string oldValue, string newValue, ParseContext context)
            {
                if (newValue != oldValue)
                {
                    var rule = (EscapedConvertRule<T>)Rule;
                    try
                    {
                        _converted = rule.Convert(rule.Unescape(newValue), context);
                    }
                    catch (Exception ex)
                    {
                        context.AddDiagnosticItem(new DiagnosticItem(DiagnosticSources.Parser, this, ex.Message));
                    }
                    OnValueChange(this, context);
                }
            }

        }
    }
}
