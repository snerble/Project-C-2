using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using QARS.Data.Models;

using System.Collections.Generic;
using System.Linq;

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

			#region CarModel Configuration
			modelBuilder.Entity<CarModel>(entity =>
			{
				entity.HasData(GetCarModels());
			});
			#endregion

			#region Extra Configuration
			// Extra needs no additional configuration
			#endregion

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
				// Add a composite primary key on ReservationId and ExtraId
				entity.ToTable($"{nameof(ReservationExtra)}s")
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

			#region UserRole Configuration
			modelBuilder.Entity<UserRole>(entity =>
			{
				// Add the composite primary key on the foreign key shadow properties
				entity.ToTable($"{nameof(UserRole)}s")
					.HasKey($"{nameof(UserRole.User)}Id", $"{nameof(UserRole.Role)}Id");

				// Configure the navigation property on User
				entity.HasOne(ur => ur.User)
					.WithMany(u => u.Roles);
			});
			#endregion

			#region User Configuration
			modelBuilder.Entity<User>(entity =>
			{
				// Set up the one-way User.Location foreign key and prevent the Location from being deleted
				entity.HasOne(u => u.Location)
					.WithMany()
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

			modelBuilder.Entity<Contact>().HasData(GetContacts());
			base.OnModelCreating(modelBuilder);
		}

		public DbSet<Car> Cars { get; set; }
		public DbSet<CarModel> CarModels { get; set; }
		public DbSet<Extra> Extras { get; set; }
		public DbSet<Location> Locations { get; set; }
		public DbSet<Reservation> Reservations { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Store> Stores { get; set; }
		public DbSet<Customer> Customers { get; set; }
		public DbSet<Employee> Employees { get; set; }
		public DbSet<Franchisee> Franchisees { get; set; }
		public DbSet<Administrator> Admins { get; set; }

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

		private List<Contact> GetContacts()
		{
			return new List<Contact>
			{

			};
		}

	}
}
