using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using LspTypes;
using NMF.AnyText.Grammars;
using NMF.AnyText.Rules;
using NMF.Expressions;
using NMF.Models;
using NMF.Models.Services;
using Range = LspTypes.Range;

namespace NMF.AnyText
{
    /// <summary>
    ///     Encapsulates all logic for handling model changes and synchronizing across different parsers.
    /// </summary>
    public class SynchronizationService
    {
        private readonly ConcurrentDictionary<Parser, CancellationTokenSource> _parserCancellationSources = new();
        private readonly ConcurrentDictionary<Parser, Channel<BubbledChangeEventArgs>> _parserQueues = new();
        private readonly Dictionary<Grammar, List<ModelSynchronization>> _leftModelSyncs = new();
        private readonly Dictionary<Grammar, List<ModelSynchronization>> _rightModelSyncs = new();
        private readonly ILspServer _lspServer;
        private readonly IModelServer _modelServer;
        
        private sealed class ChildrenCount
        {
            public int Value { get; set; }
        }
        private static readonly ConditionalWeakTable<IModelElement, ChildrenCount> LastKnownChildrenCount = new();

        /// <summary>
        ///     Initializes a new instance of the <see cref="SynchronizationService" /> class with a reference to the LSP server.
        /// </summary>
        /// <param name="lspServer">The language server instance for sending workspace edits.</param>
        /// <param name="modelServer">The model server instance that is used by other syntaxes (GLSP Server).</param>
        public SynchronizationService(ILspServer lspServer, IModelServer modelServer)
        {
            _lspServer = lspServer;
            _modelServer = modelServer;
        }

        /// <summary>
        ///     Registers a model synchronization as a left-side synchronization for the given grammar.
        /// </summary>
        /// <param name="leftLanguage">The grammar of the left language.</param>
        /// <param name="sync">The synchronization logic to register.</param>
        public void RegisterLeftModelSync(Grammar leftLanguage, ModelSynchronization sync)
        {
            if (!_leftModelSyncs.TryGetValue(leftLanguage, out var syncList))
            {
                syncList = new List<ModelSynchronization>();
                _leftModelSyncs[leftLanguage] = syncList;
            }

            syncList.Add(sync);
        }

        /// <summary>
        ///     Registers a model synchronization as a right-side synchronization for the given grammar.
        /// </summary>
        /// <param name="rightLanguage">The grammar of the right language.</param>
        /// <param name="sync">The synchronization logic to register.</param>
        public void RegisterRightModelSync(Grammar rightLanguage, ModelSynchronization sync)
        {
            if (!_rightModelSyncs.TryGetValue(rightLanguage, out var syncList))
            {
                syncList = new List<ModelSynchronization>();
                _rightModelSyncs[rightLanguage] = syncList;
            }

            syncList.Add(sync);
        }


        /// <summary>
        ///     Processes model generation and synchronization between two URIs for a specified language.
        /// </summary>
        /// <param name="uri">The source URI of the first model.</param>
        /// <param name="uri2">The target URI of the second model.</param>
        /// <param name="parsers">A dictionary mapping URIs to parsers for the corresponding models.</param>
        /// <param name="grammars">A dictionary mapping language identifiers to their respective grammars.</param>
        public void ProcessModelGeneration(string uri, string uri2, Dictionary<string, Parser> parsers,
            Dictionary<string, Grammar> grammars)
        {
            var sourceUri = new Uri(Uri.UnescapeDataString(uri));
            var targetUri = new Uri(Uri.UnescapeDataString(uri2));

            var leftGrammar = GetGrammarFromUri(sourceUri, grammars);

            if (leftGrammar == null || !TryGetModel(uri, parsers, out var firstModelElement)) return;


            TryGetModel(uri2, parsers, out var secondModelElement);

            if (secondModelElement == null && !File.Exists(targetUri.AbsolutePath))
            {
                GenerateCorrespondingModel(targetUri, leftGrammar, firstModelElement, parsers, uri2);
            }
            else
            {
                var rightGrammar = GetGrammarFromUri(targetUri, grammars);
                SynchronizeModels(uri, uri2, firstModelElement, secondModelElement, leftGrammar, rightGrammar, parsers);
            }
        }


