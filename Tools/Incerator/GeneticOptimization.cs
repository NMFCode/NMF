using GAF;
using GAF.Operators;
using GAF.Threading;
using NMF.Expressions.IncrementalizationConfiguration;
using NMF.Optimizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Incerator
{
    public class GeneticOptimization
    {
        private IBenchmark<Configuration> benchmark;
        private Configuration baseConfiguration;

        public GeneticOptimization(IBenchmark<Configuration> benchmark, Configuration baseConfiguration)
        {
            this.benchmark = benchmark;
            this.baseConfiguration = baseConfiguration;
        }

        public IEnumerable<MeasuredConfiguration<Configuration>> Optimize(int maxGenerations, int populationSize)
        {
            Console.WriteLine("Creating initial population with size {0}", populationSize);

            var population = new Population();
            var rnd = RandomProvider.GetThreadRandom();
            for (int i = 0; i < populationSize; i++)
            {
                var chromosome = new Chromosome();
                for (int j = 0; j < baseConfiguration.MethodConfigurations.Count; j++)
                {
                    chromosome.Genes.Add(new Gene(rnd.Next(baseConfiguration.MethodConfigurations[j].AllowedStrategies.Count)));
                }
                population.Solutions.Add(chromosome);
            }

            var ga = new GeneticAlgorithm(population, CalculateFitness);

            ga.Operators.Add(new Elite(5));
            ga.Operators.Add(new Crossover(0.85, false, CrossoverType.DoublePoint));
            ga.Operators.Add(new StrategyMutate(0.08, baseConfiguration));

            ga.OnGenerationComplete += OnGenerationComplete;

            Console.WriteLine("Run genetic algorithm");
            ga.Run((pop, generation, evaluations) => generation >= maxGenerations);

            return ga.Population.Solutions.Distinct(new ChromosomeComparer()).Select(ch => (MeasuredConfiguration<Configuration>)ch.Tag);
        }

        private class ChromosomeComparer : IEqualityComparer<Chromosome>
        {
            public bool Equals(Chromosome x, Chromosome y)
            {
                if ((x != null) != (y != null)) return false;
                if (x != null)
                {
                    return x.ToBinaryString() == y.ToBinaryString();
                }
                else
                {
                    return true;
                }
            }

            public int GetHashCode(Chromosome obj)
            {
                if (obj == null) return 0;
                return obj.ToBinaryString().GetHashCode();
            }
        }

        private void OnGenerationComplete(object sender, GaEventArgs e)
        {
            Console.WriteLine("Completed generation {0} with {1} evaluations in total.", e.Generation, e.Evaluations);
        }

        private double CalculateFitness(Chromosome solution)
        {
            if (solution.Tag != null)
            {
                var oldMeasurement = (MeasuredConfiguration<Configuration>)solution.Tag;
                return System.Math.Exp(-oldMeasurement.Measurements["Time"]);
            }
            var configuration = baseConfiguration.Clone();
            for (int i = 0; i < baseConfiguration.MethodConfigurations.Count; i++)
            {
                baseConfiguration.MethodConfigurations[i].Strategy = baseConfiguration.MethodConfigurations[i].AllowedStrategies[solution.Genes[i].BinaryValue];
            }
            var measurement = benchmark.MeasureConfiguration(configuration);
            solution.Tag = new MeasuredConfiguration<Configuration>(configuration, measurement);
            return System.Math.Exp(-measurement["Time"]);
        }

        private class StrategyMutate : BinaryMutate
        {
            private Configuration baseConfiguration;

            public StrategyMutate(double mutationProbability, Configuration baseConfiguration) : base(mutationProbability)
            {
                this.baseConfiguration = baseConfiguration;
            }

            protected override void Mutate(Chromosome child)
            {
                if (child.IsElite) return;
                var rnd = RandomProvider.GetThreadRandom();
                for (int i = 0; i < child.Genes.Count; i++)
                {
                    var gene = child.Genes[i];
                    if (rnd.NextDouble() <= MutationProbability)
                    {
                        gene.ObjectValue = rnd.Next(baseConfiguration.MethodConfigurations[i].AllowedStrategies.Count);
                    }
                }
            }

            public override void Invoke(Population currentPopulation, ref Population newPopulation, FitnessFunction fitnessFunctionDelegate)
            {
                if (newPopulation == null)
                {
                    newPopulation = currentPopulation.CreateEmptyCopy();
                }
                if (!this.Enabled)
                {
                    return;
                }
                if (currentPopulation.Solutions == null || currentPopulation.Solutions.Count == 0)
                {
                    throw new ArgumentException("There are no Solutions in the current Population.");
                }
                newPopulation.Solutions.Clear();
                newPopulation.Solutions.AddRange(currentPopulation.Solutions);
                List<Chromosome> nonElites = newPopulation.GetNonElites();
                foreach (Chromosome current in nonElites)
                {
                    this.Mutate(current);
                }
            }
        }
    }
}
