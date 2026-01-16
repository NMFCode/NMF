using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Models.Meta;
using NMF.Models.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Models.Tests
{
    [TestClass]
    public class RelativeUriTests
    {
        [TestMethod]
        public void Serializer_SimplifiesUris()
        {
            var c1 = new Class { Name = "A" };
            var c2 = new Class { Name = "B", BaseTypes = { c1 } };

            var modelA = new Model { ModelUri = new Uri(Path.GetFullPath("a.x")), RootElements = { c1 } };
            var modelB = new Model { ModelUri = new Uri(Path.GetFullPath("b.x")), RootElements = { c2 } };

            var repo = new ModelRepository();
            repo.Save(modelA, "a.x");
            repo.Save(modelB, "b.x");

            var bContents = File.ReadAllText("b.x");

            Assert.IsFalse(bContents.Contains(modelA.ModelUri.AbsoluteUri));
            Assert.IsTrue(bContents.Contains("a.x"));
        }

        [TestMethod]
        public void Repository_ResolvesLinkedModels()
        {
            var c1 = new Class { Name = "A" };
            var c2 = new Class { Name = "B", BaseTypes = { c1 } };

            var modelA = new Model { ModelUri = new Uri(Path.GetFullPath("a.x")), RootElements = { c1 } };
            var modelB = new Model { ModelUri = new Uri(Path.GetFullPath("b.x")), RootElements = { c2 } };

            var repo = new ModelRepository();
            repo.Save(modelA, "a.x");
            repo.Save(modelB, "b.x");

            var repo2 = new ModelRepository();
            var newModelB = repo2.Resolve("b.x");

            Assert.IsTrue(repo2.Models.ContainsKey(modelA.ModelUri));
            Assert.IsTrue(repo2.Models.ContainsKey(modelB.ModelUri));

            var loadedC2 = (Class)newModelB.RootElements[0];
            Assert.IsInstanceOfType(loadedC2, typeof(Class));
            Assert.IsNotNull(loadedC2.BaseTypes.FirstOrDefault());
            Assert.AreEqual(modelA.ModelUri, loadedC2.BaseTypes.FirstOrDefault().Model.ModelUri);
        }
    }
}
