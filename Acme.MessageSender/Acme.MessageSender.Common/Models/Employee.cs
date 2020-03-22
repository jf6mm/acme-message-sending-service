using System;

namespace Acme.MessageSender.Common.Models
{
	public class Employee
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string LastName { get; set; }

		public DateTime DateOfBirth { get; set; }

		public DateTime? EmploymentStartDate { get; set; }

		public DateTime? EmploymentEndDate { get; set; }
	}
}
