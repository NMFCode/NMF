using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using NMF.Utilities;

namespace NMF.Utilities.Tests
{
    [TestClass]
    public class TreeTest
    {
        private TreeItem root;
        private TreeItem child1;
        private TreeItem child2;
        private TreeItem child11;
        private TreeItem child111;
        private TreeItem child112;
        private TreeItem child113;
        private TreeItem child21;
        private TreeItem child211;
        private TreeItem child2111;
        private TreeItem child22;
        private TreeItem child23;
        private TreeItem nullItem = null;

        [TestInitialize]
        public void InitializeTree()
        {
            root = new TreeItem();
            child1 = root.AddChild();
            child2 = root.AddChild();
            child11 = child1.AddChild();
            child111 = child11.AddChild();
            child112 = child11.AddChild();
            child113 = child11.AddChild();
            child21 = child2.AddChild();
            child211 = child21.AddChild();
            child2111 = child211.AddChild();
            child22 = child2.AddChild();
            child23 = child2.AddChild();
        }

        [TestMethod]
        public void Utilities_Extensions_DepthOfTree1()
        {
            Func<TreeItem, TreeItem> parent = t => t.Parent;
            Assert.AreEqual(1, root.DepthOfTree(parent));
            Assert.AreEqual(0, nullItem.DepthOfTree(parent));
            Assert.AreEqual(2, child1.DepthOfTree(parent));
            Assert.AreEqual(2, child2.DepthOfTree(parent));
            Assert.AreEqual(3, child11.DepthOfTree(parent));
            Assert.AreEqual(4, child111.DepthOfTree(parent));
            Assert.AreEqual(4, child112.DepthOfTree(parent));
            Assert.AreEqual(4, child113.DepthOfTree(parent));
            Assert.AreEqual(3, child21.DepthOfTree(parent));
            Assert.AreEqual(4, child211.DepthOfTree(parent));
            Assert.AreEqual(5, child2111.DepthOfTree(parent));
            Assert.AreEqual(3, child22.DepthOfTree(parent));
            Assert.AreEqual(3, child23.DepthOfTree(parent));
        }

        [TestMethod]
        public void Utilities_Extensions_DepthOfTree2()
        {
            Func<TreeItem, IEnumerable<TreeItem>> childs = t => t.Children;
            Assert.AreEqual(5, root.DepthOfTree(childs));
            Assert.AreEqual(0, nullItem.DepthOfTree(childs));
            Assert.AreEqual(3, child1.DepthOfTree(childs));
            Assert.AreEqual(4, child2.DepthOfTree(childs));
            Assert.AreEqual(2, child11.DepthOfTree(childs));
            Assert.AreEqual(1, child111.DepthOfTree(childs));
            Assert.AreEqual(1, child112.DepthOfTree(childs));
            Assert.AreEqual(1, child113.DepthOfTree(childs));
            Assert.AreEqual(3, child21.DepthOfTree(childs));
            Assert.AreEqual(2, child211.DepthOfTree(childs));
            Assert.AreEqual(1, child2111.DepthOfTree(childs));
            Assert.AreEqual(1, child22.DepthOfTree(childs));
            Assert.AreEqual(1, child23.DepthOfTree(childs));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Utilities_Extensions_DepthOfTree3()
        {
            var result = root.DepthOfTree(null as Func<TreeItem, TreeItem>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Utilities_Extensions_DepthOfTree4()
        {
            var result = root.DepthOfTree(null as Func<TreeItem, IEnumerable<TreeItem>>);   
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Utilities_Extensions_DepthOfTree5()
        {
            root.Children.Add(root);
            var result = root.DepthOfTree(t => t.Children);
        }

        [TestMethod]
        public void Utilities_Extensions_DepthOfTree6()
        {
            Assert.AreEqual(1, root.DepthOfTree(t => null as TreeItem));
            Assert.AreEqual(1, root.DepthOfTree(t => null as IEnumerable<TreeItem>));
        }
    }

    internal class TreeItem
    {
        public TreeItem Parent { get; set; }
        public List<TreeItem> Children { get; set; }

        public TreeItem()
        {
            Children = new List<TreeItem>();
        }

        public TreeItem AddChild()
        {
            var item = new TreeItem();
            item.Parent = this;
            Children.Add(item);
            return item;
        }
    }
}
