using AnyText.Tests.Synchronization.Grammar;
using AnyText.Tests.Synchronization.Metamodel.PetriNet;
using AnyText.Tests.Synchronization.Metamodel.StateMachine;
using NMF.AnyText;
using NMF.Models;
using NMF.Models.Services;
using NMF.Synchronizations;
using NMF.Transformations;
using NUnit.Framework;

namespace AnyText.Tests.Synchronization
{
    [TestFixture]
    public class ModelSynchronizationTests
    {
        private NMF.AnyText.Grammars.Grammar _leftGrammar;
        private NMF.AnyText.Grammars.Grammar _rightGrammar;
        private SynchronizationService _service;
        private IModelElement _leftModel;
        private IModelElement _rightModel;
        private Parser _leftParser;
        private Parser _rightParser;

        [SetUp]
        public void Setup()
        {
            _leftGrammar = new StateMachineGrammar();
            _rightGrammar = new PetriNetGrammar();
            _service = new SynchronizationService(null, new ModelServer());
            var sm = new StateMachine { Id = "StateMachine" };
            var stateA = new State { Name = "StateA", IsStartState = true, IsEndState = true };
            sm.States.Add(stateA);
            var pn = new PetriNet { Id = "PetriNet" };
            var place = new Place { Name = "PlaceA" };
            pn.Places.Add(place);
            _leftModel = sm;
            _rightModel = pn;
            _rightParser = _rightGrammar.CreateParser();
            _rightParser.Context.UsesSynthesizedModel = true;

            _rightParser.UnifyInitialize(_rightGrammar.Root.Synthesize(_rightModel, default, _rightParser.Context),
                _rightGrammar.Root.Synthesize(_rightModel, _rightParser.Context),
                null
            );
            _leftParser = _leftGrammar.CreateParser();
            _leftParser.Context.UsesSynthesizedModel = true;
            _leftParser.UnifyInitialize(_leftGrammar.Root.Synthesize(_leftModel, default, _leftParser.Context),
                _leftGrammar.Root.Synthesize(_leftModel, _leftParser.Context),
                null
            );
        }

        [Test]
        public void ModelSynchronization_Constructor_SetsPropertiesCorrectly()
        {
            var customPredicate = (Func<ParseContext, ParseContext, bool>)((_, _) => false);

            var sync = new ModelSynchronization<IStateMachine, IPetriNet, FSM2PN, FSM2PN.AutomataToNet>(
                _leftGrammar,
                _rightGrammar,
                SynchronizationDirection.LeftWins, ChangePropagationMode.None, customPredicate, false);

            Assert.That(sync.LeftLanguage, Is.EqualTo(_leftGrammar));
            Assert.That(sync.RightLanguage, Is.EqualTo(_rightGrammar));
            Assert.That(sync.IsAutomatic, Is.False);
        }

        [Test]
        public void TrySynchronize_WithParsersAndModels_InitializesAndSynchronizes()
        {
            var sync = new ModelSynchronization<IStateMachine, IPetriNet, FSM2PN, FSM2PN.AutomataToNet>(
                _leftGrammar, _rightGrammar);

            sync.TrySynchronize(_leftModel, _rightModel, _leftParser, _rightParser, _service);

            Assert.That(_leftModel.IdentifierString, Is.EqualTo("StateMachine"));
            Assert.That(_rightModel.IdentifierString, Is.EqualTo("StateMachine"));
        }

        [Test]
        public void TrySynchronize_WithOnlyParsers_ExtractsModelsAndSynchronizes()
        {
            var sync = new ModelSynchronization<IStateMachine, IPetriNet, FSM2PN, FSM2PN.AutomataToNet>(
                _leftGrammar, _rightGrammar, SynchronizationDirection.RightWins);

            sync.TrySynchronize(_leftParser, _rightParser, _service);

            Assert.That(_leftModel.IdentifierString, Is.EqualTo("PetriNet"));
            Assert.That(_rightModel.IdentifierString, Is.EqualTo("PetriNet"));
        }

        [Test]
        public void TrySynchronize_OneWay_ExtractsModelsAndSynchronizes()
        {
            var sync = new ModelSynchronization<IStateMachine, IPetriNet, FSM2PN, FSM2PN.AutomataToNet>(
                _leftGrammar, _rightGrammar, SynchronizationDirection.LeftWins, ChangePropagationMode.OneWay);

            sync.TrySynchronize(_leftParser, _rightParser, _service);

            Assert.That(_leftModel.IdentifierString, Is.EqualTo("StateMachine"));
            Assert.That(_rightModel.IdentifierString, Is.EqualTo("StateMachine"));

            var pn = (PetriNet)_rightModel;
            pn.Id = "PetriNet";

            Assert.That(_leftModel.IdentifierString, Is.EqualTo("StateMachine"));
            Assert.That(_rightModel.IdentifierString, Is.EqualTo("PetriNet"));

            var sm = (StateMachine)_leftModel;
            sm.Id = "UpdatedStateMachine";
            Assert.That(_leftModel.IdentifierString, Is.EqualTo("UpdatedStateMachine"));
            Assert.That(_rightModel.IdentifierString, Is.EqualTo("UpdatedStateMachine"));
        }

        [Test]
        public void TrySynchronize_PredicateReturnsFalse_DoesNotSynchronize()
        {
            var sync = new ModelSynchronization<IStateMachine, IPetriNet, FSM2PN, FSM2PN.AutomataToNet>(_leftGrammar,
                _rightGrammar, predicate: (_, _) => false);

            sync.TrySynchronize(_leftModel, _rightModel, _leftParser, _rightParser, _service);

            Assert.That(_leftModel.IdentifierString, Is.EqualTo("StateMachine"));
            Assert.That(_rightModel.IdentifierString, Is.EqualTo("PetriNet"));
        }
    }
}