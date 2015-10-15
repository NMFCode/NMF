using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Transformations.Example.FSM
{
    public class FiniteStateMachine
    {
        public List<State> States { get; private set; }
        public List<Transition> Transitions { get; private set; }

        public string ID { get; set; }

        public FiniteStateMachine()
        {
            States = new List<State>();
            Transitions = new List<Transition>();
        }
    }

    public class State
    {
        public bool IsStartState { get; set; }
        public bool IsEndState { get; set; }

        public string Name { get; set; }

        public List<Transition> Transitions { get; private set; }

        public State()
        {
            Transitions = new List<Transition>();
        }

    }

    public class Transition
    {
        public State StartState { get; set; }
        public State EndState { get; set; }

        public string Input { get; set; }
    }
}
