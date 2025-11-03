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
        private IModelElement _leftModel;
        private IModelElement _rightModel;
        private Parser _leftParser;
        private Parser _rightParser;

        [SetUp]
        public void Setup()
        {
            _leftGrammar = new StateMachineGrammar();
            _rightGrammar = new PetriNetGrammar();
            var sm = new StateMachine { Id = "StateMachine" };
            var stateA = new State { Name = "StateA", IsStartState = true, IsEndState = true };
            sm.States.Add(stateA);
            var pn = new PetriNet { Id = "PetriNet" };
            var place = new Place { Name = "PlaceA" };
            pn.Places.Add(place);
            _leftModel = sm;
            _rightModel = pn;
            _rightParser = _rightGrammar.CreateParser();
            _rightParser.Initialize(_rightModel);
            _leftParser = _leftGrammar.CreateParser();
            _leftParser.Initialize(_leftModel);
        }

        [Test]
        public void Setup_Correct()
        {
            Assert.That(_rightParser.Context.Input, Is.Not.Null);
            Assert.That(_leftParser.Context.Input, Is.Not.Null );
        }
    }
}