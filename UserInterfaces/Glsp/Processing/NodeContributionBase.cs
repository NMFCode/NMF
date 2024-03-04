using NMF.Glsp.Graph;
using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Protocol.Modification;
using NMF.Glsp.Protocol.Types;
using System;
using System.Collections.Generic;

namespace NMF.Glsp.Processing
{
    internal abstract class NodeContributionBase<T> : ActionElement
    {
        public string ContributionId { get; } = Guid.NewGuid().ToString();

        public abstract void Contribute(T input, GElement element, ISkeletonTrace trace);

        public abstract IEnumerable<LabeledAction> SuggestActions(GElement item, GElementSkeletonBase skeleton, ICollection<GElement> selected, string contextId, EditorContext editorContext);

        public virtual GElement CreateNode(GElement container, CreateNodeOperation operation)
        {
            throw new NotSupportedException();
        }

        public abstract IEnumerable<string> ContainableElementIds();

        public abstract Type SourceType { get; }

        public abstract Type TargetType { get; }
    }
}
