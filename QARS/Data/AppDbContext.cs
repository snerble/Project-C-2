using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

using QARS.Data.Models;

using System.Collections.Generic;

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
			modelBuilder.Entity<Car>().HasData(GetCars());
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<CarModel>().HasData(GetCarModels());
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Extra>().HasData(GetExtras());
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Location>().HasData(GetLocations());
			base.OnModelCreating(modelBuilder);

			#region Reservation Configuration
			modelBuilder.Entity<Reservation>(entity =>
			{
				// Make some properties readonly after insertion
				entity.Property($"{nameof(Reservation.Customer)}Id")
					.Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
				entity.Property($"{nameof(Reservation.Car)}Id")
					.Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
				entity.Property($"{nameof(Reservation.CarLocation)}Id")
					.Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
				entity.Property(r => r.InitialMileage)
					.Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
			});
			#endregion

			#region ReservationExtra Configuration
			modelBuilder.Entity<ReservationExtra>(entity =>
			{
				// Rename the table to ReservationExtras and add composite primary key on ReservationId and ExtraId
				entity.ToTable("ReservationExtras")
					.HasKey($"{nameof(ReservationExtra.Reservation)}Id", $"{nameof(ReservationExtra.Extra)}Id");

				// Configure the many-to-many relation between Reservations and Extras
				entity.HasOne(ce => ce.Reservation)
					.WithMany(r => r.Extras);
				entity.HasOne(ce => ce.Extra)
					.WithMany(e => e.ReservationExtras);
			});
			#endregion

			modelBuilder.Entity<Store>().HasData(GetStores());
			base.OnModelCreating(modelBuilder);

			#region User Configuration
			modelBuilder.Entity<User>(entity =>
			{
				// Set up the one-way User.Location foreign key and prevent the Location from being deleted
				entity.HasOne(u => u.Location)
					.WithOne()
					.OnDelete(DeleteBehavior.Restrict);

				// Set unique key on User.Email
				entity.HasIndex(u => u.Email)
					.IsUnique();

				// Make some properties readonly after insertion
				entity.Property(u => u.LocationId) // The Location object should change instead of the FK
					.Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
			});
			#endregion

			modelBuilder.Entity<Franchisee>().HasData(GetFranchisees());
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Employee>().HasData(GetEmployees());
			base.OnModelCreating(modelBuilder);
		}

		public DbSet<Location> Locations { get; set; }

		public DbSet<Administrator> Admins { get; set; }
		public DbSet<Franchisee> Franchisees { get; set; }
		public DbSet<Employee> Employees { get; set; }
		public DbSet<Customer> Customers { get; set; }

		public DbSet<Store> Stores { get; set; }

		public DbSet<Car> Cars { get; set; }

		public DbSet<CarModel> CarModels { get; set; }

		public DbSet<Extra> Extras { get; set; }
		public DbSet<Reservation> Reservations { get; set; }

		public DbSet<Contact> Contacts { get; set; }

		private List<CarModel> GetCarModels()
		{
			return new List<CarModel>
			{

			};
		}

		private List<Car> GetCars()
		{
			return new List<Car>
			{
				
			};
		}

		private List<Store> GetStores()
		{
			return new List<Store>
			{

			};
		}

		private List<Franchisee> GetFranchisees()
		{
			return new List<Franchisee>
			{

			};
		}

		private List<Location> GetLocations()
		{
			return new List<Location>
			{

			};
		}
		private List<Employee> GetEmployees()
		{
			return new List<Employee>
			{

			};
		}

		private List<Extra> GetExtras()
		{
			return new List<Extra>
			{

			};
		}

	}
}
