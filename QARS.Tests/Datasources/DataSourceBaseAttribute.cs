using Bogus;

using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace QARS.Tests.Datasources
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public abstract class DataSourceBaseAttribute : Attribute, ITestDataSource
	{
		/// <summary>
		/// Gets whether or not this attribute returns all of it's data in a single list.
		/// </summary>
		public bool Batch { get; }

		/// <summary>
		/// Gets the seed used by this datasource attribute.
		/// </summary>
		protected abstract int Seed { get; }

		private readonly int amount;

		/// <summary>
		/// Initializes a data source attribute with a specified <paramref name="amount"/>.
		/// </summary>
		/// <param name="amount">The amount of data that should be generated.</param>
		/// <param name="batch">If <see langword="true"/>, returns the generated data in a single list. Otherwise the data is
		/// spread across multiple function calls.</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="amount"/> is less than 1.</exception>
		public DataSourceBaseAttribute(int amount = 10, bool batch = false)
		{
			if (amount < 1)
				throw new ArgumentOutOfRangeException("Amount may not be less than 1.");

			this.amount = amount;
			Batch = batch;
		}

		/// <summary>
		/// Adds the data from this data source to the given <see cref="DbContext"/>.
		/// </summary>
		/// <returns>The entities that have been added to the <paramref name="context"/>.</returns>
		public IEnumerable<object> AddToContext(DbContext context)
		{
			IEnumerable<object> entities = GetData(amount);
			context.AddRange(entities);
			return entities;
		}
		/// <inheritdoc cref="AddToContext(DbContext)"/>
		public async Task<IEnumerable<object>> AddToContextAsync(DbContext context)
		{
			IEnumerable<object> entities = GetData(amount);
			await context.AddRangeAsync(GetData(amount));
			return entities;
		}

		public IEnumerable<object[]> GetData(MethodInfo methodInfo)
		{
			// Apply the seed before getting the data
			Randomizer.Seed = new Random(Seed);
			IEnumerable<object> data = GetData(amount);

			// Pack the values in a list if batch is true, otherwise yield the individual elements
			if (Batch)
				yield return new[] { data.ToArray() };
			else
				foreach (object o in data)
					yield return new[] { o };
		}
		
		/// <inheritdoc cref="ITestDataSource.GetData(MethodInfo)"/>
		/// <param name="amount">The amount of elements that should be generated.</param>
		protected abstract IEnumerable<object> GetData(int amount);

		public string GetDisplayName(MethodInfo methodInfo, object[] data) => Batch
				? $"{methodInfo.Name} ({(data.Single() as object[]).First().GetType().Name}[{(data.Single() as object[]).Length}])"
				: $"{methodInfo.Name} ({data.Single()})";
	}
}
