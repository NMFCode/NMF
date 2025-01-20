using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NMF.Expressions.Tests
{
    [TestClass]
    public class RecordingNotifySystemTests
    {
        private InnerTests inner;
        private RecordingNotifySystem recorder;

        [TestInitialize]
        public void LoadInnerTests()
        {
            recorder = new RecordingNotifySystem(InstructionLevelNotifySystem.Instance);
            inner = new InnerTests(recorder);
            inner.LoadRailwayModel();
        }

        [TestCleanup]
        public void Teardown()
        {
            inner.Teardown();
        }


        [TestMethod]
        public void RecordingNotifySystem_CheckEntrySemaphore()
        {
            inner.NotifySystem_CheckEntrySemaphore();
            Assert.AreEqual(1, recorder.Configuration.MethodConfigurations.Count);
        }

        [TestMethod]
        public void RecordingNotifySystem_CheckEntrySemaphore_Struct()
        {
            inner.NotifySystem_CheckEntrySemaphore_Struct();
            Assert.AreEqual(1, recorder.Configuration.MethodConfigurations.Count);
        }

        [TestMethod]
        public void RecordingNotifySystem_CheckEntrySemaphore_Generated()
        {
            inner.NotifySystem_CheckEntrySemaphore_Generated();
            Assert.AreEqual(1, recorder.Configuration.MethodConfigurations.Count);
        }

        [TestMethod]
        public void RecordingNotifySystem_CheckSwitchPosition()
        {
            inner.NotifySystem_CheckSwitchPosition();
            Assert.AreEqual(1, recorder.Configuration.MethodConfigurations.Count);
        }

        [TestMethod]
        public void RecordingNotifySystem_CheckSwitchPosition_Struct()
        {
            inner.NotifySystem_CheckSwitchPosition_Struct();
            Assert.AreEqual(1, recorder.Configuration.MethodConfigurations.Count);
        }

        [TestMethod]
        public void RecordingNotifySystem_CheckSwitchPosition_Generated()
        {
            inner.NotifySystem_CheckSwitchPosition_Generated();
            Assert.AreEqual(1, recorder.Configuration.MethodConfigurations.Count);
        }

        [TestMethod]
        public void RecordingNotifySystem_CheckSwitchPositionSensor()
        {
            inner.NotifySystem_CheckSwitchPositionSensor();
            Assert.AreEqual(1, recorder.Configuration.MethodConfigurations.Count);
        }

        [TestMethod]
        public void RecordingNotifySystem_SwitchSet()
        {
            inner.NotifySystem_SwitchSet_Full();
            Assert.AreEqual(6, recorder.Configuration.MethodConfigurations.Count);
        }

        [TestMethod]
        public void RecordingNotifySystem_PosLength()
        {
            inner.NotifySystem_PosLength();
            Assert.AreEqual(2, recorder.Configuration.MethodConfigurations.Count);
        }

        private class InnerTests : NotifySystemTests
        {
            private readonly RecordingNotifySystem notifySystem;

            public InnerTests(RecordingNotifySystem system)
            {
                notifySystem = system;
            }

            protected override INotifySystem CreateNotifySystem()
            {
                return notifySystem;
            }
        }
    }
}
