namespace NMF.Glsp.Protocol.Types
{
    /// <summary>
    /// A Point is composed of the (x,y) coordinates of an object.
    /// </summary>
    /// <param name="X">  The abscissa of the point. </param>
    /// <param name="Y">  The ordinate of the point. </param>
    public readonly record struct Point(double X, double Y);
}
