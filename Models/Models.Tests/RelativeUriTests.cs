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
    }
}
