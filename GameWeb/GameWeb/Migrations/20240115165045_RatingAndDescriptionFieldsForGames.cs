using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameWeb.Migrations
{
    /// <inheritdoc />
    public partial class RatingAndDescriptionFieldsForGames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Games",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MainImagePath",
                table: "Games",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfVotes",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalStars",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "MainImagePath",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "NumberOfVotes",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "TotalStars",
                table: "Games");
        }
    }
}
