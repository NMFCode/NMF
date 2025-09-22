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
