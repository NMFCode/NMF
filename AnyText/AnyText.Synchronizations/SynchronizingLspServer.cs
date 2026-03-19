using LspTypes;
using NMF.AnyText.Grammars;
using NMF.Models.Services;
using NMF.Synchronizations.Inconsistencies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Range = LspTypes.Range;

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
        internal const string RepairLeftCommand = "anytext.repairLeft";
        internal const string RepairRightCommand = "anytext.repairRight";

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

        private readonly Dictionary<string, (IInconsistency inconsistency, IRunningSynchronization synchronization)> _inconsistencies = new Dictionary<string, (IInconsistency, IRunningSynchronization)>();

        /// <inheritdoc />
        protected override IEnumerable<CodeLens> CodeLensesForDocument(Parser document, IEnumerable<CodeLens> codeLenses)
        {
            var uri = document.Context.FileUri;
            lock (_inconsistencies)
            {
                var assigned = _inconsistencies.Where(kv => kv.Value.synchronization.SynchronizedUris.Contains(uri)).ToDictionary(kv => kv.Value.inconsistency, kv => kv.Key);
                var inconsistencies = _synchronizationService.GetSynchronizations(document);
                if (inconsistencies != null)
                {
                    var lenses = new List<CodeLens>();
                    foreach (var runningSynchronization in inconsistencies)
                    {
                        var isLeft = runningSynchronization.IsLeft(document.Context.FileUri);
                        foreach (var inconsistency in runningSynchronization.Inconsistencies)
                        {
                            var element = isLeft ? inconsistency.LeftElement : inconsistency.RightElement;
                            if (element == null)
                            {
                                continue;
                            }
                            if (document.Context.TryGetDefinitions(element, out var definitions))
                            {
                                if (!assigned.Remove(inconsistency, out var id))
                                {
                                    id = Guid.NewGuid().ToString();
                                    _inconsistencies.Add(id, (inconsistency, runningSynchronization));
                                }
                                foreach (var definition in definitions)
                                {
                                    var pos = definition.CurrentPosition;
                                    var end = pos + definition.Length;
                                    if (inconsistency.CanResolveLeft)
                                    {
                                        lenses.Add(new CodeLens
                                        {
                                            Command = new Command
                                            {
                                                Title = inconsistency.DescribeLeft(),
                                                CommandIdentifier = RepairLeftCommand,
                                                Arguments = [id]
                                            },
                                            Range = new Range
                                            {
                                                Start = new Position((uint)pos.Line, (uint)pos.Col),
                                                End = new Position((uint)end.Line, (ushort)end.Col)
                                            }
                                        });
                                    }
                                    if (inconsistency.CanResolveRight)
                                    {
                                        lenses.Add(new CodeLens
                                        {
                                            Command = new Command
                                            {
                                                Title = inconsistency.DescribeRight(),
                                                CommandIdentifier = RepairRightCommand,
                                                Arguments = [id]
                                            },
                                            Range = new Range
                                            {
                                                Start = new Position((uint)pos.Line, (uint)pos.Col),
                                                End = new Position((uint)end.Line, (ushort)end.Col)
                                            }
                                        });
                                    }
                                }
                            }
                        }
                    }
                    foreach (var item in assigned.Values)
                    {
                        _inconsistencies.Remove(item);
                    }
                    return codeLenses.Concat(lenses);
                }
            }
            return codeLenses;
        }

        /// <inheritdoc />
        protected override IEnumerable<string> SystemCommands
        {
            get
            {
                yield return SyncModelCommand;
                yield return RepairLeftCommand;
                yield return RepairRightCommand;
            }
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
                case RepairLeftCommand:
                    lock (_inconsistencies)
                    {
                        if (_inconsistencies.TryGetValue(args[0].ToString(), out var inconsistency))
                        {
                            RepairLeft(inconsistency.inconsistency, inconsistency.synchronization);
                        }
                        else
                        {
                            Console.Error.WriteLine($"Could not resolve {args[0]} as an inconsistency");
                        }
                    }
                    return true;
                case RepairRightCommand:
                    lock (_inconsistencies)
                    {
                        if (_inconsistencies.TryGetValue(args[0].ToString(), out var inconsistency2))
                        {
                            RepairRight(inconsistency2.inconsistency, inconsistency2.synchronization);
                        }
                        else
                        {
                            Console.Error.WriteLine($"Could not resolve {args[0]} as an inconsistency");
                        }
                    }
                    return true;
                default:
                    return false;
            }
        }

        private void RepairRight(IInconsistency inconsistency, IRunningSynchronization synchronization)
        {
            var rightUri = synchronization.SynchronizedUris.First(u => !synchronization.IsLeft(u));
            _synchronizationService.PrepareUpdate(rightUri);
            inconsistency.ResolveRight();
            _synchronizationService.CompleteUpdate(rightUri);
        }

        private void RepairLeft(IInconsistency inconsistency, IRunningSynchronization synchronization)
        {
            var leftUri = synchronization.SynchronizedUris.First(synchronization.IsLeft);
            _synchronizationService.PrepareUpdate(leftUri);
            inconsistency.ResolveLeft();
            _synchronizationService.CompleteUpdate(leftUri);
        }

        /// <inheritdoc/>
        protected override void OnDocumentUpdate(Parser document, IEnumerable<TextEdit> edits, string uri)
        {
            _synchronizationService.PreparePartnerUpdate(document);
            try
            {
                base.OnDocumentUpdate(document, edits, uri);
            }
            finally
            {
                _synchronizationService.CompletePartnerUpdate(document);
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
