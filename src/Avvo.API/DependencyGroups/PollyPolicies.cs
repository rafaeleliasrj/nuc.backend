using System;
using System.Net.Http;
using Polly;

namespace Avvo.API.DependencyGroups;

/// <summary>
///     PollyPolicies Class
/// </summary>
public static class PollyPolicies
{
    /// <summary>
    ///     httpRetryPolicy
    /// </summary>
    /// <returns></returns>
    public static IAsyncPolicy<HttpResponseMessage> HttpRetryPolicy =
        Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .RetryAsync(6);

    /// <summary>
    ///     httpWaitAndRetryPolicy
    /// </summary>
    /// <returns></returns>
    public static IAsyncPolicy<HttpResponseMessage> HttpWaitAndRetryPolicy =
        Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(retryAttempt));

    /// <summary>
    ///     noOpPolicy
    /// </summary>
    /// <returns></returns>
    public static IAsyncPolicy<HttpResponseMessage> NoOpPolicy = Policy.NoOpAsync()
        .AsAsyncPolicy<HttpResponseMessage>();

    /// <summary>
    ///     timeoutPolicy
    /// </summary>
    /// <returns></returns>
    public static IAsyncPolicy<HttpResponseMessage> TimeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(30);
}