        /// <summary>
        ///     Orchestrates synchronization between a source parser and a collection of other parsers.
        /// </summary>
        /// <param name="parser">The parser that triggered the synchronization.</param>
        /// <param name="otherParsers">All other parsers to check for synchronization.</param>
        /// <param name="isManual">A flag indicating if the synchronization was manually triggered.</param>
        public void ProcessSync(Parser parser, IEnumerable<Parser> otherParsers, bool isManual = false)
        {
            var currentLang = parser.Context.Grammar;
            _leftModelSyncs.TryGetValue(currentLang, out var leftSyncs);
            _rightModelSyncs.TryGetValue(currentLang, out var rightSyncs);

            foreach (var otherParser in otherParsers)
            {
                if (otherParser == parser || !otherParser.Context.RootRuleApplication.IsPositive)
                    continue;

                var otherLang = otherParser.Context.Grammar;

                var collectedSyncs = new HashSet<ModelSynchronization>();
                if (leftSyncs != null)
                {
                    foreach (var sync in leftSyncs)
                    {
                        if ((sync.IsAutomatic || isManual) &&
                            sync.RightLanguage == otherLang)
                        {
                            sync.TrySynchronize(parser, otherParser, this);
                            collectedSyncs.Add(sync);

                        }
                    }
                }            

                if (rightSyncs != null)
                {
                    foreach (var sync in rightSyncs.Where(s => (s.IsAutomatic || isManual)
                                                               && s.LeftLanguage == otherLang
                                                               && !collectedSyncs.Contains(s)))
                    {
                        sync.TrySynchronize(otherParser, parser, this);
                    }
                }
                   
            }
        }

        /// <summary>
        ///     Creates a completion item that triggers manual model synchronization if applicable.
        /// </summary>
        /// <param name="parser">The parser for the current document.</param>
        /// <param name="fileUri">The URI of the file.</param>
        /// <returns>A <see cref="CompletionItem" /> to trigger manual sync, or null if not needed.</returns>
        public CompletionItem ProcessSyncCompletion(Parser parser, string fileUri)
        {
            var grammar = parser.Context.Grammar;
            var needsManualSync =
                (_leftModelSyncs.TryGetValue(grammar, out var leftSyncs) && leftSyncs.Any(s => !s.IsAutomatic)) ||
                (_rightModelSyncs.TryGetValue(grammar, out var rightSyncs) && rightSyncs.Any(s => !s.IsAutomatic));

            if (needsManualSync)
                return new CompletionItem
                {
                    Label = "Synchronize Document",
                    Detail = "Triggers synchronization with other documents",
                    Documentation =
                        "Initiates manual synchronization for non-automatic model synchronizations at document start.",
                    Command = new Command
                    {
                        CommandIdentifier = LspServer.SyncModelCommand,
                        Arguments = [fileUri]
                    },
                    TextEdit = new LspTypes.TextEdit
                    {
                        Range = new Range
                        {
                            Start = new Position(0, 0),
                            End = new Position(0, 0)
                        },
                        NewText = string.Empty
                    }
                };

            return null;
        }

        #region Model Change

        /// <summary>
        ///     Subscribes to model changes for the specified root element and processes updates.
        /// </summary>
        /// <param name="rootElement">The root model element to monitor for changes.</param>
        /// <param name="parser">The parser responsible for the parsed text that represents the model</param>
        public void SubscribeToModelChanges(IModelElement rootElement, Parser parser)
        {
            if (_parserQueues.ContainsKey(parser)) return;

            var channel = Channel.CreateUnbounded<BubbledChangeEventArgs>();
            var cts = new CancellationTokenSource();
            _parserQueues[parser] = channel;
            _parserCancellationSources[parser] = cts;

            _ = Task.Run(() => ProcessChangeQueueAsync(parser, channel, cts.Token), cts.Token);

            rootElement.BubbledChange += (_, e) => channel.Writer.TryWrite(e);
        }
        
