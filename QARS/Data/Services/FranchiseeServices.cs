using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using QARS.Data.Models;
using Blazored.Modal;
using Blazored.Modal.Services;

namespace QARS.Data.Services
{
	public class FranchiseeServices
	{
		private AppDbContext dbContext;

		public FranchiseeServices(AppDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task<List<Franchisee>> GetFranchiseeAsync()
		{
			return await dbContext.Franchisees.ToListAsync();
		}

		public async Task<Franchisee> AddFranchiseeAsync(Franchisee franchisee)
		{
			try
			{
				dbContext.Franchisees.Add(franchisee);
				await dbContext.SaveChangesAsync();
			}
			catch (Exception)
			{
				throw;
			}
			return franchisee;
		}

		public async Task<Franchisee> UpdateFranchiseeAsync(Franchisee franchisee)
		{
			try
			{
				var franchiseeExist = dbContext.Franchisees.FirstOrDefault(p => p.Id == franchisee.Id);
				if (franchiseeExist != null)
				{
					dbContext.Update(franchisee);
					await dbContext.SaveChangesAsync();
				}
			}
			catch (Exception)
			{
				throw;
			}
			return franchisee;
		}

		public async Task DeleteFranchiseeAsync(Franchisee franchisee)
		{
			try
			{
				dbContext.Franchisees.Remove(franchisee);
				await dbContext.SaveChangesAsync();
			}
			catch (Exception)
			{
				throw;
			}
		}
		public async Task CloseModal(BlazoredModalInstance blazoredmodal)
		{

			await blazoredmodal.Close(ModalResult.Ok(true));
		}
	}
}
