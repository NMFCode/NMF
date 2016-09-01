using NMF.Models.Tests.Railway;
using NMF.Expressions.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMF.Expressions;
using NMF.Models;

namespace EngineBenchmark
{
    public class SwitchSet : TrainCase<SwitchPosition, Switch>
    {
        public override Func<RailwayContainer, INotifyEnumerable<SwitchPosition>> Query =>
            railway => (from route in railway.Routes.Concat(railway.Invalids.OfType<Route>())
                        where route.Entry != null && route.Entry.Signal == Signal.GO
                        from swP in route.Follows.OfType<SwitchPosition>()
                        where swP.Switch.CurrentPosition != swP.Position
                        select swP).AsNotifiable();

        public override Action<SwitchPosition> Repair =>
            swP => swP.Switch.CurrentPosition = swP.Position;

        public override Func<RailwayContainer, INotifyEnumerable<Switch>> InjectSelector =>
            rc => rc.Descendants().OfType<Switch>().AsNotifiable();

        public override Action<Switch> Inject =>
            sw => sw.CurrentPosition = ((Position)(((int)sw.CurrentPosition + 1) % 4));
    }
}
