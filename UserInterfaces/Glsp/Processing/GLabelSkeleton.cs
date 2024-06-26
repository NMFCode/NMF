﻿using NMF.Expressions;
using NMF.Glsp.Graph;
using NMF.Glsp.Language;
using NMF.Glsp.Notation;
using NMF.Glsp.Protocol.Types;
using NMF.Glsp.Protocol.Validation;
using System;

namespace NMF.Glsp.Processing
{
    internal class GLabelSkeleton<T> : GElementSkeleton<T>
    {
        public GLabelSkeleton(ElementDescriptor<T> elementDescriptor) : base(elementDescriptor)
        {
        }

        public ObservingFunc<T, string> LabelValue { get; set; }

        public Action<T, string> CustomSetter { get; set; }

        public bool CanEdit { get; set; }

        public Point? Position { get; set; }

        public override bool IsLabel => true;

        public EdgeLabelPlacement EdgeLabelPlacement { get; set; }

        protected override GElement CreateElement(T input, ISkeletonTrace trace, ref INotationElement notation)
        {
            var label = new GLabel();
            if (CanEdit && CustomSetter == null)
            {
                var dynamicValue = LabelValue.InvokeReversable(input);
                dynamicValue.Successors.SetDummy();
                dynamicValue.ValueChanged += label.OnTextChanged;
                label.Text = dynamicValue.Value;
                label.Collectibles.Add(LabelValue, dynamicValue);
                label.TextChanged += (text) => dynamicValue.Value = text;
            }
            else
            {
                var dynamicValue = LabelValue.Observe(input);
                dynamicValue.Successors.SetDummy();
                dynamicValue.ValueChanged += label.OnTextChanged;
                label.Text = dynamicValue.Value;
                label.Collectibles.Add(LabelValue, dynamicValue);
                if (CanEdit)
                {
                    label.TextChanged += (text) => CustomSetter(input, text);
                }
            }
            label.Position ??= Position;
            label.EdgeLabelPlacement = EdgeLabelPlacement;
            return label;
        }

        public Func<T, string, ValidationStatus> Validator { get; set; }

        public override ValidationStatus Validate(string text, GElement element)
        {
            if (Validator != null && element.CreatedFrom is T semanticElement)
            {
                return Validator(semanticElement, text);
            }
            return null;
        }
    }
}
