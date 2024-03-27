using System.Linq;
using NMF.Transformations.Core;

namespace NMF.Transformations.Example
{
    public class FSM2PN : ReflectiveTransformation
    {
        public class AutomataToNet : TransformationRule<FSM.FiniteStateMachine, PN.PetriNet>
        {
            public override void Transform(FSM.FiniteStateMachine input, PN.PetriNet output, ITransformationContext context)
            {
                output.ID = input.ID;
            }
        }

        public class StateToPlace : TransformationRule<FSM.State, PN.Place>
        {
            public override void Transform(FSM.State input, PN.Place output, ITransformationContext context)
            {
                output.ID = input.Name;

                if (input.IsStartState)
                {
                    output.TokenCount = 1;
                }
                else
                {
                    output.TokenCount = 0;
                }
            }

            public override void RegisterDependencies()
            {
                CallForEach(this.Rule<AutomataToNet>(),
                    selector: fsm => fsm.States,
                    persistor: (pn, places) => pn.Places.AddRange(places));
            }
        }

        public class TransitionToTransition : TransformationRule<FSM.Transition, PN.Transition>
        {
            public override void Transform(FSM.Transition input, PN.Transition output, ITransformationContext context)
            {
                output.Input = input.Input;
            }

            public override void RegisterDependencies()
            {
                CallForEach<FSM.FiniteStateMachine, PN.PetriNet>(
                    selector: fsm => fsm.Transitions,
                    persistor: (pn, transitions) => pn.Transitions.AddRange(transitions));

                Require(Rule<StateToPlace>(),
                    selector: t => t.StartState, 
                    persistor: (t, p) => {
                        t.From.Add(p);
                        p.Outgoing.Add(t);
                    });

                Require(Rule<StateToPlace>(),
                    selector: t => t.EndState, 
                    persistor: (t, p) => {
                        t.To.Add(p);
                        p.Incoming.Add(t);
                    });
            }
        }

        public class EndStateToTransition : TransformationRule<FSM.State, PN.Transition>
        {

            public override void Transform(FSM.State input, PN.Transition output, ITransformationContext context)
            {
                var from = context.Trace.ResolveIn(Rule<StateToPlace>(), input);

                output.From.Add(from);
                from.Outgoing.Add(output);
                output.Input = "";
            }

            public override void RegisterDependencies()
            {
                CallForEach(Rule<AutomataToNet>(),
                    selector: fsm => fsm.States.Where(s => s.IsEndState),
                    persistor: (pn, endTransitions) => pn.Transitions.AddRange(endTransitions));
            }
        }
    }
}
