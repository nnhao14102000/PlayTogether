using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Entities;

namespace PlayTogether.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Charity> Charities { get; set; }
        public DbSet<CharityWithdraw> CharityWithdraws { get; set; }
        public DbSet<Donate> Donates { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameOfUser> GameOfUsers { get; set; }
        public DbSet<GameOfOrder> GameOfOrders { get; set; }
        public DbSet<GameType> GameTypes { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<UserBalance> UserBalances { get; set; }
        public DbSet<UnActiveBalance> UnActiveBalances { get; set; }
        public DbSet<Rank> Ranks { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<TypeOfGame> TypeOfGames { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<SearchHistory> SearchHistories { get; set; }
        public DbSet<Hobby> Hobbies { get; set; }
        public DbSet<Recommend> Recommends { get; set; }
        public DbSet<TransactionHistory> TransactionHistories { get; set; }
        public DbSet<DisableUser> DisableUsers { get; set; }
        public DbSet<BehaviorPoint> BehaviorPoints { get; set; }
        public DbSet<BehaviorHistory> BehaviorHistories { get; set; }
        public DbSet<Dating> Datings { get; set; }
        public DbSet<Ignore> Ignores { get; set; }
        public DbSet<SystemFeedback> SystemFeedbacks { get; set; }
        public DbSet<SystemConfig> SystemConfigs { get; set; }
    }
}