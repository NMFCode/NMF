using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NMF.Expressions.Linq;

namespace NMF.Expressions.Test
{
    [TestClass]
    public class ProjectTodoTests
    {
        [TestMethod]
        public void Linq_TestQuery()
        {
            ObservableExtensions.KeepOrder = true;

            var projects = new ObservableCollection<Project>();
            var query = from p in projects.WithUpdates()
                        where !p.IsOnHold
                        from t in p.Todos
                        where !t.IsDone
                        orderby t.Priority descending, t.Deadline
                        select t;

            AssertSequence(query);
            var p1 = new Project { Name = "P1" };
            projects.Add(p1);

            AssertSequence(query);

            var t1 = new Todo { Text = "T1", Deadline = DateTime.Today.AddDays(5) };
            var t2 = new Todo { Text = "T2", Deadline = DateTime.Today.AddDays(1) };
            var t3 = new Todo { Text = "T3", Deadline = DateTime.Today.AddDays(2) };
            var t4 = new Todo { Text = "T4", Deadline = DateTime.Today.AddDays(3) };

            p1.Todos.Add(t1);
            p1.Todos.Add(t2);

            AssertSequence(query, "T2", "T1");

            projects.Add(new Project { Name = "P2", Todos = { t3, t4 } });

            AssertSequence(query, "T2", "T3", "T4", "T1");

            t1.Deadline = DateTime.Today;

            AssertSequence(query, "T1", "T2", "T3", "T4");

            p1.IsOnHold = true;

            AssertSequence(query, "T3", "T4");

            p1.IsOnHold = false;

            AssertSequence(query, "T1", "T2", "T3", "T4");

            t3.IsDone = true;

            AssertSequence(query, "T1", "T2", "T4");

            t4.Priority = Priority.High;

            AssertSequence(query, "T4", "T1", "T2");

            t1.Priority = Priority.Low;

            AssertSequence(query, "T4", "T2", "T1");

        }

        private static void AssertSequence(IEnumerable<Todo> actual, params string[] expected)
        {
            var index = 0;
            foreach (var item in actual)
            {
                if (index < expected.Length)
                {
                    Assert.AreEqual(expected[index], item.Text);
                }
                else
                {
                    Assert.Fail($"Did not expect todo {item.Text}");
                }
                index++;
            }
            Assert.AreEqual(expected?.Length ?? 0, index, $"Did not get correct number of elements, expected {expected?.Length}, but got {index}");
        }

        private class Project : ModelBase
        {
            private bool isOnHold;
            private string name;

            public string Name
            {
                get => name;
                set => Set(ref name, value);
            }

            public bool IsOnHold
            {
                get => isOnHold; 
                set => Set(ref isOnHold, value);
            }

            public ICollection<Todo> Todos { get; } = new ObservableCollection<Todo>();
        }

        private class Todo : ModelBase
        {
            private string text;
            private DateTime deadline;
            private bool isDone;
            private Priority priority;

            public string Text
            {
                get => text;
                set => Set(ref text, value);
            }

            public Priority Priority
            {
                get => priority;
                set => Set(ref priority, value);
            }

            public DateTime Deadline
            {
                get => deadline;
                set => Set(ref deadline, value);
            }

            public bool IsDone
            {
                get => isDone;
                set => Set(ref isDone, value);
            }
        }

        private enum Priority
        {
            Low = -1,
            Medium = 0,
            High = 1
        }

        private class ModelBase : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            protected bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
            {
                if (!EqualityComparer<T>.Default.Equals(field, value))
                {
                    field = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                    return true;
                }
                return false;
            }
        }
    }
}
