using Acme.MessageSender.Infrastructure.Interfaces;
using System.Collections.Generic;

namespace Acme.MessageSender.Core.Services.Actions
{
	public class FlagNotificationAsSentAction
	{
		private readonly IEmailRegisterFileAgent _emailRegisterFileAgent;

		public FlagNotificationAsSentAction(IEmailRegisterFileAgent emailRegisterFileAgent)
		{
			_emailRegisterFileAgent = emailRegisterFileAgent;
		}

		public void Invoke(List<int> employeeIds)
		{
			var todaysSentEmailRegister = _emailRegisterFileAgent.GetEmailRegisterDataForToday();
			todaysSentEmailRegister.BirthdayEmplyeeIdList.AddRange(employeeIds);
			_emailRegisterFileAgent.SaveEmailRegisterData(todaysSentEmailRegister);
		}
	}
}
