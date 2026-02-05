using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace NautiHub.Infrastructure.Services.Utils;

public class BenchmarkLogger<T> : IDisposable
{
    private readonly ILogger<T> _logger;
    private readonly IHostEnvironment _env;
    private readonly Stopwatch _stopwatch;
    private readonly List<string> _steps;
    private string _operation = string.Empty;

    public BenchmarkLogger(ILogger<T> logger, IHostEnvironment env)
    {
        _logger = logger;
        _env = env;
        _stopwatch = new Stopwatch();
        _steps = new List<string>();
    }

    public void Start(string operationName = "")
    {
        // if (_env.IsProduction()) return;

        _operation = operationName;
        _steps.Clear();
        _stopwatch.Restart();
    }

    public void Log(string step)
    {
        // if (_env.IsProduction() || !_stopwatch.IsRunning) return;

        var elapsed = _stopwatch.Elapsed;
        string formattedTime;

        if (elapsed.TotalSeconds < 1)
            formattedTime = $"{elapsed.TotalMilliseconds:F0} ms";
        else if (elapsed.TotalMinutes < 1)
            formattedTime = $"{elapsed.TotalSeconds:F2} s";
        else if (elapsed.TotalHours < 1)
            formattedTime = $"{elapsed.TotalMinutes:F2} min";
        else
            formattedTime = $"{elapsed.TotalHours:F2} h";

        var message = $"[Benchmark] {_operation} - {step}: {formattedTime}";
        Console.WriteLine(message);
        _logger.LogInformation(message);
        _steps.Add(message);
        _stopwatch.Restart();
    }

    public void Dispose()
    {
        // if (!_env.IsDevelopment() || _steps.Count == 0) return;

        _stopwatch.Stop();
        var message = $"[Benchmark] {_operation} - Concluído. Etapas registradas:\n{string.Join("\n", _steps)}";
        Console.WriteLine(message);
        _logger.LogInformation(message);
    }
}
