using NMF.Expressions;
using NMF.Glsp.Processing;
using NMF.Glsp.Protocol.Notification;
using NMF.Glsp.Protocol.Types;
using NMF.Glsp.Protocol.Validation;
using System;
using System.Text.Json.Serialization;

namespace NMF.Glsp.Graph
{
    /// <summary>
    /// Denotes a label in a GLSP graph
    /// </summary>
    public class GLabel : GElement
    {
        private string _text;

        /// <summary>
        /// Gets or sets the text of the label
        /// </summary>
        public string Text
        {
            get => _text;
            set
            {
                if (_text != value)
                {
                    _text = value;
                    TextChanged?.Invoke(value);
                }
            }
        }

        /// <summary>
        /// Raised when the text changed
        /// </summary>
        public event Action<string> TextChanged;

        /// <summary>
        /// Denotes where to place the label on an edge
        /// </summary>
        public EdgeLabelPlacement EdgeLabelPlacement { get; set; }

        /// <summary>
        /// True, if the label supports changes, otherwise false
        /// </summary>
        [JsonIgnore]
        public bool SupportsLabelChange => TextChanged != null;

        /// <summary>
        /// Validates the given input text
        /// </summary>
        /// <param name="text">The text to validate</param>
        /// <returns>A validation status</returns>
        public ValidationStatus Validate(string text)
        {
            if (!SupportsLabelChange)
            {
                return new ValidationStatus
                {
                    Message = "label is readonly",
                    Severity = SeverityLevels.Warning
                };
            }

            return Skeleton.Validate(text, this) ?? new ValidationStatus
            {
                Message = string.Empty,
                Severity = SeverityLevels.Ok
            };
        }

        internal void OnTextChanged(object sender, ValueChangedEventArgs e)
        {
            Text = e.NewValue as string;
        }
    }
}
