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
    /// <summary>
    /// Denotes an LSP server that performs document synchronization
    /// </summary>
    public class SynchronizingLspServer : LspServer
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="grammars">the grammars supported by this LSP server</param>
        /// <param name="synchronizations">a collection of model synchronizations</param>
        /// <param name="modelServer">a local model server</param>
        public SynchronizingLspServer(IEnumerable<Grammar> grammars, IModelServer modelServer, params IModelSynchronization[] synchronizations)
            : this(grammars, modelServer, (IEnumerable<IModelSynchronization>)synchronizations)
        { }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="grammars">the grammars supported by this LSP server</param>
        /// <param name="synchronizations">a collection of model synchronizations</param>
        /// <param name="modelServer">a local model server</param>
        public SynchronizingLspServer(IEnumerable<Grammar> grammars, IModelServer modelServer, IEnumerable<IModelSynchronization> synchronizations)
            : base(grammars, true)
        {
            _synchronizationService = new SynchronizationService(this, modelServer, synchronizations);
        }

        private const string SyncModelCommand = "anytext.syncModel";

        private readonly SynchronizationService _synchronizationService;

        /// <inheritdoc />
        protected override void OpenNewDocument(Parser parser)
        {
            _synchronizationService.StartSynchronizing(parser, OpenDocuments);
        }

        /// <inheritdoc />
        protected override IEnumerable<CompletionItem> PostProcessCompletions(Parser document, IEnumerable<CompletionItem> completions)
        {
            var syncCompletion = CreateSynchronizationCompletionItem(document);
            if (syncCompletion != null)
            {
                return completions.Concat(completions);
            }
            return completions;
        }

        /// <inheritdoc />
        protected override bool HandleExtensionCommand(string commandIdentifier, object[] args)
        {
            switch (commandIdentifier)
            {
                case SyncModelCommand:
                    var uri = args[0].ToString();
                    if (TryGetOpenDocument(uri, out var document))
                    {
                        _synchronizationService.StartSynchronizing(document, OpenDocuments, true);
                    }
                    return true;

                default:
                    return false;
            }
        }

        private CompletionItem CreateSynchronizationCompletionItem(Parser parser)
        {
            var grammar = parser.Context.Grammar;
            var needsManualSync =
                !_synchronizationService.IsSynchronized(parser.Context.FileUri);

            if (needsManualSync)
                return new CompletionItem
                {
                    Label = "Synchronize Document",
                    Detail = "Triggers synchronization with other documents",
                    Documentation =
                        "Initiates manual synchronization for non-automatic model synchronizations at document start.",
                    Command = new Command
                    {
                        CommandIdentifier = SyncModelCommand,
                        Arguments = [parser.Context.FileUri.AbsoluteUri]
                    },
                    TextEdit = new LspTypes.TextEdit
                    {
                        Range = new LspTypes.Range
                        {
                            Start = new Position(0, 0),
                            End = new Position(0, 0)
                        },
                        NewText = string.Empty
                    }
                };

            return null;
        }
    }
}
