namespace NMF.AnyText
{
    /// <summary>
    /// Denotes a range between two parse positions
    /// </summary>
    /// <param name="Start">The start of the range</param>
    /// <param name="End">The end of the range</param>
    public record struct ParseRange(ParsePosition Start, ParsePosition End)
    {

    }
}
