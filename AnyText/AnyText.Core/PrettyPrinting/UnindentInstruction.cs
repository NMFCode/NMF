namespace NMF.AnyText.PrettyPrinting
{
    internal class UnindentInstruction : FormattingInstruction
    {
        public override void Apply(PrettyPrintWriter writer)
        {
            writer.Unindent();
        }

        public override void Setup(PrettyPrintWriter writer)
        {
            writer.Unindent();
        }
    }
}
