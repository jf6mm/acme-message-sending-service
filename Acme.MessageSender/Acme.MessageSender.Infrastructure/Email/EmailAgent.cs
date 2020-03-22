using Acme.MessageSender.Common.Models.Settings;
using Acme.MessageSender.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Acme.MessageSender.Infrastructure.Email
{
	public class EmailAgent : IEmailAgent
	{
		private readonly ILogger _logger;
		private readonly SmtpSettings _smtpSettings;

		public EmailAgent(IOptions<AppSettings> appSettings, ILogger<EmailAgent> logger)
		{
			_smtpSettings = appSettings.Value.SmtpSettings;
			_logger = logger;
		}

		public void SendTextEmail(IList<string> recipients, string subject, string content)
		{
			_logger.LogDebug($"Sending email to {string.Join(",", recipients)}");

			SmtpClient client = new SmtpClient(_smtpSettings.Server);
			client.EnableSsl = _smtpSettings.UseSecureConnection;
			client.Port = _smtpSettings.Port;
			client.UseDefaultCredentials = !_smtpSettings.UseAuthentication;
			if (_smtpSettings.UseAuthentication)
			{
				client.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
			}

			MailMessage mailMessage = new MailMessage
			{
				From = new MailAddress("no-reply@acme.com"),
				Body = content,
				IsBodyHtml = false,
				Subject = subject,
				BodyEncoding = Encoding.UTF8
			};

			foreach(var address in recipients)
			{
				if (string.IsNullOrEmpty(address)) continue;
				mailMessage.To.Add(new MailAddress(address));
			}

			// Exit method here if Sending Emails is disabled - This is for use during development.
			if (!_smtpSettings.SendEmailEnabled) return;

			client.Send(mailMessage);
		}
	}
}
