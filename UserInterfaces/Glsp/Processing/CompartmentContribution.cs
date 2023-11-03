using NMF.Expressions;
using NMF.Glsp.Graph;
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
        public GElementSkeleton<T> Compartment { get; init; }

        public ObservingFunc<T, bool> Guard { get; set; }

        public override Type SourceType => typeof(T);

        public override Type TargetType => typeof(T);

        public override void Contribute(T input, GElement element, ISkeletonTrace trace)
        {
            var compartment = Compartment.Create(input, trace);
            if (Guard != null)
            {
                var dynamicGuard = Guard.Observe(input);
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

        public override IEnumerable<ShapeTypeHint> CreateShapeHints()
        {
            return Enumerable.Empty<ShapeTypeHint>();
        }

        public override IEnumerable<BaseAction> SuggestActions(GElement item, T element, List<GElement> selected, string contextId, EditorContext editorContext)
        {
            return Enumerable.Empty<BaseAction>();
        }
    }
}
