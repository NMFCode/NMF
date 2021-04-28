﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using NMF.Expressions.Test;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.Specialized;

namespace NMF.Expressions.Linq.Tests
{
    [TestClass]
    public class LinqTest
    {
        [TestMethod]
        public void Linq_SelectMany()
        {
            var coll = new ObservableCollection<Dummy<IEnumerable<string>>>();
            var update = false;

            var dummy = new Dummy<IEnumerable<string>>()
            {
                Item = new List<string>() { "42" }
            };

            var test = from d in coll.WithUpdates()
                       from s in d.Item
                       select s.Substring(0);

            test.CollectionChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                Assert.AreEqual("42", e.NewItems[0]);
            };

            Assert.IsFalse(test.Any());
            Assert.IsFalse(update);

            coll.Add(dummy);

            Assert.IsTrue(update);
        }

        [TestMethod]
        public void Linq_Test_Employees()
        {
            var violation = false;

            var employees = new ObservableCollection<Person>();
            var violations = from employee in employees.WithUpdates()
                             where employee.WorkItems > 2 *
                                (from collegue in employees.WithUpdates()
                                 where collegue.Team == employee.Team
                                 select collegue.WorkItems).Average()
                             select employee.Name;

            violations.CollectionChanged += (o, e) =>
            {
                violation = true;
            };

            Assert.IsFalse(violation);
            employees.Add(new Person() { Name = "John", WorkItems = 20, Team = "A" });
            Assert.IsFalse(violation);
            employees.Add(new Person() { Name = "Susi", WorkItems = 5, Team = "A" });
            Assert.IsFalse(violation);
            employees.Add(new Person() { Name = "Joe", WorkItems = 3, Team = "A" });
            Assert.IsTrue(violation);
        }

        [TestMethod]
        public void Linq_Group_Test_Employees()
        {
            var violation = false;

            var employees = new ObservableCollection<Person>();
            var employeesWithUpdates = employees.WithUpdates();
            var violations = from employee in employeesWithUpdates
                             group employee by employee.Team into team
                             from member in team
                             where member.WorkItems >= 2 * team.Average(e => e.WorkItems)
                             select member.Name;

            violations.CollectionChanged += (o, e) =>
            {
                violation = true;
            };

            Assert.IsFalse(violation);
            employees.Add(new Person() { Name = "John", WorkItems = 20, Team = "A" });
            Assert.IsFalse(violation);
            employees.Add(new Person() { Name = "Susi", WorkItems = 5, Team = "A" });
            Assert.IsFalse(violation);
            employees.Add(new Person() { Name = "Joe", WorkItems = 3, Team = "A" });
            Assert.IsTrue(violation);
        }


    }

    class Person : INotifyPropertyChanged
    {
        #region Name

        private string mName;

        public string Name
        {
            get
            {
                return mName;
            }
            set
            {
                if (mName != value)
                {
                    mName = value;
                    OnNameChanged(EventArgs.Empty);
                }
            }
        }

        protected virtual void OnNameChanged(EventArgs e)
        {
            NameChanged?.Invoke(this, e);
            OnPropertyChanged("Name");
        }

        public event EventHandler NameChanged;

        #endregion

        #region WorkItems

        private int mWorkItems;

        public int WorkItems
        {
            get
            {
                return mWorkItems;
            }
            set
            {
                if (mWorkItems != value)
                {
                    mWorkItems = value;
                    OnWorkItemsChanged(EventArgs.Empty);
                }
            }
        }

        protected virtual void OnWorkItemsChanged(EventArgs e)
        {
            WorkItemsChanged?.Invoke(this, e);
            OnPropertyChanged("WorkItems");
        }

        public event EventHandler WorkItemsChanged;

        #endregion

        #region Team

        private string mTeam;

        public string Team
        {
            get
            {
                return mTeam;
            }
            set
            {
                if (mTeam != value)
                {
                    mTeam = value;
                    OnTeamChanged(EventArgs.Empty);
                }
            }
        }

        protected virtual void OnTeamChanged(EventArgs e)
        {
            TeamChanged?.Invoke(this, e);
            OnPropertyChanged("Team");
        }

        public event EventHandler TeamChanged;

        #endregion

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
