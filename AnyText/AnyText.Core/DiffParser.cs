using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    /// <summary>
    /// Denotes a class that parses Git diff files
    /// </summary>
    public static class DiffParser
    {
        private static Regex LineNoRegex = new Regex(@"\+(?<num>\d+)", RegexOptions.Compiled);

        /// <summary>
        /// Converts the given collection of lines to a collection of text edits
        /// </summary>
        /// <param name="diffLines">A collection of lines</param>
        /// <returns>A collection of text edits</returns>
        public static IReadOnlyList<TextEdit> ToTextEdits(IEnumerable<string> diffLines)
        {
            if (diffLines == null)
            {
                return null;
            }

            var edits = new List<TextEdit>();
            var startLine = -1;
            var diffLength = 0;
            var lines = new List<string>();
            foreach (var line in diffLines.SkipWhile(s => !s.StartsWith("@@")))
            {
                if (line.StartsWith("@@"))
                {
                    var match = LineNoRegex.Match(line);
                    if (match.Success && int.TryParse(match.Groups["num"].Value, out var lineNo))
                    {
                        startLine = lineNo - 1;
                        diffLength = 0;
                    }
                }
                else if (line.StartsWith("+"))
                {
                    lines.Add(line.Substring(1));
                }
                else if (line.StartsWith("-"))
                {
                    diffLength++;
                }
                else if (line.StartsWith(" "))
                {
                    ProcessDiff(edits, ref startLine, ref diffLength, lines);
                    startLine++;
                }
            }
            ProcessDiff(edits, ref startLine, ref diffLength, lines);
            return edits;
        }

        private static void ProcessDiff(List<TextEdit> edits, ref int startLine, ref int diffLength, List<string> lines)
        {
            if (diffLength > 0 || lines.Count > 0)
            {
                lines.Add(string.Empty);
                edits.Add(new TextEdit(
                    new ParsePosition(startLine, 0),
                    new ParsePosition(startLine + diffLength, 0),
                    lines.ToArray()));
                startLine += lines.Count - 1;
                lines.Clear();
                diffLength = 0;
            }
        }
    }
}
