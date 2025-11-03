using AnyText.Tests.Synchronization.Grammar;
using AnyText.Tests.Synchronization.Metamodel.StateMachine;
using NMF.AnyText;
using NUnit.Framework;

namespace AnyText.Tests.Synchronization
{
    [TestFixture]
    public class StateMachineSynchronizationTests
    {
        private Parser _parser;
        private IStateMachine _stateMachine;


        [SetUp]
        public void Setup()
        {
            var grammar = new StateMachineGrammar();
            _parser = grammar.CreateParser();
            _stateMachine = CreateBasicStateMachine();
            _parser.Initialize(_stateMachine);
            _stateMachine.BubbledChange += (o, e) =>
            {
                if (e.ChangeType == NMF.Models.ChangeType.PropertyChanged || e.ChangeType == NMF.Models.ChangeType.CollectionChanged)
                {
                    _parser.Update(e.Element);
                }
            };
        }

        private IStateMachine CreateBasicStateMachine()
        {
            var sm = new StateMachine { Id = "MyStateMachine" };

            var stateA = new State { Name = "StateA", IsStartState = true };
            var stateB = new State { Name = "StateB" };
            var stateC = new State { Name = "StateC", IsEndState = true };
            var stateD = new State { Name = "StateD" };
            sm.States.Add(stateA);
            sm.States.Add(stateB);
            sm.States.Add(stateC);
            sm.States.Add(stateD);

            var transition1 = new Transition { Input = "ToB", StartState = stateA, EndState = stateB };
            var transition2 = new Transition { Input = "ToC", StartState = stateB, EndState = stateC };

            sm.Transitions.Add(transition1);
            sm.Transitions.Add(transition2);

            return sm;
        }

        [Test]
        public void TestDelete()
        {

            var stateToDelete = _stateMachine.States.First(s => s.Name == "StateC"); // Deleting StateC
            var originalStateCount = _stateMachine.States.Count;

            stateToDelete.Delete();

            var parserRootSm = (IStateMachine)_parser.Context.Root;
            Assert.That(parserRootSm.States.Count, Is.EqualTo(originalStateCount - 1));
            Assert.That(parserRootSm.States.Any(s => s.Name == stateToDelete.Name), Is.False);

            _parser.Context.TryGetDefinitions(stateToDelete, out var def);
            Assert.That(def, Is.Null.Or.Empty);

            Assert.That(parserRootSm, Is.EqualTo(_stateMachine));
            Assert.That(_parser.Context.Errors.Count(), Is.EqualTo(0));
        }

        [Test]
        public void TestChangeStateName()
        {

            var stateToModify = _stateMachine.States.First(); // StateA
            var oldName = stateToModify.Name;
            var newName = "ModifiedStateA";

            stateToModify.Name = newName;

            var parserRootSm = (IStateMachine)_parser.Context.Root;
            var modifiedStateInParser = parserRootSm.States.FirstOrDefault(s => s.Name == newName);
            Assert.That(modifiedStateInParser, Is.Not.Null);
            Assert.That(modifiedStateInParser, Is.SameAs(stateToModify));
            Assert.That(parserRootSm.States.Any(s => s.Name == oldName), Is.False);

            _parser.Context.TryGetDefinitions(stateToModify, out var defs);
            Assert.That(defs, Is.Not.Empty);
            var def = defs.Single();
            Assert.That(def.GetFirstInnerLiteral().Literal, Is.EqualTo(newName));
            Assert.That(def.ContextElement, Is.SameAs(stateToModify));


            Assert.That(parserRootSm, Is.EqualTo(_stateMachine));
            Assert.That(_parser.Context.Errors, Is.Empty);
        }

        [Test]
        public void TestChangeTransitionTarget()
        {

            var transitionToModify = _stateMachine.Transitions.First(t => t.Input == "ToB"); // Transition from StateA to StateB
            var oldTargetState = transitionToModify.EndState;
            var newTargetState = _stateMachine.States.First(s => s.Name == "StateA");
            transitionToModify.EndState = newTargetState;

            var parserRootSm = (IStateMachine)_parser.Context.Root;
            var modifiedTransitionInParser = parserRootSm.Transitions.First(t => t.Input == "ToB");
            Assert.That(modifiedTransitionInParser.EndState, Is.SameAs(newTargetState));


            Assert.That(parserRootSm, Is.EqualTo(_stateMachine));
            _parser.Context.TryGetReferences(newTargetState, out var refsA);
            Assert.That(refsA.Count, Is.EqualTo(3));
            _parser.Context.TryGetReferences(oldTargetState, out var refsB);
            Assert.That(refsB.Count, Is.EqualTo(2));

            Assert.That(_parser.Context.Errors, Is.Empty);
        }

