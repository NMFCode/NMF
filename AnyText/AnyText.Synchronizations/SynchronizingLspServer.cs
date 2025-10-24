using LspTypes;
using NMF.AnyText.Grammars;
using NMF.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    public class SynchronizingLspServer : LspServer
    {
        public SynchronizingLspServer(Grammar[] grammars, ModelSynchronization[] synchronizations, IModelServer modelServer)
            : base(grammars)
        {
            _synchronizationService = new SynchronizationService(this, modelServer);
            foreach (var sync in synchronizations)
            {
                _synchronizationService.RegisterLeftModelSync(sync.LeftLanguage, sync);
                _synchronizationService.RegisterRightModelSync(sync.RightLanguage, sync);
            }
        }

        private const string CreateModelCommand = "anytext.createModelSync";
        /// <summary>
        /// VS Code Extension Command to Synchronize Existing  Models
        /// </summary>
        public const string SyncModelCommand = "anytext.syncModel";

        private readonly SynchronizationService _synchronizationService;

        protected override void OpenNewDocument(Parser parser)
        {
            _synchronizationService.ProcessSync(parser, OpenDocuments);
        }

        protected override IEnumerable<CompletionItem> PostProcessCompletions(Parser document, string documentUri, IEnumerable<CompletionItem> completions)
        {
            var syncCompletion = _synchronizationService.ProcessSyncCompletion(document, documentUri);
            if (syncCompletion != null)
            {
                return completions.Concat(completions);
            }
            return completions;
        }

        protected override bool HandleExtensionCommand(string commandIdentifier, object[] args)
        {
            switch (commandIdentifier)
            {
                case CreateModelCommand:

                    var uri = args[0].ToString();
                    var uri2 = args[1].ToString();
                    var lang = args[2].ToString();
                    if (string.IsNullOrEmpty(uri) || string.IsNullOrEmpty(uri2) || string.IsNullOrEmpty(lang))
                    {
                        Console.WriteLine("Invalid arguments received.");
                        return true;
                    }
                    _synchronizationService.ProcessModelGeneration(uri, uri2, Documents, Grammars);
                    return true;

                case SyncModelCommand:
                    uri = args[0].ToString();
                    if (TryGetOpenDocument(uri, out var document))
                    {
                        _synchronizationService.ProcessSync(document, OpenDocuments, true);
                    }
                    return true;

                default:
                    return false;
            }
        }
    }
}
