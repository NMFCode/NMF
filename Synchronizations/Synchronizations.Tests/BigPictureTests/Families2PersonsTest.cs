using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Models;
using NMF.Models.Repository;
using NMF.Synchronizations;
using NMF.Synchronizations.Example;
using NMF.Transformations;
using System.Linq;
using NMF.Synchronizations.Example.Persons;
using NMF.Synchronizations.Example.Families;

namespace Synchronizations.Tests.BigPictureTests
{
    [TestClass]
    public class Families2PersonsTest
    {
        private ModelRepository repository;
        private Model familyModel;
        private FamiliesToPersons familiesToPersons;

        [TestInitialize]
        public void LoadModel()
        {
            familiesToPersons = new FamiliesToPersons();
            familiesToPersons.Initialize();
            repository = new ModelRepository();
            familyModel = repository.Resolve("SampleFamilies.xmi");
        }

        [TestMethod]
        public void FamiliesToPersons_NoChangePropagation()
        {
            Model personsModel = null;
            familiesToPersons.Synchronize(ref familyModel, ref personsModel, SynchronizationDirection.LeftToRight, ChangePropagationMode.None);

            AssertInitialPersonsModel(personsModel);
        }

        [TestMethod]
        public void FamiliesToPersons_OneWayChangePropagation()
        {
            Model personsModel = null;
            familiesToPersons.Synchronize(ref familyModel, ref personsModel, SynchronizationDirection.LeftToRight, ChangePropagationMode.OneWay);

            AssertInitialPersonsModel(personsModel);
            AddFamilyDoe(personsModel);
            ChangeFatherNameMarch(personsModel);
        }

        [TestMethod]
        public void FamiliesToPersons_TwoWayChangePropagation()
        {
            Model personsModel = null;
            familiesToPersons.Synchronize(ref familyModel, ref personsModel, SynchronizationDirection.LeftToRight, ChangePropagationMode.TwoWay);

            AssertInitialPersonsModel(personsModel);
            AddFamilyDoe(personsModel);
            ChangeFatherNameMarch(personsModel);
        }

        [TestMethod]
        public void FamiliesToPersons_NoChangePropagation_Forced()
        {
            Model personsModel = null;
            familiesToPersons.Synchronize(ref familyModel, ref personsModel, SynchronizationDirection.LeftToRightForced, ChangePropagationMode.None);

            AssertInitialPersonsModel(personsModel);
        }

        [TestMethod]
        public void FamiliesToPersons_OneWayChangePropagation_Forced()
        {
            Model personsModel = null;
            familiesToPersons.Synchronize(ref familyModel, ref personsModel, SynchronizationDirection.LeftToRightForced, ChangePropagationMode.OneWay);

            AssertInitialPersonsModel(personsModel);
            AddFamilyDoe(personsModel);
            ChangeFatherNameMarch(personsModel);
        }

        [TestMethod]
        public void FamiliesToPersons_TwoWayChangePropagation_Forced()
        {
            Model personsModel = null;
            familiesToPersons.Synchronize(ref familyModel, ref personsModel, SynchronizationDirection.LeftToRightForced, ChangePropagationMode.TwoWay);

            AssertInitialPersonsModel(personsModel);
            AddFamilyDoe(personsModel);
            ChangeFatherNameMarch(personsModel);
        }

        [TestMethod]
        public void FamiliesToPersons_NoChangePropagation_LeftWins()
        {
            Model personsModel = null;
            familiesToPersons.Synchronize(ref familyModel, ref personsModel, SynchronizationDirection.LeftWins, ChangePropagationMode.None);

            AssertInitialPersonsModel(personsModel);
        }

        [TestMethod]
        public void FamiliesToPersons_OneWayChangePropagation_LeftWins()
        {
            Model personsModel = null;
            familiesToPersons.Synchronize(ref familyModel, ref personsModel, SynchronizationDirection.LeftWins, ChangePropagationMode.OneWay);

            AssertInitialPersonsModel(personsModel);
            AddFamilyDoe(personsModel);
            ChangeFatherNameMarch(personsModel);
        }

        [TestMethod]
        public void FamiliesToPersons_TwoWayChangePropagation_LeftWins()
        {
            Model personsModel = null;
            familiesToPersons.Synchronize(ref familyModel, ref personsModel, SynchronizationDirection.LeftWins, ChangePropagationMode.TwoWay);

            AssertInitialPersonsModel(personsModel);
            AddFamilyDoe(personsModel);
            ChangeFatherNameMarch(personsModel);
        }

        private void ChangeFatherNameMarch(Model personsModel)
        {
            Assert.IsNotNull(FindPerson(personsModel, "Jane Doe"));

            var jimMarch = (familyModel.RootElements[0] as Family).Father;
            var jimMarchTransformed = FindPerson(personsModel, "Jim March");
            var nameChanged = false;

            jimMarchTransformed.FullNameChanged += (o, e) =>
            {
                Assert.AreEqual("Jim March", e.OldValue);
                Assert.AreEqual("Samuel March", e.NewValue);
                nameChanged = true;
            };

            jimMarch.FirstName = "Samuel";

            Assert.IsTrue(nameChanged);

            Assert.IsNull(FindPerson(personsModel, "Jim March"));
            Assert.IsNotNull(FindPerson(personsModel, "Samuel March"));
        }

        private void AddFamilyDoe(Model personsModel)
        {
            var newFamily = new Family();
            newFamily.LastName = "Doe";
            newFamily.Father = new Member() { FirstName = "John" };
            newFamily.Mother = new Member() { FirstName = "Jane" };
            familyModel.RootElements.Add(newFamily);

            Assert.AreEqual(11, personsModel.RootElements.Count);
            Assert.AreEqual(6, personsModel.RootElements.OfType<Male>().Count());
            Assert.AreEqual(5, personsModel.RootElements.OfType<Female>().Count());

            Assert.IsNotNull(FindPerson(personsModel, "John Doe"));
        }

        private static void AssertInitialPersonsModel(Model personsModel)
        {
            Assert.IsNotNull(personsModel);
            Assert.AreEqual(9, personsModel.RootElements.Count);
            Assert.AreEqual(5, personsModel.RootElements.OfType<Male>().Count());
            Assert.AreEqual(4, personsModel.RootElements.OfType<Female>().Count());

            Assert.IsNotNull(FindPerson(personsModel, "Jim March"));
            Assert.IsNotNull(FindPerson(personsModel, "Cindy March"));
            Assert.IsNotNull(FindPerson(personsModel, "Brandon March"));
            Assert.IsNotNull(FindPerson(personsModel, "Brenda March"));
            Assert.IsNotNull(FindPerson(personsModel, "Peter Sailor"));
            Assert.IsNotNull(FindPerson(personsModel, "Jackie Sailor"));
            Assert.IsNotNull(FindPerson(personsModel, "David Sailor"));
            Assert.IsNotNull(FindPerson(personsModel, "Dylan Sailor"));
            Assert.IsNotNull(FindPerson(personsModel, "Kelly Sailor"));
        }

        private static Person FindPerson(Model personsModel, string fullName)
        {
            return personsModel.RootElements.OfType<Person>().FirstOrDefault(p => p.FullName == fullName);
        }
    }
}
