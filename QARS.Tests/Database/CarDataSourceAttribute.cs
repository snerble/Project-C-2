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
	public class CarDataSourceAttribute : Attribute, ITestDataSource
	{
		/// <summary>
		/// The predefined seed that generates the random entries.
		/// </summary>
		public const int Seed = 1237415836;

		/// <summary>
		/// Gets the amount of entries that <see cref="GetData(MethodInfo)"/> will generate.
		/// </summary>
		public int Amount { get; }

		/// <summary>
		/// Initializes a new instance of <see cref="CarDataSourceAttribute"/> with the specified <paramref name="amount"/>.
		/// </summary>
		/// <param name="amount">The amount of entries to generate.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="amount"/> is less than 1.</exception>
		public CarDataSourceAttribute(int amount = 10)
		{
			if (amount < 1)
				throw new ArgumentOutOfRangeException("Amount may not be less than 1.");

			Amount = amount;
		}

		public IEnumerable<object[]> GetData(MethodInfo methodInfo)
		{
			// Create a new faker that generates random cars
			Faker<Car> faker = new Faker<Car>()
				.UseSeed(Seed)
				.RuleFor(c => c.LicensePlate, f => f.Random.Replace("??-###-?"))
				.RuleFor(c => c.Name, f => f.Vehicle.Model())
				.RuleFor(c => c.Manufacturer, f => f.Vehicle.Manufacturer())
				.RuleFor(c => c.Price, f => decimal.Parse(f.Commerce.Price(80, 300, 0)) - 0.01m);

			// Pack the generated cars into arrays
			return from car in faker.GenerateLazy(Amount)
				   select new[] { car };
		}

		public string GetDisplayName(MethodInfo methodInfo, object[] data) => $"{methodInfo.Name} : {data.Single() as Car}";
	}
}
