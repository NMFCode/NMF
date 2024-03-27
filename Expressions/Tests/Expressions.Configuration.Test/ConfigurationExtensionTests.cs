using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Expressions.IncrementalizationConfiguration;
using NMF.Utilities;

namespace NMF.Expressions.Tests
{
    [TestClass]
    public class ConfigurationExtensionTests
    {
        [TestMethod]
        public void GetAllPossibilities()
        {
            var config = new Configuration();
            config.MethodConfigurations.Add(CreateMethodConfiguration("Foo", IncrementalizationStrategy.ArgumentPromotion,
                IncrementalizationStrategy.InstructionLevel, IncrementalizationStrategy.ArgumentPromotion, IncrementalizationStrategy.ListenRepositoryChanges));
            config.MethodConfigurations.Add(CreateMethodConfiguration("Bar", IncrementalizationStrategy.ListenRepositoryChanges,
                IncrementalizationStrategy.InstructionLevel, IncrementalizationStrategy.UseAugmentation, IncrementalizationStrategy.ListenRepositoryChanges));

            var test = config.GetAllPossibilities().ToList();

            Assert.AreEqual(9, test.Count);

            Assert.IsTrue(test.Any(conf => FindConfiguration(conf, "Foo") == IncrementalizationStrategy.ArgumentPromotion && FindConfiguration(conf, "Bar") == IncrementalizationStrategy.UseAugmentation));
            Assert.IsTrue(test.Any(conf => FindConfiguration(conf, "Foo") == IncrementalizationStrategy.ArgumentPromotion && FindConfiguration(conf, "Bar") == IncrementalizationStrategy.ListenRepositoryChanges));
            Assert.IsTrue(test.Any(conf => FindConfiguration(conf, "Foo") == IncrementalizationStrategy.ListenRepositoryChanges && FindConfiguration(conf, "Bar") == IncrementalizationStrategy.UseAugmentation));
            Assert.IsTrue(test.Any(conf => FindConfiguration(conf, "Foo") == IncrementalizationStrategy.ListenRepositoryChanges && FindConfiguration(conf, "Bar") == IncrementalizationStrategy.ListenRepositoryChanges));
        }

        [TestMethod]
        public void GetAllPossibilities2()
        {
            var config = new Configuration();
            config.MethodConfigurations.Add(CreateMethodConfiguration("Foo", IncrementalizationStrategy.ArgumentPromotion,
                IncrementalizationStrategy.ArgumentPromotion, IncrementalizationStrategy.ListenRepositoryChanges));
            config.MethodConfigurations.Add(CreateMethodConfiguration("Bar", IncrementalizationStrategy.ListenRepositoryChanges,
                IncrementalizationStrategy.UseAugmentation, IncrementalizationStrategy.ListenRepositoryChanges));
            config.MethodConfigurations.Add(CreateMethodConfiguration("Test", IncrementalizationStrategy.InstructionLevel,
                IncrementalizationStrategy.InstructionLevel, IncrementalizationStrategy.ArgumentPromotion));

            var test = config.GetAllPossibilities().ToList();

            Assert.AreEqual(8, test.Count);

            Assert.IsTrue(test.Any(conf => FindConfiguration(conf, "Foo") == IncrementalizationStrategy.ArgumentPromotion && FindConfiguration(conf, "Bar") == IncrementalizationStrategy.UseAugmentation));
            Assert.IsTrue(test.Any(conf => FindConfiguration(conf, "Foo") == IncrementalizationStrategy.ArgumentPromotion && FindConfiguration(conf, "Bar") == IncrementalizationStrategy.ListenRepositoryChanges));
            Assert.IsTrue(test.Any(conf => FindConfiguration(conf, "Foo") == IncrementalizationStrategy.ListenRepositoryChanges && FindConfiguration(conf, "Bar") == IncrementalizationStrategy.UseAugmentation));
            Assert.IsTrue(test.Any(conf => FindConfiguration(conf, "Foo") == IncrementalizationStrategy.ListenRepositoryChanges && FindConfiguration(conf, "Bar") == IncrementalizationStrategy.ListenRepositoryChanges));
        }

        private IncrementalizationStrategy FindConfiguration(Configuration config, string name)
        {
            return config.MethodConfigurations.FirstOrDefault(c => c.MethodIdentifier == name).Strategy;
        }

        private MethodConfiguration CreateMethodConfiguration(string name, IncrementalizationStrategy strategy, params IncrementalizationStrategy[] allowed)
        {
            var conf = new MethodConfiguration()
            {
                MethodIdentifier = name,
                Strategy = strategy
            };
            conf.AllowedStrategies.AddRange(allowed);
            return conf;
        }
    }
}
