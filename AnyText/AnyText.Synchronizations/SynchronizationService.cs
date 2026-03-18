using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NMF.AnyText.Workspace;
using NMF.Models;
using NMF.Models.Services;
using NMF.Synchronizations.Inconsistencies;
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
        /// Orchestrates synchronization between a source parser and a collection of other parsers.
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
                        SendUpdates(parser, root);
                    }
                    if (sync == null)
                    {
                        sync = new TextSynchronization(root, parser, _lspServer);
                        _activeSynchronizations.Add(parser.Context.FileUri, sync);
                    }
                    sync.RunningSynchronizations.Add(running);
                    if (openedPartner != null)
                    {
                        SendUpdates(openedPartner, partnerRoot);
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

        private void SendUpdates(Parser parser, IModelElement root)
        {
            if (parser.Context.Root == null)
            {
                parser.Initialize(root);
                var edit = new TextEdit(default, default, parser.Context.Input);
                _ = _lspServer.ApplyWorkspaceEditAsync(parser.Context.TrackAndCreateWorkspaceEdit(new[] { edit }, parser.Context.FileUri), "synchronization")
                    ?.ContinueWith(t => _lspServer.SendDiagnosticsAsync(parser.Context, null), TaskScheduler.Default);                
            }
            else
            {
                var edits = parser.Update(root);
                if (edits != null && edits.Count > 0)
                {
                    _ = _lspServer.ApplyWorkspaceEditAsync(parser.Context.TrackAndCreateWorkspaceEdit(edits.ToArray(), parser.Context.FileUri), "synchronization");
                }
            }
        }

        /// <summary>
        /// Prepares the given document for an update
        /// </summary>
        /// <param name="document">the document that is about to update</param>
        public void PreparePartnerUpdate(Parser document)
        {
            var uri = document.Context.FileUri;
            if (_activeSynchronizations.TryGetValue(uri, out var textSync))
            {
                foreach (var partnerUri in textSync.RunningSynchronizations.SelectMany(s => s.SynchronizedUris))
                {
                    if (uri != partnerUri)
                    {
                        PrepareUpdate(partnerUri);
                    }
                }
            }
        }

        /// <summary>
        /// Prepares the given document for an update
        /// </summary>
        /// <param name="uris">the uris of the document that is about to update</param>
        public void PrepareUpdate(IEnumerable<Uri> uris)
        {
            foreach (var uri in uris)
            {
                PrepareUpdate(uri);
            }
        }

        /// <summary>
        /// Prepares the given document for an update
        /// </summary>
        /// <param name="uri">the uri of the document that is about to update</param>
        public void PrepareUpdate(Uri uri)
        {
            if (_activeSynchronizations.TryGetValue(uri, out var partner))
            {
                partner.StartListening();
            }
        }

        /// <summary>
        /// Completes the update of the given document
        /// </summary>
        /// <param name="document">the document that was updated</param>
        public void CompletePartnerUpdate(Parser document)
        {
            var uri = document.Context.FileUri;
            if (_activeSynchronizations.TryGetValue(uri, out var textSync))
            {
                foreach (var partnerUri in textSync.RunningSynchronizations.SelectMany(s => s.SynchronizedUris))
                {
                    if (uri != partnerUri)
                    {
                        CompleteUpdate(partnerUri);
                    }
                }
            }
        }

        /// <summary>
        /// Completes the update of the given document
        /// </summary>
        /// <param name="uris">the uris of the document that was updated</param>
        public void CompleteUpdate(IEnumerable<Uri> uris)
        {
            foreach(var uri in uris)
            {
                CompleteUpdate(uri);
            }
        }

        /// <summary>
        /// Completes the update of the given document
        /// </summary>
        /// <param name="uri">the uri of the document that was updated</param>
        public void CompleteUpdate(Uri uri)
        {
            if (_activeSynchronizations.TryGetValue(uri, out var partner))
            {
                partner.Complete();
            }
        }

        /// <summary>
        /// Gets inconsistencies for the given document
        /// </summary>
        /// <param name="document">the document for which inconsistencies should be calculated</param>
        /// <returns>A collection of inconsistencies</returns>
        public IEnumerable<IRunningSynchronization> GetSynchronizations(Parser document) => GetSynchronizations(document?.Context.FileUri);

        /// <summary>
        /// Gets inconsistencies for the given document
        /// </summary>
        /// <param name="document">the document for which inconsistencies should be calculated</param>
        /// <returns>A collection of inconsistencies</returns>
        public IEnumerable<IRunningSynchronization> GetSynchronizations(Uri document)
        {
            if (document != null && _activeSynchronizations.TryGetValue(document, out var textSync))
            {
                return textSync.RunningSynchronizations;
            }
            return Enumerable.Empty<IRunningSynchronization>();
        }
    }
}