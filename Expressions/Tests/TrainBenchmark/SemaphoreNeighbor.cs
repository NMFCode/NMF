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
    public class SemaphoreNeighbor : TrainCase<Tuple<IRoute, IRoute>, IRoute>
    {
        public override Func<RailwayContainer, INotifyEnumerable<Tuple<IRoute, IRoute>>> Query =>
            rc => from route1 in rc.Routes.WithUpdates().Concat(rc.Invalids.OfType<Route>())
                  from route2 in rc.Routes.WithUpdates().Concat(rc.Invalids.OfType<Route>())
                  where route1 != route2 && route2.Entry != route1.Exit
                  from sensor1 in route1.DefinedBy
                  from te1 in sensor1.Elements
                  from te2 in te1.ConnectsTo
                  where te2.Sensor == null || route2.DefinedBy.Contains(te2.Sensor)
                  select new Tuple<IRoute, IRoute>(route2, route1);

        public override Action<Tuple<IRoute, IRoute>> Repair =>
            match => match.Item1.Entry = match.Item2.Exit;

        public override Func<RailwayContainer, INotifyEnumerable<IRoute>> InjectSelector =>
            rc => rc.Routes.Concat(rc.Invalids.OfType<Route>()).Where(r => r.Entry != null).AsNotifiable();

        public override Action<IRoute> Inject =>
            route => route.Entry = null;
    }
}
