using Bogus;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using QARS.Data.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace QARS.Tests.Database
{
	/// <summary>
	/// Generates <see cref="User"/> objects for unit tests.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public class UserDataSourceAttribute : Attribute, ITestDataSource
	{
		/// <summary>
		/// The predefined seed that generates the random entries.
		/// </summary>
		public const int Seed = 765597632;

		/// <summary>
		/// Gets the amount of <see cref="User"/>s that <see cref="GetData(MethodInfo)"/> will generate.
		/// </summary>
		public int Amount { get; }

		/// <summary>
		/// Initializes a new instance of <see cref="UserDataSourceAttribute"/> with the specified <paramref name="amount"/>.
		/// </summary>
		/// <param name="amount">The amount of entries to generate.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="amount"/> is less than 1.</exception>
		public UserDataSourceAttribute(int amount = 10)
		{
			if (amount < 1)
				throw new ArgumentOutOfRangeException("Amount may not be less than 1.");

			Amount = amount;
		}

		public IEnumerable<object[]> GetData(MethodInfo methodInfo)
		{
			// Create a new faker that generates random users
			Faker<User> userFaker = new Faker<User>()
				.UseSeed(Seed)
				.RuleFor(o => o.FullName, f => f.Name.FullName())
				.RuleFor(o => o.UserName, (f, o) => {
					string[] fullName = o.FullName.Split(" ", 2);
					return f.Internet.UserNameUnicode(fullName[0], fullName[1]);
				})
				.RuleFor(o => o.Email, (f, o) => {
					string[] fullName = o.FullName.Split(" ", 2);
					return f.Internet.Email(fullName[0], fullName[1]);
				})
				.RuleFor(o => o.Age, f => f.Random.Number(18, 65));

			// Pack the generated users into arrays
			return from user in userFaker.GenerateLazy(Amount)
				   select new[] { user };
		}

		public string GetDisplayName(MethodInfo methodInfo, object[] data) => $"{methodInfo.Name} : {data.Single() as User}";
	}
}
