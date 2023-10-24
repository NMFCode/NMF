using NMF.Expressions;
using NMF.Glsp.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Language
{
    internal class GElementSkeleton<T>
    {
        public string StaticType { get; set; }

        public ObservingFunc<T, string> DynamicType { get; set; }

        public List<(string key, string value)> StaticForwards { get; } = new List<(string key, string staticKey)>();

        public List<(string key, ObservingFunc<T, string> dynamicValue)> DynamicForwards { get; } = new List<(string key, ObservingFunc<T, string> dynamicValue)>();

        public List<string> StaticCssClasses { get; } = new List<string>();

        public List<ObservingFunc<T, string>> DynamicCssClasses { get; } = new List<ObservingFunc<T, string>>();

        public List<ChildContributionBase<T>> ChildContributions { get; } = new List<ChildContributionBase<T>>();

        protected virtual GElement CreateElement(T input) => new GElement();

        public GElement Create(T input)
        {
            var element = CreateElement(input);
            if (StaticType != null)
            {
                element.Type = StaticType;
            }
            if (DynamicType != null)
            {
                var dynamicType = DynamicType.Observe(input);
                element.Collectibles.Add(dynamicType);
                element.Type = dynamicType.Value;
                dynamicType.ValueChanged += element.UpdateType;
            }
            foreach (var forward in StaticForwards)
            {
                element.Details.Add(forward.key, forward.value);
            }
            foreach (var dynamicForward in DynamicForwards)
            {
                var dynamicValue = dynamicForward.dynamicValue.Observe(input);
                element.Collectibles.Add(dynamicValue);
                element.Details.Add(dynamicForward.key, dynamicValue.Value);
                dynamicValue.ValueChanged += (_, e) => element.Details[dynamicForward.key] = e.NewValue as string;
            }
            foreach(var staticCss in StaticCssClasses)
            {
                element.CssClasses.Add(staticCss);
            }
            foreach (var dynamicCss in DynamicCssClasses)
            {
                var dynamicClass = dynamicCss.Observe(input);
                if (dynamicClass != null)
                {
                    element.CssClasses.Add(dynamicClass.Value);
                }
                element.Collectibles.Add(dynamicClass);
                dynamicClass.ValueChanged += element.UpdateClass;
            }
            foreach (var childContribution in ChildContributions)
            {
                childContribution.Contribute(input, element);
            }
            return element;
        }
    }
}
