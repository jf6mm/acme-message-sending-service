namespace Acme.MessageSender.Infrastructure.ApiAgents
{
	public class BasicHttpResponse
	{
		public int ResponseStatusCode { get; set; }

		public string ResponseStatusReason { get; set; }

		public string Content { get; set; }
	}
}
