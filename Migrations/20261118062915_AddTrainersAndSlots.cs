using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IvanVGodDota2.Migrations
{
    /// <inheritdoc />
    public partial class AddTrainersAndSlots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TrainerId",
                table: "Services",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrainerId",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Trainers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Specialization = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Services_TrainerId",
                table: "Services",
                column: "TrainerId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_TrainerId",
                table: "Bookings",
                column: "TrainerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Trainers_TrainerId",
                table: "Bookings",
                column: "TrainerId",
                principalTable: "Trainers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Trainers_TrainerId",
                table: "Services",
                column: "TrainerId",
                principalTable: "Trainers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Trainers_TrainerId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Trainers_TrainerId",
                table: "Services");

            migrationBuilder.DropTable(
                name: "Trainers");

            migrationBuilder.DropIndex(
                name: "IX_Services_TrainerId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_TrainerId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "TrainerId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "TrainerId",
                table: "Bookings");
        }
    }
}
