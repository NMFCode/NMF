using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Models.Evolution;
using NMF.Models.Repository;
using NMF.Models.Tests.Railway;
using NMF.Serialization.Xmi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NMF.Models.Tests.Evolution
{
    [TestClass]
    public class TempTests
    {
        private ModelRepository repository;
        private RailwayContainer railway;

        private const string BaseUri = "http://github.com/NMFCode/NMF/Models/Models.Test/railway.railway";

        [TestInitialize]
        public void LoadRailwayModel()
        {
            repository = new ModelRepository();
            var railwayModel = repository.Resolve(new Uri(BaseUri), "..\\..\\railway.railway").Model;
            Assert.IsNotNull(railwayModel);
            railway = railwayModel.RootElements.Single() as RailwayContainer;
            Assert.IsNotNull(railway);
        }
        
        [TestMethod]
        public void BubbledChangeListHasCorrectTreeForm()
        {
            var allUris = railway.Descendants().Select(e => e.RelativeUri).ToList();

            foreach (var uri in allUris)
            {
                LoadRailwayModel();
                var args = new List<BubbledChangeEventArgs>();
                railway.BubbledChange += (obj, e) => args.Add(e);
                var element = railway.Resolve(uri);
                element.Delete();

                CollectionAssert.AllItemsAreNotNull(args.Select(a => a.AbsoluteUri).ToList());
                AssertCorrectTreeForm(args, 0);
            }
        }

        private int AssertCorrectTreeForm(List<BubbledChangeEventArgs> list, int startingIndex)
        {
            int i = 1;
            var current = list[startingIndex];
            while (startingIndex + i < list.Count)
            {
                var other = list[startingIndex + i];
                if (((current.ChangeType == ChangeType.CollectionChanging && other.ChangeType == ChangeType.CollectionChanged) ||
                    (current.ChangeType == ChangeType.ModelElementDeleting && other.ChangeType == ChangeType.ModelElementDeleted) ||
                    (current.ChangeType == ChangeType.PropertyChanging && other.ChangeType == ChangeType.PropertyChanged))
                    && current.AbsoluteUri == other.AbsoluteUri)
                {
                    return i;
                }
                else if (other.ChangeType == ChangeType.CollectionChanging ||
                    other.ChangeType == ChangeType.ModelElementDeleting || 
                    other.ChangeType == ChangeType.PropertyChanging)
                {
                    i += AssertCorrectTreeForm(list, startingIndex + i);
                }
                else if(other.ChangeType != ChangeType.ModelElementCreated)
                {
                    Assert.Fail("Found " + other.ChangeType + " without previously encountering the corresponding -ing event. URI: " + other.AbsoluteUri);
                }

                i++;
            }
            Assert.Fail("No corresponding -ed event found for " + current.ChangeType + ". URI: " + current.AbsoluteUri);
            return -1;
        }

        [TestMethod]
        public void Test()
        {
            var rec = new ModelChangeRecorder();
            rec.Start(railway);
            railway.Semaphores[0].Delete();
            railway.Semaphores[1].Delete();
            var changes = rec.GetModelChanges();

            var ser = new XmiSerializer();
            string result;
            using (var writer = new StringWriter())
            {
                ser.Serialize(changes, writer);
                result = writer.ToString();
            }
        }
    }
}
