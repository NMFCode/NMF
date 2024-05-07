using StreamJsonRpc;
using System;

namespace NMF.Models.Services.Forms
{
    /// <summary>
    /// Denotes a service to access properties of the currently selected element
    /// </summary>
    public interface IPropertyService
    {
        /// <summary>
        /// Obtains the currently selected element
        /// </summary>
        /// <returns>An object encapsulating the currently selected element</returns>
        [JsonRpcMethod("selectedElement")]
        ModelElementInfo GetSelectedElement();

        /// <summary>
        /// Gets called when the selected element changes
        /// </summary>
        event EventHandler<ModelElementInfo> SelectedElementChanged;

        /// <summary>
        /// Patches the currently selected element
        /// </summary>
        /// <param name="selectedElement">the element that should be patched</param>
        /// <returns>true, if the operation was successful, otherwise false</returns>
        [JsonRpcMethod("patchSelectedElement")]
        bool ChangeSelectedElement(ModelElementInfo selectedElement);
    }
}
