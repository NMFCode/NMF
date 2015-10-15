using System;
using System.Collections.Generic;
using System.Text;
using NMF.Transformations;
using NMF.Transformations.Core;

namespace NMF.Transformations.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var fsm = CreateTestFiniteStateMachine();

            var pn = TransformationEngine.Transform<FSM.FiniteStateMachine, PN.PetriNet>(fsm, new FSM2PN());

            Console.WriteLine(pn.ID);
        }

        private static FSM.FiniteStateMachine CreateTestFiniteStateMachine()
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

            return fsm;
        }
    }
}
