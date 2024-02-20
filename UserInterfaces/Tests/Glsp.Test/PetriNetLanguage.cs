using NMF.Glsp.Language;
using NMF.Expressions.Linq;
using NMF.GlspTest.PetriNets;
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

            public override PetriNet CreateElement(string profile, object parent)
            {
                return new PetriNet { Name = "Examplenet" };
            }

            private class PlaceToTransitionEdgeCollection : CustomCollection<(IPlace, ITransition)>
            {
                private readonly PetriNet _pn;

                public PlaceToTransitionEdgeCollection(PetriNet pn)
                    : base(pn.Transitions.SelectMany(t => t.From, (t, p) => ValueTuple.Create(p, t)))
                {
                    _pn = pn;
                }

                public override void Add((IPlace, ITransition) item)
                {
                    item.Item2.From.Add(item.Item1);
                }

                public override void Clear()
                {
                    _pn.Places.Clear();
                }

                public override bool Remove((IPlace, ITransition) item)
                {
                    return item.Item2.From.Remove(item.Item1);
                }
            }

            private class TransitionToPlaceEdgeCollection : CustomCollection<(ITransition, IPlace)>
            {
                private readonly PetriNet _pn;

                public TransitionToPlaceEdgeCollection(PetriNet pn)
                    : base(pn.Transitions.SelectMany(t => t.To, (t, p) => ValueTuple.Create(t, p)))
                {
                    _pn = pn;
                }

                public override void Add((ITransition, IPlace) item)
                {
                    item.Item1.To.Add(item.Item2);
                }

                public override void Clear()
                {
                    _pn.Places.Clear();
                }

                public override bool Remove((ITransition, IPlace) item)
                {
                    return item.Item1.To.Remove(item.Item2);
                }
            }
        }

        public class PlaceDescriptor : NodeDescriptor<IPlace>
        {
            protected override void DefineLayout()
            {
                Label(p => p.Name)
                    .WithType("label:heading")
                    .At(40, 16);

                CssClass("task");
                CssClass("manual");
                Forward("layout", "vbox");
            }

            public override IPlace CreateElement(string profile, object parent)
            {
                return new Place { Name = "New Place" };
            }
        }

        public class TransitionDescriptor : NodeDescriptor<ITransition>
        {
            protected override void DefineLayout()
            {
                Label(t => t.Input)
                    .WithType("label:heading")
                    .At(40, 16);
            }

            public override ITransition CreateElement(string profile, object parent)
            {
                return new Transition { Input = "<trigger>" };
            }
        }
    }
}
