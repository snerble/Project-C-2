using Bogus;

using QARS.Data.Models;
using QARS.Tests.Datasources;

using System.Collections.Generic;
using System.Reflection;

namespace QARS.Tests.Database
{
	/// <summary>
	/// Generates <see cref="User"/> objects for unit tests.
	/// </summary>
	public class UserDataSourceAttribute : DataSourceBaseAttribute
	{
		/// <inheritdoc cref="DataSourceBaseAttribute(int, bool)"/>
		public UserDataSourceAttribute(int amount = 10, bool batch = false) : base(amount, batch) { }

		protected override int Seed => 765597632;

		protected override IEnumerable<object> GetData(int amount)
		{
			// Create a new faker that generates random users
			Faker<User> userFaker = new Faker<User>()
				.RuleFor(o => o.FullName, f => f.Name.FullName())
				.RuleFor(o => o.UserName, (f, o) =>
				{
					string[] fullName = o.FullName.Split(" ", 2);
					return f.Internet.UserNameUnicode(fullName[0], fullName[1]);
				})
				.RuleFor(o => o.Email, (f, o) =>
				{
					string[] fullName = o.FullName.Split(" ", 2);
					return f.Internet.Email(fullName[0], fullName[1]);
				})
				.RuleFor(o => o.Age, f => f.Random.Number(18, 65));

			// Return the generator for the data
			return userFaker.GenerateLazy(amount);
		}
	}
}
