using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Model
{
    /// <summary>
    /// Denotes a rule that applies custom conversions
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConvertRule<T> : RegexRule
    {
        private static TypeConverter _converter = TypeDescriptor.GetConverter(typeof(T));


        /// <inheritdoc />
        public override RuleApplication CreateRuleApplication(string matched, ParsePosition position, ParsePositionDelta examined, ParseContext context)
        {
            try
            {
                var converted = Convert(matched, context);
                return new ConvertRuleApplication(this, position, matched, converted, examined);
            }
            catch (Exception ex)
            {
                return new FailedRuleApplication(this, position, examined, ex.Message);
            }
        }

        /// <summary>
        /// Converts the provided text to an element of type T
        /// </summary>
        /// <param name="text">the input text</param>
        /// <param name="context">the parse context</param>
        /// <returns>the parsed element</returns>
        public virtual T Convert(string text, ParseContext context)
        {
            return (T)_converter.ConvertFromInvariantString(text);
        }

        /// <summary>
        /// Converts the provided element to text
        /// </summary>
        /// <param name="semanticElement">the semantic element</param>
        /// <param name="context">the parse context</param>
        /// <returns>the input text</returns>
        public virtual string ConvertToString(T semanticElement, ParseContext context)
        {
            return _converter.ConvertToInvariantString(semanticElement);
        }

        /// <inheritdoc />
        public override bool CanSynthesize(object semanticElement, ParseContext context)
        {
            return semanticElement is T;
        }

        /// <inheritdoc />
        public override RuleApplication Synthesize(object semanticElement, ParsePosition position, ParseContext context)
        {
            if (semanticElement is T typedElement)
            {
                return CreateRuleApplication(ConvertToString(typedElement, context), position, default, context);
            }
            return new FailedRuleApplication(this, position, default, "ConversionError");
        }

        private sealed class ConvertRuleApplication : LiteralRuleApplication
        {
            private T _value;

            public ConvertRuleApplication(Rule rule, ParsePosition currentPosition, string literal, T value, ParsePositionDelta examinedTo) : base(rule, literal, currentPosition, examinedTo)
            {
                _value = value;
            }

            public override object GetValue(ParseContext context)
            {
                return _value;
            }

            protected override void OnMigrate(string oldValue, string newValue, ParseContext context)
            {
                if (newValue != oldValue)
                {
                    var rule = (ConvertRule<T>)Rule;
                    try
                    {
                        _value = rule.Convert(newValue, context);
                    }
                    catch (Exception ex)
                    {
                        context.Errors.Add(new DiagnosticItem(DiagnosticSources.Parser, this, ex.Message));
                    }
                    OnValueChange(this, context);
                }
            }
        }
    }
}
