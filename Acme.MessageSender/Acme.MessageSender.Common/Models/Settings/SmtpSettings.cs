namespace Acme.MessageSender.Common.Models.Settings
{
	public class SmtpSettings
	{
		public string Server { get; set; }
		public bool UseAuthentication { get; set; }
		public bool UseSecureConnection { get; set; }
		public int Port { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public bool SendEmailEnabled { get; set; }
	}
}
