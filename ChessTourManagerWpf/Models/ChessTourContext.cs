using System;
using System.Collections.Generic;
using System.IO;
using ChessTourManagerWpf.Models.Entities;
using ChessTourManagerWpf.Models.TableViews;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ChessTourManagerWpf.Models;

public partial class ChessTourContext : DbContext
{
    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<Kind> Kinds { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<PlayersListView> PlayersListViews { get; set; }

    public virtual DbSet<Ratio> Ratios { get; set; }

    public virtual DbSet<SingleRatingListView> SingleRatingListViews { get; set; }

    public virtual DbSet<Entities.System> Systems { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<TeamRatingListView> TeamRatingListViews { get; set; }

    public virtual DbSet<TeamView> TeamViews { get; set; }

    public virtual DbSet<TeamsListView> TeamsListViews { get; set; }

    public virtual DbSet<Tournament> Tournaments { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Get project directory
        string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName
                               ?? throw new InvalidOperationException("Directory not found");

        IConfigurationBuilder builder = new ConfigurationBuilder()
                                       .SetBasePath(projectDirectory)
                                       .AddJsonFile("appsettings.json");

        IConfiguration configuration = builder.Build();
        string connectionString = configuration.GetConnectionString("DefaultConnection")
                               ?? throw new InvalidOperationException("Connection string not found");

        optionsBuilder.UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>(entity =>
                                  {
                                      entity.HasKey(e => new { e.WhiteId, e.BlackId, e.TournamentId, e.OrganizerId })
                                            .HasName("games_pk");

                                      entity.ToTable("games");

                                      entity
                                         .HasIndex(e => new { e.WhiteId, e.BlackId, e.TournamentId, e.OrganizerId, e.TourNumber },
                                                   "games_tour_number_uq").IsUnique();

                                      entity.Property(e => e.WhiteId).HasColumnName("white_id");
                                      entity.Property(e => e.BlackId).HasColumnName("black_id");
                                      entity.Property(e => e.TournamentId).HasColumnName("tournament_id");
                                      entity.Property(e => e.OrganizerId).HasColumnName("organizer_id");
                                      entity.Property(e => e.BlackPoints).HasColumnName("black_points");
                                      entity.Property(e => e.IsPlayed).HasColumnName("is_played");
                                      entity.Property(e => e.TourNumber).HasColumnName("tour_number");
                                      entity.Property(e => e.WhitePoints).HasColumnName("white_points");

                                      entity.HasOne(d => d.Player).WithMany(p => p.GamePlayers)
                                            .HasForeignKey(d => new { d.BlackId, d.TournamentId, d.OrganizerId })
                                            .OnDelete(DeleteBehavior.Restrict)
                                            .HasConstraintName("games_have_black_players_fk");

                                      entity.HasOne(d => d.PlayerNavigation).WithMany(p => p.GamePlayerNavigations)
                                            .HasForeignKey(d => new { d.WhiteId, d.TournamentId, d.OrganizerId })
                                            .OnDelete(DeleteBehavior.Restrict)
                                            .HasConstraintName("games_have_white_players_fk");
                                  });

        modelBuilder.Entity<Group>(entity =>
                                   {
                                       entity.HasKey(e => new { e.GroupId, e.TournamentId, e.OrganizerId })
                                             .HasName("groups_pk");

                                       entity.ToTable("groups");

                                       entity.HasIndex(e => new { e.TournamentId, e.OrganizerId, e.GroupName },
                                                       "group_name_uq").IsUnique();

                                       entity.HasIndex(e => new { e.TournamentId, e.OrganizerId, e.Identity },
                                                       "identity_uq").IsUnique();

                                       entity.Property(e => e.GroupId)
                                             .ValueGeneratedOnAdd()
                                             .HasColumnName("group_id");
                                       entity.Property(e => e.TournamentId).HasColumnName("tournament_id");
                                       entity.Property(e => e.OrganizerId).HasColumnName("organizer_id");
                                       entity.Property(e => e.GroupName)
                                             .HasMaxLength(255)
                                             .HasDefaultValueSql("'1'::character varying")
                                             .HasColumnName("group_name");
                                       entity.Property(e => e.Identity)
                                             .HasMaxLength(4)
                                             .HasDefaultValueSql("'1'::bpchar")
                                             .IsFixedLength()
                                             .HasColumnName("identity");

                                       entity.HasOne(d => d.Tournament).WithMany(p => p.Groups)
                                             .HasForeignKey(d => new { d.TournamentId, d.OrganizerId })
                                             .HasConstraintName("groups_in_tournaments_fk");
                                   });

        modelBuilder.Entity<Kind>(entity =>
                                  {
                                      entity.HasKey(e => e.KindId).HasName("kinds_pk");

                                      entity.ToTable("kinds");

                                      entity.HasIndex(e => e.KindName, "kinds_name_uq").IsUnique();

                                      entity.Property(e => e.KindId).HasColumnName("kind_id");
                                      entity.Property(e => e.KindName)
                                            .HasMaxLength(255)
                                            .HasColumnName("kind_name");
                                  });

        modelBuilder.Entity<Player>(entity =>
                                    {
                                        entity.HasKey(e => new { e.PlayerId, e.TournamentId, e.OrganizerId })
                                              .HasName("players_pk");

                                        entity.ToTable("players");

                                        entity
                                           .HasIndex(e => new { e.PlayerId, e.TournamentId, e.PlayerLastName, e.PlayerFirstName, e.PlayerAttribute },
                                                     "players_name_attr_uq").IsUnique();

                                        entity.Property(e => e.PlayerId)
                                              .ValueGeneratedOnAdd()
                                              .HasColumnName("player_id");
                                        entity.Property(e => e.TournamentId).HasColumnName("tournament_id");
                                        entity.Property(e => e.OrganizerId).HasColumnName("organizer_id");
                                        entity.Property(e => e.BoardNumber)
                                              .HasDefaultValueSql("1")
                                              .HasColumnName("board_number");
                                        entity.Property(e => e.DrawsCount).HasColumnName("draws_count");
                                        entity.Property(e => e.Gender)
                                              .HasMaxLength(1)
                                              .HasColumnName("gender");
                                        entity.Property(e => e.GroupId).HasColumnName("group_id");
                                        entity.Property(e => e.IsActive)
                                              .IsRequired()
                                              .HasDefaultValueSql("true")
                                              .HasColumnName("is_active");
                                        entity.Property(e => e.LossesCount).HasColumnName("losses_count");
                                        entity.Property(e => e.PlayerAttribute)
                                              .HasMaxLength(3)
                                              .HasDefaultValueSql("'   '::bpchar")
                                              .IsFixedLength()
                                              .HasColumnName("player_attribute");
                                        entity.Property(e => e.PlayerBirthYear)
                                              .HasDefaultValueSql("EXTRACT(year FROM now())")
                                              .HasColumnName("player_birth_year");
                                        entity.Property(e => e.PlayerFirstName)
                                              .HasMaxLength(255)
                                              .HasColumnName("player_first_name");
                                        entity.Property(e => e.PlayerLastName)
                                              .HasMaxLength(255)
                                              .HasColumnName("player_last_name");
                                        entity.Property(e => e.PointsCount).HasColumnName("points_count");
                                        entity.Property(e => e.RatioSum1)
                                              .HasPrecision(5, 2)
                                              .HasColumnName("ratio_sum1");
                                        entity.Property(e => e.RatioSum2)
                                              .HasPrecision(5, 2)
                                              .HasColumnName("ratio_sum2");
                                        entity.Property(e => e.TeamId).HasColumnName("team_id");
                                        entity.Property(e => e.WinsCount).HasColumnName("wins_count");

                                        entity.HasOne(d => d.Tournament).WithMany(p => p.Players)
                                              .HasForeignKey(d => new { d.TournamentId, d.OrganizerId })
                                              .HasConstraintName("players_take_part_in_tournaments_fk");

                                        entity.HasOne(d => d.Group).WithMany(p => p.Players)
                                              .HasForeignKey(d => new { d.GroupId, d.TournamentId, d.OrganizerId })
                                              .OnDelete(DeleteBehavior.Restrict)
                                              .HasConstraintName("players_in_groups_fk");

                                        entity.HasOne(d => d.Team).WithMany(p => p.Players)
                                              .HasForeignKey(d => new { d.TeamId, d.OrganizerId, d.TournamentId })
                                              .OnDelete(DeleteBehavior.Restrict)
                                              .HasConstraintName("players_in_teams_fk");
                                    });

        modelBuilder.Entity<PlayersListView>(entity =>
                                             {
                                                 entity
                                                    .HasNoKey()
                                                    .ToView("players_list_view");

                                                 entity.Property(e => e.Gender)
                                                       .HasMaxLength(1)
                                                       .HasColumnName("gender");
                                                 entity.Property(e => e.GroupIdent)
                                                       .HasMaxLength(4)
                                                       .IsFixedLength()
                                                       .HasColumnName("group_ident");
                                                 entity.Property(e => e.IsActive).HasColumnName("is_active");
                                                 entity.Property(e => e.OrganizerId).HasColumnName("organizer_id");
                                                 entity.Property(e => e.PlayerAttribute)
                                                       .HasMaxLength(3)
                                                       .IsFixedLength()
                                                       .HasColumnName("player_attribute");
                                                 entity.Property(e => e.PlayerBirthYear)
                                                       .HasColumnName("player_birth_year");
                                                 entity.Property(e => e.PlayerFirstName)
                                                       .HasMaxLength(255)
                                                       .HasColumnName("player_first_name");
                                                 entity.Property(e => e.PlayerId).HasColumnName("player_id");
                                                 entity.Property(e => e.PlayerLastName)
                                                       .HasMaxLength(255)
                                                       .HasColumnName("player_last_name");
                                                 entity.Property(e => e.TeamName)
                                                       .HasMaxLength(255)
                                                       .HasColumnName("team_name");
                                                 entity.Property(e => e.TournamentId).HasColumnName("tournament_id");
                                             });

        modelBuilder.Entity<Ratio>(entity =>
                                   {
                                       entity.HasKey(e => e.RatioId).HasName("ratios_pk");

                                       entity.ToTable("ratios");

                                       entity.HasIndex(e => e.RatioName, "ratios_name_uq").IsUnique();

                                       entity.Property(e => e.RatioId).HasColumnName("ratio_id");
                                       entity.Property(e => e.RatioName)
                                             .HasMaxLength(255)
                                             .HasColumnName("ratio_name");
                                   });

        modelBuilder.Entity<SingleRatingListView>(entity =>
                                                  {
                                                      entity
                                                         .HasNoKey()
                                                         .ToView("single_rating_list_view");

                                                      entity.Property(e => e.DrawsCount)
                                                            .HasColumnName("draws_count");
                                                      entity.Property(e => e.GroupIdent)
                                                            .HasMaxLength(4)
                                                            .IsFixedLength()
                                                            .HasColumnName("group_ident");
                                                      entity.Property(e => e.LossesCount)
                                                            .HasColumnName("losses_count");
                                                      entity.Property(e => e.OrganizerId)
                                                            .HasColumnName("organizer_id");
                                                      entity.Property(e => e.PlayerAttribute)
                                                            .HasMaxLength(3)
                                                            .IsFixedLength()
                                                            .HasColumnName("player_attribute");
                                                      entity.Property(e => e.PlayerFirstName)
                                                            .HasMaxLength(255)
                                                            .HasColumnName("player_first_name");
                                                      entity.Property(e => e.PlayerId).HasColumnName("player_id");
                                                      entity.Property(e => e.PlayerLastName)
                                                            .HasMaxLength(255)
                                                            .HasColumnName("player_last_name");
                                                      entity.Property(e => e.PlayerRank)
                                                            .HasColumnName("player_rank");
                                                      entity.Property(e => e.PointsCount)
                                                            .HasColumnName("points_count");
                                                      entity.Property(e => e.RatioSum1)
                                                            .HasPrecision(5, 2)
                                                            .HasColumnName("ratio_sum1");
                                                      entity.Property(e => e.RatioSum2)
                                                            .HasPrecision(5, 2)
                                                            .HasColumnName("ratio_sum2");
                                                      entity.Property(e => e.TournamentId)
                                                            .HasColumnName("tournament_id");
                                                      entity.Property(e => e.WinsCount).HasColumnName("wins_count");
                                                  });

        modelBuilder.Entity<Entities.System>(entity =>
                                             {
                                                   entity.HasKey(e => e.SystemId).HasName("systems_pk");

                                                   entity.ToTable("systems");

                                                   entity.HasIndex(e => e.SystemName, "systems_name_uq").IsUnique();

                                                   entity.Property(e => e.SystemId).HasColumnName("system_id");
                                                   entity.Property(e => e.SystemName)
                                                         .HasMaxLength(255)
                                                         .HasColumnName("system_name");
                                             });

        modelBuilder.Entity<Team>(entity =>
                                  {
                                      entity.HasKey(e => new { e.TeamId, e.OrganizerId, e.TournamentId })
                                            .HasName("teams_pk");

                                      entity.ToTable("teams");

                                      entity.HasIndex(e => new { e.TeamId, e.TournamentId, e.TeamName },
                                                      "teams_name_uq").IsUnique();

                                      entity.Property(e => e.TeamId)
                                            .ValueGeneratedOnAdd()
                                            .HasColumnName("team_id");
                                      entity.Property(e => e.OrganizerId).HasColumnName("organizer_id");
                                      entity.Property(e => e.TournamentId).HasColumnName("tournament_id");
                                      entity.Property(e => e.IsActive)
                                            .IsRequired()
                                            .HasDefaultValueSql("true")
                                            .HasColumnName("is_active");
                                      entity.Property(e => e.TeamAttribute)
                                            .HasMaxLength(3)
                                            .HasDefaultValueSql("'   '::bpchar")
                                            .IsFixedLength()
                                            .HasColumnName("team_attribute");
                                      entity.Property(e => e.TeamName)
                                            .HasMaxLength(255)
                                            .HasColumnName("team_name");

                                      entity.HasOne(d => d.Tournament).WithMany(p => p.Teams)
                                            .HasForeignKey(d => new { d.TournamentId, d.OrganizerId })
                                            .HasConstraintName("teams_in_tournaments_fk");
                                  });

        modelBuilder.Entity<TeamRatingListView>(entity =>
                                                {
                                                    entity
                                                       .HasNoKey()
                                                       .ToView("team_rating_list_view");

                                                    entity.Property(e => e.DrawsCount).HasColumnName("draws_count");
                                                    entity.Property(e => e.LossesCount)
                                                          .HasColumnName("losses_count");
                                                    entity.Property(e => e.OrganizerId)
                                                          .HasColumnName("organizer_id");
                                                    entity.Property(e => e.PointsCount)
                                                          .HasColumnName("points_count");
                                                    entity.Property(e => e.RatioSum1).HasColumnName("ratio_sum1");
                                                    entity.Property(e => e.RatioSum2).HasColumnName("ratio_sum2");
                                                    entity.Property(e => e.TeamAttribute)
                                                          .HasMaxLength(3)
                                                          .IsFixedLength()
                                                          .HasColumnName("team_attribute");
                                                    entity.Property(e => e.TeamId).HasColumnName("team_id");
                                                    entity.Property(e => e.TeamName)
                                                          .HasMaxLength(255)
                                                          .HasColumnName("team_name");
                                                    entity.Property(e => e.TeamRank).HasColumnName("team_rank");
                                                    entity.Property(e => e.TournamentId)
                                                          .HasColumnName("tournament_id");
                                                    entity.Property(e => e.WinsCount).HasColumnName("wins_count");
                                                });

        modelBuilder.Entity<TeamView>(entity =>
                                      {
                                          entity
                                             .HasNoKey()
                                             .ToView("team_view");

                                          entity.Property(e => e.BoardNumber).HasColumnName("board_number");
                                          entity.Property(e => e.GroupIdent)
                                                .HasMaxLength(4)
                                                .IsFixedLength()
                                                .HasColumnName("group_ident");
                                          entity.Property(e => e.IsActive).HasColumnName("is_active");
                                          entity.Property(e => e.OrganizerId).HasColumnName("organizer_id");
                                          entity.Property(e => e.PlayerFirstName)
                                                .HasMaxLength(255)
                                                .HasColumnName("player_first_name");
                                          entity.Property(e => e.PlayerId).HasColumnName("player_id");
                                          entity.Property(e => e.PlayerLastName)
                                                .HasMaxLength(255)
                                                .HasColumnName("player_last_name");
                                          entity.Property(e => e.TeamId).HasColumnName("team_id");
                                          entity.Property(e => e.TournamentId).HasColumnName("tournament_id");
                                      });

        modelBuilder.Entity<TeamsListView>(entity =>
                                           {
                                               entity
                                                  .HasNoKey()
                                                  .ToView("teams_list_view");

                                               entity.Property(e => e.IsActive).HasColumnName("is_active");
                                               entity.Property(e => e.OrganizerId).HasColumnName("organizer_id");
                                               entity.Property(e => e.PlayersCount).HasColumnName("players_count");
                                               entity.Property(e => e.TeamAttribute)
                                                     .HasMaxLength(3)
                                                     .IsFixedLength()
                                                     .HasColumnName("team_attribute");
                                               entity.Property(e => e.TeamId).HasColumnName("team_id");
                                               entity.Property(e => e.TeamName)
                                                     .HasMaxLength(255)
                                                     .HasColumnName("team_name");
                                               entity.Property(e => e.TournamentId).HasColumnName("tournament_id");
                                           });

        modelBuilder.Entity<Tournament>(entity =>
                                        {
                                            entity.HasKey(e => new { e.TournamentId, e.OrganizerId })
                                                  .HasName("tournaments_pk");

                                            entity.ToTable("tournaments");

                                            entity
                                               .HasIndex(e => new { e.TournamentId, e.OrganizerId, e.TournamentName },
                                                         "tournaments_name_uq").IsUnique();

                                            entity.Property(e => e.TournamentId)
                                                  .ValueGeneratedOnAdd()
                                                  .HasColumnName("tournament_id");
                                            entity.Property(e => e.OrganizerId).HasColumnName("organizer_id");
                                            entity.Property(e => e.DateCreate)
                                                  .HasDefaultValueSql("(now())::date")
                                                  .HasColumnName("date_create");
                                            entity.Property(e => e.DateLastChange)
                                                  .HasDefaultValueSql("(now())::date")
                                                  .HasColumnName("date_last_change");
                                            entity.Property(e => e.DateStart)
                                                  .HasDefaultValueSql("(now())::date")
                                                  .HasColumnName("date_start");
                                            entity.Property(e => e.Duration).HasColumnName("duration");
                                            entity.Property(e => e.IsMixedGroups)
                                                  .IsRequired()
                                                  .HasDefaultValueSql("true")
                                                  .HasColumnName("is_mixed_groups");
                                            entity.Property(e => e.KindId).HasColumnName("kind_id");
                                            entity.Property(e => e.MaxTeamPlayers)
                                                  .HasDefaultValueSql("5")
                                                  .HasColumnName("max_team_players");
                                            entity.Property(e => e.OrganizationName)
                                                  .HasMaxLength(255)
                                                  .HasDefaultValueSql("'-'::character varying")
                                                  .HasColumnName("organization_name");
                                            entity.Property(e => e.Place)
                                                  .HasMaxLength(255)
                                                  .HasDefaultValueSql("'-'::character varying")
                                                  .HasColumnName("place");
                                            entity.Property(e => e.SystemId).HasColumnName("system_id");
                                            entity.Property(e => e.TimeCreate)
                                                  .HasPrecision(6)
                                                  .HasDefaultValueSql("(now())::time(6) without time zone")
                                                  .HasColumnName("time_create");
                                            entity.Property(e => e.TimeLastChange)
                                                  .HasPrecision(6)
                                                  .HasDefaultValueSql("(now())::time(6) without time zone")
                                                  .HasColumnName("time_last_change");
                                            entity.Property(e => e.TimeStart)
                                                  .HasPrecision(6)
                                                  .HasDefaultValueSql("(now())::time(6) without time zone")
                                                  .HasColumnName("time_start");
                                            entity.Property(e => e.TournamentName)
                                                  .HasMaxLength(255)
                                                  .HasColumnName("tournament_name");
                                            entity.Property(e => e.ToursCount)
                                                  .HasDefaultValueSql("7")
                                                  .HasColumnName("tours_count");

                                            entity.HasOne(d => d.Kind).WithMany(p => p.Tournaments)
                                                  .HasForeignKey(d => d.KindId)
                                                  .OnDelete(DeleteBehavior.Restrict)
                                                  .HasConstraintName("tournaments_have_kinds_fk");

                                            entity.HasOne(d => d.Organizer).WithMany(p => p.Tournaments)
                                                  .HasForeignKey(d => d.OrganizerId)
                                                  .HasConstraintName("tournaments_have_organizers_fk");

                                            entity.HasOne(d => d.System).WithMany(p => p.Tournaments)
                                                  .HasForeignKey(d => d.SystemId)
                                                  .OnDelete(DeleteBehavior.Restrict)
                                                  .HasConstraintName("tournaments_have_systems_fk");

                                            entity.HasMany(d => d.Ratios).WithMany(p => p.Tournaments)
                                                  .UsingEntity<Dictionary<string, object>>(
                                                    "TournamentRatio",
                                                    r => r.HasOne<Ratio>().WithMany()
                                                          .HasForeignKey("RatioId")
                                                          .OnDelete(DeleteBehavior.Restrict)
                                                          .HasConstraintName("tournaments_ratios_in_ratios_fk"),
                                                    l => l.HasOne<Tournament>().WithMany()
                                                          .HasForeignKey("TournamentId", "OrganizerId")
                                                          .HasConstraintName("tournaments_ratios_in_tournaments_fk"),
                                                    j =>
                                                    {
                                                        j.HasKey("TournamentId", "OrganizerId", "RatioId")
                                                         .HasName("tournament_ratios_pk");
                                                        j.ToTable("tournament_ratios");
                                                    });
                                        });

        modelBuilder.Entity<User>(entity =>
                                  {
                                      entity.HasKey(e => e.UserId).HasName("users_pk");

                                      entity.ToTable("users");

                                      entity.HasIndex(e => e.Email, "users_email_uq").IsUnique();

                                      entity.Property(e => e.UserId).HasColumnName("user_id");
                                      entity.Property(e => e.Email)
                                            .HasMaxLength(255)
                                            .HasColumnName("email");
                                      entity.Property(e => e.PassHash)
                                            .HasMaxLength(255)
                                            .HasColumnName("pass_hash");
                                      entity.Property(e => e.RegisterDate)
                                            .HasDefaultValueSql("(now())::date")
                                            .HasColumnName("register_date");
                                      entity.Property(e => e.RegisterTime)
                                            .HasPrecision(6)
                                            .HasDefaultValueSql("(now())::time(6) without time zone")
                                            .HasColumnName("register_time");
                                      entity.Property(e => e.TournamentsLim)
                                            .HasDefaultValueSql("50")
                                            .HasColumnName("tournaments_lim");
                                      entity.Property(e => e.UserFirstname)
                                            .HasMaxLength(255)
                                            .HasColumnName("user_firstname");
                                      entity.Property(e => e.UserLastname)
                                            .HasMaxLength(255)
                                            .HasColumnName("user_lastname");
                                      entity.Property(e => e.UserPatronymic)
                                            .HasMaxLength(255)
                                            .HasDefaultValueSql("'-'::character varying")
                                            .HasColumnName("user_patronymic");
                                  });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
