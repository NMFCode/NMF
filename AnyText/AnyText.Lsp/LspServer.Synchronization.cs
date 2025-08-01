using System;
using System.Collections.Generic;
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

        private void ProcessModelGeneration(string uri, string uri2, string lang)
        {
            if (!_languages.TryGetValue(lang, out var grammar))
            {
                Console.WriteLine($"Language not found: {lang}");
                return;
            }
            var sourceUri = new Uri(Uri.UnescapeDataString(uri));
            var targetUri = new Uri(Uri.UnescapeDataString(uri2));
            
         
        }

        private void ProcessSync(Parser parser, bool isManual = false)
        {
            var currentLang = parser.Context.Grammar;

            _leftModelSyncs.TryGetValue(currentLang, out var leftSyncs);
            _rightModelSyncs.TryGetValue(currentLang, out var rightSyncs);

            foreach (var otherParser in _documents.Values)
            {
                if (otherParser == parser || !otherParser.Context.RootRuleApplication.IsPositive)
                    continue;

                var otherLang = otherParser.Context.Grammar;

                foreach (var s in (leftSyncs ?? Enumerable.Empty<ModelSynchronization>())
                         .Where(s => s.RightLanguage == otherLang && (s.IsAutomatic || isManual)))
                {
                    s.TrySynchronize(parser, otherParser, this);
                }

                foreach (var s in (rightSyncs ?? Enumerable.Empty<ModelSynchronization>())
                         .Where(s => s.LeftLanguage == otherLang && (s.IsAutomatic || isManual)))
                {
                    s.TrySynchronize(otherParser, parser, this);
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