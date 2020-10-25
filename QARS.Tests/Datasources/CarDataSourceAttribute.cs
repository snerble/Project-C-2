using Bogus;

using QARS.Data.Models;
using QARS.Tests.Datasources;

using System.Collections.Generic;
using System.Reflection;

namespace QARS.Tests
{
	public class CarDataSourceAttribute : DataSourceBaseAttribute
	{
		/// <inheritdoc cref="DataSourceBaseAttribute(int, bool)"/>
		public CarDataSourceAttribute(int amount = 10, bool batch = false) : base(amount, batch) { }

		protected override int Seed => 1237415836;

		protected override IEnumerable<object> GetData(int amount)
		{
			// Create a new faker that generates random cars
			Faker<Car> faker = new Faker<Car>()
				.UseSeed(Seed)
				.RuleFor(c => c.LicensePlate, f => f.Random.Replace("??-###-?"))
				.RuleFor(c => c.Name, f => f.Vehicle.Model())
				.RuleFor(c => c.Manufacturer, f => f.Vehicle.Manufacturer())
				.RuleFor(c => c.Price, f => decimal.Parse(f.Commerce.Price(80, 300, 0)) - 0.01m);

			// Return the generator object for the data
			return faker.GenerateLazy(amount);
		}
	}
}
