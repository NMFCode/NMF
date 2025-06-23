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

            var model1 = (IModelElement)parser1.Context.Root;
            var model2 = (IModelElement)parser2.Context.Root;

            ModelChangeHandler.SubscribeToModelChanges(model1, parser1, uri1, this);
            ModelChangeHandler.SubscribeToModelChanges(model2, parser2, uri2, this);

            var sync = _modelSyncs.Values.First();
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