﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using webapi_pred.Data;

#nullable disable

namespace webapi_pred.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250408213418_InitialFullSetup")]
    partial class InitialFullSetup
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("webapi_pred.Models.Match", b =>
                {
                    b.Property<int>("MatchId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MatchId"));

                    b.Property<DateTime>("MatchDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Team1Id")
                        .HasColumnType("int");

                    b.Property<int>("Team1Score")
                        .HasColumnType("int");

                    b.Property<int>("Team2Id")
                        .HasColumnType("int");

                    b.Property<int>("Team2Score")
                        .HasColumnType("int");

                    b.Property<int?>("WinnerTeamId")
                        .HasColumnType("int");

                    b.HasKey("MatchId");

                    b.HasIndex("Team1Id");

                    b.HasIndex("Team2Id");

                    b.HasIndex("WinnerTeamId");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("webapi_pred.Models.Prediction", b =>
                {
                    b.Property<int>("PredictionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PredictionId"));

                    b.Property<int>("MatchId")
                        .HasColumnType("int");

                    b.Property<int>("PredictedTeam1Score")
                        .HasColumnType("int");

                    b.Property<int>("PredictedTeam2Score")
                        .HasColumnType("int");

                    b.Property<int>("PredictedWinnerId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("PredictionId");

                    b.HasIndex("MatchId");

                    b.HasIndex("PredictedWinnerId");

                    b.HasIndex("UserId");

                    b.ToTable("Predictions");
                });

            modelBuilder.Entity("webapi_pred.Models.Team", b =>
                {
                    b.Property<int>("TeamId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TeamId"));

                    b.Property<string>("Teamname")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("TeamId");

                    b.HasIndex("Teamname")
                        .IsUnique();

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("webapi_pred.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Points")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("webapi_pred.Models.Match", b =>
                {
                    b.HasOne("webapi_pred.Models.Team", "Team1")
                        .WithMany("MatchesAsTeam1")
                        .HasForeignKey("Team1Id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("webapi_pred.Models.Team", "Team2")
                        .WithMany("MatchesAsTeam2")
                        .HasForeignKey("Team2Id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("webapi_pred.Models.Team", "WinnerTeam")
                        .WithMany("MatchesAsWinner")
                        .HasForeignKey("WinnerTeamId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Team1");

                    b.Navigation("Team2");

                    b.Navigation("WinnerTeam");
                });

            modelBuilder.Entity("webapi_pred.Models.Prediction", b =>
                {
                    b.HasOne("webapi_pred.Models.Match", "Match")
                        .WithMany("Predictions")
                        .HasForeignKey("MatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("webapi_pred.Models.Team", "PredictedWinner")
                        .WithMany()
                        .HasForeignKey("PredictedWinnerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("webapi_pred.Models.User", "User")
                        .WithMany("Predictions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Match");

                    b.Navigation("PredictedWinner");

                    b.Navigation("User");
                });

            modelBuilder.Entity("webapi_pred.Models.Match", b =>
                {
                    b.Navigation("Predictions");
                });

            modelBuilder.Entity("webapi_pred.Models.Team", b =>
                {
                    b.Navigation("MatchesAsTeam1");

                    b.Navigation("MatchesAsTeam2");

                    b.Navigation("MatchesAsWinner");
                });

            modelBuilder.Entity("webapi_pred.Models.User", b =>
                {
                    b.Navigation("Predictions");
                });
#pragma warning restore 612, 618
        }
    }
}
