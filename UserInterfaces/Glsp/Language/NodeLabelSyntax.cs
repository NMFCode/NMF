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
    internal class NodeLabelSyntax<T> : INodeLabelSyntax<T>
    {
        private readonly GLabelSkeleton<T> _skeleton;
        private CompartmentContribution<T> _contribution;

        public NodeLabelSyntax(GLabelSkeleton<T> skeleton, CompartmentContribution<T> contribution)
        {
            _skeleton = skeleton;
            _contribution = contribution;
        }

        public INodeLabelSyntax<T> At(double x, double y)
        {
            _skeleton.Position = new Point(x, y);
            return this;
        }

        public INodeLabelSyntax<T> If(Expression<Func<T, bool>> guard)
        {
            _contribution.Guard = guard;
            return this;
        }

        public INodeLabelSyntax<T> Validate(Func<T, string, ValidationStatus> validator)
        {
            _skeleton.Validator = validator;
            return this;
        }

        public INodeLabelSyntax<T> WithSetter(Action<T, string> setter)
        {
            _skeleton.CustomSetter = setter;
            _skeleton.CanEdit = setter != null;
            return this;
        }

        public INodeLabelSyntax<T> WithType(string type)
        {
            _skeleton.Type = type ?? _skeleton.Type;
            return this;
        }
    }
}
