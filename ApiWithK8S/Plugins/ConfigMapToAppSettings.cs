using ApiWithK8S.Extensions;
using k8s;
using System.Text.Json;

namespace ApiWithK8S.Plugins
{
    public class ConfigMapToAppSettings
    {
        private readonly IKubernetes _kubernetesClient;
        private readonly string _namespace;
        private readonly string _environment;
        private readonly string[] _configMapKeys;
        
        public ConfigMapToAppSettings(string environment)
        {
            _environment = environment;
            // Initialize Kubernetes client using the in-cluster config
            var config = KubernetesClientConfiguration.InClusterConfig();
            _kubernetesClient = new Kubernetes(config);

            // Get namespace from the mounted service account
            _namespace = File.ReadAllText("/var/run/secrets/kubernetes.io/serviceaccount/namespace");

            // Get ConfigMap keys from environment variable (comma-separated)
            _configMapKeys = Environment.GetEnvironmentVariable("CONFIGMAP_KEYS")?.Split(',')
                ?? throw new ArgumentNullException("CONFIGMAP_KEYS environment variable is not set");
        }

        public async Task GenerateAppSettingsAsync()
        {
            try
            {
                var configurationData = new Dictionary<string, object>();

                foreach (var configMapKey in _configMapKeys)
                {
                    var configMap = await _kubernetesClient.CoreV1.ReadNamespacedConfigMapAsync(
                        configMapKey.Trim(),
                        _namespace);

                    if (configMap == null || configMap.Data == null)
                    {
                        Console.WriteLine($"Warning: ConfigMap {configMapKey} not found or empty");
                        continue;
                    }

                    MergeConfigMapData(configurationData, configMap.Data);
                }

                await WriteAppSettingsFileAsync(configurationData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating appsettings: {ex.Message}");
                throw;
            }
        }

        private void MergeConfigMapData(Dictionary<string, object?> configurationData,
            IDictionary<string, string> configMapData)
        {
            foreach (var item in configMapData)
            {
                try
                {
                    // Check if the value is a JSON object
                    if (IsJsonObject(item.Value))
                    {
                        var jsonObject = JsonSerializer.Deserialize<Dictionary<string, object>>(item.Value);
                        MergeNestedDictionary(configurationData, item.Key, jsonObject);
                    }
                    else
                    {
                        // Handle nested keys (e.g., "Logging:LogLevel:Default")
                        var keyParts = item.Key.Split(':');
                        AddNestedValue(configurationData, keyParts, item.Value);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing ConfigMap item {item.Key}: {ex.Message}");
                }
            }
        }

        private bool IsJsonObject(string value)
        {
            try
            {
                JsonDocument.Parse(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void MergeNestedDictionary(Dictionary<string, object?> target, string key, Dictionary<string, object>? source)
        {
            if (!target.ContainsKey(key))
            {
                target[key] = source;
            }
            else if (target[key] is Dictionary<string, object?> existingDict)
            {
                if (source != null)
                {
                    foreach (var item in source)
                    {
                        existingDict[item.Key] = item.Value;
                    }
                }
            }
        }

        private void AddNestedValue(Dictionary<string, object?> dict, string[] keyParts, string value)
        {
            var current = dict;
            for (int i = 0; i < keyParts.Length - 1; i++)
            {
                var key = keyParts[i];
                if (!current!.ContainsKey(key))
                {
                    current[key] = new Dictionary<string, object>();
                }
                current = current[key] as Dictionary<string, object?>;
            }

            var lastKey = keyParts[keyParts.Length - 1];
            current![lastKey] = value;
        }

        private async Task WriteAppSettingsFileAsync(Dictionary<string, object> configurationData)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(configurationData, options);
            var filePath = Path.Combine(AppContext.BaseDirectory, @$"appsettings.{_environment.CapitalizeFirstCharacter}.json");

            await File.WriteAllTextAsync(filePath, json);
            Console.WriteLine($"Successfully generated appsettings.{_environment.CapitalizeFirstCharacter}.json at {filePath}");
        }
    }
}

