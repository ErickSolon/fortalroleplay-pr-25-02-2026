using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FortalRPAPI.Migrations
{
    /// <inheritdoc />
    public partial class modificacaolocais : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoLocal",
                table: "Locais",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoLocal",
                table: "Locais");
        }
    }
}
