using NMF.Expressions;
using NMF.Glsp.Graph;
using NMF.Glsp.Notation;

namespace NMF.Glsp.Processing
{
    internal class GLabelSkeleton<T> : GElementSkeleton<T>
    {
        public ObservingFunc<T, string> LabelValue { get; set; }

        public bool CanEdit { get; set; }

        protected override GElement CreateElement(T input, ISkeletonTrace trace, ref INotationElement notation)
        {
            var label = new GLabel();
            if (CanEdit)
            {
                var dynamicValue = LabelValue.InvokeReversable(input);
                label.Text = dynamicValue.Value;
                label.Collectibles.Add(LabelValue, dynamicValue);
                label.TextChanged += () => dynamicValue.Value = label.Text;
            }
            else
            {
                var dynamicValue = LabelValue.Observe(input);
                label.Text = dynamicValue.Value;
                label.Collectibles.Add(LabelValue, dynamicValue);
            }
            return label;
        }
    }
}
