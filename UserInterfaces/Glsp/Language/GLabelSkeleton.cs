using NMF.Expressions;
using NMF.Glsp.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Language
{
    internal class GLabelSkeleton<T> : GElementSkeleton<T>
    {
        public ObservingFunc<T, string> LabelValue { get; set; }

        public bool CanEdit { get; set; }

        protected override GElement CreateElement(T input)
        {
            var label = new GLabel();
            if (CanEdit)
            {
                var dynamicValue = LabelValue.InvokeReversable(input);
                label.Text = dynamicValue.Value;
                label.Collectibles.Add(dynamicValue);
                label.TextChanged += () => dynamicValue.Value = label.Text;
            }
            else
            {
                var dynamicValue = LabelValue.Observe(input);
                label.Text = dynamicValue.Value;
                label.Collectibles.Add(dynamicValue);
            }
            return label;
        }
    }
}
