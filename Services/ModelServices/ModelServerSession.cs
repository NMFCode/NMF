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
        private readonly string _path;

        /// <summary>
        /// Creates a new model session for the given server
        /// </summary>
        /// <param name="server">The model server</param>
        /// <param name="element">The element for which the session is opened</param>
        /// <param name="path">The file system path</param>
        public ModelServerSession(ModelServer server, IModelElement element, string path)
            : base(element)
        {
            _server = server;
            _path = path;
        }

        /// <inheritdoc />
        protected override void OnModelChanged()
        {
            _server.InformOtherSessions(this);
        }

        /// <inheritdoc />
        public override void Save()
        {
            _server.Repository.Save(Root, _path);
            base.Save();
        }

        /// <inheritdoc />
        protected override void OnElementSelect(IModelElement selected)
        {
            _server.SelectedElement = selected;
        }
    }
}
