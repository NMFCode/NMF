using NMF.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Linq;
using NMF.Models.Collections;
using NMF.Expressions;

namespace NMF.Synchronizations.Example.PN
{
    public class PetriNet : INotifyPropertyChanged
    {
        public ICollectionExpression<Place> Places { get; private set; }
        public ICollectionExpression<Transition> Transitions { get; private set; }

        #region Id

        private string id;

        public string Id
        {
            get
            {
                return id;
            }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged("Id");
                }
            }
        }

        #endregion

        public PetriNet()
        {
            Places = new PetriNetPlacesCollection(this);
            Transitions = new PetriNetTransitionCollection(this);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    [DebuggerDisplay("Place {Id}")]
    public class Place : INotifyPropertyChanged
    {
        #region Id

        private string id;

        public string Id
        {
            get
            {
                return id;
            }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged("Id");
                }
            }
        }

        #endregion

        public ICollectionExpression<Transition> Incoming { get; private set; }
        public ICollectionExpression<Transition> Outgoing { get; private set; }

        public Place()
        {
            Incoming = new PlaceIncomingCollection(this);
            Outgoing = new PlaceOutgoingCollection(this);
        }

        #region TokenCount

        private int tokenCount;

        public int TokenCount
        {
            get
            {
                return tokenCount;
            }
            set
            {
                if (tokenCount != value)
                {
                    tokenCount = value;
                    OnPropertyChanged("TokenCount");
                }
            }
        }

        #endregion
        

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return $"Place {Id}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    [DebuggerDisplay("{Representation}")]
    public class Transition : INotifyPropertyChanged
    {
        public ICollectionExpression<Place> From { get; private set; }
        public ICollectionExpression<Place> To { get; private set; }

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
                var start = string.Join(",", From.Select(p => p.Id));
                var end = string.Join(",", To.Select(p => p.Id));
                return string.Format("[{0}] --({1})-> [{2}]", start, input, end);
            }
        }

        public override string ToString()
        {
            return Representation;
        }


        public Transition()
        {
            From = new TransitionFromCollection(this);
            To = new TransitionToCollection(this);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class PlaceIncomingCollection : ObservableOppositeSet<Place, Transition>
    {
        public PlaceIncomingCollection(Place parent) : base(parent) { }

        protected override void SetOpposite(Transition item, Place newParent)
        {
            if (newParent != null)
            {
                item.To.Add(newParent);
            }
            else
            {
                item.To.Remove(Parent);
            }
        }

        public override string ToString()
        {
            return $"incoming transitions for {Parent}";
        }
    }

    public class PlaceOutgoingCollection : ObservableOppositeSet<Place, Transition>
    {
        public PlaceOutgoingCollection(Place parent) : base(parent) { }

        protected override void SetOpposite(Transition item, Place newParent)
        {
            if (newParent != null)
            {
                item.From.Add(newParent);
            }
            else
            {
                item.From.Remove(Parent);
            }
        }

        public override string ToString()
        {
            return $"outgoing transitions for {Parent}";
        }
    }

    public class TransitionFromCollection : ObservableOppositeSet<Transition, Place>
    {
        public TransitionFromCollection(Transition parent) : base(parent) { }

        protected override void SetOpposite(Place item, Transition newParent)
        {
            if (newParent != null)
            {
                item.Outgoing.Add(newParent);
            }
            else
            {
                item.Outgoing.Remove(Parent);
            }
        }

        public override string ToString()
        {
            return $"source places of {Parent}";
        }
    }

    public class TransitionToCollection : ObservableOppositeSet<Transition, Place>
    {
        public TransitionToCollection(Transition parent) : base(parent) { }

        protected override void SetOpposite(Place item, Transition newParent)
        {
            if (newParent != null)
            {
                item.Incoming.Add(newParent);
            }
            else
            {
                item.Incoming.Remove(Parent);
            }
        }

        public override string ToString()
        {
            return $"target places of {Parent}";
        }
    }

    public class PetriNetTransitionCollection : ObservableOppositeSet<PetriNet, Transition>
    {
        public PetriNetTransitionCollection(PetriNet parent) : base(parent)
        {
        }

        protected override void SetOpposite(Transition item, PetriNet newParent)
        {
            if (newParent == null)
            {
                item.From.Clear();
                item.To.Clear();
            }
        }

        public override string ToString()
        {
            return $"transitions of {Parent}";
        }
    }

    public class PetriNetPlacesCollection : ObservableOppositeSet<PetriNet, Place>
    {
        public PetriNetPlacesCollection(PetriNet parent) : base(parent)
        {
        }

        protected override void SetOpposite(Place item, PetriNet newParent)
        {
            if (newParent == null)
            {
                item.Incoming.Clear();
                item.Outgoing.Clear();
            }
        }

        public override string ToString()
        {
            return $"places of {Parent}";
        }
    }


}
