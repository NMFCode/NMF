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
    public class SwitchSensor : TrainCase<Switch, Switch>
    {
        public override Func<RailwayContainer, INotifyEnumerable<Switch>> Query =>
            rc => rc.Descendants().OfType<Switch>().Where(sw => sw.Sensor == null).AsNotifiable();

        public override Action<Switch> Repair =>
            sw => sw.Sensor = new Sensor();

        public override Func<RailwayContainer, IEnumerable<Switch>> InjectSelector =>
            rc => rc.Descendants().OfType<Switch>().Where(sw => sw.Sensor != null);

        public override Action<Switch> Inject =>
            sw => sw.Sensor = null;
    }
}
