using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using QARS.Data.Models;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QARS.Data.Services
{
	public class ReturnsServices
	{
		private readonly AppDbContext _dbContext;
		private readonly ILogger<ReturnsServices> _logger;

		public ReturnsServices(
			AppDbContext dbContext,
			ILogger<ReturnsServices> logger)
		{
			_dbContext = dbContext;
			_logger = logger;
		}

		public async Task<List<Return>> GetReturnAsync() => await _dbContext.Returns.ToListAsync();

		public async Task AddReturnAsync(Return returns)
		{
			try
			{
				_dbContext.Add(returns);
				await _dbContext.SaveChangesAsync();
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Error while adding reservation return.");
			}
		}

		public async Task UpdateReturnAsync(Return returns)
		{
			try
			{
				if (await _dbContext.Returns.FirstOrDefaultAsync(p => p.Id == returns.Id) is { })
				{
					_dbContext.Update(returns);
					await _dbContext.SaveChangesAsync();
				}
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Error while updating reservation return.");
			}
		}

		public async Task DeleteReturnAsync(Return returns)
		{
			try
			{
				_dbContext.Remove(returns);
				await _dbContext.SaveChangesAsync();
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Error while deleting reservation return.");
			}
		}
	}
}
