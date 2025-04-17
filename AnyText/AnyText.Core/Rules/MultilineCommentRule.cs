using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Rules
{
    /// <summary>
    /// Denotes a rule for a multiline comment
    /// </summary>
    public class MultilineCommentRule : CommentRule
    {
        /// <summary>
        /// Gets or sets the end of a multiline comment
        /// </summary>
        public string CommentEnd { get; set; }


        /// <inheritdoc />
        public override RuleApplication Match(ParseContext context, ref ParsePosition position)
        {
            if (position.Line >= context.Input.Length)
            {
                return null;
            }
            var line = context.Input[position.Line];
            if (line.Length < position.Col + CommentStart.Length)
            {
                return null;
            }

            if (MemoryExtensions.Equals(CommentStart, line.AsSpan(position.Col, CommentStart.Length), context.StringComparison))
            {
                var savedPosition = position;
                var commentBuilder = new StringBuilder();
                var lineIndex = position.Line;
                var endIndex = line.IndexOf(CommentEnd, position.Col + CommentStart.Length);
                line = line.Substring(position.Col);
                while (endIndex == -1 && lineIndex < context.Input.Length - 1)
                {
                    commentBuilder.AppendLine(line);
                    lineIndex++;
                    line = context.Input[lineIndex];
                    endIndex = line.IndexOf(CommentEnd);
                }
                if (endIndex != -1)
                {
                    var actualEnd = endIndex + CommentEnd.Length;
                    commentBuilder.AppendLine(line.Substring(0, actualEnd));
                    position = new ParsePosition(lineIndex, actualEnd);
                }
                else
                {
                    position = new ParsePosition(lineIndex, 0);
                }
                var length = position - savedPosition;
                return new LiteralRuleApplication(this, commentBuilder.ToString(), length, length);
            }

            return null;
        }
    }
}
