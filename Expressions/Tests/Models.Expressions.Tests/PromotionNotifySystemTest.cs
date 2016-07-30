using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Models;
using NMF.Models.Repository;
using NMF.Models.Tests.Railway;
using NMF.Expressions.Linq;

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
