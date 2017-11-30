using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NMF.Collections.ObjectModel;

using NMF.Tests;
using NMF.Models.Collections;

namespace NMF.Collections.Test
{
    class TestOppositeSet : OppositeSet<Dummy, string>
    {
        public TestOppositeSet() : base(new Dummy()) { Added = new List<string>(); Removed = new List<string>(); }
        public List<string> Added { get; private set; }
        public List<string> Removed { get; private set; }

        protected override void SetOpposite(string item, Dummy newParent)
        {
            if (newParent == null)
            {
                Removed.Add(item);
            }
            else
            {
                Added.Add(item);
            }
        }

        public override bool Add(string item)
        {
            if (base.Add(item))
            {
                Added.AssertContainsOnly(item);
                Removed.AssertEmpty();
                Added.Clear();
                return true;
            }
            else
            {
                Added.AssertEmpty();
                Removed.AssertEmpty();
                return false;
            }
        }

        public override void Clear()
        {
            var items = this.ToArray();
            base.Clear();
            Removed.AssertContainsOnly(items);
            Removed.Clear();
            Added.AssertEmpty();
        }

        public override bool Remove(string item)
        {
            if (base.Remove(item))
            {
                Added.AssertEmpty();
                Removed.AssertContainsOnly(item);
                Removed.Clear();
                return true;
            }
            else
            {
                Added.AssertEmpty();
                Removed.AssertEmpty();
                return false;
            }
        }
    }

    class TestOppositeOrderedSet : OppositeOrderedSet<Dummy, string>
    {
        public TestOppositeOrderedSet() : base(new Dummy()) { Added = new List<string>(); Removed = new List<string>(); }
        public List<string> Added { get; private set; }
        public List<string> Removed { get; private set; }

        protected override void SetOpposite(string item, Dummy newParent)
        {
            if (newParent == null)
            {
                Removed.Add(item);
            }
            else
            {
                Added.Add(item);
            }
        }

        public override bool Add(string item)
        {
            if (base.Add(item))
            {
                Added.AssertContainsOnly(item);
                Removed.AssertEmpty();
                Added.Clear();
                return true;
            }
            else
            {
                Added.AssertEmpty();
                Removed.AssertEmpty();
                return false;
            }
        }

        public override void Clear()
        {
            var items = this.ToArray();
            base.Clear();
            Removed.AssertContainsOnly(items);
            Removed.Clear();
            Added.AssertEmpty();
        }

        protected override bool Remove(string item, int index)
        {
            if (base.Remove(item, index))
            {
                Added.AssertEmpty();
                Removed.AssertContainsOnly(item);
                Removed.Clear();
                return true;
            }
            else
            {
                Added.AssertEmpty();
                Removed.AssertEmpty();
                return false;
            }
        }
    }

    class TestOppositeList : OppositeList<Dummy, string>
    {
        public TestOppositeList() : base(new Dummy()) { Added = new List<string>(); Removed = new List<string>(); }
        public List<string> Added { get; private set; }
        public List<string> Removed { get; private set; }

        protected override void SetOpposite(string item, Dummy newParent)
        {
            if (newParent == null)
            {
                Removed.Add(item);
            }
            else
            {
                Added.Add(item);
            }
        }

        protected override void InsertItem(int index, string item)
        {
            base.InsertItem(index, item);
            Added.AssertContainsOnly(item);
            Removed.AssertEmpty();
            Added.Clear();
        }

        protected override void ClearItems()
        {
            var items = this.ToArray();
            base.ClearItems();
            Removed.AssertContainsOnly(items);
            Removed.Clear();
            Added.AssertEmpty();
        }

        protected override void RemoveItem(int index)
        {
            var item = this[index];
            base.RemoveItem(index);
            Added.AssertEmpty();
            Removed.AssertContainsOnly(item);
            Removed.Clear();
        }
    }

    class TestObservableOppositeSet : ObservableOppositeSet<Dummy, string>
    {
        public TestObservableOppositeSet() : base(new Dummy()) { Added = new List<string>(); Removed = new List<string>(); }
        public List<string> Added { get; private set; }
        public List<string> Removed { get; private set; }

        protected override void SetOpposite(string item, Dummy newParent)
        {
            if (newParent == null)
            {
                Removed.Add(item);
            }
            else
            {
                Added.Add(item);
            }
        }

        public override bool Add(string item)
        {
            if (base.Add(item))
            {
                Added.AssertContainsOnly(item);
                Removed.AssertEmpty();
                Added.Clear();
                return true;
            }
            else
            {
                Added.AssertEmpty();
                Removed.AssertEmpty();
                return false;
            }
        }

        public override void Clear()
        {
            var items = this.ToArray();
            base.Clear();
            Removed.AssertContainsOnly(items);
            Removed.Clear();
            Added.AssertEmpty();
        }

        public override bool Remove(string item)
        {
            if (base.Remove(item))
            {
                Added.AssertEmpty();
                Removed.AssertContainsOnly(item);
                Removed.Clear();
                return true;
            }
            else
            {
                Added.AssertEmpty();
                Removed.AssertEmpty();
                return false;
            }
        }
    }

    class TestObservableOppositeOrderedSet : ObservableOppositeOrderedSet<Dummy, string>
    {
        public TestObservableOppositeOrderedSet() : base(new Dummy()) { Added = new List<string>(); Removed = new List<string>(); }
        public List<string> Added { get; private set; }
        public List<string> Removed { get; private set; }

        protected override void SetOpposite(string item, Dummy newParent)
        {
            if (newParent == null)
            {
                Removed.Add(item);
            }
            else
            {
                Added.Add(item);
            }
        }

        public override bool Add(string item)
        {
            if (base.Add(item))
            {
                Added.AssertContainsOnly(item);
                Removed.AssertEmpty();
                Added.Clear();
                return true;
            }
            else
            {
                Added.AssertEmpty();
                Removed.AssertEmpty();
                return false;
            }
        }

        public override void Clear()
        {
            var items = this.ToArray();
            base.Clear();
            Removed.AssertContainsOnly(items);
            Removed.Clear();
            Added.AssertEmpty();
        }

        protected override bool Remove(string item, int index)
        {
            if (base.Remove(item, index))
            {
                Added.AssertEmpty();
                Removed.AssertContainsOnly(item);
                Removed.Clear();
                return true;
            }
            else
            {
                Added.AssertEmpty();
                Removed.AssertEmpty();
                return false;
            }
        }
    }

    class TestObservableOppositeList : ObservableOppositeList<Dummy, string>
    {
        public TestObservableOppositeList() : base(new Dummy()) { Added = new List<string>(); Removed = new List<string>(); }
        public List<string> Added { get; private set; }
        public List<string> Removed { get; private set; }

        protected override void SetOpposite(string item, Dummy newParent)
        {
            if (newParent == null)
            {
                Removed.Add(item);
            }
            else
            {
                Added.Add(item);
            }
        }

        protected override void InsertItem(int index, string item)
        {
            base.InsertItem(index, item);
            Added.AssertContainsOnly(item);
            Removed.AssertEmpty();
            Added.Clear();
        }

        protected override void ClearItems()
        {
            var items = this.ToArray();
            base.ClearItems();
            Removed.AssertContainsOnly(items);
            Removed.Clear();
            Added.AssertEmpty();
        }

        protected override void RemoveItem(int index)
        {
            var item = this[index];
            base.RemoveItem(index);
            Added.AssertEmpty();
            Removed.AssertContainsOnly(item);
            Removed.Clear();
        }
    }
}
