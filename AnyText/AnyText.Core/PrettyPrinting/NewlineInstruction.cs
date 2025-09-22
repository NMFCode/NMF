namespace NMF.AnyText.PrettyPrinting
{
    internal class NewlineInstruction : FormattingInstruction
    {
        public override void Apply(PrettyPrintWriter writer)
        {
            writer.WriteNewLine();
        }
    }
}
