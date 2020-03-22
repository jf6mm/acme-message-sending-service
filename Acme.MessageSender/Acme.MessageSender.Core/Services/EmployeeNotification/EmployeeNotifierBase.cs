using Acme.MessageSender.Common.Models;
using Acme.MessageSender.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Acme.MessageSender.Core.Services.EmployeeNotification
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
			var employeesToNotify = await GetEmployeesToNotify();
			List<int> employeeIdsToFlagAsSent = new List<int>();

			_logger.LogDebug($"Sending birthday notifications to {employeesToNotify.Count} employees");
			foreach (var employee in employeesToNotify)
			{
				try
				{
					SendNotificationToEmployee(employee);
					employeeIdsToFlagAsSent.Add(employee.Id);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, $"Error while notifying employee with ID: \"{employee.Id}\"");
				}
			}

			FlagNotificationAsSent(employeeIdsToFlagAsSent);
		}

		protected abstract Task<IList<Employee>> GetEmployeesToNotify();

		protected abstract void SendNotificationToEmployee(Employee employee);

		protected abstract void FlagNotificationAsSent(List<int> employeeIds);
	}
}