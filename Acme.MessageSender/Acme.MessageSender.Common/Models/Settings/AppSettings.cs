namespace Acme.MessageSender.Common.Models.Settings
{
	public class AppSettings
	{
		public int NotificationTaskDelayMinutes { get; set; }

		public string EmployeeApiBaseUrl { get; set; }

		public BirthdayEmailSettings BirthdayEmailSettings { get; set; }

		public SmtpSettings SmtpSettings { get; set; }
	}
}
