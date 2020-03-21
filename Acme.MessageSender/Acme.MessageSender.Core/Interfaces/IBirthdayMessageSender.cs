using System.Threading.Tasks;

namespace Acme.MessageSender.Core.Interfaces
{
	public interface IBirthdayMessageSender
	{
		Task SendBirthdayMessages();
	}
}
