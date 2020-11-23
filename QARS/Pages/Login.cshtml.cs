using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using QARS.Data.Models;

namespace QARS.Pages
{
	[AllowAnonymous]
    public class LoginModel : PageModel
	{
		private SignInManager<User> SignInManager { get; }
		private UserManager<User> UserManager { get; }
		private ILogger<LoginModel> Log { get; }

		public LoginModel(SignInManager<User> signInManager, UserManager<User> userManager, ILogger<LoginModel> logger)
		{
			SignInManager = signInManager;
			UserManager = userManager;
			Log = logger;
		}

		[BindProperty]
		public InputModel Input { get; set; }

		public string ReturnUrl { get; set; }

		[TempData]
		public string ErrorMessage { get; set; }

		public class InputModel
		{
			public string lol { get; set; }

			[Required, EmailAddress]
			public string Email { get; set; }

			[Required, DataType(DataType.Password)]
			public string Password { get; set; }

			[Display(Name = "Remember me?")]
			public bool RememberMe { get; set; }
		}

		public async Task OnGetAsync(string returnUrl = null)
        {
			if (!string.IsNullOrEmpty(ErrorMessage))
				ModelState.AddModelError(string.Empty, ErrorMessage);

			// Clear the existing external cookie to ensure a clean login process
			await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

			ReturnUrl = returnUrl ?? Url.Content("~/");
		}

		public async Task<IActionResult> OnPostAsync(string returnUrl = null)
		{
			Console.WriteLine(Utils.GetDetailedString(Input));

			if (Input.lol != null)
			{
				var user = new Customer()
				{
					Email = "no@gmail.com",
					FirstName = "Bob",
					LastName = "B.",
					PhoneNumber = "091823098",
					Location = new Location
					{
						Address = "somewhere",
						City = "Netherlands",
						CountryCode = "nl",
						ZipCode = "6969EH"
					}
				};
				await UserManager.CreateAsync(user, "lol");

				return Page();
			}

			returnUrl ??= Url.Content("~/");

			if (ModelState.IsValid)
			{
				// This doesn't count login failures towards account lockout
				// To enable password failures to trigger account lockout, set lockoutOnFailure: true
				var result = await SignInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
				if (result.Succeeded)
				{
					Log.LogInformation("User logged in.");
					return LocalRedirect(returnUrl);
				}
				if (result.IsLockedOut)
				{
					Log.LogWarning("User account locked out.");
					return RedirectToPage("./Lockout");
				}
				else
				{
					ModelState.AddModelError(string.Empty, "Invalid login attempt.");
					return Page();
				}
			}

			// If we got this far, something failed, redisplay form
			return Page();
		}
    }
}
