using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace ConfigSync
{
    public class ConfigSynchronizer
    {
        private readonly IConfiguration _sourceConfig;
        private IConfiguration _targetConfig;

        public ConfigSynchronizer(IConfiguration sourceConfig)
        {
            _sourceConfig = sourceConfig; // The "main" settings to sync from
        }

        public ConfigSynchronizer FromJson(string jsonPath)
        {
            _targetConfig = new ConfigurationBuilder()
                .AddJsonFile(jsonPath)
                .Build();
            return this; 
        }

        public ConfigSynchronizer ToEnvVars()
        {
            _targetConfig = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();
            return this;
        }

        public void Sync()
        {
            //ToDo: sync logic
        }

        public Dictionary<string, string> GetDifferences()
        {
            var diffs = new Dictionary<string, string>();
            foreach (var pair in _sourceConfig.AsEnumerable())
            {
                string targetValue = _targetConfig[pair.Key];
                if (targetValue != pair.Value && pair.Value != null)
                {
                    diffs[pair.Key] = $"From {targetValue} to {pair.Value}";
                }
            }
            return diffs;
        }
    }
}
