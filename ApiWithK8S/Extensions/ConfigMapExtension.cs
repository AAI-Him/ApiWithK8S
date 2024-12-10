namespace ApiWithK8S.Extensions
{
    public static class ConfigMapExtension
    {
        public static IConfigurationBuilder AddConfigMap(
            this IConfigurationBuilder configurationBuilder,
            string[] configmaps)
        {
            if (!configmaps.Any())
            {
                throw new ArgumentException("Invalid argument: configmaps");
            }

            configurationBuilder.Add(new ConfigMapConfigurationSource(configmaps));

            return configurationBuilder;
        }
    }
}
