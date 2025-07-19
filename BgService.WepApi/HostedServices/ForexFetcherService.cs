
using BgService.WepApi.Data;
using BgService.WepApi.Models;
using BgService.WepApi.Services;

namespace BgService.WepApi.HostedServices
{
    public class ForexFetcherService : IHostedService, IDisposable
    {
        private readonly ILogger<ForexFetcherService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private Timer _timer;

        public ForexFetcherService(IServiceScopeFactory serviceScopeFactory, ILogger<ForexFetcherService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Forex Fetcher Service is starting.");
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(30));
            return Task.CompletedTask;
        }

        private async void DoWork(object? state)
        {
            _logger.LogInformation("Forex Fetcher Service is working.");

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var forexApiService = scope.ServiceProvider.GetRequiredService<ForexApiService>();
                var forrexRepository = scope.ServiceProvider.GetRequiredService<ForexRepository>();
                var dbContext = scope.ServiceProvider.GetRequiredService<ForexDbContext>();
                
                try
                {
                    await dbContext.Database.EnsureCreatedAsync();

                    decimal? usdToTry = await forexApiService.GetForrex("USD", "TRY");

                    if (usdToTry.HasValue)
                    {
                        var newForrex = new Forex
                        {
                            BaseCurrency = "USD",
                            TargetCurrency = "TRY",
                            Rate = usdToTry.Value,
                            Timestamp = DateTime.UtcNow
                        };

                        await forrexRepository.AddForexAsync(newForrex);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while fetching or saving forex data.");
                }

            }

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Forex Fetcher Service is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _timer?.Dispose();
            _logger.LogInformation("Forex Fetcher Service has been disposed.");
        }
    }
}
