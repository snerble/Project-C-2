using Bogus;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using QARS.Data.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace QARS.Tests.Database
{
	public class ExtraDataSourceAttribute : Attribute, ITestDataSource
	{
		/// <summary>
		/// The predefined seed that generates the random entries.
		/// </summary>
		public const int Seed = 1881166170;

		/// <summary>
		/// Gets the amount of entries that <see cref="GetData(MethodInfo)"/> will generate.
		/// </summary>
		public int Amount { get; }

		/// <summary>
		/// Initializes a new instance of <see cref="ExtraDataSourceAttribute"/> with the specified <paramref name="amount"/>.
		/// </summary>
		/// <param name="amount">The amount of entries to generate.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="amount"/> is less than 1.</exception>
		public ExtraDataSourceAttribute(int amount = 10)
		{
			if (amount < 1)
				throw new ArgumentOutOfRangeException("Amount may not be less than 1.");

			Amount = amount;
		}

		public IEnumerable<object[]> GetData(MethodInfo methodInfo)
		{
			// Create a new faker that generates random extras
			Faker<Extra> faker = new Faker<Extra>()
				.UseSeed(Seed)
				.RuleFor(e => e.Name, f => $"{f.Commerce.ProductMaterial()} {f.Commerce.Product()}")
				.RuleFor(e => e.Price, f => decimal.Parse(f.Commerce.Price(10, 80, 0)) - 0.01m);

			// Pack the generated extras into arrays
			return from extra in faker.GenerateLazy(Amount)
				   select new[] { extra };
		}

		public string GetDisplayName(MethodInfo methodInfo, object[] data) => $"{methodInfo.Name} : {data.Single() as Extra}";
	}
}
