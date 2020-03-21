using Acme.MessageSender.Common.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Acme.MessageSender.Infrastructure.Interfaces
{
	public interface IEmployeeApiAgent
	{
		Task<IList<Employee>> GetAllEmployees();

		Task<IList<int>> GetBirthdayListExclusionIds();
	}
}
