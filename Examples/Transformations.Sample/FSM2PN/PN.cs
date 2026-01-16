using System.Collections.Generic;

#pragma warning disable CS1591 // missing comments

namespace NMF.Transformations.Example.PN
{
    public class PetriNet
    {
        public List<Place> Places { get; private set; }
        public List<Transition> Transitions { get; private set; }

        public string ID { get; set; }

        public PetriNet()
        {
            Places = new List<Place>();
            Transitions = new List<Transition>();
        }
    }

    public class Place
    {
        public string ID { get; set; }

        public List<Transition> Incoming { get; private set; }
        public List<Transition> Outgoing { get; private set; }

        public Place()
        {
            Incoming = new List<Transition>();
            Outgoing = new List<Transition>();
        }

        public int TokenCount { get; set; }
    }

    public class Transition
    {
        public List<Place> From { get; private set; }
        public List<Place> To { get; private set; }

        public string Input { get; set; }

        public Transition()
        {
            From = new List<Place>();
            To = new List<Place>();
        }
    }
}

#pragma warning restore CS1591 // missing comments