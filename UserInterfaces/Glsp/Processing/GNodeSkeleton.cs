using NMF.Glsp.Graph;
using NMF.Glsp.Language;
using NMF.Glsp.Notation;
using NMF.Glsp.Protocol.Types;
using NMF.Models;
using System;

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

                shape.BubbledChange += (o, e) => UpdateNodePositionAndSize(node, e.ChangeType);
            }
            return node;
        }

        private void UpdateNodePositionAndSize(GElement node, ChangeType changeType)
        {
            if (changeType == ChangeType.PropertyChanged)
            {
                var shape = node.NotationElement as IShape;
                if (shape != null)
                {
                    if (shape.Position != null)
                    {
                        node.Position = new Point(shape.Position.X, shape.Position.Y);
                    }
                    else
                    {
                        node.Position = null;
                    }
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
                if (shape.Position != null)
                {
                    shape.Position.X = element.Position.Value.X;
                    shape.Position.Y = element.Position.Value.Y;
                }
                else
                {
                    shape.Position = new GPoint
                    {
                        X = element.Position.Value.X,
                        Y = element.Position.Value.Y
                    };
                }
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
                if (shape.Size != null)
                {
                    shape.Size.Width = element.Size.Value.Width;
                    shape.Size.Height = element.Size.Value.Height;
                }
                else
                {
                    shape.Size = new GDimension
                    {
                        Width = element.Size.Value.Width,
                        Height = element.Size.Value.Height
                    };
                }
            }
            else
            {
                shape.Size = null;
            }
        }
    }
}
