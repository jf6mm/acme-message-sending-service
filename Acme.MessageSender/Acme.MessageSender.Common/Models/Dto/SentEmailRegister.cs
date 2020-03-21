using Newtonsoft.Json;
using System.Collections.Generic;

namespace Acme.MessageSender.Common.Models.Dto
{
	[JsonObject]
	public class SentEmailRegister
	{
		[JsonProperty("fileDateEpoch")]
		public long FileDateEpoch { get; set; }

		[JsonProperty("birthdayEmplyeeIdList")]
		public List<int> BirthdayEmplyeeIdList { get; set; }

		[JsonProperty("workAnniversaryEmplyeeIdList")]
		public List<int> WorkAnniversaryEmplyeeIdList { get; set; }

		public SentEmailRegister()
		{
			BirthdayEmplyeeIdList = new List<int>();
			WorkAnniversaryEmplyeeIdList = new List<int>();
		}
	}
}
