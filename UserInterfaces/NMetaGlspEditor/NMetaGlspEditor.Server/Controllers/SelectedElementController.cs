using Microsoft.AspNetCore.Mvc;
using NMF.Models;
using NMF.Models.Services;

namespace NMetaGlspEditor.Server.Controllers
{
    [ApiController]
    [Route("/api/selection")]
    public class SelectedElementController : ControllerBase
    {
        private readonly IModelServer _modelServer;

        public SelectedElementController(IModelServer modelServer)
        {
            _modelServer = modelServer;
        }

        [HttpGet]
        public IModelElement Get()
        {
            return _modelServer.SelectedElement;
        }
    }
}
