using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Tests.MSTest;
using NMF.Models.Tests.Railway;

namespace NMF.Models.Tests
{
    [TestClass]
    public class RouteTests : ModelTest<Route> { }

    [TestClass]
    public class SegmentTests : ModelTest<Segment> { }

    [TestClass]
    public class SemaphoreTests : ModelTest<Semaphore> { }

    [TestClass]
    public class SwitchPositionTests : ModelTest<SwitchPosition> { }
}
