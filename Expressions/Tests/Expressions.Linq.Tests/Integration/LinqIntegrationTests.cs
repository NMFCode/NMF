using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Collections.ObjectModel;

namespace NMF.Expressions.Linq.Tests.Integration
{
    class TestItem : INotifyPropertyChanged
    {
        #region Team

        private string team;

        public string Team
        {
            get
            {
                return team;
            }
            set
            {
                if (team != value)
                {
                    team = value;
                    OnPropertyChanged("Team");
                }
            }
        }

        #endregion
        #region Items

        private int items;

        public int Items
        {
            get
            {
                return items;
            }
            set
            {
                if (items != value)
                {
                    items = value;
                    OnPropertyChanged("Items");
                }
            }
        }

        #endregion

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    [TestClass]
    public class LinqIntegrationTests
    {
        [TestMethod]
        public void SelectManyWithAverage1()
        {
            var updates = 0;
            var coll = new NotifyCollection<TestItem>();

            var query = from item in coll
                        group item by item.Team into team
                        let avg = team.Average(it => it.Items)
                        from item2 in team
                        where item2.Items < avg
                        select item2;


            var testItem1 = new TestItem() { Items = 1, Team = "A" };
            var testItem2 = new TestItem() { Items = 2, Team = "A" };

            query.CollectionChanged += (o, e) =>
            {
                updates++;
                Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                Assert.AreEqual(1, e.NewItems.Count);
                Assert.AreSame(testItem1, e.NewItems[0]);
            };

            coll.Add(testItem1);

            Assert.AreEqual(0, updates);

            coll.Add(testItem2);

            Assert.AreEqual(1, updates);
            Assert.AreEqual(1, query.Count());
            Assert.IsTrue(query.Contains(testItem1));
        }


        [TestMethod]
        public void SelectManyWithAverage2()
        {
            var updates = 0;
            var coll = new NotifyCollection<TestItem>();

            var query = from item in coll
                        group item by item.Team into team
                        let avg = team.Average(it => it.Items)
                        from item2 in team
                        where item2.Items < avg
                        select item2;


            var testItem1 = new TestItem() { Items = 1, Team = "A" };
            var testItem2 = new TestItem() { Items = 2, Team = "A" };

            query.CollectionChanged += (o, e) =>
            {
                updates++;
                Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                Assert.AreEqual(1, e.NewItems.Count);
                Assert.AreSame(testItem1, e.NewItems[0]);
            };

            coll.Add(testItem2);

            Assert.AreEqual(0, updates);

            coll.Add(testItem1);

            Assert.AreEqual(1, updates);
            Assert.AreEqual(1, query.Count());
            Assert.IsTrue(query.Contains(testItem1));
        }


        [TestMethod]
        public void SelectWithAverage1()
        {
            var updates = 0;
            var coll = new ObservableCollection<TestItem>();

            var query = from item in coll.WithUpdates()
                        where item.Items < coll.WithUpdates().Average(t => t.Items)
                        select item;


            var testItem1 = new TestItem() { Items = 1, Team = "A" };
            var testItem2 = new TestItem() { Items = 2, Team = "A" };

            query.CollectionChanged += (o, e) =>
            {
                updates++;
                Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                Assert.AreEqual(1, e.NewItems.Count);
                Assert.AreSame(testItem1, e.NewItems[0]);
            };

            coll.Add(testItem1);

            Assert.AreEqual(0, updates);

            coll.Add(testItem2);

            Assert.AreEqual(1, updates);
            Assert.AreEqual(1, query.Count());
            Assert.IsTrue(query.Contains(testItem1));
        }


        [TestMethod]
        public void SelectWithAverage2()
        {
            var updates = 0;
            var coll = new ObservableCollection<TestItem>();

            var query = from item in coll.WithUpdates()
                        where item.Items < coll.WithUpdates().Average(t => t.Items)
                        select item;


            var testItem1 = new TestItem() { Items = 1, Team = "A" };
            var testItem2 = new TestItem() { Items = 2, Team = "A" };

            query.CollectionChanged += (o, e) =>
            {
                updates++;
                Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                Assert.AreEqual(1, e.NewItems.Count);
                Assert.AreSame(testItem1, e.NewItems[0]);
            };

            coll.Add(testItem2);

            Assert.AreEqual(0, updates);

            coll.Add(testItem1);

            Assert.AreEqual(1, updates);
            Assert.AreEqual(1, query.Count());
            Assert.IsTrue(query.Contains(testItem1));
        }
    }
}
