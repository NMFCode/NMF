using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.PrettyPrinting
{
    internal class SupressSpaceInstruction : FormattingInstruction
    {
        public override void Apply(PrettyPrintWriter writer)
        {
            writer.SupressSpace();
        }
    }
}