        [Test]
        public void TestCreateState()
        {
            var originalStateCount = _stateMachine.States.Count;
            var newName = "NewStateD";

            var newState = new State { Name = newName, IsStartState = false, IsEndState = false };
            _stateMachine.States.Add(newState);

            var parserRootSm = (IStateMachine)_parser.Context.Root;
            Assert.That(parserRootSm.States.Count, Is.EqualTo(originalStateCount + 1));
            Assert.That(parserRootSm.States.Any(s => s.Name == newName), Is.True);
            Assert.That(parserRootSm.States.First(s => s.Name == newName), Is.SameAs(newState));

            _parser.Context.TryGetDefinitions(newState, out var defs);
            Assert.That(defs, Is.Not.Empty);
            var def = defs.Single();
            Assert.That(def.ContextElement, Is.SameAs(newState));


            Assert.That(parserRootSm, Is.EqualTo(_stateMachine));
            Assert.That(_parser.Context.Errors, Is.Empty);
        }

        [Test]
        public void TestCreateTransition()
        {
            var originalTransitionCount = _stateMachine.Transitions.Count;
            var sourceState = _stateMachine.States.First(s => s.Name == "StateA");
            var targetState = _stateMachine.States.First(s => s.Name == "StateC");
            var newTransitionInput = "AToC";

            var newTransition = new Transition
            { Input = newTransitionInput, StartState = sourceState, EndState = targetState };
            _stateMachine.Transitions.Add(newTransition);

            var parserRootSm = (IStateMachine)_parser.Context.Root;
            Assert.That(parserRootSm.Transitions.Count, Is.EqualTo(originalTransitionCount + 1));
            Assert.That(parserRootSm.Transitions.Any(t => t.Input == newTransitionInput), Is.True);
            Assert.That(parserRootSm.Transitions.First(t => t.Input == newTransitionInput), Is.SameAs(newTransition));

            _parser.Context.TryGetDefinitions(newTransition, out var defs);
            Assert.That(defs, Is.Not.Empty);
            var def = defs.Single();
            Assert.That(def.ContextElement, Is.SameAs(newTransition));


            Assert.That(parserRootSm, Is.EqualTo(_stateMachine));
            Assert.That(_parser.Context.Errors, Is.Empty);
        }

        [Test]
        public void TestReplaceState()
        {
            //Index of StateD
            var index = 3;
            var newState = new State { Name = "ReplacedState", IsStartState = false, IsEndState = false };
            var originalStateCount = _stateMachine.States.Count;

            var statesList = (IList<IState>)_stateMachine.States;
            statesList[index] = newState;

            var parserRootSm = (IStateMachine)_parser.Context.Root;
            Assert.That(_parser.Context.TryGetDefinitions(newState, out _), Is.True);
            Assert.That(_parser.Context.Input.Any(s => s.Contains("ReplacedState")), Is.True);
            Assert.That(_parser.Context.Input.Any(s => s.Contains("transition")), Is.True);
            Assert.That(parserRootSm.States.Count, Is.EqualTo(originalStateCount));
            Assert.That(parserRootSm.States.Any(s => s.Name == newState.Name), Is.True);
            Assert.That(_parser.Context.Errors, Is.Empty);
        }

        [Test]
        public void TestResetCollection()
        {
            _stateMachine.Transitions.Clear();

            var parserRootSm = (IStateMachine)_parser.Context.Root;
            Assert.That(_parser.Context.Input.Any(s => s.Contains("ToB")), Is.False);
            Assert.That(_parser.Context.Input.Any(s => s.Contains("ToC")), Is.False);
            Assert.That(_parser.Context.Input.Any(s => s.Contains("StateD")), Is.True);
            Assert.That(_parser.Context.Input.Any(s => s.Contains("transition")), Is.False);
            Assert.That(parserRootSm.Transitions, Is.Empty);
            Assert.That(_parser.Context.Errors, Is.Empty);
        }
    }
}