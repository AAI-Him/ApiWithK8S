using k8s;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace ConfigMapProcessor
{
    public class ConfigMapProcessor
    {
        private readonly IKubernetes _kubernetesClient;
        private readonly string _namespace;
        private readonly string[] _configMapKeys;
        private readonly string _environment;
        private readonly JsonSerializerOptions _jsonOptions;

        public ConfigMapProcessor()
        {
            // Initialize Kubernetes client using in-cluster config
            var config = KubernetesClientConfiguration.InClusterConfig();
            _kubernetesClient = new Kubernetes(config);

            // Get current namespace from mounted service account
            _namespace = File.ReadAllText("/var/run/secrets/kubernetes.io/serviceaccount/namespace");

            // Get ConfigMap keys and environment from environment variables
            _configMapKeys = Environment.GetEnvironmentVariable("CONFIGMAP_KEYS")?.Split(',')
                ?? throw new ArgumentNullException("CONFIGMAP_KEYS environment variable is not set");

            _environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                ?? throw new ArgumentNullException("ASPNETCORE_ENVIRONMENT environment variable is not set");

            // Validate environment
            if (!IsValidEnvironment(_environment))
            {
                throw new ArgumentException($"Invalid environment: {_environment}. Must be Development, Uat, or Production");
            }

            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };
        }

        private bool IsValidEnvironment(string env)
        {
            return env is "Development" or "Uat" or "Production";
        }

        public async Task ProcessConfigMapsAsync()
        {
            try
            {
                var mergedJson = new JsonObject();

                foreach (var configMapKey in _configMapKeys)
                {
                    var configMap = await _kubernetesClient.CoreV1.ReadNamespacedConfigMapAsync(
                        configMapKey.Trim(),
                        _namespace);

                    if (configMap?.Data == null)
                    {
                        Console.WriteLine($"Warning: ConfigMap {configMapKey} not found or empty");
                        continue;
                    }

                    foreach (var (key, value) in configMap.Data)
                    {
                        await ProcessConfigMapEntryAsync(mergedJson, key, value);
                    }
                }

                await WriteAppSettingsFileAsync(mergedJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing ConfigMaps: {ex.Message}");
                throw;
            }
        }

        private async Task ProcessConfigMapEntryAsync(JsonObject mergedJson, string key, string value)
        {
            try
            {
                if (IsJsonObject(value))
                {
                    // Handle JSON object values
                    var jsonNode = JsonNode.Parse(value);
                    MergeJsonNodes(mergedJson, key, jsonNode);
                }
                else if (key.Contains(':'))
                {
                    // Handle hierarchical key-value pairs
                    var parsedValue = ParseValue(value);
                    SetNestedValue(mergedJson, key.Split(':'), parsedValue);
                }
                else
                {
                    // Handle flat key-value pairs
                    var parsedValue = ParseValue(value);
                    mergedJson[key] = JsonValue.Create(parsedValue);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing entry {key}: {ex.Message}");
            }
        }

        private object ParseValue(string value)
        {
            // Try parse boolean
            if (bool.TryParse(value, out bool boolResult))
                return boolResult;

            // Try parse integer
            if (int.TryParse(value, out int intResult))
                return intResult;

            // Try parse double
            if (double.TryParse(value, out double doubleResult))
                return doubleResult;

            // Return as string if no other type matches
            return value;
        }

        private void MergeJsonNodes(JsonObject target, string key, JsonNode sourceNode)
        {
            if (sourceNode is JsonObject sourceObject)
            {
                if (!target.ContainsKey(key))
                {
                    target[key] = new JsonObject();
                }

                var targetObject = target[key] as JsonObject;
                foreach (var property in sourceObject)
                {
                    if (targetObject.ContainsKey(property.Key) &&
                        targetObject[property.Key] is JsonObject &&
                        property.Value is JsonObject)
                    {
                        MergeJsonNodes(targetObject[property.Key] as JsonObject, property.Key, property.Value);
                    }
                    else
                    {
                        targetObject[property.Key] = property.Value.DeepClone();
                    }
                }
            }
            else
            {
                target[key] = sourceNode.DeepClone();
            }
        }

        private void SetNestedValue(JsonObject json, string[] keyParts, object value)
        {
            JsonObject current = json;

            for (int i = 0; i < keyParts.Length - 1; i++)
            {
                var part = keyParts[i];
                if (!current.ContainsKey(part))
                {
                    current[part] = new JsonObject();
                }
                current = current[part] as JsonObject;
            }

            var lastKey = keyParts[^1];
            current[lastKey] = JsonValue.Create(value);
        }

        private bool IsJsonObject(string value)
        {
            try
            {
                using var doc = JsonDocument.Parse(value);
                return doc.RootElement.ValueKind == JsonValueKind.Object;
            }
            catch
            {
                return false;
            }
        }

        private async Task WriteAppSettingsFileAsync(JsonNode configurationData)
        {
            var fileName = $"appsettings.{_environment}.json";
            var filePath = Path.Combine(AppContext.BaseDirectory, fileName);
            await File.WriteAllTextAsync(filePath, configurationData.ToJsonString(_jsonOptions));
            Console.WriteLine($"Successfully generated {fileName} at {filePath}");
        }
    }

    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var processor = new ConfigMapProcessor();
                await processor.ProcessConfigMapsAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Application failed: {ex.Message}");
                Environment.Exit(1);
            }
        }
    }
}
