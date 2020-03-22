using Acme.MessageSender.Common.Models;
using Acme.MessageSender.Common.Models.Dto;
using Acme.MessageSender.Common.Models.Mapping;
using Acme.MessageSender.Core.Interfaces;
using Acme.MessageSender.Core.Services.Actions;
using Acme.MessageSender.Infrastructure.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace Acme.MessageSender.Test.Core.Services.Actions
{
	[TestClass]
	public class GetEmployeesForBirthdayNotificationActionTest
	{
		private Mock<IEmployeeApiAgent> _employeeApiAgent;
		private Mock<IEmailRegisterFileAgent> _emailRegisterFileAgent;
		private Mock<IEmployeeDateCalculator> _employeeDateCalculator;

		private List<EmployeeDto> _allEmployees = new List<EmployeeDto>
		{
			new EmployeeDto{ Id = 100 }
		};

		[TestInitialize]
		public void TestInitialize()
		{
			_emailRegisterFileAgent = new Mock<IEmailRegisterFileAgent>();
			_employeeApiAgent = new Mock<IEmployeeApiAgent>();
			_employeeDateCalculator = new Mock<IEmployeeDateCalculator>();
		}

		[TestMethod]
		public void GetEmployeesForBirthdayNotificationAction_MeetsAllCriteria()
		{
			// Arrange
			_employeeApiAgent.Setup(x => x.GetAllEmployees()).ReturnsAsync(_allEmployees);
			_employeeApiAgent.Setup(x => x.GetBirthdayListExclusionIds()).ReturnsAsync(new List<int> { 111, 112 });
			_employeeDateCalculator.Setup(x => x.IsBirthdayToday(It.IsAny<DateTime>())).Returns(true);
			_employeeDateCalculator.Setup(x => x.IsEmployeeActive(It.IsAny<Employee>())).Returns(true);
			SentEmailRegister sentEmailRegister = new SentEmailRegister { BirthdayEmplyeeIdList = new List<int> { 121, 122 } };
			_emailRegisterFileAgent.Setup(x => x.GetEmailRegisterDataForToday()).Returns(sentEmailRegister);

			// Act
			var action = CreateAction();
			var result = action.Invoke().Result;

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Count);
		}

		[TestMethod]
		public void GetEmployeesForBirthdayNotificationAction_IsInExclusionList()
		{
			// Arrange
			_employeeApiAgent.Setup(x => x.GetAllEmployees()).ReturnsAsync(_allEmployees);
			_employeeApiAgent.Setup(x => x.GetBirthdayListExclusionIds()).ReturnsAsync(new List<int> { 100 });
			_employeeDateCalculator.Setup(x => x.IsBirthdayToday(It.IsAny<DateTime>())).Returns(true);
			_employeeDateCalculator.Setup(x => x.IsEmployeeActive(It.IsAny<Employee>())).Returns(true);
			SentEmailRegister sentEmailRegister = new SentEmailRegister { BirthdayEmplyeeIdList = new List<int> { 121, 122 } };
			_emailRegisterFileAgent.Setup(x => x.GetEmailRegisterDataForToday()).Returns(sentEmailRegister);

			// Act
			var action = CreateAction();
			var result = action.Invoke().Result;

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(0, result.Count);
		}

		[TestMethod]
		public void GetEmployeesForBirthdayNotificationAction_BirthdayIsNotToday()
		{
			// Arrange
			_employeeApiAgent.Setup(x => x.GetAllEmployees()).ReturnsAsync(_allEmployees);
			_employeeApiAgent.Setup(x => x.GetBirthdayListExclusionIds()).ReturnsAsync(new List<int> { 111, 112 });
			_employeeDateCalculator.Setup(x => x.IsBirthdayToday(It.IsAny<DateTime>())).Returns(false);
			_employeeDateCalculator.Setup(x => x.IsEmployeeActive(It.IsAny<Employee>())).Returns(true);
			SentEmailRegister sentEmailRegister = new SentEmailRegister { BirthdayEmplyeeIdList = new List<int> { 121, 122 } };
			_emailRegisterFileAgent.Setup(x => x.GetEmailRegisterDataForToday()).Returns(sentEmailRegister);

			// Act
			var action = CreateAction();
			var result = action.Invoke().Result;

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(0, result.Count);
		}

		[TestMethod]
		public void GetEmployeesForBirthdayNotificationAction_EmployeeNotActive()
		{
			// Arrange
			_employeeApiAgent.Setup(x => x.GetAllEmployees()).ReturnsAsync(_allEmployees);
			_employeeApiAgent.Setup(x => x.GetBirthdayListExclusionIds()).ReturnsAsync(new List<int> { 111, 112 });
			_employeeDateCalculator.Setup(x => x.IsBirthdayToday(It.IsAny<DateTime>())).Returns(true);
			_employeeDateCalculator.Setup(x => x.IsEmployeeActive(It.IsAny<Employee>())).Returns(false);
			SentEmailRegister sentEmailRegister = new SentEmailRegister { BirthdayEmplyeeIdList = new List<int> { 121, 122 } };
			_emailRegisterFileAgent.Setup(x => x.GetEmailRegisterDataForToday()).Returns(sentEmailRegister);

			// Act
			var action = CreateAction();
			var result = action.Invoke().Result;

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(0, result.Count);
		}

		[TestMethod]
		public void GetEmployeesForBirthdayNotificationAction_NotificationAlreadySent()
		{
			// Arrange
			_employeeApiAgent.Setup(x => x.GetAllEmployees()).ReturnsAsync(_allEmployees);
			_employeeApiAgent.Setup(x => x.GetBirthdayListExclusionIds()).ReturnsAsync(new List<int> { 111, 112 });
			_employeeDateCalculator.Setup(x => x.IsBirthdayToday(It.IsAny<DateTime>())).Returns(true);
			_employeeDateCalculator.Setup(x => x.IsEmployeeActive(It.IsAny<Employee>())).Returns(true);
			SentEmailRegister sentEmailRegister = new SentEmailRegister { BirthdayEmplyeeIdList = new List<int> { 100 } };
			_emailRegisterFileAgent.Setup(x => x.GetEmailRegisterDataForToday()).Returns(sentEmailRegister);

			// Act
			var action = CreateAction();
			var result = action.Invoke().Result;

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(0, result.Count);
		}

		private GetEmployeesForBirthdayNotificationAction CreateAction()
		{
			return new GetEmployeesForBirthdayNotificationAction(_employeeApiAgent.Object,
				_emailRegisterFileAgent.Object,
				_employeeDateCalculator.Object,
				ModelConfiguration.CreateMapperConfiguration().CreateMapper());
		}
	}
}