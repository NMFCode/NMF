using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMF.Utilities;

namespace NMF.Expressions.IncrementalizationConfiguration
{
    /// <summary>
    /// Denotes a helper class for configurations
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Clones the provided configuration
        /// </summary>
        /// <param name="configuration">The configuration to clone</param>
        /// <returns>The cloned configuration</returns>
        public static Configuration Clone(this Configuration configuration)
        {
            if (configuration == null) return null;
            var conf = new Configuration();
            foreach (var methodConf in configuration.MethodConfigurations)
            {
                var mc = new MethodConfiguration()
                {
                    MethodIdentifier = methodConf.MethodIdentifier,
                    Strategy = methodConf.Strategy
                };
                mc.AllowedStrategies.AddRange(methodConf.AllowedStrategies);
                conf.MethodConfigurations.Add(mc);
            }
            return conf;
        }

        /// <summary>
        /// Generates a state-space of possibilities given the provided base configuration
        /// </summary>
        /// <param name="baseConfiguration">The base configuration</param>
        /// <returns>A collection of allowed configurations</returns>
        public static IEnumerable<Configuration> GetAllPossibilities(this Configuration baseConfiguration)
        {
            var nextIndices = new int[baseConfiguration.MethodConfigurations.Count];
            var nextToIncrease = nextIndices.GetUpperBound(0);

            while (true)
            {
                var config = baseConfiguration.Clone();
                for (int i = 0; i < nextIndices.Length; i++)
                {
                    config.MethodConfigurations[i].Strategy = baseConfiguration.MethodConfigurations[i].AllowedStrategies[nextIndices[i]];
                }
                yield return config;
                nextIndices[nextToIncrease]++;
                while (nextIndices[nextToIncrease] >= baseConfiguration.MethodConfigurations[nextToIncrease].AllowedStrategies.Count)
                {
                    nextIndices[nextToIncrease] = 0;
                    nextToIncrease--;
                    if (nextToIncrease < 0) yield break;
                    nextIndices[nextToIncrease]++;
                }
                nextToIncrease = nextIndices.GetUpperBound(0);
            }
        }

        /// <summary>
        /// Describes the current configuration in a string
        /// </summary>
        /// <param name="configuration">The configuration to be described</param>
        /// <returns>A descriptive string</returns>
        public static string Describe(this Configuration configuration)
        {
            return string.Join(",", from mc in configuration.MethodConfigurations
                                    select string.Format("{0}:{1}", mc.MethodIdentifier, mc.Strategy));
        }
    }
}
