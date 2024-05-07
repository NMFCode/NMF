using Microsoft.AspNetCore.Mvc;
using NMF.Models.Meta;
using NMF.Models.Repository.Serialization;
using NMF.Serialization.Json;
using NMF.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Models.Services.Forms.Controller
{
    /// <summary>
    /// Denotes a controller to obtain the selected element and properties
    /// </summary>
    [ApiController]
    [Route("/api/selection")]
    public partial class SelectionController : ControllerBase
    {
        private readonly IPropertyService _propertyService;
        private readonly IModelServer _modelServer;
        private readonly JsonModelSerializer _serializer;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="propertyService">the property service</param>
        /// <param name="serializer">the serializer that should be used</param>
        /// <param name="modelServer">the model server, the controller is connected to</param>
        public SelectionController(IPropertyService propertyService, IModelServer modelServer, JsonModelSerializer serializer)
        {
            _propertyService = propertyService;
            _modelServer = modelServer;
            _serializer = serializer;
        }

        /// <summary>
        /// Gets the selected element
        /// </summary>
        /// <returns>A structure representing the currently selected element and its schema</returns>
        [HttpGet]
        public ModelElementInfo Get()
        {
            return _propertyService.GetSelectedElement();
        }

        /// <summary>
        /// Patches the selected element with the given properties
        /// </summary>
        /// <returns>An action result indicating whether the patch was successful</returns>
        [HttpPatch]
        public IActionResult Patch()
        {
            var selected = _modelServer.SelectedElement;

            if (selected == null)
            {
                return BadRequest();
            }

            var reader = new Utf8JsonStreamReader(Request.Body, 2048);
            var updatedElement = _serializer.DeserializeFragment(ref reader, _modelServer.Repository, selected.Model) as IModelElement;

            if (_propertyService.ChangeSelectedElement(new ModelElementInfo(updatedElement, new SchemaElement(updatedElement, SchemaWriter.Instance))))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
