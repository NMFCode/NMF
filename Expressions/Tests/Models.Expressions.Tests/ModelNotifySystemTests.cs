using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Expressions.Test;
using NMF.Models.Tests.Debug;
using NMF.Models.Tests.Railway;
using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Expressions.Tests
{
    [TestClass]
    public class ModelNotifySystemTests
    {
        private INotifySystem oldSystem;

        [TestInitialize]
        public void Init()
        {
            oldSystem = NotifySystem.DefaultSystem;
            NotifySystem.DefaultSystem = new ModelNotifySystem();
        }

        [TestCleanup]
        public void Teardown()
        {
            NotifySystem.DefaultSystem = oldSystem;
        }

        [TestMethod]
        public void ParameterDependency_Works()
        {
            var semaphore = new Semaphore { Signal = Signal.FAILURE };
            var changed = false;

            var test = Observable.Expression(() => IsGo(semaphore));
            test.ValueChanged += (o, e) =>
            {
                changed = true;
            };

            Assert.AreEqual(test.Value, false);
            Assert.IsFalse(changed);

            semaphore.Signal = Signal.STOP;

            Assert.AreEqual(test.Value, false);
            Assert.IsFalse(changed);

            semaphore.Signal = Signal.GO;

            Assert.IsTrue(changed);
            Assert.IsTrue(test.Value);
        }
        [TestMethod]
        public void ParameterDependency_ListWorks()
        {
            var element = new AClass();
            element.Cont1.Add(new CClass());
            var changed = false;
            
            var test = Observable.Expression(() => CountContent(element));
            test.ValueChanged += (o, e) => changed = true;

            Assert.AreEqual(test.Value, 1);
            Assert.IsFalse(changed);

            element.Cont1.Add(new CClass());

            Assert.AreEqual(test.Value, 2);
            Assert.IsTrue(changed);

            changed = false;
            element.Cont1.RemoveAt(0);

            Assert.AreEqual(test.Value, 1);
            Assert.IsTrue(changed);
        }

    [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ParameterDependency_WrongParameterThrows()
        {
            var semaphore = new Semaphore();
            Observable.Expression(() => WrongParameters(semaphore));
        }

        [TestMethod]
        public void ParameterDependency_WrongMemberWorks()
        {
            var semaphore = new Semaphore { Id = 42 };
            var dummy = new ObservableDummy<Semaphore>(semaphore);
            var changed = false;
            var test = Observable.Expression(() => WrongMember(dummy.Item));
            test.ValueChanged += (o, e) => changed = true;

            Assert.IsTrue(test.Value);
            Assert.IsFalse(changed);

            semaphore.Id = 23;

            Assert.IsTrue(test.Value);
            Assert.IsFalse(changed);

            dummy.Item = new Semaphore { Id = 23 };

            Assert.IsTrue(changed);
            Assert.IsFalse(test.Value);
        }
        
        [ParameterDependency("semaphore", "Signal")]
        public static bool IsGo(ISemaphore semaphore)
        {
            return semaphore != null && semaphore.Signal == Signal.GO;
        }
 
        [ParameterDependency("element", "Cont1", true)]
        public static int CountContent(IAClass element)
        {
            return element.Cont1.Count;
        }

    [ParameterDependency("does not exist", "foo")]
        public static bool WrongParameters(ISemaphore semaphore)
        {
            return true;
        }

        [ParameterDependency("semaphore", "Foo")]
        public static bool WrongMember(ISemaphore semaphore)
        {
            return semaphore.Id == 42;
        }
    }
}
