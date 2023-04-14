using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final_Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class PictureURL_added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PictureURL",
                table: "TraineeProfile",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PictureURL",
                table: "TraineeProfile");
        }
    }
}
