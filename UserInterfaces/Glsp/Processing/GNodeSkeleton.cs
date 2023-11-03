using NMF.Glsp.Graph;
using NMF.Glsp.Notation;
using NMF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Processing
{
    internal class GNodeSkeleton<T> : GElementSkeleton<T>
    {
        protected override GElement CreateElement(T input, ISkeletonTrace trace, ref INotationElement notation)
        {
            var node = new GElement(notation?.Id);
            var shape = notation as IShape;
            if (shape == null && input is IModelElement semanticElement)
            {
                shape = new Shape
                {
                    Id = node.Id,
                    SemanticElement = semanticElement,
                };

                notation = shape;
            }
            if (shape != null)
            {
                node.SizeChanged += UpdateSize;
                node.PositionChanged += UpdatePosition;
            }

            return node;
        }

        private void UpdatePosition(GElement element)
        {
            var shape = (IShape)element.NotationElement;
            if (shape == null) return;
            if (element.Position != null)
            {
                shape.Position = new GPoint
                {
                    X = element.Position.Value.X,
                    Y = element.Position.Value.Y
                };
            }
            else
            {
                shape.Position = null;
            }
        }

        private void UpdateSize(GElement element)
        {
            var shape = (IShape)element.NotationElement;
            if (shape == null) return;
            if (element.Size != null)
            {
                shape.Size = new GDimension
                {
                    Width = element.Size.Value.Width,
                    Height = element.Size.Value.Height
                };
            }
            else
            {
                shape.Size = null;
            }
        }
    }
}
