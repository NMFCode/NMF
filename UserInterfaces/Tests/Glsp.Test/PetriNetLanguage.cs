using NMF.Glsp.Language;
using NMF.Expressions.Linq;
using NMF.Synchronizations.Example.PN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMF.Collections.ObjectModel;

namespace Glsp.Test
{
    internal class PetriNetLanguage : GraphicalLanguage
    {
        public override string DiagramType => "petriNet";

        public override DescriptorBase StartRule => Descriptor<PetriNetDescriptor>();

        public class PetriNetDescriptor : NodeDescriptor<PetriNet>
        {
            protected override void DefineLayout()
            {
                var places = D<PlaceDescriptor>();
                var transitions = D<TransitionDescriptor>();

                Nodes(places, pn => pn.Places);
                Nodes(transitions, pn => pn.Transitions);

                Edges(places, transitions, pn => new PlaceToTransitionEdgeCollection(pn));
                Edges(transitions, places, pn => new TransitionToPlaceEdgeCollection(pn));
            }

            private class PlaceToTransitionEdgeCollection : CustomCollection<(Place, Transition)>
            {
                private readonly PetriNet _pn;

                public PlaceToTransitionEdgeCollection(PetriNet pn)
                    : base(pn.Transitions.SelectMany(t => t.From, (t, p) => ValueTuple.Create(p, t)))
                {
                    _pn = pn;
                }

                public override void Add((Place, Transition) item)
                {
                    item.Item2.From.Add(item.Item1);
                }

                public override void Clear()
                {
                    _pn.Places.Clear();
                }

                public override bool Remove((Place, Transition) item)
                {
                    return item.Item2.From.Remove(item.Item1);
                }
            }

            private class TransitionToPlaceEdgeCollection : CustomCollection<(Transition, Place)>
            {
                private readonly PetriNet _pn;

                public TransitionToPlaceEdgeCollection(PetriNet pn)
                    : base(pn.Transitions.SelectMany(t => t.To, (t, p) => ValueTuple.Create(t, p)))
                {
                    _pn = pn;
                }

                public override void Add((Transition, Place) item)
                {
                    item.Item1.To.Add(item.Item2);
                }

                public override void Clear()
                {
                    _pn.Places.Clear();
                }

                public override bool Remove((Transition, Place) item)
                {
                    return item.Item1.To.Remove(item.Item2);
                }
            }
        }

        public class PlaceDescriptor : NodeDescriptor<Place>
        {
            protected override void DefineLayout()
            {
                Label(p => p.Id);
                Type("node");
            }
        }

        public class TransitionDescriptor : NodeDescriptor<Transition>
        {
            protected override void DefineLayout()
            {
                Label(t => t.Input);
                Type("node");
            }
        }
    }
}
