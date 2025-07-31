using System.Collections.Specialized;
using AnyText.Tests.Synchronization.Grammar;
using AnyText.Tests.Synchronization.Metamodel.StateMachine;
using NMF.AnyText;
using NMF.AnyText.AnyMeta;
using NMF.Expressions;
using NMF.Models;
using NMF.Models.Meta;
using NMF.Models.Repository;
using NUnit.Framework;
using Attribute = NMF.Models.Meta.Attribute;
using ChangeType = NMF.Models.ChangeType;


namespace AnyText.Tests.Synchronization
{
    
    [TestFixture]
    public class UnificateTests
    {
        private string _tempFilePath;
        private StateMachineGrammar _grammar;

     
        [TearDown]
        public void TearDown()
        {
            if (File.Exists(_tempFilePath))
            {
                File.Delete(_tempFilePath);
            }
        }

        private IStateMachine CreateBasicStateMachine()
        {
            var sm = new StateMachine { Id = "MyStateMachine" };

            var stateA = new State { Name = "StateA", IsStartState = true };
            var stateB = new State { Name = "StateB" };
            var stateC = new State { Name = "StateC", IsEndState = true };

            sm.States.Add(stateA);
            sm.States.Add(stateB);
            sm.States.Add(stateC);

            var transition1 = new Transition { Input= "ToB", StartState = stateA, EndState = stateB };
            var transition2 = new Transition { Input= "ToC", StartState= stateB, EndState = stateC };

            sm.Transitions.Add(transition1);
            sm.Transitions.Add(transition2);

            return sm;
        }
        
        [SetUp]
        public void Setup()
        {
            _grammar = new StateMachineGrammar();
            _tempFilePath = Path.Join(Path.GetTempPath(), Guid.NewGuid().ToString());
        }

        private (IStateMachine RootStateMachine, Parser Parser) SetupStateMachineParser()
        {
            var parser = _grammar.CreateParser();
            parser.Context.UsesSynthesizedModel = true;
            var sm = CreateBasicStateMachine();
            var synthesis = _grammar.Root.Synthesize(sm, parser.Context);

            File.WriteAllText(_tempFilePath, synthesis);


            var synthesizedRootAppForInit = _grammar.Root.Synthesize(sm, new ParsePosition(0, 0), parser.Context);
            parser.UnificateInitialize(synthesizedRootAppForInit, synthesis, new Uri(_tempFilePath));


            ModelChangeHandler.SubscribeToModelChanges(sm, parser, null);
            return (sm, parser);
        }

        [Test]
        public async Task TestDeleteAsync()
        {
            var (sm, parser) = SetupStateMachineParser();

            var stateToDelete = sm.States.Last(); // Deleting StateC
            var originalStateCount = sm.States.Count;
            
            stateToDelete.Delete();
            await Task.Delay(50);

            IStateMachine parserRootSm = (IStateMachine)parser.Context.Root;
            Assert.That(parserRootSm.States.Count, Is.EqualTo(originalStateCount - 1));
            Assert.That(parserRootSm.States.Any(s => s.Name == stateToDelete.Name), Is.False);

            parser.Context.TryGetDefinition(stateToDelete, out var def);
            Assert.That(def, Is.Null);

            Assert.That(parserRootSm, Is.EqualTo(sm));

            Assert.That(parser.Context.Errors.Count(), Is.EqualTo(1)); //Transition loses Reference

        }

