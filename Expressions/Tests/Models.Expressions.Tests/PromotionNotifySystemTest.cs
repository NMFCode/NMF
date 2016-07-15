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
        INotifySystem oldSystem;

        [TestInitialize]
        public void LoadRailwayModel()
        {
            repository = new ModelRepository();
            railwayModel = repository.Resolve("railway.railway");

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
            ObservingFunc<Route, bool> test = new ObservingFunc<Route, bool>(r => r.Entry != null && r.Entry.Signal == Signal.GO);
        }

        [TestMethod]
        public void Test_Promotion_CheckSwitchPosition()
        {
            ObservingFunc<SwitchPosition, bool> test = new ObservingFunc<SwitchPosition, bool>(swP => swP.Switch.CurrentPosition == swP.Position);
        }

        [TestMethod]
        public void Test_Promotion_CheckSwitchPositionSensor()
        {
            ObservingFunc<Route, SwitchPosition, bool> test = new ObservingFunc<Route, SwitchPosition, bool>(
                (r, swP) => swP.Switch.Sensor != null && !r.DefinedBy.Contains(swP.Switch.Sensor));
        }

        [TestMethod]
        public void Test_Promotion_SwitchSet()
        {
            ObservingFunc<RailwayContainer, IEnumerableExpression<SwitchPosition>> test = new ObservingFunc<RailwayContainer, IEnumerableExpression<SwitchPosition>>(
                rc =>
                from route in rc.Routes
                where route.Entry != null && route.Entry.Signal == Signal.GO
                from swP in route.Follows.OfType<SwitchPosition>()
                where swP.Switch.CurrentPosition != swP.Position
                select swP);
        }

        //[TestMethod]
        public void Test_Promotion_PosLength()
        {
            ObservingFunc<RailwayContainer, IEnumerableExpression<ISegment>> test = new ObservingFunc<RailwayContainer, IEnumerableExpression<ISegment>>(
                rc =>
                from seg in rc.Invalids.OfType<Segment>()
                where seg.Length <= 0
                select seg);
        }
    }
}
