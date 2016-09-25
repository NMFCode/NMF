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
    public class SwitchSensor : TrainCase<Switch, Switch>
    {
        public override Func<RailwayContainer, INotifyEnumerable<Switch>> Query =>
            rc => rc.Descendants().OfType<Switch>().Where(sw => sw.Sensor == null).AsNotifiable();

        public override Action<Switch> Repair =>
            sw =>
            {
                sw.Sensor = new Sensor();
                ((RailwayContainer)sw.ConnectsTo[0].Model.RootElements[0]).Invalids.Remove(sw);
            };

        public override Func<RailwayContainer, INotifyEnumerable<Switch>> InjectSelector =>
            rc => rc.Descendants().OfType<Switch>().Where(sw => sw.Sensor != null).AsNotifiable();

        public override Action<Switch> Inject =>
            sw =>
            {
                ((RailwayContainer)sw.ConnectsTo[0].Model.RootElements[0]).Invalids.Add(sw);
                sw.Sensor = null;
            };
    }
}
