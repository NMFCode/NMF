using NMF.Glsp.Graph;
using NMF.Glsp.Protocol.Modification;
using NMF.Glsp.Protocol.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace NMF.Glsp.Processing
{
    internal abstract class GElementSkeletonBase
    {
        public abstract IEnumerable<LabeledAction> SuggestActions(GElement item, List<GElement> selected, string contextId, EditorContext editorContext);

        public abstract bool IsApplicable(object input);

        public abstract bool TryApply(object input, ISkeletonTrace trace, GElement element);

        public abstract void CreateNode(GElement container, CreateNodeOperation createNodeOperation);

        public abstract bool CanCreateInstance { get; }

        public abstract object CreateInstance();

        public abstract void CreateEdge(GElement sourceElement, CreateEdgeOperation createEdgeOperation, GElement targetElement, ISkeletonTrace trace);

        public List<GElementSkeletonBase> Refinements { get; } = new List<GElementSkeletonBase>();

        public List<GElementSkeletonBase> PossibleParents { get; } = new List<GElementSkeletonBase>();

        public abstract string TypeName { get; }

        public virtual string ElementTypeId => TypeName;

        public virtual string[] CalculateSourceTypeIds() { return null; }

        public virtual string[] CalculateTargetTypeIds() { return null; }
    }
}
