using Newtonsoft.Json;

namespace Acme.MessageSender.Common.Models.Dto
{
	[JsonObject]
	public class EmployeeDto
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("lastName")]
		public string LastName { get; set; }

		[JsonProperty("dateOfBirth")]
		public string DateOfBirth { get; set; }

		[JsonProperty("employmentStartDate")]
		public string EmploymentStartDate { get; set; }

		[JsonProperty("employmentEndDate")]
		public string EmploymentEndDate { get; set; }
	}
}
