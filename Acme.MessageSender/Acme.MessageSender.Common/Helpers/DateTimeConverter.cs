using System;
using System.Globalization;

namespace Acme.MessageSender.Common.Helpers
{
	public static class DateTimeConverter
	{
        private static DateTime UnixStartDateUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime ConvertFromIsoDateAsLocal(string isoDateString)
		{
			return DateTime.Parse(isoDateString, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
		}

        public static long CurrentDateAsMilliseconds
        {
            get
            {
                return (long)(DateTime.UtcNow.Subtract(UnixStartDateUtc)).TotalMilliseconds;
            }
        }

        public static DateTime FromMillisecondsToDateTimeUtc(long milliseconds)
        {
            return UnixStartDateUtc.AddMilliseconds(milliseconds);
        }

        public static DateTime FromMillisecondsToDateTime(long milliseconds)
        {
            return UnixStartDateUtc.AddMilliseconds(milliseconds).ToLocalTime();
        }

        public static long FromUtcDateTimeToMilliseconds(DateTime datetime)
        {
            return (long)(datetime.Subtract(UnixStartDateUtc)).TotalMilliseconds;
        }

        public static long FromDateTimeToMilliseconds(DateTime datetime)
        {
            return (long)(datetime.ToUniversalTime().Subtract(UnixStartDateUtc)).TotalMilliseconds;
        }

        public static bool IsSameDate(DateTime dateTime1, DateTime dateTime2)
        {
            return dateTime1.Date == dateTime2.Date;
        }
    }
}
