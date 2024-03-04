using NMF.Glsp.Graph;
using NMF.Glsp.Language.Layouting;
using NMF.Glsp.Protocol.Modification;
using NMF.Glsp.Protocol.Selection;
using NMF.Glsp.Protocol.Types;
using NMF.Glsp.Protocol.Validation;
using NMF.Glsp.Server.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace NMF.Glsp.Processing
{
    internal abstract class GElementSkeletonBase
    {
        public abstract bool IsEmbedding(GElementSkeletonBase parent);

        public LayoutStrategy LayoutStrategy { get; set; } = AbsolutePositioningStrategy.Instance;

        public abstract IEnumerable<LabeledAction> SuggestActions(GElement item, List<GElement> selected, string contextId, EditorContext editorContext);

        public abstract bool IsApplicable(object input);

        public abstract bool TryApply(object input, ISkeletonTrace trace, GElement element);

        public abstract GElement CreateNode(GElement container, CreateNodeOperation createNodeOperation);

        public abstract bool CanCreateInstance { get; }

        public abstract object CreateInstance(string profile, object parent);

        public abstract string GetToolLabel(string profile);

        public abstract void CreateEdge(GElement sourceElement, CreateEdgeOperation createEdgeOperation, GElement targetElement, ISkeletonTrace trace);

        public List<GElementSkeletonBase> Refinements { get; } = new List<GElementSkeletonBase>();

        public abstract IEnumerable<string> Profiles { get; }

        public abstract string TypeName { get; }

        public virtual string ElementTypeId => TypeName;

        public abstract GGraph CreatePopup(RequestPopupModelAction popupRequest, GElement element);

        public virtual string[] CalculateSourceTypeIds() { return null; }

        public virtual string[] CalculateTargetTypeIds() { return null; }

        public virtual ValidationStatus Validate(string text, GElement element) { return null; }

        public abstract Task Perform(string kind, GElement gElement, IGlspSession session, IDictionary<string, object> args);

        public virtual bool IsLabel => false;

        public string Type { get; set; }
    }
}
