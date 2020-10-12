using Microsoft.EntityFrameworkCore;

using QARS.Data.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QARS.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
			string dataSource = Database.GetDbConnection().DataSource;

			// If the database is sqlite and in-memory, open the underlying connection and generate the database.
			if (Database.IsSqlite() && dataSource == ":memory:")
			{
				Database.OpenConnection();
				Database.EnsureCreated();
			}
		}
		
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>()
				.HasIndex(u => new { u.Email })
				.IsUnique();

			modelBuilder.Entity<CarExtra>()
				.ToTable("CarExtras")
				.HasKey(ce => new { ce.CarId, ce.ExtraId });
			
			// Configure the many-to-many relation between cars and extras using the CarExtra join table
			modelBuilder.Entity<CarExtra>()
				.HasOne(ce => ce.Car)
				.WithMany(c => c.CarExtras);
			modelBuilder.Entity<CarExtra>()
				.HasOne(ce => ce.Extra)
				.WithMany(e => e.CarExtras);
		}

		public DbSet<User> Users { get; set; }
		public DbSet<Car> Cars { get; set; }
		public DbSet<Extra> Extras { get; set; }
	}
}
