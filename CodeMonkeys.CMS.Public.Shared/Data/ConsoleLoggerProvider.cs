using Microsoft.Extensions.Logging;

public class ConsoleLoggerProvider : ILoggerProvider
{
    private readonly LogLevel _logLevel;

    public ConsoleLoggerProvider(LogLevel logLevel)
    {
        _logLevel = logLevel;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new ConsoleLogger(categoryName, _logLevel);
    }

    public void Dispose() { }
}
public class ConsoleLogger : ILogger
{
    private readonly string _categoryName;
    private readonly LogLevel _logLevel;

    public ConsoleLogger(string categoryName, LogLevel logLevel)
    {
        _categoryName = categoryName;
        _logLevel = logLevel;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        // Implement scoping logic if needed
        return null;
    }

    public bool IsEnabled(LogLevel logLevel) => logLevel >= _logLevel;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (IsEnabled(logLevel))
        {
            var message = formatter(state, exception);
            Console.WriteLine($"{DateTime.Now} [{logLevel}] {_categoryName}: {message}");
        }
    }
}