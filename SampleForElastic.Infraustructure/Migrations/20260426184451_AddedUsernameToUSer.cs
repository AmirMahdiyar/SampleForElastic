using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SampleForElastic.Infraustructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedUsernameToUSer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Username",
                schema: "dbo",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                schema: "dbo",
                table: "Users");
        }
    }
}
