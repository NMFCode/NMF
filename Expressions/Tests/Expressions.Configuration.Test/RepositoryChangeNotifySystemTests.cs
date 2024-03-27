using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NMF.Expressions.Tests
{
    [TestClass]
    public class RepositoryChangeNotifySystemTests : NotifySystemTests
    {
        protected override INotifySystem CreateNotifySystem()
        {
            return new RepositoryChangeNotificationSystem(Repository);
        }
    }
}
