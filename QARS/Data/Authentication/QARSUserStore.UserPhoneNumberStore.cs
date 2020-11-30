using Microsoft.AspNetCore.Identity;

using QARS.Data.Models;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace QARS.Data.Authentication
{
	public partial class QARSUserStore : IUserPhoneNumberStore<User>
	{
		public Task<string> GetPhoneNumberAsync(User user, CancellationToken cancellationToken) => Task.FromResult(user.PhoneNumber);
		public Task<bool> GetPhoneNumberConfirmedAsync(User user, CancellationToken cancellationToken) => throw new NotImplementedException();
		public Task SetPhoneNumberAsync(User user, string phoneNumber, CancellationToken cancellationToken)
		{
			user.PhoneNumber = phoneNumber;
			return Task.CompletedTask;
		}
		public Task SetPhoneNumberConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken) => throw new NotImplementedException();
	}
}
