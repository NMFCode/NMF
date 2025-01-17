using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NMF.AnyText.PrettyPrinting;
using NMF.AnyText.Rules;

namespace NMF.AnyText
{
    public partial class Parser
    {
        /// <summary>
        ///     Formats the current text using the parser's context and generates text edits based on the specified range of lines.
        /// </summary>
        public TextEdit[] Format(
            ParsePosition? start = null,
            ParsePosition? end = null,
            uint tabsize = 4,
            bool insertSpaces = true,
            Dictionary<string, object> otherOptions = null,
            bool trimTrailingWhitespace = false,
            bool insertFinalNewline = false,
            bool trimFinalNewlines = false)
        {
            var (startPos, endPos) = GetStartAndEndPositions(start, end);

            if (Context.Errors.Any())
                return new TextEdit[] { };

            var ruleApp = Context.Matcher.GetRuleApplicationsAt(startPos).FirstOrDefault(r => r.Rule.IsLiteral);
            if (ruleApp == null)
                return new TextEdit[] { };

            while (!(ruleApp.CurrentPosition <= startPos && ruleApp.CurrentPosition + ruleApp.Length >= endPos))
            {
                ruleApp = ruleApp.Parent;
                if (ruleApp == null)
                    return new TextEdit[] { };
            }

            var formattedLines = GetFormattedLines(tabsize, insertSpaces, ruleApp);

            formattedLines = ApplyFormattingOptions(
                formattedLines, trimTrailingWhitespace, insertFinalNewline, trimFinalNewlines);

            var originalLines = Context.Input.Skip(startPos.Line).Take(endPos.Line - startPos.Line + 1).ToArray();

            // Check if FormattedLines includes chars beyond the range
            if (!CompareStringArraysByContent(originalLines, formattedLines.ToArray())) return new TextEdit[] { };

            return GenerateTextEdits(startPos, endPos, originalLines, formattedLines);
        }

        private (ParsePosition startPos, ParsePosition endPos) GetStartAndEndPositions(ParsePosition? start,
            ParsePosition? end)
        {
            start ??= new ParsePosition(0, 0);
            end ??= GetEndPosition();
            return (start.Value, end.Value);
        }

        private bool CompareStringArraysByContent(string[] array1, string[] array2)
        {
            var firstString = RemoveWhitespace(string.Join("", array1));
            var secondString = RemoveWhitespace(string.Join("", array2));

            return firstString.Length == secondString.Length;
        }

        private string RemoveWhitespace(string input)
        {
            return new string(input.Where(c => !char.IsWhiteSpace(c)).ToArray());
        }

        private ParsePosition GetEndPosition()
        {
            var lastLineIndex = _context.Input.Length - 1;
            var lastLine = _context.Input[lastLineIndex];
            return new ParsePosition(lastLineIndex, lastLine.Length + 1);
        }

        private List<string> GetFormattedLines(uint tabsize, bool insertSpaces, RuleApplication ruleApp)
        {
            var indentation = insertSpaces ? new string(' ', (int)tabsize) : "\t";
            using var writer = new StringWriter();
            var prettyPrint = new PrettyPrintWriter(writer, indentation);
            ruleApp.Write(prettyPrint, Context);
            return writer.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();
        }

        private List<string> ApplyFormattingOptions(
            List<string> lines, bool trimTrailingWhitespace, bool insertFinalNewline, bool trimFinalNewlines)
        {
            if (trimTrailingWhitespace)
                lines = lines.Select(line => line.TrimEnd()).ToList();

            if (trimFinalNewlines)
                while (lines.Count > 0 && string.IsNullOrWhiteSpace(lines[^1]))
                    lines.RemoveAt(lines.Count - 1);

            if (insertFinalNewline && lines.Count > 0 && !string.IsNullOrEmpty(lines[^1]))
                lines[^1] += Environment.NewLine;

            return lines;
        }

        private TextEdit[] GenerateTextEdits(ParsePosition start, ParsePosition end, string[] originalLines,
            List<string> formattedLines)
        {
            var edits = new List<TextEdit>();

            for (var i = 0; i < originalLines.Length && i < formattedLines.Count; i++)
            {
                var original = originalLines[i];
                var formatted = formattedLines[i];

                if (!string.Equals(original, formatted, StringComparison.Ordinal))
                {
                    var currentLine = start.Line + i;
                    var startChar = i == 0 ? start.Col : 0;
                    var endChar = currentLine == end.Line ? end.Col : original.Length;

                    edits.Add(new TextEdit(
                        new ParsePosition(currentLine, startChar),
                        new ParsePosition(currentLine, endChar),
                        new[] { formatted }));
                }
            }

            AddExtraLinesEdits(edits, formattedLines, originalLines, start.Line);
            return edits.ToArray();
        }

        private void AddExtraLinesEdits(List<TextEdit> edits, List<string> formattedLines, string[] originalLines,
            int startLine)
        {
            if (formattedLines.Count > originalLines.Length)
            {
                var extraLinesStart = originalLines.Length;
                var extraLines = formattedLines
                    .Skip(extraLinesStart)
                    .ToArray();

                if (extraLines.Length > 0)
                    edits.Add(new TextEdit(
                        new ParsePosition(startLine + extraLinesStart, 0),
                        new ParsePosition(startLine + extraLinesStart, 0),
                        extraLines));
            }
        }
    }
}