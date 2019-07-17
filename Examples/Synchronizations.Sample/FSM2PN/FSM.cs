using NMF.Collections.ObjectModel;
using NMF.Expressions;
using NMF.Models.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace NMF.Synchronizations.Example.FSM
{

    public class FiniteStateMachine : INotifyPropertyChanged
    {
        public ICollectionExpression<State> States { get; private set; }
        public ICollectionExpression<Transition> Transitions { get; private set; }

        #region Id

        private string mId;

        public string Id
        {
            get
            {
                return mId;
            }
            set
            {
                if (mId != value)
                {
                    mId = value;
                    OnPropertyChanged("Id");
                }
            }
        }

        #endregion

        public FiniteStateMachine()
        {
            States = new ObservableSet<State>();
            Transitions = new ObservableSet<Transition>();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    [DebuggerDisplay("State {Name}")]
    public class State : INotifyPropertyChanged
    {
        #region IsStartState

        private bool isStartState;

        public bool IsStartState
        {
            get
            {
                return isStartState;
            }
            set
            {
                if (isStartState != value)
                {
                    isStartState = value;
                    OnPropertyChanged("IsStartState");
                }
            }
        }

        #endregion

        #region IsEndState

        private bool isEndState;

        public bool IsEndState
        {
            get
            {
                return isEndState;
            }
            set
            {
                if (isEndState != value)
                {
                    isEndState = value;
                    OnPropertyChanged("IsEndState");
                }
            }
        }

        #endregion

        #region Name

        private string name;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        #endregion

        public ICollectionExpression<Transition> Transitions { get; private set; }

        public State()
        {
            Transitions = new StateTransitionCollection(this);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString()
        {
            return $"State {Name}";
        }
    }

    [DebuggerDisplay("{Representation}")]
    public class Transition : INotifyPropertyChanged
    {
        #region StartState

        private State startState;

        public State StartState
        {
            get
            {
                return startState;
            }
            set
            {
                if (startState != value)
                {
                    var old = startState;
                    startState = value;
                    if (old != null) old.Transitions.Remove(this);
                    if (value != null && !value.Transitions.Contains(this)) value.Transitions.Add(this);
                    OnPropertyChanged("StartState");
                }
            }
        }

        #endregion

        #region EndState

        private State endState;

        public State EndState
        {
            get
            {
                return endState;
            }
            set
            {
                if (endState != value)
                {
                    endState = value;
                    OnPropertyChanged("EndState");
                }
            }
        }

        #endregion

        #region Input

        private string input;

        public string Input
        {
            get
            {
                return input;
            }
            set
            {
                if (input != value)
                {
                    input = value;
                    OnPropertyChanged("Input");
                }
            }
        }

        #endregion

        public string Representation
        {
            get
            {
                var start = startState != null ? startState.Name : "<null>";
                var end = endState != null ? endState.Name : "<null>";
                return start + " --(" + Input + ")-> " + end;
            }
        }

        public override string ToString()
        {
            return Representation;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class StateTransitionCollection : ObservableOppositeSet<State, Transition>
    {
        public StateTransitionCollection(State parent) : base(parent) { }

        protected override void SetOpposite(Transition item, State newParent)
        {
            item.StartState = newParent;
        }

        public override string ToString()
        {
            return $"transitions of {Parent}";
        }
    }

}
