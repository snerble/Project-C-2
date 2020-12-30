using Blazored.Modal.Services;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

using QARS.Areas.Identity.Pages.Account;
using QARS.Data.Models;
using QARS.Data.Services;

using System.Threading.Tasks;

namespace QARS.Shared
{
	public partial class AddFranchisee
	{
		[Inject] private UserManager<User> UserManager { get; set; }
		[Inject] private ILogger<AddFranchisee> Logger { get; set; }
		[Inject] private EmailManager EmailManager { get; set; }

		public RegisterModel.InputModel Input { get; set; } = new RegisterModel.InputModel();

		public async Task CreateNewFranchisee()
		{
			var user = new Franchisee
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
				Logger.LogInformation("Franchisee created a new account with password.");

				await EmailManager.SendConfirmationEmailAsync(user);
			}

			await BlazoredModal.Close(ModalResult.Ok(true));
		}
	}
}
