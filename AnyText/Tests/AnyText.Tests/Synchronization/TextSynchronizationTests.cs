using AnyText.Tests.Synchronization.Grammar;
using AnyText.Tests.Synchronization.Metamodel.StateMachine;
using NMF.AnyText;
using NMF.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyText.Tests.Synchronization
{
    [TestFixture]
    public class TextSynchronizationTests
    {
        private NMF.AnyText.Grammars.Grammar _grammar;
        private StateMachine _model;
        private State _stateA;
        private Transition _transition;
        private Parser _parser;

        [SetUp]
        public void Setup()
        {
            _grammar = new StateMachineGrammar();
            var sm = new StateMachine { Id = "StateMachine" };
            _stateA = new State { Name = "StateA", IsStartState = true, IsEndState = true };
            _transition = new Transition { StartState = _stateA, EndState = _stateA, Input = "test" };
            sm.States.Add(_stateA);
            sm.Transitions.Add(_transition);
            _model = sm;
            _parser = _grammar.CreateParser();
            _parser.Initialize(_model);
        }

        [Test]
        public void TextSynchronization_InitialTextCorrect()
        {
            Assert.That(_parser.Context.Input.Length, Is.AtLeast(3));
            Assert.That(_parser.Context.Input[0], Is.EqualTo("statemachine StateMachine:"));
            Assert.That(_parser.Context.Input[1], Is.EqualTo("  states:"));
            Assert.That(_parser.Context.Input[2], Is.EqualTo("    end start state StateA"));

            Assert.That(_parser.Context.TryGetDefinitions(_model, out var definitions));
            Assert.That(definitions.Count, Is.EqualTo(1));
            Assert.That(definitions.First().ContextElement, Is.EqualTo(_model));

            Assert.That(_parser.Context.TryGetDefinitions(_stateA, out definitions));
            Assert.That(definitions.Count, Is.EqualTo(1));
            Assert.That(definitions.First().ContextElement, Is.EqualTo(_stateA));

            Assert.That(_parser.Context.TryGetDefinitions(_transition, out definitions));
            Assert.That(definitions.Count, Is.EqualTo(1));
            Assert.That(definitions.First().ContextElement, Is.EqualTo(_transition));
        }

        [Test]
        public void TextSynchronization_AddState_CorrectUpdate()
        {
            var stateB = new State { Name = "StateB" };
            _model.States.Add(stateB);
            var edits = _parser.Update(_model);
            Assert.That(edits, Is.Not.Empty);
            Assert.That(edits.Count, Is.EqualTo(1));

            var edit = edits.First();
            Assert.That(edit.Start.Line, Is.EqualTo(3));
            Assert.That(edit.NewText.Length, Is.EqualTo(2));
            Assert.That(edit.NewText[0], Is.EqualTo("    state StateB"));
            Assert.That(edit.NewText[1], Is.EqualTo(""));

            Assert.That(_parser.Context.Input.Length, Is.AtLeast(4));
            Assert.That(_parser.Context.Input[0], Is.EqualTo("statemachine StateMachine:"));
            Assert.That(_parser.Context.Input[1], Is.EqualTo("  states:"));
            Assert.That(_parser.Context.Input[2], Is.EqualTo("    end start state StateA"));
            Assert.That(_parser.Context.Input[3], Is.EqualTo("    state StateB"));

            Assert.That(_parser.Context.TryGetDefinitions(_model, out var definitions));
            Assert.That(definitions.Count, Is.EqualTo(1));
            Assert.That(definitions.First().ContextElement, Is.EqualTo(_model));

            Assert.That(_parser.Context.TryGetDefinitions(_stateA, out definitions));
            Assert.That(definitions.Count, Is.EqualTo(1));
            Assert.That(definitions.First().ContextElement, Is.EqualTo(_stateA));

            Assert.That(_parser.Context.TryGetDefinitions(stateB, out definitions));
            Assert.That(definitions.Count, Is.EqualTo(1));
            Assert.That(definitions.First().ContextElement, Is.EqualTo(stateB));

            Assert.That(_parser.Context.TryGetDefinitions(_transition, out definitions));
            Assert.That(definitions.Count, Is.EqualTo(1));
            Assert.That(definitions.First().ContextElement, Is.EqualTo(_transition));
        }

        [Test]
        public void TextSynchronization_RemoveTransition_CorrectUpdate()
        {
            _model.Transitions.Remove(_transition);
            var edits = _parser.Update(_model);
            Assert.That(edits, Is.Not.Empty);
            Assert.That(edits.Count, Is.EqualTo(1));

            var edit = edits.First();
            Assert.That(edit.Start.Line, Is.EqualTo(3));
            Assert.That(edit.End.Line, Is.EqualTo(5));
            Assert.That(edit.NewText.Length, Is.EqualTo(1));
            Assert.That(edit.NewText[0], Is.EqualTo(""));

            Assert.That(_parser.Context.Input.Length, Is.AtLeast(3));
            Assert.That(_parser.Context.Input[0], Is.EqualTo("statemachine StateMachine:"));
            Assert.That(_parser.Context.Input[1], Is.EqualTo("  states:"));
            Assert.That(_parser.Context.Input[2], Is.EqualTo("    end start state StateA"));

            Assert.That(_parser.Context.TryGetDefinitions(_model, out var definitions));
            Assert.That(definitions.Count, Is.EqualTo(1));
            Assert.That(definitions.First().ContextElement, Is.EqualTo(_model));

            Assert.That(_parser.Context.TryGetDefinitions(_stateA, out definitions));
            Assert.That(definitions.Count, Is.EqualTo(1));
            Assert.That(definitions.First().ContextElement, Is.EqualTo(_stateA));

            Assert.That(_parser.Context.TryGetDefinitions(_transition, out _), Is.False);
        }

        [Test]
        public void TextSynchronization_RenameState_CorrectUpdate()
        {
            _stateA.Name = "StateAChanged";
            var edits = _parser.Update(_stateA);
            Assert.That(edits, Is.Not.Empty);
            Assert.That(edits.Count, Is.EqualTo(3));

            foreach (var edit in edits)
            {
                Assert.That(edit.NewText.Length, Is.EqualTo(1));
                Assert.That(edit.NewText[0], Is.EqualTo("Changed"));
            }

            Assert.That(_parser.Context.Input.Length, Is.AtLeast(3));
            Assert.That(_parser.Context.Input[0], Is.EqualTo("statemachine StateMachine:"));
            Assert.That(_parser.Context.Input[1], Is.EqualTo("  states:"));
            Assert.That(_parser.Context.Input[2], Is.EqualTo("    end start state StateAChanged"));

            Assert.That(_parser.Context.TryGetDefinitions(_model, out var definitions));
            Assert.That(definitions.Count, Is.EqualTo(1));
            Assert.That(definitions.First().ContextElement, Is.EqualTo(_model));

            Assert.That(_parser.Context.TryGetDefinitions(_stateA, out definitions));
            Assert.That(definitions.Count, Is.EqualTo(1));
            Assert.That(definitions.First().ContextElement, Is.EqualTo(_stateA));

            Assert.That(_parser.Context.TryGetDefinitions(_transition, out definitions));
            Assert.That(definitions.Count, Is.EqualTo(1));
            Assert.That(definitions.First().ContextElement, Is.EqualTo(_transition));
        }

        [Test]
        public void TextSynchronization_AddTransition_CorrectUpdates()
        {
            var t2 = new Transition { StartState = _stateA, Input = "t2", EndState = _stateA };
            _model.Transitions.Add(t2);
            var edits = _parser.Update(_model);
            Assert.That(edits, Is.Not.Empty);
            Assert.That(edits.Count, Is.EqualTo(1));

            Assert.That(_parser.Context.Input.Length, Is.AtLeast(3));
            Assert.That(_parser.Context.Input[0], Is.EqualTo("statemachine StateMachine:"));
            Assert.That(_parser.Context.Input[1], Is.EqualTo("  states:"));
            Assert.That(_parser.Context.Input[2], Is.EqualTo("    end start state StateA"));

            Assert.That(_parser.Context.TryGetDefinitions(_model, out var definitions));
            Assert.That(definitions.Count, Is.EqualTo(1));
            Assert.That(definitions.First().ContextElement, Is.EqualTo(_model));

            Assert.That(_parser.Context.TryGetDefinitions(_stateA, out definitions));
            Assert.That(definitions.Count, Is.EqualTo(1));
            Assert.That(definitions.First().ContextElement, Is.EqualTo(_stateA));

            Assert.That(_parser.Context.TryGetDefinitions(_transition, out definitions));
            Assert.That(definitions.Count, Is.EqualTo(1));
            Assert.That(definitions.First().ContextElement, Is.EqualTo(_transition));
        }
    }
}
