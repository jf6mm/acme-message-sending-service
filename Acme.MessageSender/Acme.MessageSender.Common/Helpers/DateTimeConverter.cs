using System;
using System.Globalization;

namespace Acme.MessageSender.Common.Helpers
{
	public static class DateTimeConverter
	{
		public static DateTime ConvertFromIsoDateAsLocal(string isoDateString)
		{
			return DateTime.Parse(isoDateString, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
		}
	}
}
