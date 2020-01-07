using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Models.Repository;
using NMF.Models.Tests.Minimal_example.Product;

namespace NMF.Models.Tests
{
    [TestClass]
    public class EmfIdentifierReferenceTests
    {
        [TestMethod]
        public void References_EmfStyleReferences_ResolvedCorectly()
        {
            var repository = new ModelRepository();
            var productLibraryModel = repository.Resolve("My1.product");
            var productLibrary = productLibraryModel.RootElements[0] as ProductLibrary;
            var prod1 = productLibrary.Products.Single();
            var seg = prod1.Productsegments.Single();
            var piece = prod1.Workpiecetypes.Single();
            Assert.AreEqual(seg.WorkpieceOut, piece);
        }
    }
}
