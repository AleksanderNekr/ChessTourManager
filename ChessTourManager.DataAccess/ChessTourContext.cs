using System.Collections.Generic;
using System.IO;
using ChessTourManager.DataAccess.Entities;
using ChessTourManager.DataAccess.TableViews;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace ChessTourManager.DataAccess;

public class ChessTourContext : DbContext
{
    public DbSet<Game> Games { get; set; }

    public DbSet<Group> Groups { get; set; }

    public DbSet<Kind> Kinds { get; set; }

    public DbSet<Player> Players { get; set; }

    public DbSet<PlayersListView> PlayersListViews { get; set; }

    public DbSet<Ratio> Ratios { get; set; }

    public DbSet<SingleRatingListView> SingleRatingListViews { get; set; }

    public DbSet<Entities.System> Systems { get; set; }

    public DbSet<Team> Teams { get; set; }

    public DbSet<TeamRatingListView> TeamRatingListViews { get; set; }

    public DbSet<TeamView> TeamViews { get; set; }

    public DbSet<TeamsListView> TeamsListViews { get; set; }

    public DbSet<Tournament?> Tournaments { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<IdentityUserClaim<int>> IdentityUserClaims { get; set; }

    public DbSet<IdentityUserLogin<int>> IdentityUserLogins { get; set; }

    public DbSet<IdentityUserToken<int>> IdentityUserTokens { get; set; }

    public DbSet<IdentityRole<int>> IdentityRoles { get; set; }

    public DbSet<IdentityRoleClaim<int>> IdentityRoleClaims { get; set; }

    public DbSet<IdentityUserRole<int>> IdentityUserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Get connection string from appsettings.json
        IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(Directory
                                                                          .GetParent(Directory
                                                                              .GetCurrentDirectory())
                                                                     + "/ChessTourManager.WEB")
                                                                  .AddJsonFile("appsettings.Development.json", false, true)
                                                                  .AddJsonFile("appsettings.json", false, true);

        IConfiguration configuration    = builder.Build();
        string         connectionString = configuration.GetConnectionString("DefaultConnection");

        optionsBuilder.UseNpgsql(connectionString);

        optionsBuilder.ConfigureWarnings(warnings =>
                                               warnings.Ignore(CoreEventId.NavigationBaseIncludeIgnored));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        DefineIdentities(modelBuilder);

        DefineGameEntity(modelBuilder);

        DefineGroupEntity(modelBuilder);

        DefineKindEntity(modelBuilder);

        DefinePlayerEntity(modelBuilder);

        DefinePlayersListView(modelBuilder);

        DefineRatioEntity(modelBuilder);

        DefineSingleRatingListView(modelBuilder);

        DefineSystemEntity(modelBuilder);

        DefineTeamEntity(modelBuilder);

        DefineTeamRatingListView(modelBuilder);

        DefineTeamView(modelBuilder);

        DefineTeamListView(modelBuilder);

        DefineTournamentEntity(modelBuilder);

        DefineUserEntity(modelBuilder);
    }

