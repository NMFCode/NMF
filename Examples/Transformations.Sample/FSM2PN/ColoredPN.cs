using NMF.Transformations.Core;
using System.Collections.Generic;

namespace NMF.Transformations.Example.PN
{
    public class ColoredTransition : Transition
    {
        public Color Color { get; set; }
    }

    public class Color
    {
    }

    public class ColoredPlace : Place
    {
        public Dictionary<Color, int> Tokens { get; set; }
    }

    public class ColoredPetriNet : PetriNet
    {
        public List<Color> Colors { get; private set; }
    }

    public class FSM2ColoredPN : FSM2PN
    {
        [OverrideRule]
        public class ColoredTransition2Transition : TransitionToTransition
        {
            public override Transition CreateOutput(FSM.Transition input, ITransformationContext context)
            {
                return new PN.ColoredTransition() { Color = context.Bag.DefaultColor };
            }
        }
        [OverrideRule]
        public class State2ColoredPlace : StateToPlace
        {
            public override Place CreateOutput(FSM.State input, ITransformationContext context)
            {
                return new ColoredPlace();
            }
            public override void Transform(FSM.State input, Place output, ITransformationContext context)
            {
                if (output is ColoredPlace colored && input.IsStartState)
                {
                    colored.Tokens.Add(context.Bag.DefaultColor, 1);
                }
            }
        }
        [OverrideRule]
        public class Automata2ColoredPetriNet : AutomataToNet
        {
            public override PetriNet CreateOutput(FSM.FiniteStateMachine input, ITransformationContext context)
            {
                var coloredNet = new ColoredPetriNet();
                var defaultColor = new Color();
                coloredNet.Colors.Add(defaultColor);
                context.Bag.DefaultColor = defaultColor;
                return coloredNet;
            }
        }
    }
}
