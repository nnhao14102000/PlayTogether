using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Infrastructure.Entities;

namespace PlayTogether.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
 
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Hirer> Hirers { get; set; }
        public DbSet<Charity> Charities { get; set; }
        public DbSet<Admin> Admins { get; set; }
    }
}