using Acme.MessageSender.Common.Models;
using Acme.MessageSender.Core.Services.EmployeeNotification;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Acme.MessageSender.Test.Core.Services.EmployeeNotification
{
	[TestClass]
	public class EmployeeNotifierBaseTest
	{
		#region Test Methods

		[TestMethod]
		public void EmployeeNotifierBase_NotifyEmployees_NoOneToNotify()
		{
			// Arrange
			var employeeNotifierImpl = new EmployeeNotifierImplementation();
			employeeNotifierImpl.GetEmployeesToNotifyResult = new List<Employee>();

			// Act
			employeeNotifierImpl.NotifyEmployees().Wait();

			// Assert
			Assert.IsTrue(employeeNotifierImpl.GetEmployeesToNotifyWasCalled);
			Assert.IsFalse(employeeNotifierImpl.SendNotificationToEmployeeWasCalled);
			Assert.IsTrue(employeeNotifierImpl.FlagNotificationAsSentWasCalled);
			Assert.AreEqual(0, employeeNotifierImpl.EmployeeIdsFlaggedAsSent.Count);
		}

		[TestMethod]
		public void EmployeeNotifierBase_NotifyEmployees_TwoEmployees()
		{
			// Arrange
			var employeeNotifierImpl = new EmployeeNotifierImplementation();
			employeeNotifierImpl.GetEmployeesToNotifyResult = new List<Employee>
			{
				new Employee{ Id = 1 },
				new Employee{ Id = 2 }
			};

			// Act
			employeeNotifierImpl.NotifyEmployees().Wait();

			// Assert
			Assert.IsTrue(employeeNotifierImpl.GetEmployeesToNotifyWasCalled);
			Assert.IsTrue(employeeNotifierImpl.SendNotificationToEmployeeWasCalled);
			Assert.AreEqual(2, employeeNotifierImpl.EmployeesSentNotification.Count);
			Assert.IsTrue(employeeNotifierImpl.FlagNotificationAsSentWasCalled);
			Assert.AreEqual(2, employeeNotifierImpl.EmployeeIdsFlaggedAsSent.Count);
		}

		[TestMethod]
		public void EmployeeNotifierBase_NotifyEmployees_OnNotificationErrorDoNotFlag()
		{
			// Arrange
			var employeeNotifierImpl = new EmployeeNotifierImplementation();
			employeeNotifierImpl.SendNotificationThrowExceptionOnFirstCall = true;
			employeeNotifierImpl.GetEmployeesToNotifyResult = new List<Employee>
			{
				new Employee{ Id = 1 },
				new Employee{ Id = 2 }
			};

			// Act
			employeeNotifierImpl.NotifyEmployees().Wait();

			// Assert
			Assert.IsTrue(employeeNotifierImpl.GetEmployeesToNotifyWasCalled);
			Assert.IsTrue(employeeNotifierImpl.SendNotificationToEmployeeWasCalled);
			Assert.AreEqual(2, employeeNotifierImpl.EmployeesSentNotification.Count);
			Assert.IsTrue(employeeNotifierImpl.FlagNotificationAsSentWasCalled);
			Assert.AreEqual(1, employeeNotifierImpl.EmployeeIdsFlaggedAsSent.Count); // should only have flagged the one that was successfull
		}

		#endregion
	}

	/// <summary>
	/// Base class implementation for the prupose of testing
	/// </summary>
	internal class EmployeeNotifierImplementation : EmployeeNotifierBase
	{
		public bool GetEmployeesToNotifyWasCalled { get; private set; }
		public IList<Employee> GetEmployeesToNotifyResult { get; set; }

		public bool FlagNotificationAsSentWasCalled { get; private set; }
		public List<int> EmployeeIdsFlaggedAsSent { get; private set; }

		public bool SendNotificationToEmployeeWasCalled { get; private set; }
		public List<Employee> EmployeesSentNotification { get; private set; }
		public bool SendNotificationThrowExceptionOnFirstCall { get; set; }
		public int SendNotificationTimesCalled { get; private set; }

		public EmployeeNotifierImplementation()
			: base(new Mock<ILogger>().Object)
		{
			EmployeeIdsFlaggedAsSent = new List<int>();
			EmployeesSentNotification = new List<Employee>();
		}

		protected override void FlagNotificationAsSent(List<int> employeeIds)
		{
			FlagNotificationAsSentWasCalled = true;
			EmployeeIdsFlaggedAsSent.AddRange(employeeIds);
		}

		protected override Task<IList<Employee>> GetEmployeesToNotify()
		{
			GetEmployeesToNotifyWasCalled = true;
			return Task.FromResult(GetEmployeesToNotifyResult);
		}

		protected override void SendNotificationToEmployee(Employee employee)
		{
			SendNotificationTimesCalled++;
			SendNotificationToEmployeeWasCalled = true;
			EmployeesSentNotification.Add(employee);

			if (SendNotificationTimesCalled == 1 && SendNotificationThrowExceptionOnFirstCall)
			{
				throw new System.Exception("Error while sending notification");
			}
		}
	}
}
