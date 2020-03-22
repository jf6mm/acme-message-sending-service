using Acme.MessageSender.Common.Models.Dto;
using Acme.MessageSender.Common.Models.Settings;
using Acme.MessageSender.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Acme.MessageSender.Infrastructure.ApiAgents
{
	public class EmployeeApiAgent : BaseApiAgent, IEmployeeApiAgent
	{
		private const string EmployeesApiPath = "Employees";
		private const string BirthdayWishExclusionsApiPath = "BirthdayWishExclusions";
		
		public EmployeeApiAgent(IOptions<AppSettings> appSettings, ILogger<EmployeeApiAgent> logger)
			: base(appSettings.Value.EmployeeApiBaseUrl, logger)
		{
		}

		public async Task<IList<EmployeeDto>> GetAllEmployees()
		{
			var response = await GetAsync(EmployeesApiPath);
			var employees = JsonConvert.DeserializeObject<IList<EmployeeDto>>(response.Content);
			return employees;
		}

		public async Task<IList<int>> GetBirthdayListExclusionIds()
		{
			var response = await GetAsync(BirthdayWishExclusionsApiPath);
			var exclusionIds = JsonConvert.DeserializeObject<IList<int>>(response.Content);
			return exclusionIds;
		}
	}
}