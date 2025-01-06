using NMF.Models;
using NMF.Models.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Grammars
{
    /// <summary>
    /// Denotes a serializer using AnyText
    /// </summary>
    public class AnyTextSerializer : IModelSerializer
    {
        private readonly Grammar _grammar;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="grammar">the underlying grammar</param>
        public AnyTextSerializer(Grammar grammar)
        {
            _grammar = grammar;
        }

        /// <inheritdoc />
        public Models.Model Deserialize(Stream source, Uri modelUri, IModelRepository repository, bool addToRepository)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(modelUri);

            using (var reader = new StreamReader(source))
            {
                var lines = new List<string>();
                var line = reader.ReadLine();
                while (line != null)
                {
                    lines.Add(line);
                    line = reader.ReadLine();
                }
                var parser = _grammar.CreateParser();
                var result = parser.Initialize(lines.ToArray());
                if (parser.Context.Errors.Count > 0)
                {
                    throw new ArgumentException("Deserialization failed: " + string.Join(", ", parser.Context.Errors));
                }
                if (result is IModelElement element)
                {
                    var model = new Models.Model
                    {
                        RootElements = { element },
                        ModelUri = modelUri
                    };
                    if (addToRepository)
                    {
                        repository.Models.Add(modelUri, model);
                    }
                    return model;
                }
                throw new ArgumentException($"The stream contained a {result} instead of a model element.");
            }
        }

        /// <inheritdoc />
        public void Serialize(Models.Model model, Stream target)
        {
            ArgumentNullException.ThrowIfNull(model);
            ArgumentNullException.ThrowIfNull(target);

            if (model.RootElements.Count != 1) throw new ArgumentException("Model must contain exactly one element", nameof(model));

            SerializeFragmentCore(model.RootElements[0], target);
        }

        /// <inheritdoc />
        public void SerializeFragment(ModelElement element, Stream target)
        {
            ArgumentNullException.ThrowIfNull(element);
            ArgumentNullException.ThrowIfNull(target);

            SerializeFragmentCore(element, target);
        }

        private void SerializeFragmentCore(IModelElement element, Stream target)
        {
            var type = element.GetType();
            var modelRuleType = typeof(Model.ModelElementRule<>).MakeGenericType(type);
            var rule = _grammar.Rules.FirstOrDefault(modelRuleType.IsInstanceOfType)
                ?? throw new InvalidOperationException($"No rule to serialize elements of type {type.Name}");
            var parser = _grammar.CreateParser();

            using (var writer = new StreamWriter(target))
            {
                rule.Synthesize(element, parser.Context, writer);
            }
        }
    }
}
