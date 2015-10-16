using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NMF.Expressions.Linq.Tests
{
    [TestClass]
    public class ModelSelectTests : SelectTest
    {
        [TestInitialize]
        public virtual void Setup()
        {
            NotifySystem.DefaultSystem = new GeneralizingNotifySystem(new InstructionLevelNotifySystem());
        }
    }
}
