namespace RateLimiter.Rules;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using RateLimiter.Models;

public class TimePassedSinceLastCallRateLimitRule : IRateLimitRule
{
    private readonly TimeSpan _requiredTimeSpanBetweenCalls;
    private readonly ConcurrentDictionary<RequestKey, DateTime> _clientLastCalls = new();

    public TimePassedSinceLastCallRateLimitRule(TimeSpan requiredTimeSpanBetweenCalls)
    {
        _requiredTimeSpanBetweenCalls = requiredTimeSpanBetweenCalls;
    }

    public Task<bool> IsRequestAllowedAsync(RequestContext request)
    {
        var requestKey = new RequestKey(request.ResourceId, request.AccessToken);
        var now = DateTime.UtcNow;
        var lastTimeStamp = _clientLastCalls.GetValueOrDefault(requestKey, DateTime.MinValue);

        if (lastTimeStamp < now - _requiredTimeSpanBetweenCalls)
        {
            _clientLastCalls[requestKey] = now;
            return Task.FromResult(true);
        }
        
        return Task.FromResult(false);
    }
}
