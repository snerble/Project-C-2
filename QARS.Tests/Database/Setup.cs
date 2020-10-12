using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Bogus;

using QARS;
using QARS.Data;
using QARS.Data.Models;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Storage;

namespace QARS.Tests.Database
{
	/// <summary>
	/// Contains unit tests that populate the database with data.
	/// </summary>
	[TestClass]
	public partial class Setup : TestBase
	{
		/// <summary>
		/// Saves any changes made to the database.
		/// </summary>
		[TestCleanup]
		public async Task TestCleanup()
		{
			await Context.SaveChangesAsync();
		}

		private async Task Populate(object entity)
		{
			EntityEntry tracker = await Context.AddAsync(entity);

			// Confirm that the entity was added
			Assert.AreEqual(tracker.State, EntityState.Added);
		}

		[TestMethod]
		[Description("Inserts a batch of Users into the database.")]
		[UserDataSource(10)]
		public async Task PopulateUsers(User entity) => await Populate(entity);

		[TestMethod]
		[Description("Inserts a batch of Cars into the database.")]
		[CarDataSource(20)]
		public async Task PopulateCars(Car entity) => await Populate(entity);

		[TestMethod]
		[Description("Inserts a batch of Extras into the database.")]
		[ExtraDataSource(50)]
		public async Task PopulateExtras(Extra entity) => await Populate(entity);
	}
}
