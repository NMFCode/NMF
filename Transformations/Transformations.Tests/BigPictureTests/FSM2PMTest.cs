using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Transformations.Example;
using FSM = NMF.Transformations.Example.FSM;
using PN = NMF.Transformations.Example.PN;
using NMF.Transformations;

using System.Linq;
using NMF.Transformations.Core;

namespace NMF.Transformations.Tests.BigPictureTests
{
    [TestClass]
    public class FSM2PMTest
    {
        private FSM2PN transformation;

        [TestInitialize]
        public void InitScene()
        {
            transformation = new FSM2PN();
        }

        [TestMethod]
        [TestCategory("Big picture")]
        public void TestFSM2PN1()
        {
            var fsm = new FSM.FiniteStateMachine()
            {
                ID = "Test"
            };

            var state1 = new FSM.State()
            {
                Name = "Zustand 1",
                IsStartState = true
            };

            var state2 = new FSM.State()
            {
                Name = "Zustand 2",
                IsEndState = true
            };

            var transition1 = new FSM.Transition()
            {
                StartState = state1,
                EndState = state1,
                Input = "a"
            };

            var transition2 = new FSM.Transition()
            {
                StartState = state1,
                EndState = state2,
                Input = "b"
            };

            var transition3 = new FSM.Transition()
            {
                StartState = state2,
                EndState = state1,
                Input = "a"
            };

            state1.Transitions.Add(transition1);
            state1.Transitions.Add(transition2);
            state2.Transitions.Add(transition3);

            fsm.States.Add(state1);
            fsm.States.Add(state2);
            fsm.Transitions.Add(transition1);
            fsm.Transitions.Add(transition2);
            fsm.Transitions.Add(transition3);

            var context = new TransformationContext(transformation);
            var pn = TransformationEngine.Transform<FSM.FiniteStateMachine, PN.PetriNet>(fsm, context);

            Assert.AreEqual(fsm, context.Input[0]);
            Assert.AreEqual(pn, context.Output);

            AssertSimilar(fsm, pn, context.Trace);
        }

        private void AssertSimilar(FSM.FiniteStateMachine fsm, PN.PetriNet pn, ITransformationTrace trace)
        {
            Assert.AreEqual(fsm.ID, pn.ID, "The ID of the petri net is not set correctly");
            Assert.AreEqual(fsm.States.Count, pn.Places.Count, "The number of places is incorrect");
            Assert.AreEqual(fsm.Transitions.Count + fsm.States.Where(s => s.IsEndState).Count(), pn.Transitions.Count,
                "The number of transitions is incorrect");

            foreach (var state in fsm.States)
            {
                var place = trace.Resolve<FSM.State, PN.Place>(state);

                Assert.IsNotNull(place, "A state has not been transformed");
                Assert.AreEqual(state.Name, place.ID, "The name of a place has not been set correctly");
                Assert.IsTrue(pn.Places.Contains(place), "A corresponding place has not been added to the states of the Petri net");

                if (state.IsStartState)
                {
                    Assert.AreEqual(1, place.TokenCount, "The number of tokens has not been set");
                }
                else
                {
                    Assert.AreEqual(0, place.TokenCount, "The number of tokens has not been set");
                }

                foreach (var t in state.Transitions)
                {
                    var t2 = trace.Resolve<FSM.Transition, PN.Transition>(t);

                    Assert.IsNotNull(t2, "A transition has not been transformed");
                    Assert.IsTrue(pn.Transitions.Contains(t2), "A transition has not been added to the transitions of the Petri net");
                }
            }
        }
    }
}
