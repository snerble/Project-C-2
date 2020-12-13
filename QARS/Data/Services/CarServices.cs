using Microsoft.EntityFrameworkCore;

using QARS.Data.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QARS.Data.Services
{
	public class CarServices
	{
		private AppDbContext dbContext;

		public CarServices(AppDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task<List<Car>> GetCarAsync()
		{
			return await dbContext.Cars
				.Include(c => c.Location)
				.Include(c => c.Store)
				.Include(c => c.Model)
				.ToListAsync();
		}

		public async Task<Car> AddCarAsync(Car car)
		{
			try
			{
				dbContext.Cars.Add(car);
				await dbContext.SaveChangesAsync();
			}
			catch (Exception)
			{
				throw;
			}
			return car;
		}

		public async Task<Car> UpdateCarAsync(Car car)
		{
			try
			{
				var carExist = dbContext.CarModels.FirstOrDefault(p => p.Id == car.Id);
				if (carExist != null)
				{
					dbContext.Update(car);
					await dbContext.SaveChangesAsync();
				}
			}
			catch (Exception)
			{
				throw;
			}
			return car;
		}

		public async Task DeleteCarAsync(Car car)
		{
			try
			{
				dbContext.Cars.Remove(car);
				await dbContext.SaveChangesAsync();
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
