using ConsoleTest;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace ApiWithK8S.Extensions
{
    public interface ISAPClientFactory
    {
        string AppHost => "default";
    }

    internal sealed class SAPClientFactory : ISAPClientFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IOptions<SAPClientOptions> _options;

        public SAPClientFactory(IServiceProvider serviceProvider, IServiceScopeFactory scopeFactory, IOptions<SAPClientOptions> options)
        {
            _serviceProvider = serviceProvider;
            _scopeFactory = scopeFactory;
            _options = options;
        }

        public string AppHost => _options != null ? _options!.Value!.AppHost! : String.Empty;
    }
}
