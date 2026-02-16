using AnyText.Tests.Synchronization.Metamodel.PetriNet;
using AnyText.Tests.Synchronization.Metamodel.StateMachine;
using NMF.Synchronizations;
using NMF.Transformations;
using NMF.Expressions.Linq;
using ITransition = AnyText.Tests.Synchronization.Metamodel.StateMachine.ITransition;
using PNTransition = AnyText.Tests.Synchronization.Metamodel.PetriNet.ITransition;
using NMF.Collections.ObjectModel;

namespace AnyText.Tests.Synchronization
{
    public class FSM2PN: ReflectiveSynchronization
    {
        public class AutomataToNet : SynchronizationRule<IStateMachine, IPetriNet>
        {
            public override bool ShouldCorrespond(IStateMachine left, IPetriNet right, ISynchronizationContext context)
            {
                return true;
            }

            public override void DeclareSynchronization()
            {
                SynchronizeMany(SyncRule<StateToPlace>(),
                    fsm => fsm.States, pn => pn.Places);

                SynchronizeMany(SyncRule<TransitionToTransition>(),
                    fsm => fsm.Transitions, pn => pn.Transitions.Where(t => t.To.Count > 0 && t.From.Count > 0));

                SynchronizeMany<IState, IPlace>(SyncRule<StateToPlace>(),
                    fsm => fsm.States.Where(state => state.IsEndState, true),
                    pn => new EndPlaceCollection(pn));

                SynchronizeMany(SyncRule<StateToPlace>(),
                    fsm => fsm.States.Where(state => state.IsStartState, true),
                    pn => new InitialPlaceCollection(pn));

                Synchronize(fsm => fsm.Id, pn => pn.Id);
            }

            private class EndPlaceCollection : CustomCollection<IPlace>
            {
                private readonly IPetriNet _net;

                public EndPlaceCollection(IPetriNet net)
                    : base(net.Places.Where(p => net.Transitions.Any(t => t.From.Contains(p) && t.To.Count == 0)))
                {
                    _net = net;
                }

                public override void Add(IPlace item)
                {
                    _net.Transitions.Add(new Metamodel.PetriNet.Transition
                    {
                        From = { item }
                    });
                }

                public override void Clear()
                {
                    foreach (var tr in _net.Transitions.AsEnumerable().Where(t => t.To.Count == 0 && t.From.Count > 0).ToArray())
                    {
                        _net.Transitions.Remove(tr);
                    }
                }

                public override bool Remove(IPlace item)
                {
                    var found = false;
                    foreach (var tr in _net.Transitions.AsEnumerable().Where(t => t.To.Count == 0 && t.From.Contains(item)).ToArray())
                    {
                        _net.Transitions.Remove(tr);
                        found = true;
                    }
                    return found;
                }
            }

            private class InitialPlaceCollection : CustomCollection<IPlace>
            {
                private readonly IPetriNet _net;

                public InitialPlaceCollection(IPetriNet net)
                    : base(net.Places.Where(p => net.Transitions.Any(t => t.To.Contains(p) && t.From.Count == 0)))
                {
                    _net = net;
                }

                public override void Add(IPlace item)
                {
                    _net.Transitions.Add(new Metamodel.PetriNet.Transition
                    {
                        To = { item }
                    });
                }

                public override void Clear()
                {
                    foreach (var tr in _net.Transitions.AsEnumerable().Where(t => t.From.Count == 0 && t.To.Count > 0).ToArray())
                    {
                        _net.Transitions.Remove(tr);
                    }
                }

                public override bool Remove(IPlace item)
                {
                    var found = false;
                    foreach (var tr in _net.Transitions.AsEnumerable().Where(t => t.From.Count == 0 && t.To.Contains(item)).ToArray())
                    {
                        _net.Transitions.Remove(tr);
                        found = true;
                    }
                    return found;
                }
            }
        }

        public class StateToPlace : SynchronizationRule<IState, IPlace>
        {
            public override bool ShouldCorrespond(IState left, IPlace right, ISynchronizationContext context)
            {
                return left.Name == right.Name;
            }

            public override void DeclareSynchronization()
            {
                Synchronize(state => state.Name, place => place.Name);
            }
        }

        public class TransitionToTransition : SynchronizationRule<ITransition, PNTransition>
        {

            public override void DeclareSynchronization()
            {
                Synchronize(t => t.Input, t => t.Input);

                Synchronize(SyncRule<StateToPlace>(),
                    t => t.StartState,
                    t => t.From.FirstOrDefault());

                Synchronize(SyncRule<StateToPlace>(),
                    t => t.EndState,
                    t => t.To.FirstOrDefault());
            }

            public override bool ShouldCorrespond(ITransition left, PNTransition right, ISynchronizationContext context)
            {
                var stateToPlace = SyncRule<StateToPlace>().LeftToRight;
                return left.Input == right.Input
                    && right.From.Contains(context.Trace.ResolveIn(stateToPlace, left.StartState))
                    && right.To.Contains(context.Trace.ResolveIn(stateToPlace, left.EndState));
            }
        }
    }
}