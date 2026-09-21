#nullable enable
using Microsoft.Extensions.Logging;

namespace AutoEntityGenerator.Logging;

internal static class FileLoggerExtensions
{
    public static ILoggingBuilder AddFile(
        this ILoggingBuilder builder,
        string directory,
        long maxFileSizeBytes = 10 * 1024 * 1024)
    {
        builder.AddProvider(new FileLoggerProvider(directory, maxFileSizeBytes));
        return builder;
    }
}