        /// <summary>
        ///     Unsubscribes from model changes for a given parser and stops the associated background task.
        /// </summary>
        /// <param name="parser">The parser to unsubscribe.</param>
        public void UnsubscribeFromModelChanges(Parser parser)
        {
            if (_parserCancellationSources.TryRemove(parser, out var cts))
            {
                cts.Cancel();
                cts.Dispose();
            }
            _parserQueues.TryRemove(parser, out _);
        }
        
        private async Task ProcessChangeQueueAsync(Parser parser, Channel<BubbledChangeEventArgs> channel, CancellationToken cancellationToken)
        {
            try
            {
                await foreach (var e in channel.Reader.ReadAllAsync(cancellationToken))
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    await HandleModelChangeAsync(parser, e);
                }
            }
            catch (OperationCanceledException)
            {
                await Console.Error.WriteLineAsync($"Processing queue for parser {parser.Context.FileUri} was canceled.");
            }
            catch (Exception ex)
            {
                await Console.Error.WriteLineAsync($"An unexpected error occurred while processing model changes for {parser.Context.FileUri}: {ex.Message}");   
            }
        }

        private async Task HandleModelChangeAsync(Parser parser, BubbledChangeEventArgs e)
        {
            var edits = GetEditsFromChange(parser, e);
            if (edits.Count > 0)
            {
                var workspaceEdit =
                    parser.Context.TrackAndCreateWorkspaceEdit(edits.ToArray(), parser.Context.FileUri);
                if (_lspServer != null) 
                    await _lspServer.ApplyWorkspaceEditAsync(workspaceEdit, "newChange");
            
            }
        }


        private static List<TextEdit> GetEditsFromChange(Parser parser, BubbledChangeEventArgs e)
        {
            return WithSynthesizedModel(parser, () =>
            {
                switch (e.ChangeType)
                {
                    case ChangeType.PropertyChanged:
                        return ProcessPropertyChanged(parser, e).ToList();
                    case ChangeType.CollectionChanged:
                        if (!parser.Context.IsParsing)
                            return ProcessCollectionChanged(parser, e).ToList();
                        return [];
                    default:
                        return [];
                }
            });
        }

        private static IEnumerable<TextEdit> ProcessPropertyChanged(Parser parser, BubbledChangeEventArgs e)
        {
            var edits = new List<TextEdit>();
            var origArgs = (ValueChangedEventArgs)e.OriginalEventArgs;

            if (origArgs.OldValue == null || origArgs.NewValue == null) return edits;

            if (!parser.Context.IsParsing)
                edits.AddRange(HandleElementChanged(parser, e));

            if (parser.Context.TryGetReferences(e.Element, out var references) && references.Count > 1)
                edits.AddRange(
                    HandleReferenceChanges(parser, origArgs.NewValue.ToString(), references.Skip(1)));

            return edits;
        }

        private static IEnumerable<TextEdit> ProcessCollectionChanged(Parser parser, BubbledChangeEventArgs e)
        {
            var collectionArgs = (NotifyCollectionChangedEventArgs)e.OriginalEventArgs;
            return HandleCollectionChanged(parser, e, collectionArgs);
        }

