using System;
namespace Acme.MessageSender.Core.Interfaces
{
	public interface IDateTimeProvider
	{
		DateTime CurrentDateTime();
	}
}
