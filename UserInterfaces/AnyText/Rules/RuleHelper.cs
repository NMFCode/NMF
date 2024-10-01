using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Rules
{
    internal static class RuleHelper
    {
        public static void MoveOverWhitespace(ParseContext context, ref ParsePosition position)
        {
            var lineNo = position.Line;
            var col = position.Col;
            while (lineNo < context.Input.Length)
            {
                var line = context.Input[lineNo];

                while (col < line.Length)
                {
                    if (!char.IsWhiteSpace(line[col]))
                    {
                        position = new ParsePosition(lineNo, col);
                        return;
                    }
                    col++;
                }
                lineNo++;
                col = 0;
            }
            position = new ParsePosition(lineNo, 0);
        }

        public static void Star(ParseContext context, Rule rule, List<RuleApplication> applications, ref ParsePosition position, ref ParsePositionDelta examined)
        {
            var savedPosition = position;
            while (true)
            {
                var app = context.Matcher.MatchCore(rule, context, ref position);
                if (app != null)
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
    }
}
