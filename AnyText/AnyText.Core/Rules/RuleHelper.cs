using NMF.AnyText.PrettyPrinting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Rules
{
    internal static class RuleHelper
    {        
        public static string Stringify(object input)
        {
            switch (input)
            {
                case string str: return str;
                case IEnumerable<object> enumerable: return string.Join(' ', enumerable.Select(Stringify).Where(s => s != null));
                default: return input?.ToString();
            }
        }

        public static RuleApplication Star(ParseContext context, Rule rule, List<RuleApplication> applications, ParsePosition referencePosition, ref ParsePosition position, ref ParsePositionDelta examined)
        {
            var savedPosition = position;
            RuleApplication app;
            while (true)
            {
                app = context.Matcher.MatchCore(rule, context, ref position);
                var appExamined = (savedPosition + app.ExaminedTo) - referencePosition;
                examined = ParsePositionDelta.Larger(examined, appExamined);
                if (app.IsPositive)
                {
                    applications.Add(app);
                    savedPosition = position;
                }
                else
                {
                    break;
                }
            }
            position = savedPosition;
            return app;
        }

        public static ParsePositionDelta SynthesizeStar(object semanticObject, Rule rule, List<RuleApplication> applications, ParsePosition position, ParseContext context)
        {
            var savedPosition = position;
            // do not use a synthesis plan here, because that would cache decisions that may change between synthesis attempts
            while (rule.CanSynthesize(semanticObject, context))
            {
                var app = rule.Synthesize(semanticObject, position, context);
                if (app.IsPositive)
                {
                    applications.Add(app);
                    position += app.Length;
                }
                else
                {
                    break;
                }
            }
            return position - savedPosition;
        }

        public static IEnumerable<T> NullsafeConcat<T>(this IEnumerable<T> source, IEnumerable<T> second)
        {
            if (source == null)
            {
                return second;
            }
            if (second == null)
            {
                return source;
            }
            return source.Concat(second);
        }

        public static void ApplyFormattingInstructions(FormattingInstruction[] formattingInstructions, PrettyPrintWriter writer)
        {
            if (formattingInstructions != null)
            {
                foreach (var instruction in formattingInstructions)
                {
                    instruction.Apply(writer);
                }
            }
        }

        public static IEnumerable<SynthesisRequirement> GetOrCreateSynthesisRequirements(Rule innerRule, ref IEnumerable<SynthesisRequirement> field)
        {
            if (field == null)
            {
                field = innerRule.CreateSynthesisRequirements();
            }
            return field;
        }

        public static bool CanBeNull(Type type)
        {
            return !type.IsValueType || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }
    }
}
