using HeyRed.MarkdownSharp;

using MailKit.Net.Smtp;

using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MimeKit;

using QARS.Data.Configuration;
using QARS.Views;

using System;
using System.Threading.Tasks;

namespace QARS.Data.Services
{
	public class EmailSender : IEmailSender
	{
		/// <summary>
		/// The name of the default email template view.
		/// </summary>
		public const string DefaultTemplate = "DefaultEmailTemplate";

		private readonly EmailSettings _emailSettings;
		private readonly Markdown _markdown;
		private readonly IRazorViewToStringRenderer _razor;
		private readonly IHostEnvironment _env;

		public EmailSender(
			IOptions<EmailSettings> emailSettings,
			Markdown markdown,
			IRazorViewToStringRenderer razor,
			IHostEnvironment env,
			ILogger<EmailSender> log)
		{
			_emailSettings = emailSettings.Value;
			_markdown = markdown;
			_razor = razor;
			_env = env;
			Log = log;
		}

		private ILogger<EmailSender> Log { get; }

		Task IEmailSender.SendEmailAsync(string email, string subject, string htmlMessage)
		{
			SendEmail(email, subject, new EmailModel { Message = htmlMessage });
			return Task.CompletedTask;
		}

		/// <summary>
		/// Sends an email containing markdown text.
		/// </summary>
		/// <param name="email">The email address of the recipient.</param>
		/// <param name="subject">The subject of the email.</param>
		/// <param name="markdownMessage">The message with markdown formatting.</param>
		/// <param name="templateName">The name of the Razor email template.</param>
		public void SendMarkdown(string email, string subject, string markdownMessage, string templateName = DefaultTemplate)
			=> SendEmail(email,
				subject,
				new EmailModel { Message = _markdown.Transform(markdownMessage) },
				templateName);

		/// <summary>
		/// Sends an email containing an HTML message.
		/// </summary>
		/// <param name="email">The email address of the recipient.</param>
		/// <param name="subject">The subject of the email.</param>
		/// <param name="htmlMessage">The message in HTML.</param>
		/// <param name="templateName">The name of the Razor email template.</param>
		public void SendEmail(string email, string subject, string htmlMessage, string templateName = DefaultTemplate)
			=> SendEmail(email,
				subject,
				new EmailModel { Message = htmlMessage },
				templateName);

		/// <summary>
		/// Sends an email with the specified <paramref name="model"/>.
		/// </summary>
		/// <param name="email">The email address of the recipient.</param>
		/// <param name="subject">The subject of the email.</param>
		/// <param name="model">The model to use when rendering the email template view.</param>
		/// <param name="templateName">The name of the Razor email template.</param>
		public void SendEmail<TModel>(string email, string subject, TModel model, string templateName = DefaultTemplate)
			where TModel : EmailModel
		{
			// ConfigureAwait(false) makes this a fire-and-forget task
			SendEmailAsync(email, subject, model, templateName).ConfigureAwait(false);
		}

		private async Task SendEmailAsync<TModel>(string email, string subject, TModel model, string templateName = DefaultTemplate)
			where TModel : EmailModel
		{
			try
			{
				var mimeMessage = new MimeMessage();

				mimeMessage.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.Sender));
				mimeMessage.To.Add(MailboxAddress.Parse(email));
				mimeMessage.Subject = subject;

				mimeMessage.Body = new TextPart("html")
				{
					Text = await _razor.RenderViewToStringAsync(templateName, model)
				};

				using var client = new SmtpClient
				{
					ServerCertificateValidationCallback = (s, c, h, e) => true
				};

				if (_env.IsDevelopment())
				{
					// The third parameter is useSSL (true if the client should make an SSL-wrapped
					// connection to the server; otherwise, false).
					await client.ConnectAsync(_emailSettings.MailServer, _emailSettings.MailPort, true);
				}
				else
				{
					await client.ConnectAsync(_emailSettings.MailServer);
				}

				// Note: only needed if the SMTP server requires authentication
				await client.AuthenticateAsync(_emailSettings.Sender, _emailSettings.Password);

				await client.SendAsync(mimeMessage);

				await client.DisconnectAsync(true);
			}
			catch (Exception e)
			{
				Log.LogError(e, "Error while sending email: {0}", e.Message);
			}
		}
	}
}
