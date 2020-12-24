using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

using QARS.Areas.Identity.Pages.Account;
using QARS.Data.Models;
using QARS.Data.Services;

using System.Threading.Tasks;

namespace QARS.Shared
{
	public partial class AddEmployee
	{
		[Inject] private UserManager<User> UserManager { get; set; }
		[Inject] private ILogger<AddEmployee> Logger { get; set; }
		[Inject] private EmailManager EmailManager { get; set; }

		public RegisterModel.InputModel Input { get; set; } = new RegisterModel.InputModel();

		public async Task CreateNewEmployee()
		{
			var user = new Employee
			{
				FirstName = Input.FirstName,
				LastName = Input.LastName,
				Email = Input.Email,
				PhoneNumber = Input.PhoneNumber,
				Location = Input.Location
			};
			IdentityResult result = await UserManager.CreateAsync(user, Input.Password);
			if (result.Succeeded)
			{
				Logger.LogInformation("Employee created a new account with password.");

				await EmailManager.SendConfirmationEmailAsync(user);
			}
		}
	}
}
