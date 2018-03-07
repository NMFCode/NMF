using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Emit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Models.Changes;
using NMF.Models.Repository;
using NMF.Models.Tests.Railway;
using NMF.Models.Meta;

namespace NMF.Models.Tests.Evolution
{
    /// <summary>
    /// Summary description for InvertTests
    /// </summary>
    [TestClass]
    public class InvertTests
    {
        private ModelRepository repository1;
        private ModelRepository repository2;
        private RailwayContainer railway1;
        private RailwayContainer railway2;

        private const string BaseUri = "http://github.com/NMFCode/NMF/Models/Models.Test/railway.railway";

        private RailwayContainer LoadRailwayModel(ModelRepository repository)
        {
            var railwayModel = repository.Resolve(new Uri(BaseUri), "railway.railway").Model;
            Assert.IsNotNull(railwayModel);
            var railway = railwayModel.RootElements.Single() as RailwayContainer;
            Assert.IsNotNull(railway);
            return railway;
        }

        [TestInitialize]
        public void Setup()
        {
            repository1 = new ModelRepository();
            repository2 = new ModelRepository();
            railway1 = LoadRailwayModel(repository1);
            railway2 = LoadRailwayModel(repository2);
        }

        [TestMethod]
        public void InvertPropertyChangeAttribute()
        {
            var parent = railway1.Routes.First().Entry;
            var newValue = Signal.FAILURE;
            var oldValue = repository1.Resolve(parent.AbsoluteUri).GetType().GetProperty("Signal").GetValue(parent, null);

            var change = new AttributePropertyChange
            {
                AffectedElement = parent,
                Feature = Semaphore.ClassInstance.Resolve(new Uri("signal", UriKind.Relative)) as ITypedElement,
                OldValue = oldValue.ToString(),
                NewValue = newValue.ToString()
            };

            change.Apply();
            foreach (var inverted in change.Invert())
            {
                inverted.Apply();
            }

            Assert.AreEqual(oldValue, parent.Signal);
        }

        [TestMethod]
        public void InvertPropertyChangeReference()
        {
            var parent = railway1.Routes[0];
            var newValue = railway1.Semaphores[0];
            var oldValue = parent.Entry;

            var change = new AssociationPropertyChange
            {
                AffectedElement = parent,
                Feature = Route.ClassInstance.LookupReference("entry"),
                OldValue = oldValue,
                NewValue = newValue
            };

            change.Apply();
            foreach (var inverted in change.Invert())
            {
                inverted.Apply();
            }

            Assert.AreSame(oldValue, parent.Entry);
        }

        [TestMethod]
        public void InvertListInsertionComposition()
        {
            var toInsert = new Route();

            var change = new CompositionListInsertion
            {
                AffectedElement = railway1,
                Feature = RailwayContainer.ClassInstance.LookupReference("routes"),
                AddedElement = toInsert,
                Index = 0
            };

            change.Apply();
            foreach (var inverted in change.Invert())
            {
                inverted.Apply();
            }

            Assert.AreEqual(railway1.Routes.Count, railway2.Routes.Count);
            CollectionAssert.DoesNotContain(railway1.Routes.ToList(), toInsert);
        }

        [TestMethod]
        public void InvertListInsertionAssociation()
        {
            var parent1 = railway1.Routes[0].DefinedBy[0].Elements[0];
            var parent2 = railway2.Routes[0].DefinedBy[0].Elements[0];
            var toInsert = railway1.Routes[0].DefinedBy[1].Elements[0];

            var change = new AssociationListInsertion
            {
                AffectedElement = parent1,
                Feature = TrackElement.ClassInstance.LookupReference("connectsTo"),
                Index = 0,
                AddedElement = toInsert
            };

            change.Apply();
            foreach (var inverted in change.Invert())
            {
                inverted.Apply();
            }

            Assert.AreNotSame(toInsert, parent1.ConnectsTo[0]);
            Assert.AreEqual(parent1.ConnectsTo.Count, parent2.ConnectsTo.Count);
        }

