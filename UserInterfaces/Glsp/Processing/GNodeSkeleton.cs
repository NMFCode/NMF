using NMF.Glsp.Graph;
using NMF.Glsp.Language;
using NMF.Glsp.Notation;
using NMF.Glsp.Protocol.Types;
using NMF.Models;

namespace NMF.Glsp.Processing
{
    internal class GNodeSkeleton<T> : GElementSkeleton<T>
    {
        public GNodeSkeleton(ElementDescriptor<T> elementDescriptor) : base(elementDescriptor)
        {
            Dimension = new Dimension(60, 30);
        }

        protected override GElement CreateElement(T input, ISkeletonTrace trace, ref INotationElement notation)
        {
            var node = new GElement(notation?.Id);
            node.Type = DefaultTypes.Node;
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
            node.Position = new Point(0, 0);
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
