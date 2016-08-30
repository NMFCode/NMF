using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Expressions.Linq;
using NMF.Models;
using NMF.Models.Repository;
using NMF.Models.Tests.Railway;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Tests
{
    public abstract class NotifySystemTests
    {
        private struct Struct<T>
        {
            public Struct(T item) : this()
            {
                Item = item;
            }

            public T Item { get; private set; }
        }

        [GeneratedCode("Foo", "0.0")]
        private class Generated<T>
        {
            public Generated(T item)
            {
                Item = item;
            }

            public T Item { get; private set; }
        }

        public ModelRepository Repository { get; private set; }
        public Model RailwayModel { get; private set; }
        public RailwayContainer RailwayContainer { get; private set; }

        private INotifySystem oldSystem;

        [TestInitialize]
        public void LoadRailwayModel()
        {
            Repository = new ModelRepository();
            RailwayModel = Repository.Resolve("railway.railway");
            RailwayContainer = RailwayModel.RootElements[0] as RailwayContainer;

            oldSystem = NotifySystem.DefaultSystem;
            NotifySystem.DefaultSystem = CreateNotifySystem();
        }

        protected abstract INotifySystem CreateNotifySystem();

        [TestCleanup]
        public void Teardown()
        {
            NotifySystem.DefaultSystem = oldSystem;
        }
        [TestMethod]
        public void NotifySystem_CheckEntrySemaphore()
        {
            ObservingFunc<IRoute, bool> func = new ObservingFunc<IRoute, bool>(r => r.Entry != null && r.Entry.Signal == Signal.GO);

            var route = RailwayContainer.Routes[0];
            var test = func.Observe(route);
            test.Successors.Add(null);
            var resultChanged = false;
            test.ValueChanged += (o, e) =>
            {
                resultChanged = true;
                Assert.AreEqual(true, e.OldValue);
                Assert.AreEqual(false, e.NewValue);
            };

            Assert.IsTrue(test.Value);
            Assert.IsFalse(resultChanged);

            route.Entry.Signal = Signal.STOP;

            Assert.IsTrue(resultChanged);
            Assert.IsFalse(test.Value);
        }

        [TestMethod]
        public void NotifySystem_CheckEntrySemaphore_Struct()
        {
            ObservingFunc<Struct<IRoute>, bool> func = new ObservingFunc<Struct<IRoute>, bool>(r => r.Item.Entry != null && r.Item.Entry.Signal == Signal.GO);

            var route = RailwayContainer.Routes[0];
            var test = func.Observe(new Struct<IRoute>(route));
            test.Successors.Add(null);
            var resultChanged = false;
            test.ValueChanged += (o, e) =>
            {
                resultChanged = true;
                Assert.AreEqual(true, e.OldValue);
                Assert.AreEqual(false, e.NewValue);
            };

            Assert.IsTrue(test.Value);
            Assert.IsFalse(resultChanged);

            route.Entry.Signal = Signal.STOP;

            Assert.IsTrue(resultChanged);
            Assert.IsFalse(test.Value);
        }

        [TestMethod]
        public void NotifySystem_CheckEntrySemaphore_Generated()
        {
            ObservingFunc<Generated<IRoute>, bool> func = new ObservingFunc<Generated<IRoute>, bool>(r => r.Item.Entry != null && r.Item.Entry.Signal == Signal.GO);

            var route = RailwayContainer.Routes[0];
            var test = func.Observe(new Generated<IRoute>(route));
            test.Successors.Add(null);
            var resultChanged = false;
            test.ValueChanged += (o, e) =>
            {
                resultChanged = true;
                Assert.AreEqual(true, e.OldValue);
                Assert.AreEqual(false, e.NewValue);
            };

            Assert.IsTrue(test.Value);
            Assert.IsFalse(resultChanged);

            route.Entry.Signal = Signal.STOP;

            Assert.IsTrue(resultChanged);
            Assert.IsFalse(test.Value);
        }

        [TestMethod]
        public void NotifySystem_CheckSwitchPosition()
        {
            ObservingFunc<ISwitchPosition, bool> func = new ObservingFunc<ISwitchPosition, bool>(swP => swP.Switch.CurrentPosition == swP.Position);

            var switchPosition = RailwayContainer.Routes[0].Follows.OfType<ISwitchPosition>().FirstOrDefault();
            var test = func.Observe(switchPosition);
            test.Successors.Add(null);
            var resultChanged = false;
            var expectedOld = true;
            var expectedNew = false;
            test.ValueChanged += (o, e) =>
            {
                resultChanged = true;
                Assert.AreEqual(expectedOld, e.OldValue);
                Assert.AreEqual(expectedNew, e.NewValue);
            };

            Assert.IsTrue(test.Value);
            Assert.IsFalse(resultChanged);

            switchPosition.Switch.CurrentPosition = Position.LEFT;

            Assert.IsTrue(resultChanged);
            Assert.IsFalse(test.Value);

            resultChanged = false;
            expectedOld = false;
            expectedNew = true;

            switchPosition.Position = Position.LEFT;

            Assert.IsTrue(resultChanged);
            Assert.IsTrue(test.Value);
        }

        [TestMethod]
        public void NotifySystem_CheckSwitchPosition_Struct()
        {
            ObservingFunc<Struct<ISwitchPosition>, bool> func = new ObservingFunc<Struct<ISwitchPosition>, bool>(swP => swP.Item.Switch.CurrentPosition == swP.Item.Position);

            var switchPosition = RailwayContainer.Routes[0].Follows.OfType<ISwitchPosition>().FirstOrDefault();
            var test = func.Observe(new Struct<ISwitchPosition>(switchPosition));
            test.Successors.Add(null);
            var resultChanged = false;
            var expectedOld = true;
            var expectedNew = false;
            test.ValueChanged += (o, e) =>
            {
                resultChanged = true;
                Assert.AreEqual(expectedOld, e.OldValue);
                Assert.AreEqual(expectedNew, e.NewValue);
            };

            Assert.IsTrue(test.Value);
            Assert.IsFalse(resultChanged);

            switchPosition.Switch.CurrentPosition = Position.LEFT;

            Assert.IsTrue(resultChanged);
            Assert.IsFalse(test.Value);

            resultChanged = false;
            expectedOld = false;
            expectedNew = true;

            switchPosition.Position = Position.LEFT;

            Assert.IsTrue(resultChanged);
            Assert.IsTrue(test.Value);
        }

        [TestMethod]
        public void NotifySystem_CheckSwitchPosition_Generated()
        {
            ObservingFunc<Generated<ISwitchPosition>, bool> func = new ObservingFunc<Generated<ISwitchPosition>, bool>(swP => swP.Item.Switch.CurrentPosition == swP.Item.Position);

            var switchPosition = RailwayContainer.Routes[0].Follows.OfType<ISwitchPosition>().FirstOrDefault();
            var test = func.Observe(new Generated<ISwitchPosition>(switchPosition));
            test.Successors.Add(null);
            var resultChanged = false;
            var expectedOld = true;
            var expectedNew = false;
            test.ValueChanged += (o, e) =>
            {
                resultChanged = true;
                Assert.AreEqual(expectedOld, e.OldValue);
                Assert.AreEqual(expectedNew, e.NewValue);
            };

            Assert.IsTrue(test.Value);
            Assert.IsFalse(resultChanged);

            switchPosition.Switch.CurrentPosition = Position.LEFT;

            Assert.IsTrue(resultChanged);
            Assert.IsFalse(test.Value);

            resultChanged = false;
            expectedOld = false;
            expectedNew = true;

            switchPosition.Position = Position.LEFT;

            Assert.IsTrue(resultChanged);
            Assert.IsTrue(test.Value);
        }

        [TestMethod]
        public void NotifySystem_CheckSwitchPositionSensor()
        {
            ObservingFunc<IRoute, ISwitchPosition, bool> func = new ObservingFunc<IRoute, ISwitchPosition, bool>(
                (r, swP) => swP.Switch.Sensor != null && !r.DefinedBy.Contains(swP.Switch.Sensor));

            var route = RailwayContainer.Routes[0];
            var route2 = RailwayContainer.Invalids.OfType<IRoute>().FirstOrDefault();
            var switchPosition = route.Follows.OfType<ISwitchPosition>().FirstOrDefault();
            var test = func.Observe(route, switchPosition);
            test.Successors.Add(null);
            var resultChanged = false;
            var expectedOld = false;
            var expectedNew = true;
            test.ValueChanged += (o, e) =>
            {
                resultChanged = true;
                Assert.AreEqual(expectedOld, e.OldValue);
                Assert.AreEqual(expectedNew, e.NewValue);
            };

            Assert.IsFalse(test.Value);
            Assert.IsFalse(resultChanged);

            // For this to work, we would need to execute the model changes in a transaction
            //var originalSwitch = switchPosition.Switch;
            //var newSwitch = route2.DefinedBy[0].Elements.OfType<ISwitch>().FirstOrDefault();
            //switchPosition.Switch = newSwitch;

            //Assert.IsTrue(resultChanged);
            //Assert.IsTrue(test.Value);

            //route.DefinedBy.Remove(originalSwitch.Sensor);

            //Assert.IsFalse(resultChanged);

            //route.DefinedBy.Add(newSwitch.Sensor);

            //resultChanged = false;
            //expectedOld = true;
            //expectedNew = false;

            //switchPosition.Switch.Sensor = null;

            //Assert.IsTrue(resultChanged);
            //Assert.IsFalse(test.Value);
        }

        [TestMethod]
        public void NotifySystem_SwitchSet_Full()
        {
            ObservingFunc<RailwayContainer, IEnumerableExpression<SwitchPosition>> func = new ObservingFunc<RailwayContainer, IEnumerableExpression<SwitchPosition>>(
                rc =>
                from route in rc.Routes
                where route.Entry != null && route.Entry.Signal == Signal.GO
                from swP in route.Follows.OfType<SwitchPosition>()
                where swP.Switch.CurrentPosition != swP.Position
                select swP);

            var switchPosition = RailwayContainer.Routes[0].Follows.OfType<ISwitchPosition>().FirstOrDefault();
            var test = func.Observe(RailwayContainer);
            test.Successors.Add(null);
            var resultChanged = false;
            test.ValueChanged += (o, e) =>
            {
                resultChanged = true;
            };

            var testCollection = test.Value.AsNotifiable();
            Assert.AreEqual(3, testCollection.Count());
            Assert.IsFalse(resultChanged);
        }

        [TestMethod]
        public void NotifySystem_SwitchSet_Predicates()
        {
            var func = CreateExpression(from route in RailwayContainer.Routes
                where route.Entry != null && route.Entry.Signal == Signal.GO
                from swP in route.Follows.OfType<SwitchPosition>()
                where swP.Switch.CurrentPosition != swP.Position
                select swP);

            var switchPosition = RailwayContainer.Routes[0].Follows.OfType<ISwitchPosition>().FirstOrDefault();
            var test = func.AsNotifiable();
            var resultChanged = false;
            test.CollectionChanged += (o, e) =>
            {
                resultChanged = true;
            };
            
            Assert.AreEqual(func.Count(), test.Count());
            Assert.IsFalse(resultChanged);

            var switchP = func.FirstOrDefault();
            switchP.Switch.CurrentPosition = switchP.Position;

            Assert.IsTrue(resultChanged);
            Assert.AreEqual(func.Count(), test.Count());
        }

        [TestMethod]
        public void NotifySystem_PosLength()
        {
            ObservingFunc<RailwayContainer, IEnumerableExpression<ISegment>> func = new ObservingFunc<RailwayContainer, IEnumerableExpression<ISegment>>(
                rc =>
                from seg in rc.Descendants().OfType<Segment>()
                where seg.Length <= 0
                select seg);

            var test = func.Observe(RailwayContainer);
            test.Successors.Add(null);
            var resultChanged = false;
            test.ValueChanged += (o, e) =>
            {
                resultChanged = true;
            };

            var testCollection = test.Value.AsNotifiable();

            testCollection.CollectionChanged += (o, e) =>
            {
                resultChanged = true;
            };

            Assert.AreEqual(43, testCollection.Count());
            Assert.IsFalse(resultChanged);

            var first = test.Value.FirstOrDefault();
            first.Length = -first.Length + 1;

            Assert.AreEqual(42, testCollection.Count());
            Assert.IsTrue(resultChanged);
        }

        [TestMethod]
        public void NotifySystem_RouteSensor()
        {
            var func = CreateExpression(from route in RailwayContainer.Invalids.OfType<Route>()
                                        from swP in route.Follows.OfType<SwitchPosition>()
                                        where swP.Switch.Sensor != null && !route.DefinedBy.Contains(swP.Switch.Sensor)
                                        select new { Route = route, Sensor = swP.Switch.Sensor, SwitchPos = swP });

            var first = func.FirstOrDefault();
            var incremental = func.AsNotifiable();
            var changed = false;
            incremental.CollectionChanged += (o, e) =>
            {
                changed = true;
            };

            Assert.IsFalse(changed);
            Assert.AreEqual(func.Count(), incremental.Count());

            first.Route.DefinedBy.Add(first.Sensor);

            Assert.IsTrue(changed);
            Assert.AreEqual(func.Count(), incremental.Count());
        }

        [TestMethod]
        public void NotifySystem_SwitchSensor()
        {
            var func = CreateExpression(RailwayContainer.Descendants().OfType<Switch>().Where(sw => sw.Sensor == null));

            var first = func.FirstOrDefault();
            var incremental = func.AsNotifiable();
            var changed = false;
            incremental.CollectionChanged += (o, e) =>
            {
                changed = true;
                Assert.AreEqual(NotifyCollectionChangedAction.Remove, e.Action);
                Assert.AreEqual(1, e.OldItems.Count);
                Assert.AreEqual(first, e.OldItems[0]);
            };

            Assert.IsFalse(changed);
            Assert.AreEqual(func.Count(), incremental.Count());

            first.Sensor = new Sensor();

            Assert.IsTrue(changed);
            Assert.AreEqual(func.Count(), incremental.Count());
        }

        private IEnumerableExpression<T> CreateExpression<T>(IEnumerableExpression<T> value)
        {
            return value;
        }
    }
}
