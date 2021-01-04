using Microsoft.EntityFrameworkCore;
using QARS.Data;
using QARS.Data.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace QARS.Data.Services
{
	public class ReturnsServices
	{
		private AppDbContext dbContext;

		public ReturnsServices(AppDbContext dbcontext)
		{
			this.dbContext = dbcontext;
		}

		public async Task<List<Return>> getReturnasync()
		{
			return await dbContext.Returns.ToListAsync();
		}

		public async Task<Return> addreturnasync(Return returns)
		{
			try
			{
				dbContext.Returns.Add(returns);
				await dbContext.SaveChangesAsync();
			}
			catch (Exception)
			{
				throw;
			}
			return returns;
		}

		public async Task<Return> updatereturnasync(Return returns)
		{
			try
			{
				var returnexist = dbContext.Returns.FirstOrDefault(p => p.Id == returns.Id);
				if (returnexist != null)
				{
					dbContext.Update(returns);
					await dbContext.SaveChangesAsync();
				}
			}
			catch (Exception)
			{
				throw;
			}
			return returns;
		}

		public async Task Deletereturnssasync(Return returns)
		{
			try
			{
				dbContext.Returns.Remove(returns);
				await dbContext.SaveChangesAsync();
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
