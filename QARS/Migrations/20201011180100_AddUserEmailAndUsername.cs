using Microsoft.EntityFrameworkCore.Migrations;

namespace QARS.Migrations
{
    public partial class AddUserEmailAndUsername : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Users",
				newName: "FullName");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
			// Rename the old users table
			migrationBuilder.RenameTable(
				name: "Users",
				newName: "Users_old");

			// Recreate the old users table
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

			// Transfer the data from the old users table
			migrationBuilder.Sql(@"INSERT INTO Users SELECT Id,FullName,Age FROM Users_old;");

			// Drop the old users table
			migrationBuilder.DropTable(
				name: "Users_old");
        }
    }
}
