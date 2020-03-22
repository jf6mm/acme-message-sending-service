using Acme.MessageSender.Common.Caching;
using Acme.MessageSender.Common.Models;
using Acme.MessageSender.Common.Models.Dto;
using Acme.MessageSender.Core.Interfaces;
using Acme.MessageSender.Infrastructure.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Acme.MessageSender.Core.Services.Actions
{
	public class GetEmployeesForBirthdayNotificationAction
	{
		private const string EmployeesCacheKey = "EmployeeApi_AllEmployees";
		private const string ExcludedEmployeesCacheKey = "EmployeeApi_ExcludedEmployees";
		private TimeSpan EmployeeApiCacheTime = new TimeSpan(1, 0, 0);

		private readonly IEmployeeApiAgent _employeeApiAgent;
		private readonly IEmailRegisterFileAgent _emailRegisterFileAgent;
		private readonly IEmployeeDateCalculator _employeeDateCalculator;
		private readonly IMapper _mapper;
		private readonly ICacheStore _cacheStore;
		private readonly ILogger _logger;

		public GetEmployeesForBirthdayNotificationAction(IEmployeeApiAgent employeeApiAgent,
			IEmailRegisterFileAgent emailRegisterFileAgent,
			IEmployeeDateCalculator employeeDateCalculator,
			IMapper mapper,
			ICacheStore cacheStore,
			ILogger logger)
		{
			_employeeApiAgent = employeeApiAgent;
			_emailRegisterFileAgent = emailRegisterFileAgent;
			_employeeDateCalculator = employeeDateCalculator;
			_mapper = mapper;
			_cacheStore = cacheStore;
			_logger = logger;
		}

		public async Task<IList<Employee>> Invoke()
		{
			var allEmployees = await GetAllEmployees();
			var excludedEmployeeIds = await GetEmployeeExclusionList();
			SentEmailRegister sentEmailRegisterToday = _emailRegisterFileAgent.GetEmailRegisterDataForToday();

			// Filter list to employees who should be notified today
			var employeesToNotifyToday = allEmployees.Where(e =>
				_employeeDateCalculator.IsBirthdayToday(e.DateOfBirth)
				&& _employeeDateCalculator.IsEmployeeActive(e)
				&& !excludedEmployeeIds.Contains(e.Id)
				&& !sentEmailRegisterToday.BirthdayEmplyeeIdList.Contains(e.Id));

			return employeesToNotifyToday.ToList();
		}

		#region Private Methods

		private async Task<IList<Employee>> GetAllEmployees()
		{
			var employeesFromCache = _cacheStore.Get<IList<Employee>>(EmployeesCacheKey);
			if (employeesFromCache != null)
			{
				_logger.LogDebug("Loaded employee list from cache");
				return employeesFromCache;
			}

			var employeesFromApi = _mapper.Map<IList<EmployeeDto>, IList<Employee>>(await _employeeApiAgent.GetAllEmployees());
			_cacheStore.Add(EmployeesCacheKey, employeesFromApi, EmployeeApiCacheTime);
			_logger.LogDebug("Saved new employee list to cache");
			return employeesFromApi;
		}

		private async Task<IList<int>> GetEmployeeExclusionList()
		{
			var exclusionListFromCache = _cacheStore.Get<IList<int>>(ExcludedEmployeesCacheKey);
			if (exclusionListFromCache != null)
			{
				_logger.LogDebug("Loaded employee exclusion list from cache");
				return exclusionListFromCache;
			}

			var exclusionListFromApi = await _employeeApiAgent.GetBirthdayListExclusionIds();
			_cacheStore.Add(ExcludedEmployeesCacheKey, exclusionListFromApi, EmployeeApiCacheTime);
			_logger.LogDebug("Saved new employee exclusion list to cache");
			return exclusionListFromApi;
		}

		#endregion
	}
}
