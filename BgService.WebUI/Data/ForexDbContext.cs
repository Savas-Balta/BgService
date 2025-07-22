using BgService.WebUI.Models;
using Microsoft.EntityFrameworkCore;

namespace BgService.WebUI.Data
{
    public class ForexDbContext : DbContext
    {
        public ForexDbContext(DbContextOptions<ForexDbContext> options)
            : base(options)
        {
        }
        public DbSet<Forex> Forexs { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
