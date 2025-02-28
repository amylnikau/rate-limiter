namespace RateLimiter;

using System.Threading.Tasks;
using RateLimiter.Models;
using RateLimiter.Store;

public class RequestRateLimiter : IRateLimiter
{
    private readonly IRateLimitRuleStore _ruleStore;

    public RequestRateLimiter(IRateLimitRuleStore ruleStore)
    {
        _ruleStore = ruleStore;
    }

    public async Task<bool> IsRequestAllowedAsync(RequestContext request)
    {
        var rules = _ruleStore.GetRules(request.ResourceId);
        foreach (var rule in rules)
        {
            if (!await rule.IsRequestAllowedAsync(request))
            {
                return false;
            }
        }

        return true;
    }
}
