using AnyText.Tests.Synchronization.Metamodel.PetriNet;
using AnyText.Tests.Synchronization.Metamodel.StateMachine;
using NMF.Synchronizations;
using NMF.Transformations;
using NMF.Expressions.Linq;
using ITransition = AnyText.Tests.Synchronization.Metamodel.StateMachine.ITransition;

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

                SynchronizeMany(SyncRule<EndStateToTransition>(),
                    fsm => fsm.States.Where(state => state.IsEndState, true),
                    pn => pn.Transitions.Where(t => t.From.Count > 0 && t.To.Count == 0));

                SynchronizeMany(SyncRule<StartStateToTransition>(),
                    fsm => fsm.States.Where(state => state.IsStartState, true),
                    pn => pn.Transitions.Where(t => t.From.Count == 0 && t.To.Count > 0));

                Synchronize(fsm => fsm.Id, pn => pn.Id);
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

        public class TransitionToTransition : SynchronizationRule<ITransition, AnyText.Tests.Synchronization.Metamodel.PetriNet.ITransition>
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

            public override bool ShouldCorrespond(ITransition left, AnyText.Tests.Synchronization.Metamodel.PetriNet.ITransition right, ISynchronizationContext context)
            {
                var stateToPlace = SyncRule<StateToPlace>().LeftToRight;
                return left.Input == right.Input
                    && right.From.Contains(context.Trace.ResolveIn(stateToPlace, left.StartState))
                    && right.To.Contains(context.Trace.ResolveIn(stateToPlace, left.EndState));
            }
        }

        public class EndStateToTransition : SynchronizationRule<IState, AnyText.Tests.Synchronization.Metamodel.PetriNet.ITransition>
        {
            public override bool ShouldCorrespond(IState left, AnyText.Tests.Synchronization.Metamodel.PetriNet.ITransition right, ISynchronizationContext context)
            {
                return context.Trace.ResolveIn(SyncRule<StateToPlace>().LeftToRight, left) == right.From.FirstOrDefault();
            }

            protected override IState CreateLeftOutput(AnyText.Tests.Synchronization.Metamodel.PetriNet.ITransition transition, IEnumerable<IState> candidates, ISynchronizationContext context, out bool existing)
            {
                if (transition.From.Count == 0) throw new InvalidOperationException();
                existing = true;
                return context.Trace.ResolveIn(SyncRule<StateToPlace>().RightToLeft, transition.From.FirstOrDefault());
            }

            public override void DeclareSynchronization()
            {
                SynchronizeLeftToRightOnly(SyncRule<StateToPlace>()!,
                    state => state.IsEndState ? state : null,
                    transition => transition.From.FirstOrDefault());
            }
        }

        public class StartStateToTransition : SynchronizationRule<IState, Metamodel.PetriNet.ITransition>
        {
            public override bool ShouldCorrespond(IState left, Metamodel.PetriNet.ITransition right, ISynchronizationContext context)
            {
                return context.Trace.ResolveIn(SyncRule<StateToPlace>().LeftToRight, left) == right.To.FirstOrDefault();
            }

            protected override IState CreateLeftOutput(Metamodel.PetriNet.ITransition transition, IEnumerable<IState> candidates, ISynchronizationContext context, out bool existing)
            {
                if (transition.To.Count == 0) throw new InvalidOperationException();
                existing = true;
                return context.Trace.ResolveIn(SyncRule<StateToPlace>().RightToLeft, transition.To.FirstOrDefault());
            }

            public override void DeclareSynchronization()
            {
                SynchronizeLeftToRightOnly(SyncRule<StateToPlace>()!,
                    state => state.IsStartState ? state : null,
                    transition => transition.To.FirstOrDefault());
            }
        }
    }
}