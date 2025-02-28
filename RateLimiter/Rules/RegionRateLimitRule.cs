namespace RateLimiter.Rules;

using System.Threading.Tasks;
using RateLimiter.Models;

public class RegionRateLimitRule : IRateLimitRule
{
    private readonly Region _region;
    private readonly IRateLimitRule _rateLimitRule;

    public RegionRateLimitRule(Region region, IRateLimitRule rateLimitRule)
    {
        _region = region;
        _rateLimitRule = rateLimitRule;
    }

    public Task<bool> IsRequestAllowedAsync(RequestContext request)
    {
        return _region == request.ClientRegion
            ? _rateLimitRule.IsRequestAllowedAsync(request)
            : Task.FromResult(true);
    }
}
