using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    /// <summary>
    /// Denotes a range of start and end positions in a document
    /// </summary>
    public class Range
    {
        public Position Start {  get; set; }
        public Position End { get; set; }
    }
}
