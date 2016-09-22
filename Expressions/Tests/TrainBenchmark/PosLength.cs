using NMF.Models;
using NMF.Models.Tests.Railway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMF.Expressions;
using NMF.Expressions.Linq;

namespace TrainBenchmark
{
    public class PosLength : TrainCase<Segment, Segment>
    {
        public override Func<RailwayContainer, INotifyEnumerable<Segment>> Query =>
            rc => rc.Descendants().OfType<Segment>().Where(seg => seg.Length <= 0).AsNotifiable();

        public override Action<Segment> Repair =>
            segment => segment.Length = -segment.Length + 1;

        public override Func<RailwayContainer, INotifyEnumerable<Segment>> InjectSelector =>
            rc => rc.Descendants().OfType<Segment>().Where(seg => seg.Length > 0).AsNotifiable();

        public override Action<Segment> Inject =>
            segment => segment.Length = 0;
    }
}
