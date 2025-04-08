using Microsoft.EntityFrameworkCore;
using webapi_pred.Models;

namespace webapi_pred.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        //public DbSet<Team> Teams { get; set; }
        //public DbSet<Match> Matches { get; set; }
        //public DbSet<Prediction> Predictions { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                // columns
                entity.HasKey(u => u.UserId);
                entity.HasIndex(u => u.Username).IsUnique();
                entity.Property(u => u.Username)
                      .IsRequired();
                entity.Property(u => u.Password)
                      .IsRequired();
                entity.Property(u => u.Points)
                      .IsRequired(false)
                      .HasDefaultValue(0);

                // relationships
                // entity.HasMany(u => u.Predictions)
                //       .WithOne(p => p.User)
                //       .HasForeignKey(p => p.UserId)
                //       .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasKey(t => t.TeamId);
                entity.HasIndex(t => t.Teamname).IsUnique();
            });

            modelBuilder.Entity<Match>(entity =>
            {
                entity.HasKey(m => m.MatchId);
                entity.Property(m => m.Team1Id).IsRequired()
                entity.Property(m => m.Team2Id).IsRequired()
                entity.Property(m => m.WinnerTeam)
                entity.Property(m => m.MatchDate).IsRequired();

                // team1 relation
                entity.HasOne(m => m.Team1)
                    .WithMany(t => t.MatchesAsTeam1)
                    .HasForeignKey(m => m.Team1Id)
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}