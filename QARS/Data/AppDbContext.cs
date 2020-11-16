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

			#region Extra Configuration
			// Extra needs no additional configuration
			#endregion

			#region Location Configuration
			// Location needs no additional configuration
			#endregion

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

			#region Store Configuration
			modelBuilder.Entity<Store>(entity =>
			{
				// Set up the one-way Store.Location foreign key and prevent the Location from being deleted
				entity.HasOne(s => s.Location)
					.WithOne()
					.OnDelete(DeleteBehavior.Restrict);

				// Make some properties readonly after insertion
				entity.Property($"{nameof(Store.Franchisee)}Id")
					.Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
				entity.Property(s => s.LocationId) // The Location object should change instead of the FK
					.Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
			});
			#endregion

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

		private List<CarModel> GetCarModels()
		{
			return new List<CarModel>
			{
				new CarModel
				{
					Id = 1,
					Brand = "tesla",
					DayRate = 12.00m,
					Category = CarCategory.Midsize,
					Description = "it's a car",
					Doors = 5,
					Efficiency = 10.0f,
					Emission = 5,
					FreeMileage = 100m,
					FuelType = FuelType.Electric,
					HasAirconditioning = true,
					KMRate = 0.19m,
					Passengers = 5,
					SuitCases = 3,
					Transmission = TransmissionType.Automatic,
					Type = "person car",

				},
			};
		}

		private List<Car> GetCars()
		{
			return new List<Car>
			{
				
			};
		}


	}
}
