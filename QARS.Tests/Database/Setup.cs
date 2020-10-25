using Bogus;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using QARS.Data.Models;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QARS.Tests.Database
{
	/// <summary>
	/// Contains unit tests that populate the database with data.
	/// </summary>
	[TestClass]
	public partial class Setup : TestBase
	{
		public IDbContextTransaction Transaction { get; private set; }

		[TestInitialize]
		public async Task TestInitialize()
		{
			Transaction = await Context.Database.BeginTransactionAsync();
		}

		[TestCleanup]
		public async Task TestCleanup()
		{
			try
			{
				Context.SaveChanges();
			}
			finally
			{
				// If the transaction is still open, roll it back
				if (Transaction == Context.Database.CurrentTransaction)
					await Transaction.RollbackAsync();
				await Transaction.DisposeAsync();

				// Reload all tracked entities since the transaction may have affected them
				Context.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
			}
		}

		[TestMethod]
		public async Task ExportTestDatabase()
		{
			await new UserDataSourceAttribute(10).AddToContextAsync(Context);
			await PopulateCarExtras();

			await Transaction.CommitAsync();
		}

		[TestMethod]
		[Description("Inserts a batch of Users into the database.")]
		[UserDataSource(10, true)]
		public async Task PopulateUsers(object[] entities) => await Context.AddRangeAsync(entities);

		[TestMethod]
		[Description("Inserts a batch of Cars into the database.")]
		[CarDataSource(20, true)]
		public async Task PopulateCars(object[] entities) => await Context.AddRangeAsync(entities);

		[TestMethod]
		[Description("Inserts a batch of Extras into the database.")]
		[ExtraDataSource(50, true)]
		public async Task PopulateExtras(object[] entities) => await Context.AddRangeAsync(entities);

		[TestMethod]
		[Description("Inserts cars and extrans into the database and then inserts a random combination of CarExtras into the database.")]
		public async Task PopulateCarExtras()
		{
			Task<IEnumerable<object>> addCarsTask = new CarDataSourceAttribute(20).AddToContextAsync(Context);
			Task<IEnumerable<object>> addExtrasTask = new ExtraDataSourceAttribute(50).AddToContextAsync(Context);

			// Set up the randomizer with a predefined seed
			var r = new Randomizer(58982990);
			var carExtras = new List<CarExtra>();

			var cars = (await addCarsTask).Cast<Car>().ToList();
			var extras = (await addExtrasTask).Cast<Extra>().ToList();
			
			// Loop through all cars
			foreach	(Car car in cars)
			{
				// Determine how many extras should be assigned to the car
				int extrasAmount = r.Number(3);

				// Shuffle the extras list, take the first few elements and pack them into CarExtra objects and add them to the carExtras list
				carExtras.AddRange(r.Shuffle(extras).Take(extrasAmount).Select(extra => new CarExtra() { Car = car, Extra = extra }));
			}

			// Insert the carExtras
			await Context.AddRangeAsync(carExtras); 
		}
	}
}
