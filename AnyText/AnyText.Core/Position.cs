using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    /// <summary>
    /// Denotes a position in a document
    /// </summary>
    public class Position
    {
        public uint Line {  get; set; }
        public uint Character { get; set; }
    }
}
