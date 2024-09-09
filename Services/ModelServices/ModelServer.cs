using NMF.Expressions;
using NMF.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NMF.Models.Services
{
    /// <summary>
    /// Denotes the standard implementation of a model server
    /// </summary>
    public class ModelServer : IModelServer
    {
        private readonly ModelRepository _repository = new ModelRepository();
        private readonly Dictionary<string, ModelSession> _sessions = new Dictionary<string, ModelSession>();
        private IEnumerable<IModelElement> _selectedElements = Enumerable.Empty<IModelElement>();
        private IModelSession _activeSession;

        /// <inheritdoc />
        public event EventHandler SelectedElementChanged;

        /// <inheritdoc />
        public IModelSession GetOrCreateSession(Uri uri)
        {
            return GetOrCreateSession(uri, uri.AbsolutePath);
        }

        /// <inheritdoc />
        public IModelSession GetOrCreateSession(Uri uri, string path)
        {
            if (_sessions.TryGetValue(path, out var session) && session is ModelSession modelSession)
            {
                return modelSession;
            }
            var model = _repository.Resolve(path) ?? new Model() { ModelUri = uri };
            modelSession = new ModelServerSession(this, model.RootElements.FirstOrDefault(), model, path);
            _sessions[path] = modelSession;
            return modelSession;
        }

        internal void InformOtherSessions(ModelSession changedSession)
        {
            foreach (var session in _sessions.Values)
            {
                if (session != changedSession)
                {
                    session.NotifyOperationPerformed();
                }
            }
        }

        /// <inheritdoc />
        public ModelRepository Repository => _repository;

        /// <inheritdoc />
        public IEnumerable<IModelElement> SelectedElements
        {
            get { return _selectedElements; }
        }

        /// <summary>
        /// Selects the given elements
        /// </summary>
        /// <param name="elements">The elements</param>
        /// <param name="session">The session that provides the elements</param>
        public void Select(IEnumerable<IModelElement> elements, IModelSession session)
        {
            _selectedElements = elements;
            _activeSession = session;
            SelectedElementChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc />
        public IModelSession ActiveSession => _activeSession;
    }
}
