using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NMF.Models.Services.Forms
{
    /// <summary>
    /// Denotes a service to access properties of the currently selected element
    /// </summary>
    public interface IPropertyService
    {
        /// <summary>
        /// Obtains the currently selected elements
        /// </summary>
        /// <returns>An object encapsulating the currently selected element</returns>
        [JsonRpcMethod("selectedElements")]
        IEnumerable<ModelElementInfo> GetSelectedElements();

        /// <summary>
        /// Gets called when the selected element changes
        /// </summary>
        event EventHandler<IEnumerable<ModelElementInfo>> SelectedElementsChanged;

        /// <summary>
        /// Patches the currently selected element
        /// </summary>
        /// <param name="updated">the element that should be patched</param>
        /// <returns>true, if the operation was successful, otherwise false</returns>
        [JsonRpcMethod("update")]
        bool ChangeSelectedElement(ModelElementInfo updated);

        /// <summary>
        ///  Send a `shutdown` notification to the server.
        /// </summary>
        [JsonRpcMethod("shutdown")]
        Task ShutdownAsync();

        /// <summary>
        /// Notifies the server that the given URI has been opened
        /// </summary>
        /// <param name="uri">The URI of the model that should be observed</param>
        [JsonRpcMethod("observeUri")]
        void ObserveUri(string uri);
    }
}
