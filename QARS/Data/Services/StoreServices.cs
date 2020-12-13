using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using QARS.Data.Models;

namespace QARS.Data.Services
{
	public class StoreServices
	{
		private AppDbContext dbContext;

		public StoreServices(AppDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task<List<Store>> GetStoreAsync()
		{
			return await dbContext.Stores
				.Include(s => s.Location)
				.Include(s => s.Franchisee)
				.ToListAsync();
		}

		public async Task<Store> AddStoreAsync(Store store)
		{
			try
			{
				dbContext.Stores.Add(store);
				await dbContext.SaveChangesAsync();
			}
			catch (Exception)
			{
				throw;
			}
			return store;
		}

		public async Task<Store> UpdateStoreAsync(Store store)
		{
			try
			{
				var storeExist = dbContext.Stores.FirstOrDefault(p => p.Id == store.Id);
				if (storeExist != null)
				{
					dbContext.Update(store);
					await dbContext.SaveChangesAsync();
				}
			}
			catch (Exception)
			{
				throw;
			}
			return store;
		}

		public async Task DeleteStoreAsync(Store store)
		{
			try
			{
				dbContext.Stores.Remove(store);
				await dbContext.SaveChangesAsync();
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
