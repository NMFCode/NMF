using AnyText.Tests.Synchronization.Grammar;
using AnyText.Tests.Synchronization.Metamodel.PetriNet;
using AnyText.Tests.Synchronization.Metamodel.StateMachine;
using NMF.AnyText;
using NMF.Models.Services;
using NUnit.Framework;
using Transition = AnyText.Tests.Synchronization.Metamodel.StateMachine.Transition;

namespace AnyText.Tests.Synchronization
{
    [TestFixture]
    public class SynchonizationServiceTests
    {
        private string _tempDir;
        private SynchronizationService _service;
        private LspServer _lspServer;
        private Dictionary<string, Parser> _parsers;
        private Dictionary<string, NMF.AnyText.Grammars.Grammar> _grammars;

        [SetUp]
        public void Setup()
        {
            _tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(_tempDir);
            var rpc = AnyTextJsonRpcServerUtil.CreateServer(new MemoryStream());

            _lspServer = new LspServer();
            _lspServer.SetRpc(rpc);
            _service = new SynchronizationService(_lspServer, new ModelServer());

            _parsers = new Dictionary<string, Parser>();
            _grammars = new Dictionary<string, NMF.AnyText.Grammars.Grammar>
            {
                { "statemachine", new StateMachineGrammar() },
                { "petrinet", new PetriNetGrammar() }
            };
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(_tempDir)) Directory.Delete(_tempDir, true);
        }


        [Test]
        public void ProcessModelGeneration_NewFile_GeneratesAndWritesToDisk()
        {
            var sourcePath = Path.Combine(_tempDir, "source.statemachine");
            var targetPath = Path.Combine(_tempDir, "target.statemachine");

            var stateMachine = new StateMachine();
            stateMachine.Id = "MyTestMachine";
            stateMachine.States.Add(new State { Name = "Start", IsStartState = true });
            stateMachine.States.Add(new State { Name = "End", IsEndState = true });
            stateMachine.Transitions.Add(new Transition
                { Input = "t1", StartState = stateMachine.States.First(), EndState = stateMachine.States.Last() });

            var sourceParser = _grammars["statemachine"].CreateParser();
            sourceParser.Context.UsesSynthesizedModel = true;

            sourceParser.UnifyInitialize(
                _grammars["statemachine"].Root.Synthesize(stateMachine, default, sourceParser.Context),
                _grammars["statemachine"].Root.Synthesize(stateMachine, sourceParser.Context),
                null
            );
            _parsers.Add(sourcePath, sourceParser);

            _service.ProcessModelGeneration(sourcePath, targetPath, _parsers, _grammars);

            var generatedText = File.ReadAllText(targetPath);
            Assert.That(generatedText, Does.Contain("statemachine MyTestMachine"));
            Assert.That(generatedText, Does.Contain("state Start"));
            Assert.That(generatedText, Does.Contain("state End"));
            Assert.That(generatedText, Does.Contain("Start --( t1 )--> End"));
            Assert.That(File.Exists(targetPath), Is.True);
            Assert.That(_parsers.ContainsKey(targetPath), Is.True);
            Assert.That(_parsers[targetPath], Is.Not.Null);
        }

        [Test]
        public void ProcessModelGeneration_ExistingFiles_SynchronizesContent()
        {
            var sourcePath = Path.Combine(_tempDir, "source.statemachine");
            var targetPath = Path.Combine(_tempDir, "target.statemachine");

            var sourceMachine = new StateMachine { Id = "SourceMachine" };
            sourceMachine.States.Add(new State { Name = "S1", IsStartState = true });


            File.WriteAllText(targetPath, "statemachine TargetMachine: states: state T1");

            var sourceParser = _grammars["statemachine"].CreateParser();
            sourceParser.Context.UsesSynthesizedModel = true;
            var sourceInput = _grammars["statemachine"].Root.Synthesize(sourceMachine, sourceParser.Context);
            File.WriteAllText(sourcePath, sourceInput);
            sourceParser.UnifyInitialize(
                _grammars["statemachine"].Root.Synthesize(sourceMachine, default, sourceParser.Context),
                sourceInput,
                null
            );
            sourceParser.Context.UsesSynthesizedModel = false;
            _parsers.Add(sourcePath, sourceParser);

            var targetParser = _grammars["statemachine"].CreateParser();
            targetParser.Initialize(new Uri(targetPath));
            _parsers.Add(targetPath, targetParser);

            _service.ProcessModelGeneration(sourcePath, targetPath, _parsers, _grammars);

            var synchronizedText = File.ReadAllText(targetPath);
            Assert.That(synchronizedText, Does.Contain("statemachine SourceMachine"));
            Assert.That(synchronizedText, Does.Contain("start state S1"));
            Assert.That(synchronizedText, Does.Not.Contain("TargetMachine"));
            Assert.That(synchronizedText, Does.Not.Contain("state T1"));
        }

