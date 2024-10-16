using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.PrettyPrinting
{
    /// <summary>
    /// Denotes a formatting instruction
    /// </summary>
    public abstract class FormattingInstruction
    {
        /// <summary>
        /// Applies the formatting instruction to the provided pretty print writer
        /// </summary>
        /// <param name="writer">the writer to apply the instruction on</param>
        public abstract void Apply(PrettyPrintWriter writer);

        /// <summary>
        /// Denotes a shared instance for an indenting instruction
        /// </summary>
        public static readonly FormattingInstruction Indent = new IndentInstruction();

        /// <summary>
        /// Denotes a shared instance for an unindent instruction
        /// </summary>
        public static readonly FormattingInstruction Unindent = new UnindentInstruction();

        /// <summary>
        /// Denotes a shared instance for a newline instruction
        /// </summary>
        public static readonly FormattingInstruction Newline = new NewlineInstruction();
    }
}
