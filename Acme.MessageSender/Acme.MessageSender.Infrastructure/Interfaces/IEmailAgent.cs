using System.Collections.Generic;

namespace Acme.MessageSender.Infrastructure.Interfaces
{
	public interface IEmailAgent
	{
		void SendTextEmail(IList<string> recipients, string subject, string content);
	}
}
