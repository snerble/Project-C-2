using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using QARS.Data.Models;
using QARS.Data.Services;

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace QARS.Areas.Identity.Pages.Account
{
	[AllowAnonymous]
    public class ResendEmailModel : PageModel
    {
        private readonly UserManager<User> _userManager;
		private readonly EmailManager _emailManager;

		public ResendEmailModel(
			UserManager<User> userManager,
			EmailManager emailManager)
        {
            _userManager = userManager;
			_emailManager = emailManager;
		}

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        [TempData]
        public string StatusMessage { get; set; }

		public IActionResult OnGet()
		{
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
				User user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || await _userManager.IsEmailConfirmedAsync(user))
                {
                    // Don't reveal that the user does not exist or is already confirmed
                    return RedirectToPage("./ResendEmailConfirmation");
                }

                var b = new UriBuilder
                {
                    Scheme = Request.Scheme,
                    Host = Request.Host.Host
                };
                if (Request.Host.Port.HasValue)
                    b.Port = Request.Host.Port.Value;

                await _emailManager.SendConfirmationEmailAsync(user, b.ToString());

                return RedirectToPage("./ResendEmailConfirmation");
            }

            return Page();
        }
    }
}
