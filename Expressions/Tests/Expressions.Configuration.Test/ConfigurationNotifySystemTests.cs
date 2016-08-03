using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NMF.Expressions.Tests
{
    [TestClass]
    public class ConfigurationNotifySystemTests : NotifySystemTests
    {
        protected override INotifySystem CreateNotifySystem()
        {
            return new ConfiguredNotifySystem(Repository, new IncrementalizationConfiguration.Configuration());
        }
    }
}
