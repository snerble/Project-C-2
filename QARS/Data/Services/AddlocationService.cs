using Microsoft.EntityFrameworkCore;

using QARS.Data.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QARS.Data.Services
{
	public class AddlocationServices
	{


		private AppDbContext dbContext;

		public AddlocationServices(AppDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task<List<Location>> GetLocationAsync()
		{
			return await dbContext.Locations.ToListAsync();
		}

		public async Task<Location> AddLocationsAsync(Location location)
		{
			try
			{
				dbContext.Locations.Add(location);
				await dbContext.SaveChangesAsync();
			}
			catch (Exception)
			{
				throw;
			}
			return location;
		}
		public async Task<Location> UpdatelocationAsync(Location location)
		{
			try
			{
				var locationExist = dbContext.Locations.FirstOrDefault(p => p.Id == location.Id);
				if (locationExist != null)
				{
					dbContext.Update(location);
					await dbContext.SaveChangesAsync();
				}
			}
			catch (Exception)
			{
				throw;
			}
			return location;
		}

		public async Task DeleteLocationAsync(Location location)
		{
			try
			{
				dbContext.Locations.Remove(location);
				await dbContext.SaveChangesAsync();
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
