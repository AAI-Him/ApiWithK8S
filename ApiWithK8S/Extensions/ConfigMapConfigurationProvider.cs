namespace ApiWithK8S.Extensions
{
    public class ConfigMapConfigurationProvider : ConfigurationProvider
    {
        private readonly string[] _configMaps;
        public ConfigMapConfigurationProvider(string[] configMaps)
        {
            _configMaps = configMaps;
        }

        public override void Load()
        {
            // implement the Load() function here
        }
    }
}
