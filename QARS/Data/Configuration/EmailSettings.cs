using Microsoft.AspNetCore.Identity.UI.Services;

namespace QARS.Data.Configuration
{
	/// <summary>
	/// Defines settings for the <see cref="IEmailSender"/> implementation.
	/// </summary>
	public class EmailSettings
	{
		/// <summary>
		/// Gets the address of the mail server to connect and send emails to.
		/// </summary>
		public string MailServer { get; set; }
		/// <summary>
		/// Gets the port of the mail server to connect to. If the value is 0, then
		/// the default port will be used.
		/// </summary>
		public int MailPort { get; set; }
		/// <summary>
		/// Gets the name of the email sender.
		/// </summary>
		public string SenderName { get; set; }
		/// <summary>
		/// Gets the address of the email sender.
		/// </summary>
		public string Sender { get; set; }
		/// <summary>
		/// Gets the password of this email sender.
		/// </summary>
		public string Password { get; set; }
	}
}
