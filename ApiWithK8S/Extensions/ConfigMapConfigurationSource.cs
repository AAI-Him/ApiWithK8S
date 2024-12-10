namespace ApiWithK8S.Extensions
{
    public class ConfigMapConfigurationSource : IConfigurationSource
    {
        private readonly string[] _configMaps;
        public ConfigMapConfigurationSource(string[] configMaps)
        {
            _configMaps = configMaps;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
            => new ConfigMapConfigurationProvider(_configMaps);
    }
}
