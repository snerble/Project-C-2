using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

using QARS.Data.Models;

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace QARS.Data.Services
{
	public class EmailManager
	{
		private const string ConfirmEmailTemplate = "ConfirmEmailTemplate";

		private readonly NavigationManager _navigationManager;
		private readonly UserManager<User> _userManager;
		private readonly EmailSender _emailSender;

		public EmailManager(
			NavigationManager navigationManager,
			UserManager<User> userManager,
			EmailSender emailSender)
		{
			_navigationManager = navigationManager;
			_userManager = userManager;
			_emailSender = emailSender;
		}

		/// <summary>
		/// Sends an account confirmation email to the specified <paramref name="user"/>.
		/// </summary>
		/// <param name="user">The user to whom a confirmation email must be sent.</param>
		/// <exception cref="ArgumentException">Thrown when the <paramref name="user"/>'s
		/// <see cref="User.Email"/> is invalid.</exception>
		/// <exception cref="InvalidOperationException">Thrown when the <paramref name="user"/>'s
		/// email has already been verified.</exception>
		public Task SendConfirmationEmailAsync(User user)
			=> SendConfirmationEmailAsync(user, _navigationManager.BaseUri);

		/// <summary>
		/// Sends an account confirmation email to the specified <paramref name="user"/>.
		/// </summary>
		/// <param name="user">The user to whom a confirmation email must be sent.</param>
		/// <param name="baseUrl">The base url of the QARS website.</param>
		/// <exception cref="ArgumentException">Thrown when the <paramref name="user"/>'s
		/// <see cref="User.Email"/> is invalid.</exception>
		/// <exception cref="InvalidOperationException">Thrown when the <paramref name="user"/>'s
		/// email has already been verified.</exception>
		/// <remarks>
		/// Use this method if the 'RemoteNavigationManager' has not been initialized yet.
		/// </remarks>
		public async Task SendConfirmationEmailAsync(User user, string baseUrl)
		{
			if (!new EmailAddressAttribute().IsValid(user.Email))
				throw new ArgumentException("Invalid Email address for user.", $"{nameof(user)}.{nameof(user.Email)}");
			if (user.IsEmailVerified)
				throw new InvalidOperationException($"The user's email is already verified (Id:{user.Id})");

			// Get the token that lets a user verify their account
			var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
			code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

			// Build the query
			var qb = new QueryBuilder
			{
				{ "userId", user.Id.ToString() },
				{ "code", code }
			};
			var b = new UriBuilder(baseUrl)
			{
				Path = "Identity/Account/ConfirmEmail",
				Query = qb.ToQueryString().ToUriComponent()
			};

			// Send the email using the ConfirmEmail template
			_emailSender.SendEmail(user.Email,
				"Confirm your email",
				$"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(b.ToString())}'>clicking here</a>.",
				ConfirmEmailTemplate);
		}
	}
}
