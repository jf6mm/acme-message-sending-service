using Acme.MessageSender.Common.Models;
using Acme.MessageSender.Core.Interfaces;
using System;

namespace Acme.MessageSender.Core.Services
{
	public class EmployeeDateCalculator : IEmployeeDateCalculator
	{
		private const int DefaultLeapYear = 2000; // used in calculations where the year is irrelevant, but it must be a leap year

		private IDateTimeProvider _dateTimeProvider;

		public EmployeeDateCalculator(IDateTimeProvider dateTimeProvider)
		{
			_dateTimeProvider = dateTimeProvider;
		}

		public bool IsBirthdayToday(DateTime dateOfBirth)
		{
			return IsSameDayOfYear(_dateTimeProvider.CurrentDateTime(), dateOfBirth)
				|| (IsTodayMarchOneInANonLeapYear() && IsBirthdayOnLeapDay(dateOfBirth));
		}

		public bool IsSameDayOfYear(DateTime date1, DateTime date2)
		{
			return date1.Month == date2.Month
				&& date1.Day == date2.Day;
		}

		public bool IsLeapYear()
		{
			return _dateTimeProvider.CurrentDateTime().Year % 4 == 0;
		}

		public bool IsTodayMarchOneInANonLeapYear()
		{
			return !IsLeapYear() && IsSameDayOfYear(new DateTime(DefaultLeapYear, 3, 1), _dateTimeProvider.CurrentDateTime());
		}

		public bool IsBirthdayOnLeapDay(DateTime birthDay)
		{
			return IsSameDayOfYear(new DateTime(DefaultLeapYear, 2, 29), birthDay);
		}

		public bool IsEmployeeActive(Employee employee)
		{
			return (!employee.EmploymentStartDate.HasValue 
				|| employee.EmploymentStartDate.Value.Date <= _dateTimeProvider.CurrentDateTime().Date)
				&& (!employee.EmploymentEndDate.HasValue
				|| employee.EmploymentEndDate.Value.Date >= _dateTimeProvider.CurrentDateTime().Date);
		}
	}
}
