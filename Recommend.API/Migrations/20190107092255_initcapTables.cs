using Microsoft.EntityFrameworkCore.Migrations;

namespace Recommend.API.Migrations
{
    public partial class initcapTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "FinStage",
                table: "ProjectRecommends",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FinStage",
                table: "ProjectRecommends",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
