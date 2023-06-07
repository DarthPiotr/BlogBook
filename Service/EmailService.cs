using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace BlogBook.Service
{
	public class EmailService : IEmailSender
	{
		private readonly ILogger _logger;

		public EmailService(IOptions<AuthMessageSenderOptions> optionsAccessor,
						   ILogger<EmailService> logger)
		{
			Options = optionsAccessor.Value;
			_logger = logger;
		}

		public AuthMessageSenderOptions Options { get; } //Set with Secret Manager.

		public async Task SendEmailAsync(string toEmail, string subject, string message)
		{
			if (string.IsNullOrEmpty(Options.SendGridKey))
			{
				throw new Exception("Null SendGridKey");
			}
			await Execute(Options.SendGridKey, subject, message, toEmail);
		}

		public async Task Execute(string apiKey, string subject, string message, string toEmail)
		{
			var client = new SendGridClient(apiKey);
			var msg = new SendGridMessage()
			{
				From = new EmailAddress("piotr.marciniak@student.put.poznan.pl", "BlogBook"),
				Subject = subject,
				PlainTextContent = message,
				HtmlContent = message
			};
			msg.AddTo(new EmailAddress(toEmail));

			// Disable click tracking.
			// See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
			msg.SetClickTracking(false, false);
			var response = await client.SendEmailAsync(msg);
			_logger.LogInformation(response.IsSuccessStatusCode
								   ? $"Email to {toEmail} queued successfully!"
								   : $"Failure Email to {toEmail}");
		}

	}
}
