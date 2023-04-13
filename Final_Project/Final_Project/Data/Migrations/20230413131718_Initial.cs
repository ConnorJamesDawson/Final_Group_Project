using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final_Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Personal_Tracker",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Stop_SelfFeedback = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Start_SelfFeedback = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Continue_SelfFeedback = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comments_SelfFeedback = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpartanId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personal_Tracker", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Personal_Tracker_AspNetUsers_SpartanId",
                        column: x => x.SpartanId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TraineeProfile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AboutMe = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    WorkExperience = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Complete = table.Column<bool>(type: "bit", nullable: false),
                    SpartanId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TraineeProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TraineeProfile_AspNetUsers_SpartanId",
                        column: x => x.SpartanId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Personal_Tracker_SpartanId",
                table: "Personal_Tracker",
                column: "SpartanId");

            migrationBuilder.CreateIndex(
                name: "IX_TraineeProfile_SpartanId",
                table: "TraineeProfile",
                column: "SpartanId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Personal_Tracker");

            migrationBuilder.DropTable(
                name: "TraineeProfile");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");
        }
    }
}
