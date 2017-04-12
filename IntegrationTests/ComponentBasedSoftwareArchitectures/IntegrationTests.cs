using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Models.Repository;
using NMF.Models;
using NMFExamples.Pcm.Repository;
using NMFExamples.Pcm.System;
using NMFExamples.Pcm.Resourceenvironment;
using NMFExamples.Pcm.Allocation;
using System.IO;
using NMF.Models.Changes;
using NMFExamples.ComponentBasedSystems;
using NMF.Expressions;

namespace NMFExamples
{
    [TestClass]
    public class IntegrationTests
    {
        [TestMethod]
        public void LoadDefaultAndCheckConnections()
        {
            LoadModelAndCheck("default", PcmToComponentBasedSystems.AnalyseConnectionViolations, PcmToComponentBasedSystems.AnalyzeConnectionViolationsInc);
        }

        [TestMethod]
        public void LoadDefaultAndCheckAllocations()
        {
            LoadModelAndCheck("default", PcmToComponentBasedSystems.AnalyzeAllocationViolations, PcmToComponentBasedSystems.AnalyzeAllocationViolationsInc);
        }

        [TestMethod]
        public void LoadWithCacheAndCheckConnections()
        {
            LoadModelAndCheck("WithCache", PcmToComponentBasedSystems.AnalyseConnectionViolations, PcmToComponentBasedSystems.AnalyzeConnectionViolationsInc);
        }

        [TestMethod]
        public void LoadWithCacheAndCheckAllocations()
        {
            LoadModelAndCheck("WithCache", PcmToComponentBasedSystems.AnalyzeAllocationViolations, PcmToComponentBasedSystems.AnalyzeAllocationViolationsInc);
        }

        [TestMethod]
        public void LoadPrioritizingMediaStoreAndCheckConnections()
        {
            LoadModelAndCheck("PrioritizingMediaStore", PcmToComponentBasedSystems.AnalyseConnectionViolations, PcmToComponentBasedSystems.AnalyzeConnectionViolationsInc);
        }

        [TestMethod]
        public void LoadPrioritizingMediaStoreAndCheckAllocations()
        {
            LoadModelAndCheck("PrioritizingMediaStore", PcmToComponentBasedSystems.AnalyzeAllocationViolations, PcmToComponentBasedSystems.AnalyzeAllocationViolationsInc);
        }

        public void LoadModelAndCheck(string model, Func<Root_MM06, int> analysis, Func<Root_MM06, INotifyValue<int>> incAnalysis)
        {
            var pcmChangePath = CreateTempPathPattern();
            var cbsChangePath = CreateTempPathPattern();
            try
            {
                GenerateChanges(model, pcmChangePath);
                TransformChanges(model, pcmChangePath, cbsChangePath, (r1,r2) =>
                {
                    Assert.AreEqual(analysis(r1), analysis(r2));
                });
                CheckIncAnalysis(cbsChangePath, analysis, incAnalysis);
            }
            finally
            {
                DeleteAllThatExist(pcmChangePath);
                DeleteAllThatExist(cbsChangePath);
            }
        }

        private static void DeleteAllThatExist(string pathPattern)
        {
            for (int i = 0; i < PcmChangeGenerator.NumChanges; i++)
            {
                var pcmPath = string.Format(pathPattern, i);
                if (File.Exists(pcmPath))
                {
                    File.Delete(pcmPath);
                }
            }
            var initial = string.Format(pathPattern, "initial");
            if (File.Exists(initial))
            {
                File.Delete(initial);
            }
        }

        private static string CreateTempPathPattern()
        {
            return Path.GetTempFileName().Replace('{', 'a').Replace('}', 'b') + "_{0}.xmi";
        }

        private void GenerateChanges(string pcmModel, string target)
        {
            var repository = new ModelRepository();
            var model = repository.Resolve(pcmModel + ".xmi");

            var pcmRepository = model.RootElements.OfType<Repository>().Single();
            var pcmSystem = model.RootElements.OfType<System0>().Single();
            var pcmResourceEnvironment = model.RootElements.OfType<ResourceEnvironment>().Single();
            var pcmAllocation = model.RootElements.OfType<Allocation>().Single();
            
            PcmChangeGenerator.PerformChanges(pcmRepository, pcmSystem, pcmResourceEnvironment, pcmAllocation, model, target);
        }

        private void TransformChanges(string pcmModel, string changes, string target, Action<Root_MM06, Root_MM06> compareResults)
        {
            var repository = new ModelRepository();
            var model = repository.Resolve(pcmModel + ".xmi");
            var targetModel = PcmToComponentBasedSystems.Transform(model, NMF.Transformations.ChangePropagationMode.OneWay);
            var targetRoot = targetModel.RootElements[0] as Root_MM06;

            repository.Save(targetModel, string.Format(target, "initial"));

            for (int i = 0; i < PcmChangeGenerator.NumChanges; i++)
            {
                var change = repository.Resolve(string.Format(changes, i)).RootElements[0] as ModelChangeSet;
                var recorder = new ModelChangeRecorder();
                recorder.Start(targetModel);
                foreach (var subChange in change.Changes)
                {
                    subChange.Apply(repository);
                }
                recorder.Stop();
                if (compareResults != null)
                {
                    compareResults(targetRoot, PcmToComponentBasedSystems.Transform(model, NMF.Transformations.ChangePropagationMode.None).RootElements[0] as Root_MM06);
                }
                var targetChange = recorder.GetModelChanges();
                repository.Save(targetChange, string.Format(target, i));
            }
        }

        private void CheckIncAnalysis(string pattern, Func<Root_MM06, int> analysis, Func<Root_MM06, INotifyValue<int>> incAnalysis)
        {
            var repository = new ModelRepository();
            var model = repository.Resolve(string.Format(pattern, "initial"));
            var root = model.RootElements[0] as Root_MM06;
            var incResult = incAnalysis(root);
            Assert.AreEqual(incResult.Value, analysis(root));

            for (int i = 0; i < PcmChangeGenerator.NumChanges; i++)
            {
                var changeSet = repository.Resolve(string.Format(pattern, i)).RootElements[0] as ModelChangeSet;
                foreach (var change in changeSet.Changes)
                {
                    change.Apply(repository);
                }
                Assert.AreEqual(incResult.Value, analysis(root));
            }
        }
    }
}
