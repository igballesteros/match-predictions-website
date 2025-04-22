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
            ConfigureUserEntity(modelBuilder);
            ConfigureTeamEntity(modelBuilder);
            ConfigureMatchEntity(modelBuilder);
            ConfigurePredictionEntity(modelBuilder);
            ConfigureRelationships(modelBuilder);

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    Username = "admin",
                    Password = "1a2b3c4d5e",
                    Role = "Admin"
                },
                new User
                {
                    UserId = 2,
                    Username = "regularuser",
                    Password = "userpassword",
                    Role = "User" // Add this role
                }
            );

            modelBuilder.Entity<Team>().HasData(
                new Team
                {
                    TeamId = 1,
                    Teamname = "Atlanta Faze"
                },
                new Team
                {
                    TeamId = 2,
                    Teamname = "Boston Breach"
                },
                new Team
                {
                    TeamId = 3,
                    Teamname = "Carolina Ravens"
                },
                new Team
                {
                    TeamId = 4,
                    Teamname = "Cloud9 New York"
                },
                new Team
                {
                    TeamId = 5,
                    Teamname = "Los Angeles Guerrilas M8"
                },
                new Team
                {
                    TeamId = 6,
                    Teamname = "Los Angeles Thieves"
                },
                new Team
                {
                    TeamId = 7,
                    Teamname = "Miami Heretics"
                },
                new Team
                {
                    TeamId = 8,
                    Teamname = "Minnesota Røkkr"
                },
                new Team
                {
                    TeamId = 9,
                    Teamname = "Optic Texas"
                },
                new Team
                {
                    TeamId = 10,
                    Teamname = "Toronto Ultra"
                },
                new Team
                {
                    TeamId = 11,
                    Teamname = "Vancouver Surge"
                },
                new Team
                {
                    TeamId = 12,
                    Teamname = "Vegas Falcons"
                }

            );

            modelBuilder.Entity<Match>().HasData(MatchSeeder.GetMatchSeeds());

            base.OnModelCreating(modelBuilder);
        }

        private void ConfigureUserEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                // Primary Key
                entity.HasKey(u => u.UserId);

                // Properties
                entity.HasIndex(u => u.Username).IsUnique();
                entity.Property(u => u.Username).IsRequired();
                entity.Property(u => u.Password).IsRequired();
                entity.Property(u => u.Points)
                    .IsRequired()
                    .HasDefaultValue(0);
                entity.Property(u => u.Role)
                    .IsRequired()
                    .HasDefaultValue("User");

                // Relationships
                entity.HasMany(u => u.Predictions)
                    .WithOne(p => p.User)
                    .HasForeignKey(p => p.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void ConfigureTeamEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Team>(entity =>
            {
                // Primary Key
                entity.HasKey(t => t.TeamId);

                // Properties
                entity.HasIndex(t => t.Teamname).IsUnique();
            });
        }

        private void ConfigureMatchEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Match>(entity =>
            {
                // Primary Key
                entity.HasKey(m => m.MatchId);

                // Properties
                entity.Property(m => m.Team1Score).IsRequired();
                entity.Property(m => m.Team2Score).IsRequired();
                entity.Property(m => m.MatchDate).IsRequired();

                // Indexes
                entity.HasIndex(m => m.MatchDate);
                entity.HasIndex(m => m.WinnerTeamId);
            });
        }

        private void ConfigurePredictionEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Prediction>(entity =>
            {
                // Primary Key
                entity.HasKey(p => p.PredictionId);

                // Properties
                entity.Property(p => p.MatchId).IsRequired();
                entity.Property(p => p.PredictedWinnerId).IsRequired();
                entity.Property(p => p.PredictedTeam1Score).IsRequired();
                entity.Property(p => p.PredictedTeam2Score).IsRequired();
            });
        }

        private void ConfigureRelationships(ModelBuilder modelBuilder)
        {
            // Match Relationships
            modelBuilder.Entity<Match>()
                .HasOne(m => m.Team1)
                .WithMany(t => t.MatchesAsTeam1)
                .HasForeignKey(m => m.Team1Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Team2)
                .WithMany(t => t.MatchesAsTeam2)
                .HasForeignKey(m => m.Team2Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.WinnerTeam)
                .WithMany(t => t.MatchesAsWinner)
                .HasForeignKey(m => m.WinnerTeamId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Match>()
                .HasMany(m => m.Predictions)
                .WithOne(p => p.Match)
                .OnDelete(DeleteBehavior.Cascade);

            // Prediction Relationships
            modelBuilder.Entity<Prediction>()
                .HasOne(p => p.User)
                .WithMany(u => u.Predictions)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Prediction>()
                .HasOne(p => p.PredictedWinner)
                .WithMany()
                .HasForeignKey(p => p.PredictedWinnerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}