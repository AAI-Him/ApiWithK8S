using SapCo2.Abstract;

namespace AAISAPClient
{
    public class Worker(ILogger<Worker> logger, IServiceProvider serviceProvider) 
        : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (logger.IsEnabled(LogLevel.Information))
                {
                    logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }

                using (IRfcClient client = serviceProvider.GetRequiredService<IRfcClient>())
                { 
                }
                //BomOutputParameter result = null;

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
