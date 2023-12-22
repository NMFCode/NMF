using NMF.Glsp.Language;
using NMF.GlspTest.FiniteStateMachines;
using NMF.Collections;
using NMF.Expressions.Linq;
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

        public override DescriptorBase StartRule => Descriptor<StateMachineDescriptor>();

        public class StateMachineDescriptor : NodeDescriptor<StateMachine>
        {
            protected override void DefineLayout()
            {
                Nodes(D<StateDescriptor>(), m => m.States);
                Edges(D<TransitionDescriptor>(), m => m.States.SelectMany(s => s.Outgoing).IgnoreUpdates());
            }
        }

        public class StateDescriptor : NodeDescriptor<IState>
        {
            protected override void DefineLayout()
            {
                Label(s => s.Name, "label:name");
                Forward("isStartState", s => false.ToString());
                Forward("isEndState", s => s.IsFinalState.ToString());
            }
        }

        public class TransitionDescriptor : EdgeDescriptor<ITransition>
        {
            protected override void DefineLayout()
            {
                SourceNode(D<StateDescriptor>(), t => t.Source);
                TargetNode(D<StateDescriptor>(), t => t.Target);
                Label(t => t.Trigger);
            }
        }
    }
}
