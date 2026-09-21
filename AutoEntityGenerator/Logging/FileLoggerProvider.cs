#nullable enable
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;

namespace AutoEntityGenerator.Logging;

internal sealed class FileLoggerProvider : ILoggerProvider
{
    private readonly string _directory;
    private readonly long _maxFileSizeBytes;
    private readonly object _syncRoot = new();

    private StreamWriter? _writer;
    private string? _currentFilePath;
    private int _currentFileIndex;
    private string? _currentMonth;

    public FileLoggerProvider(
        string directory,
        long maxFileSizeBytes = 10 * 1024 * 1024)
    {
        if (string.IsNullOrWhiteSpace(directory))
            throw new ArgumentException("Directory cannot be null or whitespace.", nameof(directory));

        if (maxFileSizeBytes <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxFileSizeBytes));

        _directory = directory;
        _maxFileSizeBytes = maxFileSizeBytes;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new FileLogger(categoryName, this);
    }

    internal void Write(
        string categoryName,
        LogLevel logLevel,
        EventId eventId,
        string message,
        Exception? exception)
    {
        try
        {
            lock (_syncRoot)
            {
                EnsureWriter();

                var line = FormatLine(
                    categoryName,
                    logLevel,
                    eventId,
                    message,
                    exception);

                if (_writer!.BaseStream.Length + GetByteCount(line) + Environment.NewLine.Length
                    > _maxFileSizeBytes)
                {
                    Rotate();
                }

                _writer!.WriteLine(line);
                _writer.Flush();
            }
        }
        catch
        {
            // Logging must never cause the application to fail.
        }
    }

    private void EnsureWriter()
    {
        var month = DateTime.Now.ToString("yyyy-MM");

        if (_writer is not null && _currentMonth == month)
            return;

        _writer?.Dispose();

        Directory.CreateDirectory(_directory);

        _currentMonth = month;
        _currentFileIndex = FindAvailableFileIndex(month);
        _currentFilePath = GetFilePath(month, _currentFileIndex);

        _writer = CreateWriter(_currentFilePath);
    }

    private void Rotate()
    {
        _writer?.Dispose();

        _currentFileIndex++;

        _currentFilePath = GetFilePath(
            _currentMonth!,
            _currentFileIndex);

        _writer = CreateWriter(_currentFilePath);
    }

    private int FindAvailableFileIndex(string month)
    {
        var index = 0;

        while (File.Exists(GetFilePath(month, index)))
        {
            var path = GetFilePath(month, index);

            try
            {
                if (new FileInfo(path).Length < _maxFileSizeBytes)
                    return index;
            }
            catch
            {
                // Try the next file.
            }

            index++;
        }

        return index;
    }

    private string GetFilePath(string month, int index)
    {
        var fileName = index == 0
            ? $"{month}.log"
            : $"{month}.{index}.log";

        return Path.Combine(_directory, fileName);
    }

    private static StreamWriter CreateWriter(string path)
    {
        var stream = new FileStream(
            path,
            FileMode.Append,
            FileAccess.Write,
            FileShare.ReadWrite);

        return new StreamWriter(
            stream,
            new UTF8Encoding(encoderShouldEmitUTF8Identifier: false))
        {
            AutoFlush = false
        };
    }

    private static string FormatLine(
        string categoryName,
        LogLevel logLevel,
        EventId eventId,
        string message,
        Exception? exception)
    {
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        var level = GetLevelText(logLevel);

        var result = $"{timestamp} [{level}] {categoryName}";

        if (eventId.Id != 0)
            result += $" ({eventId.Id})";

        result += $": {message}";

        if (exception is not null)
        {
            result += Environment.NewLine + exception;
        }

        return result;
    }

    private static string GetLevelText(LogLevel level)
    {
        return level switch
        {
            LogLevel.Trace => "TRC",
            LogLevel.Debug => "DBG",
            LogLevel.Information => "INF",
            LogLevel.Warning => "WRN",
            LogLevel.Error => "ERR",
            LogLevel.Critical => "CRT",
            _ => "???"
        };
    }

    private static int GetByteCount(string value)
    {
        return Encoding.UTF8.GetByteCount(value);
    }

    public void Dispose()
    {
        lock (_syncRoot)
        {
            _writer?.Dispose();
            _writer = null;
        }
    }
}
