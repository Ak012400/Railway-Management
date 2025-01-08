using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Railway_Management.Migrations
{
    /// <inheritdoc />
    public partial class newtables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "TrainSchedule",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "trainID",
                table: "Train_Details",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.CreateTable(
                name: "AllCountries",
                columns: table => new
                {
                    countryphone = table.Column<int>(type: "int", nullable: false),
                    countrycode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    countryname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    countrycodealpha3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    flag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    symbol = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllCountries", x => x.countryphone);
                });

            migrationBuilder.CreateTable(
                name: "AllStates",
                columns: table => new
                {
                    stateid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    countryphone = table.Column<int>(type: "int", nullable: false),
                    statename = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllStates", x => x.stateid);
                    table.ForeignKey(
                        name: "FK_AllStates_AllCountries_countryphone",
                        column: x => x.countryphone,
                        principalTable: "AllCountries",
                        principalColumn: "countryphone",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AllStates_countryphone",
                table: "AllStates",
                column: "countryphone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AllStates");

            migrationBuilder.DropTable(
                name: "AllCountries");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "TrainSchedule",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "trainID",
                table: "Train_Details",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");
        }
    }
}
