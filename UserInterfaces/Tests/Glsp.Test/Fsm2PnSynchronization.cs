using NMF.GlspTest.FiniteStateMachines;
using NMF.GlspTest.PetriNets;
using NMF.Synchronizations;
using NMF.Expressions.Linq;
using NMF.Transformations;

using FsmTransition = NMF.GlspTest.FiniteStateMachines.ITransition;
using PnTransition = NMF.GlspTest.PetriNets.ITransition;

using NMF.Collections;

namespace NMF.GlspTest
{
    public class Fsm2PnSynchronization : ReflectiveSynchronization
    {
        public class AutomataToNet : SynchronizationRule<StateMachine, PetriNet>
        {
            public override bool ShouldCorrespond(StateMachine left, PetriNet right, ISynchronizationContext context)
            {
                return true;
            }

            public override void DeclareSynchronization()
            {
                SynchronizeMany(SyncRule<StateToPlace>(),
                    fsm => fsm.States, pn => pn.Places);

                SynchronizeMany(SyncRule<TransitionToTransition>(),
                    fsm => fsm.States.SelectMany(s => s.Outgoing).IgnoreUpdates(), pn => pn.Transitions.Where(t => t.To.Count > 0));

                SynchronizeMany(SyncRule<EndStateToTransition>(),
                    fsm => fsm.States.Where(state => state.IsFinalState),
                    pn => pn.Transitions.Where(t => t.From.Count > 0 && t.To.Count == 0));

                Synchronize(fsm => fsm.Name, pn => pn.Name);
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

        public class TransitionToTransition : SynchronizationRule<FsmTransition, PnTransition>
        {

            public override void DeclareSynchronization()
            {
                Synchronize(t => t.Trigger, t => t.Input);

                Synchronize(SyncRule<StateToPlace>(),
                    t => t.Source,
                    t => t.From.FirstOrDefault());

                Synchronize(SyncRule<StateToPlace>(),
                    t => t.Target,
                    t => t.To.FirstOrDefault());
            }

            public override bool ShouldCorrespond(FsmTransition left, PnTransition right, ISynchronizationContext context)
            {
                var stateToPlace = SyncRule<StateToPlace>().LeftToRight;
                return left.Trigger == right.Input
                    && right.From.Contains(context.Trace.ResolveIn(stateToPlace, left.Source))
                    && right.To.Contains(context.Trace.ResolveIn(stateToPlace, left.Target));
            }
        }

        public class EndStateToTransition : SynchronizationRule<IState, PnTransition>
        {
            public override bool ShouldCorrespond(IState left, PnTransition right, ISynchronizationContext context)
            {
                return context.Trace.ResolveIn(SyncRule<StateToPlace>().LeftToRight, left) == right.From.FirstOrDefault();
            }

            protected override IState CreateLeftOutput(PnTransition transition, IEnumerable<IState> candidates, ISynchronizationContext context, out bool existing)
            {
                if (transition.From.Count == 0) throw new InvalidOperationException();
                existing = true;
                return context.Trace.ResolveIn(SyncRule<StateToPlace>().RightToLeft, transition.From.FirstOrDefault());
            }

            public override void DeclareSynchronization()
            {
                SynchronizeLeftToRightOnly(SyncRule<StateToPlace>(),
                    state => state.IsFinalState ? state : null,
                    transition => transition.From.FirstOrDefault());
            }
        }

    }
}
