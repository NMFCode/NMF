using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMF.Utilities;

namespace NMF.Expressions.IncrementalizationConfiguration
{
    public static class ConfigurationExtensions
    {
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

        public static IEnumerable<Configuration> GetAllPossibilities(Configuration baseConfiguration)
        {
            var nextIndices = new int[baseConfiguration.MethodConfigurations.Count];
            var nextToIncrease = nextIndices.GetUpperBound(0);

            while (true)
            {
                var config = baseConfiguration.Clone();
                for (int i = 0; i < nextIndices.Length; i++)
                {
                    config.MethodConfigurations[i].Strategy = baseConfiguration.MethodConfigurations[i].AllowedStrategies[nextIndices[0]];
                }
                yield return config;
                nextIndices[nextToIncrease]++;
                while (nextIndices[nextToIncrease] == baseConfiguration.MethodConfigurations[nextToIncrease].AllowedStrategies.Count)
                {
                    nextIndices[nextToIncrease] = 0;
                    nextToIncrease--;
                    if (nextToIncrease < 0) yield break;
                    nextIndices[nextToIncrease]++;
                }
            }
        }
    }
}
