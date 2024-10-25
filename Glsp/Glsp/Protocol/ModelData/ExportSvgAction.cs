using NMF.Glsp.Protocol.BaseProtocol;

namespace NMF.Glsp.Protocol.ModelData
{
    /// <summary>
    /// The client sends an ExportSvgAction to indicate that the diagram, which represents the current model state, 
    /// should be exported in SVG format. The action only provides the diagram SVG as plain string. The expected 
    /// result of executing an ExportSvgAction is a new file in SVG-format on the underlying filesystem. However, 
    /// other details like the target destination, concrete file name, file extension etc. are not specified in the 
    /// protocol. So it is the responsibility of the action handler to process this information accordingly and 
    /// export the result to the underlying filesystem.
    /// </summary>
    public class ExportSvgAction : ResponseAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string ExportSvgActionKind = "exportSvg";

        /// <inheritdoc/>
        public override string Kind => ExportSvgActionKind;

        /// <summary>
        ///  The diagram GModel as serializable SVG.
        /// </summary>
        public string Svg { get; set; }
    }
}
