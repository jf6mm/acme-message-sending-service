namespace Acme.MessageSender.Common.Models.Settings
{
	public class BirthdayEmailSettings
	{
		public string TargetEmailAddress { get; set; }

		public string EmailTemplate { get; set; }
		
		public string EmailSubject { get; set; }
	}
}
