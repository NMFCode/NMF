namespace NMF.AnyText.PrettyPrinting
{
    internal class UnindentInstruction : FormattingInstruction
    {
        public override void Apply(PrettyPrintWriter writer)
        {
            writer.Unindent();
        }
    }
}
