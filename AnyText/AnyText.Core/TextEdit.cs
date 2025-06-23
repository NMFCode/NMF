using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    /// <summary>
    /// Denotes an edit for text
    /// </summary>
    public class TextEdit
    {
        private static readonly string[] EmptyString = { string.Empty };

        /// <summary>
        /// Creates a new text edit
        /// </summary>
        /// <param name="start">the start of the edit</param>
        /// <param name="end">the end of the edit</param>
        /// <param name="newText">the new text inserted between start and end</param>
        /// <exception cref="ArgumentException">thrown if start is after end</exception>
        public TextEdit(ParsePosition start, ParsePosition end, string[] newText)
        {
            if (start.Line > end.Line || (start.Line == end.Line && start.Col > end.Col))
            {
                throw new ArgumentException("End cannot be before start");
            }
            Start = start;
            End = end;
            NewText = newText;
            if (newText == null || newText.Length == 0)
            {
                NewText = EmptyString;
            }
        }

        /// <summary>
        /// Gets the start of the edit
        /// </summary>
        public ParsePosition Start { get; }

        /// <summary>
        /// Gets the end of the edit
        /// </summary>
        public ParsePosition End { get; }

        /// <summary>
        /// Gets the new text inserted between start and end
        /// </summary>
        public string[] NewText { get; }

        /// <summary>
        /// Applies the text edit to the given input
        /// </summary>
        /// <param name="input">an input array of string lines</param>
        /// <returns>a text array including the changes</returns>
        public string[] Apply(string[] input)
        {
            if (Start.Line == End.Line && NewText.Length == 1)
            {
                if (Start.Line < input.Length)
                {
                    return ApplyInlineChange(input);
                }
                else
                {
                    return ApplyAddLine(input);
                }
            }
            if (NewText.Length == End.Line - Start.Line + 1)
            {
                return ApplyInplaceChange(input);
            }
            return ApplyReconstructArray(input);
        }
        /// <summary>
        /// Updates the position after applying this edit.
        /// Only Updates the Postion if it is fully after the Edit.
        /// </summary>
        /// <param name="position">The position to update.</param>
        public void UpdatePosition(ref ParsePosition position)
        {
            var lineDelta = NewText.Length - (End.Line - Start.Line + 1);
            if (End < position)
            {
                position.Line += lineDelta;
                if (End.Line + lineDelta == position.Line) 
                {
                    var lastLineText = NewText[NewText.Length-1];
                    if (Start.Line == End.Line && NewText.Length == 1)
                    {
                        int columnDelta =  lastLineText.Length - (End.Col - Start.Col);
                      
                        position.Col += columnDelta;
                    }
                 
                    else
                    {
                        int columnDelta = lastLineText.Length - End.Col;
                        if(Start.Col > End.Col)
                        {
                            columnDelta += Start.Col;
                        }
                        position.Col += columnDelta;
                       
                    }
                }
            }
        }


        private string[] ApplyAddLine(string[] input)
        {
            var newArray = new string[End.Line + 1];
            Array.Copy(input, 0, newArray, 0, input.Length);
            newArray[End.Line] = NewText[0];
            return newArray;
        }

        private string[] ApplyReconstructArray(string[] input)
        {
            var offset = NewText.Length - (End.Line - Start.Line + 1);
            var newArray = new string[Math.Max(input.Length + offset, End.Line + offset + 1)];
            Array.Copy(input, 0, newArray, 0, Start.Line);
            if (Start.Line >= input.Length)
            {
                newArray[Start.Line] = NewText[0];
            }
            else
            {
                newArray[Start.Line] = input[Start.Line].Substring(0, Start.Col) + NewText[0];
            }
            if (NewText.Length > 2)
            {
                Array.Copy(NewText, 1, newArray, Start.Line + 1, NewText.Length - 2);
            }
            if (End.Line >= input.Length)
            {
                newArray[End.Line + offset] = NewText[NewText.Length - 1];
            }
            else
            {
                var endline = input[End.Line].Substring(End.Col);
                if (End.Line + offset == Start.Line)
                {
                    newArray[Start.Line] += endline;
                }
                else
                {
                    newArray[End.Line + offset] = NewText[NewText.Length - 1] + endline;
                }
            }
            if (input.Length > End.Line)
            {
                Array.Copy(input, End.Line + 1, newArray, End.Line + offset + 1, input.Length - End.Line - 1);
            }
            return newArray;
        }

        private string[] ApplyInplaceChange(string[] input)
        {
            input[Start.Line] = ChangeLine(input[Start.Line], Start.Col, int.MaxValue, NewText[0]);
            for (int i = 1; i < NewText.Length - 1; i++)
            {
                input[Start.Line + i] = ChangeLine(input[Start.Line + i], 0, int.MaxValue, NewText[i]);
            }
            input[End.Line] = ChangeLine(input[End.Line], 0, End.Col, NewText[^1]);
            return input;
        }

        private string[] ApplyInlineChange(string[] input)
        {
            // inline change, most common case
            input[Start.Line] = ChangeLine(input[Start.Line], Start.Col, End.Col, NewText.Length == 1 ? NewText[0] : string.Empty);
            return input;
        }
        

        private static string ChangeLine(string line, int start, int end, string newText)
        {
            string result = string.Empty;
            if (start > 0)
            {
                result = line.Substring(0, start);
            }
            if (!string.IsNullOrEmpty(newText))
            {
                result += newText;
            }
            if (end < line.Length)
            {
                result += line.Substring(end);
            }
            return result;
        }
    }
}
