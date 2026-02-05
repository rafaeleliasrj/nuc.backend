using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System.Text;

namespace NautiHub.Infrastructure.Services.Utilitarios;

public class LoggingHandler : DelegatingHandler
{
    private readonly ILogger<LoggingHandler> _logger;
    private readonly IHostEnvironment _env;

    public LoggingHandler(ILogger<LoggingHandler> logger, IHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (_env.IsProduction()) return await base.SendAsync(request, cancellationToken);

        var curl = new StringBuilder();

        // Método e URL
        curl.Append($"curl --location --request {request.Method} '{request.RequestUri}' \\\n");

        // Headers
        foreach (KeyValuePair<string, IEnumerable<string>> header in request.Headers)
        {
            foreach (var value in header.Value)
            {
                curl.Append($"--header '{EscapeShell(header.Key)}: {EscapeShell(value)}' \\\n");
            }
        }

        if (request.Content?.Headers?.ContentType != null)
        {
            curl.Append($"--header 'Content-Type: {EscapeShell(request.Content.Headers.ContentType.ToString())}' \\\n");
        }

        // Body
        if (request.Content is MultipartFormDataContent multipartContent)
        {
            foreach (HttpContent part in multipartContent)
            {
                var name = part.Headers.ContentDisposition?.Name?.Trim('"') ?? "campo";

                if (part is StringContent stringContent)
                {
                    var value = await stringContent.ReadAsStringAsync(cancellationToken);
                    curl.Append($"--form '{EscapeShell(name)}=\"{EscapeShell(value)}\"' \\\n");
                }
                else if (part is StreamContent)
                {
                    var fileName = part.Headers.ContentDisposition?.FileName?.Trim('"') ?? "arquivo.pfx";
                    curl.Append($"--form '{EscapeShell(name)}=@\"{EscapeShell(fileName)}\"' \\\n");
                }
            }
        }
        else if (request.Content != null)
        {
            var body = await request.Content.ReadAsStringAsync(cancellationToken);
            curl.Append($"--data-raw '{EscapeShell(body)}' \\\n");
        }

        if (curl.ToString().EndsWith(" \\\n"))
            curl.Length -= 3;

        Console.WriteLine("----- HTTP REQUEST AS cURL -----\n{0}", curl.ToString());

        return await base.SendAsync(request, cancellationToken);
    }

    private static string EscapeShell(string input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        return input
            .Replace("'", "'\\''")
            .Replace("\r", "")
            .Replace("\n", "")
            .Replace("%", "%25");
    }
}
