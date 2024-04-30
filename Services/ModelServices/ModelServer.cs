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
        private IModelElement _selectedElement;

        /// <inheritdoc />
        public event EventHandler SelectedElementChanged;

        /// <inheritdoc />
        public IModelSession GetOrCreateSession(Uri uri)
        {
            return GetOrCreateSession(uri.AbsolutePath);
        }

        /// <inheritdoc />
        public IModelSession GetOrCreateSession(string path)
        {
            if (_sessions.TryGetValue(path, out var session) && session is ModelSession modelSession)
            {
                return modelSession;
            }
            var model = _repository.Resolve(path) ?? new Model();
            modelSession = new ModelServerSession(this, model.RootElements.FirstOrDefault(), path);
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

        internal ModelRepository Repository => _repository;

        /// <inheritdoc />
        public IModelElement SelectedElement
        {
            get { return _selectedElement; }
            set
            {
                if ( _selectedElement != value)
                {
                    _selectedElement = value;
                    SelectedElementChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}
