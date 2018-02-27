using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Models;
using NMF.Models.Changes;
using NMF.Models.Meta;
using NMF.Synchronizations;
using NMF.Synchronizations.Example.Persons;
using NMF.Synchronizations.Models;
using NMF.Transformations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Synchronizations.Models.Tests
{
    [TestClass]
    public class SynchronizationAsDifferencingTests
    {
        private TestSynchronization transformation;

        [TestInitialize]
        public void Setup()
        {
            transformation = new TestSynchronization();
            transformation.Initialize();
        }

        [TestMethod]
        public void DifferencingForSimpleDifferencesWorks()
        {
            var model = new Model();
            var lisa = new Female { FullName = "Lisa Simpson" };
            model.RootElements.Add(new Male { FullName = "Homer Simpson" });
            model.RootElements.Add(new Female { FullName = "Marge Simpson" });
            model.RootElements.Add(new Male { FullName = "Bart Simpson" });
            model.RootElements.Add(lisa);
            model.RootElements.Add(new Female { FullName = "Maggie Simpson" });

            var copy = Copy(model);
            lisa.FullName = "Lisa Flanders";

            var diff = GetDiff<IModel>(copy, model);
            Assert.AreEqual(2, diff.Changes.Count);
            var insert = diff.Changes.OfType<CompositionListInsertion>().FirstOrDefault();
            var delete = diff.Changes.OfType<CompositionListDeletion>().FirstOrDefault();
            Assert.IsNotNull(insert);
            Assert.IsNotNull(delete);
            Assert.AreEqual("Lisa Simpson", (delete.DeletedElement as IFemale).FullName);
            Assert.AreEqual("Lisa Flanders", (insert.AddedElement as IFemale).FullName);
        }

        private ModelChangeSet GetDiff<T>(T from, T to)
            where T : class, IModelElement
        {
            var recorder = new ModelChangeRecorder(false);
            recorder.Start(from);
            transformation.Synchronize(ref from, ref to, SynchronizationDirection.RightToLeftForced, ChangePropagationMode.None);
            recorder.Stop();
            return recorder.GetModelChanges();
        }

        private Model Copy(Model model)
        {
            var newModel = new Model();
            newModel.ModelUri = model.ModelUri;
            foreach (var person in model.RootElements)
            {
                if (person is Female woman)
                {
                    newModel.RootElements.Add(new Female
                    {
                        FullName = woman.FullName
                    });
                }
                else if (person is Male man)
                {
                    newModel.RootElements.Add(new Male
                    {
                        FullName = man.FullName
                    });
                }
            }
            return newModel;
        }
    }

    public class TestSynchronization : HomogeneousSynchronization<Person>
    {
        [OverrideRule]
        public class FemaleCheck : ModelCopyRule<IFemale>
        {
            public override bool ShouldCorrespond(IFemale left, IFemale right, ISynchronizationContext context)
            {
                return left.FullName == right.FullName;
            }
        }

        [OverrideRule]
        public class MaleCheck : ModelCopyRule<IMale>
        {
            public override bool ShouldCorrespond(IMale left, IMale right, ISynchronizationContext context)
            {
                return left.FullName == right.FullName;
            }
        }
    }
}
