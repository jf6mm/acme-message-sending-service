using Acme.MessageSender.Common.Models;
using Acme.MessageSender.Core.Interfaces;
using Acme.MessageSender.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Acme.MessageSender.Test.Core.Services
{
	[TestClass]
	public class EmployeeDateCalculatorTest
	{
		private Mock<IDateTimeProvider> _dateTimeProvider;

		[TestInitialize]
		public void TestInitialize()
		{
			_dateTimeProvider = new Mock<IDateTimeProvider>();
		}

		[TestCleanup]
		public void TestCleanup()
		{

		}

		#region IsBirthdayToday

		[TestMethod]
		public void EmployeeDateCalculator_IsBirthdayToday_No()
		{
			// Arrange
			_dateTimeProvider.Setup(x => x.CurrentDateTime()).Returns(new DateTime(2000, 1, 2));
			var dateCalculator = CreateEmployeeDateCalculator();
			var dateOfBirth = new DateTime(1980, 1, 3);

			// Act
			var result = dateCalculator.IsBirthdayToday(dateOfBirth);

			// Assert
			Assert.IsFalse(result);
		}

		[TestMethod]
		public void EmployeeDateCalculator_IsBirthdayToday_Yes()
		{
			// Arrange
			_dateTimeProvider.Setup(x => x.CurrentDateTime()).Returns(new DateTime(2000, 1, 2));
			var dateCalculator = CreateEmployeeDateCalculator();
			var dateOfBirth = new DateTime(1980, 1, 2);

			// Act
			var result = dateCalculator.IsBirthdayToday(dateOfBirth);

			// Assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void EmployeeDateCalculator_IsBirthdayToday_Yes_LeapBirthday_1MarchInANonLeapYear()
		{
			// Arrange
			_dateTimeProvider.Setup(x => x.CurrentDateTime()).Returns(new DateTime(2001, 3, 1));
			var dateCalculator = CreateEmployeeDateCalculator();
			var dateOfBirth = new DateTime(1980, 2, 29);

			// Act
			var result = dateCalculator.IsBirthdayToday(dateOfBirth);

			// Assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void EmployeeDateCalculator_IsBirthdayToday_No_LeapBirthday_1MarchInALeapYear()
		{
			// Arrange
			_dateTimeProvider.Setup(x => x.CurrentDateTime()).Returns(new DateTime(2000, 3, 1));
			var dateCalculator = CreateEmployeeDateCalculator();
			var dateOfBirth = new DateTime(1980, 2, 29);

			// Act
			var result = dateCalculator.IsBirthdayToday(dateOfBirth);

			// Assert
			Assert.IsFalse(result);
		}

		[TestMethod]
		public void EmployeeDateCalculator_IsBirthdayToday_Yes_LeapBirthday_OnLeapDay()
		{
			// Arrange
			_dateTimeProvider.Setup(x => x.CurrentDateTime()).Returns(new DateTime(2000, 2, 29));
			var dateCalculator = CreateEmployeeDateCalculator();
			var dateOfBirth = new DateTime(1980, 2, 29);

			// Act
			var result = dateCalculator.IsBirthdayToday(dateOfBirth);

			// Assert
			Assert.IsTrue(result);
		}

		#endregion

		#region IsSameDayOfYear

		[TestMethod]
		public void EmployeeDateCalculator_IsSameDayOfYear_No()
		{
			// Arrange
			var dateCalculator = CreateEmployeeDateCalculator();

			// Act
			var result = dateCalculator.IsSameDayOfYear(new DateTime(2000, 2, 3), new DateTime(2000, 2, 4));

			// Assert
			Assert.IsFalse(result);
		}

		[TestMethod]
		public void EmployeeDateCalculator_IsSameDayOfYear_Yes()
		{
			// Arrange
			var dateCalculator = CreateEmployeeDateCalculator();

			// Act
			var result = dateCalculator.IsSameDayOfYear(new DateTime(2000, 2, 3), new DateTime(2002, 2, 3));

			// Assert
			Assert.IsTrue(result);
		}

		#endregion

		#region IsLeapYear Tests

		[TestMethod]
		public void EmployeeDateCalculator_IsLeapYear_No()
		{
			// Arrange
			_dateTimeProvider.Setup(x => x.CurrentDateTime()).Returns(new DateTime(2001, 1, 1));
			var dateCalculator = CreateEmployeeDateCalculator();

			// Act
			var result = dateCalculator.IsLeapYear();

			// Assert
			Assert.IsFalse(result);
		}

		[TestMethod]
		public void EmployeeDateCalculator_IsLeapYear_Yes()
		{
			// Arrange
			_dateTimeProvider.Setup(x => x.CurrentDateTime()).Returns(new DateTime(2000, 1, 1));
			var dateCalculator = CreateEmployeeDateCalculator();

			// Act
			var result = dateCalculator.IsLeapYear();

			// Assert
			Assert.IsTrue(result);
		}

		#endregion

		#region IsTodayMarchOneInANonLeapYear Tests

		[TestMethod]
		public void EmployeeDateCalculator_IsTodayMarchOneInANonLeapYear_No_ItIsALeapYear()
		{
			// Arrange
			_dateTimeProvider.Setup(x => x.CurrentDateTime()).Returns(new DateTime(2000, 3, 1));
			var dateCalculator = CreateEmployeeDateCalculator();

			// Act
			var result = dateCalculator.IsTodayMarchOneInANonLeapYear();

			// Assert
			Assert.IsFalse(result);
		}

		[TestMethod]
		public void EmployeeDateCalculator_IsTodayMarchOneInANonLeapYear_No_ItIsNotMarchOne()
		{
			// Arrange
			_dateTimeProvider.Setup(x => x.CurrentDateTime()).Returns(new DateTime(2001, 2, 28));
			var dateCalculator = CreateEmployeeDateCalculator();

			// Act
			var result = dateCalculator.IsTodayMarchOneInANonLeapYear();

			// Assert
			Assert.IsFalse(result);
		}

		[TestMethod]
		public void EmployeeDateCalculator_IsTodayMarchOneInANonLeapYear_Yes()
		{
			// Arrange
			_dateTimeProvider.Setup(x => x.CurrentDateTime()).Returns(new DateTime(2001, 3, 1));
			var dateCalculator = CreateEmployeeDateCalculator();

			// Act
			var result = dateCalculator.IsTodayMarchOneInANonLeapYear();

			// Assert
			Assert.IsTrue(result);
		}

		#endregion

		#region IsBirthdayOnLeapDay Tests

		[TestMethod]
		public void EmployeeDateCalculator_IsBirthdayOnLeapDay_No()
		{
			// Arrange
			var dateCalculator = CreateEmployeeDateCalculator();
			var dateOfBirth = new DateTime(2000, 2, 28);

			// Act
			var result = dateCalculator.IsBirthdayOnLeapDay(dateOfBirth);

			// Assert
			Assert.IsFalse(result);
		}

		[TestMethod]
		public void EmployeeDateCalculator_IsBirthdayOnLeapDay_Yes()
		{
			// Arrange
			var dateCalculator = CreateEmployeeDateCalculator();
			var dateOfBirth = new DateTime(2000, 2, 29);

			// Act
			var result = dateCalculator.IsBirthdayOnLeapDay(dateOfBirth);

			// Assert
			Assert.IsTrue(result);
		}

		#endregion

		#region IsEmployeeActive Tests

		[TestMethod]
		public void EmployeeDateCalculator_IsEmployeeActive_NoEndDate()
		{
			// Arrange
			_dateTimeProvider.Setup(x => x.CurrentDateTime()).Returns(new DateTime(2000, 1, 1));
			var dateCalculator = CreateEmployeeDateCalculator();
			var employee = new Employee { EmploymentStartDate = new DateTime(1999, 12, 31) };

			// Act
			var result = dateCalculator.IsEmployeeActive(employee);

			// Assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void EmployeeDateCalculator_IsEmployeeActive_NoStartDate()
		{
			// Arrange
			_dateTimeProvider.Setup(x => x.CurrentDateTime()).Returns(new DateTime(2000, 1, 1));
			var dateCalculator = CreateEmployeeDateCalculator();
			var employee = new Employee { EmploymentEndDate = new DateTime(2001, 1, 1) };

			// Act
			var result = dateCalculator.IsEmployeeActive(employee);

			// Assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void EmployeeDateCalculator_IsEmployeeActive_FirstDayTermination()
		{
			// Arrange
			_dateTimeProvider.Setup(x => x.CurrentDateTime()).Returns(new DateTime(2000, 1, 1));
			var dateCalculator = CreateEmployeeDateCalculator();
			var employee = new Employee
			{
				EmploymentStartDate = new DateTime(2000, 1, 1),
				EmploymentEndDate = new DateTime(2000, 1, 1)
			};

			// Act
			var result = dateCalculator.IsEmployeeActive(employee);

			// Assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void EmployeeDateCalculator_IsEmployeeActive_Negative_Past()
		{
			// Arrange
			_dateTimeProvider.Setup(x => x.CurrentDateTime()).Returns(new DateTime(2000, 1, 1));
			var dateCalculator = CreateEmployeeDateCalculator();
			var employee = new Employee
			{
				EmploymentStartDate = new DateTime(1999, 1, 1),
				EmploymentEndDate = new DateTime(1999, 12, 31)
			};

			// Act
			var result = dateCalculator.IsEmployeeActive(employee);

			// Assert
			Assert.IsFalse(result);
		}

		[TestMethod]
		public void EmployeeDateCalculator_IsEmployeeActive_Negative_Future()
		{
			// Arrange
			_dateTimeProvider.Setup(x => x.CurrentDateTime()).Returns(new DateTime(2000, 1, 1));
			var dateCalculator = CreateEmployeeDateCalculator();
			var employee = new Employee
			{
				EmploymentStartDate = new DateTime(2000, 1, 2),
				EmploymentEndDate = new DateTime(2000, 1, 5)
			};

			// Act
			var result = dateCalculator.IsEmployeeActive(employee);

			// Assert
			Assert.IsFalse(result);
		}

		#endregion

		#region Private Methods

		private EmployeeDateCalculator CreateEmployeeDateCalculator()
		{
			return new EmployeeDateCalculator(_dateTimeProvider.Object);
		}

		#endregion
	}
}
