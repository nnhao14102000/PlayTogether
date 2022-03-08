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
        
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Charity> Charities { get; set; }
        public DbSet<CharityWithdraw> CharityWithdraws { get; set; }
        public DbSet<Deposit> Deposits { get; set; }
        public DbSet<Donate> Donates { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameOfPlayer> GameOfPlayers { get; set; }
        public DbSet<GameType> GameTypes { get; set; }
        public DbSet<Hirer> Hirers { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Music> Musics { get; set; }
        public DbSet<MusicOfPlayer> MusicOfPlayers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerWithdraw> PlayerWithdraws { get; set; }
        public DbSet<Rank> Ranks { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<TypeOfGame> TypeOfGames { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<FavoriteSearch> FavoriteSearches { get; set; }
        public DbSet<Hobby> Hobbies { get; set; }
        public DbSet<Recommend> Recommends { get; set; }
    }
}