using System;
using System.IO;
using System.Linq;
using LspTypes;
using NMF.Models;
using NMF.Models.Repository;
using Range = LspTypes.Range;

namespace NMF.AnyText
{
    public partial class LspServer
    {
        private void ProcessModelSynchronization(string uri1, string uri2)
        {
            if (!_documents.TryGetValue(uri1, out var parser1)) return;
            if (!_documents.TryGetValue(uri2, out var parser2)) return;
            
            var model1 = (IModelElement)parser1.Context.Root;
            var model2 = (IModelElement)parser2.Context.Root;
            ModelChangeHandler.SubscribeToModelChanges(model1, parser1, this);
            ModelChangeHandler.SubscribeToModelChanges(model2, parser2, this);

            _leftModelSyncs.TryGetValue(parser1.Context.Grammar, out var leftSyncs);
            _rightModelSyncs.TryGetValue(parser1.Context.Grammar, out var rightSyncs);

            foreach (var s in (leftSyncs ?? Enumerable.Empty<ModelSynchronization>())
                     .Where(s => s.RightLanguage == parser2.Context.Grammar))
                s.Synchronize(model1, model2);
            foreach (var s in (rightSyncs ?? Enumerable.Empty<ModelSynchronization>())
                     .Where(s => s.LeftLanguage == parser2.Context.Grammar))
                s.Synchronize(model2, model1);
        }


        private void ProcessModelGeneration(string uri, string uri2, string lang)
        {
            if (!_languages.TryGetValue(lang, out var grammar))
            {
                Console.WriteLine($"Language not found: {lang}");
                return;
            }

            var sourceUri = new Uri(Uri.UnescapeDataString(uri));
            var targetUri = new Uri(Uri.UnescapeDataString(uri2));

            var repo = new ModelRepository();
            var rootElement = repo.Resolve(sourceUri.AbsolutePath).RootElements.First();

            var parser = grammar.CreateParser();
            parser.Context.UsesSynthesizedModel = true;
            var rootApp = grammar.Root.Synthesize(rootElement, new ParsePosition(0, 0), parser.Context);
            var synthesis = grammar.Root.Synthesize(rootElement, parser.Context);

            parser.UnificateInitialize(rootApp, synthesis, targetUri);

            File.WriteAllText(targetUri.LocalPath, synthesis);
            _documents.Add(uri2, parser);

            ModelChangeHandler.SubscribeToModelChanges(rootElement, parser, this);
        }

        private void ProcessSync(Parser parser, bool isManual = false)
        {
            var lang = parser.Context.Grammar;
            _leftModelSyncs.TryGetValue(lang, out var leftSyncs);
            _rightModelSyncs.TryGetValue(lang, out var rightSyncs);
            foreach (var otherParser in _documents.Values)
            {
                if (otherParser == parser || !otherParser.Context.RootRuleApplication.IsPositive)
                    continue;
                var model = (IModelElement)parser.Context.Root;
                var otherModel = (IModelElement)otherParser.Context.Root;
                var otherLang = otherParser.Context.Grammar;

                foreach (var s in (leftSyncs ?? Enumerable.Empty<ModelSynchronization>())
                         .Where(s => s.RightLanguage == otherLang && (s.IsAutomatic || isManual)))
                    if (s.SynchronizationPredicate(parser.Context, otherParser.Context))
                    {
                        ModelChangeHandler.SubscribeToModelChanges(model, parser, this);
                        ModelChangeHandler.SubscribeToModelChanges(otherModel, otherParser, this);
                        s.Synchronize(model, otherModel);
                    }

                foreach (var s in (rightSyncs ?? Enumerable.Empty<ModelSynchronization>())
                         .Where(s => s.LeftLanguage == otherLang && (s.IsAutomatic || isManual)))
                    if (s.SynchronizationPredicate(otherParser.Context, parser.Context))
                    {
                        ModelChangeHandler.SubscribeToModelChanges(model, parser, this);
                        ModelChangeHandler.SubscribeToModelChanges(otherModel, otherParser, this);
                        s.Synchronize(otherModel, model);
                    }
            }
        }

        private CompletionItem ProcessSyncCompletion(Parser parser, string fileUri)
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
                        CommandIdentifier = SyncModelCommand,
                        Arguments = [fileUri],
                    },
                    TextEdit = new LspTypes.TextEdit
                    {
                        Range = new Range
                        {
                            Start = new Position(0, 0),
                            End = new Position(0, 0)
                        },
                        NewText = String.Empty
                    }
                };

            return null;
        }
    }
}