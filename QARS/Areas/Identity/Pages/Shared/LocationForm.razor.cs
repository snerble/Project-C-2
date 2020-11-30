using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using QARS.Data.Models;

using System.ComponentModel.DataAnnotations;

namespace QARS.Areas.Identity.Pages.Account
{
	[AllowAnonymous]
	public class LocationModel : PageModel
	{
		[BindProperty]
		public Location Input { get; set; }

		public class InputModel
		{
			[Required]
			[EmailAddress]
			public string Email { get; set; }

			[Required]
			[DataType(DataType.Password)]
			public string Password { get; set; }

			[Display(Name = "Remember me?")]
			public bool RememberMe { get; set; }
		}
	}
}
