using System.Reflection;
using AnyText.Tests.Synchronization.Grammar;
using AnyText.Tests.Synchronization.Metamodel.PetriNet;
using AnyText.Tests.Synchronization.Metamodel.StateMachine;
using LspTypes;
using Newtonsoft.Json.Linq;
using NMF.AnyText;
using NMF.AnyText.AnyMeta;
using NMF.Models.Meta;
using NMF.Models.Services;
using NUnit.Framework;
using Range = LspTypes.Range;
using TextEdit = NMF.AnyText.TextEdit;

namespace AnyText.Tests.Synchronization
{
    [TestFixture]
    public class SynchronizationTests
    {
        private string _tempDir;
        private readonly StateMachineGrammar _stateMachineGrammar = new();
        private readonly PetriNetGrammar _petriNetGrammar = new();
        private readonly AnyMetaGrammar _anyMetaGrammar = new();
        private LspServer _lspServer;
        private ModelServer _modelServer;
        private string _anyMetaPath;
        private string _stateMachinePath;
        private string _petriNetPath;

        private readonly string _anyMetaText = @"namespace StateMachine ( statemachine ) = anytext:statemachine
{
    class StateMachine
    {
        id : nmeta.String [1]
        composite reference transitions : Transition [0..*]
        composite reference states : State [0..*]

    }
    class State
    {
        name : nmeta.String [1]
        isStartState : nmeta.Boolean [0..1]
        isEndState : nmeta.Boolean [0..1]
    }
    class Transition
    {
        input : nmeta.String [1]
        reference endState : State [0..1]
        reference startState : State [0..1]
    }
}";

        private readonly string _stateMachineText = @"statemachine TrafficLight:
 states:
     start state Red
     state Green
     end state Yellow
 transitions:
     Red --( timer1 )--> Green
     Green --( timer2 )--> Yellow
     Yellow --( timer3 )--> Red
";

        private readonly string _petriNetText = @"petrinet TrafficLight:
 transitions:
   transition:
     input timer1
     from[ Red ]
     to[ Green ]
 places:
   place Red
   place Green
   place Yellow
";

        [SetUp]
        public void Setup()
        {
            var rpc = AnyTextJsonRpcServerUtil.CreateServer(new MemoryStream());
            _tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(_tempDir);

            var sync = new ModelSynchronization<IStateMachine, IPetriNet, FSM2PN, FSM2PN.AutomataToNet>(
                _stateMachineGrammar, _petriNetGrammar, isAutomatic:false);
            _modelServer = new ModelServer();
            _lspServer = new LspServer([_stateMachineGrammar, _petriNetGrammar, _anyMetaGrammar], [sync], _modelServer);
            _lspServer.SetRpc(rpc);

            _stateMachinePath = Path.Combine(_tempDir, "trafficlight.statemachine");
            _petriNetPath = Path.Combine(_tempDir, "trafficlight.petrinet");
            _anyMetaPath = Path.Combine(_tempDir, "statemachine.anymeta");

            File.WriteAllText(_stateMachinePath, _stateMachineText);
            File.WriteAllText(_petriNetPath, _petriNetText);
            File.WriteAllText(_anyMetaPath, _anyMetaText);
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(_tempDir)) Directory.Delete(_tempDir, true);
        }

        [Test]
        public async Task Test_WhiteSpace_TextualChangeAsync()
        {
            OpenDocument(_stateMachinePath, "statemachine");
            OpenDocument(_petriNetPath, "petrinet");
            var smParser = GetParserForUri(new Uri(_stateMachinePath).ToString());
            if (smParser.Context.Root is not IStateMachine smModel)
            {
                Assert.Fail("Model is not of type IStateMachine.");
                return;
            }

            Assert.That(smModel.States.Count, Is.EqualTo(3));
            ExecuteSyncCommand(new Uri(_stateMachinePath).ToString());

            var newText =  Environment.NewLine+ "      ";
            var textEdit = new TextEdit(new ParsePosition(3, 16), new ParsePosition(3, 16), [newText]);
            ChangeDocument(_stateMachinePath, [textEdit]);

            await Task.Delay(100);
            var pnParser = GetParserForUri(new Uri(_petriNetPath).ToString());
            if (pnParser.Context.Root is not IPetriNet pnModel)
            {
                Assert.Fail("Model is not of type IPetriNet.");
                return;
            }
            Assert.That(pnModel.Places.Count, Is.EqualTo(3));
            Assert.That(smModel.States.Count, Is.EqualTo(3));
            Assert.That(pnParser.Context.ShouldParseChange, Is.False);
            Assert.That(smParser.Context.ShouldParseChange, Is.True);
        }
        
