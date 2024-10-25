namespace NMF.Glsp.Protocol.Types
{
    /// <summary>
    /// The Dimension of an object is composed of its width and height.
    /// </summary>
    /// <param name="Width">  The width of an element. </param>
    /// <param name="Height">  the height of an element. </param>
    public readonly record struct Dimension(double Width, double Height);
}
