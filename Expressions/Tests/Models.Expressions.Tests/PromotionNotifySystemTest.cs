using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Models;
using NMF.Models.Repository;
using NMF.Models.Tests.Railway;
using NMF.Expressions.Linq;

namespace NMF.Expressions.Tests
{
    [TestClass]
    public class PromotionNotifySystemTest
    {
        ModelRepository repository;
        Model railwayModel;
        RailwayContainer rc;
        INotifySystem oldSystem;

        [TestInitialize]
        public void LoadRailwayModel()
        {
            repository = new ModelRepository();
            railwayModel = repository.Resolve("railway.railway");
            rc = railwayModel.RootElements[0] as RailwayContainer;

            oldSystem = NotifySystem.DefaultSystem;
            NotifySystem.DefaultSystem = new PromotionNotifySystem();
        }

        [TestCleanup]
        public void Teardown()
        {
            NotifySystem.DefaultSystem = oldSystem;
        }
        [TestMethod]
        public void Test_Promotion_CheckEntrySemaphore()
        {
            ObservingFunc<IRoute, bool> func = new ObservingFunc<IRoute, bool>(r => r.Entry != null && r.Entry.Signal == Signal.GO);

            var route = rc.Routes[0];
            var test = func.Observe(route);
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
        public void Test_Promotion_CheckSwitchPosition()
        {
            ObservingFunc<ISwitchPosition, bool> func = new ObservingFunc<ISwitchPosition, bool>(swP => swP.Switch.CurrentPosition == swP.Position);
            
            var switchPosition = rc.Routes[0].Follows.OfType<ISwitchPosition>().FirstOrDefault();
            var test = func.Observe(switchPosition);
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
        public void Test_Promotion_CheckSwitchPositionSensor()
        {
            ObservingFunc<IRoute, ISwitchPosition, bool> func = new ObservingFunc<IRoute, ISwitchPosition, bool>(
                (r, swP) => swP.Switch.Sensor != null && !r.DefinedBy.Contains(swP.Switch.Sensor));

            var route = rc.Routes[0];
            var route2 = rc.Invalids.OfType<IRoute>().FirstOrDefault();
            var switchPosition = route.Follows.OfType<ISwitchPosition>().FirstOrDefault();
            var test = func.Observe(route, switchPosition);
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
        public void Test_Promotion_SwitchSet()
        {
            ObservingFunc<RailwayContainer, IEnumerableExpression<SwitchPosition>> func = new ObservingFunc<RailwayContainer, IEnumerableExpression<SwitchPosition>>(
                rc =>
                from route in rc.Routes
                where route.Entry != null && route.Entry.Signal == Signal.GO
                from swP in route.Follows.OfType<SwitchPosition>()
                where swP.Switch.CurrentPosition != swP.Position
                select swP);

            var switchPosition = rc.Routes[0].Follows.OfType<ISwitchPosition>().FirstOrDefault();
            var test = func.Observe(rc);
            var resultChanged = false;
            test.ValueChanged += (o, e) =>
            {
                resultChanged = true;
            };
        }

        [TestMethod]
        public void Test_Promotion_PosLength()
        {
            ObservingFunc<RailwayContainer, IEnumerableExpression<ISegment>> func = new ObservingFunc<RailwayContainer, IEnumerableExpression<ISegment>>(
                rc =>
                from seg in rc.Invalids.OfType<Segment>()
                where seg.Length <= 0
                select seg);
            
            var test = func.Observe(rc);
            var resultChanged = false;
            var expectedOld = true;
            var expectedNew = false;
            test.ValueChanged += (o, e) =>
            {
                resultChanged = true;
                Assert.AreEqual(expectedOld, e.OldValue);
                Assert.AreEqual(expectedNew, e.NewValue);
            };
        }
    }
}
