using Microsoft.EntityFrameworkCore;

using QARS.Data.Models;

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

			modelBuilder.Entity<CarExtra>(entity =>
			{
				entity
					.ToTable("CarExtras")
					.HasKey(ce => new { ce.CarId, ce.ExtraId });

				// Configure the many-to-many relation between Cars and Extras
				entity
					.HasOne(ce => ce.Car)
					.WithMany(c => c.CarExtras);
				entity
					.HasOne(ce => ce.Extra)
					.WithMany(e => e.CarExtras);
			});
		}

		public DbSet<User> Users { get; set; }
		public DbSet<Car> Cars { get; set; }
		public DbSet<Extra> Extras { get; set; }
	}
}
