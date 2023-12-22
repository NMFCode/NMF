using NMF.Glsp.Processing;
using NMF.Glsp.Protocol.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Language
{
    internal class LabelSyntax<T> : ILabelSyntax<T>
    {
        private readonly GLabelSkeleton<T> _skeleton;
        private CompartmentContribution<T> _contribution;

        public LabelSyntax(GLabelSkeleton<T> skeleton, CompartmentContribution<T> contribution)
        {
            _skeleton = skeleton;
            _contribution = contribution;
        }

        public ILabelSyntax<T> At(double x, double y)
        {
            _skeleton.Position = new Point(x, y);
            return this;
        }

        public ILabelSyntax<T> If(Expression<Func<T, bool>> guard)
        {
            _contribution.Guard = guard;
            return this;
        }

        public ILabelSyntax<T> WithType(string type)
        {
            _skeleton.Type = type ?? _skeleton.Type;
            return this;
        }
    }
}
