using System.Threading.Tasks;
using LspTypes;
using WorkspaceEdit = NMF.AnyText.Workspace.WorkspaceEdit;

namespace NMF.AnyText
{
    public partial interface ILspServer
    { 
        /// <summary>
        ///     Sends a <c>workspace/applyEdit</c> to the client.
        /// </summary>
        /// <param name="edit">
        ///     The <c>WorkspaceEdit</c> to apply.
        /// </param>
        /// <param name="label">
        ///     A descriptive label for the edit operation.
        /// </param>
        /// <returns>
        ///     A <c>Task</c> representing the asynchronous operation, containing the <c>ApplyWorkspaceEditResponse</c>.
        /// </returns>
        Task<ApplyWorkspaceEditResponse> ApplyWorkspaceEditAsync(WorkspaceEdit edit, string label);

    }
}