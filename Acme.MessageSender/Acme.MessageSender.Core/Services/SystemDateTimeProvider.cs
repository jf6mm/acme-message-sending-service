using Acme.MessageSender.Core.Interfaces;
using System;
namespace Acme.MessageSender.Core.Services
{
	public class SystemDateTimeProvider : IDateTimeProvider
	{
		public DateTime CurrentDateTime()
		{
			return DateTime.Now;
		}
	}
}
