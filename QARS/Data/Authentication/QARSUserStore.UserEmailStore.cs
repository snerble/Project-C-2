using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using QARS.Data.Models;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace QARS.Data.Authentication
{
	public partial class QARSUserStore : IUserEmailStore<User>
	{
		public async Task<User> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
			=> await Users.FirstOrDefaultAsync(x => x.NormalizedEmail == normalizedEmail, cancellationToken);

		public Task<string> GetEmailAsync(User user, CancellationToken cancellationToken)
			=> Task.FromResult(user.Email);
		public Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken)
			=> Task.FromResult(user.NormalizedEmail);
		public Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancellationToken) => throw new NotImplementedException();

		public Task SetEmailAsync(User user, string email, CancellationToken cancellationToken)
		{
			user.Email = email;
			return Task.CompletedTask;
		}
		public Task SetNormalizedEmailAsync(User user, string normalizedEmail, CancellationToken cancellationToken)
		{
			user.NormalizedEmail = normalizedEmail;
			return Task.CompletedTask;
		}
		public Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken) => throw new NotImplementedException();
	}
}
