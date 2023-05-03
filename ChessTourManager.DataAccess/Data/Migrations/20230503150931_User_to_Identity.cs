using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ChessTourManager.DataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class User_to_Identity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "kinds",
                columns: table => new
                {
                    kind_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    kind_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("kinds_pk", x => x.kind_id);
                });

            migrationBuilder.CreateTable(
                name: "ratios",
                columns: table => new
                {
                    ratio_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ratio_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ratios_pk", x => x.ratio_id);
                });

            migrationBuilder.CreateTable(
                name: "systems",
                columns: table => new
                {
                    system_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    system_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("systems_pk", x => x.system_id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    NormalizedUserName = table.Column<string>(type: "text", nullable: false),
                    user_lastname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    user_firstname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    user_patronymic = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, defaultValueSql: "'-'::character varying"),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    pass_hash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    tournaments_lim = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "50"),
                    register_date = table.Column<DateOnly>(type: "date", nullable: false, defaultValueSql: "(now())::date"),
                    register_time = table.Column<TimeOnly>(type: "time(6) without time zone", precision: 6, nullable: false, defaultValueSql: "(now())::time(6) without time zone"),
                    NormalizedEmail = table.Column<string>(type: "text", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_pk", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "tournaments",
                columns: table => new
                {
                    tournament_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    organizer_id = table.Column<int>(type: "integer", nullable: false),
                    tournament_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    tours_count = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "7"),
                    place = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, defaultValueSql: "'-'::character varying"),
                    date_start = table.Column<DateOnly>(type: "date", nullable: false, defaultValueSql: "(now())::date"),
                    time_start = table.Column<TimeOnly>(type: "time(6) without time zone", precision: 6, nullable: false, defaultValueSql: "(now())::time(6) without time zone"),
                    duration = table.Column<int>(type: "integer", nullable: false),
                    max_team_players = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "5"),
                    organization_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, defaultValueSql: "'-'::character varying"),
                    is_mixed_groups = table.Column<bool>(type: "boolean", nullable: false, defaultValueSql: "true"),
                    date_create = table.Column<DateOnly>(type: "date", nullable: false, defaultValueSql: "(now())::date"),
                    time_create = table.Column<TimeOnly>(type: "time(6) without time zone", precision: 6, nullable: false, defaultValueSql: "(now())::time(6) without time zone"),
                    date_last_change = table.Column<DateOnly>(type: "date", nullable: false, defaultValueSql: "(now())::date"),
                    time_last_change = table.Column<TimeOnly>(type: "time(6) without time zone", precision: 6, nullable: false, defaultValueSql: "(now())::time(6) without time zone"),
                    system_id = table.Column<int>(type: "integer", nullable: false),
                    kind_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("tournaments_pk", x => new { x.tournament_id, x.organizer_id });
                    table.ForeignKey(
                        name: "tournaments_have_kinds_fk",
                        column: x => x.kind_id,
                        principalTable: "kinds",
                        principalColumn: "kind_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "tournaments_have_organizers_fk",
                        column: x => x.organizer_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "tournaments_have_systems_fk",
                        column: x => x.system_id,
                        principalTable: "systems",
                        principalColumn: "system_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "groups",
                columns: table => new
                {
                    group_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tournament_id = table.Column<int>(type: "integer", nullable: false),
                    organizer_id = table.Column<int>(type: "integer", nullable: false),
                    identity = table.Column<string>(type: "character(4)", fixedLength: true, maxLength: 4, nullable: false, defaultValueSql: "'1'::bpchar"),
                    group_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, defaultValueSql: "'1'::character varying")
                },
                constraints: table =>
                {
                    table.PrimaryKey("groups_pk", x => new { x.group_id, x.tournament_id, x.organizer_id });
                    table.ForeignKey(
                        name: "groups_in_tournaments_fk",
                        columns: x => new { x.tournament_id, x.organizer_id },
                        principalTable: "tournaments",
                        principalColumns: new[] { "tournament_id", "organizer_id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "teams",
                columns: table => new
                {
                    team_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    organizer_id = table.Column<int>(type: "integer", nullable: false),
                    tournament_id = table.Column<int>(type: "integer", nullable: false),
                    team_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    team_attribute = table.Column<string>(type: "character(3)", fixedLength: true, maxLength: 3, nullable: false, defaultValueSql: "'   '::bpchar"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValueSql: "true")
                },
                constraints: table =>
                {
                    table.PrimaryKey("teams_pk", x => new { x.team_id, x.organizer_id, x.tournament_id });
                    table.ForeignKey(
                        name: "teams_in_tournaments_fk",
                        columns: x => new { x.tournament_id, x.organizer_id },
                        principalTable: "tournaments",
                        principalColumns: new[] { "tournament_id", "organizer_id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tournament_ratios",
                columns: table => new
                {
                    TournamentId = table.Column<int>(type: "integer", nullable: false),
                    OrganizerId = table.Column<int>(type: "integer", nullable: false),
                    RatioId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("tournament_ratios_pk", x => new { x.TournamentId, x.OrganizerId, x.RatioId });
                    table.ForeignKey(
                        name: "tournaments_ratios_in_ratios_fk",
                        column: x => x.RatioId,
                        principalTable: "ratios",
                        principalColumn: "ratio_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "tournaments_ratios_in_tournaments_fk",
                        columns: x => new { x.TournamentId, x.OrganizerId },
                        principalTable: "tournaments",
                        principalColumns: new[] { "tournament_id", "organizer_id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "players",
                columns: table => new
                {
                    player_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tournament_id = table.Column<int>(type: "integer", nullable: false),
                    organizer_id = table.Column<int>(type: "integer", nullable: false),
                    player_last_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    player_first_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    gender = table.Column<char>(type: "character(1)", maxLength: 1, nullable: false),
                    player_attribute = table.Column<string>(type: "character(3)", fixedLength: true, maxLength: 3, nullable: false, defaultValueSql: "'   '::bpchar"),
                    player_birth_year = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "EXTRACT(year FROM now())"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValueSql: "true"),
                    points_count = table.Column<double>(type: "double precision", nullable: false),
                    wins_count = table.Column<int>(type: "integer", nullable: false),
                    losses_count = table.Column<int>(type: "integer", nullable: false),
                    draws_count = table.Column<int>(type: "integer", nullable: false),
                    ratio_sum1 = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    ratio_sum2 = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    board_number = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "1"),
                    team_id = table.Column<int>(type: "integer", nullable: true),
                    group_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("players_pk", x => new { x.player_id, x.tournament_id, x.organizer_id });
                    table.ForeignKey(
                        name: "players_in_groups_fk",
                        columns: x => new { x.group_id, x.tournament_id, x.organizer_id },
                        principalTable: "groups",
                        principalColumns: new[] { "group_id", "tournament_id", "organizer_id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "players_in_teams_fk",
                        columns: x => new { x.team_id, x.organizer_id, x.tournament_id },
                        principalTable: "teams",
                        principalColumns: new[] { "team_id", "organizer_id", "tournament_id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "players_take_part_in_tournaments_fk",
                        columns: x => new { x.tournament_id, x.organizer_id },
                        principalTable: "tournaments",
                        principalColumns: new[] { "tournament_id", "organizer_id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "games",
                columns: table => new
                {
                    white_id = table.Column<int>(type: "integer", nullable: false),
                    black_id = table.Column<int>(type: "integer", nullable: false),
                    tournament_id = table.Column<int>(type: "integer", nullable: false),
                    organizer_id = table.Column<int>(type: "integer", nullable: false),
                    tour_number = table.Column<int>(type: "integer", nullable: false),
                    white_points = table.Column<double>(type: "double precision", nullable: false),
                    black_points = table.Column<double>(type: "double precision", nullable: false),
                    is_played = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("games_pk", x => new { x.white_id, x.black_id, x.tournament_id, x.organizer_id });
                    table.ForeignKey(
                        name: "games_have_black_players_fk",
                        columns: x => new { x.black_id, x.tournament_id, x.organizer_id },
                        principalTable: "players",
                        principalColumns: new[] { "player_id", "tournament_id", "organizer_id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "games_have_white_players_fk",
                        columns: x => new { x.white_id, x.tournament_id, x.organizer_id },
                        principalTable: "players",
                        principalColumns: new[] { "player_id", "tournament_id", "organizer_id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "games_tour_number_uq",
                table: "games",
                columns: new[] { "white_id", "black_id", "tournament_id", "organizer_id", "tour_number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_games_black_id_tournament_id_organizer_id",
                table: "games",
                columns: new[] { "black_id", "tournament_id", "organizer_id" });

            migrationBuilder.CreateIndex(
                name: "IX_games_white_id_tournament_id_organizer_id",
                table: "games",
                columns: new[] { "white_id", "tournament_id", "organizer_id" });

            migrationBuilder.CreateIndex(
                name: "group_name_uq",
                table: "groups",
                columns: new[] { "tournament_id", "organizer_id", "group_name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "identity_uq",
                table: "groups",
                columns: new[] { "tournament_id", "organizer_id", "identity" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "kinds_name_uq",
                table: "kinds",
                column: "kind_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_players_group_id_tournament_id_organizer_id",
                table: "players",
                columns: new[] { "group_id", "tournament_id", "organizer_id" });

            migrationBuilder.CreateIndex(
                name: "IX_players_team_id_organizer_id_tournament_id",
                table: "players",
                columns: new[] { "team_id", "organizer_id", "tournament_id" });

            migrationBuilder.CreateIndex(
                name: "IX_players_tournament_id_organizer_id",
                table: "players",
                columns: new[] { "tournament_id", "organizer_id" });

            migrationBuilder.CreateIndex(
                name: "players_name_attr_uq",
                table: "players",
                columns: new[] { "player_id", "tournament_id", "player_last_name", "player_first_name", "player_attribute" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ratios_name_uq",
                table: "ratios",
                column: "ratio_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "systems_name_uq",
                table: "systems",
                column: "system_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_teams_tournament_id_organizer_id",
                table: "teams",
                columns: new[] { "tournament_id", "organizer_id" });

            migrationBuilder.CreateIndex(
                name: "teams_name_uq",
                table: "teams",
                columns: new[] { "team_id", "tournament_id", "team_name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tournament_ratios_RatioId",
                table: "tournament_ratios",
                column: "RatioId");

            migrationBuilder.CreateIndex(
                name: "IX_tournaments_kind_id",
                table: "tournaments",
                column: "kind_id");

            migrationBuilder.CreateIndex(
                name: "IX_tournaments_organizer_id",
                table: "tournaments",
                column: "organizer_id");

            migrationBuilder.CreateIndex(
                name: "IX_tournaments_system_id",
                table: "tournaments",
                column: "system_id");

            migrationBuilder.CreateIndex(
                name: "tournaments_name_uq",
                table: "tournaments",
                columns: new[] { "tournament_id", "organizer_id", "tournament_name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "users_email_uq",
                table: "users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "games");

            migrationBuilder.DropTable(
                name: "tournament_ratios");

            migrationBuilder.DropTable(
                name: "players");

            migrationBuilder.DropTable(
                name: "ratios");

            migrationBuilder.DropTable(
                name: "groups");

            migrationBuilder.DropTable(
                name: "teams");

            migrationBuilder.DropTable(
                name: "tournaments");

            migrationBuilder.DropTable(
                name: "kinds");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "systems");
        }
    }
}
