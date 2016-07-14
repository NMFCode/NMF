using System;
using NMF.Expressions.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Sys = System.Linq.Enumerable;

namespace NMF.Expressions.Linq.Tests
{
    [TestClass]
    public class SelectTest
    {
        [TestMethod]
        public void Select_NoObservableSourceItemAdded_NoUpdate()
        {
            var update = false;
            ICollection<Dummy<string>> coll = new List<Dummy<string>>();

            var test = coll.WithUpdates().Select(d => d.Item);

            test.CollectionChanged += (o, e) => update = true;

            Assert.IsTrue(!test.Any());
            Assert.IsFalse(update);

            coll.Add(new Dummy<string>() { Item = "42" });

            Assert.IsTrue(!test.Any());
            Assert.IsFalse(update);
        }

        [TestMethod]
		public void Select_ObservableSourceItemAdded_Update()
        {
            var update = false;
            ICollection<Dummy<string>> coll = new ObservableCollection<Dummy<string>>();

            var test = coll.WithUpdates().Select(d => d.Item);

            test.CollectionChanged += (o, e) =>
            {
                Assert.IsTrue(e.NewItems.Contains("42"));
                Assert.AreEqual(1, e.NewItems.Count);
                update = true;
            };

            Assert.IsTrue(!test.Any());
            Assert.IsFalse(update);

            coll.Add(new Dummy<string>() { Item = "42" });

            Assert.IsTrue(update);
            Assert.IsFalse(!test.Any());
            Assert.IsTrue(Sys.Contains(test, "42"));
        }

		[TestMethod]
		public void Select_NoObservableSourceItemRemoved_NoUpdate()
		{
			var update = false;
			ICollection<Dummy<string>> coll = new List<Dummy<string>>();
			var dummy = new Dummy<string>() { Item = "42" };
			coll.Add(dummy);

			var test = coll.WithUpdates().Select(d => d.Item);

			test.CollectionChanged += (o, e) => update = true;

			Assert.IsTrue(Sys.Contains(test, "42"));
			Assert.IsFalse(update);

			coll.Remove(dummy);

			Assert.IsFalse(Sys.Contains(test, "42"));
			Assert.IsFalse(update);
		}

		[TestMethod]
		public void Select_ObservableSourceItemRemoved_Update()
		{
			var update = false;
			ICollection<Dummy<string>> coll = new ObservableCollection<Dummy<string>>();
			var dummy = new Dummy<string>() { Item = "42" };
			coll.Add(dummy);

			var test = coll.WithUpdates().Select(d => d.Item);

			test.CollectionChanged += (o, e) =>
			{
				Assert.IsTrue(e.OldItems.Contains("42"));
				Assert.AreEqual(1, e.OldItems.Count);
				update = true;
			};

			Assert.IsTrue(Sys.Contains(test, "42"));
			Assert.IsFalse(update);

			coll.Remove(dummy);

			Assert.IsTrue(update);
			Assert.IsFalse(Sys.Contains(test, "42"));
		}

        [TestMethod]
        public void Select_NoObservableItem_NoUpdates()
        {
            var update = false;
            ICollection<Dummy<string>> coll = new List<Dummy<string>>();
            var dummy = new Dummy<string>() { Item = "23" };
            coll.Add(dummy);

            var test = coll.WithUpdates().Select(d => d.Item);

            test.CollectionChanged += (o, e) => update = true;

            Assert.IsTrue(Sys.Contains(test, "23"));
            Assert.IsFalse(update);

            dummy.Item = "42";

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Select_ObservableItem_Update()
        {
            var update = false;
            ICollection<Dummy<string>> coll = new List<Dummy<string>>();
            var dummy = new ObservableDummy<string>() { Item = "23" };
            coll.Add(dummy);

            var test = coll.WithUpdates().Select(d => d.Item);

            test.CollectionChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("23", e.OldItems[0]);
            };

            Assert.IsTrue(Sys.Contains(test, "23"));
            Assert.IsFalse(update);

            dummy.Item = "42";

            Assert.IsTrue(update);
            Assert.IsTrue(Sys.Contains(test, "42"));
        }


        [TestMethod]
        public void Select_ObservableIteam_Update()
        {
            var update = false;
            ICollection<Dummy<string>> coll = new List<Dummy<string>>();
            var dummy = new ObservableDummy<string>() { Item = "23" };
            coll.Add(dummy);

            var test = coll.WithUpdates().Select(d => d.Item);

            test.CollectionChanged += (o, e) =>
            {
                update = true;
//                Assert.AreEqual("23", e.OldItems[0]);
            };

            dummy.Item = "42";


        }
    }
}
