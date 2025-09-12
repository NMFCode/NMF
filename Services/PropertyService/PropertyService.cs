using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NMF.Models.Changes;
using NMF.Models.Meta;
using NMF.Utilities;

namespace NMF.Models.Services.Forms
{
    /// <summary>
    /// The default implementation for a property service
    /// </summary>
    public class PropertyService : IPropertyService
    {
        private readonly IModelServer _modelServer;
        private EventHandler<IEnumerable<ModelElementInfo>> _selectionChanged;
        private IModelSession _lastSession;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="modelServer">the model server, the controller is connected to</param>
        public PropertyService(IModelServer modelServer)
        {
            _modelServer = modelServer;
        }

        /// <inheritdoc />
        public event EventHandler<IEnumerable<ModelElementInfo>> SelectedElementsChanged
        {
            add
            {
                if (_selectionChanged == null)
                {
                    _modelServer.SelectedElementChanged += ForwardModelServerChanged;
                    RegisterActiveSession();
                }
                _selectionChanged += value;
            }
            remove
            {
                _selectionChanged -= value;
                if (_selectionChanged == null)
                {
                    _modelServer.SelectedElementChanged -= ForwardModelServerChanged;
                    if (_lastSession != null)
                    {
                        _lastSession.PerformedOperation -= OnOperationPerformed;
                        _lastSession = null;
                    }
                }
            }
        }

        private void RegisterActiveSession()
        {
            if (_modelServer.ActiveSession != _lastSession)
            {
                if (_lastSession != null)
                {
                    _lastSession.PerformedOperation -= OnOperationPerformed;
                }
                _lastSession = _modelServer.ActiveSession;
                if (_lastSession != null)
                {
                    _lastSession.PerformedOperation += OnOperationPerformed;
                }
            }
        }

        private bool TransactionAffectsElements(IEnumerable<IModelChange> changes, IEnumerable<IModelElement> elements)
        {
            return changes.Any(ch => MatchesAny(ch, elements));
        }

        private bool MatchesAny(IModelChange change, IEnumerable<IModelElement> elements)
        {
            switch (change)
            {
                case IElementaryChange elementaryChange:
                    return elements.Contains(elementaryChange.AffectedElement);
                case IChangeTransaction changeTransaction:
                    return MatchesAny(changeTransaction.SourceChange, elements) || changeTransaction.NestedChanges.Any(ch => MatchesAny(ch, elements));
                default:
                    return false;
            }
        }

        private void OnOperationPerformed(object sender, ModelChangeSet e)
        {
            if (TransactionAffectsElements(e.Changes, _modelServer.SelectedElements))
            {
                RaiseSelectionChanged();
            }
        }

        private void ForwardModelServerChanged(object sender, EventArgs e)
        {
            RaiseSelectionChanged();
            RegisterActiveSession();
        }

        private void RaiseSelectionChanged()
        {
            _selectionChanged?.Invoke(this, GetSelectedElements());
        }

        /// <inheritdoc />
        public bool ChangeSelectedElement(ModelElementInfo updated)
        {
            if (updated.Uri == null || !Uri.TryCreate(updated.Uri, UriKind.Absolute, out var parsedUri))
            {
                return false;
            }

            if (_modelServer.ActiveSession == null)
            {
                return false;
            }

            var selected = _modelServer.Repository.Resolve(parsedUri);
            var updatedElement = updated.Data;

            if (updatedElement == null || selected == null || selected.GetType() != updatedElement.GetType() || !_modelServer.SelectedElements.Contains(selected))
            {
                return false;
            }

            return _modelServer.ActiveSession.PerformOperation(() => CopyAttributesAndReferences(selected, updatedElement));
        }

        /// <inheritdoc />
        public IEnumerable<ModelElementInfo> GetSelectedElements()
        {
            return _modelServer.SelectedElements.Select(ModelElementInfo.FromModelElement);
        }

        private void CopyAttributesAndReferences(IModelElement selected, IModelElement updatedElement)
        {
            var shadowedAttributes = new HashSet<IAttribute>();
            var shadowedReferences = new HashSet<IReference>();

            foreach (var cl in selected.GetClass().Closure(c => c.BaseTypes))
            {
                if (cl == ModelElement.ClassInstance) continue;
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

        /// <inheritdoc />
        public Task ShutdownAsync()
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public void ObserveUri(string uri)
        {
        }
    }
}
