using Acme.MessageSender.Common.Models.Dto;
using Acme.MessageSender.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Acme.MessageSender.Core.Services
{
	public class WorkAnniversaryNotifier : EmployeeNotifierBase, IWorkAnniversaryNotifier
	{
		public WorkAnniversaryNotifier(ILogger<WorkAnniversaryNotifier> logger)
			: base(logger)
		{
		}

		protected override void FlagNotificationAsSent(Employee employee)
		{
			throw new System.NotImplementedException();
		}

		protected override Task<IList<Employee>> GetEmployeesToNotify()
		{
			throw new System.NotImplementedException();
		}

		protected override void SendNotificationToEmployee(Employee employee)
		{
			throw new System.NotImplementedException();
		}
	}
}
