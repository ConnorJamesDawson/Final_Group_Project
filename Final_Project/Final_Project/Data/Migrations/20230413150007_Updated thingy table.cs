using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final_Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class Updatedthingytable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Stop_SelfFeedback",
                table: "Personal_Tracker",
                newName: "TrainerComments");

            migrationBuilder.RenameColumn(
                name: "Start_SelfFeedback",
                table: "Personal_Tracker",
                newName: "StopSelfFeedback");

            migrationBuilder.RenameColumn(
                name: "Continue_SelfFeedback",
                table: "Personal_Tracker",
                newName: "StartSelfFeedback");

            migrationBuilder.RenameColumn(
                name: "Comments_SelfFeedback",
                table: "Personal_Tracker",
                newName: "ContinueSelfFeedback");

            migrationBuilder.AddColumn<string>(
                name: "CommentsSelfFeedback",
                table: "Personal_Tracker",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommentsSelfFeedback",
                table: "Personal_Tracker");

            migrationBuilder.RenameColumn(
                name: "TrainerComments",
                table: "Personal_Tracker",
                newName: "Stop_SelfFeedback");

            migrationBuilder.RenameColumn(
                name: "StopSelfFeedback",
                table: "Personal_Tracker",
                newName: "Start_SelfFeedback");

            migrationBuilder.RenameColumn(
                name: "StartSelfFeedback",
                table: "Personal_Tracker",
                newName: "Continue_SelfFeedback");

            migrationBuilder.RenameColumn(
                name: "ContinueSelfFeedback",
                table: "Personal_Tracker",
                newName: "Comments_SelfFeedback");
        }
    }
}
