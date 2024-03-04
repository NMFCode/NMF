using NMF.Expressions;
using NMF.Glsp.Graph;
using NMF.Glsp.Notation;
using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Protocol.Modification;
using NMF.Glsp.Protocol.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NMF.Glsp.Processing
{
    internal class CompartmentContribution<T> : NodeContributionBase<T>
    {
        public GElementSkeleton<T> CompartmentSkeleton { get; init; }

        public ObservingFunc<T, bool> Guard { get; set; }

        public override Type SourceType => typeof(T);

        public override Type TargetType => typeof(T);

        public override IEnumerable<string> ContainableElementIds()
        {
            return CompartmentSkeleton.ContainableTypeIds();
        }

        public override void Contribute(T input, GElement element, ISkeletonTrace trace)
        {
            var compartment = CompartmentSkeleton.Create(input, trace, element.NotationElement);
            if (Guard != null)
            {
                var dynamicGuard = Guard.Observe(input);
                dynamicGuard.Successors.SetDummy();
                if (dynamicGuard.Value)
                {
                    element.Children.Add(compartment);
                    compartment.Parent = element;
                }
                dynamicGuard.ValueChanged += (_, e) =>
                {
                    if ((bool)e.NewValue)
                    {
                        element.Children.Add(compartment);
                        compartment.Parent = element;
                    }
                    else
                    {
                        element.Children.Remove(compartment);
                    }
                };
                element.Collectibles.Add(this, dynamicGuard);
            }
            else
            {
                element.Children.Add(compartment);
                compartment.Parent = element;
            }
        }

        public override IEnumerable<LabeledAction> SuggestActions(GElement item, GElementSkeletonBase skeleton, ICollection<GElement> selected, string contextId, EditorContext editorContext)
        {
            var compartment = item?.FindCompartment(CompartmentSkeleton);
            if ((item != null && compartment == null) || CompartmentSkeleton.IsEmbedding(skeleton))
            {
                return Enumerable.Empty<LabeledAction>();
            }
            return CompartmentSkeleton.NodeContributions.SelectMany(cc => cc.SuggestActions(compartment, CompartmentSkeleton, selected, contextId, editorContext));
        }
    }
}
