namespace NMF.Glsp.Protocol.Types
{
    /// <summary>
    /// A Point is composed of the (x,y) coordinates of an object.
    /// </summary>
    /// <param name="X">  The abscissa of the point. </param>
    /// <param name="Y">  The ordinate of the point. </param>
    public readonly record struct Point(double X, double Y)
    {
        /// <summary>
        /// Calculates the difference of two points
        /// </summary>
        /// <param name="a">the first point</param>
        /// <param name="b">the second point</param>
        /// <returns>the result</returns>
        public static Point operator -(Point a, Point b)
        {
            return new Point(a.X - b.X, a.Y - b.Y);
        }

        /// <summary>
        /// Calculates the sum of two points
        /// </summary>
        /// <param name="a">the first point</param>
        /// <param name="b">the second point</param>
        /// <returns>the result</returns>
        public static Point operator +(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }
    }
}
