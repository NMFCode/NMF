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

        public static void Star(ParseContext context, Rule rule, List<RuleApplication> applications, ref ParsePosition position, ref ParsePositionDelta examined)
        {
            var savedPosition = position;
            while (true)
            {
                var app = context.Matcher.MatchCore(rule, context, ref position);
                if (app.IsPositive)
                {
                    applications.Add(app);
                    savedPosition = position;
                    examined = ParsePositionDelta.Larger(examined, app.ExaminedTo);
                }
                else
                {
                    break;
                }
            }
            position = savedPosition;
        }

        public static ParsePositionDelta SynthesizeStar(object semanticObject, Rule rule, List<RuleApplication> applications, ParsePosition position, ParseContext context)
        {
            var savedPosition = position;
            while (rule.CanSynthesize(semanticObject))
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
    }
}
