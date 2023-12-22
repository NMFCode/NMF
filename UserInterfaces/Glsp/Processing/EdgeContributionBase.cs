using NMF.Glsp.Graph;
using NMF.Glsp.Notation;
using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Protocol.Modification;
using NMF.Glsp.Protocol.Types;
using System;
using System.Collections.Generic;

namespace NMF.Glsp.Processing
{

    internal abstract class EdgeContributionBase<T>
    {
        public string ContributionId { get; } = Guid.NewGuid().ToString();

        public abstract void Contribute(T input, GElement element, ISkeletonTrace trace);

        public abstract LabeledAction CreateAction(GElement item, List<GElement> selected, string contextId, EditorContext editorContext);

        public virtual GElement CreateEdge(GElement sourceElement, GElement targetElement, INotationElement parentNotation, CreateEdgeOperation createEdgeOperation, ISkeletonTrace trace)
        {
            throw new NotSupportedException();
        }

        public abstract Type SourceType { get; }

        public abstract Type TargetType { get; }

        public abstract IEnumerable<EdgeTypeHint> CreateEdgeTypesHint();
    }
}
