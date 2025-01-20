using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Models.Services
{
    /// <summary>
    /// Denotes a model session at a model server
    /// </summary>
    public class ModelServerSession : ModelSession
    {
        private readonly ModelServer _server;
        private string _path;

        /// <summary>
        /// Creates a new model session for the given server
        /// </summary>
        /// <param name="server">The model server</param>
        /// <param name="element">The element for which the session is opened</param>
        /// <param name="path">The file system path</param>
        /// <param name="model">The encapsulated model</param>
        public ModelServerSession(ModelServer server, IModelElement element, Model model, string path)
            : base(element, model)
        {
            _server = server;
            _path = path;
        }

        /// <inheritdoc />
        protected override void OnModelChanged()
        {
            _server.InformOtherSessions(this);
        }

        /// <summary>
        /// Gets the path of the model
        /// </summary>
        public string Path => _path;

        /// <inheritdoc />
        public override void Save(Uri target)
        {
            if (target != null)
            {
                _path = target.IsAbsoluteUri ? target.AbsolutePath : target.OriginalString;
            }
            _server.Repository.Save(Root, _path);
            base.Save(target);
        }

        /// <inheritdoc />
        protected override void OnElementSelect(IEnumerable<IModelElement> selected)
        {
            _server.Select(selected, this);
        }
    }
}
