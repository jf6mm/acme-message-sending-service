using Acme.MessageSender.Common.Models;
using Acme.MessageSender.Common.Models.Settings;
using Acme.MessageSender.Infrastructure.Interfaces;
using System.Collections.Generic;

namespace Acme.MessageSender.Core.Services.Actions
{
	public class SendBirthDayNotificationToEmployeesAction
	{
		private readonly BirthdayEmailSettings _birthdayEmailSettings;
		private readonly IEmailAgent _emailAgent;

		public SendBirthDayNotificationToEmployeesAction(BirthdayEmailSettings birthdayEmailSettings,
			IEmailAgent emailAgent)
		{
			_birthdayEmailSettings = birthdayEmailSettings;
			_emailAgent = emailAgent;
		}

		public void Invoke(Employee employee)
		{
			string emailContent = string.Format(_birthdayEmailSettings.EmailTemplate, employee.Name, employee.LastName);

			_emailAgent.SendTextEmail(
				new List<string> { _birthdayEmailSettings.TargetEmailAddress },
				_birthdayEmailSettings.EmailSubject,
				emailContent);
		}
	}
}
