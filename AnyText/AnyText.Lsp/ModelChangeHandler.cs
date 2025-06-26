using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
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
        /// <summary>
        /// Subscribes to model changes for the specified root element and processes updates.
        /// </summary>
        /// <param name="rootElement">The root model element to monitor for changes.</param>
        /// <param name="parser">The parser responsible for the parsed text that represents the model</param>
        /// <param name="uriKey">The URI key identifying the document being edited.</param>
        /// <param name="server">The language server protocol (LSP) server for applying workspace edits.</param>
        public static void SubscribeToModelChanges(IModelElement rootElement, Parser parser, string uriKey,
            ILspServer server)
        {
            rootElement.BubbledChange += (sender, e) =>
            {
                _ = HandleModelChangeAsync(parser, uriKey, e, server);
            };
        }

        private static async Task HandleModelChangeAsync(Parser parser, string uriKey, BubbledChangeEventArgs e,
            ILspServer server)
        {
            try
            {
                var context = parser.Context;
                var edits = new List<TextEdit>();
                
                switch (e.ChangeType)
                {
                    case ChangeType.PropertyChanged:
                        edits.AddRange(ProcessPropertyChange(parser, context, e));
                        break;
                    case ChangeType.CollectionChanged:
                        if(parser.Context.IsParsing) return;
                        edits.AddRange(ProcessCollectionChange(parser, context, e));
                        break;
                }


                if (edits.Count > 0)
                {
                    var workspaceEdit = context.TrackAndCreateWorkspaceEdit(edits.ToArray(), uriKey);
                    if (server != null)
                        await server.ApplyWorkspaceEditAsync(workspaceEdit, "newChange");
                }
            }
            catch (Exception ex)
            {
                await Console.Error.WriteLineAsync($"Unhandled exception in model change handler: {ex}");
            }
        }
        private static IEnumerable<TextEdit> ProcessPropertyChange(Parser parser, ParseContext context, BubbledChangeEventArgs e)
        {
            var edits = new List<TextEdit>();
            var origArgs = (ValueChangedEventArgs)e.OriginalEventArgs;

            if (origArgs.NewValue == null || origArgs.OldValue == null)
            {
                return edits;
            }
            if(!parser.Context.IsParsing)
                edits.AddRange(HandleElementChanged(parser, context, e));

            if (context.TryGetReferences(e.Element, out var references) && references.Count > 1)
            {
                edits.AddRange(HandleElementReference(parser, context, e, references.Skip(1)));
            }

            return edits;
        }

        private static IEnumerable<TextEdit> ProcessCollectionChange(Parser parser, ParseContext context, BubbledChangeEventArgs e)
        {
            var collectionArgs = (NotifyCollectionChangedEventArgs)e.OriginalEventArgs;
            return HandleCollectionChanged(parser, context, e, collectionArgs, collectionArgs.Action);
        }

        private static TextEdit[] HandleCollectionChanged(Parser parser, ParseContext context, BubbledChangeEventArgs e,
            NotifyCollectionChangedEventArgs args, NotifyCollectionChangedAction changetype)
        {
            context.UsesSynthesizedModel = true;
            TextEdit[] edits = null;
            try
            {
                switch (changetype)
                {
                    case NotifyCollectionChangedAction.Add:
                        edits = HandleCollectionAdd(parser, context, e, args);
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        edits = HandleCollectionRemove(parser, context, e, args);
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        edits = [];
                        break;
                    case NotifyCollectionChangedAction.Move:
                        edits = HandleCollectionMove(parser, context, e, args);
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        edits = [];
                        break;
                    default:
                        return [];
                }
            }
            finally
            {
                context.UsesSynthesizedModel = false;
            }
            return edits;
        }

        private static TextEdit[] HandleCollectionMove(Parser parser, ParseContext context, BubbledChangeEventArgs e,
            NotifyCollectionChangedEventArgs args)
        {
            TextEdit[] edits = null;
            return edits;
        }

        private static TextEdit[] HandleCollectionReset(Parser parser, ParseContext context, BubbledChangeEventArgs e,
            NotifyCollectionChangedEventArgs args)
        {
            TextEdit[] edits = null;
            return edits;
        }

        private static TextEdit[] HandleCollectionRemove(Parser parser, ParseContext context, BubbledChangeEventArgs e,
            NotifyCollectionChangedEventArgs args)
        {
            var elementToDelete = args.OldItems?[0];

            if (context.TryGetDefinition(e.Element, out var currentDef) &&
                context.TryGetDefinition(elementToDelete, out var deletedDef))
            {
                var start = deletedDef.Parent.CurrentPosition;
                var end = start + deletedDef.Parent.Length;

                TextEdit[] edits = [new(start, end, [])];
                parser.Unificate(currentDef.Parent, edits, true, args.Action);
                return edits;
            }

            return [];
        }

        private static TextEdit[] HandleCollectionAdd(Parser parser, ParseContext context, BubbledChangeEventArgs e,
            NotifyCollectionChangedEventArgs args)
        {
            if (!context.TryGetDefinition(e.Element, out var definition))
            {
                return [];
            }
            
            var rule = definition.Parent.Rule;
            var syn = rule.Synthesize(e.Element, default, context);
            var input = rule.Synthesize(e.Element, context);

            var start = definition.Parent.CurrentPosition;
            
            var end = start + definition.Parent.Length;
            var inputLines = input.Split(Environment.NewLine);
            if (end.Line+1 == inputLines.Length)
                end.Line += 1;
            
            TextEdit[] edits = [new(start, end, inputLines)];
            
            parser.Unificate(syn, edits, true, args.Action);
            return edits;
        }

        private static TextEdit[] HandleElementReference(Parser parser, ParseContext context, BubbledChangeEventArgs e,
            IEnumerable<RuleApplication> refApps)
        {
            List<TextEdit> edits = [];
            context.UsesSynthesizedModel = true;
            try
            {
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
            }
            finally
            {
                context.UsesSynthesizedModel = false;
            }

            return edits.ToArray();
        }

        private static TextEdit[] HandleElementChanged(Parser parser, ParseContext context, BubbledChangeEventArgs e)
        {
            if (!context.TryGetDefinition(e.Element, out var def))
            {
                return [];
            }

            context.UsesSynthesizedModel = true;
            try
            {
                var definitionRuleApp = def;
                if(!definitionRuleApp.Rule.IsDefinition)
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
            finally
            { 
                context.UsesSynthesizedModel = false;
            }
        }

        private static List<TextEdit> CreateTextEditsInRange(
            string[] input,
            string[] contextInput,
            ParsePosition start,
            ParsePosition end)
        {
            var edits = new List<TextEdit>();

            
            if (start.Line < 0 || start.Line >= contextInput.Length || 
                end.Line < start.Line || end.Line > contextInput.Length)
            {
                return edits;
            }

            var inputLineCount = input.Length;
            var maxLines = Math.Min(inputLineCount, end.Line - start.Line + 1);

            for (var i = 0; i < maxLines; i++)
            {
                var inputLine = input[i];
                var contextLine = contextInput[start.Line + i];

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