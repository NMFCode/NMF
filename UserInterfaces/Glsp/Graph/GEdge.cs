using NMF.Collections.ObjectModel;
using NMF.Expressions;
using NMF.Glsp.Server.Protocol.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NMF.Glsp.Graph
{
    public class GEdge : GElement
    {
        public string SourceId { get; set; }

        public string TargetId { get; set; }

        public event Action SourceIdChanged;

        public event Action TargetIdChanged;

        [JsonIgnore]
        public bool SupportsChangingSourceId => SourceIdChanged != null;

        [JsonIgnore]
        public bool SupportsChangingTargetId => TargetIdChanged != null;

        public IListExpression<Point> RoutingPoints { get; } = new ObservableList<Point>();
    }
}
