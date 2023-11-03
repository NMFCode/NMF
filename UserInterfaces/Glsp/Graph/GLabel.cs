using System;
using System.Text.Json.Serialization;

namespace NMF.Glsp.Graph
{
    /// <summary>
    /// Denotes a label in a GLSP graph
    /// </summary>
    public class GLabel : GElement
    {
        /// <summary>
        /// Gets or sets the text of the label
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Raised when the text changed
        /// </summary>
        public event Action TextChanged;

        /// <summary>
        /// True, if the label supports changes, otherwise false
        /// </summary>
        [JsonIgnore]
        public bool SupportsLabelChange => TextChanged != null;
    }
}