        private (Parser Source, Parser Target) SetupSyncTestParsers(string sourceContent, string targetContent,
            bool isAutomatic)
        {
            var sourcePath = Path.Combine(_tempDir, "source.statemachine");
            var targetPath = Path.Combine(_tempDir, "target.petrinet");

            File.WriteAllText(sourcePath, sourceContent);
            File.WriteAllText(targetPath, targetContent);

            var sourceParser = _grammars["statemachine"].CreateParser();
            sourceParser.Initialize(new Uri(sourcePath));
            _parsers[sourcePath] = sourceParser;

            var targetParser = _grammars["petrinet"].CreateParser();
            targetParser.Initialize(new Uri(targetPath));
            _parsers[targetPath] = targetParser;


            var sync = new ModelSynchronization<IStateMachine, IPetriNet, FSM2PN, FSM2PN.AutomataToNet>(
                _grammars["statemachine"],
                _grammars["petrinet"],
                isAutomatic: isAutomatic);

            _service.RegisterLeftModelSync(_grammars["statemachine"], sync);
            _service.RegisterRightModelSync(_grammars["petrinet"], sync);

            return (sourceParser, targetParser);
        }


        [Test]
        public async Task ProcessSync_AutomaticSync_IsTriggeredCorrectlyAsync()
        {
            var (sourceParser, targetParser) = SetupSyncTestParsers(
                "statemachine TestMachine: states: state A",
                "petrinet TestNet: places: place B",
                true
            );

            var initialSourceRuleApp = sourceParser.Context.RootRuleApplication;
            var initialStateRuleApps = initialSourceRuleApp.Children.ElementAt(3).Children.First().Children.Last()
                .Children.ToList();
            var initialTargetRuleApp = targetParser.Context.RootRuleApplication;
            var initialPlaceRuleApps = initialTargetRuleApp.Children.ElementAt(4).Children.First().Children.Last()
                .Children.ToList();

            Assert.That(initialStateRuleApps.Count(), Is.EqualTo(1));
            Assert.That(initialPlaceRuleApps.Count(), Is.EqualTo(1));


            _service.ProcessSync(sourceParser, _parsers.Values);
            await SynchronizationTests.WaitForSynchronizationAsync(sourceParser, _service);
            await SynchronizationTests.WaitForSynchronizationAsync(targetParser, _service);


            var finalSourceRuleApp = sourceParser.Context.RootRuleApplication;
            var finalStateRuleApps = finalSourceRuleApp.Children.ElementAt(3).Children.First().Children.Last().Children
                .ToList();
            var finalTargetRuleApp = targetParser.Context.RootRuleApplication;
            var finalPlaceRuleApps = finalTargetRuleApp.Children.ElementAt(4).Children.First().Children.Last().Children
                .ToList();

            Assert.That(finalStateRuleApps.Count(), Is.EqualTo(2));
            Assert.That(finalStateRuleApps.Last().GetLastInnerLiteral().Literal, Is.EqualTo("B"));
            Assert.That(finalPlaceRuleApps.Count(), Is.EqualTo(2));
            Assert.That(finalPlaceRuleApps.Last().GetLastInnerLiteral().Literal, Is.EqualTo("A"));
        }

