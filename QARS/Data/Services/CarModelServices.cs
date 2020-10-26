using Microsoft.EntityFrameworkCore;

using QARS.Data.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QARS.Data.Services
{
	public class CarModelServices
	{
		private AppDbContext dbContext;

		public CarModelServices(AppDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task<List<CarModel>> GetCarModelAsync()
		{
			return await dbContext.CarModels.ToListAsync();
		}

		public async Task<CarModel> AddCarModelAsync(CarModel carmodel)
		{
			try
			{
				dbContext.CarModels.Add(carmodel);
				await dbContext.SaveChangesAsync();
			}
			catch (Exception)
			{
				throw;
			}
			return carmodel;
		}

		public async Task<CarModel> UpdateCarModelAsync(CarModel carmodel)
		{
			try
			{
				var carmodelExist = dbContext.CarModels.FirstOrDefault(p => p.Id == carmodel.Id);
				if (carmodelExist != null)
				{
					dbContext.Update(carmodel);
					await dbContext.SaveChangesAsync();
				}
			}
			catch (Exception)
			{
				throw;
			}
			return carmodel;
		}

		public async Task DeleteCarModelAsync(CarModel carmodel)
		{
			try
			{
				dbContext.CarModels.Remove(carmodel);
				await dbContext.SaveChangesAsync();
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
