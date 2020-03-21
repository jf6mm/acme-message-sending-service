using Acme.MessageSender.Common.Models.Dto;

namespace Acme.MessageSender.Infrastructure.Interfaces
{
	public interface IEmailRegisterFileAgent
	{
		SentEmailRegister GetEmailRegisterData();

		void SaveEmailRegisterData(SentEmailRegister sentEmailRegister);

		SentEmailRegister GetEmailRegisterDataForToday();
	}
}
