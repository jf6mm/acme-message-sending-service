using Acme.MessageSender.Common.Models.Settings;
using Acme.MessageSender.Core.Interfaces;
using Acme.MessageSender.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Acme.MessageSender.Core.Services
{
	public class BirthdayMessageSender : IBirthdayMessageSender
	{
		private readonly IEmployeeApiAgent _employeeApiAgent;
		private readonly ILogger<BirthdayMessageSender> _logger;
		private readonly IEmailAgent _emailAgent;
		private readonly BirthdayEmailSettings _birthdayEmailSettings;

		public BirthdayMessageSender(IEmployeeApiAgent employeeApiAgent,
			IEmailAgent emailAgent,
			ILogger<BirthdayMessageSender> logger,
			IOptions<AppSettings> appSettings)
		{
			_employeeApiAgent = employeeApiAgent;
			_emailAgent = emailAgent;
			_logger = logger;
			_birthdayEmailSettings = appSettings.Value.BirthdayEmailSettings;
		}

		public async Task SendBirthdayMessages()
		{
			// 1. Get list of messages from API

			// 2. Get list of exclusions form API

			// 3. Apply filtering of employess who's bday is today (look at leap years as well)

			// 4. Apply filtering of exclusions and inactive employees

			// 5. Apply filtering of already sent bdays

			// 6. Now we have the list of employees who still needs a message today
			// 6.1 Send email message
			// 6.2 Flag employee as sent in persistence layer. (perhaps to a daily text file since it will always be small). Note in the readme that the ideal solution would be an app DB, but since the scope of this solution is very small a DB might be overkill... In that case is there hope for a SQL file DB? yes there must be... Investigate...
			// Note: implement some kind of error handling for the flag code to always run or at least retry.

			// Test sending an email
			//if (_birthdayEmailSettings.SendEmailEnabled)
			//{
			//	string firstName = "Jacques";
			//	string lastName = "Février";
			//	string emailContent = string.Format(_birthdayEmailSettings.EmailTemplate, firstName, lastName );
			//	_emailAgent.SendTextEmail(
			//		new List<string> { _birthdayEmailSettings.TargetEmailAddress },
			//		_birthdayEmailSettings.EmailSubject,
			//		emailContent);
			//}
		}
	}
}