        [Test]
        public async Task Test_AddState_TextualChangeAsync()
        {
            OpenDocument(_stateMachinePath, "statemachine");
            OpenDocument(_petriNetPath, "petrinet");
            var smParser = GetParserForUri(new Uri(_stateMachinePath).ToString());
            if (smParser.Context.Root is not IStateMachine smModel)
            {
                Assert.Fail("Model is not of type IStateMachine.");
                return;
            }

            Assert.That(smModel.States.Count, Is.EqualTo(3));
            ExecuteSyncCommand(new Uri(_stateMachinePath).ToString());

            var newText =  " state NewState";
            var textEdit = new TextEdit(new ParsePosition(3, 16), new ParsePosition(3, 16), [newText]);
            ChangeDocument(_stateMachinePath, [textEdit]);

            await Task.Delay(100);
            var pnParser = GetParserForUri(new Uri(_petriNetPath).ToString());
            if (pnParser.Context.Root is not IPetriNet pnModel)
            {
                Assert.Fail("Model is not of type IPetriNet.");
                return;
            }

            Assert.That(smModel.States.Count, Is.EqualTo(4));
            Assert.That(pnModel.Places.Any(p => p.Name == "NewState"), Is.True);
            Assert.That(pnParser.Context.Input.Any(s => s.Contains("place NewState")), Is.True);
            Assert.That(pnParser.Context.ShouldParseChange, Is.False);
            Assert.That(smParser.Context.ShouldParseChange, Is.True);
            
            var newText2 =  "Changed";
            var textEdit2 = new TextEdit(new ParsePosition(3, 23), new ParsePosition(3, 31), [newText2]);
            ChangeDocument(_stateMachinePath, [textEdit2]);
            await Task.Delay(500);
            
            Assert.That(smParser.Context.Input.Any(s => s.Contains("state Green")), Is.True);

        }

        [Test]
        public async Task Test_DeleteState_TextualChangeAsync()
        {
            OpenDocument(_stateMachinePath, "statemachine");
            OpenDocument(_petriNetPath, "petrinet");
            var smParser = GetParserForUri(new Uri(_stateMachinePath).ToString());
            if (smParser.Context.Root is not IStateMachine smModel)
            {
                Assert.Fail("Model is not of type IStateMachine.");
                return;
            }

            Assert.That(smModel.States.Count, Is.EqualTo(3));

            ExecuteSyncCommand(new Uri(_stateMachinePath).ToString());

            var textEdit = new TextEdit(new ParsePosition(4, 0), new ParsePosition(4, 23), [""]);
            ChangeDocument(_stateMachinePath, [textEdit]);
            var pnParser = GetParserForUri(new Uri(_petriNetPath).ToString());
            if (pnParser.Context.Root is not IPetriNet pnModel)
            {
                Assert.Fail("Model is not of type IPetriNet.");
                return;
            }
            await Task.Delay(100);

            Assert.That(smModel.States.Count, Is.EqualTo(2));
            Assert.That(pnModel.Places.Any(p => p.Name == "Yellow"), Is.False);
            Assert.That(pnParser.Context.ShouldParseChange, Is.False);
            Assert.That(smParser.Context.ShouldParseChange, Is.True);
        }
        
        [Test]
        public async Task Test_DeleteTransition_TextualChangeAsync()
        {
            OpenDocument(_stateMachinePath, "statemachine");
            OpenDocument(_petriNetPath, "petrinet");
            var smParser = GetParserForUri(new Uri(_stateMachinePath).ToString());
            if (smParser.Context.Root is not IStateMachine smModel)
            {
                Assert.Fail("Model is not of type IStateMachine.");
                return;
            }

            Assert.That(smModel.States.Count, Is.EqualTo(3));

            ExecuteSyncCommand(new Uri(_stateMachinePath).ToString());
            await Task.Delay(200);

            var textEdit = new TextEdit(new ParsePosition(7, 0), new ParsePosition(7, 33), [""]);
            ChangeDocument(_stateMachinePath, [textEdit]);
            var pnParser = GetParserForUri(new Uri(_petriNetPath).ToString());
            if (pnParser.Context.Root is not IPetriNet pnModel)
            {
                Assert.Fail("Model is not of type IPetriNet.");
                return;
            }
            await Task.Delay(200);

            Assert.That(smModel.Transitions.Count, Is.EqualTo(2));
            Assert.That(pnModel.Transitions.Count, Is.EqualTo(3));
            Assert.That(pnParser.Context.ShouldParseChange, Is.False);
        }
        
