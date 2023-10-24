using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NMF.Glsp.Graph
{
    public class GLabel : GElement
    {
        public string Text { get; set; }

        public event Action TextChanged;

        [JsonIgnore]
        public bool SupportsLabelChange => TextChanged != null;
    }
}
