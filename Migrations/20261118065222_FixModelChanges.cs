using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IvanVGodDota2.Migrations
{
    /// <inheritdoc />
    public partial class FixModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TrainerId",
                table: "CartItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_TrainerId",
                table: "CartItems",
                column: "TrainerId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Trainers_TrainerId",
                table: "CartItems",
                column: "TrainerId",
                principalTable: "Trainers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Trainers_TrainerId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_TrainerId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "TrainerId",
                table: "CartItems");
        }
    }
}
