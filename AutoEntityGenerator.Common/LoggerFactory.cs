using AutoEntityGenerator.Common.Interfaces;
using System;

namespace AutoEntityGenerator.Common
{
    internal class LoggerFactory
    {
        private ILogger _logger;

        public ILogger CreateLogger()
        {
            if (_logger is null)
            {
                _logger = new Logger();
            }
            return _logger;
        }
    }

    // TODO: Replace internal Logger class implementation with an actual logger.
    internal class Logger : ILogger
    {
        public void Debug(string message)
        {
            // Do nothing for now
        }

        public void Error(Exception ex, string message)
        {
            // Do nothing for now
        }

        public void Information(string message)
        {
            // Do nothing for now
        }
    }
}
