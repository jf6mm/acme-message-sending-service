using Acme.MessageSender.Common.Models;
using Acme.MessageSender.Common.Models.Dto;
using Acme.MessageSender.Core.Interfaces;
using Acme.MessageSender.Infrastructure.Interfaces;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Acme.MessageSender.Core.Services.Actions
{
	public class GetEmployeesForBirthdayNotificationAction
	{
		private readonly IEmployeeApiAgent _employeeApiAgent;
		private readonly IEmailRegisterFileAgent _emailRegisterFileAgent;
		private readonly IEmployeeDateCalculator _employeeDateCalculator;
		private readonly IMapper _mapper;

		public GetEmployeesForBirthdayNotificationAction(IEmployeeApiAgent employeeApiAgent,
			IEmailRegisterFileAgent emailRegisterFileAgent,
			IEmployeeDateCalculator employeeDateCalculator,
			IMapper mapper)
		{
			_employeeApiAgent = employeeApiAgent;
			_emailRegisterFileAgent = emailRegisterFileAgent;
			_employeeDateCalculator = employeeDateCalculator;
			_mapper = mapper;
		}

		public async Task<IList<Employee>> Invoke()
		{
			var allEmployees = _mapper.Map<IList<EmployeeDto>, IList<Employee>>(await _employeeApiAgent.GetAllEmployees());
			var excludedEmployeeIds = await _employeeApiAgent.GetBirthdayListExclusionIds();
			SentEmailRegister sentEmailRegisterToday = _emailRegisterFileAgent.GetEmailRegisterDataForToday();

			// Filter list to employees who should be notified today
			var employeesToNotifyToday = allEmployees.Where(e =>
				_employeeDateCalculator.IsBirthdayToday(e.DateOfBirth)
				&& _employeeDateCalculator.IsEmployeeActive(e)
				&& !excludedEmployeeIds.Contains(e.Id)
				&& !sentEmailRegisterToday.BirthdayEmplyeeIdList.Contains(e.Id));

			return employeesToNotifyToday.ToList();
		}
	}
}
