using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class LicenseTypoAndLoadingValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LicenceText",
                table: "Plates",
                newName: "LicenseText");

            migrationBuilder.AddColumn<bool>(
                name: "IsLoading",
                table: "Plates",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLoading",
                table: "Plates");

            migrationBuilder.RenameColumn(
                name: "LicenseText",
                table: "Plates",
                newName: "LicenceText");
        }
    }
}
