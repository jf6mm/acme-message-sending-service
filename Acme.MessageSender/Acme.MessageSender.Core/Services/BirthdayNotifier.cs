using Acme.MessageSender.Common.Helpers;
using Acme.MessageSender.Common.Models.Dto;
using Acme.MessageSender.Common.Models.Settings;
using Acme.MessageSender.Core.Interfaces;
using Acme.MessageSender.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Acme.MessageSender.Core.Services
{
	public class BirthdayNotifier : EmployeeNotifierBase, IBirthdayNotifier
	{
		private readonly IEmployeeApiAgent _employeeApiAgent;
		private readonly IEmailRegisterFileAgent _emailRegisterFileAgent;
		private readonly IEmailAgent _emailAgent;
		private readonly BirthdayEmailSettings _birthdayEmailSettings;

		public BirthdayNotifier(IEmployeeApiAgent employeeApiAgent,
			IEmailRegisterFileAgent emailRegisterFileAgent,
			IEmailAgent emailAgent,
			IOptions<AppSettings> appSettings,
			ILogger<BirthdayNotifier> logger)
			: base(logger)
		{
			_employeeApiAgent = employeeApiAgent;
			_emailRegisterFileAgent = emailRegisterFileAgent;
			_emailAgent = emailAgent;
			_birthdayEmailSettings = appSettings.Value.BirthdayEmailSettings;
		}
		protected override async Task<IList<Employee>> GetEmployeesToNotify()
		{
			// 1. Get list of messages from API
			var employees = await _employeeApiAgent.GetAllEmployees();

			// 2. Get list of exclusions form API
			var excludedEmployeeIds = await _employeeApiAgent.GetBirthdayListExclusionIds();

			// 3. Apply filtering of employees who's bday is today (look at leap years as well)
			var todaysBirthdayEmployees = employees.Where(e => IsBirthdayToday(DateTimeConverter.FromIsoDate(e.DateOfBirth)));

			// 4. Apply filtering of exclusions and inactive employees
			var bDayEmployeesMinusExclusions = todaysBirthdayEmployees.Where(e => !excludedEmployeeIds.Contains(e.Id));
			var bDayEmployeesMinusInactive = bDayEmployeesMinusExclusions.Where(e => IsEmployeeActive(e));

			// 5. Apply filtering of already sent bdays
			SentEmailRegister sentEmailRegisterToday = _emailRegisterFileAgent.GetEmailRegisterDataForToday();
			var bDayEmployeesMinusAlreadySent = bDayEmployeesMinusInactive
				.Where(e => !sentEmailRegisterToday.BirthdayEmplyeeIdList.Contains(e.Id));

			return bDayEmployeesMinusAlreadySent.ToList();
		}

		protected override void SendNotificationToEmployee(Employee employee)
		{
			string emailContent = string.Format(_birthdayEmailSettings.EmailTemplate, employee.Name, employee.LastName);

			_emailAgent.SendTextEmail(
				new List<string> { _birthdayEmailSettings.TargetEmailAddress },
				_birthdayEmailSettings.EmailSubject,
				emailContent);
		}

		protected override void FlagNotificationAsSent(Employee employee)
		{
			var todaysSentEmailRegister = _emailRegisterFileAgent.GetEmailRegisterDataForToday();
			if (!todaysSentEmailRegister.BirthdayEmplyeeIdList.Contains(employee.Id))
			{
				todaysSentEmailRegister.BirthdayEmplyeeIdList.Add(employee.Id);
			}
			_emailRegisterFileAgent.SaveEmailRegisterData(todaysSentEmailRegister);
		}



		//TODO: Move logic to seperate class(es)
		private bool IsBirthdayToday(DateTime dateOfBirth)
		{
			return IsSameDayOfYear(DateTime.Now, dateOfBirth)
				|| (IsTodayMarchOneInANonLeapYear() && IsBirthdayOnLeapDay(dateOfBirth));
		}

		private bool IsSameDayOfYear(DateTime date1, DateTime date2)
		{
			return date1.Month == date2.Month
				&& date1.Day == date2.Day;
		}

		private bool IsLeapYear()
		{
			return DateTime.Now.Year % 4 == 0;
		}

		private bool IsTodayMarchOneInANonLeapYear()
		{
			return !IsLeapYear() && IsSameDayOfYear(new DateTime(0, 3, 1), DateTime.Now);
		}

		private bool IsBirthdayOnLeapDay(DateTime birthDay)
		{
			return IsSameDayOfYear(new DateTime(0, 2, 29), birthDay);
		}

		private bool IsEmployeeActive(Employee employee)
		{
			var startDate = DateTimeConverter.FromIsoDate(employee.EmploymentStartDate);
			var endDate = DateTimeConverter.FromIsoDate(employee.EmploymentStartDate);

			return startDate.Date <= DateTime.Now.Date
				&& endDate == DateTime.MinValue || endDate.Date >= DateTime.Now.Date;
		}
	}
}
