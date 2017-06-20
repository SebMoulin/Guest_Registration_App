using System;

namespace Company.Welcome.Commons
{
    public interface IProvideDateTime
    {
        DateTime Now();
        DateTime MaxValue();
        DateTime MinValue();
        DateTime Today();
        DateTime UtcNow();
        Int32 ToUnixDateTime(DateTime date);
        DateTime FromUnixDateTime(Int32 unixDateTime);
    }
}