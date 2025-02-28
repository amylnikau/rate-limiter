namespace RateLimiter.Store;

using System.Collections.Generic;
using RateLimiter.Rules;

public interface IRateLimitRuleStore
{
    public void AddRules(string resourceId, params IRateLimitRule[] rules);

    IReadOnlyCollection<IRateLimitRule> GetRules(string resourceId);
}
