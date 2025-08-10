using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
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
        private readonly ConcurrentDictionary<Parser, Channel<BubbledChangeEventArgs>> _parserQueues = new();
        private readonly Dictionary<Grammar, List<ModelSynchronization>> _leftModelSyncs = new();
        private readonly Dictionary<Grammar, List<ModelSynchronization>> _rightModelSyncs = new();
        private readonly ILspServer _lspServer;
        private readonly IModelServer _modelServer;

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
        /// <param name="lang">The language identifier used to look up the grammar.</param>
        /// <param name="parsers">A dictionary mapping URIs to parsers for the corresponding models.</param>
        /// <param name="grammars">A dictionary mapping language identifiers to their respective grammars.</param>
        public void ProcessModelGeneration(string uri, string uri2, string lang, Dictionary<string, Parser> parsers,
            Dictionary<string, Grammar> grammars)
        {
            if (!grammars.TryGetValue(lang, out var grammar))
            {
                Console.WriteLine($"Language not found: {lang}");
                return;
            }

            var sourceUri = new Uri(Uri.UnescapeDataString(uri));
            var targetUri = new Uri(Uri.UnescapeDataString(uri2));

            IModelElement firstModelElement = null;
            IModelElement secondModelElement = null;

            if (parsers.TryGetValue(uri, out var parser1))
                firstModelElement = (IModelElement)parser1.Context.Root;
            else if (_modelServer.Repository.Models.TryGetValue(sourceUri, out var model1))
                firstModelElement = model1.Children.First();
            if (parsers.TryGetValue(uri2, out var parser2))
                secondModelElement = (IModelElement)parser2.Context.Root;
            else if (_modelServer.Repository.Models.TryGetValue(targetUri, out var model2))
                secondModelElement = model2.Children.First();

            if (secondModelElement == null && firstModelElement != null && !File.Exists(targetUri.AbsolutePath))
            {
                var parser = grammar.CreateParser();
                parser.Context.UsesSynthesizedModel = true;

                var input = grammar.Root.Synthesize(firstModelElement, parser.Context);
                var syn = grammar.Root.Synthesize(firstModelElement, default, parser.Context);
                parser.UnifyInitialize(syn, input, targetUri);
                File.WriteAllText(targetUri.AbsolutePath, input);

                parser.Context.UsesSynthesizedModel = false;
                parsers[uri2] = parser;
                SubscribeToModelChanges(firstModelElement, parser);
            }

            if (firstModelElement == null || secondModelElement == null) return;


            var leftGrammar = grammar;
            var rightGrammar = grammar;

            _leftModelSyncs.TryGetValue(leftGrammar, out var leftSyncs);
            _rightModelSyncs.TryGetValue(leftGrammar, out var rightSyncs);

            ModelSynchronization executed = null;
            var leftSync = (leftSyncs ?? Enumerable.Empty<ModelSynchronization>())
                .FirstOrDefault(s => s.RightLanguage == rightGrammar);

            if (leftSync != null)
            {
                executed = leftSync;
                leftSync.TrySynchronize(firstModelElement, secondModelElement, parser1, parser2, this);
            }

            var rightSync = (rightSyncs ?? Enumerable.Empty<ModelSynchronization>())
                .FirstOrDefault(s => s.LeftLanguage == rightGrammar && s != executed);

            if (rightSync != null)
                rightSync.TrySynchronize(secondModelElement, firstModelElement, parser2, parser1, this);
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

                foreach (var s in (leftSyncs ?? Enumerable.Empty<ModelSynchronization>())
                         .Where(s => s.RightLanguage == otherLang && (s.IsAutomatic || isManual)))
                    s.TrySynchronize(parser, otherParser, this);

                foreach (var s in (rightSyncs ?? Enumerable.Empty<ModelSynchronization>())
                         .Where(s => s.LeftLanguage == otherLang && (s.IsAutomatic || isManual)))
                    s.TrySynchronize(otherParser, parser, this);
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
            _leftModelSyncs.TryGetValue(parser.Context.Grammar, out var leftSyncs);
            _rightModelSyncs.TryGetValue(parser.Context.Grammar, out var rightSyncs);
            if ((leftSyncs?.Any(s => !s.IsAutomatic) ?? false) || (rightSyncs?.Any(s => !s.IsAutomatic) ?? false))
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


        /// <summary>
        ///     Subscribes to model changes for the specified root element and processes updates.
        /// </summary>
        /// <param name="rootElement">The root model element to monitor for changes.</param>
        /// <param name="parser">The parser responsible for the parsed text that represents the model</param>
        public void SubscribeToModelChanges(IModelElement rootElement, Parser parser)
        {
            if (_parserQueues.ContainsKey(parser)) return;

            var channel = Channel.CreateUnbounded<BubbledChangeEventArgs>();
            _parserQueues[parser] = channel;

            _ = Task.Run(() => ProcessChangeQueueAsync(parser, channel));

            rootElement.BubbledChange += (sender, e) => channel.Writer.TryWrite(e);
        }

        private async Task ProcessChangeQueueAsync(Parser parser, Channel<BubbledChangeEventArgs> channel)
        {
            await foreach (var e in channel.Reader.ReadAllAsync()) await HandleModelChangeAsync(parser, e);
        }

        private async Task HandleModelChangeAsync(Parser parser, BubbledChangeEventArgs e)
        {
            try
            {
                var edits = await GetEditsFromChangeAsync(parser, e);

                if (edits.Count > 0)
                {
                    var workspaceEdit =
                        parser.Context.TrackAndCreateWorkspaceEdit(edits.ToArray(), parser.Context.FileUri.AbsoluteUri);
                    await _lspServer.ApplyWorkspaceEditAsync(workspaceEdit, "newChange");
                }
            }
            catch (Exception ex)
            {
                await Console.Error.WriteLineAsync($"Exception in model change handler: {ex}");
            }
        }


        private static Task<List<TextEdit>> GetEditsFromChangeAsync(Parser parser, BubbledChangeEventArgs e)
        {
            return Task.Run(() =>
            {
                var edits = new List<TextEdit>();

                switch (e.ChangeType)
                {
                    case ChangeType.PropertyChanged:
                        edits.AddRange(ProcessPropertyChanged(parser, e));
                        break;
                    case ChangeType.CollectionChanged:
                        if (!parser.Context.IsParsing)
                            edits.AddRange(ProcessCollectionChanged(parser, e));
                        break;
                }

                return edits;
            });
        }

        private static T WithSynthesizedModel<T>(Parser parser, Func<T> action)
        {
            var context = parser.Context;
            context.UsesSynthesizedModel = true;
            try
            {
                return action();
            }
            finally
            {
                context.UsesSynthesizedModel = false;
            }
        }

        private static IEnumerable<TextEdit> ProcessPropertyChanged(Parser parser, BubbledChangeEventArgs e)
        {
            var edits = new List<TextEdit>();
            var origArgs = (ValueChangedEventArgs)e.OriginalEventArgs;

            if (origArgs.NewValue == null || origArgs.OldValue == null) return edits;
            if (!parser.Context.IsParsing)
                edits.AddRange(WithSynthesizedModel(parser, () => HandleElementChanged(parser, e)));

            if (parser.Context.TryGetReferences(e.Element, out var references) && references.Count > 1)
                edits.AddRange(
                    WithSynthesizedModel(parser, () => HandleReferenceChanges(parser, e, references.Skip(1))));

            return edits;
        }

        private static IEnumerable<TextEdit> ProcessCollectionChanged(Parser parser, BubbledChangeEventArgs e)
        {
            var collectionArgs = (NotifyCollectionChangedEventArgs)e.OriginalEventArgs;
            return WithSynthesizedModel(parser,
                () => HandleCollectionChanged(parser, parser.Context, e, collectionArgs, collectionArgs.Action));
        }

        private static TextEdit[] HandleCollectionChanged(Parser parser, ParseContext context, BubbledChangeEventArgs e,
            NotifyCollectionChangedEventArgs args, NotifyCollectionChangedAction changeType)
        {
            switch (changeType)
            {
                case NotifyCollectionChangedAction.Add:
                    return HandleCollectionAdd(parser, e, args);
                case NotifyCollectionChangedAction.Remove:
                    return HandleCollectionRemove(parser, e, args);
                case NotifyCollectionChangedAction.Replace:
                    return [];
                case NotifyCollectionChangedAction.Move:
                    return [];
                case NotifyCollectionChangedAction.Reset:
                    return [];
                default:
                    return [];
            }
        }

        private static TextEdit[] HandleCollectionReset(Parser parser, ParseContext context, BubbledChangeEventArgs e,
            NotifyCollectionChangedEventArgs args)
        {
            TextEdit[] edits = null;
            return edits;
        }

        private static TextEdit[] HandleCollectionRemove(Parser parser, BubbledChangeEventArgs e,
            NotifyCollectionChangedEventArgs args)
        {
            var context = parser.Context;
            var elementToDelete = args.OldItems?[0];
            var currentChildrenCount = e.Element.Children.Count();

            if (context.TryGetDefinition(e.Element, out var currentDef) &&
                context.TryGetDefinition(elementToDelete, out var deletedDef))
            {
                var start = deletedDef.Parent.CurrentPosition;
                var end = start + deletedDef.Parent.Length;

                TextEdit[] edits = [new(start, end, [])];
                parser.Unify(currentDef.Parent, edits, true, args.Action);

                lastKnownChildrenCount[e.Element] = currentChildrenCount;
                return edits;
            }

            return [];
        }

        private static readonly Dictionary<IModelElement, int> lastKnownChildrenCount = new();

        private static TextEdit[] HandleCollectionAdd(Parser parser, BubbledChangeEventArgs e,
            NotifyCollectionChangedEventArgs args)
        {
            var element = e.Element;
            var currentChildrenCount = element.Children.Count();
            if (lastKnownChildrenCount.TryGetValue(element, out var lastCount) &&
                currentChildrenCount == lastCount) return [];


            var context = parser.Context;
            if (!context.TryGetDefinition(element, out var definition)) return [];
            var rule = definition.Parent.Rule;
            var syn = rule.Synthesize(element, default, context);
            if (e.Element.Children.Count() != currentChildrenCount)
                return [];
            var input = rule.Synthesize(element, context);

            var start = definition.Parent.CurrentPosition;
            var end = start + definition.Parent.Length;
            var inputLines = input.Split(Environment.NewLine);

            if (end.Line + 1 == inputLines.Length)
                end.Line += 1;
            if (end.Col != 0)
                end.Col -= 1;
            TextEdit[] edits = [new(start, end, inputLines)];
            parser.Unify(syn, edits, true, args.Action);
            lastKnownChildrenCount[element] = currentChildrenCount;
            return edits;
        }

        private static TextEdit[] HandleReferenceChanges(Parser parser, BubbledChangeEventArgs e,
            IEnumerable<RuleApplication> refApps)
        {
            List<TextEdit> edits = [];
            var orig = (ValueChangedEventArgs)e.OriginalEventArgs;
            foreach (var refApp in refApps)
            {
                var start = refApp.CurrentPosition;
                var end = start + refApp.Length;
                var inputLines = new[] { orig.NewValue.ToString() };
                var refEdit = new TextEdit(start, end, inputLines);
                edits.Add(refEdit);
                parser.Update(refEdit);
            }

            return edits.ToArray();
        }

        private static TextEdit[] HandleElementChanged(Parser parser, BubbledChangeEventArgs e)
        {
            var context = parser.Context;
            if (!context.TryGetDefinition(e.Element, out var def)) return [];

            var definitionRuleApp = def;
            if (!definitionRuleApp.Rule.IsDefinition)
                definitionRuleApp = def.Parent;

            var syn = definitionRuleApp.Rule.Synthesize(e.Element, new ParsePosition(0, 0), context);
            var input = definitionRuleApp.Rule.Synthesize(e.Element, context);

            var start = definitionRuleApp.CurrentPosition;
            var end = start + definitionRuleApp.Length;
            var inputLines = input.Split(Environment.NewLine);

            var edits = CreateTextEditsInRange(inputLines, context.Input, start, end).ToArray();
            
            parser.Unify(syn, edits);
            return edits;
        }

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
            var maxLines = Math.Min(newLines.Length, end.Line - start.Line + 1);

            for (var i = 0; i < maxLines; i++)
            {
                var inputLine = newLines[i];
                var contextLine = oldLines[start.Line + i];

                if (!string.Equals(inputLine.Trim(), contextLine.Trim(), StringComparison.Ordinal))
                {
                    var lineIndex = start.Line + i;

                    var leadingSpaces = contextLine.TakeWhile(char.IsWhiteSpace).Count();

                    var startCol = Math.Min(leadingSpaces, contextLine.Length);
                    var endCol = contextLine.Length;

                    edits.Add(new TextEdit(
                        new ParsePosition(lineIndex, startCol),
                        new ParsePosition(lineIndex, endCol),
                        [inputLine]
                    ));
                }
            }

            return edits;
        }
    }
}