    private static void DefineIdentities(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdentityUserLogin<int>>(entity =>
                                                    {
                                                        entity.HasKey(e => new { e.LoginProvider, e.ProviderKey })
                                                              .HasName("user_logins_pk");

                                                        entity.ToTable("user_logins");

                                                        entity.HasIndex(e => e.UserId, "user_logins_user_id_idx");

                                                        entity.Property(e => e.LoginProvider)
                                                              .HasMaxLength(128)
                                                              .HasColumnName("login_provider");
                                                        entity.Property(e => e.ProviderKey)
                                                              .HasMaxLength(128)
                                                              .HasColumnName("provider_key");
                                                        entity.Property(e => e.ProviderDisplayName)
                                                              .HasColumnName("provider_display_name");
                                                        entity.Property(e => e.UserId)
                                                              .HasColumnName("user_id")
                                                              .HasColumnType("integer");
                                                    });

        modelBuilder.Entity<IdentityUserRole<int>>(entity =>
                                                   {
                                                       entity.HasKey(e => new { e.UserId, e.RoleId })
                                                             .HasName("user_roles_pk");

                                                       entity.ToTable("user_roles");

                                                       entity.HasIndex(e => e.RoleId, "user_roles_role_id_idx");

                                                       entity.Property(e => e.UserId)
                                                             .HasColumnName("user_id")
                                                             .HasColumnType("integer");
                                                       entity.Property(e => e.RoleId)
                                                             .HasColumnName("role_id")
                                                             .HasColumnType("integer");
                                                   });

        modelBuilder.Entity<IdentityUserToken<int>>(entity =>
                                                    {
                                                        entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name })
                                                              .HasName("user_tokens_pk");

                                                        entity.ToTable("user_tokens");

                                                        entity.Property(e => e.UserId)
                                                              .HasColumnName("user_id")
                                                              .HasColumnType("integer");
                                                        entity.Property(e => e.LoginProvider)
                                                              .HasMaxLength(128)
                                                              .HasColumnName("login_provider");
                                                        entity.Property(e => e.Name)
                                                              .HasMaxLength(128)
                                                              .HasColumnName("name");
                                                        entity.Property(e => e.Value)
                                                              .HasColumnName("value");
                                                    });

        modelBuilder.Entity<IdentityUserClaim<int>>(entity =>
                                                    {
                                                        entity.HasKey(e => e.Id).HasName("user_claims_pk");

                                                        entity.ToTable("user_claims");

                                                        entity.HasIndex(e => e.UserId, "user_claims_user_id_idx");

                                                        entity.Property(e => e.Id)
                                                              .HasColumnName("user_claim_id")
                                                              .HasColumnType("integer");
                                                        entity.Property(e => e.UserId)
                                                              .HasColumnName("user_id")
                                                              .HasColumnType("integer");
                                                        entity.Property(e => e.ClaimType)
                                                              .HasColumnName("claim_type");
                                                        entity.Property(e => e.ClaimValue)
                                                              .HasColumnName("claim_value");
                                                    });

        modelBuilder.Entity<IdentityRole<int>>(entity =>
                                               {
                                                   entity.HasKey(e => e.Id).HasName("roles_pk");

                                                   entity.ToTable("roles");

                                                   entity.HasIndex(e => e.NormalizedName, "roles_name_uq")
                                                         .IsUnique();

                                                   entity.Property(e => e.Id)
                                                         .HasColumnName("role_id")
                                                         .HasColumnType("integer");
                                                   entity.Property(e => e.Name)
                                                         .HasColumnName("name");
                                                   entity.Property(e => e.NormalizedName)
                                                         .HasColumnName("normalized_name");
                                               });

        modelBuilder.Entity<IdentityRoleClaim<int>>(entity =>
                                                    {
                                                        entity.HasKey(e => e.Id).HasName("role_claims_pk");

                                                        entity.ToTable("role_claims");

                                                        entity.HasIndex(e => e.RoleId, "role_claims_role_id_idx");

                                                        entity.Property(e => e.Id)
                                                              .HasColumnName("role_claim_id")
                                                              .HasColumnType("integer");
                                                        entity.Property(e => e.RoleId)
                                                              .HasColumnName("role_id")
                                                              .HasColumnType("integer");
                                                        entity.Property(e => e.ClaimType)
                                                              .HasColumnName("claim_type");
                                                        entity.Property(e => e.ClaimValue)
                                                              .HasColumnName("claim_value");
                                                    });
    }

    private static void DefineUserEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
                                  {
                                      entity.HasKey(e => e.Id).HasName("users_pk");

                                      entity.ToTable("users");

                                      entity.HasIndex(e => e.Email, "users_email_uq").IsUnique();

                                      entity.Property(e => e.Id).HasColumnName("user_id");
                                      entity.Property(e => e.Email)
                                            .HasMaxLength(255)
                                            .HasColumnName("email");
                                      entity.Property(e => e.PasswordHash)
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
                                      entity.Property(e => e.UserFirstName)
                                            .HasMaxLength(255)
                                            .HasColumnName("user_firstname");
                                      entity.Property(e => e.UserLastName)
                                            .HasMaxLength(255)
                                            .HasColumnName("user_lastname");
                                      entity.Property(e => e.UserPatronymic)
                                            .HasMaxLength(255)
                                            .HasDefaultValueSql("'-'::character varying")
                                            .HasColumnName("user_patronymic");
                                  });
    }

    private static void DefineTournamentEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tournament>(entity =>
                                        {
                                            entity.HasKey(e => new { TournamentId = e.Id, e.OrganizerId })
                                                  .HasName("tournaments_pk");

                                            entity.ToTable("tournaments");

                                            entity
                                               .HasIndex(e => new { TournamentId = e.Id, e.OrganizerId, e.TournamentName },
                                                         "tournaments_name_uq").IsUnique();

                                            entity.Property(e => e.Id)
                                                  .ValueGeneratedOnAdd()
                                                  .HasColumnName("tournament_id");
                                            entity.Property(e => e.OrganizerId)
                                                  .HasColumnName("organizer_id");
                                            entity.Property(e => e.DateCreate)
                                                  .HasDefaultValueSql("(now())::date")
                                                  .HasColumnName("date_create");
                                            entity.Property(e => e.DateLastChange)
                                                  .HasDefaultValueSql("(now())::date")
                                                  .HasColumnName("date_last_change");
                                            entity.Property(e => e.DateStart)
                                                  .HasDefaultValueSql("(now())::date")
                                                  .HasColumnName("date_start");
                                            entity.Property(e => e.Duration)
                                                  .HasColumnName("duration");
                                            entity.Property(e => e.IsMixedGroups)
                                                  .IsRequired()
                                                  .HasDefaultValueSql("true")
                                                  .HasColumnName("is_mixed_groups");
                                            entity.Property(e => e.KindId)
                                                  .HasColumnName("kind_id");
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
                                            entity.Property(e => e.SystemId)
                                                  .HasColumnName("system_id");
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

                                            entity.HasOne(d => d.Kind)
                                                  .WithMany(p => p.Tournaments)
                                                  .HasForeignKey(d => d.KindId)
                                                  .OnDelete(DeleteBehavior.Restrict)
                                                  .HasConstraintName("tournaments_have_kinds_fk");

                                            entity.HasOne(d => d.Organizer)
                                                  .WithMany(p => p.Tournaments)
                                                  .HasForeignKey(d => d.OrganizerId)
                                                  .HasConstraintName("tournaments_have_organizers_fk");

                                            entity.HasOne(d => d.System)
                                                  .WithMany(p => p.Tournaments)
                                                  .HasForeignKey(d => d.SystemId)
                                                  .OnDelete(DeleteBehavior.Restrict)
                                                  .HasConstraintName("tournaments_have_systems_fk");

                                            entity.HasMany(d => d.Ratios)
                                                  .WithMany(p => p.Tournaments)
                                                  .UsingEntity<Dictionary<string, object>>(
                                                    "TournamentRatio",
                                                    r => r.HasOne<Ratio>()
                                                          .WithMany()
                                                          .HasForeignKey("RatioId")
                                                          .OnDelete(DeleteBehavior.Restrict)
                                                          .HasConstraintName("tournaments_ratios_in_ratios_fk"),
                                                    l => l.HasOne<Tournament>()
                                                          .WithMany()
                                                          .HasForeignKey("TournamentId", "OrganizerId")
                                                          .HasConstraintName("tournaments_ratios_in_tournaments_fk"),
                                                    j =>
                                                    {
                                                        j.HasKey("TournamentId", "OrganizerId",
                                                                 "RatioId")
                                                         .HasName("tournament_ratios_pk");
                                                        j.ToTable("tournament_ratios");
                                                    });
                                        });
    }

    private static void DefineTeamListView(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TeamsListView>(entity =>
                                           {
                                               entity
                                                  .HasNoKey()
                                                  .ToView("teams_list_view");

                                               entity.Property(e => e.IsActive)
                                                     .HasColumnName("is_active");
                                               entity.Property(e => e.OrganizerId)
                                                     .HasColumnName("organizer_id");
                                               entity.Property(e => e.PlayersCount)
                                                     .HasColumnName("players_count");
                                               entity.Property(e => e.TeamAttribute)
                                                     .HasMaxLength(3)
                                                     .IsFixedLength()
                                                     .HasColumnName("team_attribute");
                                               entity.Property(e => e.TeamId)
                                                     .HasColumnName("team_id");
                                               entity.Property(e => e.TeamName)
                                                     .HasMaxLength(255)
                                                     .HasColumnName("team_name");
                                               entity.Property(e => e.TournamentId)
                                                     .HasColumnName("tournament_id");
                                           });
    }

    private static void DefineTeamView(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TeamView>(entity =>
                                      {
                                          entity
                                             .HasNoKey()
                                             .ToView("team_view");

                                          entity.Property(e => e.BoardNumber)
                                                .HasColumnName("board_number");
                                          entity.Property(e => e.GroupIdent)
                                                .HasMaxLength(4)
                                                .IsFixedLength()
                                                .HasColumnName("group_ident");
                                          entity.Property(e => e.IsActive)
                                                .HasColumnName("is_active");
                                          entity.Property(e => e.OrganizerId)
                                                .HasColumnName("organizer_id");
                                          entity.Property(e => e.PlayerFirstName)
                                                .HasMaxLength(255)
                                                .HasColumnName("player_first_name");
                                          entity.Property(e => e.PlayerId)
                                                .HasColumnName("player_id");
                                          entity.Property(e => e.PlayerLastName)
                                                .HasMaxLength(255)
                                                .HasColumnName("player_last_name");
                                          entity.Property(e => e.TeamId)
                                                .HasColumnName("team_id");
                                          entity.Property(e => e.TournamentId)
                                                .HasColumnName("tournament_id");
                                      });
    }

    private static void DefineTeamRatingListView(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TeamRatingListView>(entity =>
                                                {
                                                    entity
                                                       .HasNoKey()
                                                       .ToView("team_rating_list_view");

                                                    entity.Property(e => e.DrawsCount)
                                                          .HasColumnName("draws_count");
                                                    entity.Property(e => e.LossesCount)
                                                          .HasColumnName("losses_count");
                                                    entity.Property(e => e.OrganizerId)
                                                          .HasColumnName("organizer_id");
                                                    entity.Property(e => e.PointsCount)
                                                          .HasColumnName("points_count");
                                                    entity.Property(e => e.RatioSum1)
                                                          .HasColumnName("ratio_sum1");
                                                    entity.Property(e => e.RatioSum2)
                                                          .HasColumnName("ratio_sum2");
                                                    entity.Property(e => e.TeamAttribute)
                                                          .HasMaxLength(3)
                                                          .IsFixedLength()
                                                          .HasColumnName("team_attribute");
                                                    entity.Property(e => e.TeamId)
                                                          .HasColumnName("team_id");
                                                    entity.Property(e => e.TeamName)
                                                          .HasMaxLength(255)
                                                          .HasColumnName("team_name");
                                                    entity.Property(e => e.TeamRank)
                                                          .HasColumnName("team_rank");
                                                    entity.Property(e => e.TournamentId)
                                                          .HasColumnName("tournament_id");
                                                    entity.Property(e => e.WinsCount)
                                                          .HasColumnName("wins_count");
                                                });
    }

    private static void DefineTeamEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Team>(entity =>
                                  {
                                      entity.HasKey(e => new
                                                         {
                                                             TeamId = (int?)e.Id, e.OrganizerId,
                                                             e.TournamentId
                                                         })
                                            .HasName("teams_pk");

                                      entity.ToTable("teams");

                                      entity.HasIndex(e => new { TeamId = e.Id, e.TournamentId, e.TeamName },
                                                      "teams_name_uq")
                                            .IsUnique();

                                      entity.Property(e => e.Id)
                                            .ValueGeneratedOnAdd()
                                            .HasColumnName("team_id");
                                      entity.Property(e => e.OrganizerId)
                                            .HasColumnName("organizer_id");
                                      entity.Property(e => e.TournamentId)
                                            .HasColumnName("tournament_id");
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

                                      entity.HasOne(d => d.Tournament)
                                            .WithMany(p => p.Teams)
                                            .HasForeignKey(d => new { d.TournamentId, d.OrganizerId })
                                            .HasConstraintName("teams_in_tournaments_fk");
                                  });
    }

    private static void DefineSystemEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Entities.System>(entity =>
                                             {
                                                   entity.HasKey(e => e.Id).HasName("systems_pk");

                                                   entity.ToTable("systems");

                                                   entity.HasIndex(e => e.Name, "systems_name_uq")
                                                         .IsUnique();

                                                   entity.Property(e => e.Id)
                                                         .HasColumnName("system_id");
                                                   entity.Property(e => e.Name)
                                                         .HasMaxLength(255)
                                                         .HasColumnName("system_name");
                                             });
    }

    private static void DefineSingleRatingListView(ModelBuilder modelBuilder)
    {
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
                                                      entity.Property(e => e.PlayerId)
                                                            .HasColumnName("player_id");
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
                                                      entity.Property(e => e.WinsCount)
                                                            .HasColumnName("wins_count");
                                                  });
    }

    private static void DefineRatioEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ratio>(entity =>
                                   {
                                       entity.HasKey(e => e.RatioId)
                                             .HasName("ratios_pk");

                                       entity.ToTable("ratios");

                                       entity.HasIndex(e => e.RatioName, "ratios_name_uq")
                                             .IsUnique();

                                       entity.Property(e => e.RatioId)
                                             .HasColumnName("ratio_id");
                                       entity.Property(e => e.RatioName)
                                             .HasMaxLength(255)
                                             .HasColumnName("ratio_name");
                                   });
    }

    private static void DefinePlayersListView(ModelBuilder modelBuilder)
    {
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
                                                 entity.Property(e => e.IsActive)
                                                       .HasColumnName("is_active");
                                                 entity.Property(e => e.OrganizerId)
                                                       .HasColumnName("organizer_id");
                                                 entity.Property(e => e.PlayerAttribute)
                                                       .HasMaxLength(3)
                                                       .IsFixedLength()
                                                       .HasColumnName("player_attribute");
                                                 entity.Property(e => e.PlayerBirthYear)
                                                       .HasColumnName("player_birth_year");
                                                 entity.Property(e => e.PlayerFirstName)
                                                       .HasMaxLength(255)
                                                       .HasColumnName("player_first_name");
                                                 entity.Property(e => e.PlayerId)
                                                       .HasColumnName("player_id");
                                                 entity.Property(e => e.PlayerLastName)
                                                       .HasMaxLength(255)
                                                       .HasColumnName("player_last_name");
                                                 entity.Property(e => e.TeamName)
                                                       .HasMaxLength(255)
                                                       .HasColumnName("team_name");
                                                 entity.Property(e => e.TournamentId)
                                                       .HasColumnName("tournament_id");
                                             });
    }

    private static void DefinePlayerEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Player>(entity =>
                                    {
                                        entity.HasKey(e => new { PlayerId = e.Id, e.TournamentId, e.OrganizerId })
                                              .HasName("players_pk");

                                        entity.ToTable("players");

                                        entity
                                           .HasIndex(e => new { PlayerId = e.Id, e.TournamentId, e.PlayerLastName, e.PlayerFirstName, e.PlayerAttribute },
                                                     "players_name_attr_uq")
                                           .IsUnique();

                                        entity.Property(e => e.Id)
                                              .ValueGeneratedOnAdd()
                                              .HasColumnName("player_id");
                                        entity.Property(e => e.TournamentId)
                                              .HasColumnName("tournament_id");
                                        entity.Property(e => e.OrganizerId)
                                              .HasColumnName("organizer_id");
                                        entity.Property(e => e.BoardNumber)
                                              .HasDefaultValueSql("1")
                                              .HasColumnName("board_number");
                                        entity.Property(e => e.DrawsCount)
                                              .HasColumnName("draws_count");
                                        entity.Property(e => e.Gender)
                                              .HasMaxLength(1)
                                              .HasColumnName("gender");
                                        entity.Property(e => e.GroupId)
                                              .HasColumnName("group_id");
                                        entity.Property(e => e.IsActive)
                                              .IsRequired()
                                              .HasDefaultValueSql("true")
                                              .HasColumnName("is_active");
                                        entity.Property(e => e.LossesCount)
                                              .HasColumnName("losses_count");
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
                                        entity.Property(e => e.PointsAmount)
                                              .HasColumnName("points_count");
                                        entity.Property(e => e.RatioSum1)
                                              .HasPrecision(5, 2)
                                              .HasColumnName("ratio_sum1");
                                        entity.Property(e => e.RatioSum2)
                                              .HasPrecision(5, 2)
                                              .HasColumnName("ratio_sum2");
                                        entity.Property(e => e.TeamId).HasColumnName("team_id");
                                        entity.Property(e => e.WinsCount).HasColumnName("wins_count");

                                        entity.HasOne(d => d.Tournament)
                                              .WithMany(p => p.Players)
                                              .HasForeignKey(d => new { d.TournamentId, d.OrganizerId })
                                              .HasConstraintName("players_take_part_in_tournaments_fk");

                                        entity.HasOne(d => d.Group)
                                              .WithMany(p => p.Players)
                                              .HasForeignKey(d => new
                                                                  {
                                                                      d.GroupId, d.TournamentId,
                                                                      d.OrganizerId
                                                                  })
                                              .OnDelete(DeleteBehavior.Restrict)
                                              .HasConstraintName("players_in_groups_fk");

                                        entity.HasOne(d => d.Team).WithMany(p => p.Players)
                                              .HasForeignKey(d => new
                                                                  {
                                                                      d.TeamId, d.OrganizerId,
                                                                      d.TournamentId
                                                                  })
                                              .OnDelete(DeleteBehavior.Restrict)
                                              .HasConstraintName("players_in_teams_fk");
                                    });
    }

    private static void DefineKindEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Kind>(entity =>
                                  {
                                      entity.HasKey(e => e.Id).HasName("kinds_pk");

                                      entity.ToTable("kinds");

                                      entity.HasIndex(e => e.Name, "kinds_name_uq").IsUnique();

                                      entity.Property(e => e.Id).HasColumnName("kind_id");
                                      entity.Property(e => e.Name)
                                            .HasMaxLength(255)
                                            .HasColumnName("kind_name");
                                  });
    }

    private static void DefineGroupEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Group>(entity =>
                                   {
                                       entity.HasKey(e => new
                                                          {
                                                              GroupId = (int?)e.Id, e.TournamentId,
                                                              e.OrganizerId
                                                          })
                                             .HasName("groups_pk");

                                       entity.ToTable("groups");

                                       entity.HasIndex(e => new { e.TournamentId, e.OrganizerId, e.GroupName },
                                                       "group_name_uq").IsUnique();

                                       entity.HasIndex(e => new { e.TournamentId, e.OrganizerId, e.Identity },
                                                       "identity_uq").IsUnique();

                                       entity.Property(e => e.Id)
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

                                       entity.HasOne(d => d.Tournament)
                                             .WithMany(p => p.Groups)
                                             .HasForeignKey(d => new { d.TournamentId, d.OrganizerId })
                                             .HasConstraintName("groups_in_tournaments_fk");
                                   });
    }

    private static void DefineGameEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>(entity =>
                                  {
                                      entity.HasKey(e => new
                                                         {
                                                             e.WhiteId, e.BlackId, e.TournamentId,
                                                             e.OrganizerId
                                                         })
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

                                      entity.HasOne(d => d.PlayerBlack)
                                            .WithMany(p => p.GamesWhiteOpponents)
                                            .HasForeignKey(d => new
                                                                {
                                                                    d.BlackId, d.TournamentId,
                                                                    d.OrganizerId
                                                                })
                                            .OnDelete(DeleteBehavior.Restrict)
                                            .HasConstraintName("games_have_black_players_fk");

                                      entity.HasOne(d => d.PlayerWhite)
                                            .WithMany(p => p.GamesBlackOpponents)
                                            .HasForeignKey(d => new
                                                                {
                                                                    d.WhiteId, d.TournamentId,
                                                                    d.OrganizerId
                                                                })
                                            .OnDelete(DeleteBehavior.Restrict)
                                            .HasConstraintName("games_have_white_players_fk");
                                  });
    }
}
