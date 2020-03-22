using Acme.MessageSender.Common.Caching;
using Acme.MessageSender.Common.Models;
using Acme.MessageSender.Common.Models.Settings;
using Acme.MessageSender.Core.Interfaces;
using Acme.MessageSender.Core.Services.Actions;
using Acme.MessageSender.Infrastructure.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Acme.MessageSender.Core.Services.EmployeeNotification
{
	public class BirthdayNotifier : EmployeeNotifierBase, IBirthdayNotifier
	{
		private readonly IEmployeeApiAgent _employeeApiAgent;
		private readonly IEmailRegisterFileAgent _emailRegisterFileAgent;
		private readonly IEmailAgent _emailAgent;
		private readonly IEmployeeDateCalculator _employeeDateCalculator;
		private readonly BirthdayEmailSettings _birthdayEmailSettings;
		private readonly IMapper _mapper;
		private readonly ICacheStore _cacheStore;

		public BirthdayNotifier(IEmployeeApiAgent employeeApiAgent,
			IEmailRegisterFileAgent emailRegisterFileAgent,
			IEmailAgent emailAgent,
			IEmployeeDateCalculator employeeDateCalculator,
			IOptions<AppSettings> appSettings,
			ILogger<BirthdayNotifier> logger,
			IMapper mapper,
			ICacheStore cacheStore)
			: base(logger)
		{
			_employeeApiAgent = employeeApiAgent;
			_emailRegisterFileAgent = emailRegisterFileAgent;
			_emailAgent = emailAgent;
			_employeeDateCalculator = employeeDateCalculator;
			_birthdayEmailSettings = appSettings.Value.BirthdayEmailSettings;
			_mapper = mapper;
			_cacheStore = cacheStore;
		}
		protected override async Task<IList<Employee>> GetEmployeesToNotify()
		{
			var action = new GetEmployeesForBirthdayNotificationAction(_employeeApiAgent,
				_emailRegisterFileAgent,
				_employeeDateCalculator,
				_mapper,
				_cacheStore,
				_logger);

			return await action.Invoke();
		}

		protected override void SendNotificationToEmployee(Employee employee)
		{
			var action = new SendBirthDayNotificationToEmployeesAction(_birthdayEmailSettings, _emailAgent);
			action.Invoke(employee);
		}

		protected override void FlagNotificationAsSent(List<int> employeeIds)
		{
			var action = new FlagNotificationAsSentAction(_emailRegisterFileAgent);
			action.Invoke(employeeIds);
		}
	}
}
