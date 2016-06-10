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
        private static readonly Uri uri = new Uri("http://TestUri/");
        private static readonly string property = "TestProperty";

        [TestMethod]
        public void Minimize3To1()
        {
            var changes = new List<IModelChange>
            {
                new PropertyChange<int>(uri, property, 1, 2),
                new PropertyChange<int>(uri, property, 2, 3),
                new PropertyChange<int>(uri, property, 3, 4)
            };
            var strategy = new MultiplePropertyChanges();
            var expected = new List<IModelChange>() { new PropertyChange<int>(uri, property, 1, 4) };

            var actual = strategy.Execute(changes);

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Minimize4To2()
        {
            var property2 = "SomeOtherProperty";
            var changes = new List<IModelChange>
            {
                new PropertyChange<int>(uri, property, 1, 2),
                new PropertyChange<string>(uri, property2, "hello", "hi"),
                new PropertyChange<string>(uri, property2, "hi", "welcome"),
                new PropertyChange<int>(uri, property, 2, 3)
            };
            var strategy = new MultiplePropertyChanges();
            var expected = new List<IModelChange>()
            {
                new PropertyChange<int>(uri, property, 1, 3),
                new PropertyChange<string>(uri, property2, "hello", "welcome"),
            };

            var actual = strategy.Execute(changes);

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
