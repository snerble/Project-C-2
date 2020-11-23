using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using QARS.Data.Models;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QARS.Data.Authentication
{
	public class QARSRoleStore : IRoleStore<Role>, IQueryableRoleStore<Role>
	{
		/// <summary>
		/// Initializes a new instance of <see cref="QARSRoleStore"/> with the given database
		/// context.
		/// </summary>
		/// <param name="dbContext">The database context used to obtain <see cref="Role"/>
		/// instances.</param>
		public QARSRoleStore(AppDbContext dbContext) => DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

		public IQueryable<Role> Roles => DbContext.Roles;

		private AppDbContext DbContext { get; }

		public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken)
		{
			EntityEntry<Role> entry = await DbContext.AddAsync(role, cancellationToken);
			await DbContext.SaveChangesAsync(cancellationToken);
			if (entry.State == EntityState.Added)
				return IdentityResult.Success;
			return IdentityResult.Failed();
		}

		public async Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken)
		{
			int.TryParse(roleId, out int id);
			return await DbContext.Roles.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
		}

		public async Task<Role> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
			=> await DbContext.Roles.FirstOrDefaultAsync(x => x.NormalizedName == normalizedRoleName, cancellationToken);

		public Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken)
			=> Task.FromResult(role.Id.ToString());

		public Task<string> GetRoleNameAsync(Role role, CancellationToken cancellationToken)
			=> Task.FromResult(role.Name);

		public Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken)
			=> Task.FromResult(role.NormalizedName);

		public Task SetRoleNameAsync(Role role, string roleName, CancellationToken cancellationToken)
		{
			role.Name = roleName;
			return Task.CompletedTask;
		}

		public Task SetNormalizedRoleNameAsync(Role role, string normalizedName, CancellationToken cancellationToken)
		{
			role.NormalizedName = normalizedName;
			return Task.CompletedTask;
		}

		public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken)
		{
			EntityEntry<Role> entry = DbContext.Update(role);
			await DbContext.SaveChangesAsync(cancellationToken);
			if (entry.State == EntityState.Detached)
				return IdentityResult.Failed();
			return IdentityResult.Success;
		}

		public async Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken)
		{
			EntityEntry<Role> entry = DbContext.Remove(role);
			await DbContext.SaveChangesAsync(cancellationToken);
			if (entry.State == EntityState.Deleted)
				return IdentityResult.Success;
			return IdentityResult.Failed();
		}

		public void Dispose()
		{
			DbContext.Dispose();
		}
	}
}