        [Test]
        public async Task Test_RenameState_TextualChangeAsync()
        {
            OpenDocument(_stateMachinePath, "statemachine");
            OpenDocument(_petriNetPath, "petrinet");
            var smParser = GetParserForUri(new Uri(_stateMachinePath).ToString());
            if (smParser.Context.Root is not IStateMachine smModel)
            {
                Assert.Fail("Model is not of type IStateMachine.");
                return;
            }

            ExecuteSyncCommand(new Uri(_stateMachinePath).ToString());

            var newName = "Amber";
            var textEdit = new TextEdit(new ParsePosition(4, 15), new ParsePosition(4, 21), [newName]);
            ChangeDocument(_stateMachinePath, [textEdit]);


            var pnParser = GetParserForUri(new Uri(_petriNetPath).ToString());
            if (pnParser.Context.Root is not IPetriNet pnModel)
            {
                Assert.Fail("Model is not of type IPetriNet.");
                return;
            }
            await Task.Delay(100);

            Assert.That(smModel.States.Any(s => s.Name == newName), Is.True,
                "State not renamed in statemachine model.");
            Assert.That(pnModel.Places.Any(p => p.Name == newName), Is.True,
                "State rename not synchronized to PetriNet.");
            Assert.That(pnParser.Context.ShouldParseChange, Is.False);
            Assert.That(pnParser.Context.Input.Any(s => s.Equals("    place Amber")), Is.True);
            //Should have Updated References -> false
            Assert.That(smParser.Context.ShouldParseChange, Is.False);
        }

        [Test]
        public async Task Test_Rename_GLSPChangeAsync()
        {
            var uri = new Uri(Path.GetFullPath("Synchronization/StateMachine.nmeta"));

            await using (var stream = File.OpenRead("Synchronization/StateMachine.nmeta"))
            {
                _modelServer.Repository.Serializer.Deserialize(stream, uri, _modelServer.Repository, true);
            }

            OpenDocument(_anyMetaPath, "anymeta");
            var anyMetaUri = new Uri(_anyMetaPath);

            ExecuteCreateModelCommand(uri.AbsoluteUri, anyMetaUri.ToString(), "anymeta");


            var nsParser = GetParserForUri(anyMetaUri.ToString());
            if (nsParser.Context.Root is not INamespace nsModel)
            {
                Assert.Fail("Model is not of type IPetriNet.");
                return;
            }

            if (_modelServer.Repository.Models.TryGetValue(uri, out var repositoryModel))
            {
                var model = (INamespace)repositoryModel.Children.First();
                model.Name = "UpdatedName";
            }

            await Task.Delay(200);

            Assert.That(nsModel.Name, Is.EqualTo("UpdatedName"));
            Assert.That(nsParser.Context.Input.Any(s => s.Contains("namespace UpdatedName")), Is.True);
            Assert.That(nsParser.Context.ShouldParseChange, Is.False);

            if (repositoryModel != null)
            {
                var model = (Namespace)repositoryModel.Children.First();
                nsModel.Name = "LatestName";
                await Task.Delay(100);
                Assert.That(model.Name, Is.EqualTo("LatestName"));
            }
        }

        [Test]
        public async Task Test_Delete_GLSPChangeAsync()
        {
            var uri = new Uri(Path.GetFullPath("Synchronization/StateMachine.nmeta"));

            await using (var stream = File.OpenRead("Synchronization/StateMachine.nmeta"))
            {
                _modelServer.Repository.Serializer.Deserialize(stream, uri, _modelServer.Repository, true);
            }

            OpenDocument(_anyMetaPath, "anymeta");
            var anyMetaUri = new Uri(_anyMetaPath);
            ExecuteCreateModelCommand(uri.AbsoluteUri, anyMetaUri.ToString(), "anymeta");

            var nsParser = GetParserForUri(anyMetaUri.ToString());
            if (nsParser.Context.Root is not INamespace nsModel)
            {
                Assert.Fail("Model is not of type IPetriNet.");
                return;
            }

            if (_modelServer.Repository.Models.TryGetValue(uri, out var repositoryModel))
            {
                var model = (INamespace)repositoryModel.Children.First();
                var toDelete = model.Children.Last();
                toDelete.Delete();
                Assert.That(model.Children.Any(c => c.IdentifierString == "State"), Is.True);
            }

            await Task.Delay(100);

            Assert.That(nsModel.Children.Count(), Is.EqualTo(2));
            Assert.That(nsParser.Context.Input.Any(s => s.Contains("class Transition")), Is.False);
            Assert.That(nsParser.Context.ShouldParseChange, Is.False);


            if (repositoryModel != null)
            {
                var model = (Namespace)repositoryModel.Children.First();
                var toDelete2 = nsModel.Children.Last();
                toDelete2.Delete();

                await Task.Delay(100);

                Assert.That(model.Children.Count(), Is.EqualTo(1));
                Assert.That(model.Children.Any(c => c.IdentifierString == "State"), Is.False);
            }
        }

