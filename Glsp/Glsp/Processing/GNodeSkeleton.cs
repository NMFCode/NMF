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
            var node = new GNode(notation?.Id);
            node.Type = DefaultTypes.Node;
            var shape = notation as IShape;
            if (shape == null)
            {
                if (input is IModelElement semanticElement)
                {
                    shape = new Shape
                    {
                        Id = node.Id,
                        SemanticElement = semanticElement,
                    };

                    notation = shape;
                }
                node.Position = new Point(0, 0);
            }
            else
            {
                if (shape.Position != null)
                {
                    node.Position = new Point(shape.Position.X, shape.Position.Y);
                }
                if (shape.Size != null)
                {
                    node.Size = new Dimension(shape.Size.Width, shape.Size.Height);
                }
            }
            if (shape != null)
            {
                node.SizeChanged += UpdateSize;
                node.PositionChanged += UpdatePosition;
                node.IsManualLayout = true;

                shape.BubbledChange += (o, e) => UpdateNodePositionAndSize(node, e);
            }
            return node;
        }

        private void UpdateNodePositionAndSize(GElement node, BubbledChangeEventArgs e)
        {
            if (e.ChangeType == ChangeType.PropertyChanged && node.NotationElement is IShape shape)
            {
                if (e.PropertyName == nameof(IShape.Position))
                {
                    if (shape.Position != null)
                    {
                        node.Position = new Point(shape.Position.X, shape.Position.Y);
                    }
                    else
                    {
                        node.Position = null;
                    }
                }
                if (e.PropertyName == nameof(IShape.Size))
                {
                    if (shape.Size != null)
                    {
                        node.Size = new Dimension(shape.Size.Width, shape.Size.Height);
                    }
                    else
                    {
                        node.Size = null;
                    }
                }
            }
        }

        private void UpdatePosition(GElement element)
        {
            var shape = (IShape)element.NotationElement;
            if (shape == null) return;
            if (element.Position != null)
            {
                var x = element.Position.Value.X;
                var y = element.Position.Value.Y;
                shape.Position = new GPoint
                {
                    X = x,
                    Y = y
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
                var width = element.Size.Value.Width;
                var height = element.Size.Value.Height;
                shape.Size = new GDimension
                {
                    Width = width,
                    Height = height
                };
            }
            else
            {
                shape.Size = null;
            }
        }
    }
}
