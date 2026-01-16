namespace NMF.AnyText.PrettyPrinting
{
    internal class NewlineInstruction : FormattingInstruction
    {
        public override void Apply(PrettyPrintWriter writer)
        {
            writer.WriteNewLine();
        }

        public override void Setup(PrettyPrintWriter writer)
        {
        }
    }
}
