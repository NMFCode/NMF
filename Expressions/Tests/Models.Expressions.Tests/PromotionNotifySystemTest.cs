using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NMF.Expressions.Tests
{
    [TestClass]
    public class PromotionNotifySystemTest : NotifySystemTests
    {
        protected override INotifySystem CreateNotifySystem()
        {
            return new PromotionNotifySystem();
        }
    }
}