        [Test]
        public async Task ProcessSync_ManualSync_IsNotTriggeredAutomaticallyAsync()
        {
            var (sourceParser, targetParser) = SetupSyncTestParsers(
                "statemachine TestMachine: states: state A",
                "petrinet TestNet: places: place B",
                false
            );

            var initialSourceRuleApp = sourceParser.Context.RootRuleApplication;
            var initialStateRuleApps =
                initialSourceRuleApp.Children.ElementAt(3).Children.First().Children.Last().Children;
            var initialTargetRuleApp = targetParser.Context.RootRuleApplication;
            var initialPlaceRuleApps =
                initialTargetRuleApp.Children.ElementAt(4).Children.First().Children.Last().Children;

            Assert.That(initialStateRuleApps.Count(), Is.EqualTo(1));
            Assert.That(initialPlaceRuleApps.Count(), Is.EqualTo(1));


            _service.ProcessSync(sourceParser, _parsers.Values);
            await SynchronizationTests.WaitForSynchronizationAsync(sourceParser, _service);
            await SynchronizationTests.WaitForSynchronizationAsync(targetParser, _service);


            var finalSourceRuleApp = sourceParser.Context.RootRuleApplication;
            var finalStateRuleApps = finalSourceRuleApp.Children.ElementAt(3).Children.First().Children.Last().Children
                .ToList();
            var finalTargetRuleApp = targetParser.Context.RootRuleApplication;
            var finalPlaceRuleApps = finalTargetRuleApp.Children.ElementAt(4).Children.First().Children.Last().Children
                .ToList();

            Assert.That(finalStateRuleApps.Count(), Is.EqualTo(1));
            Assert.That(finalStateRuleApps.Last().GetLastInnerLiteral().Literal, Is.EqualTo("A"));
            Assert.That(finalPlaceRuleApps.Count(), Is.EqualTo(1));
            Assert.That(finalPlaceRuleApps.Last().GetLastInnerLiteral().Literal, Is.EqualTo("B"));
        }

        [Test]
        public async Task ProcessSync_ManualSync_IsTriggeredWithManualFlagAsync()
        {
            var (sourceParser, targetParser) = SetupSyncTestParsers(
                "statemachine TestMachine: states: state A",
                "petrinet TestNet: places: place B",
                false
            );


            var initialSourceRuleApp = sourceParser.Context.RootRuleApplication;
            var initialStateRuleApps =
                initialSourceRuleApp.Children.ElementAt(3).Children.First().Children.Last().Children;
            var initialTargetRuleApp = targetParser.Context.RootRuleApplication;
            var initialPlaceRuleApps =
                initialTargetRuleApp.Children.ElementAt(4).Children.First().Children.Last().Children;

            Assert.That(initialStateRuleApps.Count(), Is.EqualTo(1));
            Assert.That(initialPlaceRuleApps.Count(), Is.EqualTo(1));

            _service.ProcessSync(sourceParser, _parsers.Values, true);
            await SynchronizationTests.WaitForSynchronizationAsync(sourceParser, _service);
            await SynchronizationTests.WaitForSynchronizationAsync(targetParser, _service);

            var finalSourceRuleApp = sourceParser.Context.RootRuleApplication;
            var finalStateRuleApps = finalSourceRuleApp.Children.ElementAt(3).Children.First().Children.Last().Children
                .ToList();
            var finalTargetRuleApp = targetParser.Context.RootRuleApplication;
            var finalPlaceRuleApps = finalTargetRuleApp.Children.ElementAt(4).Children.First().Children.Last().Children
                .ToList();

            Assert.That(finalStateRuleApps.Count(), Is.EqualTo(2));
            Assert.That(finalStateRuleApps.Last().GetLastInnerLiteral().Literal, Is.EqualTo("B"));
            Assert.That(finalPlaceRuleApps.Count(), Is.EqualTo(2));
            Assert.That(finalPlaceRuleApps.Last().GetLastInnerLiteral().Literal, Is.EqualTo("A"));
        }

        [Test]
        public void ProcessSyncCompletion_ManualSyncExists_ReturnsCompletionItem()
        {
            var filePath = Path.Combine(_tempDir, "test.statemachine");
            var parser = _grammars["statemachine"].CreateParser();

            _service.RegisterLeftModelSync(_grammars["statemachine"],
                new ModelSynchronization<IStateMachine, IPetriNet, FSM2PN, FSM2PN.AutomataToNet>(
                    _grammars["statemachine"],
                    _grammars["petrinet"],
                    isAutomatic: false
                ));

            var completionItem = _service.ProcessSyncCompletion(parser, filePath);

            Assert.That(completionItem.Label, Is.EqualTo("Synchronize Document"));
            Assert.That(completionItem.Command.CommandIdentifier, Is.EqualTo(LspServer.SyncModelCommand));
        }

        [Test]
        public void ProcessSyncCompletion_OnlyAutomaticSyncExists_ReturnsNull()
        {
            var filePath = Path.Combine(_tempDir, "test.statemachine");
            var parser = _grammars["statemachine"].CreateParser();

            _service.RegisterLeftModelSync(_grammars["statemachine"],
                new ModelSynchronization<IStateMachine, IPetriNet, FSM2PN, FSM2PN.AutomataToNet>(
                    _grammars["statemachine"],
                    _grammars["petrinet"],
                    isAutomatic: true
                ));

            var completionItem = _service.ProcessSyncCompletion(parser, filePath);

            Assert.That(completionItem, Is.Null);
        }
    }
}