using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

using QARS.Data.Models;
using QARS.Data.Services;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace QARS.Areas.Identity.Pages.Account
{
	[AllowAnonymous]
	public class RegisterModel : PageModel
	{
		private readonly SignInManager<User> _signInManager;
		private readonly UserManager<User> _userManager;
		private readonly ILogger<RegisterModel> _logger;
		private readonly EmailManager _emailManager;

		public RegisterModel(
			UserManager<User> userManager,
			SignInManager<User> signInManager,
			ILogger<RegisterModel> logger,
			EmailManager emailManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_logger = logger;
			_emailManager = emailManager;
		}

		[BindProperty]
		public InputModel Input { get; set; }

		public string ReturnUrl { get; set; }

		public IList<AuthenticationScheme> ExternalLogins { get; set; }

		public class InputModel
		{
			[Required]
			[Display(Name = "First name")]
			[StringLength(250, ErrorMessage = "The {0} may not be longer than {1} characters.")]
			public string FirstName { get; set; }

			[Required]
			[Display(Name = "Last name")]
			[StringLength(250, ErrorMessage = "The {0} may not be longer than {1} characters.")]
			public string LastName { get; set; }

			[Required]
			[EmailAddress]
			[Display(Name = "Email")]
			public string Email { get; set; }

			[Required]
			[Phone]
			[Display(Name = "Phone number")]
			public string PhoneNumber { get; set; }

			[Required]
			[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
			[DataType(DataType.Password)]
			[Display(Name = "Password")]
			public string Password { get; set; }

			[DataType(DataType.Password)]
			[Display(Name = "Confirm password")]
			[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
			public string ConfirmPassword { get; set; }

			public Location Location { get; set; }
		}

		public async Task OnGetAsync(string returnUrl = null)
		{
			ReturnUrl = returnUrl;
			ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
		}

		public async Task<IActionResult> OnPostAsync(string returnUrl = null)
		{
			returnUrl ??= Url.Content("~/");
			ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
			if (ModelState.IsValid)
			{
				var user = new Customer
				{
					FirstName = Input.FirstName,
					LastName = Input.LastName,
					Email = Input.Email,
					PhoneNumber = Input.PhoneNumber,
					Location = Input.Location
				};
				IdentityResult result = await _userManager.CreateAsync(user, Input.Password);
				if (result.Succeeded)
				{
					_logger.LogInformation("User created a new account with password.");

					await _emailManager.SendConfirmationEmailAsync(user);

					if (_userManager.Options.SignIn.RequireConfirmedAccount)
						return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl });
					else
					{
						await _signInManager.SignInAsync(user, isPersistent: false);
						return LocalRedirect(returnUrl);
					}
				}
				foreach (IdentityError error in result.Errors)
				{
					switch (error.Code)
					{
						case "DuplicateUserName": // ignore since usernames are not used
							continue;
						default:
							ModelState.AddModelError(string.Empty, error.Description);
							break;
					}
				}
			}

			// If we got this far, something failed, redisplay form
			return Page();
		}
	}
}
