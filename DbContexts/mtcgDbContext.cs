using Microsoft.EntityFrameworkCore;

public class mtcgDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public mtcgDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Card> Cards { get; set; }
    public DbSet<Package> Packages { get; set; }
    public DbSet<TradingDeal> TradingDeals { get; set; }
    public DbSet<BattleResult> BattleResults { get; set; }
    public DbSet<Round> Rounds { get; set; }
    public DbSet<Battle> Battles { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<UserStats> UserStats { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Only configure the database connection here
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseNpgsql(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Battle>()
            .HasOne(b => b.Result)
            .WithOne()
            .HasForeignKey<BattleResult>(br => br.BattleResultId); 

        modelBuilder.Entity<UserStats>()
            .HasKey(us => us.UserId); // Set UserId as the primary key

        modelBuilder.Entity<UserStats>()
            .HasOne(us => us.User)
            .WithOne(u => u.UserStats)
            .HasForeignKey<UserStats>(us => us.UserId);
    }
}