        private static IEnumerable<TextEdit> HandleCollectionChanged(Parser parser, BubbledChangeEventArgs e,
            NotifyCollectionChangedEventArgs args)
        {
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    return HandleCollectionAdd(parser, e, args);
                case NotifyCollectionChangedAction.Remove:
                    return HandleCollectionRemove(parser, e, args);
                case NotifyCollectionChangedAction.Replace:
                    return HandleCollectionReset(parser, e, args, true);
                case NotifyCollectionChangedAction.Move:
                    return [];
                case NotifyCollectionChangedAction.Reset:
                    return HandleCollectionReset(parser, e, args);
                default:
                    return [];
            }
        }


        private static IEnumerable<TextEdit> HandleCollectionAdd(Parser parser, BubbledChangeEventArgs e,
            NotifyCollectionChangedEventArgs args)
        {
            var element = e.Element;
            var currentChildrenCount = element.Children.Count();
            if (LastKnownChildrenCount.TryGetValue(element, out var lastCount) &&
                lastCount != null &&
                currentChildrenCount == lastCount.Value) return [];


            var context = parser.Context;
            if (!context.TryGetDefinition(element, out var definition)) return [];
            if (!definition.Rule.IsDefinition)
                definition = definition.Parent;

            var rule = definition.Rule;
            var synthesizedApp = rule.Synthesize(element, default, context);
            var input = rule.Synthesize(element, context);

            if (e.Element.Children.Count() != currentChildrenCount)
                return [];

            var start = definition.CurrentPosition;
            var lastInner = definition.Children.Last();
            var end = lastInner.CurrentPosition + lastInner.Length;
            var inputLines = input.Split(Environment.NewLine);

            if (end.Line == context.Input.Length)
            {
                end.Line--;
                end.Col = context.Input.Last().Length;
            }

            TextEdit[] edits = [new(start, end, inputLines)];
            parser.Unify(synthesizedApp, edits, true, args.Action);
            LastKnownChildrenCount.AddOrUpdate(element, new ChildrenCount(){ Value = currentChildrenCount});
            return edits;
        }

        private static IEnumerable<TextEdit> HandleCollectionRemove(Parser parser, BubbledChangeEventArgs e,
            NotifyCollectionChangedEventArgs args)
        {
            var context = parser.Context;
            var elementToDelete = args.OldItems?[0];

            var currentChildrenCount = e.Element.Children.Count();
            if (LastKnownChildrenCount.TryGetValue(e.Element, out var lastCount) &&
                lastCount != null &&
                currentChildrenCount == lastCount.Value) return [];

            if (context.TryGetDefinition(e.Element, out var currentDef) &&
                context.TryGetDefinition(elementToDelete, out var deletedDef))
            {
                if (!currentDef.Rule.IsDefinition)
                    currentDef = currentDef.Parent;
                if (!deletedDef.Rule.IsDefinition)
                    deletedDef = deletedDef.Parent;

                var edits = new List<TextEdit>();


                var start = deletedDef.CurrentPosition;
                var lastInner = deletedDef.Children.Last();
                var end = lastInner.CurrentPosition + lastInner.Length;

                if (parser.Context.TryGetReferences(elementToDelete, out var references) && references.Count > 1
                    && references.Any(r => r.ContextElement == e.Element))
                {
                    start = currentDef.CurrentPosition;
                    lastInner = currentDef.Children.Last();
                    end = lastInner.CurrentPosition + lastInner.Length;

                    var input = currentDef.Rule.Synthesize(e.Element, context) + Environment.NewLine;

                    var inputLines = input.Split(Environment.NewLine);
                    edits.Add(new TextEdit(start, end, inputLines));
                }
                else
                {
                    edits.Add(new TextEdit(start, end, []));
                }


                var synthesizedApp = currentDef.Rule.Synthesize(e.Element, default, context);
                if (e.Element.Children.Count() != currentChildrenCount)
                    return [];

                parser.Unify(synthesizedApp, edits.ToArray(), true, args.Action);

                LastKnownChildrenCount.AddOrUpdate(e.Element, new ChildrenCount(){ Value = currentChildrenCount});

                return edits.ToArray();
            }

            return [];
        }

        private static IEnumerable<TextEdit> HandleCollectionReset(Parser parser, BubbledChangeEventArgs e,
            NotifyCollectionChangedEventArgs args, bool isReplace = false)
        {
            var element = e.Element;
            var currentChildrenCount = e.Element.Children.Count();
       
            var context = parser.Context;
            if (!context.TryGetDefinition(element, out var definition)) return [];

            if (!definition.Rule.IsDefinition)
                definition = definition.Parent;

            var rule = definition.Rule;
            var synthesizedApp = rule.Synthesize(element, default, context);
            var input = rule.Synthesize(element, context);

            var start = definition.CurrentPosition;
            var inputLines = input.Split(Environment.NewLine);
            var lastInner = definition.Children.Last();
            var end = lastInner.CurrentPosition + lastInner.Length;

            TextEdit[] edits = [new(start, end, inputLines)];
            if (isReplace)
                context.ReplacedModelElement = args.OldItems?[0];
            parser.Unify(synthesizedApp, edits, true, args.Action);
            context.ReplacedModelElement = null;
            LastKnownChildrenCount.AddOrUpdate(e.Element, new ChildrenCount(){ Value = currentChildrenCount});
            return edits;
        }

        private static IEnumerable<TextEdit> HandleReferenceChanges(Parser parser, string newValue,
            IEnumerable<RuleApplication> refApps)
        {
            List<TextEdit> edits = [];
            foreach (var refApp in refApps)
            {
                var start = refApp.CurrentPosition;
                var end = start + refApp.Length;
                var inputLines = new[] { newValue };
                var refEdit = new TextEdit(start, end, inputLines);
                parser.Update(refEdit);
                edits.Add(refEdit);
            }

            return edits;
        }

        private static TextEdit[] HandleElementChanged(Parser parser, BubbledChangeEventArgs e)
        {
            var context = parser.Context;
            if (!context.TryGetDefinition(e.Element, out var definitionRuleApp)) return [];

            if (!definitionRuleApp.Rule.IsDefinition)
                definitionRuleApp = definitionRuleApp.Parent;

            var synthesizedApp = definitionRuleApp.Rule.Synthesize(e.Element, new ParsePosition(0, 0), context);
            var input = definitionRuleApp.Rule.Synthesize(e.Element, context) + Environment.NewLine;

            var start = definitionRuleApp.CurrentPosition;
            var end = start + definitionRuleApp.Length;


            var inputLines = input.Split(Environment.NewLine);
            var edits = new List<TextEdit>();
            edits.AddRange(CreateTextEditsInRange(inputLines, context.Input, start, end).ToArray());

            parser.Unify(synthesizedApp, edits.ToArray());
            return edits.ToArray();
        }

        #endregion

        #region Private Helpers

        private static List<TextEdit> CreateTextEditsInRange(
            string[] newLines,
            string[] oldLines,
            ParsePosition start,
            ParsePosition end)
        {
            var edits = new List<TextEdit>();

            if (start.Line == 1) start.Line = 0;
            if (start.Line < 0 || start.Line >= oldLines.Length ||
                end.Line < start.Line || end.Line > oldLines.Length)
                return edits;
            var maxLines = Math.Min(newLines.Length, end.Line - start.Line);

            for (var i = 0; i < maxLines; i++)
            {
                var inputLine = newLines[i];
                var contextLine = oldLines[start.Line + i];

                if (!string.Equals(inputLine.Trim(), contextLine.Trim(), StringComparison.Ordinal))
                {
                    var lineIndex = start.Line + i;

                    var leadingSpaces = contextLine.TakeWhile(char.IsWhiteSpace).Count();

                    var startCol = Math.Min(leadingSpaces, contextLine.Length);
                    startCol = Math.Max(startCol, start.Col);
                    var endCol = contextLine.Length;

                    edits.Add(new TextEdit(
                        new ParsePosition(lineIndex, startCol),
                        new ParsePosition(lineIndex, endCol),
                        [inputLine]
                    ));
                }
            }

            if (newLines.Length > oldLines.Length)
            {
                var linesToAppend = newLines.Skip(oldLines.Length).ToArray();
                var newText = string.Join(Environment.NewLine, linesToAppend);
                if (string.IsNullOrWhiteSpace(newText)) return edits;
                var insertionPos = new ParsePosition(oldLines.Length - 1, oldLines.Last().Length);

                if (!oldLines.Last().EndsWith(Environment.NewLine)) newText = Environment.NewLine + newText;

                edits.Add(new TextEdit(
                    insertionPos,
                    insertionPos,
                    [newText]
                ));
            }

            return edits;
        }


        private static T WithSynthesizedModel<T>(Parser parser, Func<T> action)
        {
            var context = parser.Context;
            context.ExecuteActivationEffects = true;
            try
            {
                return action();
            }
            finally
            {
                context.ExecuteActivationEffects = false;
            }
        }

        private bool TryGetModel(string uri, Dictionary<string, Parser> parsers, out IModelElement model)
        {
            if (parsers.TryGetValue(uri, out var parser))
            {
                model = (IModelElement)parser.Context.Root;
                return true;
            }

            if (_modelServer.Repository.Models.TryGetValue(new Uri(Uri.UnescapeDataString(uri)),
                    out var repositoryModel))
            {
                model = repositoryModel.Children.First();
                return true;
            }

            model = null;
            return false;
        }

        private Grammar GetGrammarFromUri(Uri fileUri, Dictionary<string, Grammar> grammars)
        {
            var extension = Path.GetExtension(fileUri.AbsolutePath).TrimStart('.');

            if (!string.IsNullOrEmpty(extension))
            {
                // Special case: nmeta → anymeta
                if (extension.Equals("nmeta", StringComparison.OrdinalIgnoreCase))
                    if (grammars.TryGetValue("anymeta", out var anymetaGrammar))
                        return anymetaGrammar;

                // Default case: lookup by actual extension
                if (grammars.TryGetValue(extension, out var grammar)) return grammar;
            }

            return null;
        }

        private void GenerateCorrespondingModel(Uri targetUri, Grammar grammar, IModelElement firstModelElement,
            Dictionary<string, Parser> parsers, string uri2)
        {
            var parser = grammar.CreateParser();

            WithSynthesizedModel(parser, () =>
            {
                var input = grammar.Root.Synthesize(firstModelElement, parser.Context);
                var syn = grammar.Root.Synthesize(firstModelElement, default, parser.Context);
                parser.UnifyInitialize(syn, input, targetUri);
                File.WriteAllText(targetUri.AbsolutePath, input);
                return true;
            });
            parsers[uri2] = parser;
            SubscribeToModelChanges(firstModelElement, parser);
        }

        private void SynchronizeModels(string uri, string uri2, IModelElement firstModelElement,
            IModelElement secondModelElement, Grammar leftGrammar, Grammar rightGrammar,
            Dictionary<string, Parser> parsers)
        {
            parsers.TryGetValue(uri, out var parser1);
            parsers.TryGetValue(uri2, out var parser2);

            if (leftGrammar == rightGrammar)
                SynchronizeSameGrammar(firstModelElement, parser2, leftGrammar);
            else
                SynchronizeCrossGrammar(firstModelElement, secondModelElement, parser1, parser2, leftGrammar,
                    rightGrammar);
        }

        private void SynchronizeSameGrammar(IModelElement sourceModel, Parser targetParser, Grammar grammar)
        {
            if (targetParser == null) return;

            WithSynthesizedModel(targetParser, () =>
            {
                var input = grammar.Root.Synthesize(sourceModel, targetParser.Context);
                var syn = grammar.Root.Synthesize(sourceModel, default, targetParser.Context);

                targetParser.UnifyInitialize(syn, input, targetParser.Context.FileUri, true);
                targetParser.Context.TrackAndCreateWorkspaceEdit([], targetParser.Context.FileUri);
                File.WriteAllText(targetParser.Context.FileUri.AbsolutePath, input);
                return true;
            });

            SubscribeToModelChanges(sourceModel, targetParser);
        }

        private void SynchronizeCrossGrammar(IModelElement model1, IModelElement model2, Parser parser1, Parser parser2,
            Grammar leftGrammar, Grammar rightGrammar)
        {
            _leftModelSyncs.TryGetValue(leftGrammar, out var leftSyncs);
            _rightModelSyncs.TryGetValue(leftGrammar, out var rightSyncs);

            ModelSynchronization executed = null;
            var leftSync = (leftSyncs ?? Enumerable.Empty<ModelSynchronization>())
                .FirstOrDefault(s => s.RightLanguage == rightGrammar);

            if (leftSync != null)
            {
                executed = leftSync;
                leftSync.TrySynchronize(model1, model2, parser1, parser2, this);
            }

            var rightSync = (rightSyncs ?? Enumerable.Empty<ModelSynchronization>())
                .FirstOrDefault(s => s.LeftLanguage == rightGrammar && s != executed);

            if (rightSync != null) rightSync.TrySynchronize(model2, model1, parser2, parser1, this);
        }

        #endregion
    }
}