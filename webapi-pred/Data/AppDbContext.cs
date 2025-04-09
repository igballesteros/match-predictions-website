using Microsoft.EntityFrameworkCore;
using webapi_pred.Models;

namespace webapi_pred.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Prediction> Predictions { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // setup for users table
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
                entity.HasMany(u => u.Predictions)
                      .WithOne(p => p.User)
                      .HasForeignKey(p => p.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // setup for teams table
            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasKey(t => t.TeamId);
                entity.HasIndex(t => t.Teamname).IsUnique();
            });

            // setup for matches table
            modelBuilder.Entity<Match>(entity =>
            {
                // columns
                entity.HasKey(m => m.MatchId);
                entity.HasOne(m => m.Team1)
                    .WithMany()
                    .HasForeignKey(m => m.Team1Id)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(m => m.Team2)
                    .WithMany()
                    .HasForeignKey(m => m.Team2Id)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(m => m.WinnerTeam)
                    .WithMany()
                    .HasForeignKey(m => m.WinnerTeamId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.Property(m => m.Team1Score)
                    .IsRequired();
                entity.Property(m => m.Team2Score)
                    .IsRequired();
                entity.Property(m => m.MatchDate)
                    .IsRequired();
            });

            // setup for predictions table
            modelBuilder.Entity<Prediction>(entity =>
            {
                entity.HasKey(p => p.PredictionId);
                entity.Property(p => p.MatchId).IsRequired();
                entity.Property(p => p.PredictedWinnerId).IsRequired();
                entity.Property(p => p.PredictedTeam1Score).IsRequired();
                entity.Property(p => p.PredictedTeam2Score).IsRequired();

                entity.HasOne(p => p.User)
                      .WithMany(u => u.Predictions)
                      .HasForeignKey(p => p.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(p => p.Match)
                      .WithMany(m => m.Predictions)
                      .HasForeignKey(p => p.MatchId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(p => p.PredictedWinner)
                      .WithMany()
                      .HasForeignKey(p => p.PredictedWinnerId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // setup for teams table relations
            modelBuilder.Entity<Team>()
                .HasMany<Match>(t => t.MatchesAsTeam1)
                .WithOne(m => m.Team1)
                .HasForeignKey(m => m.Team1Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Team>()
                .HasMany<Match>(t => t.MatchesAsTeam2)
                .WithOne(m => m.Team2)
                .HasForeignKey(m => m.Team2Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Team>()
                .HasMany<Match>(t => t.MatchesAsWinner)
                .WithOne(m => m.WinnerTeam)
                .HasForeignKey(m => m.WinnerTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}