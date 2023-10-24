using NMF.Glsp.Language;
using NMF.Synchronizations.Example.FSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glsp.Test
{
    internal class FsmLanguage : GraphicalLanguage
    {
        public override string DiagramType => "finiteStateMachines";

        public override Type SemanticRootType => typeof(FiniteStateMachine);

        public class StateMachineDescriptor : NodeDescriptor<FiniteStateMachine>
        {
            protected override void DefineLayout()
            {
                Nodes(D<StateDescriptor>(), m => m.States);
                Edges(D<TransitionDescriptor>(), m => m.Transitions);
            }
        }

        public class StateDescriptor : NodeDescriptor<State>
        {
            protected override void DefineLayout()
            {
                Label(s => s.Name, "label:name");
                Forward("isStartState", s => s.IsStartState.ToString());
                Forward("isEndState", s => s.IsEndState.ToString());
            }
        }

        public class TransitionDescriptor : EdgeDescriptor<Transition>
        {
            protected override void DefineLayout()
            {
                SourceNode(t => t.StartState);
                TargetNode(t => t.EndState);
                Label(t => t.Input);
            }
        }
    }
}
