using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

using QARS.Areas.Identity.Pages.Account;
using QARS.Data.Models;
using QARS.Data.Services;

using System.Threading.Tasks;
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.Modal.Services;

namespace QARS.Shared
{
	public partial class AddEmployee
	{
		[Inject] private UserManager<User> UserManager { get; set; }
		[Inject] private ILogger<AddEmployee> Logger { get; set; }
		[Inject] private EmailManager EmailManager { get; set; }


		public class InputModel : RegisterModel.InputModel
		{
			[Required]
			[Display(Name = "Franchisee Id")]
			public int FranchiseeId { get; set; }

			[Required]
			[Display(Name = "Store Id")]
			public int StoreId { get; set; }

		}

		public InputModel Input { get; set; } = new InputModel();

		[CascadingParameter]
		private Task<AuthenticationState> authenticationStateTask { get; set; }

		public async Task CreateNewEmployee()
		{

			var authState = await authenticationStateTask;
			int UserId = 0;

			if (authState.User.Identity.IsAuthenticated)
			{
				var User = await UserManager.GetUserAsync(authState.User);
				UserId = User.Id;
			}

				var user = new Employee
			{
				FirstName = Input.FirstName,
				LastName = Input.LastName,
				Email = Input.Email,
				PhoneNumber = Input.PhoneNumber,
				Location = Input.Location,
				FranchiseeId = UserId,
				StoreId = Input.StoreId
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
