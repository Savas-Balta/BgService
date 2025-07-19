using BgService.WepApi.Data;
using BgService.WepApi.Models;

namespace BgService.WepApi.Services
{
    public class ForexRepository
    {
        private readonly ForexDbContext _context;
        private readonly ILogger<ForexRepository> _logger;

        public ForexRepository(ForexDbContext context, ILogger<ForexRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddForexAsync(Forex forex)
        {
            try
            {
                _context.Forexs.Add(forex);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Forex data added: {forex.BaseCurrency} to {forex.TargetCurrency} at rate {forex.Rate}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding forex data to the database");
                throw;
            }
        }

    }
}
