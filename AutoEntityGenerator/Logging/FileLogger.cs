#nullable enable
using System;
using Microsoft.Extensions.Logging;

namespace AutoEntityGenerator.Logging;

internal sealed class FileLogger(
    string categoryName,
    FileLoggerProvider provider) : ILogger
{
    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel != LogLevel.None;
    }

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
            return;

        try
        {
            var message = formatter(state, exception);

            provider.Write(
                categoryName,
                logLevel,
                eventId,
                message,
                exception);
        }
        catch
        {
            // Logging must never throw.
        }
    }
}
