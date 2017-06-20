using System;

namespace Company.Welcome.Commons
{
    public interface ILogger
    {
        void Log(Exception ex);
    }

    public class Logger : ILogger
    {
        public void Log(Exception ex)
        {
            
        }
    }
}