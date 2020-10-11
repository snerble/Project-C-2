using Microsoft.EntityFrameworkCore.Migrations;

namespace QARS.Migrations
{
#pragma warning disable IDE1006 // Naming Styles
	public partial class initial : Migration
#pragma warning restore IDE1006 // Naming Styles
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Users",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					Name = table.Column<string>(nullable: true),
					Age = table.Column<int>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Users", x => x.Id);
				});
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Users");
		}
	}
}
