using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using QARS.Data.Models;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QARS.Data.Authentication
{
	public partial class QARSUserStore : IUserRoleStore<User>
	{
		public async Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken)
		{
			List<UserRole> roles = user.Roles ?? new List<UserRole>();

			roles.Add(new UserRole
			{
				User = user,
				Role = await RoleManager.FindByNameAsync(roleName)
			});

			user.Roles = roles;
		}

		public async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken)
		{
			return (await Users
				.Include(u => u.Roles)
				.ThenInclude(ur => ur.Role)
				.FirstOrDefaultAsync(u => u.Id == user.Id, cancellationToken)
			)?.Roles.Select(ur => ur.Role.Name).ToList() ?? new List<string>();
		}

		public async Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
		{
			return await Users
				.Include(u => u.Roles)
				.ThenInclude(ur => ur.Role)
				.Where(u => u.Roles.Any(ur => ur.Role.Name == roleName))
				.ToListAsync(cancellationToken);
		}

		public async Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken)
		{
			return (await Users
				.Include(u => u.Roles)
				.ThenInclude(ur => ur.Role)
				.FirstOrDefaultAsync(u => u.Id == user.Id, cancellationToken)
			)?.Roles.Any(ur => ur.Role.Name == roleName) ?? false;
		}

		public async Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken)
		{
			// Get the specified user while including the foreign keys so we can find the correct role
			UserRole userRole = (await Users
				.Include(u => u.Roles)
				.ThenInclude(ur => ur.Role)
				.FirstOrDefaultAsync(u => u.Id == user.Id, cancellationToken)
			)?.Roles.FirstOrDefault(ur => ur.Role.Name == roleName);

			if (userRole is null)
				return;

			DbContext.Remove(userRole);
			await DbContext.SaveChangesAsync(cancellationToken);
		}
	}
}
