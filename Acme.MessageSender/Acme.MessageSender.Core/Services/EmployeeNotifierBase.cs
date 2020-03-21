using Acme.MessageSender.Common.Models.Dto;
using Acme.MessageSender.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Acme.MessageSender.Core.Services
{
	public abstract class EmployeeNotifierBase : IEmployeeNotifier
	{
		protected readonly ILogger _logger;

		public EmployeeNotifierBase(ILogger logger)
		{
			_logger = logger;
		}

		public async Task NotifyEmployees()
		{
			foreach (var employee in await GetEmployeesToNotify())
			{
				try
				{
					SendNotificationToEmployee(employee);
					FlagNotificationAsSent(employee);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, $"Error while notifying employee with ID: \"{employee.Id}\"");
				}
			}
		}

		protected abstract Task<IList<Employee>> GetEmployeesToNotify();

		protected abstract void SendNotificationToEmployee(Employee employee);

		protected abstract void FlagNotificationAsSent(Employee employee);
	}
}
