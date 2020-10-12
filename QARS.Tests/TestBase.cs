using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using QARS.Data;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace QARS.Tests
{
	/// <summary>
	/// Base class for all QARS unit testing.
	/// </summary>
	[TestClass]
	public abstract class TestBase
	{
		/// <summary>
		/// Gets the <see cref="IConfiguration"/> for this application.
		/// </summary>
		public static IConfiguration Configuration { get; private set; }
		/// <summary>
		/// Gets the database context for unit testing.
		/// </summary>
		public static AppDbContext Context { get; private set; }

		/// <summary>
		/// Gets or sets the testing context.
		/// </summary>
		public TestContext TestContext { get; set; }

		/// <summary>
		/// Sets up static resources used for testing.
		/// </summary>
		[AssemblyInitialize]
		public static async Task AssemblyInitialize(TestContext _)
		{
			// Load the configuration from appsettings.json
			Configuration = Host.CreateDefaultBuilder().Build().Services.GetService(typeof(IConfiguration)) as IConfiguration;

			// Create the database connection
			Context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
				.UseSqlite(Configuration.GetConnectionString(Configuration.GetValue<string>("UseConnection"))
					?? Configuration.GetConnectionString("DefaultConnection"))
				.Options);

			// Always ensure the database exists
			await Context.Database.EnsureCreatedAsync();
		}

		[AssemblyCleanup]
		public static void AssemblyCleanup()
		{
			if (Configuration.GetValue<bool>("ExportDatabase"))
			{
				var conn = Context.Database.GetDbConnection() as SqliteConnection;
				conn.Open();

				using var export = new SqliteConnection(Configuration.GetConnectionString("ExportConnection"));
				export.Open();
				conn.BackupDatabase(export);

				conn.Dispose();
			}
		}
	}
}
