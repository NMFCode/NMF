using System;
using System.IO;
using System.Linq;
using NMF.Models;
using NMF.Models.Repository;

namespace NMF.AnyText
{
    public partial class LspServer
    {
        private void ProcessModelSynchronization(string uri1, string uri2)
        {
            if (!_documents.TryGetValue(uri1, out var parser1)) return;
            if (!_documents.TryGetValue(uri2, out var parser2)) return;
            if(!parser1.Context.RootRuleApplication.IsPositive || !parser2.Context.RootRuleApplication.IsPositive) throw new Exception("TEst");
            var model1 = (IModelElement)parser1.Context.Root;
            var model2 = (IModelElement)parser2.Context.Root;
            parser1.Context.UsesSynthesizedModel = true;
            parser2.Context.UsesSynthesizedModel = true;
            ModelChangeHandler.SubscribeToModelChanges(model1, parser1, uri1, this);
            ModelChangeHandler.SubscribeToModelChanges(model2, parser2, uri2, this);

            if (!_modelSyncs.TryGetValue(parser1.Context.Grammar.LanguageId + parser2.Context.Grammar.LanguageId,
                    out var sync)) return;
            sync.Initialize();
            sync.Synchronize(model1, model2);
        }


        private void ProcessModelGeneration(string uri, string uri2, string lang)
        {
            if (!_languages.TryGetValue(lang, out var grammar))
            {
                Console.WriteLine($"Language not found: {lang}");
                return;
            }

            var sourcePath = new Uri(Uri.UnescapeDataString(uri));
            var targetPath = new Uri(Uri.UnescapeDataString(uri2));

            var repo = new ModelRepository();
            var rootElement = repo.Resolve(sourcePath.AbsolutePath).RootElements.First();

            var parser = grammar.CreateParser();
            parser.Context.UsesSynthesizedModel = true;
            var rootApp = grammar.Root.Synthesize(rootElement, new ParsePosition(0, 0), parser.Context);
            var synthesis = grammar.Root.Synthesize(rootElement, parser.Context);

            parser.UnificateInitialize(rootApp, synthesis);

            File.WriteAllText(targetPath.LocalPath, synthesis);
            _documents.Add(uri2, parser);

            ModelChangeHandler.SubscribeToModelChanges(rootElement, parser, uri2, this);
        }
    }
}