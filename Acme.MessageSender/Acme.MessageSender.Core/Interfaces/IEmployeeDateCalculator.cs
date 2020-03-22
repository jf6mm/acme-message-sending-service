using Acme.MessageSender.Common.Models;
using System;

namespace Acme.MessageSender.Core.Interfaces
{
	public interface IEmployeeDateCalculator
	{
		bool IsBirthdayToday(DateTime dateOfBirth);

		bool IsSameDayOfYear(DateTime date1, DateTime date2);

		bool IsLeapYear();

		bool IsTodayMarchOneInANonLeapYear();

		bool IsBirthdayOnLeapDay(DateTime birthDay);

		bool IsEmployeeActive(Employee employee);
	}
}
