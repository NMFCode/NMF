using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace NMF.Optimizations.Tests
{
    [TestClass]
    public class ParetoFilterTests
    {
        [TestMethod]
        public void Pareto_OneDimension_SmallerIsBetter()
        {
            var benchmark = new MockedBenchmark(
                time: n => 10.0 * n + 30,
                memory: n => 20.0 * n + 10);

            var pareto = new ParetoFilter<string>(new Dictionary<string, DimensionRating>()
            {
                { "Time", DimensionRating.SmallerIsBetter }
            });

            var result = pareto.Filter(benchmark.CreateMeasurements(20)).ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(30.0, result.Single().Measurements["Time"]);
        }

        [TestMethod]
        public void Pareto_OneDimension_BiggerIsBetter()
        {
            var benchmark = new MockedBenchmark(
                time: n => 10.0 * n + 30,
                memory: n => 20.0 * n + 10);

            var pareto = new ParetoFilter<string>(new Dictionary<string, DimensionRating>()
            {
                { "Time", DimensionRating.BiggerIsBetter }
            });

            var result = pareto.Filter(benchmark.CreateMeasurements(20)).ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(220.0, result.Single().Measurements["Time"]);
        }

        [TestMethod]
        public void Pareto_TwoDimension_Correlated()
        {
            var benchmark = new MockedBenchmark(
                time: n => 10.0 * n + 30,
                memory: n => 20.0 * n + 10);

            var pareto = new ParetoFilter<string>(new Dictionary<string, DimensionRating>()
            {
                { "Time", DimensionRating.SmallerIsBetter },
                { "Memory", DimensionRating.SmallerIsBetter }
            });

            var result = pareto.Filter(benchmark.CreateMeasurements(20)).ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(30.0, result.Single().Measurements["Time"]);
        }

        [TestMethod]
        public void Pareto_TwoDimension_NotCorrelated()
        {
            var benchmark = new MockedBenchmark(
                time: n => n < 25 ? 10.0 * n + 30
                                  : 10.0 * (n-25) + 10,
                memory: n => n < 25 ? 20.0 * n + 10
                                    : 30.0 * (n-25) + 20);

            var pareto = new ParetoFilter<string>(new Dictionary<string, DimensionRating>()
            {
                { "Time", DimensionRating.SmallerIsBetter },
                { "Memory", DimensionRating.BiggerIsBetter }
            });

            var result = pareto.Filter(benchmark.CreateMeasurements(50)).ToList();

            Assert.AreEqual(25, result.Count);
        }
    }
}
