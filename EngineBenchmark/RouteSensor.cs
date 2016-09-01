using NMF.Models;
using NMF.Models.Tests.Railway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMF.Expressions;
using NMF.Expressions.Linq;

namespace EngineBenchmark
{
    public class RouteSensor : TrainCase<Tuple<IRoute, ISensor>, Switch>
    {
        public override Func<RailwayContainer, INotifyEnumerable<Tuple<IRoute, ISensor>>> Query =>
            rc => (from route in rc.Routes.Concat(rc.Invalids.OfType<Route>())
                   from swP in route.Follows.OfType<SwitchPosition>()
                   where swP.Switch.Sensor != null && !route.DefinedBy.Contains(swP.Switch.Sensor)
                   select new Tuple<IRoute, ISensor>(route, swP.Switch.Sensor)).AsNotifiable();

        public override Action<Tuple<IRoute, ISensor>> Repair =>
            match => match.Item1.DefinedBy.Add(match.Item2);

        public override Func<RailwayContainer, INotifyEnumerable<Switch>> InjectSelector =>
            rc => rc.Descendants().OfType<Switch>().AsNotifiable();

        public override Action<Switch> Inject =>
            sw => sw.Sensor = null;
    }
}
