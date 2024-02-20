using NMF.Glsp.Processing;
using NMF.Glsp.Protocol.Notification;
using NMF.Glsp.Protocol.Types;
using NMF.Glsp.Protocol.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NMF.Glsp.Language
{
    internal class EdgeLabelSyntax<T> : IEdgeLabelSyntax<T>
    {
        private readonly GLabelSkeleton<T> _skeleton;
        private CompartmentContribution<T> _contribution;

        public EdgeLabelSyntax(GLabelSkeleton<T> skeleton, CompartmentContribution<T> contribution)
        {
            _skeleton = skeleton;
            _contribution = contribution;
        }

        public IEdgeLabelSyntax<T> If(Expression<Func<T, bool>> guard)
        {
            _contribution.Guard = guard;
            return this;
        }

        public IEdgeLabelSyntax<T> WithType(string type)
        {
            _skeleton.Type = type ?? _skeleton.Type;
            return this;
        }
        public IEdgeLabelSyntax<T> At(double pos, EdgeSide side, bool rotate = false)
        {
            _skeleton.EdgeLabelPlacement = new EdgeLabelPlacement(pos, rotate, ConvertSide(side), _skeleton.EdgeLabelPlacement.MoveMode);
            return this;
        }

        public IEdgeLabelSyntax<T> MoveMode(EdgeMoveMode mode)
        {
            _skeleton.EdgeLabelPlacement.Deconstruct(out var position, out var rotate, out var side, out _);
            _skeleton.EdgeLabelPlacement = new EdgeLabelPlacement(position, rotate, side, ConvertMoveMode(mode));
            return this;
        }

        private string ConvertMoveMode(EdgeMoveMode mode)
        {
            switch (mode)
            {
                case EdgeMoveMode.None: return "none";
                case EdgeMoveMode.Edge: return "edge";
                default: return "free";
            }
        }

        private string ConvertSide(EdgeSide side)
        {
            switch (side)
            {
                case EdgeSide.Left: return "left";
                case EdgeSide.Right: return "right";
                case EdgeSide.Top: return "top";
                case EdgeSide.Bottom: return "bottom";
                default: return "on";
            }
        }

        public IEdgeLabelSyntax<T> Validate(Func<T, string, ValidationStatus> validator)
        {
            _skeleton.Validator = validator;
            return this;
        }

        public IEdgeLabelSyntax<T> WithSetter(Action<T, string> setter)
        {
            _skeleton.CustomSetter = setter;
            _skeleton.CanEdit = setter != null;
            return this;
        }
    }
}
