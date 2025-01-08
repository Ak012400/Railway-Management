using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Railway_Management.Migrations
{
    /// <inheritdoc />
    public partial class addnewdatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AllStates_AllCountries_countryphone",
                table: "AllStates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AllCountries",
                table: "AllCountries");

            migrationBuilder.RenameColumn(
                name: "countryphone",
                table: "AllStates",
                newName: "countryID");

            migrationBuilder.RenameIndex(
                name: "IX_AllStates_countryphone",
                table: "AllStates",
                newName: "IX_AllStates_countryID");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "countryphone",
                table: "AllCountries",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "countryID",
                table: "AllCountries",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AllCountries",
                table: "AllCountries",
                column: "countryID");

            migrationBuilder.AddForeignKey(
                name: "FK_AllStates_AllCountries_countryID",
                table: "AllStates",
                column: "countryID",
                principalTable: "AllCountries",
                principalColumn: "countryID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AllStates_AllCountries_countryID",
                table: "AllStates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AllCountries",
                table: "AllCountries");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "countryID",
                table: "AllCountries");

            migrationBuilder.RenameColumn(
                name: "countryID",
                table: "AllStates",
                newName: "countryphone");

            migrationBuilder.RenameIndex(
                name: "IX_AllStates_countryID",
                table: "AllStates",
                newName: "IX_AllStates_countryphone");

            migrationBuilder.AlterColumn<int>(
                name: "countryphone",
                table: "AllCountries",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AllCountries",
                table: "AllCountries",
                column: "countryphone");

            migrationBuilder.AddForeignKey(
                name: "FK_AllStates_AllCountries_countryphone",
                table: "AllStates",
                column: "countryphone",
                principalTable: "AllCountries",
                principalColumn: "countryphone",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
