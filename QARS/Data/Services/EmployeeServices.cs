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
	public class EmployeeServices
	{
		private AppDbContext dbContext;

		public EmployeeServices(AppDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task<List<Employee>> GetEmployeesAsync()
		{
			return await dbContext.Employees.ToListAsync();
		}

		public async Task<Employee> AddEmployeeAsync(Employee employee)
		{
			try
			{
				dbContext.Employees.Add(employee);
				await dbContext.SaveChangesAsync();
			}
			catch (Exception)
			{
				throw;
			}
			return employee;
		}

		public async Task<Employee> UpdateEmployeeAsync(Employee employee)
		{
			try
			{
				var employeeExist = dbContext.Employees.FirstOrDefault(p => p.Id == employee.Id);
				if (employeeExist != null)
				{
					dbContext.Update(employee);
					await dbContext.SaveChangesAsync();
				}
			}
			catch (Exception)
			{
				throw;
			}
			return employee;
		}

		public async Task DeleteEmployeeAsync(Employee employee)
		{
			try
			{
				dbContext.Employees.Remove(employee);
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
