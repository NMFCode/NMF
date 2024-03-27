using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Expressions.Linq;
using NMF.Expressions.Tests.SocialNetwork;
using NMF.Models;

namespace NMF.Expressions.Tests
{
    [TestClass]
    public class SocialNetworkTests
    {
        [TestMethod]
        public void TestPostScore()
        {
            var post = new Post
            {
                Active = true,
                Content = "Test",
                Score = 10,
                Id = "42"
            };
            var comment1 = new Comment
            {
                Content = "Foo",
                Id = "42.1",
                Score = 10,
                Post = post
            };
            var comment2 = new Comment
            {
                Content = "Foo",
                Id = "42.2",
                Score = 10,
                Post = post
            };
            post.Comments.Add(comment1);
            post.Comments.Add(comment2);
            var totalScore = Observable.Expression(() => post.Score + post.Descendants().OfType<IComment>().Sum(c => c.Score));

            Assert.AreEqual(30, totalScore.Value);

            var currentValue = 29;
            var oldValue = 30;
            var updated = false;

            totalScore.ValueChanged += (o, e) =>
            {
                Assert.AreEqual(e.OldValue, oldValue);
                Assert.AreEqual(e.NewValue, currentValue);
                updated = true;
            };

            comment1.Score = 9;

            Assert.IsTrue(updated);
            Assert.AreEqual(29, totalScore.Value);

            currentValue = 39;
            oldValue = 29;
            updated = false;

            var comment11 = new Comment
            {
                Content = "Bullshit",
                Id = "42.1.1",
                Score = 10
            };
            comment1.Comments.Add(comment11);

            Assert.IsTrue(updated);
            Assert.AreEqual(39, totalScore.Value);

            currentValue = 38;
            oldValue = 39;
            updated = false;

            comment1.Comments[0].Score = 9;

            Assert.IsTrue(updated);
            Assert.AreEqual(38, totalScore.Value);

            currentValue = 20;
            oldValue = 38;
            updated = false;

            post.Comments.Remove(comment1);

            Assert.IsTrue(updated);
            Assert.AreEqual(20, totalScore.Value);

            currentValue = 19;
            oldValue = 20;
            updated = false;

            post.Score = 9;

            Assert.IsTrue(updated);
            Assert.AreEqual(19, totalScore.Value);
        }
    }
}
