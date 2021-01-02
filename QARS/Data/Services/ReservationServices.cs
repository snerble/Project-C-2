using Microsoft.EntityFrameworkCore;

using QARS.Data.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QARS.Data.Services
{
	public class ReservationServices
	{
		private AppDbContext dbContext;

		public ReservationServices(AppDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task<List<Reservation>> GetReservationAsync(int userid, string role)
		{
			if (role == "Customer")
			{
				return await dbContext.Reservations
					.Where(r => r.Customer.Id == userid)
					.ToListAsync();
			}
			else if (role == "Franchisee")
			{
				return await dbContext.Reservations
					.Where(r => r.Car.Store.Franchisee.Id == userid)
					.ToListAsync();
			}
			else
			{
				return await dbContext.Reservations
					.Where(r => r.Car.Store.Id == userid)
					.ToListAsync();
			}
		}

		public async Task<Reservation> AddReservationAsync(Reservation reservation)
		{
			try
			{
				dbContext.Reservations.Add(reservation);
				await dbContext.SaveChangesAsync();
			}
			catch (Exception)
			{
				throw;
			}
			return reservation;
		}

		public async Task DeleteReservationAsync(Reservation reservation)
		{
			try
			{
				dbContext.Reservations.Remove(reservation);
				await dbContext.SaveChangesAsync();
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
