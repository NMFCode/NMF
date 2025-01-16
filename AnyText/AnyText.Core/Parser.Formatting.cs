using NMF.AnyText.PrettyPrinting;
using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NMF.AnyText
{
    public partial class Parser
    {
        /// <summary>
        /// Formats the current text using the parser's context and generates text edits based on the specified range of lines.
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
            start ??= new ParsePosition(0, 0);
            end ??= GetEndPosition();

            var formattedLines = GetFormattedLines(tabsize, insertSpaces);
            formattedLines = ApplyFormattingOptions(
                formattedLines, trimTrailingWhitespace, insertFinalNewline, trimFinalNewlines);

            return GenerateTextEdits(start.Value, end.Value, formattedLines);
        }

        private ParsePosition GetEndPosition()
        {
            var lastLineIndex = _context.Input.Length - 1;
            var lastLine = _context.Input[lastLineIndex];
            return new ParsePosition(lastLineIndex, lastLine.Length + 1);
        }

        private List<string> GetFormattedLines(uint tabsize, bool insertSpaces)
        {
            var indentation = insertSpaces ? new string(' ', (int)tabsize) : "\t";
            using var writer = new StringWriter();
            var prettyPrint = new PrettyPrintWriter(writer, indentation);
            Context.RootRule.Write(prettyPrint, Context, Context.RootRuleApplication as MultiRuleApplication);
            return writer.ToString().Split(new[] { '\n' }, StringSplitOptions.None).ToList();
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
                lines[^1] += "\n";

            return lines;
        }

        private TextEdit[] GenerateTextEdits(ParsePosition start, ParsePosition end, List<string> formattedLines)
        {
            var edits = new List<TextEdit>();
            var originalLines = _context.Input;

            for (int i = start.Line; i <= end.Line && i < formattedLines.Count; i++)
            {
                var original = originalLines[i];
                var formatted = formattedLines[i];

                if (!string.Equals(original, formatted, StringComparison.Ordinal))
                {
                    var startChar = i == start.Line ? start.Col : 0;
                    var endChar = i == end.Line ? end.Col : original.Length;

                    edits.Add(new TextEdit(
                        new ParsePosition(i, startChar),
                        new ParsePosition(i, endChar),
                        new[] { formatted }));
                }
            }

            AddExtraLinesEdits(edits, formattedLines, originalLines, start.Line);
            return edits.ToArray();
        }

        private void AddExtraLinesEdits(List<TextEdit> edits, List<string> formattedLines, string[] originalLines, int startLine)
        {
            if (formattedLines.Count > originalLines.Length)
            {
                var extraLinesStart = startLine + originalLines.Length;
                var extraLines = formattedLines
                    .Skip(extraLinesStart)
                    .ToArray();

                if (extraLines.Length > 0)
                {
                    edits.Add(new TextEdit(
                        new ParsePosition(extraLinesStart, 0),
                        new ParsePosition(extraLinesStart, 0),
                        extraLines));
                }
            }
        }
    }
}
