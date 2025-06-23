using System.Collections.Specialized;
using NMF.AnyText;
using NMF.AnyText.AnyMeta;
using NMF.Expressions;
using NMF.Models;
using NMF.Models.Meta;
using NMF.Models.Repository;
using NUnit.Framework;
using Attribute = NMF.Models.Meta.Attribute;
using ChangeType = NMF.Models.ChangeType;


namespace AnyText.Tests
{
    [TestFixture]
    public class SyncTest
    {
        private (IModelElement, Parser) SetupNamespace()
        {
            var fileName = "schema";
            var grammar = new AnyMetaGrammar();
            var parser = grammar.CreateParser();
            parser.Context.UsesSynthesizedModel = true;
            var repo = new ModelRepository();

            var ns = repo.Resolve($"{fileName}.nmeta").RootElements.First();
            var rootApp = grammar.Root.Synthesize(ns, new ParsePosition(0, 0), parser.Context);
            var synthesis = grammar.Root.Synthesize(ns, parser.Context);
            parser.Unificate(rootApp, synthesis);
            
            ns.BubbledChange += (sender, e) =>
            {
                NMF.AnyText.ChangeType changeType;
                var context = parser.Context;
                TextEdit[] edits = null;

                switch (e.ChangeType)
                {
                    case ChangeType.PropertyChanged:
                        var propertyChangedArgs = (ValueChangedEventArgs)e.OriginalEventArgs;
                        changeType = propertyChangedArgs.OldValue is null
                            ? NMF.AnyText.ChangeType.ElementAdded
                            : NMF.AnyText.ChangeType.PropertyChanged;
                        break;

                    case ChangeType.CollectionChanged:
                        var collectionChangedArgs = (NotifyCollectionChangedEventArgs)e.OriginalEventArgs;
                        if (collectionChangedArgs.Action != NotifyCollectionChangedAction.Remove)
                            return;

                        changeType = NMF.AnyText.ChangeType.ElementDeleted;
                        var elementToDelete = collectionChangedArgs.OldItems?[0];

                        if (context.TryGetDefinition(e.Element, out var currentDef) &&
                            context.TryGetDefinition(elementToDelete, out var deletedDef))
                        {
                            var start = deletedDef.Parent.CurrentPosition;
                            var end = start + deletedDef.Parent.Length;
                            edits = [new TextEdit(start, end, [])];
                            parser.Unificate(
                                currentDef.Parent,
                                edits,
                                toDelete: deletedDef.Parent,
                                changeType: changeType
                            );
                        }
                        break;

                    default:
                        return;
                }
                
                if (changeType == NMF.AnyText.ChangeType.ElementAdded)
                {
                    var parent = e.Element.Parent;
                    parser.Context.TryGetDefinition(parent, out var parentRuleDefinition);
                    var parentRule = parentRuleDefinition.Parent.Rule;
                    var syn = parentRule.Synthesize(parent, default, parser.Context);
                    var input = parentRule.Synthesize(parent, parser.Context);
                    
                    var start = parentRuleDefinition.Parent.CurrentPosition;
                    var end = start + parentRuleDefinition.Parent.Length;
                    var inputLines = input.Split(Environment.NewLine);
                    edits = [new TextEdit(start, end, inputLines)];

                    
                    parser.Unificate(syn, edits, changeType:changeType);
                    
                }
                else if(changeType == NMF.AnyText.ChangeType.PropertyChanged)
                {
                    parser.Context.TryGetDefinition(e.Element, out var definition);
                    var elementApp = definition.Parent;
                    var rule = elementApp.Rule;

                    var synthesized = rule.Synthesize(e.Element, new ParsePosition(0, 0), context);
                    var input = rule.Synthesize(e.Element, context);

                    var start = elementApp.CurrentPosition;
                    var end = start + elementApp.Length;
                    var inputLines = input.Split(Environment.NewLine);
                    edits = CreateTextEditsInRange(inputLines, context.Input, start, end).ToArray();  

                    parser.Unificate(synthesized, edits, changeType: changeType);
                }
                
                var wEdit = context.TrackAndCreateWorkspaceEdit(edits, changeType, "");                
           
            };

            return (ns, parser);
        }

        [Test]
        public void TestDelete()
        {
            var (ns, parser) = SetupNamespace();


            var classToDelete = (IClass)ns.Children.Last();
            var originalCount = ns.Children.Count();
            classToDelete.Delete();
        }

        [Test]
        public void TestChange()
        {
            var (ns, parser) = SetupNamespace();


            var classToModify = ns.Children.OfType<IClass>().Last();
            var oldName = classToModify.Name;
            classToModify.Name = "ModifiedClass";
            classToModify.Name = "SecondModifiedClass";
        }

        [Test]
        public void TestCreate()
        {
            var (ns, parser) = SetupNamespace();

            var newClass = new Class
            {
                Name = "NewClass",
                Parent = ns
            };

            // newClass.Name ="ChangedClass";
        }
        [Test]
        public void TestCreate2()
        {
            var (me, parser) = SetupNamespace();
            INamespace ns = (INamespace) me;
            var firstChild = ns.ChildNamespaces.Last();
            var y = (IClass) ns.Children.Last();
            var s = y.Attributes;
            var newClass = new Class
            {
                Name = "NewClass",
                Parent = firstChild,
                IsAbstract = true,
                
            };

            // newClass.Name ="ChangedClass";
        }
      private List<TextEdit> CreateTextEditsInRange(
          string[] input,
          string[] contextInput,
          ParsePosition start,
          ParsePosition end)
      {
          var edits = new List<TextEdit>();

          if (start.Line < 0 || start.Line >= contextInput.Length)
              throw new ArgumentOutOfRangeException(nameof(start.Line));
          if (end.Line < start.Line || end.Line >= contextInput.Length)
              throw new ArgumentOutOfRangeException(nameof(end.Line));

          int inputLineCount = input.Length;
          int maxLines = Math.Min(inputLineCount, end.Line - start.Line + 1);

          for (int i = 0; i < maxLines; i++)
          {
              var inputLine = input[i];
              var contextLine = contextInput[start.Line + i];

              if (!string.Equals(inputLine.Trim(), contextLine.Trim(), StringComparison.Ordinal))
              {
                  int lineIndex = start.Line + i;
                    
                  int leadingSpaces = contextLine.TakeWhile(char.IsWhiteSpace).Count();

                 
                  int startCol = Math.Min(leadingSpaces, contextLine.Length);
                  int endCol = contextLine.Length;
                    
                  edits.Add(new TextEdit(
                      new ParsePosition(lineIndex, startCol),
                      new ParsePosition(lineIndex, endCol),
                      new[] { inputLine }
                  ));
              }
          }

          return edits;
      }
    }
}