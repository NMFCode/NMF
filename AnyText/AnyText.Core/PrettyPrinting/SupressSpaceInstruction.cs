namespace NMF.AnyText.PrettyPrinting
{
    internal class SupressSpaceInstruction : FormattingInstruction
    {
        public override void Apply(PrettyPrintWriter writer)
        {
            writer.SupressSpace();
        }

        public override void Setup(PrettyPrintWriter writer)
        {
        }
    }
}
