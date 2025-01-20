using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NMF.Expressions.Tests
{
    [TestClass]
    public class TreeExtensionNotifySystemTests : NotifySystemTests
    {
        protected override INotifySystem CreateNotifySystem()
        {
            return new TreeExtensionNotifySystem();
        }
    }
}