        [TestMethod]
        public void InvertCollectionInsertionComposition()
        {
            var toInsert = new Route();

            var change = new CompositionCollectionInsertion
            {
                AffectedElement = railway1,
                Feature = RailwayContainer.ClassInstance.LookupReference("routes"),
                AddedElement = toInsert
            };

            change.Apply();
            foreach (var inverted in change.Invert())
            {
                inverted.Apply();
            }

            CollectionAssert.DoesNotContain(railway1.Routes.ToList(), toInsert);
            Assert.AreEqual(railway1.Routes.Count, railway2.Routes.Count);
        }

        [TestMethod]
        public void InvertCollectionInsertionAssociation()
        {
            var parent1 = railway1.Routes[0].DefinedBy[0].Elements[0];
            var parent2 = railway1.Routes[0].DefinedBy[0].Elements[0];
            var toInsert = railway1.Routes[0].DefinedBy[1].Elements[0];

            var change = new AssociationCollectionInsertion
            {
                AffectedElement = parent1,
                Feature = TrackElement.ClassInstance.LookupReference("connectsTo"),
                AddedElement = toInsert
            };

            change.Apply();
            foreach (var inverted in change.Invert())
            {
                inverted.Apply();
            }

            Assert.AreNotSame(toInsert, parent1.ConnectsTo[parent1.ConnectsTo.Count - 1]);
            Assert.AreEqual(parent2.ConnectsTo.Count, parent1.ConnectsTo.Count);
        }


        [TestMethod]
        public void InvertListDeletionComposition()
        {
            var toDelete = railway1.Routes[0];

            var change = new CompositionListDeletion
            {
                AffectedElement = railway1,
                Feature = RailwayContainer.ClassInstance.LookupReference("routes"),
                Index = 0,
                DeletedElement = toDelete
            };

            change.Apply();
            foreach (var inverted in change.Invert())
            {
                inverted.Apply();
            }

            CollectionAssert.Contains(railway1.Routes.ToList(), toDelete);
            Assert.AreEqual(railway1.Routes.Count, railway2.Routes.Count);
        }

        [TestMethod]
        public void InvertListDeletionAssociation()
        {
            var parent1 = railway1.Routes[0].DefinedBy[0].Elements[0];
            var parent2 = railway1.Routes[0].DefinedBy[0].Elements[0];
            var toDelete = parent1.ConnectsTo[0];

            var change = new AssociationListDeletion
            {
                AffectedElement = parent1,
                Feature = TrackElement.ClassInstance.LookupReference("connectsTo"),
                Index = 0,
                DeletedElement = toDelete
            };

            change.Apply();
            foreach (var inverted in change.Invert())
            {
                inverted.Apply();
            }

            Assert.AreEqual(parent1.ConnectsTo.Count, parent2.ConnectsTo.Count);
            CollectionAssert.Contains(parent1.ConnectsTo.ToList(), toDelete);
        }

        [TestMethod]
        public void InvertCollectionDeletionComposition()
        {
            var toDelete = railway1.Routes[0];

            var change = new CompositionCollectionDeletion
            {
                AffectedElement = railway1,
                Feature = RailwayContainer.ClassInstance.LookupReference("routes"),
                DeletedElement = toDelete
            };

            change.Apply();
            foreach (var inverted in change.Invert())
            {
                inverted.Apply();
            }

            CollectionAssert.Contains(railway1.Routes.ToList(), toDelete);
            Assert.AreEqual(railway1.Routes.Count, railway2.Routes.Count);
        }

        [TestMethod]
        public void InvertChangeTransaction()
        {
            var rec = new ModelChangeRecorder(true);
            rec.Start(railway1);

            railway1.Semaphores.RemoveAt(0);
            rec.Stop();
            var changes = rec.GetModelChanges().Changes[0];

            foreach (var inverted in changes.Invert())
            {
                inverted.Apply();
            }

            Assert.AreEqual(railway1.Semaphores.Count, railway2.Semaphores.Count);
        }
    }
}
