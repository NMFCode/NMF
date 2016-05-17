using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Models.Evolution;
using NMF.Models.Evolution.Minimizing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models.Tests.Evolution
{
    [TestClass]
    public class MultiplePropertyChangesTests
    {
        [TestMethod]
        public void Minimize3To1()
        {
            var uri = new Uri("//TestUri");
            var property = "TestProperty";
            var changes = new List<IModelChange>
            {
                new PropertyChange(uri, property, 2, 1),
                new PropertyChange(uri, property, 3, 2),
                new PropertyChange(uri, property, 4, 3)
            };
            var strategy = new MultiplePropertyChanges();
            var expected = new List<IModelChange>() { new PropertyChange(uri, property, 4, 1) };

            var actual = strategy.Execute(changes);

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Minimize4To2()
        {
            var uri = new Uri("//TestUri");
            var property = "TestProperty";
            var property2 = "SomeOtherProperty";
            var changes = new List<IModelChange>
            {
                new PropertyChange(uri, property, 2, 1),
                new PropertyChange(uri, property2, "hi", "hello"),
                new PropertyChange(uri, property2, "welcome", "hi"),
                new PropertyChange(uri, property, 3, 2)
            };
            var strategy = new MultiplePropertyChanges();
            var expected = new List<IModelChange>()
            {
                new PropertyChange(uri, property, 3, 1),
                new PropertyChange(uri, property2, "welcome", "hello"),
            };

            var actual = strategy.Execute(changes);

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
