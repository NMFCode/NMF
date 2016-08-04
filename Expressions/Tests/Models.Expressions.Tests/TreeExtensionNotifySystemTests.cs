using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
