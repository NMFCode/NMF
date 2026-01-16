using System;
using System.Collections.Generic;
using System.Linq;
using NMF.AnyText.Workspace;
using NMF.Models;
using NMF.Models.Services;
using NMF.Utilities;

namespace NMF.AnyText
{
    /// <summary>
    ///     Encapsulates all logic for handling model changes and synchronizing across different parsers.
    /// </summary>
    public class SynchronizationService
    {
        private readonly List<IModelSynchronization> _synchronizations = new List<IModelSynchronization>();
        private readonly Dictionary<Uri, TextSynchronization> _activeSynchronizations = new Dictionary<Uri, TextSynchronization>();
        private readonly ILspServer _lspServer;
        private readonly IModelServer _modelServer;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SynchronizationService" /> class with a reference to the LSP server.
        /// </summary>
        /// <param name="lspServer">The language server instance for sending workspace edits.</param>
        /// <param name="modelServer">The model server instance that is used by other syntaxes (GLSP Server).</param>
        /// <param name="synchronizations">the synchronizations supported by this service</param>
        public SynchronizationService(ILspServer lspServer, IModelServer modelServer, IEnumerable<IModelSynchronization> synchronizations)
        {
            _lspServer = lspServer;
            _modelServer = modelServer;
            _synchronizations = synchronizations.ToList();
        }

        /// <summary>
        /// Indicates whether the given uri is currently synchronized
        /// </summary>
        /// <param name="uri">the uri of the document</param>
        /// <returns>true, if a synchronization is active, otherwise false</returns>
        public bool IsSynchronized(Uri uri)
        {
            return _activeSynchronizations.TryGetValue(uri, out var synchronization) && synchronization != null;
        }

        /// <summary>
        /// Indicates whether the given Uri can be synchronized
        /// </summary>
        /// <param name="uri">the uri of the document</param>
        /// <returns>true, if a synchronization is available, otherwise false</returns>
        public bool CanSynchronize(Uri uri)
        {
            foreach (var synchronization in _synchronizations)
            {
                if (synchronization.CanSynchronize(uri, out _))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Stops all synchronizations of the given parser
        /// </summary>
        /// <param name="parser">the document whose synchronization shall be stopped</param>
        public void StopSynchronization(Parser parser)
        {
            if (_activeSynchronizations.TryGetValue(parser.Context.FileUri, out var sync))
            {
                var syncsAffected = new HashSet<(Uri uri,TextSynchronization s)>() { (parser.Context.FileUri, sync) };
                foreach (var runningSync in sync.RunningSynchronizations)
                {
                    foreach(var uri in runningSync.SynchronizedUris)
                    {
                        if (uri != parser.Context.FileUri && _activeSynchronizations.TryGetValue(uri, out var synchronized))
                        {
                            syncsAffected.Add((uri, synchronized));
                            synchronized.RunningSynchronizations.Remove(runningSync);
                        }
                    }
                    runningSync.Dispose();
                }
                foreach (var textSynchronization in syncsAffected)
                {
                    if (textSynchronization.s == sync || textSynchronization.s.RunningSynchronizations.Count == 0)
                    {
                        textSynchronization.s.Dispose();
                        _activeSynchronizations.Remove(textSynchronization.uri);
                    }
                }
            }
        }


        /// <summary>
        ///     Orchestrates synchronization between a source parser and a collection of other parsers.
        /// </summary>
        /// <param name="parser">The parser that triggered the synchronization.</param>
        /// <param name="otherParsers">All other parsers to check for synchronization.</param>
        /// <param name="isManual">A flag indicating if the synchronization was manually triggered.</param>
        public void StartSynchronizing(Parser parser, IEnumerable<Parser> otherParsers, bool isManual = false)
        {
            if (_activeSynchronizations.ContainsKey(parser.Context.FileUri))
            {
                return;
            }

            var root = parser.Context.Root as IModelElement;

            var effectiveSynchronizations = !isManual ? _synchronizations.Where(s => s.IsAutomatic) : _synchronizations;
            TextSynchronization sync = null;

            foreach (var synchronization in effectiveSynchronizations)
            {
                if (synchronization.CanSynchronize(parser.Context.FileUri, out var synchronizationPartner))
                {
                    var openedPartner = otherParsers.FirstOrDefault(p => p.Context.FileUri == synchronizationPartner);
                    var partnerRoot = openedPartner?.Context.Root as IModelElement;
                    if (openedPartner == null && _modelServer != null && _modelServer.TryGetSession(synchronizationPartner, out var partnerSession))
                    {
                        partnerRoot = partnerSession.Root;
                    }
                    if (openedPartner == null && partnerRoot == null)
                    {
                        continue;
                    }
                    var running = synchronization.Synchronize(parser.Context.FileUri, ref root, synchronizationPartner, ref partnerRoot);
                    if (running == null)
                    {
                        continue;
                    }
                    if (parser.Context.Root == null && root != null)
                    {
                        parser.Update(root);
                    }
                    if (sync == null)
                    {
                        sync = new TextSynchronization(root, parser, _lspServer);
                        _activeSynchronizations.Add(parser.Context.FileUri, sync);
                    }
                    sync.RunningSynchronizations.Add(running);
                    if (openedPartner != null)
                    {
                        var edits = openedPartner.Update(partnerRoot);
                        if (edits != null && edits.Count > 0)
                        {
                            _ = _lspServer.ApplyWorkspaceEditAsync(openedPartner.Context.TrackAndCreateWorkspaceEdit(edits.ToArray(), openedPartner.Context.FileUri), "synchronization");
                        }
                        if (!_activeSynchronizations.TryGetValue(synchronizationPartner, out var partnerTextSync))
                        {
                            partnerTextSync = new TextSynchronization(partnerRoot, openedPartner, _lspServer);
                            _activeSynchronizations.Add(synchronizationPartner, partnerTextSync);
                        }
                        partnerTextSync.RunningSynchronizations.Add(running);
                    }
                }
            }
        }
    }
}