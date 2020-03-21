namespace Acme.MessageSender.Common.Models.Settings
{
	public class AppSettings
	{
		public string EmployeeApiBaseUrl { get; set; }

		public BirthdayEmailSettings BirthdayEmailSettings { get; set; }

		public SmtpSettings SmtpSettings { get; set; }
	}
}
