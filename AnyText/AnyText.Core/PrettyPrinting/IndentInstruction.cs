namespace NMF.AnyText.PrettyPrinting
{
    internal class IndentInstruction : FormattingInstruction
    {
        public override void Apply(PrettyPrintWriter writer)
        {
            writer.Indent();
        }
    }
}
