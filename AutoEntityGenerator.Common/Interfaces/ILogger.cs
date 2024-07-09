using System;

namespace AutoEntityGenerator.Common.Interfaces
{
    public interface ILogger
    {
        void Debug(string message);
        void Error(Exception ex, string message);
        void Information(string message);
    }
}
