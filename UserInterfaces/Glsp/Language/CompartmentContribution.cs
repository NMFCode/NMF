using NMF.Expressions;
using NMF.Glsp.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Language
{
    internal class CompartmentContribution<T> : ChildContributionBase<T>
    {
        public GElementSkeleton<T> Compartment { get; init; }

        public ObservingFunc<T, bool> Guard { get; set; }

        public override void Contribute(T input, GElement element)
        {
            var compartment = Compartment.Create(input);
            if (Guard != null)
            {
                var dynamicGuard = Guard.Observe(input);
                if (dynamicGuard.Value)
                {
                    element.Children.Add(compartment);
                }
                dynamicGuard.ValueChanged += (_, e) =>
                {
                    if ((bool)e.NewValue)
                    {
                        element.Children.Add(compartment);
                    }
                    else
                    {
                        element.Children.Remove(compartment);
                    }
                };
                element.Collectibles.Add(dynamicGuard);
            }
            else
            {
                element.Children.Add(compartment);
            }
        }
    }
}
