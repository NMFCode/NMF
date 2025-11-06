using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using LspTypes;
using WorkspaceEdit = NMF.AnyText.Workspace.WorkspaceEdit;

namespace NMF.AnyText
{
    public partial class LspServer
    {
        /// <inheritdoc cref="ILspServer.ApplyWorkspaceEditAsync" />
        public async Task<ApplyWorkspaceEditResponse> ApplyWorkspaceEditAsync(WorkspaceEdit edit, string label)
        {
            var editParams = new ApplyWorkspaceEditParams
            {
                Label = label,
                Edit = LspTypesMapper.MapWorkspaceEdit(edit, "", true)
            };
        
            try
            {
                var result =
                    await _rpc.InvokeWithParameterObjectAsync<ApplyWorkspaceEditResponse>(
                        Methods.WorkspaceApplyEditName,
                        editParams);
                
                return result;
            }
            catch (Exception e)
            {
                var errorMessage = $"Error applying WorkspaceEdit. Exception: {e.Message}";
                await SendLogMessageAsync(MessageType.Error, errorMessage);
                return null;
            }
          
        }
        
    }
}