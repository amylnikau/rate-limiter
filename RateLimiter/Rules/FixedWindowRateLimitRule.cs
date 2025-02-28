namespace RateLimiter.Rules;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RateLimiter.Models;

public class FixedWindowRateLimitRule : IRateLimitRule
{

    private class FixedWindow
    {
        private int _count;

        public FixedWindow(DateTime timestamp, int count)
        {
            Timestamp = timestamp;
            _count = count;
        }

        public DateTime Timestamp { get; }
        public int Count => _count;

        public void IncrementCount() => Interlocked.Increment(ref _count);
    };


    private readonly TimeSpan _windowDuration;
    private readonly int _maxRequestsCount;
    private readonly ConcurrentDictionary<RequestKey, FixedWindow> _clientWindows = new();

    public FixedWindowRateLimitRule(TimeSpan windowDuration, int maxRequestsCount)
    {
        _windowDuration = windowDuration;
        _maxRequestsCount = maxRequestsCount;
    }

    public Task<bool> IsRequestAllowedAsync(RequestContext request)
    {
        var requestKey = new RequestKey(request.ResourceId, request.AccessToken);
        var now = DateTime.UtcNow;
        var fixedWindow = _clientWindows.GetValueOrDefault(requestKey);

        if (fixedWindow is null || fixedWindow.Timestamp < now - _windowDuration)
        {
            fixedWindow = new FixedWindow(now, 0);
            _clientWindows[requestKey] = fixedWindow;
        }

        if (fixedWindow.Count >= _maxRequestsCount)
        {
            return Task.FromResult(false);
        }

        fixedWindow.IncrementCount();

        return Task.FromResult(true);
    }
}
