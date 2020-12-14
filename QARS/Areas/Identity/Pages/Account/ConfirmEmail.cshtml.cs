using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

using QARS.Data.Models;

using System.Text;
using System.Threading.Tasks;

namespace QARS.Areas.Identity.Pages.Account
{
	[AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<User> _userManager;
		private readonly ILogger<ConfirmEmailModel> _logger;

		public ConfirmEmailModel(
			UserManager<User> userManager,
			ILogger<ConfirmEmailModel> logger)
		{
            _userManager = userManager;
			_logger = logger;
		}

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

			User user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
			IdentityResult result = await _userManager.ConfirmEmailAsync(user, code);

			if (result.Succeeded)
			{
				_logger.LogInformation("User {0} has confirmed their email.", user.Id);
			}
			else
			{
				_logger.LogError("Error while confirming email of user {0}", user.Id);
				foreach (IdentityError error in result.Errors)
				{
					_logger.LogError("{0}: {1}", error.Code, error.Description);
				}
			}

            StatusMessage = result.Succeeded
				? "Thank you for confirming your email."
				: "Error confirming your email.";

            return Page();
        }
    }
}
