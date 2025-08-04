using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using NMF.AnyText.Rules;
using NMF.Expressions;
using NMF.Models;

namespace NMF.AnyText
{
    /// <summary>
    ///     Handles model change subscriptions and updates for text synchronization.
    /// </summary>
    public static class ModelChangeHandler
    {
        private static readonly ConcurrentDictionary<Parser, Channel<BubbledChangeEventArgs>> parserQueues = new();

        /// <summary>
        ///     Subscribes to model changes for the specified root element and processes updates.
        /// </summary>
        /// <param name="rootElement">The root model element to monitor for changes.</param>
        /// <param name="parser">The parser responsible for the parsed text that represents the model</param>
        /// <param name="server">The language server protocol (LSP) server for applying workspace edits.</param>
        public static void SubscribeToModelChanges(IModelElement rootElement, Parser parser, ILspServer server)
        {
            if (parserQueues.ContainsKey(parser)) return;

            var channel = Channel.CreateUnbounded<BubbledChangeEventArgs>();
            parserQueues[parser] = channel;

            _ = Task.Run(() => ProcessChangeQueueAsync(parser, server, channel));

            rootElement.BubbledChange += (sender, e) => channel.Writer.TryWrite(e);

        }

        private static async Task ProcessChangeQueueAsync(Parser parser, ILspServer server,
            Channel<BubbledChangeEventArgs> channel)
        {
            await foreach (var e in channel.Reader.ReadAllAsync()) await HandleModelChangeAsync(parser, e, server);
        }

        private static async Task HandleModelChangeAsync(Parser parser, BubbledChangeEventArgs e,
            ILspServer server)
        {
            try
            {
                var edits = await GetEditsFromChangeAsync(parser, e);

                if (edits.Count > 0)
                {
                    var workspaceEdit =
                        parser.Context.TrackAndCreateWorkspaceEdit(edits.ToArray(), parser.Context.FileUri.AbsoluteUri);
                    if (server != null)
                        await server.ApplyWorkspaceEditAsync(workspaceEdit, "newChange");
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
                parser.Unificate(currentDef.Parent, edits, true, args.Action);
                
                lastKnownChildrenCount[e.Element] = currentChildrenCount;
                return edits;
            }

            return [];
        }
        private static readonly Dictionary<IModelElement, int> lastKnownChildrenCount = new();

        private static TextEdit[] HandleCollectionAdd(Parser parser, BubbledChangeEventArgs e,
            NotifyCollectionChangedEventArgs args)
        {
            var currentChildrenCount = e.Element.Children.Count();
            if (lastKnownChildrenCount.TryGetValue(e.Element, out var lastCount) && currentChildrenCount == lastCount)
            {
                return [];
            }

            
            var context = parser.Context;
            if (!context.TryGetDefinition(e.Element, out var definition)) return [];
            var rule = definition.Parent.Rule;
            var syn = rule.Synthesize(e.Element, default, context);
            var input = rule.Synthesize(e.Element, context);

            var start = definition.Parent.CurrentPosition;
            var end = start + definition.Parent.Length;
            var inputLines = input.Split(Environment.NewLine);

            if (end.Line + 1 == inputLines.Length)
                end.Line += 1;

            TextEdit[] edits = [new(start, end, inputLines)];
            parser.Unificate(syn, edits, true, args.Action);
            lastKnownChildrenCount[e.Element] = currentChildrenCount;
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

            parser.Unificate(syn, edits);
            return edits;
        }

        private static List<TextEdit> CreateTextEditsInRange(
            string[] newLines,
            string[] oldLines,
            ParsePosition start,
            ParsePosition end)
        {
            var edits = new List<TextEdit>();


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