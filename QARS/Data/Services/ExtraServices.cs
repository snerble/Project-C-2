using Microsoft.EntityFrameworkCore;

using QARS.Data.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QARS.Data.Services
{
	public class ExtraServices
	{


		private AppDbContext dbContext;

		public ExtraServices(AppDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task<List<Extra>> GetExtraAsync()
		{
			return await dbContext.Extras.ToListAsync();
		}

		public async Task<Extra> AddExtraAsync(Extra extra)
		{
			try
			{
				dbContext.Extras.Add(extra);
				await dbContext.SaveChangesAsync();
			}
			catch (Exception)
			{
				throw;
			}
			return extra;
		}
		public async Task<Extra> UpdateExtraAsync(Extra extra)
		{
			try
			{
				var extraExist = dbContext.Extras.FirstOrDefault(p => p.Id == extra.Id);
				if (extraExist != null)
				{
					dbContext.Update(extra);
					await dbContext.SaveChangesAsync();
				}
			}
			catch (Exception)
			{
				throw;
			}
			return extra;
		}

		public async Task DeleteExtranAsync(Extra extra)
		{
			try
			{
				dbContext.Extras.Remove(extra);
				await dbContext.SaveChangesAsync();
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
