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
    public class SelectionController : ControllerBase
    {
        private readonly IModelServer _modelServer;
        private readonly JsonModelSerializer _serializer;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="serializer">the serializer that should be used</param>
        /// <param name="modelServer">the model server, the controller is connected to</param>
        public SelectionController(IModelServer modelServer, JsonModelSerializer serializer)
        {
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
            var selected = _modelServer.SelectedElement;
            if (selected == null)
            {
                return null;
            }
            return new ModelElementInfo(selected, new SchemaElement(selected, SchemaWriter.Instance));
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
            reader.Read();
            var updatedElement = _serializer.DeserializeFragment(ref reader, _modelServer.Repository, selected.Model) as IModelElement;

            if (updatedElement == null || selected.GetType() != updatedElement.GetType())
            {
                return BadRequest();
            }

            CopyAttributesAndReferences(selected, updatedElement);

            return Ok();
        }

        private void CopyAttributesAndReferences(IModelElement selected, IModelElement updatedElement)
        {
            var shadowedAttributes = new HashSet<IAttribute>();
            var shadowedReferences = new HashSet<IReference>();

            foreach (var cl in selected.GetClass().Closure(c => c.BaseTypes))
            {
                CopyAttributes(selected, updatedElement, cl, shadowedAttributes);
                CopyReferences(selected, updatedElement, cl, shadowedReferences);
            }
        }

        private void CopyReferences(IModelElement selected, IModelElement updatedElement, IClass cl, HashSet<IReference> shadows)
        {
            foreach (var reference in cl.References.Where(r => !r.IsContainment && !r.IsContainerReference()))
            {
                if (reference.UpperBound == 1)
                {
                    selected.SetReferencedElement(reference, updatedElement.GetReferencedElement(reference));
                }
                else if (reference.IsOrdered)
                {
                    SynchronizeCollections(updatedElement.GetReferencedElements(reference), selected.GetReferencedElements(reference));
                }
                else
                {
                    SynchronizeSets(updatedElement.GetReferencedElements(reference), selected.GetReferencedElements(reference));
                }

                if (reference.Refines != null)
                {
                    shadows.Add(reference.Refines);
                }
            }

            foreach (var constraint in cl.ReferenceConstraints.Where(c => c.Constrains != null))
            {
                shadows.Add(constraint.Constrains);
            }
        }

        private void SynchronizeCollections(IList source, IList target)
        {
            var index = 0;
            while (index < source.Count && index < target.Count)
            {
                target[index] = source[index];
                index++;
            }
            while (index < target.Count)
            {
                target.RemoveAt(index);
            }
            while (index < source.Count)
            {
                target.Add(source[index]);
                index++;
            }
        }

        private void SynchronizeSets(IList source, IList target)
        {
            if (SetEquals(source, target)) return;

            target.Clear();
            foreach (var item in source)
            {
                target.Add(item);
            }
        }

        private bool SetEquals(IList source, IList target)
        {
            if (source.Count != target.Count) return false;

            var items = new List<object>();
            foreach (var item in source)
            {
                items.Add(item);
            }
            foreach (var item in target)
            {
                if (!items.Remove(item)) return false; 
            }
            if (items.Any()) return false;

            return true;
        }

        private void CopyAttributes(IModelElement selected, IModelElement updatedElement, IClass cl, HashSet<IAttribute> shadows)
        {
            foreach (var att in cl.Attributes)
            {
                if (att.UpperBound == 1)
                {
                    selected.SetAttributeValue(att, updatedElement.GetAttributeValue(att));
                }
                else if (att.IsOrdered)
                {
                    SynchronizeCollections(updatedElement.GetAttributeValues(att), selected.GetAttributeValues(att));
                }
                else
                {
                    SynchronizeSets(updatedElement.GetAttributeValues(att), selected.GetAttributeValues(att));
                }

                if (att.Refines != null)
                {
                    shadows.Add(att.Refines);
                }
            }

            foreach (var constraint in cl.AttributeConstraints.Where(c => c.Constrains != null))
            {
                shadows.Add(constraint.Constrains);
            }
        }

        /// <summary>
        /// The record for a model element and schema information
        /// </summary>
        /// <param name="modelElement">the selected model element</param>
        /// <param name="schema">the schema</param>
        public record ModelElementInfo(IModelElement modelElement, SchemaElement schema);
    }
}