        [Test]
        public async Task Test_Add_GLSPChangeAsync()
        {
            var uri = new Uri(Path.GetFullPath("Synchronization/StateMachine.nmeta"));

            using (var stream = File.OpenRead("Synchronization/StateMachine.nmeta"))
            {
                _modelServer.Repository.Serializer.Deserialize(stream, uri, _modelServer.Repository, true);
            }

            OpenDocument(_anyMetaPath, "anymeta");
            var anyMetaUri = new Uri(_anyMetaPath);
            ExecuteCreateModelCommand(uri.AbsoluteUri, anyMetaUri.ToString(), "anymeta");

            var nsParser = GetParserForUri(anyMetaUri.ToString());
            if (nsParser.Context.Root is not INamespace nsModel)
            {
                Assert.Fail("Model is not of type IPetriNet.");
                return;
            }

            if (_modelServer.Repository.Models.TryGetValue(uri, out var repositoryModel))
            {
                var model = (Namespace)repositoryModel.Children.First();
                _ = new Class { Name = "NewClass", Namespace = model };
            }

            await Task.Delay(100);

            Assert.That(nsModel.Children.Count(), Is.EqualTo(4));
            Assert.That(nsParser.Context.Input.Any(s => s.Contains("class NewClass")), Is.True);
            Assert.That(nsParser.Context.ShouldParseChange, Is.False);

            if (repositoryModel != null)
            {
                var model = (Namespace)repositoryModel.Children.First();
                _ = new Class { Name = "NewClass2", Namespace = nsModel };

                await Task.Delay(100);

                Assert.That(model.Children.Count(), Is.EqualTo(5));
                Assert.That(model.Children.Any(c => c.IdentifierString == "NewClass2"), Is.True);
            }
        }

        private void OpenDocument(string path, string languageId)
        {
            var openParams = new DidOpenTextDocumentParams
            {
                TextDocument = new TextDocumentItem
                {
                    Uri = new Uri(path).ToString(),
                    LanguageId = languageId,
                    Text = File.ReadAllText(path),
                    Version = 1
                }
            };
            _lspServer.DidOpen(JToken.FromObject(openParams));
        }

        private Parser GetParserForUri(string uri)
        {
            var documentsField = typeof(LspServer)
                .GetField("_documents", BindingFlags.NonPublic | BindingFlags.Instance);

            if (documentsField?.GetValue(_lspServer) is Dictionary<string, Parser> documents &&
                documents.TryGetValue(uri, out var parser))
                return parser;

            return null!;
        }

        private void ChangeDocument(string path, TextEdit[] changes)
        {
            var changeParams = new DidChangeTextDocumentParams
            {
                TextDocument = new VersionedTextDocumentIdentifier
                {
                    Uri = new Uri(path).ToString(),
                    Version = 2
                },
                ContentChanges = changes.Select(c => new TextDocumentContentChangeEvent
                {
                    Range = new Range
                    {
                        Start = new Position((uint)c.Start.Line, (uint)c.Start.Col),
                        End = new Position((uint)c.End.Line, (uint)c.End.Col)
                    },
                    Text = string.Join(Environment.NewLine, c.NewText)
                }).ToArray()
            };
            _lspServer.DidChange(JToken.FromObject(changeParams));
        }

        private void ExecuteCommand(string command, params object[] args)
        {
            var executeCommandParams = new ExecuteCommandParams
            {
                Command = command,
                Arguments = args
            };
            _lspServer.ExecuteCommand(JToken.FromObject(executeCommandParams));
        }

        private void ExecuteSyncCommand(string uri)
        {
            ExecuteCommand(LspServer.SyncModelCommand, uri);
        }

        private void ExecuteCreateModelCommand(string uri, string uri2, string lang)
        {
            ExecuteCommand("anytext.createModelSync", uri, uri2, lang);
        }
    }
}