        [Test]
        public async Task TestChangeStateNameAsync()
        {
            var (sm, parser) = SetupStateMachineParser();

            var stateToModify = sm.States.First(); // StateA
            var oldName = stateToModify.Name;
            var newName = "ModifiedStateA";

            stateToModify.Name = newName;
            await Task.Delay(50);

            IStateMachine parserRootSm = (IStateMachine)parser.Context.Root;
            var modifiedStateInParser = parserRootSm.States.FirstOrDefault(s => s.Name == newName);
            Assert.That(modifiedStateInParser, Is.Not.Null);
            Assert.That(modifiedStateInParser, Is.SameAs(stateToModify));
            Assert.That(parserRootSm.States.Any(s => s.Name == oldName), Is.False);

            parser.Context.TryGetDefinition(stateToModify, out var def);
            Assert.That(def, Is.Not.Null);
            Assert.That(def.GetFirstInnerLiteral().Literal, Is.EqualTo(newName));
            Assert.That(def.ContextElement, Is.SameAs(stateToModify));


            Assert.That(parserRootSm, Is.EqualTo(sm));
            Assert.That(parser.Context.Errors, Is.Empty);
        }

        [Test]
        public async Task TestChangeTransitionTargetAsync()
        {
            var (sm, parser) = SetupStateMachineParser();

            var transitionToModify = sm.Transitions.First(t => t.Input == "ToB"); // Transition from StateA to StateB
            var oldTargetState = transitionToModify.EndState;
            var newTargetState = sm.States.First(s => s.Name == "StateA");
            transitionToModify.EndState = newTargetState;
            await Task.Delay(50);

            IStateMachine parserRootSm = (IStateMachine)parser.Context.Root;
            var modifiedTransitionInParser = parserRootSm.Transitions.First(t => t.Input == "ToB");
            Assert.That(modifiedTransitionInParser.EndState, Is.SameAs(newTargetState));
            Assert.That(modifiedTransitionInParser.EndState, Is.Not.SameAs(oldTargetState));


            Assert.That(parserRootSm, Is.EqualTo(sm));
            Assert.That(parser.Context.Errors, Is.Empty);
        }

        [Test]
        public async Task TestCreateStateAsync()
        {
            var (sm, parser) = SetupStateMachineParser();

            var originalStateCount = sm.States.Count;
            var newName = "NewStateD";

            var newState = new State { Name = newName, IsStartState = false, IsEndState = false };
            sm.States.Add(newState);
            await Task.Delay(50);

            IStateMachine parserRootSm = (IStateMachine)parser.Context.Root;
            Assert.That(parserRootSm.States.Count, Is.EqualTo(originalStateCount + 1));
            Assert.That(parserRootSm.States.Any(s => s.Name == newName), Is.True);
            Assert.That(parserRootSm.States.First(s => s.Name == newName), Is.SameAs(newState));

            parser.Context.TryGetDefinition(newState, out var def);
            Assert.That(def, Is.Not.Null);
            Assert.That(def.ContextElement, Is.SameAs(newState));


            Assert.That(parserRootSm, Is.EqualTo(sm));
            Assert.That(parser.Context.Errors, Is.Empty);
        }

        [Test]
        public async Task TestCreateTransitionAsync()
        {
            var (sm, parser) = SetupStateMachineParser();

            var originalTransitionCount = sm.Transitions.Count;
            var sourceState = sm.States.First(s => s.Name == "StateA");
            var targetState = sm.States.First(s => s.Name == "StateC");
            var newTransitionInput = "AToC";

            var newTransition = new Transition { Input = newTransitionInput, StartState = sourceState, EndState = targetState };
            sm.Transitions.Add(newTransition);
            await Task.Delay(50);

            IStateMachine parserRootSm = (IStateMachine)parser.Context.Root;
            Assert.That(parserRootSm.Transitions.Count, Is.EqualTo(originalTransitionCount + 1));
            Assert.That(parserRootSm.Transitions.Any(t => t.Input == newTransitionInput), Is.True);
            Assert.That(parserRootSm.Transitions.First(t => t.Input == newTransitionInput), Is.SameAs(newTransition));

            parser.Context.TryGetDefinition(newTransition, out var def);
            Assert.That(def, Is.Not.Null);
            Assert.That(def.ContextElement, Is.SameAs(newTransition));


            Assert.That(parserRootSm, Is.EqualTo(sm));
            Assert.That(parser.Context.Errors, Is.Empty);
        }
    }
}