using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace ConfigSync
{
    public class ConfigSynchronizer
    {
        private readonly IConfiguration _sourceConfig;
        public IConfiguration _targetConfig;
        private ILogger _logger;

        public ConfigSynchronizer(IConfiguration sourceConfig, ILogger logger=null)
        {
            _sourceConfig = sourceConfig; 
            _logger = logger;
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
            var diffs = GetDifferences();
            if (_targetConfig is IConfigurationRoot targetRoot)
            {
                foreach (var diff in diffs)
                {
                    string key = diff.Key;
                    string value = _sourceConfig[key];
                    _logger?.LogInformation($"Syncing {key}: {diff.Value}");
                    
                }
            }
        }

        public Dictionary<string, string> GetDifferences()
        {
            var diffs = new Dictionary<string, string>();
            foreach (var pair in _sourceConfig.AsEnumerable())
            {
                if (pair.Value == null) continue; 
                string targetValue = _targetConfig[pair.Key];
                if (targetValue == null)
                {
                    diffs[pair.Key] = $"Missing in target, added {pair.Value}";
                }
                else if (targetValue != pair.Value)
                {
                    diffs[pair.Key] = $"From {targetValue} to {pair.Value}";
                }
            }
            return diffs;
        }
    }
}
