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

        /// <inheritdoc />
        public IModelSession<T> GetOrCreateSession<T>(Uri uri) where T : class, IModelElement
        {
            return GetOrCreateSession<T>(uri.AbsolutePath);
        }

        /// <inheritdoc />
        public IModelSession<T> GetOrCreateSession<T>(string path) where T : class, IModelElement
        {
            if (_sessions.TryGetValue(path, out var session) && session is ModelSession<T> modelSession)
            {
                return modelSession;
            }
            var model = _repository.Resolve(path);
            if (model == null)
            {
                model = new Model();
            }
            var element = model.RootElements.FirstOrDefault() as T;
            if (element == null)
            {
                element = (T)Activator.CreateInstance(typeof(T));
            }
            modelSession = new ModelSession<T>(this, element, path);
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
    }
}
