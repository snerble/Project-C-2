using Bogus;

using QARS.Data.Models;
using QARS.Tests.Datasources;

using System.Collections.Generic;
using System.Reflection;

namespace QARS.Tests.Database
{
	public class ExtraDataSourceAttribute : DataSourceBaseAttribute
	{
		/// <inheritdoc cref="DataSourceBaseAttribute(int, bool)"/>
		public ExtraDataSourceAttribute(int amount = 10, bool batch = false) : base(amount, batch) { }

		protected override int Seed => 1881166170;

		protected override IEnumerable<object> GetData(int amount)
		{
			// Create a new faker that generates random extras
			Faker<Extra> faker = new Faker<Extra>()
				.UseSeed(Seed)
				.RuleFor(e => e.Name, f => $"{f.Commerce.ProductMaterial()} {f.Commerce.Product()}")
				.RuleFor(e => e.Price, f => decimal.Parse(f.Commerce.Price(10, 80, 0)) - 0.01m);

			// Return the generator for the data
			return faker.GenerateLazy(amount);
		}
	}
}
