using System;

namespace Company.Welcome.Commons
{
    public class DateTimeProvider : IProvideDateTime
    {
        public DateTime Now()
        {
            return DateTime.Now;
        }

        public DateTime MaxValue()
        {

            return DateTime.MaxValue;
        }

        public DateTime MinValue()
        {
            return DateTime.MinValue;
        }

        public DateTime Today()
        {
            return DateTime.Today;
        }

        public DateTime UtcNow()
        {
            return DateTime.UtcNow;
        }

        public Int32 ToUnixDateTime(DateTime date)
        {
            return (Int32)(date.Subtract(new DateTime(1970, 1, 1)))
                .TotalSeconds;
        }

        public DateTime FromUnixDateTime(Int32 unixDateTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixDateTime);
        }
    }
}