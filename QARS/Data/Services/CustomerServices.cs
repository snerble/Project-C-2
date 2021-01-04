using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using QARS.Data.Models;
using Blazored.Modal;
using Blazored.Modal.Services;

namespace QARS.Data.Services
{
	public class CustomerServices
	{
		private AppDbContext dbContext;

		public CustomerServices(AppDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task<Customer> UpdateCustomerAsync(Customer customer)
		{
			try
			{
				var customerExist = dbContext.Customers.FirstOrDefault(p => p.Id == customer.Id);
				if (customerExist != null)
				{
					dbContext.Update(customer);
					await dbContext.SaveChangesAsync();
				}
			}
			catch (Exception)
			{
				throw;
			}
			return customer;
		}
	}
}
