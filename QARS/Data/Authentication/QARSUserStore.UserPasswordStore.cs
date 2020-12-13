using Microsoft.AspNetCore.Identity;

using QARS.Data.Models;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace QARS.Data.Authentication
{
	public partial class QARSUserStore : IUserPasswordStore<User>
	{
		public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
			=> Task.FromResult(Convert.ToBase64String(user.Password));

		public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
			=> Task.FromResult(user.Password != null && user.Password.Length > 0);

		public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
		{
			// passwordHash is a base64 string, so convert it to bytes and set password
			user.Password = Convert.FromBase64String(passwordHash);
			return Task.CompletedTask;
		}
	}
}
