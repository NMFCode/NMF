using NMF.Expressions.Linq;
using NMF.Transformations;
using System;
using System.Collections.Generic;

namespace NMF.Synchronizations.Example
{
    public class FSM2PN : ReflectiveSynchronization
    {
        public class AutomataToNet : SynchronizationRule<FSM.FiniteStateMachine, PN.PetriNet>
        {
            public override bool ShouldCorrespond(FSM.FiniteStateMachine left, PN.PetriNet right, ISynchronizationContext context)
            {
                return true;
            }

            public override void DeclareSynchronization()
            {
                SynchronizeMany(SyncRule<StateToPlace>(),
                    fsm => fsm.States, pn => pn.Places);

                SynchronizeMany(SyncRule<TransitionToTransition>(),
                    fsm => fsm.Transitions, pn => pn.Transitions.Where(t => t.To.Count > 0));

                SynchronizeMany(SyncRule<EndStateToTransition>(),
                    fsm => fsm.States.Where(state => state.IsEndState),
                    pn => pn.Transitions.Where(t => t.To.Count == 0));

                Synchronize(fsm => fsm.Id, pn => pn.Id);
            }
        }

        public class StateToPlace : SynchronizationRule<FSM.State, PN.Place>
        {
            public override bool ShouldCorrespond(FSM.State left, PN.Place right, ISynchronizationContext context)
            {
                return left.Name == right.Id;
            }

            public override void DeclareSynchronization()
            {
                Synchronize(state => state.Name, place => place.Id);
            }
        }

        public class TransitionToTransition : SynchronizationRule<FSM.Transition, PN.Transition>
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

            public override bool ShouldCorrespond(FSM.Transition left, PN.Transition right, ISynchronizationContext context)
            {
                var stateToPlace = SyncRule<StateToPlace>().LeftToRight;
                return left.Input == right.Input
                    && right.From.Contains(context.Trace.ResolveIn(stateToPlace, left.StartState))
                    && right.To.Contains(context.Trace.ResolveIn(stateToPlace, left.EndState));
            }
        }

        public class EndStateToTransition : SynchronizationRule<FSM.State, PN.Transition>
        {
            public override bool ShouldCorrespond(FSM.State left, PN.Transition right, ISynchronizationContext context)
            {
                return context.Trace.ResolveIn(SyncRule<StateToPlace>().LeftToRight, left) == right.From.FirstOrDefault();
            }

            protected override FSM.State CreateLeftOutput(PN.Transition transition, IEnumerable<FSM.State> candidates, ISynchronizationContext context, out bool existing)
            {
                if (transition.From.Count == 0) throw new InvalidOperationException();
                existing = true;
                return context.Trace.ResolveIn(SyncRule<StateToPlace>().RightToLeft, transition.From.FirstOrDefault());
            }

            public override void DeclareSynchronization()
            {
                SynchronizeLeftToRightOnly(SyncRule<StateToPlace>(),
                    state => state.IsEndState ? state : null,
                    transition => transition.From.FirstOrDefault());
            }
        }
    }
}
