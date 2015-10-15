using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Utilities;
using System.Linq;

namespace NMF.Utilities.Tests
{
    [TestClass]
    public class ClosureTest
    {
        State state1 = new State();
        State state2 = new State();
        State state3 = new State();
        State state4 = new State();
        State state5 = new State();

        [TestInitialize]
        public void InitStates()
        {
            state1.Sucessor(state2)
                .Sucessor(state3);
            state2.Sucessor(state1);
            state3.Sucessor(state3)
                .Sucessor(state4);
            state4.Sucessor(state5);
        }

        [TestMethod]
        public void Utilities_Extensions_Closure1()
        {
            var states = state1.Closure(s => s.Successors);

            Assert.IsTrue(states.Contains(state1));
            Assert.IsTrue(states.Contains(state2));
            Assert.IsTrue(states.Contains(state3));
            Assert.IsTrue(states.Contains(state4));
            Assert.IsTrue(states.Contains(state5));
            Assert.AreEqual(5, states.Count());
        }

        [TestMethod]
        public void Utilities_Extensions_Closure2()
        {
            var states = state2.Closure(s => s.Successors);

            Assert.IsTrue(states.Contains(state1));
            Assert.IsTrue(states.Contains(state2));
            Assert.IsTrue(states.Contains(state3));
            Assert.IsTrue(states.Contains(state4));
            Assert.IsTrue(states.Contains(state5));
            Assert.AreEqual(5, states.Count());
        }

        [TestMethod]
        public void Utilities_Extensions_Closure3()
        {
            var states = state3.Closure(s => s.Successors);

            Assert.IsFalse(states.Contains(state1));
            Assert.IsFalse(states.Contains(state2));
            Assert.IsTrue(states.Contains(state3));
            Assert.IsTrue(states.Contains(state4));
            Assert.IsTrue(states.Contains(state5));
            Assert.AreEqual(3, states.Count());
        }

        [TestMethod]
        public void Utilities_Extensions_Closure4()
        {
            var states = state4.Closure(s => s.Successors);

            Assert.IsFalse(states.Contains(state1));
            Assert.IsFalse(states.Contains(state2));
            Assert.IsFalse(states.Contains(state3));
            Assert.IsTrue(states.Contains(state4));
            Assert.IsTrue(states.Contains(state5));
            Assert.AreEqual(2, states.Count());
        }

        [TestMethod]
        public void Utilities_Extensions_Closure5()
        {
            var states = state5.Closure(s => s.Successors);

            Assert.IsFalse(states.Contains(state1));
            Assert.IsFalse(states.Contains(state2));
            Assert.IsFalse(states.Contains(state3));
            Assert.IsFalse(states.Contains(state4));
            Assert.IsTrue(states.Contains(state5));
            Assert.AreEqual(1, states.Count());
        }

        [TestMethod]
        public void Utilities_Extensions_Closure6()
        {
            var result = (null as State).Closure(s => s.Successors);

            Assert.IsNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Utilities_Extensions_Closure7()
        {
            state1.Closure(null);    
        }
    }
}
