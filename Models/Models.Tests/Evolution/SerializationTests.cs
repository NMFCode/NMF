using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Models.Evolution;
using NMF.Models.Repository;
using NMF.Models.Tests.Railway;
using NMF.Serialization.Xmi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace NMF.Models.Tests.Evolution
{
    [TestClass]
    public class SerializationTests
    {
        private static readonly Uri uri = new Uri("http://TestUri");
        private static readonly string property = "TestProperty";

        [TestMethod]
        public void SerializePropertyChangeAttribute()
        {
            var change = new PropertyChangeAttribute<Signal>(uri, property, Signal.STOP, Signal.FAILURE);
            SerializeAndAssert(change);
        }

        [TestMethod]
        public void SerializePropertyChangeReference()
        {
            var change = new PropertyChangeReference<Semaphore>(uri, property, new Uri("http://ReferenceUri"));
            SerializeAndAssert(change);
        }

        [TestMethod]
        public void SerializeElementDeletion()
        {
            var change = new ElementDeletion(uri);
            SerializeAndAssert(change);
        }

        [TestMethod]
        public void SerializeListInsertionComposition()
        {
            var change = new ListInsertionComposition<int>(uri, property, 0, new List<int>() { 23, 42 });
            SerializeAndAssert(change);
        }

        [TestMethod]
        public void SerializeListInsertionAssociation()
        {
            var change = new ListInsertionAssociation<IRoute>(uri, property, 0, new List<Uri>() { new Uri("http://ReferenceUri1"), new Uri("http://ReferenceUri2") });
            SerializeAndAssert(change);
        }

        [TestMethod]
        public void SerializeCollectionInsertionComposition()
        {
            var change = new CollectionInsertionComposition<int>(uri, property, new Collection<int>() { 23, 42 });
            SerializeAndAssert(change);
        }

        [TestMethod]
        public void SerializeCollectionInsertionAssociation()
        {
            var change = new CollectionInsertionAssociation<IRoute>(uri, property, new Collection<Uri>() { new Uri("http://ReferenceUri1"), new Uri("http://ReferenceUri2") });
            SerializeAndAssert(change);
        }

        [TestMethod]
        public void SerializeListDeletionComposition()
        {
            var change = new ListDeletionComposition<int>(uri, property, 42, 23);
            SerializeAndAssert(change);
        }

        [TestMethod]
        public void SerializeListDeletionAssociation()
        {
            var list = new List<Uri>() {new Uri("http://ReferenceUri1"), new Uri("http://ReferenceUri2")};
            var change = new ListDeletionAssociation<IRoute>(uri, property, 0, list.Count, list);
            SerializeAndAssert(change);
        }

        [TestMethod]
        public void SerializeCollectionDeletionComposition()
        {
            var change = new CollectionDeletionComposition<int>(uri, property, new Collection<int>() {42});
            SerializeAndAssert(change);
        }

        [TestMethod]
        public void SerializeCollectionDeletionAssociation()
        {
            var list = new Collection<Uri>() { new Uri("http://ReferenceUri1"), new Uri("http://ReferenceUri2") };
            var change = new CollectionDeletionAssociation<IRoute>(uri, property, list);
            SerializeAndAssert(change);
        }

        [TestMethod]
        public void SerializeCollectionResetComposition()
        {
            var change = new CollectionResetComposition<int>(uri, property, null, null, new Collection<int>() {42});
            SerializeAndAssert(change);
        }

        [TestMethod]
        public void SerializeChangeTransaction()
        {
            var sourceChange = new ElementDeletion(uri);
            var nestedChanges = new[] { new ListDeletionComposition<int>(uri, property, 0, 1),  };
            var change = new ChangeTransaction(sourceChange, nestedChanges);
            SerializeAndAssert(change);
        }

        [TestMethod]
        public void SerializeElementCreation()
        {
            var toCreate = new Semaphore() { Id = 1 };
            var change = new ElementCreation(toCreate);
            var before = new ModelChangeCollection();
            before.Changes.Add(change);
            var after = Serialize(before, typeof(ElementCreation));

            // We can't use SerializeAndAssert, because ElementCreation.Equals
            // compares the Semaphores, which don't implement Equals.

            Assert.AreEqual(1, after.Changes.Count);
            Assert.IsInstanceOfType(after.Changes[0], typeof(ElementCreation));
            var afterChange = (ElementCreation)after.Changes[0];
            Assert.IsInstanceOfType(afterChange.Element, typeof(Semaphore));
            Assert.AreEqual(toCreate.Id, ((Semaphore)afterChange.Element).Id);
        }

        private void SerializeAndAssert(IModelChange change)
        {
            var before = new ModelChangeCollection();
            before.Changes.Add(change);
            var after = Serialize(before, change.GetType());
            Assert.AreEqual(before, after);
        }

        private ModelChangeCollection Serialize(ModelChangeCollection before, params Type[] additionalTypes)
        {
            var serializer = new XmiSerializer(additionalTypes);

            string xmi;
            using (var writer = new StringWriter())
            {
                serializer.Serialize(before, writer);
                xmi = writer.ToString();
            }
            
            using (var reader = new StringReader(xmi))
            {
                return serializer.Deserialize(reader) as ModelChangeCollection;
            }
        }
    }
}
