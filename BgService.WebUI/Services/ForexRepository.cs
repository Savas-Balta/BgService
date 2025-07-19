using BgService.WebUI.Data;
using BgService.WebUI.Models;
using Microsoft.EntityFrameworkCore;

namespace BgService.WebUI.Services
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

        public async Task<List<Forex>> GetAllForexAsync()
        {
            try
            {
                return await _context.Forexs.OrderByDescending(f => f.Timestamp).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching forex rates");
                return new List<Forex>();
            }
        }


    }
}
