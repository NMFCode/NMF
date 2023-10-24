using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Types
{
    /// <summary>
    /// The Dimension of an object is composed of its width and height.
    /// </summary>
    /// <param name="Width">  The width of an element. </param>
    /// <param name="Height">  the height of an element. </param>
    public readonly record struct Dimension(double Width, double Height);
}
